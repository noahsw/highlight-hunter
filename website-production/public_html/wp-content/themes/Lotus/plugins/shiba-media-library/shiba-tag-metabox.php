<?php
// don't load directly
if (!function_exists('is_admin')) {
    header('Status: 403 Forbidden');
    header('HTTP/1.1 403 Forbidden');
    exit();
}

/**
 * Modified post tags form fields.
 *
 */
if (!class_exists("Shiba_Tag_Metabox")) :
	 
class Shiba_Tag_Metabox {
 
	function post_tags_meta_box($post, $box) {
		$tax_name = esc_attr(substr($box['id'], 8));
		$taxonomy = get_taxonomy($tax_name);
		$helps      = isset( $taxonomy->helps      ) ? esc_attr( $taxonomy->helps ) : esc_attr__('Separate tags with commas.');
		$help_hint  = isset( $taxonomy->help_hint  ) ? $taxonomy->help_hint         : __('Add new tag');
		$help_nojs  = isset( $taxonomy->help_nojs  ) ? $taxonomy->help_nojs         : __('Add or remove tags');
		$help_cloud = isset( $taxonomy->help_cloud ) ? $taxonomy->help_cloud        : __('Choose from the most used tags');
	
		$disabled = !current_user_can($taxonomy->cap->assign_terms) ? 'disabled="disabled"' : '';
	?>
	<div class="tagsdiv" id="<?php echo $tax_name; ?>">
		<div class="jaxtag">
		<div class="nojs-tags hide-if-js">
		<p><?php echo $help_nojs; ?></p>
		<textarea name="<?php echo "tax_input[$tax_name]"; ?>" rows="3" cols="20" class="the-tags" id="tax-input-<?php echo $tax_name; ?>" <?php echo $disabled; ?>><?php if (is_object($post)) echo esc_attr(get_terms_to_edit( $post->ID, $tax_name )); ?></textarea></div>
		<?php if ( current_user_can($taxonomy->cap->assign_terms) ) : ?>
		<div class="ajaxtag hide-if-no-js">
			<label class="screen-reader-text" for="new-tag-<?php echo $tax_name; ?>"><?php echo $box['title']; ?></label>
			<div class="taghint"><?php echo $help_hint; ?></div>
			<p><input type="text" id="new-tag-<?php echo $tax_name; ?>" name="newtag[<?php echo $tax_name; ?>]" class="newtag form-input-tip" size="16" autocomplete="off" value="" />
			<input type="button" class="button tagadd" value="<?php esc_attr_e('Add'); ?>" tabindex="3" /></p>
		</div>
		<p class="howto"><?php echo $helps; ?></p>
		<?php endif; ?>
		</div>
		<div class="tagchecklist"></div>
	</div>
	<?php if ( current_user_can($taxonomy->cap->assign_terms) ) : ?>
		<p class="hide-if-no-js">
		<a href="#titlediv" class="tagcloud-link" id="link-<?php echo $tax_name.'-shiba_post'; ?>"><?php printf( __('Post Tags')); ?></a>
		</p>
		<p class="hide-if-no-js">
		 <a href="#titlediv" class="tagcloud-link" id="link-<?php echo $tax_name.'-shiba_attachment'; ?>"><?php printf( __('Attachment Tags')); ?></a>
		</p>
		<p class="hide-if-no-js">
	   <a href="#titlediv" class="tagcloud-link" id="link-<?php echo $tax_name.'-shiba_gallery'; ?>"><?php printf( __('Gallery Tags'), $box['title'] ); ?></a>
	<?php else : ?>
	<p><em><?php _e('You cannot modify this taxonomy.'); ?></em></p>
	<?php endif; ?>
	<?php
	}
		

} // end class	
endif;
?>