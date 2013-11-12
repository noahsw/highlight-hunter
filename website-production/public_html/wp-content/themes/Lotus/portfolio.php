<?php
/**
 * Template Name: Portfolio
 * The main template file for display portfolio page.
 *
 * @package WordPress
 * @subpackage Soon
*/

if(!isset($hide_header) OR !$hide_header)
{
	get_header(); 
}

$pp_portfolio_style = get_option('pp_portfolio_style');

if(empty($pp_portfolio_style))
{
	$pp_portfolio_style = '1';
}

include (TEMPLATEPATH . "/templates/template-portfolio-".$pp_portfolio_style.".php");

?>