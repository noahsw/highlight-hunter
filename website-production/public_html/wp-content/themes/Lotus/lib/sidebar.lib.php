<?php
if ( function_exists('register_sidebar') )
    register_sidebar(array('name' => 'Home Right Sidebar'));

/**
*	Setup Page side bar
**/
if ( function_exists('register_sidebar') )
    register_sidebar(array('name' => 'Page Sidebar'));
    
/**
*	Setup Contact side bar
**/
if ( function_exists('register_sidebar') )
    register_sidebar(array('name' => 'Contact Sidebar'));
    
/**
*	Setup Blog side bar
**/
if ( function_exists('register_sidebar') )
    register_sidebar(array('name' => 'Blog Sidebar'));
    
/**
*	Setup Footer side bar
**/
if ( function_exists('register_sidebar') )
    register_sidebar(array('name' => 'Footer Sidebar'));
    
    
//Register dynamic sidebar
$dynamic_sidebar = get_option('pp_sidebar');

if(!empty($dynamic_sidebar))
{
	foreach($dynamic_sidebar as $sidebar)
	{
		if ( function_exists('register_sidebar') )
	    register_sidebar(array('name' => $sidebar));
	}
}
?>