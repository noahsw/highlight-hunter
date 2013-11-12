<?php
/**
 * The main template file for display single post page.
 *
 * @package WordPress
*/


get_header();

if($post->post_type == 'gallery')
{
	include (TEMPLATEPATH . "/gallery.php");
    exit;
}

if($post->post_type == 'portfolios')
{
	include (TEMPLATEPATH . "/templates/template-portfolio-single.php");
    exit;
}

$pp_blog_page = get_option('pp_blog_page');
$page_sidebar = get_post_meta($pp_blog_page, 'page_sidebar', true);

if(empty($page_sidebar))
{
	$page_sidebar = 'Blog Sidebar';
}

$caption_class = "page_caption";

$pp_title = get_option('pp_blog_title');

if(empty($pp_title))
{
	$pp_title = 'Blog';
}

//Make blog menu active
if(!empty($pp_blog_page))
{
?>

<script>
$('ul#main_menu li.page-item-<?php echo $pp_blog_page; ?>').addClass('current_page_item');
</script>

<?php
}
?>

<?php

if (have_posts()) : while (have_posts()) : the_post();
?>
<!--
		<div class="page_caption">
			<div class="caption_inner">
				<div class="caption_header">
					<h1 class="cufon"><?php echo $pp_title; ?></h1>
				</div>

				<?php
					if(!empty($page_button_title))
					{
				?>
						<div style="float:right;margin:20px 20px 0 0">
							<a class="button" href="<?php echo $page_button_href; ?>"><?php echo $page_button_title; ?></a>
						</div>
				<?php
					}
				?>
			</div>
		</div>
		-->

		</div>
		<div id="header_pattern"></div>

		<!-- Begin content -->
		<div id="content_wrapper">

			<div class="inner">

				<!-- Begin main content -->
				<div class="inner_wrapper">

					<div class="sidebar_content" style="border:0">

<?php

	$image_thumb = '';

	if(has_post_thumbnail(get_the_ID(), 'large'))
	{
	    $image_id = get_post_thumbnail_id(get_the_ID());
	    $image_thumb = wp_get_attachment_image_src($image_id, 'large', true);
	    $post_img_align = get_post_meta(get_the_ID(), 'post_img_align', true);
		if(empty($post_img_align))
		{
			$post_img_align = 'Top';
		}

		$post_img_align = strtolower($post_img_align);

	  	if($post_img_align == 'top')
	    {
	  		$pp_blog_image_width = 640;
			$pp_blog_image_height = 240;
		}
		else
		{
			$pp_blog_image_width = 240;
			$pp_blog_image_height = 180;
		}
	}
?>

						<!-- Begin each blog post -->
						<div class="post_wrapper">

							<div class="post_header">
								<h3 class="cufon">
									<a href="<?php the_permalink(); ?>" title="<?php the_title(); ?>">
										<?php the_title(); ?>
									</a>
								</h1>
								<div class="post_detail">
									<?php the_time('d M Y'); ?>&nbsp;|&nbsp;
									Posted by&nbsp;<?php the_author(); ?>&nbsp;|&nbsp; <?php comments_number('0 Comment', '1 Comment', '% Comments'); ?>.
								</div>
							</div>
							<div class="circle_comment">
								<div><?php comments_number('0', '1', '%'); ?></div>
							</div>

							<br class="clear"/>

							<?php
								if(!empty($image_thumb))
								{
									if($post_img_align == 'top')
	   			 					{
							?>
									<br/>
									<a href="<?php the_permalink(); ?>" title="<?php the_title(); ?>">
										<img src="<?php echo get_bloginfo( 'stylesheet_directory' ); ?>/timthumb.php?src=<?php echo $image_thumb[0]; ?>&amp;h=<?php echo $pp_blog_image_height; ?>&amp;w=<?php echo $pp_blog_image_width; ?>&amp;zc=1" alt="" class="img_nofade frame"/>
									</a>
									<br class="clear"/><br/>
							<?php
									}
									else
									{
							?>
									<br/>
									<a href="<?php the_permalink(); ?>" title="<?php the_title(); ?>">
										<img src="<?php echo get_bloginfo( 'stylesheet_directory' ); ?>/timthumb.php?src=<?php echo $image_thumb[0]; ?>&amp;h=<?php echo $pp_blog_image_height; ?>&amp;w=<?php echo $pp_blog_image_width; ?>&amp;zc=1" alt="" class="img_nofade frame align<?php echo $post_img_align; ?>"/>
									</a>
							<?php
									}
								}
							?>

							<?php echo the_content(); ?>

							<br class="clear"/>

							<hr/>

							<br class="clear"/>
						</div>
						<!-- End each blog post -->

						<?php
							$pp_blog_display_author = get_option('pp_blog_display_author');

							if(!empty($pp_blog_display_author))
							{
						?>

						<h4 class="cufon">About the author</h4>

						<div id="about_the_author">
							<div class="thumb"><?php echo get_avatar( get_the_author_email(), '50' ); ?></div>
							<div class="description">
								<strong><?php the_author_link(); ?></strong><br/>
								<?php the_author_description(); ?>
							</div>
						</div>
						<br class="clear"/><hr/>

						<?php
							}
						?>

						<?php
							$pp_blog_display_related = get_option('pp_blog_display_related');

							if(!empty($pp_blog_display_related))
							{
						?>

						<?php
						//for use in the loop, list 5 post titles related to first tag on current post
						$tags = wp_get_post_tags($post->ID);
						if ($tags) {
						  $first_tag = $tags[0]->term_id;
						  $args=array(
						    'tag__in' => array($first_tag),
						    'post__not_in' => array($post->ID),
						    'showposts'=>3,
						    'caller_get_posts'=>1
						   );
						  $my_query = new WP_Query($args);
						  if( $my_query->have_posts() ) {
						  	echo '<br class="clear"/><h4 class="cufon">Related Posts</h4><br class="clear"/>';
						 ?>

						  <div class="related_posts">

						 <?php
						    while ($my_query->have_posts()) : $my_query->the_post();
						    	$image_thumb = '';

								if(has_post_thumbnail($post->ID, 'large'))
								{
								    $image_id = get_post_thumbnail_id($post->ID);
								    $image_thumb = wp_get_attachment_image_src($image_id, 'large', true);
								}
						    ?>

						    	<?php
						    		if(!empty($image_thumb))
						    		{
						    	?>
						    		<div class="one_third">
						    			<a href="<?php the_permalink() ?>" rel="bookmark" title="Permanent Link to <?php the_title_attribute(); ?>"><img src="<?php bloginfo( 'stylesheet_directory' ); ?>/timthumb.php?src=<?php echo $image_thumb[0]; ?>&h=100&w=175&zc=1" alt="" class="frame img_nofade" />
						    			</a>
						    		</div>
						    	<?php
						    		}
						    	?>


						    		<div class="two_third last">
						      			<h6 class="cufon"><a href="<?php the_permalink() ?>" rel="bookmark" title="Permanent Link to <?php the_title_attribute(); ?>"><?php the_title(); ?></a></h6><?php echo pp_substr(strip_tags(strip_shortcodes($post->post_content)), 200); ?>
						      		</div>

						    	<br class="clear"/><br/><br/>

						      <?php
								    endwhile;

								    wp_reset_query();
						    	?>
						    	</div>
						<?php
						  }
						}
						?>

						<br class="clear"/><hr/>

						<?php
							}
						?>


						<?php comments_template( '' ); ?>


<?php endwhile; endif; ?>

						</div>

					<div class="sidebar_wrapper">
						<div class="sidebar_top" style="border:0"></div>

						<div class="sidebar">

							<div class="content">

								<ul class="sidebar_widget">
									<?php dynamic_sidebar($page_sidebar); ?>
								</ul>

							</div>

						</div>

						<div class="sidebar_bottom"></div>
						<br class="clear"/>

					</div>

				</div>
				<!-- End main content -->

				<br class="clear"/>
			</div>

		</div>
		<!-- End content -->



<?php get_footer(); ?>
