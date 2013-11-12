<?php
// don't load directly
if (!function_exists('is_admin')) {
    header('Status: 403 Forbidden');
    header('HTTP/1.1 403 Forbidden');
    exit();
}

/*
 * Media Plus menu for the Media >> Library menu.
 *
 * Extends the WordPress media library to enable bulk detachments, attachments, and tagging.
 *
 */

if (!class_exists("Shiba_Library_Menu")) :
	 
class Shiba_Library_Menu {
	
	// Javascript for processing the added Media Library bulk-action form in the Media>> Library screen
	function library_header() {
	?>
	<style>
	.column-new_icon { width: 70px; text-align: center; }
	.column-title, .column-new_title { width: 25%; }
	.fixed .column-date { width: 80px; }
	.upload-php .fixed .column-parent { width: 20%; }
	</style>
	
	<script type="text/javascript">
	<!--
	// get_selected_media(document.posts-filter.list)
	
	function addNewArg(name, value) {
		var newArg = document.createElement("input");
		newArg.type = "hidden";
		newArg.name = name;
		newArg.value = value;
//		newArg.id = name;
		return newArg;
	}
	
	function addMediaActions(mediaPlusForm) {
		// If action is remove then unset doaction so that it does not get caught in upload.php
		var action = document.getElementById('mlib_action').value;
		if ((action == 'remove') || (action == 'set_tags') || (action == 'add_tags')) {
			var ref = document.getElementsByName('_wp_http_referer');
			for (var i = 0; i < ref.length; i++) {			
				if (ref[i].form.id == mediaPlusForm.id) {
					mediaPlusForm.removeChild(ref[i]);	
				}
			}	
			document.getElementById('mlib_doaction').name="shiba_doaction";		
			// For 3.1 need to rename action and action2
			document.getElementById('mlib_action').name = "shiba_action";
		}
	}	
	
	function redirectPage(mediaPlusForm) {
		// Redirect to previous page by adding previous mime_type and detached state
		var hasType = location.href.indexOf('post_mime_type');
		if (hasType >= 0) {
			var sPos = location.href.indexOf('=', mimeType);
			var ePos = location.href.indexOf('&', sPos);
			
			if (ePos >= 0) {
				var mimeStr = location.href.substring(sPos+1, ePos);
			} else {
				var mimeStr = location.href.substring(sPos+1);
			}
	
			mediaPlusForm.appendChild(addNewArg('post_mime_type', mimeStr));
		}
		
		if (location.href.indexOf('detached') >= 0) {
				mediaPlusForm.appendChild(addNewArg('detached', '1'));
		}
	}

	function getSelectedMedia(mediaPlusForm, wordpressForm) {
		var formElements = document.getElementById(wordpressForm).elements;
		for (i = 0; i < formElements.length; i++) {
			if (formElements[i].name == "media[]" && formElements[i].checked) {
				mediaPlusForm.appendChild(addNewArg("media[]", formElements[i].value));
			}
		}		
	}

	function processMediaPlusForm(wordpressForm) {
		var mediaPlusForm=document.getElementById('shiba-mlib-form');
		getSelectedMedia(mediaPlusForm, wordpressForm);
		addMediaActions(mediaPlusForm);
		redirectPage(mediaPlusForm);
		}
	
	function processGalleryForm(galleryForm) {
		var mediaPlusForm=document.getElementById(galleryForm);
		addMediaActions(mediaPlusForm);
	}	

	function processEditGalleryForm(galleryForm) {
		var galleryEditForm=document.getElementById(galleryForm);
		var postName = document.getElementById('editable-post-name').innerHTML;
		galleryEditForm.appendChild(addNewArg("post_name", postName));
	}	

	//-->
	</script>
	<?php
	}
	
	// Change redirect set in upload.php
	function library_redirect($location, $status) {
		
		global $shiba_mlib;		
		if (strpos($location, 'redirect_back=upload.php') !== FALSE )
			$location = $shiba_mlib->helper->process_upload_messages($location);

		if ( isset($_REQUEST['found_post_id']) && isset($_REQUEST['media']) ) {
			if (!isset($_REQUEST['detached']))
				$location = remove_query_arg('detached', $location);
		}
		return $location;
	}
	
	
	function library_action() {
		global $wpdb, $shiba_mlib;
		
		if ( !isset($_REQUEST['shiba_doaction']) || !isset($_REQUEST['shiba_action']) || !$_REQUEST['media'] )return;
		check_admin_referer('bulk-media');
		
		$location = 'upload.php';
		if ( $referer = wp_get_referer() ) {
			if ( false !== strpos($referer, 'upload.php') || false !== strpos($referer, 'post.php') )
				$location = $referer;
		}
		
		switch ($_REQUEST['shiba_action']) :
		case 'remove':
			$attach = array();
			foreach( (array) $_REQUEST['media'] as $att_id ) {
				$att_id = (int) $att_id;
		
				if ( !current_user_can('edit_post', $att_id) )
					continue;
		
				$attach[] = $att_id;
			}
		
			if ( ! empty($attach) ) {
				$attach = implode(',', $attach);
				$attached = $wpdb->query( $wpdb->prepare("UPDATE $wpdb->posts SET post_parent = %d WHERE post_type = 'attachment' AND ID IN ($attach)", '') );
			}
		
			if ( isset($attached) ) {
				if (strpos($location, 'post.php?post') !== FALSE) {	
					$location = remove_query_arg( array('trashed', 'untrashed', 'deleted', 'ids', 'posted', 'attached'), $location );
					$location = add_query_arg( array( 'message' => 11 ) , $location );
				} else $location = add_query_arg( array( 'attached' => $attached ) , $location );
				$shiba_mlib->javascript_redirect($location);
			}
			break;
		case 'set_tags':
			if (!isset($_REQUEST['tax_input']['post_tag'])) return;
			$tag_arr = explode(',',esc_attr($_REQUEST['tax_input']['post_tag']));
			
			foreach( (array) $_REQUEST['media'] as $att_id ) {
				$att_id = absint($att_id);
				if ( !current_user_can('edit_post', $att_id) )
					continue;
		
				// Replace tags for attachment
				wp_set_object_terms( $att_id, $tag_arr, 'post_tag' );
			}
	
			$location = add_query_arg('posted', 1, $location);
			$shiba_mlib->javascript_redirect($location);
			break;
		case 'add_tags':
			if (!isset($_REQUEST['tax_input']['post_tag'])) return;
			$tag_arr = explode(',',esc_attr($_REQUEST['tax_input']['post_tag']));
			
			foreach( (array) $_REQUEST['media'] as $att_id ) {
				$att_id = absint($att_id);
				if ( !current_user_can('edit_post', $att_id) )
					continue;
		
				// Add tags for attachment
				wp_set_object_terms( $att_id, $tag_arr, 'post_tag', TRUE );
			}
	
			$location = add_query_arg('posted', 1, $location);
			$shiba_mlib->javascript_redirect($location);
			break;
		default:
			break;	
		endswitch;
	}
	
	function library_expanded_menu() {
		global $wpdb, $shiba_mlib, $current_screen;
	
		// Only add expanded menu to upload screen
		if (!isset($current_screen) || !isset($current_screen->id) || ($current_screen->id != 'upload')) return;
		$this->library_action();
	
		require_once('includes/meta-boxes.php');

		global $message;
		if ($message) $search_top = 150; else $search_top = 100;				
		add_meta_box('tagsdiv-post_tag', __('Attachment Tags'), array(&$shiba_mlib->tag_metabox,'post_tags_meta_box'), 'media_library_page', 'normal', 'core'); 
		
		?>
		<style>
        #shiba-search-box { float:right; }
		#shiba-filter-box { float:right; }
        </style>	
        <div class="wrap">   
        <?php screen_icon(); ?>
        <h2>Shiba Media Library</h2> 

        <div id="shiba-search-box">
        <form class="shiba-search-form" action="" method="get">            
            <label class="screen-reader-text" for="media-search-input"><?php _e( 'Search Media' ); ?>:</label>
            <input type="text" id="media-search-input" name="s" value="<?php the_search_query(); ?>" />
            <input type="submit" value="<?php esc_attr_e( 'Search Media' ); ?>" class="button" />
            
            <!-- Add drop down box to allow search for other gallery attributes -->
            <select name='search_attribute' id='search_attribute'>
                <option class='search-option' value='title'>Title</option>
                <option class='search-option' value='tag' <?php if (isset($_REQUEST['search_attribute']) && ($_REQUEST['search_attribute'] == 'tag')) echo "selected";?>>Tag</option>
            </select>    
        </form>	
        </div>

        <form id="shiba-filter-box" action="" method="get">
            <?php $shiba_mlib->helper->media_library_date_filter(); ?>
        </form>    

        <div id="shiba-mlib-box">
        <form id="shiba-mlib-form" action="" method="<?php if (class_exists('WP_Media_List_Table')) echo 'post'; else echo 'get';?>">
            <?php wp_nonce_field('bulk-media'); ?>
        
            <select name="action" id="mlib_action">
            <option value="-1" selected="selected"><?php _e('Bulk Actions'); ?></option>
            <option value="delete"><?php _e('Delete Permanently'); ?></option>
            <option value="attach"><?php _e('Attach to a Post'); ?></option>
            <option value="remove"><?php _e('Detach from a Post'); ?></option>

            <option value="set_tags"><?php _e('Set/Replace Tags'); ?></option>
            <option value="add_tags"><?php _e('Add Tags'); ?></option>
            </select>
            <!-- Click container mlib_doaction defined in shiba-mlib.dev.js -->
            <input type="submit" value="<?php esc_attr_e('Apply'); ?>" name="doaction" id="mlib_doaction" class="button-secondary action" onClick="processMediaPlusForm('posts-filter');" />


            <div id="poststuff" class="metabox-holder" style="width:400px;" >	
            <?php $tag_meta_box = do_meta_boxes('media_library_page', 'normal', NULL); ?>
            </div>				
 
        </form>
        
       </div>

        </div> <!-- End div wrap -->
       
    	<div style="clear:both;"></div>
	<?php	
	}
     
} // end class	
endif;
?>