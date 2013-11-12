<?php
/**
 * The Header for the template.
 *
 * @package WordPress
 */

$pp_theme_version = '1.1';

$isTest = !(strpos($_SERVER["SERVER_NAME"], "test") === false);

// Redirect to mobile?
// This goes here because we can't use is_front_page() from a plugin. It's too early.
if (!is_admin() &&
    is_home_page() &&
    is_mobile_device() &&
    !isset($_GET['desktop']))
{
    if ($isTest)
        header("Location: http://test.m.highlighthunter.com");
    else
        header("Location: http://m.highlighthunter.com");
    exit;
}

function is_home_page() {
    return (is_front_page() || is_page("home") || is_page("home2"));
}

function is_mobile_device() {
    // from http://detectmobilebrowsers.com/
    $useragent=$_SERVER['HTTP_USER_AGENT'];
    if (preg_match('/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i',
            $useragent) ||
        preg_match('/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i',
            substr($useragent,0,4)))
        return true;
    else
        return false;
}


?><!DOCTYPE html>
<html <?php language_attributes(); ?>>
<head>

<?php if (is_front_page()) { ?>
<script>
<?php
if ($isTest) {
    $analytics_key = '54666024-0';
    echo '_udn = "test.highlighthunter.com";';
} else {
    $analytics_key = '54666024-1';
    echo '_udn = "www.highlighthunter.com";';
}
?>
</script>
<!-- Google Analytics Content Experiment code -->
<script>function utmx_section(){}function utmx(){}(function(){var
k='<?php echo $analytics_key; ?>',d=document,l=d.location,c=d.cookie;
if(l.search.indexOf('utm_expid='+k)>0)return;
function f(n){if(c){var i=c.indexOf(n+'=');if(i>-1){var j=c.
indexOf(';',i);return escape(c.substring(i+n.length+1,j<0?c.
length:j))}}}var x=f('__utmx'),xx=f('__utmxx'),h=l.hash;d.write(
'<sc'+'ript src="'+'http'+(l.protocol=='https:'?'s://ssl':
'://www')+'.google-analytics.com/ga_exp.js?'+'utmxkey='+k+
'&utmx='+(x?x:'')+'&utmxx='+(xx?xx:'')+'&utmxtime='+new Date().
valueOf()+(h?'&utmxhash='+escape(h.substr(1)):'')+
'" type="text/javascript" charset="utf-8"><\/sc'+'ript>')})();
</script><script>utmx('url','A/B');</script>
<!-- End of Google Analytics Content Experiment code -->
<?php } // if is front page ?>

<meta charset="<?php bloginfo( 'charset' ); ?>" />
<title><?php wp_title('&lsaquo;', true, 'right'); ?></title>
<link rel="profile" href="http://gmpg.org/xfn/11" />
<link rel="pingback" href="<?php bloginfo( 'pingback_url' ); ?>" />
<link href='http://fonts.googleapis.com/css?family=Open+Sans:400italic,400' rel='stylesheet' type='text/css'>



<?php
	/**
	*	Get favicon URL
	**/
	$pp_favicon = get_option('pp_favicon');

	if(!empty($pp_favicon))
	{
?>
		<link rel="shortcut icon" href="<?php echo get_bloginfo( 'stylesheet_directory' ); ?>/cache/<?php echo $pp_favicon; ?>" />
<?php
	}
?>

<!-- Template stylesheet -->
<?php

	///* combined into single.css (which is now loaded above as the main stylesheet
	wp_enqueue_style("jqueryui_css", get_bloginfo( 'stylesheet_directory' )."/css/jqueryui/custom.css", false, $pp_theme_version, "all");
	wp_enqueue_style("screen_css", get_bloginfo( 'stylesheet_directory' )."/css/screen.css", false, $pp_theme_version, "all");
	wp_enqueue_style("fancybox_css", get_bloginfo( 'stylesheet_directory' )."/js/fancybox/jquery.fancybox-1.3.0.css", false, $pp_theme_version, "all");

	//wp_enqueue_style("custom_css", get_bloginfo( 'stylesheet_directory' )."/css/custom.css", false, $pp_theme_version, "all");

	/* not used (these are for html5 video player)
	//wp_enqueue_style("videojs_css", get_bloginfo( 'stylesheet_directory' )."/js/video-js.css", false, $pp_theme_version, "all");
	//wp_enqueue_style("vim_css", get_bloginfo( 'stylesheet_directory' )."/js/skins/vim.css", false, $pp_theme_version, "all");
	*/

	/**
	*	Get custom CSS
	**/
	$pp_custom_css = get_option('pp_custom_css');


	if(!empty($pp_custom_css))
	{
		echo '<style>';
		echo $pp_custom_css;
		echo '</style>';
	}
?>
<!--[if lt IE 8]>
<link rel="stylesheet" href="<?php bloginfo( 'stylesheet_directory' ); ?>/css/ie7.css?v=<?php echo $pp_theme_version; ?>" type="text/css" media="all"/>
<![endif]-->

<!--[if lte IE 8]>
<link rel="stylesheet" href="<?php bloginfo( 'stylesheet_directory' ); ?>/css/ie.css?v=<?php echo $pp_theme_version; ?>" type="text/css" media="all"/>
<![endif]-->



<?

	/**
	*	Check Google Maps key
	**/
	$pp_gm_key = get_option('pp_gm_key');

	if(!empty($pp_gm_key))
	{

?>
<script type="text/javascript" src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=<?php echo $pp_gm_key; ?>&amp;hl=en"></script>
<?php
	}
?>

<?php
	// this file doesn't exist. seems to be a bug in the theme
	wp_enqueue_script("jquery", get_bloginfo( 'stylesheet_directory' )."/js/jquery.js", false, $pp_theme_version);


	wp_enqueue_script("jQuery_UI_js", get_bloginfo( 'stylesheet_directory' )."/js/jquery-ui.js", false, $pp_theme_version);
	wp_enqueue_script("fancybox_js", get_bloginfo( 'stylesheet_directory' )."/js/fancybox/jquery.fancybox-1.3.0.js", false, $pp_theme_version);
	wp_enqueue_script("jQuery_easing", get_bloginfo( 'stylesheet_directory' )."/js/jquery.easing.js", false, $pp_theme_version);
	wp_enqueue_script("custom_js", get_bloginfo( 'stylesheet_directory' )."/js/custom.js", false, $pp_theme_version);


	//REMED because not used
	//wp_enqueue_script("jQuery_nivo", get_bloginfo( 'stylesheet_directory' )."/js/jquery.nivo.slider.js", false, $pp_theme_version);

	//REMED because not used
	//wp_enqueue_script("jQuery_anything_slider", get_bloginfo( 'stylesheet_directory' )."/js/anythingSlider.js", false, $pp_theme_version);

	/**
	*	Check Google Maps key
	**/
	$pp_gm_key = get_option('pp_gm_key');

	if(!empty($pp_gm_key))
	{
		wp_enqueue_script("jQuery_gmap", get_bloginfo( 'stylesheet_directory' )."/js/gmap.js", false, $pp_theme_version);
	}

	wp_enqueue_script("hint", get_bloginfo( 'stylesheet_directory' )."/js/hint.js", false, $pp_theme_version);

	//I don't think we need this because all our forms are done by Contact Form
	//wp_enqueue_script("jQuery_validate", get_bloginfo( 'stylesheet_directory' )."/js/jquery.validate.js", false, $pp_theme_version);

	//Cufon is for displaying custom fonts. We use Google Fonts for that
	//wp_enqueue_script("jQuery_cufon", get_bloginfo( 'stylesheet_directory' )."/js/cufon.js", false, $pp_theme_version);

	//Used by Fancybox, but I'm not sure we need fancybox.
	wp_enqueue_script("browser_js", get_bloginfo( 'stylesheet_directory' )."/js/browser.js", false, $pp_theme_version);

	//Not used. This is for html5 video player
	//wp_enqueue_script("video_js", get_bloginfo( 'stylesheet_directory' )."/js/video.js", false, $pp_theme_version);

	//Not used
	//wp_enqueue_script("jquery.tipsy", get_bloginfo( 'stylesheet_directory' )."/js/jquery.tipsy.js", false, $pp_theme_version);

	// AddThis code
    if (! (is_front_page() || is_page("home") || is_page("home2")) ) // don't add if home page. we don't need it there.
	    wp_enqueue_script("addthis_js", "http://s7.addthis.com/js/250/addthis_widget.js#pubid=ra-4f4bfb024f7f9f18&async=1", false, NULL);

	// Download redirect (never use CloudFront for this because each request is dynamic!)
	if (isset($_REQUEST["force_user_agent"]))
		$force_user_agent = "?force_user_agent=" . $_REQUEST["force_user_agent"];
	else
		$force_user_agent = '';
	wp_enqueue_script("download_js", "/download.js.php" . $force_user_agent, false, NULL);




	/* Always have wp_head() just before the closing </head>
	 * tag of your theme, or you will break many plugins, which
	 * generally use this hook to add elements to <head> such
	 * as styles, scripts, and meta tags.
	 */
	wp_head();


if ( function_exists( 'yoast_analytics' ) ) {
	yoast_analytics();
}

if(is_front_page() || is_page("home") || is_page("home2"))
{
?>
<script>
$j(document).ready(function(){
	var homeLink = $j('#menu_wrapper .menu-main-menu-container .nav li a[title=Home]');
	if (homeLink)
		homeLink.parent('li').addClass('current-menu-item');
});
</script>
<?php
} // is_front_page
?>

</head>

<?php

/**
*	Get Current page object
**/
$page = get_page($post->ID);


/**
*	Get current page id
**/
$current_page_id = '';

if(isset($page->ID))
{
    $current_page_id = $page->ID;
}

if (is_page("home") || is_page("home2"))
	$add_body_class = "home";
else
	$add_body_class = "";

?>

<body <?php body_class($add_body_class); ?>>


	<!-- Begin template wrapper -->
	<div id="wrapper">

		<!-- Begin header -->
		<div id="header_wrapper">
			<div id="top_bar">
				<div class="logo">
					<!-- Begin logo -->

					<?php
						//get custom logo
						$pp_logo = get_option('pp_logo');

						if(empty($pp_logo))
						{
							$pp_logo = get_bloginfo( 'stylesheet_directory' ).'/images/logo.png';
						}
						else
						{
							$pp_logo = get_bloginfo( 'stylesheet_directory' ).'/cache/'.$pp_logo;
						}

					?>

					<a id="custom_logo" href="<?php bloginfo( 'url' ); ?>"><img src="<?php echo $pp_logo?>" alt=""/></a>

					<!-- End logo -->
				</div>

				<!-- Begin main nav -->
				<div id="menu_wrapper">

				    <?php
				    			//Get page nav
				    			wp_nav_menu(
				    					array(
				    						'menu_id'			=> 'main_menu',
				    						'menu_class'		=> 'nav',
				    						'theme_location' 	=> 'primary-menu',
				    					)
				    			);
				    ?>

				</div>
				<!-- End main nav -->

			</div> <!-- End top_bar -->

		<br class="clear"/>


	<!-- wrapper div ends in footer -->
