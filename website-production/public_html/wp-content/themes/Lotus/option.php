<?php
session_start();
$pp_url = 'http://www.gallyapp.com/tf_themes/chalong_wp/';

if(isset($_GET['reset']) && $_GET['reset'])
{
	session_destroy();
}
else
{
	if(isset($_GET['pp_show_hide_option']))
	{
		$_SESSION['pp_show_hide_option'] = $_GET['pp_show_hide_option'];
		exit;
	}

	if(isset($_GET['pp_bg_color']))
	{
		$_SESSION['pp_bg_color'] = $_GET['pp_bg_color'];
	}
	
	if(isset($_GET['pp_menu_bg_color']))
	{
		$_SESSION['pp_menu_bg_color'] = $_GET['pp_menu_bg_color'];
	}
}

header( 'Location: '.$_SERVER['HTTP_REFERER'] ) ;
?>