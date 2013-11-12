<?php
session_start();
$pp_url = 'http://www.gallyapp.com/tf_themes/chalong_wp/';

if(isset($_GET['pp_homepage_slider_style']))
{
	$_SESSION['pp_homepage_slider_style'] = $_GET['pp_homepage_slider_style'];
	header( 'Location: '.$pp_url ) ;
	exit;
}

if(isset($_GET['pp_portfolio_style']))
{
	$_SESSION['pp_portfolio_style'] = $_GET['pp_portfolio_style'];
	header( 'Location: '.$pp_url.'?page_id=11' ) ;
	exit;
}

if(isset($_GET['cl_skin_bg_style']))
{
	$_SESSION['cl_skin_bg_style'] = $_GET['cl_skin_bg_style'];
	header( 'Location: '.$pp_url ) ;
	exit;
}

header( 'Location: '.$_SERVER['HTTP_REFERER'] ) ;
?>