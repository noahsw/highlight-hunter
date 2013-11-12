<?php
if (!function_exists('is_admin')) {
    header('Status: 403 Forbidden');
    header('HTTP/1.1 403 Forbidden');
    exit();
}

if (!class_exists("Shiba_Media_Parallelize")) :

class Shiba_Media_Parallelize {

	var $link_num = 0;
	var $link_cache;
	var $image_domains, $wpcontent_domains;
	var $num_domains, $num_wpcontent;
	var $parallel_option, $parallel_wpcontent;

	function clean_image_domains($image_domain_str) {
		$image_domain_str = trim($image_domain_str);	
		$image_domain_str = str_replace("\r", "", $image_domain_str);
		if (empty($image_domain_str)) return NULL;

		$image_domains = explode("\n", $image_domain_str);

		// Clear out empty image domains
		foreach ($image_domains as $key => $domain)
			if (!$domain) unset($image_domains[$key]);		
		$image_domains = array_values($image_domains);
		return implode("\n",$image_domains);
	}	
	
	function get_absolute_link($post_id, $link) {
		$url_parts = parse_url($link);	
		if (isset($url_parts['scheme']) && isset($url_parts['host'])) return $link;
		
		// else relative link
		$permalink = get_permalink($post_id);
		// remove trailing slash from permalink if link is a bookmark
		if ($link[0] == '#') {
			$last = $permalink[strlen($permalink)-1];
			if ($last == '/') { $permalink = substr($permalink, 0, strlen($permalink)-1); }
		} else { // add a trailing slash if necessary		
			$last = $permalink[strlen($permalink)-1];
			if ($last != '/') $permalink .= '/';
		}	
		return $permalink . $link;
	}
	
	function get_filename($original_url, &$dim_arr) {
		global $shiba_mlib;
		$filename = esc_attr(strrchr($original_url, '/'));
		$filename = substr($filename, 1);

		// clean out file dimensions from the filename if any.
		// e.g. shiba-custom-theme10-410x330.jpg - remove part between '-' and '.'								
		$dims = $shiba_mlib->substring($filename,'-','.');
		if ($dims) {
			// ensure dimensional format
			$dim_arr = explode('x', $dims);
			if (is_array($dim_arr) && (count($dim_arr) == 2) &&
				is_numeric($dim_arr[0]) && is_numeric($dim_arr[1])) {
				$dims = '-'.$dims;
				$filename = str_replace($dims, '', $filename);
			}	
		}
		return $filename;
	}
	
	
	function parallelize_image($url, $use_wpcontent=FALSE) {
		global $shiba_mlib;
		
		if (!$use_wpcontent) {
			$domains = $this->image_domains;
			$num_domains = $this->num_domains;
		} else {
			$domains = $this->wpcontent_domains;
			$num_domains = $this->num_wpcontent;
			$this->link_num = 1;			
		}
		$shiba_mlib->print_debug("parallel option = {$this->parallel_option} num domains = {$num_domains}");
		if (($this->parallel_option == 'none') || ($num_domains <= 1)) return $url;		

		if ($this->link_num >= $num_domains) $this->link_num = ($use_wpcontent) ? 1 : 0;
		// Get file name
		$filename = $this->get_filename($url, $size);		
		$shiba_mlib->print_debug("Filename: $filename");

		$pos = strpos($url, $domains[0]);
		if (($pos === FALSE) && $this->parallel_wpcontent && !$use_wpcontent) {
			$wpcontent_url = $this->parallelize_image($url, TRUE);
			$shiba_mlib->print_debug("wpcontent URL: $wpcontent_url");
			return $wpcontent_url;
		}
		
		$shiba_mlib->print_debug("link_num = {$this->link_num} link = {$url} pos = {$pos}");

		// Don't assign to primary domain
		if (!$this->link_num) $this->link_num++;
		if (isset($this->link_cache[$filename])) { // image has previously been assigned a domain
			if ($this->link_cache[$filename] == $this->link_num) $this->link_num++;
			if (!$this->link_cache[$filename]) // primary domain
				return $url;
			$domain = $domains[$this->link_cache[$filename]];
		} elseif (!$this->link_num) { // assign to primary domain by skipping link
			$this->link_cache[$filename] = $this->link_num;
			$this->link_num++; 
			return $url;
			
		} elseif (!$url || ($pos === FALSE) ) { // link is invalid
			return $url;
			
		} else { // reassign domain	
			$domain = $domains[$this->link_num];
			$this->link_cache[$filename] = $this->link_num;				
			$this->link_num++;	
		}
		$new_url = str_replace($domains[0], $domain, $url);
		return $new_url;
	}
	
	
	function _parallelize_link($matches) {
		global $shiba_mlib;
//		$shiba_mlib->print_debug("Parallelize link: " . print_r($matches, TRUE));

		if (is_array($matches))
			$image_link = $matches[0];
		else
			$image_link = $matches;
		$shiba_mlib->print_debug("Image link: " . substr($image_link, 0, 20));

		preg_match_all('/(alt|title|src|width|height)=("[^"]*")/i',$image_link, $result);
		if ((count($result[1]) == 0) || (count($result[2]) == 0))
			preg_match_all("/(alt|title|src|width|height)=('[^']*')/i",$image_link, $result);
		if ((count($result[1]) == 0) || (count($result[2]) == 0))
			return $image_link;
			
		$link_attr = array_combine($result[1], $result[2]);
		$link_attr = array_change_key_case($link_attr);
		$shiba_mlib->print_debug("Link attributes: " . print_r($link_attr, TRUE));
		$link_attr['src'] = trim($link_attr['src'],"\"'");

		$new_link = $this->parallelize_image($link_attr['src']);
		if ($new_link == $link_attr['src']) return $image_link;
		
		$new_html = str_replace($link_attr['src'], $new_link, $image_link);
		$shiba_mlib->print_debug("Image link updated:<br/>{$link_attr['src']} => $new_link");	
		return $new_html;
	}
							
	function parallelize_links($content) {
		global $shiba_mlib;
		
		if (!$content) return $content;
			
		if ($this->num_domains <= 0) return $content;
		// remove <br/> from content since that causes complications with getting img links
		$new_content = preg_replace_callback('/<img [^>]+>/i', array(&$this, '_parallelize_link'), $content);
		if ($new_content) return $new_content;
		else return $content;
	}
	
	function update_image_domains() {
		global $shiba_mlib;
		
		$options = get_option('shiba_mlib_options');
		$this->parallel_option = $options['parallel_option'];
		$this->parallel_wpcontent = isset($options['parallel_wpcontent']) ? TRUE : FALSE;
		$this->image_domains = explode("\n", $options['image_domains']);
		$this->num_domains = count($this->image_domains);
		if (isset($options['wpcontent_domains'])) {
			$this->wpcontent_domains = explode("\n", $options['wpcontent_domains']);
			$this->num_wpcontent = count($this->wpcontent_domains);
		} else $this->parallel_wpcontent = FALSE;
		
		$shiba_mlib->print_debug("Image domains: " . print_r($this->image_domains, TRUE));
		$shiba_mlib->print_debug("WP content domains: " . print_r($this->wpcontent_domains, TRUE));
	}
	
	function clear_cache() {
		// generate links for posts
		$args = array(
			'post_type' => 'any',
			'numberposts' => -1
		); 
		$posts = get_posts($args);
		foreach ($posts as $post) {
			delete_post_meta($post->ID, 'parallel_content');
		}
	}
	
	
	// update parallel content cache on post save		 
	function invalidate_post_cache($post_id) {	
		// verify if this is an auto save routine. If it is our form has not been submitted, so we dont want
		// to do anything
		if ( defined('DOING_AUTOSAVE') && DOING_AUTOSAVE ) 
			return $post_id;
	
		// Check permissions
		if ( 'page' == $_POST['post_type'] ) {
			if ( !current_user_can( 'edit_page', $post_id ) )
				return $post_id;
		} else {
			if ( !current_user_can( 'edit_post', $post_id ) )
			return $post_id;
		}
	
		// OK, we're authenticated: we need to find and save the data
		if (($_POST['post_type'] != 'post') && ($_POST['post_type'] != 'page') && ($_POST['post_type'] != 'gallery')) 
			return $post_id;

		delete_post_meta($post_id, 'parallel_content');
		return $post_id;
	}
	
	
	function get_parallel_content($content) {
		global $shiba_mlib, $post; // assume post is stored in post global
		
		switch($this->parallel_option) {
		case 'none': 
			return $content;
		case 'single':
			if (!is_singular()) return $content; 
			break;
		case 'all':
			break;
		default:
			return $content;
		}			
		$shiba_mlib->print_debug("Get parallel content: " . print_r($post->post_type, TRUE));
		if (($post->post_type != 'post') && ($post->post_type != 'page') && ($post->post_type != 'gallery')) return $content;

		$content = $this->parallelize_links($content);
		return $content;
	}
	
} // end class	
endif;
?>