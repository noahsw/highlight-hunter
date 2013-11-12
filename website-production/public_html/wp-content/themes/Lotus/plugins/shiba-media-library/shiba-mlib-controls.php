<?php
if (!function_exists('is_admin')) {
    header('Status: 403 Forbidden');
    header('HTTP/1.1 403 Forbidden');
    exit();
}

if (!is_admin()) exit();

global $shiba_mlib;
$location = wp_get_referer(); //'upload.php?page=shiba_media_tools';

if (isset($_POST['save_domains'])) {
	check_admin_referer('shiba_media_tools');

	$_POST['image_domains'] = $shiba_mlib->parallelize_obj->clean_image_domains($_POST['image_domains']);
	if (isset($_POST['wpcontent_domains']))
		$_POST['wpcontent_domains'] = $shiba_mlib->parallelize_obj->clean_image_domains($_POST['wpcontent_domains']);
	$_POST['parallel_option'] = esc_attr($_POST['parallel_option']);
	unset($_POST['save_domains'], $_POST['_wpnonce'], $_POST['_wp_http_referer']);
	update_option('shiba_mlib_options', $_POST);
			
	$shiba_mlib->javascript_redirect($location);
	exit;
} elseif (isset($_POST['clear_cache'])) {
	check_admin_referer('shiba_media_tools');
	$shiba_mlib->parallelize_obj->clear_cache();
	$shiba_mlib->parallelize_obj->javascript_redirect($location);
	exit;
}

$options = get_option('shiba_mlib_options');
$po = $options['parallel_option'];	
$title = __('Shiba Media Tools');
?>
<div class="wrap">   
    <?php screen_icon(); ?>
    <h2><?php echo esc_html( $title ); ?></h2>
    <div style="height:30px;"></div>

    <form name="validate_links" id="validate_links" method="post" action="" class="">
        <?php wp_nonce_field('shiba_media_tools'); ?> 

        <div style="clear:both; width:590px;">
		<h3><?php _e('Image Link Domains: ') ?></h3>
        <div style="padding-bottom:30px;">
		<input type="radio" name="parallel_option" value="none" <?php if ($po == 'none') echo "checked";?>> Don't parallelize images.<br>	
		<input type="radio" name="parallel_option" value="single" <?php if ($po == 'single') echo "checked";?>> Parallelize single posts/pages.<br>
		<input type="radio" name="parallel_option" value="all" <?php if ($po == 'all') echo "checked";?>> Parallelize all.
		</div>
        
       <textarea name="image_domains" cols=80 rows=5><?php echo $options['image_domains'];?></textarea>
        
        <small><p>The first domain MUST always be your primary domain - i.e., the domain used for your image files.  
        <strong>NOTE - all domains must point to the SAME directory structure as your primary domain.</strong></p></small>
		</div>

		<!-- For multisite - add ability to parallelize shared wp-content media. This will be media used in plugin directories -->       
        <?php if (function_exists('is_multisite') && is_multisite()) : ?>
			<h3><?php _e('Shared wp-content domains for multisite: ') ?></h3>
			<div class="shiba-field">
            <input type="checkbox" name="parallel_wpcontent" <?php if (isset($options['parallel_wpcontent'])) echo 'checked';?>/> 
            <label>Parallelize shared wp-content directory.</label>
       		<textarea name="wpcontent_domains" cols=80 rows=5><?php echo $options['wpcontent_domains'];?></textarea>  	
           <small><p>Parallelize media assets in the shared wp-content directory. I.e., media assets of shared themes and plugins.</p></small>
            </div>
 		<?php endif; ?>

        <input type="submit" class="button" name="save_domains" value="<?php esc_attr_e('Save Image Domains'); ?>" />
<!--        <input type="submit" class="button" name="clear_cache" value="<?php esc_attr_e('Clear Cache'); ?>" /> -->

    </form>
    <div style="height:50px;clear:both;"></div>
	
</div>


