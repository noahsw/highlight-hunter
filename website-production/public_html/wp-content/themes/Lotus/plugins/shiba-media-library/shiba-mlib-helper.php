<?php
if (!function_exists('is_admin')) {
    header('Status: 403 Forbidden');
    header('HTTP/1.1 403 Forbidden');
    exit();
}

if (!class_exists("Shiba_Media_Library_Helper")) :

class Shiba_Media_Library_Helper {
	
	function init_admin() {
		global $shiba_mlib;

		add_action('admin_head', array(&$this,'mlib_header'));

		// Add Insert into Post button
		add_filter('attachment_fields_to_edit', array(&$this, 'add_insert_into_post_button'), 10, 2);
		
		// Edit Gallery Page
		add_filter('manage_edit-gallery_columns', array(&$this, 'gallery_columns'));
		add_action('manage_posts_custom_column', array(&$this,'manage_gallery_columns'), 10, 2);
		add_filter('wp_redirect', array(&$this,'gallery_redirect'), 10, 2);

		// New Gallery Page
		//add_meta_box(	'gallery-type-div', __('Gallery Type'), array(&$this, 'gallery_type_metabox'), 'gallery', 'normal', 'high');
		add_meta_box(	'post-content-div', __('Gallery Description'), array(&$this, 'gallery_description_metabox'), 'gallery', 'normal', 'high');
//		remove_meta_box('tagsdiv-post_tag','gallery','core');
		/*add_meta_box(	'tagsdiv-post_tag', __('Gallery Tags'), 
						array(&$shiba_mlib->tag_metabox,'post_tags_meta_box'), 'gallery', 'normal', 'high');*/
		add_action('save_post', array(&$this,'save_gallery_data') );

		// Add Media Library functions and additional form for new gallery page
		if (isset($_GET['post']) && (get_post_type($_GET['post']) == 'gallery') && ($_GET['action'] == 'edit') && strpos($_SERVER["REQUEST_URI"], "wp-admin/post.php") !== FALSE) {
			// Add advanced form for new gallery page
			add_action('edit_form_advanced', array(&$this,'edit_gallery__advanced_form'));
			add_filter('post_updated_messages', array(&$this,'gallery_updated_messages'));

			require_once(SHIBA_MLIB_DIR.'/shiba-library-menu.php');
			if (class_exists("Shiba_Library_Menu"))
				$shiba_mlib->library_menu = new Shiba_Library_Menu();	
					 
			add_action('admin_head', array(&$shiba_mlib->library_menu,'library_header'), 51);
			add_action('restrict_manage_posts', array(&$shiba_mlib->library_menu,'library_expanded_menu') );	
			add_filter('wp_redirect', array(&$shiba_mlib->library_menu,'library_redirect'), 10, 2);
		}
		
	}

	function add_pages() {
		global $shiba_mlib;
		
		$url = 'edit.php?post_type=gallery'; //menu_page_url('gallery');
		// Add a new submenu under Gallery:
		//$shiba_mlib->media_tools = add_submenu_page( $url, 'Media Tools', 'Media Tools', 'install_plugins', 'shiba_media_tools', array(&$this,'media_tools_page') );

	}


	function media_tools_page() {
		include('shiba-mlib-controls.php');
	}

	function mlib_header() {
		global $post_type;
		// icons are 16x16
		?>
		<style>
		<?php if (isset($_GET['post_type']) && ($_GET['post_type'] == 'gallery') || ($post_type == 'gallery')) : ?>
		#icon-edit { background:transparent url('<?php echo get_bloginfo('url');?>/wp-admin/images/icons32.png') no-repeat -251px -5px; }
		.column-id { width: 50px; }
		.column-new_icon { width: 70px; text-align: center; }
		.column-images { width: 70px; text-align: center; }
		.column-title { width: 25%; }
		.fixed .column-date { width: 80px; }		
		.fixed .column-parent { width: 20%; }
		.column-gallery_categories, .column-gallery_tags { width: 15%; }
		<?php endif; ?>
		
		#adminmenu #menu-posts-gallery div.wp-menu-image{background:transparent url('<?php echo get_bloginfo('url');?>/wp-admin/images/menu.png') no-repeat scroll -121px -33px;}
		#adminmenu #menu-posts-gallery:hover div.wp-menu-image,#adminmenu #menu-posts-gallery.wp-has-current-submenu div.wp-menu-image{background:transparent url('<?php echo get_bloginfo('url');?>/wp-admin/images/menu.png') no-repeat scroll -121px -1px;}		
        </style>
        <?php
	}
		
	function gallery_columns($posts_columns) {
		$new_columns['cb'] = '<input type="checkbox" />';
		
		$new_columns['id'] = __('ID');
		$new_columns['new_icon'] = '';
		$new_columns['title'] = _x('Gallery Name', 'column name');
		$new_columns['images'] = __('Images');
		$new_columns['author'] = __('Author');
		
		$new_columns['gallery_categories'] = __('Categories');
		$new_columns['gallery_tags'] = __('Tags');
	
		$new_columns['date'] = _x('Date', 'column name');
	
		return $new_columns;
	}
	

	function manage_gallery_columns($column_name, $id) {
		global $shiba_mlib, $wpdb, $post;
		switch ($column_name) {
		case 'id':
			echo $id;
		break;

		case 'new_icon':
			$attachment_id = 0;
			if (function_exists('get_post_thumbnail_id')) 
				$attachment_id = get_post_thumbnail_id($post->ID);
			
			// wp_mime_type_icon throws a notice error in 3.1 RC2 when wp_get_attchment_image is called
			if (!$attachment_id) break;		
			if ( $thumb = wp_get_attachment_image( $attachment_id, array(80, 60), true ) ) {
				if ( $post->post_status == 'trash' ) echo $thumb;
				else {
				echo '	
				<a href="media.php?action=edit&amp;attachment_id='.$attachment_id.'" title="'.esc_attr(sprintf(__('Edit &#8220;%s&#8221;'), $post->post_title)).'">';
				echo $thumb;
				echo "</a>\n";
				}
			}
			break;		
			
		case 'images':
			$gallery_type = get_post_meta($id, '_gallery_type', TRUE);
			if (!$gallery_type) $gallery_type = 'attachment';

			$args = array(
					'post_parent' => $id,
					'post_type' => $gallery_type,
					'posts_per_page' => -1,	
					'post_status' => 'any',
					'suppress_filters' => TRUE
				); 
			$tmp_query = new WP_Query;
			$images = $tmp_query->query($args); 
			
			echo count($images); 
			unset($tmp_query); unset($images);
			break;
		case 'gallery_categories':
			$categories = get_the_category();
			if ( !empty( $categories ) ) {
				$out = array();
				foreach ( $categories as $c )
					$out[] = "<a href='edit.php?post_type=gallery&amp;category_name=$c->slug'> " . esc_html(sanitize_term_field('name', $c->name, $c->term_id, 'category', 'display')) . "</a>";
					echo join( ', ', $out );
			} else {
				_e('Uncategorized');
			}
			break;

		case 'gallery_tags':
			$tags = get_the_tags($post->ID);
			if ( !empty( $tags ) ) {
				$out = array();
				foreach ( $tags as $c )
					$out[] = "<a href='edit.php?post_type=gallery&amp;tag=$c->slug'> " . esc_html(sanitize_term_field('name', $c->name, $c->term_id, 'post_tag', 'display')) . "</a>";
				echo join( ', ', $out );
			} else {
				_e('No Tags');
			}
			break;
		default:
			break;
		} // end switch
	}	

	// Captures delete permanently link from new gallery screen
	function gallery_redirect($location, $status) {
		if (isset($_GET['action']) && isset($_GET['post']) && ($_GET['action'] == 'delete') && 
			strpos($location, 'edit.php?post_type=attachment&deleted=1') !== FALSE) {
			// redirect to http referer
			$referer = wp_get_referer();
			$location = remove_query_arg( array('trashed', 'untrashed', 'deleted', 'ids', 'posted'), $referer );
			$location = add_query_arg('message', 12, $location);
		}
		return $location;
	}
	
	function process_upload_messages($location) {

		if ( strpos($location, 'deleted=1') !== FALSE ) {
			$location = add_query_arg('message', 12, $location);
		}
		
		if ( strpos($location, 'trashed=1') !== FALSE ) {
			$location = add_query_arg('message', 13, $location);
		}

		if ( strpos($location, 'untrashed=1') !== FALSE ) {
			$location = add_query_arg('message', 14, $location);
		}
		$location = remove_query_arg( array('trashed', 'untrashed', 'deleted', 'attached', 'posted'), $location );
		return $location;		
	}

	function gallery_updated_messages( $messages ) {
		global $post, $post_ID;

		$messages['gallery'] = array(
			0 => '', // Unused. Messages start at index 1.
			1 => sprintf( __('Gallery updated. <a href="%s">View gallery</a>'), esc_url( get_permalink($post_ID) ) ),
			2 => __('Custom field updated.'),
			3 => __('Custom field deleted.'),
			4 => __('Gallery updated.'),
			/* translators: %s: date and time of the revision */
			5 => isset($_GET['revision']) ? sprintf( __('Gallery restored to revision from %s'), wp_post_revision_title( (int) $_GET['revision'], false ) ) : false,
			6 => sprintf( __('Gallery published. <a href="%s">View gallery</a>'), esc_url( get_permalink($post_ID) ) ),
			7 => __('Gallery saved.'),
			8 => sprintf( __('Gallery submitted. <a target="_blank" href="%s">Preview gallery</a>'), esc_url( add_query_arg( 'preview', 'true', get_permalink($post_ID) ) ) ),
			9 => sprintf( __('Gallery scheduled for: <strong>%1$s</strong>. <a target="_blank" href="%2$s">Preview gallery</a>'),
			  // translators: Publish box date format, see http://php.net/date
			  date_i18n( __( 'M j, Y @ G:i' ), strtotime( $post->post_date ) ), esc_url( get_permalink($post_ID) ) ),
			10 => sprintf( __('Gallery draft updated. <a target="_blank" href="%s">Preview gallery</a>'), esc_url( add_query_arg( 'preview', 'true', get_permalink($post_ID) ) ) ),
			11 => __('Media removed from gallery.'),
			12 => __('Media permanently deleted.'),
			13 => __('Media moved to the trash.'.' <a href="' . esc_url( wp_nonce_url( 'upload.php?doaction=undo&action=untrash&ids='.(isset($_GET['ids']) ? $_GET['ids'] : ''), "bulk-media" ) ) . '">' . __('Undo') . '</a>'),
			14 => __('Media restored from the trash.')
		  );

		return $messages;
	}
	
	function edit_gallery__advanced_form() {
		global $is_trash;
		
		if (!isset($_GET['post'])) return;
		$id = absint($_GET['post']);
//		$gallery = get_post($id);
//		$action = 'updategallery';
		$post_type = get_post_meta($id, '_gallery_type', TRUE);
		if (!$post_type) $post_type = 'attachment';
		// Can't paginate if we are adding in tagged attachment results    
		$args = array(
			'post_type' => $post_type,
			'posts_per_page' => -1,
			'post_status' => 'any',
			'post_parent' => $id
			); 		
		query_posts($args);
						
//		if (!isset($_GET['post_type']) || ($_GET['post_type'] != 'gallery')) return;

		// list images in gallery
		$image_title = __('Add an Image');    
		// add upload.php in referer address
		$_SERVER['REQUEST_URI'] = add_query_arg( array('redirect_back' => 'upload.php'), $_SERVER['REQUEST_URI']);
		?>
        </div> <!-- This ends post-body-content -->
       	</div> <!-- This ends post-body -->
      	</div> <!-- This ends post-stuff -->
      	</form> <!-- This ends post form -->
		<div style="clear:both;"></div>
		
		<h3 style="float:left;padding-right:20px;">Gallery Images</h3>
		
		<div style="position:relative; top:10px;">    
		<form id="gallery-add-image" action="media-new.php" method="get"> 
			<input type="hidden" name="post_id" id="post_id" value="<?php echo $id;?>" />        
			<input type="submit" value="<?php esc_attr_e('Add New'); ?>" name="addimage-to-gallery" class="button-secondary action"/>
		</form>
		</div>
	 
		<form id="gallery-list" action="upload.php" method="<?php if (class_exists('WP_Media_List_Table')) echo 'post'; else echo 'get';?>">
			<?php wp_nonce_field('bulk-media'); ?>			
			<div class="tablenav">
		   
			 <div class="alignleft actions">
			<select name="action" id="mlib_action" class="select-action">
			<option value="-1" selected="selected"><?php _e('Bulk Actions'); ?></option>
			<option value="remove"><?php _e('Detach from Gallery'); ?></option>
			<?php if (function_exists('wp_trash_post') && !$is_trash) { ?>
			<option value="trash"><?php _e('Move to Trash'); ?></option>
			<?php } ?>       
<!--		Removed "Delete Permanently" option because it ONLY works on attachments and causes errors for others	
			<option value="delete"><?php _e('Delete Permanently'); ?></option>  -->           
			</select>
			<input type="submit" value="<?php esc_attr_e('Apply'); ?>" name="doaction" id="mlib_doaction" class="button-secondary action" onClick="processGalleryForm('gallery-list');"/>
			</div> <!-- End alignleft actions -->
			
			<div style="clear:both;"></div>
			</div> <!-- End tablenav -->
			
			<style>
				#the-list td.column-parent a.hide-if-no-js { display:none; }
			</style>
	
			<?php
			global $post;
			if (file_exists('edit-attachment-rows.php')) // for below 3.1
				include( 'edit-attachment-rows.php' );
			else if (function_exists('_get_list_table')) {// for WordPress 3.1 and above	
				$wp_list_table = _get_list_table('WP_Media_List_Table');
				$this->display_media_table($wp_list_table);			
			}	
			?>       	
			<div style="clear:both;"></div>
		</form> <!-- End gallery-list form -->
        <form> <!-- Need to open up div to match the close post form -->
        <div> <!-- Need to open up div to match the close div of post-stuff -->
        <div> <!-- Need to open up div to match the close div of post-body -->       
        <div> <!-- Need to open up div to match the close div of post-body-content -->

		<?php
	}

	function display_media_table($table) { // for wordpress 3.1 and above. Replaces edit-attachment-rows.php
		// filling in arguments needed from get_column_info
		global $shiba_media_table, $wp_query;
		$shiba_media_table = $table; $table->is_trash = FALSE;
		$table->_column_headers = array($table->get_columns(), array(), $table->get_sortable_columns());
		extract( $table->_args );
//		$this->display_tablenav( 'top' );

		?>
		<table class="<?php echo implode( ' ', $table->get_table_classes() ); ?>" cellspacing="0">
			<thead>
			<tr><?php $table->print_column_headers(); ?></tr>
			</thead>

			<tfoot>
			<tr><?php $table->print_column_headers( false ); ?></tr>
			</tfoot>

			<tbody id="the-list"<?php if ( $singular ) echo " class='list:$singular'"; ?>>
				<?php $table->display_rows_or_placeholder(); ?>
			</tbody>
		</table>
		<?php 
//		$this->display_tablenav( 'bottom' );
 }

	function gallery_description_metabox($post) {
		?>
		<label class="screen-reader-text" for="excerpt"><?php _e('Description') ?></label>
        <textarea rows="5" cols="40" name="content" tabindex="6" id="content"><?php echo $post->post_content; ?></textarea>
		<p><?php _e('The description is not prominent by default, however some plugins may show it.'); ?></p>
		<?php
	}
	
	function gallery_type_metabox($post) {
		$gallery_type = get_post_meta($post->ID, '_gallery_type', TRUE);
		if (!$gallery_type) $gallery_type = 'attachment'; 	 
		?>
        <style>
		#gallery-type-div { margin-top: 80px; }
		</style>
        <input type="hidden" name="gallery_type_noncename" id="gallery_type_noncename" value="<?php echo wp_create_nonce( 'gallery_type'.$post->ID );?>" />
		<input type="radio" name="gallery_type" value="any" <?php if ($gallery_type == 'any') echo "checked=1";?>> Any.<br/>
		<input type="radio" name="gallery_type" value="attachment" <?php if ($gallery_type == 'attachment') echo "checked=1";?>> Only Attachments.<br/>
		<input type="radio" name="gallery_type" value="post" <?php if ($gallery_type == 'post') echo "checked=1";?>> Only Posts.<br/>
		<input type="radio" name="gallery_type" value="gallery" <?php if ($gallery_type == 'gallery') echo "checked=1";?>> Only Galleries.<br/>
		<?php
	}
	
	function save_gallery_data($post_id) {	
		// verify this came from the our screen and with proper authorization.
		if ( !isset($_POST['gallery_type_noncename']) || !wp_verify_nonce( $_POST['gallery_type_noncename'], 'gallery_type'.$post_id )) {
			return $post_id;
		}
	
		// verify if this is an auto save routine. If it is our form has not been submitted, so we dont want
		// to do anything
		if ( defined('DOING_AUTOSAVE') && DOING_AUTOSAVE ) 
			return $post_id;
	
		// Check permissions
		if ( !current_user_can( 'edit_post', $post_id ) )
			return $post_id;
		
	
		// OK, we're authenticated: we need to find and save the data	
		$post = get_post($post_id);
		if ($post->post_type == 'gallery') { 
			update_post_meta($post_id, '_gallery_type', esc_attr($_POST['gallery_type']) );
		}
		return $post_id;
	}	
	
	// from media_upload_library_form
	function media_library_date_filter() {
		global $wpdb, $wp_locale;
		$arc_query = "SELECT DISTINCT YEAR(post_date) AS yyear, MONTH(post_date) AS mmonth FROM $wpdb->posts WHERE post_type = 'attachment' ORDER BY post_date DESC";
		$arc_result = $wpdb->get_results( $arc_query );
		$month_count = count($arc_result);

		if ( $month_count && !( 1 == $month_count && 0 == $arc_result[0]->mmonth ) ) : ?>
		<select name='m' style="width:170px;height:25px;">
		<option value='0'><?php _e('Show all dates'); ?></option>
		<?php
		foreach ($arc_result as $arc_row) {
			if ( $arc_row->yyear == 0 )
				continue;
			$arc_row->mmonth = zeroise( $arc_row->mmonth, 2 );
		
			if ( isset($_GET['m']) && ( $arc_row->yyear . $arc_row->mmonth == $_GET['m'] ) )
				$default = ' selected="selected"';
			else
				$default = '';
		
			echo "<option$default value='" . esc_attr("$arc_row->yyear$arc_row->mmonth") . "'>";
			echo $wp_locale->get_month($arc_row->mmonth) . " $arc_row->yyear";
			echo "</option>\n";
		}
		?>
		</select>
		<?php endif; // month_count ?>
		<input type="submit" id="mlib-post-query-submit" value="<?php esc_attr_e('Filter'); ?>" class="button-secondary" style="margin-left:5px"/>	
		<?php
	}
	
	
	function add_insert_into_post_button($form_fields, $post) {
		$attachment_id = $post->ID; $filename = basename( $post->guid );
		$calling_post_id = 0;
		if ( isset( $_GET['post_id'] ) )
			$calling_post_id = absint( $_GET['post_id'] );
		elseif ( isset( $_POST ) && count( $_POST ) ) // Like for async-upload where $_GET['post_id'] isn't set
			$calling_post_id = $post->post_parent;

		$post_type = get_post_type($calling_post_id);
		$insert_media_button_array = apply_filters('shiba_insert_media_button', array('post','page'));
		if ($calling_post_id && in_array($post_type, $insert_media_button_array))
			$send = "<input type='submit' class='button' name='send[$attachment_id]' value='" . esc_attr__( 'Insert into Post' ) . "' />";
		else $send = '';	
		if ( current_user_can( 'delete_post', $attachment_id ) ) {
			if ( !EMPTY_TRASH_DAYS ) {
				$delete = "<a href='" . wp_nonce_url( "post.php?action=delete&amp;post=$attachment_id", 'delete-attachment_' . $attachment_id ) . "' id='del[$attachment_id]' class='delete'>" . __( 'Delete Permanently' ) . '</a>';
			} elseif ( !MEDIA_TRASH ) {
				$delete = "<a href='#' class='del-link' onclick=\"document.getElementById('del_attachment_$attachment_id').style.display='block';return false;\">" . __( 'Delete' ) . "</a>
				 <div id='del_attachment_$attachment_id' class='del-attachment' style='display:none;'>" . sprintf( __( 'You are about to delete <strong>%s</strong>.' ), $filename ) . "
				 <a href='" . wp_nonce_url( "post.php?action=delete&amp;post=$attachment_id", 'delete-attachment_' . $attachment_id ) . "' id='del[$attachment_id]' class='button'>" . __( 'Continue' ) . "</a>
				 <a href='#' class='button' onclick=\"this.parentNode.style.display='none';return false;\">" . __( 'Cancel' ) . "</a>
				 </div>";
			} else {
				$delete = "<a href='" . wp_nonce_url( "post.php?action=trash&amp;post=$attachment_id", 'trash-attachment_' . $attachment_id ) . "' id='del[$attachment_id]' class='delete'>" . __( 'Move to Trash' ) . "</a>
				<a href='" . wp_nonce_url( "post.php?action=untrash&amp;post=$attachment_id", 'untrash-attachment_' . $attachment_id ) . "' id='undo[$attachment_id]' class='undo hidden'>" . __( 'Undo' ) . "</a>";
			}
		} else {
			$delete = '';
		}

		$thumbnail = '';
		if ( (strpos($post->post_mime_type, 'image') !== FALSE) && $calling_post_id &&  current_theme_supports( 'post-thumbnails') &&  get_post_thumbnail_id( $calling_post_id ) != $attachment_id ) {
			$ajax_nonce = wp_create_nonce( "set_post_thumbnail-$calling_post_id" );
			$thumbnail = "<a class='wp-post-thumbnail' id='wp-post-thumbnail-" . $attachment_id . "' href='#' onclick='WPSetAsThumbnail(\"$attachment_id\", \"$ajax_nonce\");return false;'>" . esc_html__( "Use as featured image" ) . "</a>";
		}
		
		$form_fields['buttons'] = array('tr' => "\t\t<tr class='submit'><td></td><td class='savesend'>$send $thumbnail $delete</td></tr>\n");
		return $form_fields;		
	}
	
} // end Shiba_Media_Library_Helper class
endif;


?>