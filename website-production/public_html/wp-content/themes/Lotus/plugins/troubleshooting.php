<?php

/*
	Begin Create Troubleshooting Options
*/

add_action('admin_menu', 'pp_troubleshooting');

function pp_troubleshooting() {

  add_submenu_page('functions.php', 'Troubleshooting', 'Troubleshooting', 'manage_options', 'pp_troubleshooting', 'pp_troubleshooting_options');

}

function pp_troubleshooting_options() {

  	if (!current_user_can('manage_options'))  {
    	wp_die( __('You do not have sufficient permissions to access this page.') );
  	}
  	
  	$plugin_url = get_bloginfo( 'stylesheet_directory' ).'/plugins/troubleshooting';
?>

<div class="wrap rm_wrap">
	<div class="header_wrap">
	<h2>Troubleshooting</h2>
	
	For future updates follow me <a href="http://themeforest.net/user/peerapong">@themeforest</a> or <a href="http://twitter.com/ipeerapong">@twitter</a>
	<br/><br/>
	
	<div id="message" class="updated fade">
	<p style="line-height:1.5em"><strong>
		Please read the instructions in /manual folder from your download package. It will help guide you taking advantage all features of <?php echo $themename; ?> Template ;)
	</p></strong>
	</div>
	</div>
	
	<div style="padding:10px 20px 30px 20px;background:#fff">
	<h3>Q: Why the menu, slider or everything are broken?</h3>
	
	Please check.<br/><br/>
	
	<ol>
		<li>Make sure you unzip the file first because there are many folder in the file ex. manual, theme, license info.</li>
		<li>There might be javascript conflict between theme’s script and plugin’s script. <strong>(Try to disable all plugins)</strong></li>
		<li>Check if you create the Wordpress custom menu.</li>
		<li>Try to re-download the file again from Themeforest and upload to your server. Some files are broken while downloading or uploading which cause this issue.</li>
	</ol>
	
	<br/><hr/>
	
	<h3>Q: Why the menu isn't show up?</h3>
	A: You have to setup the menu via <strong>Appearance > Menus</strong>. There are 2 menu location in this theme.
	
	<p>
			For those who are not familiar with Wordpress 3.0 menu below are some tutorials. 
		</p>
		
		<object width="640" height="385"><param name="movie" value="http://www.youtube.com/v/Idt7oyCD1qY?fs=1&amp;hl=en_US&amp;rel=0&amp;color1=0x3a3a3a&amp;color2=0x999999"></param><param name="allowFullScreen" value="true"></param><param name="allowscriptaccess" value="always"></param><embed src="http://www.youtube.com/v/Idt7oyCD1qY?fs=1&amp;hl=en_US&amp;rel=0&amp;color1=0x3a3a3a&amp;color2=0x999999" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="640" height="385"></embed></object>
		<br/><br/>
		
		<ol>
			<li><a href="http://templatic.com/news/wordpress-3-0-menu-management">http://templatic.com/news/wordpress-3-0-menu-management</a></li>
			<li><a href="http://www.wonderhowto.com/how-to-use-new-menu-system-wordpress-3-0-thelonious-376792/">http://www.wonderhowto.com/how-to-use-new-menu-system-wordpress-3-0-thelonious-376792/</a></li>
		</ol>
		
		<strong>*Note: Please check if you named your menu as "Main Menu".</strong>
		
	<br/><br/><hr/>
	
	<h3>Q: Why I use shortcode but it's incorrectly align?</h3>
	
	A: It's because Wordpress automatically insert p tag, you can solve this issue using below plugin
<a href="http://wordpress.org/extend/plugins/text-control/">http://wordpress.org/extend/plugins/text-control/</a>
	
	<br/><br/><hr/>
	
	<h3>Q: Why all of my images isn't show up?</h3>
	A: There are many reason for that, please follow the checklist below.
	<br/><br/>
	<ol>
		<li>Please check if your <strong><?php echo TEMPLATEPATH; ?>/cahce</strong> folder of the theme is writable (chmod 777)</li>
		<li>Try to visit <strong><a href="<?php bloginfo( 'stylesheet_directory' ); ?>/timthumb.php"><?php bloginfo( 'stylesheet_directory' ); ?>/timthumb.php</a></strong> and see if you get any errors <br/><br/>ex. internal server error, 404 not found. If so then <strong>contact you hosting provider, there might be some server settings which block accessing timthumb file (<a href="http://skulblakashven.com/wp_skull/?p=934">See solution here</a>)</strong></li>
		<li><strong>Hot-link image is not support.</strong> Please use only image from your server.</li>
		<li>Check if you <strong>set featured image</strong> in Wordpress post page.</li>
	</ol>
	<br/>
	
	<p>Below are some solutions about timthumb.php file return "404 Not found" or "Internal Server Error"</p>
	
	<ol>
		<li><a href="http://www.jeffkee.com/timthumb-php-404-apache-php-security-chmod-755-777-error-lo/">http://www.jeffkee.com/timthumb-php-404-apache-php-security-chmod-755-777-error-lo/</a></li>
		<li><a href="http://joshhighland.com/blog/2010/05/20/timthumb-php-returns-a-404-error-in-wordpress/">http://joshhighland.com/blog/2010/05/20/timthumb-php-returns-a-404-error-in-wordpress/</a></li>
	</ol>
	
	<br/><hr/>
	
	<h3>Q: Why map shortcode is not working?</h3>
	A: You have to enter a Google Maps API key in Yen admin panel. <a href="http://code.google.com/apis/maps/signup.html">You can get your API key here</a>
	
	<br/><br/><hr/>
	
	<h3>Q: Why column shortcodes alignment is not display properly?</h3>
	A: It's because Wordpress automatically insert p tag, you can solve this issue using below plugin
<a href="http://wordpress.org/extend/plugins/text-control/">http://wordpress.org/extend/plugins/text-control/</a>

	<br/><br/>
	<strong>*Note: Pelase make sure, you enter shortcode in "HTML" mode only.</strong>
	
	<br/><br/><hr/>
	
	<h3>Q: How can I link to sets, galleries?</h3>
	A: To use portfolio sets, galleries.<br/><br/>

	Once you assign portfolio to set or create gallery. Go to appearance >menus. Then select which set or gallery you want to add to your menu items.<br/><br/>

If you can see it, click on screen option (top right) and check on sets, galleries</a>
	
	<br/><hr/>
	
	<h3>Q: How can I create example page like demo ex. about us, services?</h3>
	A: Open folder name "Styled Pages" and copy page's code and paste to your page content (in "HTML" mode)
	
	<br/>
	
	</div>
	
</div>
<br style="clear:both"/>

<?php

}

/*
	End Create Troubleshooting Options
*/

?>