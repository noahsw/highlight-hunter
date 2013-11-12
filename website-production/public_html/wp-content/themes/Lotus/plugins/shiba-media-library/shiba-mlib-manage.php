<?php
if (!function_exists('is_admin')) {
    header('Status: 403 Forbidden');
    header('HTTP/1.1 403 Forbidden');
    exit();
}

global $shiba_mlib, $shiba_mlib_menu;


/**
 *
 * Based on files from -
 * wp-admin/edit-post-rows.php, 
 * wp-admin/includes/template.php - print column headers, post_rows
 * wp-admin/edit-attachment-rows.php
 *
 */
global $wpdb, $wp_locale;

// create shiba library menu class
require_once('shiba-manage-class.php');
if (class_exists("Shiba_Media_Library_Menu") && !$shiba_mlib_menu) {
    $shiba_mlib_menu = new Shiba_Media_Library_Menu();	
}	

if ( isset($_GET['doaction']) || isset($_GET['doaction2']) || isset($_GET['delete_all']) || isset($_GET['delete_all2']) ) {
	check_admin_referer('bulk-media');

	if ( isset($_GET['delete_all']) || isset($_GET['delete_all2']) ) {
		$post_ids = $wpdb->get_col( "SELECT ID FROM $wpdb->posts WHERE post_type='gallery' AND post_status = 'trash'" );
		$doaction = 'delete';
	} elseif ( ( $_GET['action'] != -1 || $_GET['action2'] != -1 ) && ( isset($_GET['gallery']) || isset($_GET['ids']) ) ) {
		$post_ids = isset($_GET['gallery']) ? $_GET['gallery'] : explode(',', $_GET['ids']);
		$doaction = ($_GET['action'] != -1) ? $_GET['action'] : $_GET['action2'];
	} else {
		$shiba_mlib->javascript_redirect($_SERVER['HTTP_REFERER']);
	}

	$location = 'upload.php';
	if ( $referer = wp_get_referer() ) {
		if ( false !== strpos($referer, 'upload.php') )
			$location = remove_query_arg( array('trashed', 'untrashed', 'deleted', 'message', 'ids', 'posted'), $referer );
	}

	if (empty($post_ids)) {
		$shiba_mlib->javascript_redirect($location);
		exit;
	}
	
	switch ( $doaction ) {
		case 'trash':
			foreach( (array) $post_ids as $post_id ) {
				if ( !current_user_can('delete_post', $post_id) )
					wp_die( __('You are not allowed to move this post to the trash.') );

				if ( !wp_trash_post($post_id) )
					wp_die( __('Error in moving to trash...') );
			}
			$location = add_query_arg( array( 'message' => 4, 'ids' => join(',', $post_ids) ), $location );
			break;
		case 'untrash':
			foreach( (array) $post_ids as $post_id ) {
				if ( !current_user_can('delete_post', $post_id) )
					wp_die( __('You are not allowed to move this post out of the trash.') );

				if ( !wp_untrash_post($post_id) )
					wp_die( __('Error in restoring from trash...') );
			}
			$location = add_query_arg('message', 5, $location);
			break;
		case 'delete':
			foreach( (array) $post_ids as $post_id_del ) {
				if ( !current_user_can('delete_post', $post_id_del) )
					wp_die( __('You are not allowed to delete this post.') );
					
				if ( !wp_delete_post($post_id_del, TRUE) )
						wp_die( __('Error in deleting...') );
	
			}
			$location = add_query_arg('message', 2, $location);
			break;
	}

	$shiba_mlib->javascript_redirect($location);
	exit;
} elseif ( isset($_GET['_wp_http_referer']) && ! empty($_GET['_wp_http_referer']) ) {
	 $shiba_mlib->javascript_redirect( remove_query_arg( array('_wp_http_referer', '_wpnonce'), stripslashes($_SERVER['REQUEST_URI']) ) );
	 exit;
}


// Get galleries
wp_edit_attachments_query();
$is_trash = ( isset($_GET['status']) && $_GET['status'] == 'trash' );

add_filter('manage_shiba_mlib_columns', array(&$shiba_mlib_menu,'gallery_columns') );

	?>
<div class="wrap">   
	<?php screen_icon(); ?>
    <h2>Manage Gallery

	<?php if ( isset($_GET['s']) && $_GET['s'] )
	printf( '<span class="subtitle">' . __('Search results for &#8220;%s&#8221;') . '</span>', esc_html( get_search_query() ) ); ?>

    </h2>
    
<?php
$message = '';
if ( isset($_GET['posted']) && (int) $_GET['posted'] ) {
	$_GET['message'] = '1';
	$_SERVER['REQUEST_URI'] = remove_query_arg(array('posted'), $_SERVER['REQUEST_URI']);
}

if ( isset($_GET['deleted']) && (int) $_GET['deleted'] ) {
	$_GET['message'] = '2';
	$_SERVER['REQUEST_URI'] = remove_query_arg(array('deleted'), $_SERVER['REQUEST_URI']);
}

if ( isset($_GET['trashed']) && (int) $_GET['trashed'] ) {
	$_GET['message'] = '4';
	$_SERVER['REQUEST_URI'] = remove_query_arg(array('trashed'), $_SERVER['REQUEST_URI']);
}

if ( isset($_GET['untrashed']) && (int) $_GET['untrashed'] ) {
	$_GET['message'] = '5';
	$_SERVER['REQUEST_URI'] = remove_query_arg(array('untrashed'), $_SERVER['REQUEST_URI']);
}
$messages[1] = __('Gallery added.');
$messages[2] = __('Gallery permanently deleted.');
$messages[3] = __('Error saving gallery.');
$messages[4] = __('Gallery moved to the trash.') . ' <a href="' . esc_url( wp_nonce_url( 'upload.php?doaction=undo&action=untrash&ids='.(isset($_GET['ids']) ? $_GET['ids'] : ''), "bulk-media" ) ) . '">' . __('Undo') . '</a>';
$messages[5] = __('Gallery restored from the trash.');

if ( isset($_GET['message']) && (int) $_GET['message'] ) {
	$message = $messages[$_GET['message']];
	$_SERVER['REQUEST_URI'] = remove_query_arg(array('message'), $_SERVER['REQUEST_URI']);
}

if ( !empty($message) ) { ?>
<div id="message" class="updated fade"><p><?php echo $message; ?></p></div>
<?php } ?>

<form class="search-form" action="" method="get">
    <input type="hidden" name="page" value="shiba_manage_gallery"/>

<p class="search-box">
	<label class="screen-reader-text" for="media-search-input"><?php _e( 'Search Gallery' ); ?>:</label>
	<input type="text" id="media-search-input" name="s" value="<?php the_search_query(); ?>" />
	<input type="submit" value="<?php esc_attr_e( 'Search Gallery' ); ?>" class="button" />
    
    <!-- Add drop down box to allow search for other gallery attributes -->
    <select name='search_attribute' id='search_attribute'>
		<option class='search-option' value='title'>Title</option>
		<option class='search-option' value='tag' <?php if (isset($_GET['search_attribute']) && ($_GET['search_attribute'] == 'tag')) echo "selected";?>>Tag</option>
		<option class='search-option' value='category' <?php if (isset($_GET['search_attribute']) && ($_GET['search_attribute'] == 'category')) echo "selected";?>>Category</option>
    </select>    
</p>
</form>


<form id="posts-filter" action="" method="get">
    <input type="hidden" name="page" value="shiba_manage_gallery"/>

    <ul class="subsubsub">
    <?php
    $gallery_links = array();
    
    $num_posts = wp_count_posts( 'gallery', 'readable' );
    $total_posts = array_sum( (array) $num_posts ) - $num_posts->trash;
    
    $gallery_links[] = "<li><a href='upload.php?page=shiba_manage_gallery'>" . sprintf( _nx( 'All <span class="count">(%s)</span>', 'All <span class="count">(%s)</span>', $total_posts, 'posts' ), number_format_i18n( $total_posts ) ) . '</a>';
    if (function_exists('wp_trash_post'))
    $gallery_links[] = "<li><a href='upload.php?page=shiba_manage_gallery&amp;status=trash'>" . sprintf( _nx( 'Trash <span class="count">(%s)</span>', 'Trash <span class="count">(%s)</span>', $num_posts->trash, 'posts' ), number_format_i18n( $num_posts->trash ) ) . '</a>';
    
    echo implode( " |</li>\n", $gallery_links ) . '</li>';
    unset( $gallery_links );
    
    ?>
    </ul>

    <div class="tablenav">

    
    <div class="alignleft actions">
    	<!-- Bulk action menu -->
        <select name="action" class="select-action">
        <option value="-1" selected="selected"><?php _e('Bulk Actions'); ?></option>
        <?php if ( $is_trash ) { ?>
        <option value="untrash"><?php _e('Restore'); ?></option>
        <?php }         
        if (function_exists('wp_trash_post') && !$is_trash) {
        ?>
        <option value="trash"><?php _e('Move to Trash'); ?></option>
        <?php } ?>
        <option value="delete"><?php _e('Delete Permanently'); ?></option>       
        </select>
        <input type="submit" value="<?php esc_attr_e('Apply'); ?>" name="doaction" id="doaction" class="button-secondary action" />
        <?php wp_nonce_field('bulk-media'); ?>
        
        <?php
		// Date Filter
		if ( !is_singular() && !$is_trash ) {
		$arc_query = "SELECT DISTINCT YEAR(post_date) AS yyear, MONTH(post_date) AS mmonth FROM $wpdb->posts WHERE post_type = 'gallery' ORDER BY post_date DESC";
	
		$arc_result = $wpdb->get_results( $arc_query );
	
		$month_count = count($arc_result);
	
		if ( $month_count && !( 1 == $month_count && 0 == $arc_result[0]->mmonth ) ) : ?>
            <select name='m'>
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
																															
		<input type="submit" id="post-query-submit" value="<?php esc_attr_e('Filter'); ?>" class="button-secondary" />
	
		<?php } // ! is_singular ?>

		<?php if ( isset($_GET['status']) && $_GET['status'] == 'trash' && current_user_can('edit_others_posts') ) { ?>
            <input type="submit" id="delete_all" name="delete_all" value="<?php esc_attr_e('Empty Trash'); ?>" class="button-secondary apply" />
        <?php } ?>

	</div> <!-- end alignleft actions -->

	<?php
	// Display pagination links
     global $wp_query;
     
     if (isset($_GET['paged'])) $cur_page = absint( $_GET['paged'] );
     else $cur_page = 1;
     
    // Paging gallery image list
    $page_links_total =  $wp_query->max_num_pages;
    $page_links = paginate_links( array(
        'base' => add_query_arg( 'paged', '%#%' ),
        'format' => '',
        'prev_text' => __('&laquo;'),
        'next_text' => __('&raquo;'),
        'total' => $page_links_total,
        'current' => $cur_page
    ));
    
    
    if ( $page_links ) : 
    ?>
    <div class="tablenav-pages">
        <?php $page_links_text = sprintf( '<span class="displaying-num">' . __( 'Displaying %s&#8211;%s of %s' ) . '</span>%s',
            number_format_i18n( ( $cur_page - 1 ) * $wp_query->query_vars['posts_per_page'] + 1 ),
            number_format_i18n( min( $cur_page * $wp_query->query_vars['posts_per_page'], $wp_query->found_posts ) ),
            number_format_i18n( $wp_query->found_posts ),
            $page_links
        ); echo $page_links_text; 
        ?>
    </div>
    <?php
    endif;
    ?>
    </div> <!-- end tablenav -->

    <div class="clear"></div>
    <table class="widefat post fixed" cellspacing="0">
        <thead>
        <tr>
    <?php print_column_headers('shiba_mlib'); ?>
        </tr>
        </thead>
    
        <tfoot>
        <tr>
    <?php print_column_headers('shiba_mlib', false); ?>
        </tr>
        </tfoot>
    
        <tbody>
    <?php $shiba_mlib_menu->post_galleries(); ?>
        </tbody>
    </table>
	
    
    <div class="tablenav">
		<?php
        if ( $page_links )
            echo "<div class='tablenav-pages'>$page_links_text</div>";
        ?>

        <div class="alignleft actions">
        <select name="action2" class="select-action">
        <option value="-1" selected="selected"><?php _e('Bulk Actions'); ?></option>
        
        <?php if ( $is_trash ) { ?>
        <option value="untrash"><?php _e('Restore'); ?></option>
        <?php } 
        
        if (function_exists('wp_trash_post') && !$is_trash) { ?>
        <option value="trash"><?php _e('Move to Trash'); ?></option>
        <?php } ?>
        <option value="delete"><?php _e('Delete Permanently'); ?></option>
        </select>
        <input type="submit" value="<?php esc_attr_e('Apply'); ?>" name="doaction2" id="doaction2" class="button-secondary action" />
        
        <?php if ( isset($_GET['status']) && $_GET['status'] == 'trash' && current_user_can('edit_others_posts') ) { ?>
            <input type="submit" id="delete_all2" name="delete_all2" value="<?php esc_attr_e('Empty Trash'); ?>" class="button-secondary apply" />
        <?php } ?>
        </div> <!-- end alignleft actions -->

		<br class="clear" />
	</div> <!-- End tablenav -->
</form>