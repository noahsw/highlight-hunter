<?php
// don't load directly
if (!function_exists('is_admin')) {
    header('Status: 403 Forbidden');
    header('HTTP/1.1 403 Forbidden');
    exit();
}

/**
 * Functions that support AJAX callbacks.
 *
 */
if (!class_exists("Shiba_Media_Ajax")) :
	 
class Shiba_Media_Ajax {


	// For post tag metabox
	function tag_box_title($title) {
		switch ($title) {
		case 'post_tag-shiba_post':
			$this->generate_tag_cloud('post');
			break;
		case 'post_tag-shiba_attachment':
			$this->generate_tag_cloud('attachment');
			break;
		case 'post_tag-shiba_gallery':
			$this->generate_tag_cloud('gallery');
			break;
		}
		return $title;
	}


	function generate_tag_cloud($post_type) {
		global $wpdb;

		// database calls must be sensitive to multisite
		$query = $wpdb->prepare("SELECT ID FROM {$wpdb->posts} WHERE post_type = %s", $post_type);
		$attachment_ids = $wpdb->get_col($query);
		$terms = wp_get_object_terms($attachment_ids, 'post_tag', array('orderby' => 'count', 'order' => 'DESC'));
		$tags = array(); 
		// limit to 45 tags
		foreach ($terms as $term) {
			$tags[$term->term_id] = $term;	
			if (count($tags) >= 45) break;	
		}
	
		if ( empty( $tags ) )
			die( __('No tags found!') );
	
		if ( is_wp_error($tags) )
			die($tags->get_error_message());
	
		foreach ( $tags as $key => $tag ) {
			$tags[ $key ]->link = '#';
			$tags[ $key ]->id = $tag->term_id;
		}
	
		// We need raw tag names here, so don't filter the output
		$return = wp_generate_tag_cloud( $tags, array('filter' => 0) );
	
		if ( empty($return) )
			die('0');
	
		echo $return;
	
		exit;
	}

	// For find posts pop-up on media library menu
	function ajax_library_actions() {
		global $wpdb;
		switch ( $action = $_POST['action'] ) :
		
		case 'shiba_find_posts':
		
		check_ajax_referer( 'find-posts' );
	
		if ( empty($_POST['ps']) )
			exit;
	
		$what = isset($_POST['type']) ? $_POST['type'] : 'post';
		$s = stripslashes($_POST['ps']);
		preg_match_all('/".*?("|$)|((?<=[\\s",+])|^)[^\\s",+]+/', $s, $matches);
		$search_terms = array_map(create_function('$a', 'return trim($a, "\\"\'\\n\\r ");'), $matches[0]);
	
		$searchand = $search = '';
		foreach( (array) $search_terms as $term) {
			$term = addslashes_gpc($term);
			$search .= "{$searchand}(($wpdb->posts.post_title LIKE '%{$term}%') OR ($wpdb->posts.post_content LIKE '%{$term}%'))";
			$searchand = ' AND ';
		}
		$term = $wpdb->escape($s);
		if ( count($search_terms) > 1 && $search_terms[0] != $s )
			$search .= " OR ($wpdb->posts.post_title LIKE '%{$term}%') OR ($wpdb->posts.post_content LIKE '%{$term}%')";
	
		$posts = $wpdb->get_results( "SELECT ID, post_title, post_status, post_date FROM $wpdb->posts WHERE post_type = '$what' AND $search ORDER BY post_date_gmt DESC LIMIT 50" );
	
		if ( ! $posts )
			exit( __('No posts found.') );
	
		$html = '<table class="widefat" cellspacing="0"><thead><tr><th class="found-radio"><br /></th><th>'.__('Title').'</th><th>'.__('Time').'</th><th>'.__('Status').'</th></tr></thead><tbody>';
		foreach ( $posts as $post ) {
	
			switch ( $post->post_status ) {
				case 'publish' :
				case 'private' :
					$stat = __('Published');
					break;
				case 'future' :
					$stat = __('Scheduled');
					break;
				case 'pending' :
					$stat = __('Pending Review');
					break;
				case 'draft' :
					$stat = __('Unpublished');
					break;
			}
	
			if ( '0000-00-00 00:00:00' == $post->post_date ) {
				$time = '';
			} else {
				/* translators: date format in table columns, see http://php.net/date */
				$time = mysql2date(__('Y/m/d'), $post->post_date);
			}
	
			$html .= '<tr class="found-posts"><td class="found-radio"><input type="radio" id="found-'.$post->ID.'" name="found_post_id" value="' . esc_attr($post->ID) . '"></td>';
			$html .= '<td><label for="found-'.$post->ID.'">'.esc_html( $post->post_title ).'</label></td><td>'.esc_html( $time ).'</td><td>'.esc_html( $stat ).'</td></tr>'."\n\n";
		}
		$html .= '</tbody></table>';
	
		$x = new WP_Ajax_Response();
		$x->add( array(
			'what' => $what,
			'data' => $html
		));
		$x->send();	
		break;
		endswitch; // end switch
	 }
} // end class	
endif;
?>