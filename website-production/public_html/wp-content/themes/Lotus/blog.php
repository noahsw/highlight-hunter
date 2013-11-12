<?php
/**
 * Template Name: Blog With Sidebar
 * The main template file for display blog page.
 *
 * @package WordPress
*/

/**
*	Get Current page object
**/
$page = get_page($post->ID);

/**
*	Get current page id
**/

if(!isset($current_page_id) && isset($page->ID))
{
    $current_page_id = $page->ID;
}

if(!isset($hide_header) OR !$hide_header)
{
	get_header();
}
else
{
	include_once(TEMPLATEPATH.'/blog_home.php');
	exit;
}

$page_style = get_post_meta($current_page_id, 'page_style', true);
$page_sidebar = get_post_meta($current_page_id, 'page_sidebar', true);
$caption_style = get_post_meta($current_page_id, 'caption_style', true);

if(empty($caption_style))
{
	$caption_style = 'Title & Description';
}

if(!isset($sidebar_home))
{
	$sidebar_home = '';
}

if(empty($page_sidebar))
{
	$page_sidebar = 'Blog Sidebar';
}
$caption_class = "page_caption";

if(!isset($add_sidebar))
{
	$add_sidebar = FALSE;
}

$sidebar_class = '';

if($page_style == 'Right Sidebar')
{
	$add_sidebar = TRUE;
	$page_class = 'sidebar_content';
}
elseif($page_style == 'Left Sidebar')
{
	$add_sidebar = TRUE;
	$page_class = 'sidebar_content';
	$sidebar_class = 'left_sidebar';
}
else
{
	$page_class = 'inner_wrapper';
}

$pp_title = get_option('pp_blog_title');

if(empty($pp_title))
{
	$pp_title = 'Blog';
}

$additional_style = '';

if(!isset($hide_header) OR !$hide_header)
{
		$page_button_title = get_post_meta($current_page_id, 'page_button_title', true);
		$page_button_href = get_post_meta($current_page_id, 'page_button_href', true);
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

<?php
}
else
{
	$additional_style = 'style="margin-top:-10px"';
}
?>

					<?php
						if($add_sidebar && $page_style == 'Left Sidebar')
						{
					?>
						<div class="sidebar_wrapper <?php echo $sidebar_class; ?>">

							<div class="sidebar <?php echo $sidebar_class; ?> <?php echo $sidebar_home; ?>">

								<div class="content">

									<ul class="sidebar_widget">
									<?php dynamic_sidebar($page_sidebar); ?>
									</ul>

								</div>

							</div>
							<br class="clear"/>

							<div class="sidebar_bottom <?php echo $sidebar_class; ?>"></div>
						</div>
					<?php
						}
					?>

					<div class="sidebar_content" <?php echo $additional_style; ?>>

<?php

global $more; $more = false; # some wordpress wtf logic

$query_string ="post_type=post&paged=$paged";

$cat_id = get_cat_ID(single_cat_title('', false));
if(!empty($cat_id))
{
	$query_string.= '&cat='.$cat_id;
}

query_posts($query_string);

if (have_posts()) : while (have_posts()) : the_post();

	$image_thumb = '';

	$post_img_align = get_post_meta(get_the_ID(), 'post_img_align', true);
	if(empty($post_img_align))
	{
		$post_img_align = 'Top';
	}

	$post_img_align = strtolower($post_img_align);

	if(has_post_thumbnail(get_the_ID(), 'large'))
	{
	    $image_id = get_post_thumbnail_id(get_the_ID());
	    $image_thumb = wp_get_attachment_image_src($image_id, 'large', true);

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
									Posted by&nbsp;<?php the_author(); ?>&nbsp;|&nbsp; <?php comments_number('0 Comments', '1 Comment', '% Comments'); ?>.
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




<?php endwhile; endif; ?>

						<div class="pagination"><p><?php posts_nav_link(' '); ?></p></div>

					</div>

					<?php
						if($add_sidebar && $page_style == 'Right Sidebar')
						{
					?>
						<div class="sidebar_wrapper <?php echo $sidebar_class; ?>">

							<div class="sidebar_top <?php echo $sidebar_class; ?>"></div>

							<div class="sidebar <?php echo $sidebar_class; ?> <?php echo $sidebar_home; ?>">

								<div class="content">

									<ul class="sidebar_widget">
									<?php dynamic_sidebar($page_sidebar); ?>
									</ul>

								</div>

							</div>
							<br class="clear"/>

							<div class="sidebar_bottom <?php echo $sidebar_class; ?>"></div>
						</div>
					<?php
						}
					?>

<?php
$pp_homepage_hide_right_sidebar = get_option('pp_homepage_hide_right_sidebar');

if(!isset($hide_header) OR !$hide_header)
{
?>
			</div>
			<!-- End main content -->

			<br class="clear"/>

			</div>

		</div>
		<!-- End content -->


<?php get_footer(); ?>

<?php
}
elseif(!$pp_homepage_hide_right_sidebar)
{
?>

</div>
			<!-- End main content -->

			<br class="clear"/>

			</div>

		</div>
		<!-- End content -->

<?php
}
?>
