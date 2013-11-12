<?php
/**
 * The main template file for display portfolio page.
 *
 * @package WordPress
*/

if(!isset($hide_header) OR !$hide_header)
{
	get_header(); 
}

$caption_class = "page_caption";
$portfolio_sets_query = '';

if(!empty($term))
{
	$portfolio_sets_query.= $term;
	
	$obj_term = get_term_by('slug', $term, 'portfoliosets');
	$custom_title = 'Sets / '.$obj_term->name;
}
else
{
	$custom_title = get_the_title();
}

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

$caption_style = get_post_meta($current_page_id, 'caption_style', true);

if(empty($caption_style))
{
	$caption_style = 'Title & Description';
}

if(!isset($hide_header) OR !$hide_header)
{
	$page_button_title = get_post_meta($current_page_id, 'page_button_title', true);
	$page_button_href = get_post_meta($current_page_id, 'page_button_href', true);
?>				
		<div class="page_caption">
			<div class="caption_inner">
				<div class="caption_header">
					<h1 class="cufon"><?php echo $custom_title; ?></h1>
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
		
		<div id="header_pattern"></div>
		
		</div>
				
		<!-- Begin content -->
		<div id="content_wrapper">
			
			<div class="inner">
			
				<!-- Begin main content -->
				<div class="inner_wrapper portfolio">
				
<?php
}
?>	
						
						<?php  
							if(empty($term) && !$hide_header) {
								if ( have_posts() ) while ( have_posts() ) : the_post(); ?>		
					
							<?php do_shortcode(the_content()); break;  ?>

						<?php endwhile; 
							}
							else
							{
								$pp_homepage = get_option('pp_homepage');
								$obj_home = get_page($pp_homepage);
							    $pp_home_content = $obj_home->post_content;
							    echo do_shortcode(html_entity_decode($pp_home_content));
							}
						?>
						
						<br class="clear"/>
						
						<!-- Begin portfolio content -->
						
						<?php
							$menu_sets_query = '';

							$portfolio_items = get_option('pp_portfolio_items'); 
							if(empty($portfolio_items))
							{
								$portfolio_items = 12;
							}
							
							$portfolio_sort = get_option('pp_portfolio_sort'); 
							if(empty($portfolio_sort))
							{
								$portfolio_sort = 'DESC';
							}
							
							//prepare data for pagintion
							$offset_query = '';
							if(!isset($_GET['page']) OR empty($_GET['page']) OR $_GET['page'] == 1)
							{
							    $current_page = 1;
							}
							else
							{ 
							    $current_page = $_GET['page'];
							    $offset = (($current_page-1) * $portfolio_items);
							}
							
							$args = array(
								'numberposts' => $portfolio_items,
								'order' => $portfolio_sort,
								'orderby' => 'date',
								'post_type' => array('portfolios'),
								'offset' => $offset,
							);
							if(!empty($term))
							{
								$args['portfoliosets'].= $term;
							}
							
							$page_photo_arr = get_posts($args);
							
							
							//Get all portfolio items for paging
							
							$args = array(
								'numberposts' => -1,
								'order' => $portfolio_sort,
								'orderby' => 'date',
								'post_type' => array('portfolios'),
							);
							if(!empty($term))
							{
								$args['portfoliosets'].= $term;
							}
							
							$all_photo_arr = get_posts($args);
							$total = count($all_photo_arr);
		
							if(isset($page_photo_arr) && !empty($page_photo_arr))
							{
								
						?>
						
											<?php

												foreach($page_photo_arr as $key => $portfolio_item)
												{
													
													$image_url = '';
								
													if(has_post_thumbnail($portfolio_item->ID, 'large'))
													{
														$image_id = get_post_thumbnail_id($portfolio_item->ID);
														$image_url = wp_get_attachment_image_src($image_id, 'large', true);
													}
													
													$last_class = '';
													$line_break = '';
													if(($key+1) % 4 == 0)
													{	
														$last_class = ' last';
														
														if(isset($page_photo_arr[$key+1]))
														{
															$line_break = '';
														}
														else
														{
															$line_break = '<br class="clear"/>';
														}
													}
													
													$portfolio_link_url = get_post_meta($portfolio_item->ID, 'portfolio_link_url', true);
													
													if(empty($portfolio_link_url))
													{
														$permalink_url = get_permalink($portfolio_item->ID);
													}
													else
													{
														$permalink_url = $portfolio_link_url;
													}
											?>
															<div class="one_fourth<?php echo $last_class?>" style="margin-right:2%;margin-top:-20px">
																<a title="<?php echo $portfolio_item->post_title?>" href="<?php echo $permalink_url; ?>" class="one_fourth_img">
																	<img src="<?php bloginfo( 'stylesheet_directory' ); ?>/timthumb.php?src=<?php echo $image_url[0]?>&h=370&w=350&zc=1" alt=""/>
																</a>
															</div>
										    
										    <?php
												
													echo $line_break;
												}
												//End foreach loop
												
										    ?>
							
							<br class="clear"/><br/>
							<?php
								if(!isset($hide_header) OR !$hide_header)
								{
									if(empty($term))
									{
										$base_link = get_permalink($post->ID);
									}
									else
									{
										$base_link = curPageURL();
									}
									
									echo gen_pagination($total, $current_page, $base_link, TRUE, $portfolio_items);
								}
								
							}
							//End if have portfolio items
							?>

				
<?php
if(!isset($hide_header) OR !$hide_header)
{
?>				
			</div>
			<!-- End main content -->
					
			<br class="clear"/><br/>
				
			</div>
			
		</div>
		<!-- End content -->
				

<?php get_footer(); ?>
<?php
}
?>