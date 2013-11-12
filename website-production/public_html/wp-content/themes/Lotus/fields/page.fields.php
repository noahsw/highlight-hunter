<?php

/**
 * The PHP code for setup Theme page custom fields.
 *
 * @package WordPress
 * @subpackage Pai
 */


/*
	Begin creating custom fields
*/

$theme_sidebar = array('','Page Sidebar', 'Contact Sidebar', 'Blog Sidebar');
$dynamic_sidebar = get_option('pp_sidebar');

if(!empty($dynamic_sidebar))
{
	foreach($dynamic_sidebar as $sidebar)
	{
		$theme_sidebar[] = $sidebar;
	}
}

$page_postmetas = 
	array (
		/*
			Begin Page custom fields
		*/
		array("section" => "Page Style", "id" => "page_style", "type" => "select", "title" => "Select Page style (Right Sidebar style will include wiget on the right side)", "items" => array("Full Width", "Right Sidebar", "Left Sidebar")),
		
		array("section" => "Select Sidebar", "id" => "page_sidebar", "type" => "select", "title" => "Select this page's sidebar to display", "items" => $theme_sidebar),
		
		array("section" => "Page Button", "id" => "page_button_title", "type" => "text", "title" => "Page Button title"),
		array("section" => "Page Button", "id" => "page_button_href", "type" => "text", "title" => "Page Button hyperlink URL."),
		/*
			End Page custom fields
		*/
		
	);
?>
<?php

function page_create_meta_box() {

	global $page_postmetas;
	if ( function_exists('add_meta_box') && isset($page_postmetas) && count($page_postmetas) > 0 ) {  
		add_meta_box( 'page_metabox', 'Page Options', 'page_new_meta_box', 'page', 'normal', 'high' );  
	}

}  

function page_new_meta_box() {
	global $post, $page_postmetas;

	echo '<input type="hidden" name="myplugin_noncename" id="myplugin_noncename" value="' . wp_create_nonce( plugin_basename(__FILE__) ) . '" />';
	echo '<br/>';
	
	$meta_section = '';

	foreach ( $page_postmetas as $postmeta ) {

		$meta_id = $postmeta['id'];
		$meta_title = $postmeta['title'];
		
		$meta_type = '';
		if(isset($postmeta['type']))
		{
			$meta_type = $postmeta['type'];
		}
		
		if(empty($meta_section) OR $meta_section != $postmeta['section'])
		{
			$meta_section = $postmeta['section'];
			
			echo "<h3>".$meta_section."</h3><br/>";
		}
		$meta_section = $postmeta['section'];

		echo "<p><label for='$meta_id'>$meta_title </label>";

		if ($meta_type == 'checkbox') {
			$checked = get_post_meta($post->ID, $meta_id, true) == '1' ? "checked" : "";
			echo "<input type='checkbox' name='$meta_id' id='$meta_id' value='1' $checked /></p>";
		}
		else if ($meta_type == 'select') {
			echo "<p><select name='$meta_id' id='$meta_id'>";
			
			if(!empty($postmeta['items']))
			{
				foreach ($postmeta['items'] as $item)
				{
					$page_style = get_post_meta($post->ID, $meta_id);
				
					if(isset($page_style[0]) && $item == $page_style[0])
					{
						$css_string = 'selected';
					}
					else
					{
						$css_string = '';
					}
				
					echo '<option value="'.$item.'" '.$css_string.'>'.$item.'</option>';
				}
			}
			
			echo "</select></p>";
		}
		else {
			echo "<input type='text' name='$meta_id' id='$meta_id' class='code' value='".get_post_meta($post->ID, $meta_id, true)."' style='width:99%' /></p>";
		}
	}
	
	echo '<br/>';

}

function page_save_postdata( $post_id ) {

	global $page_postmetas;

	// verify this came from the our screen and with proper authorization,
	// because save_post can be triggered at other times

	if ( isset($_POST['myplugin_noncename']) && !wp_verify_nonce( $_POST['myplugin_noncename'], plugin_basename(__FILE__) )) {
		return $post_id;
	}

	// verify if this is an auto save routine. If it is our form has not been submitted, so we dont want to do anything

	if ( defined('DOING_AUTOSAVE') && DOING_AUTOSAVE ) return $post_id;

	// Check permissions

	if ( isset($_POST['post_type']) && 'page' == $_POST['post_type'] ) {
		if ( !current_user_can( 'edit_page', $post_id ) )
			return $post_id;
		} else {
		if ( !current_user_can( 'edit_post', $post_id ) )
			return $post_id;
	}

	// OK, we're authenticated

	if ( $parent_id = wp_is_post_revision($post_id) )
	{
		$post_id = $parent_id;
	}

	foreach ( $page_postmetas as $postmeta ) {
	
		if ($_POST[$postmeta['id']]) {
			page_update_custom_meta($post_id, $_POST[$postmeta['id']], $postmeta['id']);
		}

		if ($_POST[$postmeta['id']] == "") {
			delete_post_meta($post_id, $postmeta['id']);
		}
	}

}

function page_update_custom_meta($postID, $newvalue, $field_name) {

	if (!get_post_meta($postID, $field_name)) {
		add_post_meta($postID, $field_name, $newvalue);
	} else {
		update_post_meta($postID, $field_name, $newvalue);
	}

}

//init

add_action('admin_menu', 'page_create_meta_box'); 
add_action('save_post', 'page_save_postdata'); 

/*
	End creating custom fields
*/

?>
