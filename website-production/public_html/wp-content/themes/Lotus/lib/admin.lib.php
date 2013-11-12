<?php
/*
	Begin creating admin options
*/

$themename = THEMENAME;
$shortname = SHORTNAME;

$pp_slider_items = get_option('pp_slider_items');
if(empty($pp_slider_items))
{
	$pp_slider_items = 5;
}

$slides = get_posts(array(
	'post_type' => 'slides',
	'numberposts' => $pp_slider_items,
));
$wp_slides = array(
	0		=> "Choose a slide"
);
foreach ($slides as $slide_list ) {
       $wp_slides[$slide_list->ID] = $slide_list->post_title;
}

$categories = get_categories('hide_empty=0&orderby=name');
$wp_cats = array(
	0		=> "Choose a category"
);
foreach ($categories as $category_list ) {
       $wp_cats[$category_list->cat_ID] = $category_list->cat_name;
}

$pages = get_pages(array('parent' => -1));
$wp_pages = array(
	0		=> "Choose a page"
);
foreach ($pages as $page_list ) {
	$template_name = get_post_meta( $page_list->ID, '_wp_page_template', true );
	
	//exclude contact template
	if($template_name != 'contact.php')
	{
       $wp_pages[$page_list->ID] = $page_list->post_title;
    }
}

$pp_handle = opendir(TEMPLATEPATH.'/fonts');
$pp_font_arr = array();

while (false!==($pp_file = readdir($pp_handle))) {
	if ($pp_file != "." && $pp_file != ".." && $pp_file != ".DS_Store") {
		$pp_file_name = basename($pp_file, '.js');
		
		if($pp_file_name != 'Quicksand_300.font')
		{
			$pp_name = explode('_', $pp_file_name);
		
			$pp_font_arr[$pp_file_name] = $pp_name[0];
		}
	}
}
closedir($pp_handle);
asort($pp_font_arr);


$options = array (
 
//Begin admin header
array( 
		"name" => $themename." Options",
		"type" => "title"
),
//End admin header
 

//Begin first tab "General"
array( 
		"name" => "General",
		"type" => "section"
)
,

array( "type" => "open"),

array( "name" => "Header Logo",
	"desc" => "Choose an image that you want to use as the logo in header",
	"id" => $shortname."_logo",
	"type" => "image",
	"std" => "",
),
array( "name" => "Google Analytics Domain ID ",
	"desc" => "Get analytics on your site. Simply give us your Google Analytics Domain ID (something like UA-123456-1)",
	"id" => $shortname."_ga_id",
	"type" => "text",
	"std" => ""

),
array( "name" => "Google Maps API key ",
	"desc" => "Get maps on your site. Simply give us your Google Maps API key (<a href=\"http://code.google.com/apis/maps/signup.html\">You can get it here</a>)",
	"id" => $shortname."_gm_key",
	"type" => "text",
	"std" => ""

),
array( "name" => "Custom Favicon",
	"desc" => "A favicon is a 16x16 pixel icon that represents your site; paste the URL to a .ico image that you want to use as the image",
	"id" => $shortname."_favicon",
	"type" => "image",
	"std" => "",
),

array( "name" => "Footer Content",
	"desc" => "You can text and HTML in here",
	"id" => $shortname."_footer_text",
	"type" => "textarea",
	"std" => ""

),

array( "name" => "Your email address",
	"desc" => "Enter which email address will be sent from contact form",
	"id" => $shortname."_contact_email",
	"type" => "text",
	"std" => ""
),

array( "name" => "Custom CSS",
	"desc" => "You can add your custom CSS here",
	"id" => $shortname."_custom_css",
	"type" => "textarea",
	"std" => ""

),
	
array( "type" => "close"),
//End first tab "General"


//Begin first tab "Font"
array( 
		"name" => "Font",
		"type" => "section"
)
,

array( "type" => "open"),

array( "name" => "H1 Size (in pixels)",
	"desc" => "",
	"id" => $shortname."_h1_size",
	"type" => "jslider",
	"size" => "40px",
	"std" => "38",
	"from" => 13,
	"to" => 60,
	"step" => 1,
),
array( "name" => "H2 Size (in pixels)",
	"desc" => "",
	"id" => $shortname."_h2_size",
	"type" => "jslider",
	"size" => "40px",
	"std" => "34",
	"from" => 13,
	"to" => 60,
	"step" => 1,
),
array( "name" => "H3 Size (in pixels)",
	"desc" => "",
	"id" => $shortname."_h3_size",
	"type" => "jslider",
	"size" => "40px",
	"std" => "30",
	"from" => 13,
	"to" => 60,
	"step" => 1,
),
array( "name" => "H4 Size (in pixels)",
	"desc" => "",
	"id" => $shortname."_h4_size",
	"type" => "jslider",
	"size" => "40px",
	"std" => "28",
	"from" => 13,
	"to" => 60,
	"step" => 1,
),
array( "name" => "H5 Size (in pixels)",
	"desc" => "",
	"id" => $shortname."_h5_size",
	"type" => "jslider",
	"size" => "40px",
	"std" => "24",
	"from" => 13,
	"to" => 60,
	"step" => 1,
),
array( "name" => "H6 Size (in pixels)",
	"desc" => "",
	"id" => $shortname."_h6_size",
	"type" => "jslider",
	"size" => "40px",
	"std" => "22",
	"from" => 13,
	"to" => 60,
	"step" => 1,
),
	
array( "type" => "close"),
//End first tab "Font"


//Begin first tab "Colors"
array( 
		"name" => "Colors",
		"type" => "section"
)
,

array( "type" => "open"),

array( "name" => "Header Background Pattern",
	"desc" => "Select background style your header",
	"id" => $shortname."_bg_pattern",
	"type" => "select",
	"options" => array(
		'' => 'None',
		'ver_strip' => 'Vertical Strip',
		'hor_strip' => 'Horizontal Strip',
		'right_strip' => 'Right Strip',
		'right_strip2' => 'Right Strip2',
		'left_strip' => 'Left Strip',
		'left_strip2' => 'Left Strip2',
		'square' => 'Square',
		'square2' => 'Square2',
		'dot' => 'Dot',
		'circle' => 'Circle',
		'flower' => 'Flower1',
		'flower2' => 'Flower2',
		'flower3' => 'Flower3',
		'flower4' => 'Flower4',
		'paper' => 'Paper',
		'gear' => 'Gear',
		'mosaic' => 'Mosaic',
		'metal' => 'Metal',
		'jeans' => 'Jeans',
		'bokeh' => 'Bokeh',
	),
	"std" => ''
),

array( "name" => "Header Background Color",
	"desc" => "Select color for the header background (default #212C35)",
	"id" => $shortname."_bg",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#212C35"

),

array( "name" => "Menu Font Color",
	"desc" => "Select color for the menu font (default #ffffff)",
	"id" => $shortname."_menu_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#ffffff"

),

array( "name" => "Title Font Color",
	"desc" => "Select color for the title font (default #ffffff)",
	"id" => $shortname."_title_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#ffffff"

),

array( "name" => "Font Color",
	"desc" => "Select color for the font (default #B1B1B1)",
	"id" => $shortname."_font_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#B1B1B1"

),

array( "name" => "Link Color",
	"desc" => "Select color for the link (default #21759B)",
	"id" => $shortname."_link_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#21759B"

),

array( "name" => "Hover Link Color",
	"desc" => "Select color for the hover link (default #3587AD)",
	"id" => $shortname."_hover_link_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#3587AD"

),

array( "name" => "H1, H2, H3, H4, H5, H6 Color",
	"desc" => "Select color for the H1, H2, H3, H4, H5, H6 (default #555555)",
	"id" => $shortname."_h1_font_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#555555"

),

array( "name" => "Button Background Color",
	"desc" => "Select color for the button background (default #07418D)",
	"id" => $shortname."_button_bg_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#07418D"

),

array( "name" => "Button Font Color",
	"desc" => "Select color for the button font (default #ffffff)",
	"id" => $shortname."_button_font_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#ffffff"

),

array( "name" => "Button Border Color",
	"desc" => "Select color for the button border (default #07418D)",
	"id" => $shortname."_button_border_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#07418D"

),

array( "name" => "Footer Font Color",
	"desc" => "Select color for the footer font (default #ccc)",
	"id" => $shortname."_footer_font_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#ccc"

),

array( "name" => "Footer Link Color",
	"desc" => "Select color for the footer link (default #ffffff)",
	"id" => $shortname."_footer_link_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#ffffff"

),

array( "name" => "Footer Hover Link Color",
	"desc" => "Select color for the footer hover link (default #ffffff)",
	"id" => $shortname."_footer_hover_link_color",
	"type" => "colorpicker",
	"size" => "60px",
	"std" => "#ffffff"

),

array( "type" => "close"),
//End first tab "Colors"


//Begin second tab "Slider"
array( "name" => "Slider",
	"type" => "section"),
array( "type" => "open"),

array( "name" => "Slider sort by",
	"desc" => "Select sorting type for contents in slider",
	"id" => $shortname."_slider_sort",
	"type" => "select",
	"options" => array(
		'DESC' => 'Newest First',
		'ASC' => 'Oldest First',
	),
	"std" => "ASC"
),

array( "name" => "Slider items",
	"desc" => "How many items you want display in slider?",
	"id" => $shortname."_slider_items",
	"type" => "jslider",
	"size" => "40px",
	"std" => "5",
	"from" => 1,
	"to" => 10,
	"step" => 1,
),

array( "name" => "Slider timer (in second)",
	"desc" => "Enter number of seconds for slider timer",
	"id" => $shortname."_slider_timer",
	"type" => "jslider",
	"size" => "40px",
	"std" => "5",
	"from" => 1,
	"to" => 10,
	"step" => 1,
),

array( "name" => "Enable Auto Play",
	"desc" => "",
	"id" => $shortname."_slider_auto_play",
	"type" => "iphone_checkboxes",
	"std" => 1
),

array( "name" => "Slider animation time (in milliseconds)",
	"desc" => "Enter number of milliseconds for slider animation",
	"id" => $shortname."_slider_animation_time",
	"type" => "jslider",
	"size" => "40px",
	"std" => "600",
	"from" => 100,
	"to" => 2000,
	"step" => 100,
),

array( "type" => "close"),
//End second tab "Slider"


//Begin second tab "Homepage"
array( "name" => "Homepage",
	"type" => "section"),
array( "type" => "open"),

array( "name" => "Select and sort contents on your homepage. <br/><br/> Use pages you want to show on your homepage <br/><br/><a href='".admin_url("post-new.php?post_type=page")."' class='button'>Create Page</a>",
	"sort_title" => "Homepage Content Manager",
	"desc" => "",
	"id" => $shortname."_homepage_content",
	"type" => "sortable",
	"options" => $wp_pages,
	"std" => ''
),

array( "name" => "Show Right Sidebar on Homepage",
	"desc" => "Select display right sidebar on homepage ",
	"id" => $shortname."_homepage_hide_right_sidebar",
	"type" => "iphone_checkboxes",
	"std" => 1
),

array( "name" => "Show Slider on Homepage",
	"desc" => "Select if you want to show or hide content slider on homepage",
	"id" => $shortname."_homepage_hide_slider",
	"type" => "iphone_checkboxes",
	"std" => 1
),

array( "name" => "Show Tagline on Homepage",
	"desc" => "Select if you want to show or hide tagline on homepage",
	"id" => $shortname."_homepage_hide_tagline",
	"type" => "iphone_checkboxes",
	"std" => 1
),

array( "name" => "Tagline title text",
	"desc" => "Enter text to display in homepage tagline header",
	"id" => $shortname."_homepage_tagline_title",
	"type" => "text",
	"std" => "Built with the latest Wordpress features",
),

array( "name" => "Tagline button text",
	"desc" => "Enter text to display in button",
	"id" => $shortname."_tagline_button_title",
	"type" => "text",
	"std" => "Buy Now",
),

array( "name" => "Tagline button link URL",
	"desc" => "Enter URL for tagline button",
	"id" => $shortname."_tagline_button_href",
	"type" => "text",
	"std" => "",
),

array( "type" => "close"),
//End second tab "Homepage"


//Begin second tab "Portfolio"
array( "name" => "Portfolio",
	"type" => "section"),
array( "type" => "open"),

array( "name" => "Portfolio styles",
	"desc" => "Select the columns style for the portfolio",
	"id" => $shortname."_portfolio_style",
	"type" => "select",
	"options" => array(
		3 => '3 Columns',
		4 => '4 Columns',
		'thumb' => 'Thumbnail Only',
		'thumb-detail' => 'Thumbnail With Detail',
	),
	"std" => 1
),
array( "name" => "Portfolio sort by",
	"desc" => "Select sorting type for contents in portfolio",
	"id" => $shortname."_portfolio_sort",
	"type" => "select",
	"options" => array(
		'DESC' => 'Newest First',
		'ASC' => 'Oldest First',
	),
	"std" => "ASC"
),
array( "name" => "Portfolio items per page",
	"desc" => "Enter how many items get displayed in portfolio page (default is 12 items)",
	"id" => $shortname."_portfolio_items",
	"type" => "jslider",
	"size" => "40px",
	"std" => "12",
	"from" => 1,
	"to" => 20,
	"step" => 1,
),

array( "type" => "close"),
//End second tab "Portfolio"


//Begin second tab "Gallery"
array( "name" => "Gallery",
	"type" => "section"),
array( "type" => "open"),

array( "name" => "Gallery sort by",
	"desc" => "Select sorting type for contents in gallery",
	"id" => $shortname."_gallery_sort",
	"type" => "select",
	"options" => array(
		'DESC' => 'Newest First',
		'ASC' => 'Oldest First',
	),
	"std" => "ASC"
),

array( "type" => "close"),
//End second tab "Gallery"


//Begin second tab "Sidebar"
array( "name" => "Sidebar",
	"type" => "section"),
array( "type" => "open"),

array( "name" => "Add a new sidebar",
	"desc" => "Enter sidebar name",
	"id" => $shortname."_sidebar0",
	"type" => "text",
	"std" => "",
),
array( "type" => "close"),
//End second tab "Sidebar"


//Begin second tab "Blog"
array( "name" => "Blog",
	"type" => "section"),
array( "type" => "open"),

array( "name" => "Custom Blog Page Title",
	"desc" => "Enter title text to display in Blog template",
	"id" => $shortname."_blog_title",
	"type" => "text",
	"std" => "Blog",
),
array( "name" => "Show About author module",
	"desc" => "Select display about the author in single blog page ",
	"id" => $shortname."_blog_display_author",
	"type" => "iphone_checkboxes",
	"std" => 1
),
array( "name" => "Show Related posts module",
	"desc" => "Select display related posts in single blog page ",
	"id" => $shortname."_blog_display_related",
	"type" => "iphone_checkboxes",
	"std" => 1
),
array( "type" => "close"),
//End second tab "Blog"


//Begin first tab "Contact"
array( 
		"name" => "Contact",
		"type" => "section"
)
,

array( "type" => "open"),

array( "name" => "Your email address",
	"desc" => "Enter which email address will be sent from contact form",
	"id" => $shortname."_contact_email",
	"type" => "text",
	"std" => ""
),
array( "name" => "Show map in contact page",
	"desc" => "Select display map in contact page",
	"id" => $shortname."_contact_display_map",
	"type" => "iphone_checkboxes",
	"std" => 1
),
array( "name" => "Address Latitude",
	"desc" => "<a href=\"http://www.tech-recipes.com/rx/5519/the-easy-way-to-find-latitude-and-longitude-values-in-google-maps/\">Find here</a>",
	"id" => $shortname."_contact_lat",
	"type" => "text",
	"std" => ""
),
array( "name" => "Address Longtitude",
	"desc" => "<a href=\"http://www.tech-recipes.com/rx/5519/the-easy-way-to-find-latitude-and-longitude-values-in-google-maps/\">Find here</a>",
	"id" => $shortname."_contact_long",
	"type" => "text",
	"std" => ""
),
array( "name" => "Map Zoom level",
	"desc" => "",
	"id" => $shortname."_contact_map_zoom",
	"type" => "jslider",
	"size" => "40px",
	"std" => "12",
	"from" => 1,
	"to" => 18,
	"step" => 1,
),
	
array( "type" => "close"),
//End first tab "Contact"

 
array( "type" => "close")
 
);
?>