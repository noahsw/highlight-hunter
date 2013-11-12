<?php
/**
 * The main template file for display gallery page.
 *
 * @package WordPress
*/

if(!isset($hide_header) OR !$hide_header)
{
	get_header(); 
}

$caption_class = "page_caption";
$portfolio_sets_query = '';
$custom_title = '';

if(!empty($term))
{
	$portfolio_sets_query.= $term;
	
	$obj_term = get_term_by('slug', $term, 'photos_galleries');
	$custom_title = $obj_term->name;
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

if(!isset($hide_header) OR !$hide_header)
{
?>				
		<div class="page_caption">
			<div class="caption_inner">
				<div class="caption_header">
					<h1 class="cufon"><?php echo the_title(); ?></h1>
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
				<div id="gallery_wrapper" class="inner_wrapper portfolio">
				
<?php
}
else
{
	echo '<br class="clear"/>';
}
?>	

						<?php echo do_shortcode(html_entity_decode($page->post_content)); ?>
						
						<!-- Begin portfolio content -->
						
						<?php
							$menu_sets_query = '';

							$portfolio_items = -1;
							
							$portfolio_sort = get_option('pp_gallery_sort'); 
							if(empty($portfolio_sort))
							{
								$portfolio_sort = 'DESC';
							}
							
							$args = array( 
								'post_type' => 'attachment', 
								'numberposts' => $portfolio_items, 
								'post_status' => null, 
								'post_parent' => $post->ID,
								'order' => $portfolio_sort,
								'orderby' => 'date',
							); 								
							$all_photo_arr = get_posts( $args );
		
							if(isset($all_photo_arr) && !empty($all_photo_arr))
							{

						?>
						
											<?php

												foreach($all_photo_arr as $key => $portfolio_item)
												{
													
													$image_url = '';
								
													if(!empty($portfolio_item->guid))
													{
														$image_id = $portfolio_item->ID;
														$image_url[0] = $portfolio_item->guid;
													}
													
													$last_class = '';
													$line_break = '';
													if(($key+1) % 4 == 0)
													{	
														$last_class = ' last';
														
														if(isset($page_photo_arr[$key+1]))
														{
															$line_break = '<br class="clear"/><br/>';
														}
														else
														{
															$line_break = '<br class="clear"/>';
														}
													}
													
											?>
															<div class="one_fourth<?php echo $last_class?>" style="margin-right:2%;margin-top:-20px">
																<a title="<?php echo $portfolio_item->post_title?>" href="<?php echo $image_url[0]?>" class="one_fourth_img" rel="gallery" href="<?php echo $image_url[0]?>">
																	<img src="<?php bloginfo( 'stylesheet_directory' ); ?>/timthumb.php?src=<?php echo $image_url[0]?>&h=370&w=350&zc=1" alt=""/>
																</a>
															</div>
										    
										    <?php
												
													echo $line_break;
												}
												//End foreach loop
												
										    ?>
								
							<?php
								
							}
							//End if have portfolio items
							?>
						
						    
						</div>
						<!-- End main content -->
					
					<br class="clear"/><br/>
				
				</div>
				
<?php
if(!isset($hide_header) OR !$hide_header)
{
?>				
			
		</div>
		<!-- End content -->
				

<?php get_footer(); ?>
<?php
}
?>