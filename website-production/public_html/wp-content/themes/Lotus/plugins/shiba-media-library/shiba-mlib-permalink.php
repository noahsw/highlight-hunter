<?php
// don't load directly
if (!function_exists('is_admin')) {
    header('Status: 403 Forbidden');
    header('HTTP/1.1 403 Forbidden');
    exit();
}


if (!class_exists("Shiba_Media_Permalink")) :

class Shiba_Media_Permalink {

	function activate() {
		add_option('gallery_base', '');
 		add_option('gallery_structure', "");
	}

	function init() {
		global $wp_rewrite;
		// Register gallery post_type for WordPress 3.0
		$this->register_gallery_object();
		add_filter('post_type_link', array(&$this,'gallery_permalink'), 10, 3);	

		// Add 'gallery' to permalink structure					
		$wp_rewrite->add_rewrite_tag("%gallery%", '([^/]+)', "gallery=");
		$this->set_gallery_permastruct();
	}
	
	function register_gallery_object($name = 'Gallery') {
		$labels = array(
			'name' => _x('Galleries', 'post type general name'),
			'singular_name' => _x('Gallery', 'post type singular name'),
			'add_new' => _x('Add New', 'gallery'),
			'add_new_item' => __("Add New Gallery"),
			'edit_item' => __("Edit Gallery"),
			'new_item' => __("New Gallery"),
			'view_item' => __("View Gallery"),
			'search_items' => __("Search Gallery"),
			'not_found' =>  __('No galleries found'),
			'not_found_in_trash' => __('No galleries found in Trash'), 
			'parent_item_colon' => ''
		  );
		  $args = array(
			'labels' => $labels,
			'public' => true,
			'show_ui' => true, 
			'publicly_queryable' => true,
			'query_var' => true,
			'rewrite' => false,
			'capability_type' => 'post',
			'hierarchical' => false,
			'menu_position' => 10,
			'taxonomies' => array(),
			'supports' => array('title')
		  ); 
		  register_post_type('gallery',$args);
	}
	
		
	function show_gallery_structure() {
		global $blog_prefix, $shiba_mlib;
		$gallery_structure = get_option('gallery_structure');
		if ( function_exists('is_multisite') && is_multisite() && !is_subdomain_install() && is_main_site() ) {
			$gallery_structure = preg_replace( '|^/?blog|', '', $gallery_structure );
		}	
		echo $blog_prefix;
		echo "<input name=\"gallery_structure\" id=\"gallery_structure\" type=\"text\" value=\"{$gallery_structure}\" class=\"regular-text code\" />";
		echo "<p style='font-size:90%;'>Gallery permastruct must contain %gallery% and cannot be a duplicate of blog permastruct. In addition /%gallery%/, /%author%/%gallery%/, /%category%/%gallery%/, are all not valid because it disrupts other object permalinks.</p>";
	}

	function show_gallery_base() {
		$gallery_base = get_option('gallery_base');
		echo "<input name=\"gallery_base\" id=\"gallery_base\" type=\"text\" value=\"{$gallery_base}\" class=\"regular-text code\" />";
	}

	function set_gallery_permastruct() {
		
		$permalink_structure = get_option('permalink_structure');
		$gallery_structure = get_option('gallery_structure');

		if  ( $gallery_structure ) {
			// false is important in add_permastruct so that it does not append the general blog structure our gallery structure
			add_permastruct('gallery', $gallery_structure, false);    
		} else {
			add_permastruct('gallery', "", false);  
		}	   
	}
	
	function set_gallery_structure($input) {
		global $shiba_mlib;
		if ( isset($_POST['gallery_structure']) ) {
			check_admin_referer('update-permalink');
			$permalink_structure = get_option('permalink_structure');
			$gallery_structure = $_POST['gallery_structure'];
			
			// MUST NOT contain %postname% tag
			if ($gallery_structure && (strpos($gallery_structure, "%postname%") !== FALSE)) 
				$gallery_structure = str_replace("%postname%","%gallery%", $gallery_structure);
						
			// Must be using permalink structure
			if ( !$permalink_structure ) {
				$gallery_structure = "";
			} elseif (! empty($gallery_structure) ) {
				$gallery_structure = preg_replace('#/+#', '/', '/' . $gallery_structure);

				//build an array of the tags (note that said array ends up being in $tokens[0])
				preg_match_all('/%.+?%/', $gallery_structure, $tokens);
				$gallery_tags = $tokens[0];

				if (!is_array($gallery_tags)) { // no tokens
					$gallery_structure = ""; 
				// MUST contain %gallery% token
				} elseif (!in_array("%gallery%", $gallery_tags)) {
					$gallery_structure = ""; 
				} 
			} else {// empty gallery struct
				$gallery_structure = ""; }

			if ($gallery_structure) {
				$gallery_structure = rtrim($gallery_structure, '/');
				// gallery structure must have trailing slash or not according to permalink structure
				$last = $permalink_structure[strlen($permalink_structure)-1];
				if ($last == '/')
					$gallery_structure .= '/';

				// It can't just be gallery because if so, the rules will disrupt page structure rules
				$trim_gallery_structure = trim($gallery_structure,'/');
				if ( $trim_gallery_structure == "%gallery%")
					$gallery_structure = "";
					
				// It can't just be %author% or %category% and gallery because that will desrupt category and tag rules
				if (($trim_gallery_structure == '%category%/%gallery%') || 
					($trim_gallery_structure == '%author%/%gallery%') ) {
						$gallery_structure = "";
				}		
			}
			
			// check if gallery structure is duplicate of general permalink structure
			if (str_replace('%gallery%','%postname%', $gallery_structure) == $permalink_structure)
				$gallery_structure = "";	

			// check if gallery structure is new
			$old_structure = get_option('gallery_structure');
			if ( $gallery_structure && function_exists('is_multisite') && is_multisite() && !is_subdomain_install() && is_main_site() ) 
				$gallery_structure = '/blog' . $gallery_structure;
			if ($gallery_structure != $old_structure)	
				update_option('gallery_structure', $gallery_structure);
			
			$this->set_gallery_permastruct();
		}
		return $input;	
	}
	
	function gallery_permalink($permalink, $post_id, $leavename) {
		$post = get_post($post_id);
		$rewritecode = array(
			'%year%',
			'%monthnum%',
			'%day%',
			'%hour%',
			'%minute%',
			'%second%',
			$leavename? '' : '%postname%',
			'%post_id%',
			'%category%',
			'%author%',
			$leavename? '' : '%pagename%',
		);

		// Taken from get_permalink function in wp-includes/link-template.php
		if ( '' != $permalink && !in_array($post->post_status, array('draft', 'pending', 'auto-draft')) ) {
			$unixtime = strtotime($post->post_date);
	
			$category = '';
			if ( strpos($permalink, '%category%') !== false ) {
				$cats = get_the_category($post->ID);
				if ( $cats ) {
					usort($cats, '_usort_terms_by_ID'); // order by ID
					$category = $cats[0]->slug;
					if ( $parent = $cats[0]->parent )
						$category = get_category_parents($parent, false, '/', true) . $category;
				}
				// show default category in permalinks, without
				// having to assign it explicitly
				if ( empty($category) ) {
					$default_category = get_category( get_option( 'default_category' ) );
					$category = is_wp_error( $default_category ) ? '' : $default_category->slug;
				}
			}
	
			$author = '';
			if ( strpos($permalink, '%author%') !== false ) {
				$authordata = get_userdata($post->post_author);
				$author = $authordata->user_nicename;
			}
	
			$date = explode(" ",date('Y m d H i s', $unixtime));
			$rewritereplace =
			array(
				$date[0],
				$date[1],
				$date[2],
				$date[3],
				$date[4],
				$date[5],
				$post->post_name,
				$post->ID,
				$category,
				$author,
				$post->post_name,
			);
			$permalink = str_replace($rewritecode, $rewritereplace, $permalink);
//			$permalink = home_url( str_replace($rewritecode, $rewritereplace, $permalink) );
//			$permalink = user_trailingslashit($permalink, 'single');
		} else { // if they're not using the fancy permalink option
//			$permalink = home_url('?p=' . $post->ID);
		}
		return $permalink;
	}
	

} // end class	
endif;
?>