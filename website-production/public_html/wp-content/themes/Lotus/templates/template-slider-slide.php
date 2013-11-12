		<?php
				$pp_slider_items = get_option('pp_slider_items');
				if(empty($pp_slider_items))
				{
					$pp_slider_items = 5;
				}

				$pp_slider_sort = get_option('pp_slider_sort');
				if(empty($pp_slider_sort))
				{
					$pp_slider_sort = 'ASC';
				}

				$pp_slider_offset = 1;
				if (is_page("home2")) // only for second variation
					$pp_slider_offset = 0;

				$slider_arr = get_posts('numberposts='.$pp_slider_items.'&order='.$pp_slider_sort.'&orderby=date&post_type=slides&offset='.$pp_slider_offset);

				//print_r($slider_arr);

				if(!empty($slider_arr))
				{
		?>

				<div id="anything_slider">
					<div class="wrapper">
						<ul>
							<?php
								foreach($slider_arr as $key => $gallery_item)
								{
									$image_url = '';

									if(has_post_thumbnail($gallery_item->ID, 'large'))
									{
										$image_id = get_post_thumbnail_id($gallery_item->ID);
										$image_url = wp_get_attachment_image_src($image_id, 'full', true);
									}

									$gallery_type = get_post_meta($gallery_item->ID, 'gallery_type', true);
									if(empty($gallery_type))
									{
										$gallery_type = 'Image';
									}

									$hyperlink_url = get_post_meta($gallery_item->ID, 'gallery_link_url', true);


									switch($gallery_type)
									{
										case 'Image':
							?>
							<li id="anythingslide<?php echo $key+1; ?>">
								<a href="<?php echo $hyperlink_url;?>">
									<img src="<?php echo $image_url[0]; ?>" alt=""/>
								</a>
							</li>
							<?php
										break;
										//End image gallery

										case 'Custom HTML':
							?>
							<li>
								<?php
									echo do_shortcode($gallery_item->post_content);
								?>
							</li>
							<?php
										break;
									}
								}
							?>
						</ul>
					</div>
				</div>

		<?php
				}
		?>

<?php
	$pp_homepage_slider_nav = true;

	$pp_slider_auto_play = get_option('pp_slider_auto_play');
	if(!empty($pp_slider_auto_play))
	{
		$pp_slider_auto_play = 'true';
	}
	else
	{
		$pp_slider_auto_play = 'false';
	}

	$pp_slider_animation_time = get_option('pp_slider_animation_time');
	if(empty($pp_slider_animation_time))
	{
		$pp_slider_animation_time = 600;
	}
?>

<?php
/* commented out because this disables Google Website Optimizer
<script type="text/javascript">
    $j(function () {

    	$j('#anything_slider').anythingSlider({
    	        easing: "easeInOutExpo",
    	        autoPlay: <?php echo $pp_slider_auto_play; ?>,
    	        delay: parseInt($j('#slider_timer').val() * 1000),
    	        startStopped: false,
    	        animationTime: <?php echo $pp_slider_animation_time; ?>,
    	        hashTags: false,
    	        buildNavigation: true,
    	        buildArrows: true,
    			pauseOnHover: true,
    			startText: "Go",
    	        stopText: "Stop"
    	    });

    	<?php
    		if($pp_homepage_slider_nav)
    		{
    	?>
    			$j('#anything_slider').hover(function()
				{
					$j(this).find('.arrow').css('z-index', '9999');
					$j(this).find('.arrow').css('visibility', 'visible');
				},
				function()
				{
					$j(this).find('.arrow').css('visibility', 'hidden');
				});
    	<?php
    		}
    	?>
    });
</script>
*/
?>
