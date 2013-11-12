<?php

/*
	Begin Create Shortcode Generator Options
*/

add_action('admin_menu', 'pp_shortcode_generator');

function pp_shortcode_generator() {

  add_submenu_page('functions.php', 'Shortcode Generator', 'Shortcode Generator', 'manage_options', 'pp_shortcode_generator', 'pp_shortcode_generator_options');

}

function pp_shortcode_generator_options() {

  	if (!current_user_can('manage_options'))  {
    	wp_die( __('You do not have sufficient permissions to access this page.') );
  	}
  	
  	$plugin_url = get_bloginfo( 'stylesheet_directory' ).'/plugins/shortcode_generator';

	//Begin shortcode array
	$shortcodes = array(
		'dropcap' => array(
			'attr' => array(),
			'desc' => array(),
			'content' => TRUE,
		),
		'quote' => array(
			'attr' => array(),
			'desc' => array(),
			'content' => TRUE,
		),
		'button' => array(
			'attr' => array(
				'href' => 'text',
				'align' => 'select',
				'bg_color' => 'text',
				'text_color' => 'text',
			),
			'desc' => array(
				'href' => 'Enter URL for button',
				'align' => 'Button Alignment',
				'bg_color' => 'Enter background color code ex. #000000',
				'text_color' => 'Enter text color code ex. #ffffff',
			),
			'options' => array(
				'left' => 'left',
				'right' => 'right',
				'center' => 'center',
			),
			'content' => TRUE,
			'content_text' => 'Enter text on button',
		),
		'lightbox' => array(
			'attr' => array(
				'type' => 'select',
				'title' => 'text',
				'href' => 'text',
				'youtube_id' => 'text',
				'vimeo_id' => 'text',
			),
			'desc' => array(
				'href' => 'Enter URL for button',
				'align' => 'Button Alignment',
				'bg_color' => 'Enter background color code ex. #000000',
				'text_color' => 'Enter text color code ex. #ffffff',
			),
			'options' => array(
				'image' => 'Image',
				'iframe' => 'iFrame',
				'youtube' => 'Youtube Video',
				'vimeo' => 'Vimeo Video',
			),
			'content' => TRUE,
			'content_text' => 'Enter content (can be normal text, HTML code or shortcode)',
		),
		'frame_left' => array(
			'attr' => array(
				'src' => 'text',
				'href' => 'text',
			),
			'desc' => array(
				'src' => 'Enter image URL',
				'href' => 'Enter hyperlink URL for image',
			),
			'content' => TRUE,
			'content_text' => 'Image Caption',
		),
		'frame_right' => array(
			'attr' => array(
				'src' => 'text',
				'href' => 'text',
			),
			'desc' => array(
				'src' => 'Enter image URL',
				'href' => 'Enter hyperlink URL for image',
			),
			'content' => TRUE,
			'content_text' => 'Image Caption',
		),
		'frame_center' => array(
			'attr' => array(
				'src' => 'text',
				'href' => 'text',
			),
			'desc' => array(
				'src' => 'Enter image URL',
				'href' => 'Enter hyperlink URL for image',
			),
			'content' => TRUE,
			'content_text' => 'Image Caption',
		),
		'one_half' => array(
			'attr' => array(),
			'desc' => array(),
			'content' => TRUE,
			'repeat' => 1,
		),
		'one_third' => array(
			'attr' => array(),
			'desc' => array(),
			'content' => TRUE,
			'repeat' => 2,
		),
		'one_fourth' => array(
			'attr' => array(),
			'desc' => array(),
			'content' => TRUE,
			'repeat' => 3,
		),
		'styled_box' => array(
			'attr' => array(
				'title' => 'text',
				'color' => 'select',
			),
			'desc' => array(
				'href' => 'Enter URL for button',
				'color' => 'Select box color',
			),
			'options' => array(
				'black' => 'Black',
				'gray' => 'Gray',
				'white' => 'White',
				'blue' => 'Blue',
				'yellow' => 'Yellow',
				'red' => 'Red',
				'orange' => 'Orange',
				'green' => 'Green',
				'pink' => 'Pink',
				'purple' => 'Purple',
			),
			'content' => TRUE,
			'content_text' => 'Enter Content',
		),
		'pp_gallery' => array(
			'attr' => array(),
			'desc' => array(),
			'content' => TRUE,
			'content_text' => htmlentities('Your Images (line by line) ex. <a href="image.jpg"><img src="image_small.jpg"/></a>'),
		),
		'map' => array(
			'attr' => array(
				'width' => 'text',
				'height' => 'text',
				'lat' => 'text',
				'long' => 'text',
				'zoom' => 'text',
			),
			'desc' => array(
				'width' => 'Map width in pixels',
				'height' => 'Map height in pixels',
				'lat' => 'Map latitude <a href="http://www.tech-recipes.com/rx/5519/the-easy-way-to-find-latitude-and-longitude-values-in-google-maps/">Find here</a>',
				'long' => 'Map longitude <a href="http://www.tech-recipes.com/rx/5519/the-easy-way-to-find-latitude-and-longitude-values-in-google-maps/">Find here</a>',
				'zoom' => 'Enter zoom number (1-16)',
			),
			'content' => FALSE,
			'options' => array(
				1 => 'Open',
				0 => 'Close',
			),
		),
		'youtube' => array(
			'attr' => array(
				'width' => 'text',
				'height' => 'text',
				'video_id' => 'text',
			),
			'desc' => array(
				'width' => 'Video width in pixels',
				'height' => 'Video height in pixels',
				'video_id' => 'Youtube video ID something like Js9Z8UQAA4E',
			),
			'content' => FALSE,
		),
		'vimeo' => array(
			'attr' => array(
				'width' => 'text',
				'height' => 'text',
				'video_id' => 'text',
			),
			'desc' => array(
				'width' => 'Video width in pixels',
				'height' => 'Video height in pixels',
				'video_id' => 'Vimeo video ID something like 9380243',
			),
			'content' => FALSE,
		),
		'html5video' => array(
			'attr' => array(
				'width' => 'text',
				'height' => 'text',
				'poster' => 'text',
				'mp4' => 'text',
				'webm' => 'text',
				'ogg' => 'text',
			),
			'desc' => array(
				'width' => 'Video width in pixels',
				'height' => 'Video height in pixels',
				'poster' => 'Poster image for video',
				'mp4' => 'Video URL in mp4 format',
				'webm' => 'Video URL in mp4 webm',
				'ogg' => 'Video URL in mp4 ogg',
			),
			'content' => FALSE,
		),
		'slideshow' => array(
			'attr' => array(
				'width' => 'text',
				'height' => 'text',
			),
			'desc' => array(
				'width' => 'Slideshow width in pixels',
				'height' => 'Slideshow height in pixels',
			),
			'content' => TRUE,
			'content_text' => htmlentities('Your Images URL (line by line) ex. /example/photo1.jpg'),
		),
		'nivoslide' => array(
			'attr' => array(
				'width' => 'text',
				'height' => 'text',
				'effect' => 'select',
				'pauseTime' => 'text',
			),
			'options' => array(
				'sliceDown' => 'sliceDown',
				'sliceDownLeft' => 'sliceDownLeft',
				'sliceUp' => 'sliceUp',
				'sliceUpLeft' => 'sliceUpLeft',
				'sliceUpDown' => 'sliceUpDown',
				'sliceUpDownLeft' => 'sliceUpDownLeft',
				'fold' => 'fold',
				'fade' => 'fade',
				'random' => 'random',
			),
			'desc' => array(
				'width' => 'Slideshow width in pixels',
				'height' => 'Slideshow height in pixels',
				'effect' => 'The effect parameter can be any of the following',
				'pauseTime' => 'Enter pause time for each slide (in seconds)',
			),
			'content' => TRUE,
			'content_text' => htmlentities('Your Images URL (line by line) ex. /example/photo1.jpg'),
		),
	);

?>
<script>
jQuery(document).ready(function(){ 
	jQuery('#shortcode_select').change(function() {
  		var target = jQuery(this).val();
  		jQuery('.rm_section').css('display', 'none');
  		jQuery('#div_'+target).css('display', '');
	});	
	
	jQuery('.code_area').click(function() { 
		document.getElementById(jQuery(this).attr('id')).focus();
    	document.getElementById(jQuery(this).attr('id')).select();
	});
	
	jQuery('.button').click(function() { 
		var target = jQuery(this).attr('id');
		var gen_shortcode = '';
  		gen_shortcode+= '['+target;
  		
  		if(jQuery('#'+target+'_attr_wrapper .attr').length > 0)
  		{
  			jQuery('#'+target+'_attr_wrapper .attr').each(function() {
				gen_shortcode+= ' '+jQuery(this).attr('name')+'="'+jQuery(this).val()+'"';
			});
		}
		
		gen_shortcode+= ']\n';
		
		if(jQuery('#'+target+'_content').length > 0)
  		{
  			gen_shortcode+= jQuery('#'+target+'_content').val()+'\n[/'+target+']\n';
  			
  			var repeat = jQuery('#'+target+'_content_repeat').val();
  			for (count=1;count<=repeat;count=count+1)
			{
				if(count<repeat)
				{
					gen_shortcode+= '['+target+']\n';
					gen_shortcode+= jQuery('#'+target+'_content').val()+'\n[/'+target+']\n';
				}
				else
				{
					gen_shortcode+= '['+target+'_last]\n';
					gen_shortcode+= jQuery('#'+target+'_content').val()+'\n[/'+target+'_last]';
				}
			}
  		}
  		
  		jQuery('#'+target+'_code').val(gen_shortcode);
	});
});
</script>

<div class="wrap rm_wrap">
	<div class="header_wrap">
	<h2>Shortcode Generator</h2>
	
	For future updates follow me <a href="http://themeforest.net/user/peerapong">@themeforest</a> or <a href="http://twitter.com/ipeerapong">@twitter</a>
	</div><br/>
	
	<div style="padding:30px 20px 30px 20px;background:#fff">
	<?php
		if(!empty($shortcodes))
		{
	?>
			<strong>Select Shortcode:</strong>
			<select id="shortcode_select">
				<option value="">---Select---</option>
			
	<?php
			foreach($shortcodes as $shortcode_name => $shortcode)
			{
	?>
	
			<option value="<?php echo $shortcode_name; ?>"><?php echo $shortcode_name; ?></option>
	
	<?php
			}
	?>
			</select>
	<?php
		}
	?>
	
	<br/><br/>
	
	<?php
		if(!empty($shortcodes))
		{
			foreach($shortcodes as $shortcode_name => $shortcode)
			{
	?>
	
			<div id="div_<?php echo $shortcode_name; ?>" class="rm_section" style="display:none">
				<div class="rm_title">
					<h3 style="margin-left:15px"><?php echo ucfirst($shortcode_name); ?></h3>
					<div class="clearfix"></div>
				</div>
				
				<div class="rm_input rm_text" style="padding-left:20px">
				
				<!-- img src="<?php echo $plugin_url.'/'.$shortcode_name.'.png'; ?>" alt=""/><br/><br/><br/ -->
				
				<?php
					if(isset($shortcode['content']) && $shortcode['content'])
					{
						if(isset($shortcode['content_text']))
						{
							$content_text = $shortcode['content_text'];
						}
						else
						{
							$content_text = 'Your Content';
						}
				?>
				
				<strong><?php echo $content_text; ?>:</strong><br/>
				<input type="hidden" id="<?php echo $shortcode_name; ?>_content_repeat" value="<?php echo $shortcode['repeat']; ?>"/>
				<textarea id="<?php echo $shortcode_name; ?>_content" style="width:90%;height:70px" rows="3" wrap="off"></textarea><br/><br/>
				
				<?php
					}
				?>
			
				<?php
					if(isset($shortcode['attr']) && !empty($shortcode['attr']))
					{
				?>
						
						<div id="<?php echo $shortcode_name; ?>_attr_wrapper">
						
				<?php
						foreach($shortcode['attr'] as $attr => $type)
						{
				?>
				
							<?php echo '<strong>'.ucfirst($attr).'</strong>: '.$shortcode['desc'][$attr]; ?><br/>
							
							<?php
								switch($type)
								{
									case 'text':
							?>
							
									<input type="text" id="<?php echo $shortcode_name; ?>_text" style="width:90%" class="attr" name="<?php echo $attr; ?>"/>
							
							<?php
									break;
									
									case 'select':
							?>
							
									<select id="<?php echo $shortcode_name; ?>_select" style="width:25%" class="attr" name="<?php echo $attr; ?>">
									
										<?php
											if(isset($shortcode['options']) && !empty($shortcode['options']))
											{
												foreach($shortcode['options'] as $select_key => $option)
												{
										?>
										
													<option value="<?php echo $select_key; ?>"><?php echo $option; ?></option>
										
										<?php	
												}
											}
										?>							
									
									</select>
							
							<?php
									break;
								}
							?>
							
							<br/><br/>
				
				<?php
						} //end attr foreach
				?>
				
						</div>
				
				<?php
					}
				?>
				<br/>
				
				<input type="button" id="<?php echo $shortcode_name; ?>" value="Generate Shortcode" class="button"/>
				
				<br/><br/><br/>
				
				<strong>Shortcode:</strong><br/>
				<textarea id="<?php echo $shortcode_name; ?>_code" style="width:90%;height:70px" rows="3" readonly="readonly" class="code_area" wrap="off"></textarea>
				
				</div>
				
			</div>
	
	<?php
			} //end shortcode foreach
		}
	?>
	
	</div>
</div>
<br style="clear:both"/>

<?php

}

/*
	End Create Shortcode Generator Options
*/

?>