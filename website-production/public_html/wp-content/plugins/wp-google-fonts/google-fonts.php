<?php
/* 
Plugin Name: WP Google Fonts
Plugin URI: http://adrian3.com/projects/wordpress-plugins/wordpress-google-fonts-plugin/
Version: v2.6
Author: <a href="http://adrian3.com/">Adrian3</a>
Description: The Wordpress Google Fonts Plugin makes it even easier to add and customize Google fonts on your site through Wordpress. 
Author: Adrian Hanft
Author URI: http://adrian3.com/projects/wordpress-plugins/
*/

/*  Copyright 2010-2011  Adrian Hanft

* Licensed under the Apache License, Version 2.0 (the "License")
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

// Pre-2.6 compatibility
if ( ! defined( 'WP_CONTENT_URL' ) )
      define( 'WP_CONTENT_URL', get_option( 'siteurl' ) . '/wp-content' );
if ( ! defined( 'WP_CONTENT_DIR' ) )
      define( 'WP_CONTENT_DIR', ABSPATH . 'wp-content' );
if ( ! defined( 'WP_PLUGIN_URL' ) )
      define( 'WP_PLUGIN_URL', WP_CONTENT_URL. '/plugins' );
if ( ! defined( 'WP_PLUGIN_DIR' ) )
      define( 'WP_PLUGIN_DIR', WP_CONTENT_DIR . '/plugins' );


if (!class_exists('googlefonts')) {
    class googlefonts {
        //This is where the class variables go, don't forget to use @var to tell what they're for
        /**
        * @var string The options string name for this plugin
        */
        var $optionsName = 'googlefonts_options';
        
        /**
        * @var string $localizationDomain Domain used for localization
        */
        var $localizationDomain = "googlefonts";
        
        /**
        * @var string $pluginurl The path to this plugin
        */ 
        var $thispluginurl = '';
        /**
        * @var string $pluginurlpath The path to this plugin
        */
        var $thispluginpath = '';
            
        /**
        * @var array $options Stores the options for this plugin
        */
        var $options = array();
        
        //Class Functions
        /**
        * PHP 4 Compatible Constructor
        */
        function googlefonts(){$this->__construct();}
        
        /**
        * PHP 5 Constructor
        */        
        function __construct(){
            //Language Setup
            $locale = get_locale();
            $mo = dirname(__FILE__) . "/languages/" . $this->localizationDomain . "-".$locale.".mo";
            load_textdomain($this->localizationDomain, $mo);

            //"Constants" setup
            $this->thispluginurl = PLUGIN_URL . '/' . dirname(plugin_basename(__FILE__)).'/';
            $this->thispluginpath = PLUGIN_PATH . '/' . dirname(plugin_basename(__FILE__)).'/';
            
            //Initialize the options
            //This is REQUIRED to initialize the options when the plugin is loaded!
            $this->getOptions();
            
            //Actions        
            add_action("admin_menu", array(&$this,"admin_menu_link"));
            add_action("wp_head", array(&$this,"googlefontsstart"));
            add_action("wp_head", array(&$this,"addgooglefontscss"));            

            /*
            add_action("wp_head", array(&$this,"add_css"));
            add_action('wp_print_scripts', array(&$this, 'add_js'));
            */
            
            //Filters
            /*
            add_filter('the_content', array(&$this, 'filter_content'), 0);
            */
        }
        
        
        
 function googlefontsstart() {

// check to see if site is uses https
$http = (!empty($_SERVER['HTTPS'])) ? "https" : "http";

echo '

<!-- Google Fonts -->
';




$googlefont1 = $this->options['googlefonts_font1'];
if ($googlefont1!='off' && $googlefont1) {
echo '<link href=\''.$http.'://fonts.googleapis.com/css?family='.$googlefont1.'\' rel=\'stylesheet\' type=\'text/css\' />';
}
$googlefont2 = $this->options['googlefonts_font2'];
if ($googlefont2!='off' && $googlefont2) {
echo '<link href=\''.$http.'://fonts.googleapis.com/css?family='.$googlefont2.'\' rel=\'stylesheet\' type=\'text/css\' />';
}
$googlefont3 = $this->options['googlefonts_font3'];
if ($googlefont3!='off' && $googlefont3) {
echo '<link href=\''.$http.'://fonts.googleapis.com/css?family='.$googlefont3.'\' rel=\'stylesheet\' type=\'text/css\' />';
}
$googlefont4 = $this->options['googlefonts_font4'];
if ($googlefont4!='off' && $googlefont4) {
echo '<link href=\''.$http.'://fonts.googleapis.com/css?family='.$googlefont4.'\' rel=\'stylesheet\' type=\'text/css\' />';
}
$googlefont5 = $this->options['googlefonts_font5'];
if ($googlefont5!='off' && $googlefont5) {
echo '<link href=\''.$http.'://fonts.googleapis.com/css?family='.$googlefont5.'\' rel=\'stylesheet\' type=\'text/css\' />';
}
$googlefont6 = $this->options['googlefonts_font6'];
if ($googlefont6!='off' && $googlefont6) {
echo '<link href=\''.$http.'://fonts.googleapis.com/css?family='.$googlefont16.'\' rel=\'stylesheet\' type=\'text/css\' />';
}



}


function addgooglefontscss() {
	$fullfontname1 = $this->options['googlefonts_font1'];
	$shortfontname1 = explode(":", $fullfontname1);
	$fullfontname2 = $this->options['googlefonts_font2'];
	$shortfontname2 = explode(":", $fullfontname2);
	$fullfontname3 = $this->options['googlefonts_font3'];
	$shortfontname3 = explode(":", $fullfontname3);
	$fullfontname4 = $this->options['googlefonts_font4'];
	$shortfontname4 = explode(":", $fullfontname4);
	$fullfontname5 = $this->options['googlefonts_font5'];
	$shortfontname5 = explode(":", $fullfontname5);
	$fullfontname6 = $this->options['googlefonts_font6'];
	$shortfontname6 = explode(":", $fullfontname6);

	
echo '
<style type="text/css" media="screen">
';

//Google Font #1 Styles:
if ($this->options['googlefont1_heading1'] == "checked") { echo 'h1 { font-family: \''; echo $shortfontname1[0];  echo '\', arial, serif; } 
'; }
if ($this->options['googlefont1_heading2'] == "checked") { echo 'h2 { font-family: \''; echo $shortfontname1[0]; echo '\', arial, serif; } 
'; }                                                                                         
if ($this->options['googlefont1_heading3'] == "checked") { echo 'h3 { font-family: \''; echo $shortfontname1[0]; echo '\', arial, serif; } 
'; }                                                                                         
if ($this->options['googlefont1_heading4'] == "checked") { echo 'h4 { font-family: \''; echo $shortfontname1[0]; echo '\', arial, serif; } 
'; }                                                                                         
if ($this->options['googlefont1_heading5'] == "checked") { echo 'h5 { font-family: \''; echo $shortfontname1[0]; echo '\', arial, serif; } 
'; }                                                                                         
if ($this->options['googlefont1_heading6'] == "checked") { echo 'h6 { font-family: \''; echo $shortfontname1[0]; echo '\', arial, serif; } 
'; }
if ($this->options['googlefont1_body'] == "checked") { echo 'body 				{ font-family: \''; echo $shortfontname1[0]; echo '\', arial, serif; } 
'; }
if ($this->options['googlefont1_p'] == "checked") { echo 'p 					{ font-family: \''; echo $shortfontname1[0]; echo '\', arial, serif; } 
'; }
if ($this->options['googlefont1_blockquote'] == "checked") { echo 'blockquote 	{ font-family: \''; echo $shortfontname1[0]; echo '\', arial, serif; } 
'; }
echo stripslashes($this->options['googlefont1_css']);

//Google Font #2 Styles:
if ($this->options['googlefont2_heading1'] == "checked") { echo 'h1 { font-family: \''; echo $shortfontname2[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont2_heading2'] == "checked") { echo 'h2 { font-family: \''; echo $shortfontname2[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont2_heading3'] == "checked") { echo 'h3 { font-family: \''; echo $shortfontname2[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont2_heading4'] == "checked") { echo 'h4 { font-family: \''; echo $shortfontname2[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont2_heading5'] == "checked") { echo 'h5 { font-family: \''; echo $shortfontname2[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont2_heading6'] == "checked") { echo 'h6 { font-family: \''; echo $shortfontname2[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont2_body'] == "checked") { echo 'body 				{ font-family: \''; echo $shortfontname2[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont2_p'] == "checked") { echo 'p 					{ font-family: \''; echo $shortfontname2[0]; echo '\', arial, serif; } 
'; }
if ($this->options['googlefont2_blockquote'] == "checked") { echo 'blockquote 	{ font-family: \''; echo $shortfontname2[0]; echo '\', arial, serif; } 
'; }
echo stripslashes($this->options['googlefont2_css']);

//Google Font #3 Styles:
if ($this->options['googlefont3_heading1'] == "checked") { echo 'h1 { font-family: \''; echo $shortfontname3[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont3_heading2'] == "checked") { echo 'h2 { font-family: \''; echo $shortfontname3[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont3_heading3'] == "checked") { echo 'h3 { font-family: \''; echo $shortfontname3[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont3_heading4'] == "checked") { echo 'h4 { font-family: \''; echo $shortfontname3[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont3_heading5'] == "checked") { echo 'h5 { font-family: \''; echo $shortfontname3[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont3_heading6'] == "checked") { echo 'h6 { font-family: \''; echo $shortfontname3[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont3_body'] == "checked") { echo 'body 				{ font-family: \''; echo $shortfontname3[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont3_p'] == "checked") { echo 'p 					{ font-family: \''; echo $shortfontname3[0]; echo '\', arial, serif; } 
'; }
if ($this->options['googlefont3_blockquote'] == "checked") { echo 'blockquote 	{ font-family: \''; echo $shortfontname3[0]; echo '\', arial, serif; } 
'; }
echo stripslashes($this->options['googlefont3_css']);

//Google Font #4 Styles:
if ($this->options['googlefont4_heading1'] == "checked") { echo 'h1 { font-family: \''; echo $shortfontname4[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont4_heading2'] == "checked") { echo 'h2 { font-family: \''; echo $shortfontname4[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont4_heading3'] == "checked") { echo 'h3 { font-family: \''; echo $shortfontname4[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont4_heading4'] == "checked") { echo 'h4 { font-family: \''; echo $shortfontname4[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont4_heading5'] == "checked") { echo 'h5 { font-family: \''; echo $shortfontname4[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont4_heading6'] == "checked") { echo 'h6 { font-family: \''; echo $shortfontname4[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont4_body'] == "checked") { echo 'body 				{ font-family: \''; echo $shortfontname4[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont4_p'] == "checked") { echo 'p 					{ font-family: \''; echo $shortfontname4[0]; echo '\', arial, serif; } 
'; }
if ($this->options['googlefont4_blockquote'] == "checked") { echo 'blockquote 	{ font-family: \''; echo $shortfontname4[0]; echo '\', arial, serif; } 
'; }
echo stripslashes($this->options['googlefont4_css']);

//Google Font #5 Styles:
if ($this->options['googlefont5_heading1'] == "checked") { echo 'h1 { font-family: \''; echo $shortfontname5[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont5_heading2'] == "checked") { echo 'h2 { font-family: \''; echo $shortfontname5[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont5_heading3'] == "checked") { echo 'h3 { font-family: \''; echo $shortfontname5[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont5_heading4'] == "checked") { echo 'h4 { font-family: \''; echo $shortfontname5[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont5_heading5'] == "checked") { echo 'h5 { font-family: \''; echo $shortfontname5[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont5_heading6'] == "checked") { echo 'h6 { font-family: \''; echo $shortfontname5[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont5_body'] == "checked") { echo 'body 				{ font-family: \''; echo $shortfontname5[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont5_p'] == "checked") { echo 'p 					{ font-family: \''; echo $shortfontname5[0]; echo '\', arial, serif; } 
'; }
if ($this->options['googlefont5_blockquote'] == "checked") { echo 'blockquote 	{ font-family: \''; echo $shortfontname5[0]; echo '\', arial, serif; } 
'; }
echo stripslashes($this->options['googlefont5_css']);

//Google Font #6 Styles:
if ($this->options['googlefont6_heading1'] == "checked") { echo 'h1 { font-family: \''; echo $shortfontname6[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont6_heading2'] == "checked") { echo 'h2 { font-family: \''; echo $shortfontname6[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont6_heading3'] == "checked") { echo 'h3 { font-family: \''; echo $shortfontname6[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont6_heading4'] == "checked") { echo 'h4 { font-family: \''; echo $shortfontname6[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont6_heading5'] == "checked") { echo 'h5 { font-family: \''; echo $shortfontname6[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont6_heading6'] == "checked") { echo 'h6 { font-family: \''; echo $shortfontname6[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont6_body'] == "checked") { echo 'body 				{ font-family: \''; echo $shortfontname6[0]; echo '\', arial, serif; } 
'; }                          
if ($this->options['googlefont6_p'] == "checked") { echo 'p 					{ font-family: \''; echo $shortfontname6[0]; echo '\', arial, serif; } 
'; }
if ($this->options['googlefont6_blockquote'] == "checked") { echo 'blockquote 	{ font-family: \''; echo $shortfontname6[0]; echo '\', arial, serif; } 
'; }
echo stripslashes($this->options['googlefont6_css']);


echo '
</style>
<!-- fonts delivered by Wordpress Google Fonts, a plugin by Adrian3.com -->

';
}








       
        /**
        * Retrieves the plugin options from the database.
        * @return array
        */
        function getOptions() {
            //Don't forget to set up the default options
            if (!$theOptions = get_option($this->optionsName)) {
                $theOptions = array(
	

	
	'googlefonts1_on_off'=>'unchecked',
	'googlefonts2_on_off'=>'unchecked',
	'googlefonts3_on_off'=>'unchecked',
	'googlefonts4_on_off'=>'unchecked',
	'googlefonts5_on_off'=>'unchecked',
	'googlefonts6_on_off'=>'unchecked',	

	'googlefont1_css_on_off'=>'unchecked',
	'googlefont1_css'=>' ',
	'googlefont1_heading1'=>'unchecked',
	'googlefont1_heading2'=>'unchecked',
	'googlefont1_heading3'=>'unchecked',
	'googlefont1_heading4'=>'unchecked',
	'googlefont1_heading5'=>'unchecked',
	'googlefont1_heading6'=>'unchecked',
	'googlefont1_body'=>'unchecked',
	'googlefont1_blockquote'=>'unchecked',
	'googlefont1_p'=>'unchecked',
	'googlefont1_li'=>'unchecked',

	'googlefont2_css_on_off'=>'unchecked',
	'googlefont2_css'=>' ',
	'googlefont2_heading1'=>'unchecked',
	'googlefont2_heading2'=>'unchecked',
	'googlefont2_heading3'=>'unchecked',
	'googlefont2_heading4'=>'unchecked',
	'googlefont2_heading5'=>'unchecked',
	'googlefont2_heading6'=>'unchecked',
	'googlefont2_body'=>'unchecked',
	'googlefont2_blockquote'=>'unchecked',
	'googlefont2_p'=>'unchecked',
	'googlefont2_li'=>'unchecked',	

	'googlefont3_css_on_off'=>'unchecked',
	'googlefont3_css'=>' ',
	'googlefont3_heading1'=>'unchecked',
	'googlefont3_heading2'=>'unchecked',
	'googlefont3_heading3'=>'unchecked',
	'googlefont3_heading4'=>'unchecked',
	'googlefont3_heading5'=>'unchecked',
	'googlefont3_heading6'=>'unchecked',
	'googlefont3_body'=>'unchecked',
	'googlefont3_blockquote'=>'unchecked',
	'googlefont3_p'=>'unchecked',
	'googlefont3_li'=>'unchecked',
	
	'googlefont4_css_on_off'=>'unchecked',
	'googlefont4_css'=>' ',
	'googlefont4_heading1'=>'unchecked',
	'googlefont4_heading2'=>'unchecked',
	'googlefont4_heading3'=>'unchecked',
	'googlefont4_heading4'=>'unchecked',
	'googlefont4_heading5'=>'unchecked',
	'googlefont4_heading6'=>'unchecked',
	'googlefont4_body'=>'unchecked',
	'googlefont4_blockquote'=>'unchecked',
	'googlefont4_p'=>'unchecked',
	'googlefont4_li'=>'unchecked',
	
	'googlefont5_css_on_off'=>'unchecked',
	'googlefont5_css'=>' ',
	'googlefont5_heading1'=>'unchecked',
	'googlefont5_heading2'=>'unchecked',
	'googlefont5_heading3'=>'unchecked',
	'googlefont5_heading4'=>'unchecked',
	'googlefont5_heading5'=>'unchecked',
	'googlefont5_heading6'=>'unchecked',
	'googlefont5_body'=>'unchecked',
	'googlefont5_blockquote'=>'unchecked',
	'googlefont5_p'=>'unchecked',
	'googlefont5_li'=>'unchecked',

	'googlefont6_css_on_off'=>'unchecked',
	'googlefont6_css'=>' ',
	'googlefont6_heading1'=>'unchecked',
	'googlefont6_heading2'=>'unchecked',
	'googlefont6_heading3'=>'unchecked',
	'googlefont6_heading4'=>'unchecked',
	'googlefont6_heading5'=>'unchecked',
	'googlefont6_heading6'=>'unchecked',
	'googlefont6_body'=>'unchecked',
	'googlefont6_blockquote'=>'unchecked',
	'googlefont6_p'=>'unchecked',
	'googlefont6_li'=>'unchecked',

	'googlefonts_on_off'=>'off'	
			);
                update_option($this->optionsName, $theOptions);
            }
            $this->options = $theOptions;
            
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //There is no return here, because you should use the $this->options variable!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
        /**
        * Saves the admin options to the database.
        */
        function saveAdminOptions(){
            return update_option($this->optionsName, $this->options);
        }
        
        /**
        * @desc Adds the options subpanel
        */
        function admin_menu_link() {
            //If you change this from add_options_page, MAKE SURE you change the filter_plugin_actions function (below) to
            //reflect the page filename (ie - options-general.php) of the page your plugin is under!
            add_options_page('Google Fonts', 'Google Fonts', 10, basename(__FILE__), array(&$this,'admin_options_page'));
            add_filter( 'plugin_action_links_' . plugin_basename(__FILE__), array(&$this, 'filter_plugin_actions'), 10, 2 );
        }
        
        /**
        * @desc Adds the Settings link to the plugin activate/deactivate page
        */
        function filter_plugin_actions($links, $file) {
           //If your plugin is under a different top-level menu than Settiongs (IE - you changed the function above to something other than add_options_page)
           //Then you're going to want to change options-general.php below to the name of your top-level page
           $settings_link = '<a href="options-general.php?page=' . basename(__FILE__) . '">' . __('Settings') . '</a>';
           array_unshift( $links, $settings_link ); // before other links

           return $links;
        }
        
        /**
        * Adds settings/options page
        */
        function admin_options_page() { 
            if($_POST['googlefonts_save']){
                if (! wp_verify_nonce($_POST['_wpnonce'], 'googlefonts-update-options') ) die('Whoops! There was a problem with the data you posted. Please go back and try again.'); 

$this->options['googlefonts_on_off'] = $_POST['googlefonts_on_off'];

$this->options['googlefonts1_on_off'] = $_POST['googlefonts1_on_off'];
$this->options['googlefonts2_on_off'] = $_POST['googlefonts2_on_off'];
$this->options['googlefonts3_on_off'] = $_POST['googlefonts3_on_off'];
$this->options['googlefonts4_on_off'] = $_POST['googlefonts4_on_off'];
$this->options['googlefonts5_on_off'] = $_POST['googlefonts5_on_off'];
$this->options['googlefonts6_on_off'] = $_POST['googlefonts6_on_off'];

$this->options['googlefonts_font1'] = $_POST['googlefonts_font1'];
$this->options['googlefonts_font2'] = $_POST['googlefonts_font2'];
$this->options['googlefonts_font3'] = $_POST['googlefonts_font3'];
$this->options['googlefonts_font4'] = $_POST['googlefonts_font4'];
$this->options['googlefonts_font5'] = $_POST['googlefonts_font5'];
$this->options['googlefonts_font6'] = $_POST['googlefonts_font6'];


$this->options['googlefont1_heading1'] = $_POST['googlefont1_heading1'];
$this->options['googlefont1_heading2'] = $_POST['googlefont1_heading2'];
$this->options['googlefont1_heading3'] = $_POST['googlefont1_heading3'];
$this->options['googlefont1_heading4'] = $_POST['googlefont1_heading4'];
$this->options['googlefont1_heading5'] = $_POST['googlefont1_heading5'];
$this->options['googlefont1_heading6'] = $_POST['googlefont1_heading6'];
$this->options['googlefont1_body'] = $_POST['googlefont1_body'];
$this->options['googlefont1_blockquote'] = $_POST['googlefont1_blockquote'];
$this->options['googlefont1_p'] = $_POST['googlefont1_p'];
$this->options['googlefont1_li'] = $_POST['googlefont1_li'];
$this->options['googlefont1_css_on_off'] = $_POST['googlefont1_css_on_off'];
$this->options['googlefont1_css'] = $_POST['googlefont1_css'];

$this->options['googlefont2_heading1'] = $_POST['googlefont2_heading1'];
$this->options['googlefont2_heading2'] = $_POST['googlefont2_heading2'];
$this->options['googlefont2_heading3'] = $_POST['googlefont2_heading3'];
$this->options['googlefont2_heading4'] = $_POST['googlefont2_heading4'];
$this->options['googlefont2_heading5'] = $_POST['googlefont2_heading5'];
$this->options['googlefont2_heading6'] = $_POST['googlefont2_heading6'];
$this->options['googlefont2_body'] = $_POST['googlefont2_body'];
$this->options['googlefont2_blockquote'] = $_POST['googlefont2_blockquote'];
$this->options['googlefont2_p'] = $_POST['googlefont2_p'];
$this->options['googlefont2_li'] = $_POST['googlefont2_li'];
$this->options['googlefont2_css_on_off'] = $_POST['googlefont2_css_on_off'];
$this->options['googlefont2_css'] = $_POST['googlefont2_css'];

$this->options['googlefont3_heading1'] = $_POST['googlefont3_heading1'];
$this->options['googlefont3_heading2'] = $_POST['googlefont3_heading2'];
$this->options['googlefont3_heading3'] = $_POST['googlefont3_heading3'];
$this->options['googlefont3_heading4'] = $_POST['googlefont3_heading4'];
$this->options['googlefont3_heading5'] = $_POST['googlefont3_heading5'];
$this->options['googlefont3_heading6'] = $_POST['googlefont3_heading6'];
$this->options['googlefont3_body'] = $_POST['googlefont3_body'];
$this->options['googlefont3_blockquote'] = $_POST['googlefont3_blockquote'];
$this->options['googlefont3_p'] = $_POST['googlefont3_p'];
$this->options['googlefont3_li'] = $_POST['googlefont3_li'];
$this->options['googlefont3_css_on_off'] = $_POST['googlefont3_css_on_off'];
$this->options['googlefont3_css'] = $_POST['googlefont3_css'];

$this->options['googlefont4_heading1'] = $_POST['googlefont4_heading1'];
$this->options['googlefont4_heading2'] = $_POST['googlefont4_heading2'];
$this->options['googlefont4_heading3'] = $_POST['googlefont4_heading3'];
$this->options['googlefont4_heading4'] = $_POST['googlefont4_heading4'];
$this->options['googlefont4_heading5'] = $_POST['googlefont4_heading5'];
$this->options['googlefont4_heading6'] = $_POST['googlefont4_heading6'];
$this->options['googlefont4_body'] = $_POST['googlefont4_body'];
$this->options['googlefont4_blockquote'] = $_POST['googlefont4_blockquote'];
$this->options['googlefont4_p'] = $_POST['googlefont4_p'];
$this->options['googlefont4_li'] = $_POST['googlefont4_li'];
$this->options['googlefont4_css_on_off'] = $_POST['googlefont4_css_on_off'];
$this->options['googlefont4_css'] = $_POST['googlefont4_css'];

$this->options['googlefont5_heading1'] = $_POST['googlefont5_heading1'];
$this->options['googlefont5_heading2'] = $_POST['googlefont5_heading2'];
$this->options['googlefont5_heading3'] = $_POST['googlefont5_heading3'];
$this->options['googlefont5_heading4'] = $_POST['googlefont5_heading4'];
$this->options['googlefont5_heading5'] = $_POST['googlefont5_heading5'];
$this->options['googlefont5_heading6'] = $_POST['googlefont5_heading6'];
$this->options['googlefont5_body'] = $_POST['googlefont5_body'];
$this->options['googlefont5_blockquote'] = $_POST['googlefont5_blockquote'];
$this->options['googlefont5_p'] = $_POST['googlefont5_p'];
$this->options['googlefont5_li'] = $_POST['googlefont5_li'];
$this->options['googlefont5_css_on_off'] = $_POST['googlefont5_css_on_off'];
$this->options['googlefont5_css'] = $_POST['googlefont5_css'];

$this->options['googlefont6_heading1'] = $_POST['googlefont6_heading1'];
$this->options['googlefont6_heading2'] = $_POST['googlefont6_heading2'];
$this->options['googlefont6_heading3'] = $_POST['googlefont6_heading3'];
$this->options['googlefont6_heading4'] = $_POST['googlefont6_heading4'];
$this->options['googlefont6_heading5'] = $_POST['googlefont6_heading5'];
$this->options['googlefont6_heading6'] = $_POST['googlefont6_heading6'];
$this->options['googlefont6_body'] = $_POST['googlefont6_body'];
$this->options['googlefont6_blockquote'] = $_POST['googlefont6_blockquote'];
$this->options['googlefont6_p'] = $_POST['googlefont6_p'];
$this->options['googlefont6_li'] = $_POST['googlefont6_li'];
$this->options['googlefont6_css_on_off'] = $_POST['googlefont6_css_on_off'];
$this->options['googlefont6_css'] = $_POST['googlefont6_css'];
                                        
                                        
                $this->saveAdminOptions();
                
                echo '<div class="updated"><p>Success! Your changes were sucessfully saved!</p></div>';
            }
?>                                   
                <div class="wrap">
<table width="650" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td>

<h1><?php _e('Google Font Control Panel', 'googlefonts'); ?></h1>
<p><img src="<?php 	echo get_bloginfo('wpurl'); 
	echo '/wp-content/plugins/wp-google-fonts/images/google-fonts-logo.jpg'; ?>" align="right" /><?php _e('This control panel gives you the ability to control how your Google Fonts fonts are displayed. For more information about this plugin, please visit the', 'googlefonts'); ?> <a href="http://adrian3.com/projects/wordpress-plugins/wordpress-google-fonts-plugin/" title="Google Fonts plugin page"><?php _e('Google Fonts plugin page', 'googlefonts'); ?></a>. <?php _e('Thanks for using Google Fonts, and I hope you like this plugin.', 'googlefonts'); ?> <a href="http://adrian3.com/" title="-Adrian 3">-Adrian3</a></p>
 




                <form method="post" id="googlefonts_options">
                <?php wp_nonce_field('googlefonts-update-options'); ?>

<hr />

<!-- most recent font added was Spicy Rice December 29, 2011-->
<?php
	function listgooglefontoptions() { echo '
<option value="Abel">Abel </option>
<option value="Abril Fatface">Abril Fatface</option>
<option value="Aclonica">Aclonica</option>
<option value="Acme">Acme</option>
<option value="Actor">Actor </option>
<option value="Adamina">Adamina</option>
<option value="Aguafina Script">Aguafina Script</option>
<option value="Aladin">Aladin</option>
<option value="Aldrich">Aldrich </option>
<option value="Alex Brush">Alex Brush</option>
<option value="Alfa Slab One">Alfa Slab One</option>
<option value="Alice">Alice </option>
<option value="Alike Angular">Alike Angular</option>
<option value="Alike">Alike </option>
<option value="Allan">Allan</option>
<option value="Allerta Stencil">Allerta Stencil</option>
<option value="Allerta">Allerta</option>
<option value="Almendra SC">Almendra SC</option>
<option value="Almendra">Almendra</option>
<option value="Almendra:400,bold">Almendra:400,bold</option>
<option value="Amaranth">Amaranth</option>
<option value="Amatic SC">Amatic SC</option>
<option value="Amatic SC:400,700">Amatic SC:400,700</option>
<option value="Andada">Andada</option>
<option value="Andika">Andika </option>
<option value="Annie Use Your Telescope">Annie Use Your Telescope</option>
<option value="Anonymous Pro">Anonymous Pro</option>
<option value="Anonymous Pro:regular,italic,bold,bolditalic">Anonymous Pro (plus italic, bold, and bold italic)</option>
<option value="Antic">Antic</option>
<option value="Anton">Anton</option>
<option value="Arapey">Arapey</option>
<option value="Arapey:400,400italic">Arapey:400,400italic</option>
<option value="Arbutus">Arbutus</option>
<option value="Architects Daughter">Architects Daughter</option>
<option value="Arimo">Arimo</option>
<option value="Arimo:regular,italic,bold,bolditalic">Arimo (plus italic, bold, and bold italic)</option>
<option value="Arizonia">Arizonia</option>
<option value="Armata">Armata</option>
<option value="Artifika">Artifika</option>
<option value="Arvo">Arvo</option>
<option value="Arvo:regular,italic,bold,bolditalic">Arvo (plus italic, bold, and bold italic)</option>
<option value="Asset">Asset</option>
<option value="Astloch">Astloch</option>
<option value="Astloch:regular,bold">Astloch (plus bold)</option>
<option value="Asul">Asul</option>
<option value="Asul:400,bold">Asul:400,bold</option>
<option value="Atomic Age">Atomic Age</option>
<option value="Aubrey">Aubrey </option>
<option value="Bad Script">Bad Script</option>
<option value="Balthazar">Balthazar</option>
<option value="Bangers">Bangers</option>
<option value="Basic">Basic</option>
<option value="Baumans">Baumans</option>
<option value="Belgrano">Belgrano</option>
<option value="Bentham">Bentham</option>
<option value="Bevan">Bevan</option>
<option value="Bigshot One">Bigshot One</option>
<option value="Bilbo Swash Caps">Bilbo Swash Caps</option>
<option value="Bilbo">Bilbo</option>
<option value="Bitter">Bitter</option>
<option value="Bitter:400,400italic,700">Bitter:400,400italic,700</option>
<option value="Black Ops One">Black Ops One </option>
<option value="Bonbon">Bonbon</option>
<option value="Boogaloo">Boogaloo</option>
<option value="Bowlby One SC">Bowlby One SC</option>
<option value="Bowlby One">Bowlby One</option>
<option value="Brawler">Brawler </option>
<option value="Bree Serif">Bree Serif</option>
<option value="Bubblegum Sans">Bubblegum Sans</option>
<option value="Buda:light">Buda</option>
<option value="Buenard">Buenard</option>
<option value="Buenard:400,bold">Buenard:400,bold</option>
<option value="Butcherman">Butcherman</option>
<option value="Cabin Condensed">Cabin Condensed</option>
<option value="Cabin Condensed:400,500,600,700">Cabin Condensed:400,500,600,700</option>
<option value="Cabin Sketch">Cabin Sketch</option>
<option value="Cabin Sketch:bold">Cabin Sketch</option>
<option value="Cabin Sketch:regular,bold">Cabin Sketch:regular,bold</option>
<option value="Cabin">Cabin</option>
<option value="Cabin:regular,500,600,bold">Cabin (plus 500, 600, and bold)</option>
<option value="Caesar Dressing">Caesar Dressing</option>
<option value="Cagliostro">Cagliostro</option>
<option value="Calligraffitti">Calligraffitti</option>
<option value="Cambo">Cambo</option>
<option value="Candal">Candal</option>
<option value="Cantarell">Cantarell</option>
<option value="Cantarell:regular,italic,bold,bolditalic">Cantarell (plus italic, bold, and bold italic)</option>
<option value="Cardo">Cardo</option>
<option value="Carme">Carme </option>
<option value="Carter One">Carter One</option>
<option value="Caudex">Caudex</option>
<option value="Caudex:regular,italic,bold,bolditalic">Caudex (plus italic, bold, and bold italic)</option>
<option value="Cedarville Cursive">Cedarville Cursive</option>
<option value="Ceviche One">Ceviche One</option>
<option value="Changa One">Changa One</option>
<option value="Chango">Chango</option>
<option value="Cherry Cream Soda">Cherry Cream Soda</option>
<option value="Chewy">Chewy</option>
<option value="Chicle">Chicle</option>
<option value="Chivo">Chivo</option>
<option value="Chivo:400,900">Chivo (plus bold)</option>
<option value="Coda">Coda</option>
<option value="Coda">Coda</option>
<option value="Coda:400,800">Coda:400,800</option>
<option value="Comfortaa">Comfortaa </option>
<option value="Comfortaa:300,400,700">Comfortaa (plus book and bold) </option>
<option value="Coming Soon">Coming Soon</option>
<option value="Concert One">Concert One</option>
<option value="Contrail One">Contrail One</option>
<option value="Convergence">Convergence</option>
<option value="Cookie">Cookie</option>
<option value="Copse">Copse</option>
<option value="Corben">Corben</option>
<option value="Corben:400,bold">Corben:400,bold</option>
<option value="Corben:bold">Corben</option>
<option value="Cousine">Cousine</option>
<option value="Cousine:regular,italic,bold,bolditalic">Cousine (plus italic, bold, and bold italic)</option>
<option value="Coustard">Coustard </option>
<option value="Coustard:400,900">Coustard (plus ultra bold) </option>
<option value="Covered By Your Grace">Covered By Your Grace</option>
<option value="Crafty Girls">Crafty Girls</option>
<option value="Creepster">Creepster</option>
<option value="Crete Round">Crete Round</option>
<option value="Crete Round:400,400italic">Crete Round:400,400italic</option>
<option value="Crimson Text">Crimson Text</option>
<option value="Crushed">Crushed</option>
<option value="Cuprum">Cuprum</option>
<option value="Damion">Damion</option>
<option value="Dancing Script">Dancing Script</option>
<option value="Dawning of a New Day">Dawning of a New Day</option>
<option value="Days One">Days One </option>
<option value="Delius Swash Caps">Delius Swash Caps </option>
<option value="Delius Unicase">Delius Unicase </option>
<option value="Delius">Delius </option>
<option value="Devonshire">Devonshire</option>
<option value="Didact Gothic">Didact Gothic</option>
<option value="Dorsa">Dorsa</option>
<option value="Dr Sugiyama">Dr Sugiyama</option>
<option value="Droid Sans Mono">Droid Sans Mono</option>
<option value="Droid Sans">Droid Sans</option>
<option value="Droid Sans:regular,bold">Droid Sans (plus bold)</option>
<option value="Droid Serif">Droid Serif</option>
<option value="Droid Serif:regular,italic,bold,bolditalic">Droid Serif (plus italic, bold, and bold italic)</option>
<option value="Duru Sans">Duru Sans</option>
<option value="Dynalight">Dynalight</option>
<option value="EB Garamond">EB Garamond</option>
<option value="Eater">Eater</option>
<option value="Electrolize">Electrolize</option>
<option value="Engagement">Engagement</option>
<option value="Enriqueta">Enriqueta</option>
<option value="Enriqueta:400,700">Enriqueta:400,700</option>
<option value="Expletus Sans">Expletus Sans</option>
<option value="Expletus Sans:regular,500,600,bold">Expletus Sans (plus 500, 600, and bold)</option>
<option value="Fanwood Text">Fanwood Text</option>
<option value="Fanwood Text:400,400italic">Fanwood Text (plus italic)</option>
<option value="Fascinate Inline">Fascinate Inline</option>
<option value="Fascinate">Fascinate</option>
<option value="Federant">Federant</option>
<option value="Federo">Federo </option>
<option value="Fjord One">Fjord One</option>
<option value="Flamenco">Flamenco</option>
<option value="Flamenco:300">Flamenco:300</option>
<option value="Flavors">Flavors</option>
<option value="Fondamento">Fondamento</option>
<option value="Fondamento:400,400italic">Fondamento:400,400italic</option>
<option value="Fontdiner Swanky">Fontdiner Swanky</option>
<option value="Forum">Forum</option>
<option value="Francois One">Francois One</option>
<option value="Fresca">Fresca</option>
<option value="Frijole">Frijole</option>
<option value="Fugaz One">Fugaz One</option>
<option value="Galdeano">Galdeano</option>
<option value="Gentium Basic">Gentium Basic </option>
<option value="Gentium Basic:400,700,400italic,700italic">Gentium Basic (plus bold and italic) </option>
<option value="Gentium Book Basic">Gentium Book Basic </option>
<option value="Gentium Book Basic:400,400italic,700,700italic">Gentium Book Basic (plus bold and italic) </option>
<option value="Geo">Geo</option>
<option value="Geostar Fill">Geostar Fill </option>
<option value="Geostar">Geostar </option>
<option value="Give You Glory">Give You Glory</option>
<option value="Gloria Hallelujah">Gloria Hallelujah </option>
<option value="Goblin One">Goblin One</option>
<option value="Gochi Hand">Gochi Hand</option>
<option value="Gochi Hand:400">Gochi Hand:400</option>
<option value="Goudy Bookletter 1911">Goudy Bookletter 1911</option>
<option value="Gravitas One">Gravitas One</option>
<option value="Gruppo">Gruppo</option>
<option value="Habibi">Habibi</option>
<option value="Hammersmith One">Hammersmith One</option>
<option value="Handlee">Handlee</option>
<option value="Herr Von Muellerhoff">Herr Von Muellerhoff</option>
<option value="Holtwood One SC">Holtwood One SC</option>
<option value="Homemade Apple">Homemade Apple</option>
<option value="IM Fell DW Pica SC">IM Fell DW Pica SC</option>
<option value="IM Fell DW Pica">IM Fell DW Pica</option>
<option value="IM Fell DW Pica:regular,italic">IM Fell DW Pica (plus italic)</option>
<option value="IM Fell Double Pica SC">IM Fell Double Pica SC</option>
<option value="IM Fell Double Pica">IM Fell Double Pica</option>
<option value="IM Fell Double Pica:regular,italic">IM Fell Double Pica (plus italic)</option>
<option value="IM Fell English SC">IM Fell English SC</option>
<option value="IM Fell English">IM Fell English</option>
<option value="IM Fell English:regular,italic">IM Fell English (plus italic)</option>
<option value="IM Fell French Canon SC">IM Fell French Canon SC</option>
<option value="IM Fell French Canon">IM Fell French Canon</option>
<option value="IM Fell French Canon:regular,italic">IM Fell French Canon (plus italic)</option>
<option value="IM Fell Great Primer SC">IM Fell Great Primer SC</option>
<option value="IM Fell Great Primer">IM Fell Great Primer</option>
<option value="IM Fell Great Primer:regular,italic">IM Fell Great Primer (plus italic)</option>
<option value="Iceland">Iceland</option>
<option value="Inconsolata">Inconsolata</option>
<option value="Inder">Inder</option>
<option value="Indie Flower">Indie Flower</option>
<option value="Irish Grover">Irish Grover</option>
<option value="Irish Growler">Irish Growler</option>
<option value="Istok Web">Istok Web</option>
<option value="Istok Web:400,700,400italic,700italic">Istok Web (plus italic, bold, and bold italic)</option>
<option value="Italianno">Italianno</option>
<option value="Jockey One">Jockey One</option>
<option value="Josefin Sans">Josefin Sans Regular 400</option>
<option value="Josefin Sans:100">Josefin Sans 100</option>
<option value="Josefin Sans:100,100italic">Josefin Sans 100 (plus italic)</option>
<option value="Josefin Sans:600">Josefin Sans 600</option>
<option value="Josefin Sans:600,600italic">Josefin Sans 600 (plus italic)</option>
<option value="Josefin Sans:bold">Josefin Sans Bold 700</option>
<option value="Josefin Sans:bold,bolditalic">Josefin Sans Bold 700 (plus italic)</option>
<option value="Josefin Sans:light">Josefin Sans Light 300</option>
<option value="Josefin Sans:light,lightitalic">Josefin Sans Light 300 (plus italic)</option>
<option value="Josefin Sans:regular,regularitalic">Josefin Sans Regular 400 (plus italic)</option>
<option value="Josefin Slab">Josefin Slab Regular 400</option>
<option value="Josefin Slab:100">Josefin Slab 100</option>
<option value="Josefin Slab:100,100italic">Josefin Slab 100 (plus italic)</option>
<option value="Josefin Slab:600">Josefin Slab 600</option>
<option value="Josefin Slab:600,600italic">Josefin Slab 600 (plus italic)</option>
<option value="Josefin Slab:bold">Josefin Slab Bold 700</option>
<option value="Josefin Slab:bold,bolditalic">Josefin Slab Bold 700 (plus italic)</option>
<option value="Josefin Slab:light">Josefin Slab Light 300</option>
<option value="Josefin Slab:light,lightitalic">Josefin Slab Light 300 (plus italic)</option>
<option value="Josefin Slab:regular,regularitalic">Josefin Slab Regular 400 (plus italic)</option>
<option value="Judson">Judson</option>
<option value="Judson:regular,regularitalic,bold">Judson (plus bold)</option>
<option value="Julee">Julee</option>
<option value="Jura"> Jura Regular</option>
<option value="Jura:500"> Jura 500</option>
<option value="Jura:600"> Jura 600</option>
<option value="Jura:light"> Jura Light</option>
<option value="Just Another Hand">Just Another Hand</option>
<option value="Just Me Again Down Here">Just Me Again Down Here</option>
<option value="Kameron">Kameron</option>
<option value="Kameron:400,700">Kameron (plus bold)</option>
<option value="Kelly Slab">Kelly Slab </option>
<option value="Kenia">Kenia</option>
<option value="Knewave">Knewave</option>
<option value="Kranky">Kranky</option>
<option value="Kreon">Kreon</option>
<option value="Kreon:light,regular,bold">Kreon (plus light and bold)</option>
<option value="Kristi">Kristi</option>
<option value="La Belle Aurore">La Belle Aurore</option>
<option value="Lancelot">Lancelot</option>
<option value="Lato:100">Lato 100</option>
<option value="Lato:100,100italic">Lato 100 (plus italic)</option>
<option value="Lato:900">Lato 900</option>
<option value="Lato:900,900italic">Lato 900 (plus italic)</option>
<option value="Lato:bold">Lato Bold 700</option>
<option value="Lato:bold,bolditalic">Lato Bold 700 (plus italic)</option>
<option value="Lato:light">Lato Light 300</option>
<option value="Lato:light,lightitalic">Lato Light 300 (plus italic)</option>
<option value="Lato:regular">Lato Regular 400</option>
<option value="Lato:regular,regularitalic">Lato Regular 400 (plus italic)</option>
<option value="League Script">League Script</option>
<option value="Leckerli One">Leckerli One </option>
<option value="Lekton"> Lekton </option>
<option value="Lekton:regular,italic,bold">Lekton (plus italic and bold)</option>
<option value="Lemon">Lemon</option>
<option value="Limelight"> Limelight </option>
<option value="Linden Hill">Linden Hill</option>
<option value="Linden Hill:400,400italic">Linden Hill:400,400italic</option>
<option value="Lobster Two">Lobster Two</option>
<option value="Lobster Two:400,400italic,700,700italic">Lobster Two (plus italic, bold, and bold italic)</option>
<option value="Lobster">Lobster</option>
<option value="Lora">Lora</option>
<option value="Lora:400,700,400italic,700italic">Lora (plus bold and italic)</option>
<option value="Love Ya Like A Sister">Love Ya Like A Sister</option>
<option value="Loved by the King">Loved by the King</option>
<option value="Luckiest Guy">Luckiest Guy</option>
<option value="Maiden Orange">Maiden Orange</option>
<option value="Mako">Mako</option>
<option value="Marck Script">Marck Script</option>
<option value="Marko One">Marko One</option>
<option value="Marmelad">Marmelad</option>
<option value="Marvel">Marvel </option>
<option value="Marvel:400,400italic,700,700italic">Marvel (plus bold and italic) </option>
<option value="Mate SC">Mate SC</option>
<option value="Mate">Mate</option>
<option value="Mate:400,400italic">Mate:400,400italic</option>
<option value="Maven Pro"> Maven Pro</option>
<option value="Maven Pro:500"> Maven Pro 500</option>
<option value="Maven Pro:900"> Maven Pro 900</option>
<option value="Maven Pro:bold"> Maven Pro 700</option>
<option value="Meddon">Meddon</option>
<option value="MedievalSharp">MedievalSharp</option>
<option value="Medula One">Medula One</option>
<option value="Megrim">Megrim</option>
<option value="Merienda One">Merienda One</option>
<option value="Merriweather">Merriweather</option>
<option value="Metamorphous">Metamorphous</option>
<option value="Metrophobic">Metrophobic</option>
<option value="Michroma">Michroma</option>
<option value="Miltonian Tattoo">Miltonian Tattoo</option>
<option value="Miltonian">Miltonian</option>
<option value="Miniver">Miniver</option>
<option value="Miss Fajardose">Miss Fajardose</option>
<option value="Miss Saint Delafield">Miss Saint Delafield</option>
<option value="Modern Antiqua">Modern Antiqua</option>
<option value="Molengo">Molengo</option>
<option value="Monofett">Monofett</option>
<option value="Monoton">Monoton </option>
<option value="Monsieur La Doulaise">Monsieur La Doulaise</option>
<option value="Montez">Montez </option>
<option value="Montserrat">Montserrat</option>
<option value="Mountains of Christmas">Mountains of Christmas</option>
<option value="Mr Bedford">Mr Bedford</option>
<option value="Mr Dafoe">Mr Dafoe</option>
<option value="Mr De Haviland">Mr De Haviland</option>
<option value="Mrs Sheppards">Mrs Sheppards</option>
<option value="Muli">Muli Regular</option>
<option value="Muli:light">Muli Light</option>
<option value="Muli:light,lightitalic">Muli Light (plus italic)</option>
<option value="Muli:regular,regularitalic">Muli Regular (plus italic)</option>
<option value="Neucha">Neucha</option>
<option value="Neuton">Neuton</option>
<option value="News Cycle">News Cycle</option>
<option value="Niconne">Niconne</option>
<option value="Nixie One">Nixie One</option>
<option value="Nobile">Nobile</option>
<option value="Nobile:regular,italic,bold,bolditalic">Nobile (plus italic, bold, and bold italic)</option>
<option value="Nokora">Nokora</option>
<option value="Nokora:400,700">Nokora:400,700</option>
<option value="Nosifer">Nosifer</option>
<option value="Noticia Text">Noticia Text</option>
<option value="Noticia Text:400,400italic,700,700italic">Noticia Text:400,400italic,700,700italic</option>
<option value="Nova Cut">Nova Cut</option>
<option value="Nova Flat">Nova Flat</option>
<option value="Nova Mono">Nova Mono</option>
<option value="Nova Oval">Nova Oval</option>
<option value="Nova Round">Nova Round</option>
<option value="Nova Script">Nova Script</option>
<option value="Nova Slim">Nova Slim</option>
<option value="Nova Square">Nova Square</option>
<option value="Numans">Numans </option>
<option value="Nunito"> Nunito Regular</option>
<option value="Nunito:light"> Nunito Light</option>
<option value="OFL Sorts Mill Goudy TT">OFL Sorts Mill Goudy TT</option>
<option value="OFL Sorts Mill Goudy TT:regular,italic">OFL Sorts Mill Goudy TT (plus italic)</option>
<option value="Old Standard TT">Old Standard TT</option>
<option value="Old Standard TT:regular,italic,bold">Old Standard TT (plus italic and bold)</option>
<option value="Open Sans Condensed:light,lightitalic">Open Sans Condensed</option>
<option value="Open Sans:600,600italic">Open Sans 600</option>
<option value="Open Sans:800,800italic">Open Sans 800</option>
<option value="Open Sans:bold,bolditalic">Open Sans bold</option>
<option value="Open Sans:light,lightitalic">Open Sans light</option>
<option value="Open Sans:light,lightitalic,regular,regularitalic,600,600italic,bold,bolditalic,800,800italic">Open Sans (all weights)</option>
<option value="Open Sans:regular,regularitalic">Open Sans regular</option>
<option value="Orbitron">Orbitron Regular (400)</option>
<option value="Orbitron:500">Orbitron 500</option>
<option value="Orbitron:900">Orbitron 900</option>
<option value="Orbitron:bold">Orbitron Regular (700)</option>
<option value="Original Surfer">Original Surfer</option>
<option value="Oswald">Oswald</option>
<option value="Over the Rainbow">Over the Rainbow</option>
<option value="Overlock SC">Overlock SC</option>
<option value="Overlock">Overlock</option>
<option value="Overlock:400,400italic,700,700italic,900,900italic">Overlock:400,400italic,700,700italic,900,900italic</option>
<option value="Ovo">Ovo </option>
<option value="PT Sans Caption">PT Sans Caption</option>
<option value="PT Sans Caption:regular,bold">PT Sans Caption (plus bold)</option>
<option value="PT Sans Narrow">PT Sans Narrow</option>
<option value="PT Sans Narrow:regular,bold">PT Sans Narrow (plus bold)</option>
<option value="PT Sans">PT Sans</option>
<option value="PT Sans:regular,italic,bold,bolditalic">PT Sans (plus itlic, bold, and bold italic)</option>
<option value="PT Serif Caption">PT Serif Caption</option>
<option value="PT Serif Caption:regular,italic">PT Serif Caption (plus italic)</option>
<option value="PT Serif">PT Serif</option>
<option value="PT Serif:regular,italic,bold,bolditalic">PT Serif (plus italic, bold, and bold italic)</option>
<option value="Pacifico">Pacifico</option>
<option value="Passero One">Passero One</option>
<option value="Passion One">Passion One</option>
<option value="Passion One:400,700,900">Passion One:400,700,900</option>
<option value="Patrick Hand">Patrick Hand</option>
<option value="Patua One">Patua One</option>
<option value="Paytone One">Paytone One</option>
<option value="Permanent Marker">Permanent Marker</option>
<option value="Petrona">Petrona</option>
<option value="Philosopher">Philosopher</option>
<option value="Piedra">Piedra</option>
<option value="Pinyon Script">Pinyon Script</option>
<option value="Plaster">Plaster</option>
<option value="Play">Play</option>
<option value="Play:regular,bold">Play (plus bold)</option>
<option value="Playball">Playball</option>
<option value="Playfair Display"> Playfair Display </option>
<option value="Podkova"> Podkova </option>
<option value="Poller One">Poller One</option>
<option value="Poly">Poly</option>
<option value="Poly:400,400italic">Poly:400,400italic</option>
<option value="Pompiere">Pompiere </option>
<option value="Prata">Prata</option>
<option value="Prociono">Prociono</option>
<option value="Puritan">Puritan</option>
<option value="Puritan:regular,italic,bold,bolditalic">Puritan (plus italic, bold, and bold italic)</option>
<option value="Quantico">Quantico</option>
<option value="Quantico:400,400italic,700,700italic">Quantico:400,400italic,700,700italic</option>
<option value="Quattrocento Sans">Quattrocento Sans</option>
<option value="Quattrocento">Quattrocento</option>
<option value="Questrial">Questrial </option>
<option value="Quicksand">Quicksand</option>
<option value="Quicksand:300,400,700">Quicksand:300,400,700</option>
<option value="Qwigley">Qwigley</option>
<option value="Radley">Radley</option>
<option value="Raleway:100">Raleway</option>
<option value="Rammetto One">Rammetto One</option>
<option value="Rancho">Rancho</option>
<option value="Rationale">Rationale </option>
<option value="Redressed">Redressed</option>
<option value="Reenie Beanie">Reenie Beanie</option>
<option value="Ribeye Marrow">Ribeye Marrow</option>
<option value="Ribeye">Ribeye</option>
<option value="Righteous">Righteous</option>
<option value="Rochester">Rochester </option>
<option value="Rock Salt">Rock Salt</option>
<option value="Rokkitt">Rokkitt</option>
<option value="Rosario">Rosario </option>
<option value="Ruge Boogie">Ruge Boogie</option>
<option value="Ruslan Display">Ruslan Display</option>
<option value="Ruthie">Ruthie</option>
<option value="Sail">Sail</option>
<option value="Salsa">Salsa</option>
<option value="Sancreek">Sancreek</option>
<option value="Sansita One">Sansita One</option>
<option value="Sarina">Sarina</option>
<option value="Satisfy">Satisfy</option>
<option value="Schoolbell">Schoolbell</option>
<option value="Shadows Into Light">Shadows Into Light</option>
<option value="Shanti">Shanti</option>
<option value="Short Stack">Short Stack </option>
<option value="Sigmar One">Sigmar One</option>
<option value="Signika Negative">Signika Negative</option>
<option value="Signika Negative:300,400,600,700">Signika Negative:300,400,600,700</option>
<option value="Signika">Signika</option>
<option value="Signika:300,400,600,700">Signika:300,400,600,700</option>
<option value="Six Caps">Six Caps</option>
<option value="Slackey">Slackey</option>
<option value="Smokum">Smokum </option>
<option value="Smythe">Smythe</option>
<option value="Sniglet:800">Sniglet</option>
<option value="Snippet">Snippet </option>
<option value="Sofia">Sofia</option>
<option value="Sorts Mill Goudy">Sorts Mill Goudy</option>
<option value="Sorts Mill Goudy:400,400italic">Sorts Mill Goudy (plus italic)</option>
<option value="Special Elite">Special Elite</option>
<option value="Spicy Rice">Spicy Rice</option>
<option value="Spinnaker">Spinnaker</option>
<option value="Spirax">Spirax</option>
<option value="Squada One">Squada One</option>
<option value="Stardos Stencil">Stardos Stencil</option>
<option value="Stardos Stencil:400,700">Stardos Stencil (plus bold)</option>
<option value="Stint Ultra Condensed">Stint Ultra Condensed</option>
<option value="Stoke">Stoke</option>
<option value="Sue Ellen Francisco">Sue Ellen Francisco</option>
<option value="Sunshiney">Sunshiney</option>
<option value="Supermercado One">Supermercado One</option>
<option value="Swanky and Moo Moo">Swanky and Moo Moo</option>
<option value="Syncopate">Syncopate</option>
<option value="Tangerine">Tangerine</option>
<option value="Tenor Sans"> Tenor Sans </option>
<option value="Terminal Dosis Light">Terminal Dosis Light</option>
<option value="Terminal Dosis">Terminal Dosis Regular</option>
<option value="Terminal Dosis:200">Terminal Dosis 200</option>
<option value="Terminal Dosis:300">Terminal Dosis 300</option>
<option value="Terminal Dosis:500">Terminal Dosis 500</option>
<option value="Terminal Dosis:600">Terminal Dosis 600</option>
<option value="Terminal Dosis:700">Terminal Dosis 700</option>
<option value="Terminal Dosis:800">Terminal Dosis 800</option>
<option value="The Girl Next Door">The Girl Next Door</option>
<option value="Tinos">Tinos</option>
<option value="Tinos:regular,italic,bold,bolditalic">Tinos (plus italic, bold, and bold italic)</option>
<option value="Trade Winds">Trade Winds</option>
<option value="Trykker">Trykker</option>
<option value="Tulpen One">Tulpen One </option>
<option value="Ubuntu Condensed">Ubuntu Condensed</option>
<option value="Ubuntu Mono">Ubuntu Mono</option>
<option value="Ubuntu Mono:regular,italic,bold,bolditalic">Ubuntu Mono:regular,italic,bold,bolditalic</option>
<option value="Ubuntu">Ubuntu</option>
<option value="Ubuntu:regular,italic,bold,bolditalic">Ubuntu (plus italic, bold, and bold italic)</option>
<option value="Ultra">Ultra</option>
<option value="Uncial Antiqua">Uncial Antiqua</option>
<option value="UnifrakturCook:bold">UnifrakturCook</option>
<option value="UnifrakturMaguntia">UnifrakturMaguntia</option>
<option value="Unkempt">Unkempt</option>
<option value="Unlock">Unlock</option>
<option value="Unna">Unna </option>
<option value="VT323">VT323</option>
<option value="Varela Round">Varela Round</option>
<option value="Varela">Varela</option>
<option value="Vast Shadow">Vast Shadow</option>
<option value="Vibur">Vibur</option>
<option value="Vidaloka">Vidaloka </option>
<option value="Viga">Viga</option>
<option value="Volkhov">Volkhov </option>
<option value="Volkhov:400,400italic,700,700italic">Volkhov (plus bold and italic) </option>
<option value="Vollkorn">Vollkorn</option>
<option value="Vollkorn:regular,italic,bold,bolditalic">Vollkorn (plus italic, bold, and bold italic)</option>
<option value="Voltaire">Voltaire </option>
<option value="Waiting for the Sunrise">Waiting for the Sunrise</option>
<option value="Wallpoet">Wallpoet</option>
<option value="Walter Turncoat">Walter Turncoat</option>
<option value="Wire One">Wire One</option>
<option value="Yanone Kaffeesatz">Yanone Kaffeesatz</option>
<option value="Yanone Kaffeesatz:300">Yanone Kaffeesatz:300</option>
<option value="Yanone Kaffeesatz:400">Yanone Kaffeesatz:400</option>
<option value="Yanone Kaffeesatz:700">Yanone Kaffeesatz:700</option>
<option value="Yellowtail">Yellowtail </option>
<option value="Yeseva One">Yeseva One</option>
<option value="Yesteryear">Yesteryear</option>
<option value="Zeyada">Zeyada</option>
';
	}
?>


<h2><?php _e('Font 1 Options', 'googlefonts'); ?></h2>

<p><strong><?php _e('Select Font:', 'googlefonts'); ?></strong></p>

<select name="googlefonts_font1" id="googlefonts_font1">
<option selected="selected"><?php echo $this->options['googlefonts_font1'] ;?></option>
<option value="off"><?php _e('None (Turn off Font 1)', 'googlefonts'); ?></option>

<?php listgooglefontoptions(); ?>

</select>

<p><strong><?php _e('Elements you want to assign this font to:*', 'googlefonts'); ?></strong></p>

<input type="checkbox" <?php if ($this->options['googlefont1_body'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_body" value="checked"> <?php _e('All (body tags)', 'googlefonts'); ?><br>                                                         
<input type="checkbox" <?php if ($this->options['googlefont1_heading1'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_heading1" value="checked"> <?php _e('Headline 1 (h1 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont1_heading2'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_heading2" value="checked"> <?php _e('Headline 2 (h2 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont1_heading3'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_heading3" value="checked"> <?php _e('Headline 3 (h3 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont1_heading4'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_heading4" value="checked"> <?php _e('Headline 4 (h4 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont1_heading5'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_heading5" value="checked"> <?php _e('Headline 5 (h5 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont1_heading6'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_heading6" value="checked"> <?php _e('Headline 6 (h6 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont1_blockquote'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_blockquote" value="checked"> <?php _e('Blockquotes', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont1_p'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_p" value="checked"> <?php _e('Paragraphs (p tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont1_li'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont1_li" value="checked"> <?php _e('Lists (li tags)', 'googlefonts'); ?><br>

<p><strong><?php _e('Custom CSS:*', 'googlefonts'); ?></strong></p>
<textarea name="googlefont1_css" cols="70" rows="8" id="googlefont1_css">
<?php echo stripslashes($this->options['googlefont1_css']) ; ?>
</textarea>

<p><input type="submit" name="googlefonts_save" value="<?php _e('Save', 'googlefonts'); ?>" /></p>

<hr />








<h2><?php _e('Font 2 Options', 'googlefonts'); ?></h2>

<p><strong><?php _e('Select Font:', 'googlefonts'); ?></strong></p>

<select name="googlefonts_font2" id="googlefonts_font2">
<option selected="selected"><?php echo $this->options['googlefonts_font2'] ;?></option>
<option value="off"><?php _e('None (Turn off Font 2)', 'googlefonts'); ?></option>

<?php listgooglefontoptions(); ?>

</select>

<p><strong><?php _e('Elements you want to assign this font to:*', 'googlefonts'); ?></strong><br />

<input type="checkbox" <?php if ($this->options['googlefont2_body'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_body" value="checked"> All (body tags)<br>
<input type="checkbox" <?php if ($this->options['googlefont2_heading1'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_heading1" value="checked"> <?php _e('Headline 1 (h1 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont2_heading2'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_heading2" value="checked"> <?php _e('Headline 2 (h2 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont2_heading3'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_heading3" value="checked"> <?php _e('Headline 3 (h3 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont2_heading4'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_heading4" value="checked"> <?php _e('Headline 4 (h4 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont2_heading5'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_heading5" value="checked"> <?php _e('Headline 5 (h5 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont2_heading6'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_heading6" value="checked"> <?php _e('Headline 6 (h6 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont2_blockquote'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_blockquote" value="checked"> <?php _e('Blockquotes', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont2_p'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_p" value="checked"> <?php _e('Paragraphs (p tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont2_li'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont2_li" value="checked"> <?php _e('Lists (li tags)', 'googlefonts'); ?><br>


<p><strong><?php _e('Custom CSS:*', 'googlefonts'); ?></strong></p>
<p><textarea name="googlefont2_css" cols="70" rows="8" id="googlefont2_css">
<?php echo stripslashes($this->options['googlefont2_css']) ; ?>
</textarea>

<p><input type="submit" name="googlefonts_save" value="<?php _e('Save', 'googlefonts'); ?>" /></p>

<hr />







<h2><?php _e('Font 3 Options', 'googlefonts'); ?></h2>

<p><strong><?php _e('Select Font:', 'googlefonts'); ?></strong></p>

<select name="googlefonts_font3" id="googlefonts_font3">
<option selected="selected"><?php echo $this->options['googlefonts_font3'] ;?></option>
<option value="off"><?php _e('None (Turn off Font 3)', 'googlefonts'); ?></option>

<?php listgooglefontoptions(); ?>

</select>

<p><strong><?php _e('Elements you want to assign this font to:*', 'googlefonts'); ?></strong><br />

<input type="checkbox" <?php if ($this->options['googlefont3_body'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_body" value="checked"> <?php _e('All (body tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont3_heading1'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_heading1" value="checked"> <?php _e('Headline 1 (h1 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont3_heading2'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_heading2" value="checked"> <?php _e('Headline 2 (h2 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont3_heading3'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_heading3" value="checked"> <?php _e('Headline 3 (h3 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont3_heading4'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_heading4" value="checked"> <?php _e('Headline 4 (h4 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont3_heading5'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_heading5" value="checked"> <?php _e('Headline 5 (h5 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont3_heading6'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_heading6" value="checked"> <?php _e('Headline 6 (h6 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont3_blockquote'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_blockquote" value="checked"> <?php _e('Blockquotes', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont3_p'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_p" value="checked"> <?php _e('Paragraphs (p tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont3_li'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont3_li" value="checked"> <?php _e('Lists (li tags)', 'googlefonts'); ?><br>


<p><strong><?php _e('Custom CSS:*', 'googlefonts'); ?></strong></p>
<p><textarea name="googlefont3_css" cols="70" rows="8" id="googlefont3_css">
<?php echo stripslashes($this->options['googlefont3_css']) ; ?>
</textarea>

<p><input type="submit" name="googlefonts_save" value="<?php _e('Save', 'googlefonts'); ?>" /></p>

<hr />







<h2><?php _e('Font 4 Options', 'googlefonts'); ?></h2>

<p><strong><?php _e('Select Font:', 'googlefonts'); ?></strong></p>

<select name="googlefonts_font4" id="googlefonts_font4">
<option selected="selected"><?php echo $this->options['googlefonts_font4'] ;?></option>
<option value="off"><?php _e('None (Turn off Font 4)', 'googlefonts'); ?></option>

<?php listgooglefontoptions(); ?>

</select>

<p><strong><?php _e('Elements you want to assign this font to:*', 'googlefonts'); ?></strong><br />

<input type="checkbox" <?php if ($this->options['googlefont4_body'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_body" value="checked"> <?php _e('All (body tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont4_heading1'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_heading1" value="checked"> <?php _e('Headline 1 (h1 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont4_heading2'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_heading2" value="checked"> <?php _e('Headline 2 (h2 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont4_heading3'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_heading3" value="checked"> <?php _e('Headline 3 (h3 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont4_heading4'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_heading4" value="checked"> <?php _e('Headline 4 (h4 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont4_heading5'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_heading5" value="checked"> <?php _e('Headline 5 (h5 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont4_heading6'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_heading6" value="checked"> <?php _e('Headline 6 (h6 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont4_blockquote'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_blockquote" value="checked"> <?php _e('Blockquotes', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont4_p'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_p" value="checked"> <?php _e('Paragraphs (p tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont4_li'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont4_li" value="checked"> <?php _e('Lists (li tags)', 'googlefonts'); ?><br>


<p><strong><?php _e('Custom CSS:*', 'googlefonts'); ?></strong></p>
<p><textarea name="googlefont4_css" cols="70" rows="8" id="googlefont4_css">
<?php echo stripslashes($this->options['googlefont4_css']) ; ?>
</textarea>

<p><input type="submit" name="googlefonts_save" value="<?php _e('Save', 'googlefonts'); ?>" /></p>

<hr />







<h2><?php _e('Font 5 Options', 'googlefonts'); ?></h2>

<p><strong><?php _e('Select Font:', 'googlefonts'); ?></strong></p>

<select name="googlefonts_font5" id="googlefonts_font5">
<option selected="selected"><?php echo $this->options['googlefonts_font5'] ;?></option>

<option value="off"><?php _e('None (Turn off Font 5)', 'googlefonts'); ?></option>

<?php listgooglefontoptions(); ?>
</select>

<p><strong><?php _e('Elements you want to assign this font to:*', 'googlefonts'); ?></strong><br />

<input type="checkbox" <?php if ($this->options['googlefont5_body'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_body" value="checked"> <?php _e('All (body tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont5_heading1'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_heading1" value="checked"> <?php _e('Headline 1 (h1 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont5_heading2'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_heading2" value="checked"> <?php _e('Headline 2 (h2 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont5_heading3'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_heading3" value="checked"> <?php _e('Headline 3 (h3 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont5_heading4'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_heading4" value="checked"> <?php _e('Headline 4 (h4 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont5_heading5'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_heading5" value="checked"> <?php _e('Headline 5 (h5 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont5_heading6'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_heading6" value="checked"> <?php _e('Headline 6 (h6 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont5_blockquote'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_blockquote" value="checked"> <?php _e('Blockquotes', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont5_p'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_p" value="checked"> <?php _e('Paragraphs (p tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont5_li'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont5_li" value="checked"> <?php _e('Lists (li tags)', 'googlefonts'); ?><br>


<p><strong><?php _e('Custom CSS:*', 'googlefonts'); ?></strong></p>
<p><textarea name="googlefont5_css" cols="70" rows="8" id="googlefont5_css">
<?php echo stripslashes($this->options['googlefont5_css']) ; ?>
</textarea>

<p><input type="submit" name="googlefonts_save" value="<?php _e('Save', 'googlefonts'); ?>" /></p>

<hr />







<h2><?php _e('Font 6 Options', 'googlefonts'); ?></h2>

<p><strong><?php _e('Select Font:', 'googlefonts'); ?></strong></p>

<select name="googlefonts_font6" id="googlefonts_font6">
<option selected="selected"><?php echo $this->options['googlefonts_font6'] ;?></option>
<option value="off"><?php _e('None (Turn off Font 6)', 'googlefonts'); ?></option>

<?php listgooglefontoptions(); ?>

</select>

<p><strong><?php _e('Elements you want to assign this font to:*', 'googlefonts'); ?></strong><br />

<input type="checkbox" <?php if ($this->options['googlefont6_body'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_body" value="checked"> <?php _e('All (body tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont6_heading1'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_heading1" value="checked"> <?php _e('Headline 1 (h1 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont6_heading2'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_heading2" value="checked"> <?php _e('Headline 2 (h2 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont6_heading3'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_heading3" value="checked"> <?php _e('Headline 3 (h3 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont6_heading4'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_heading4" value="checked"> <?php _e('Headline 4 (h4 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont6_heading5'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_heading5" value="checked"> <?php _e('Headline 5 (h5 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont6_heading6'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_heading6" value="checked"> <?php _e('Headline 6 (h6 tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont6_blockquote'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_blockquote" value="checked"> <?php _e('Blockquotes', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont6_p'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_p" value="checked"> <?php _e('Paragraphs (p tags)', 'googlefonts'); ?><br>
<input type="checkbox" <?php if ($this->options['googlefont6_li'] == "checked") { echo 'checked'; } else { echo 'unchecked'; } ?> name="googlefont6_li" value="checked"> <?php _e('Lists (li tags)', 'googlefonts'); ?><br>


<p><strong><?php _e('Custom CSS:*', 'googlefonts'); ?></strong></p>
<p><textarea name="googlefont6_css" cols="70" rows="8" id="googlefont6_css">
<?php echo stripslashes($this->options['googlefont6_css']) ; ?>
</textarea>

<p><input type="submit" name="googlefonts_save" value="<?php _e('Save', 'googlefonts'); ?>" /></p>

<hr />


<h2><?php _e('Troubleshooting', 'googlefonts'); ?></h2>
<p><?php _e('The most common error occurs when a font is specified as "off" but has boxes checked beneath it. Make sure that there are no boxes checked beneath a font that you want to be off.', 'googlefonts'); ?></p>

<p><?php _e('This plugin uses open source fonts that are hosted on Google&rsquo;s servers. For more information about this service, you can visit the', 'googlefonts'); ?> 
	<a href="http://code.google.com/webfonts/"><?php _e('Google Font Directory', 'googlefonts'); ?></a>.
</p>
<hr />


<h2><?php _e('* CSS WARNING', 'googlefonts'); ?></h2>
<p><?php _e('Most likely the theme you are using has defined very specific elements in its stylesheet and these may override the generic tags specified above. If you don&rsquo;t see any changes after checking the style boxes above, you will need to enter custom css into the CSS box. An example of CSS that would be more specific would be:', 'googlefonts'); ?></p>
	
<p>#container p { font-family: 'Reenie Beanie', arial, serif; }</p>

<p><?php _e('This would define all paragraphs found within a &lt;div id=&quot;container&quot;&gt;&lt;/div&gt; element. Stylesheets (CSS) can be sensitive and tricky sometimes. If you are new to CSS the <a href="http://www.w3schools.com/css/" title="w3schools tutorials">w3schools tutorials</a> are a great place to start.', 'googlefonts'); ?>

</form>
    </td>
  </tr>
</table>

<?php }
  } //End Class
} //End if class exists statement

//instantiate the class
if (class_exists('googlefonts')) {
    $googlefonts_var = new googlefonts();
}
?>