<?php
/**
 * The main template file for display blog for homepage.
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
								<h1 class="cufon">
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

							<?php echo get_the_content_with_formatting(); ?>

							<br class="clear"/>

							<hr/>

							<br class="clear"/>
						</div>
						<!-- End each blog post -->




<?php endwhile; endif; ?>
