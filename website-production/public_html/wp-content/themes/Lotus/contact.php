<?php
/**
 * Template Name: Contact
 * The main template file for display contact page.
 *
 * @package WordPress
*/


/**
*	if not submit form
**/

if(!isset($_GET['your_name']))
{

if(!isset($hide_header) OR !$hide_header)
{
	get_header(); 
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

$page_sidebar = get_post_meta($current_page_id, 'page_sidebar', true);

if(empty($page_sidebar))
{
	$page_sidebar = 'Contact Sidebar';
}

$caption_class = 'page_caption';

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
		
		<?php
			$pp_contact_display_map = get_option('pp_contact_display_map');
			
			if(!empty($pp_contact_display_map))
			{
				$pp_contact_lat = get_option('pp_contact_lat');
				$pp_contact_long = get_option('pp_contact_long');
				$pp_contact_map_zoom = get_option('pp_contact_map_zoom');
		?>
				<div id="map_contact" style="width:100%;height:300px;"></div>
				<script>
					$j(document).ready(function(){ 
						$j("#map_contact").gMap({ zoom: <?php echo $pp_contact_map_zoom; ?>, markers: [ { latitude: '<?php echo $pp_contact_lat; ?>', longitude: '<?php echo $pp_contact_long; ?>' } ] });
					});
				</script>
		<?php
			}
		?>

		<!-- Begin content -->
		<div id="content_wrapper">
			
			<div class="inner">

				<!-- Begin main content -->
				<div class="inner_wrapper">
				
<?php
}
?>
				
					<div class="sidebar_content">
						<?php 
							if(!isset($hide_header) OR !$hide_header)
							{
								if ( have_posts() ) while ( have_posts() ) : the_post(); ?>		

									<?php the_content(); break; ?><br/><br/>

						<?php endwhile; 
							}
						?>
						
						<form id="contact_form" method="post" action="<?php echo curPageURL(); ?>">
						    <p>
						    	<label for="your_name">Name</label><br/>
						    	<input id="your_name" name="your_name" type="text" style="width:94%"/>
						    </p>
						    <p style="margin-top:20px">
						    	<label for="email">Email</label><br/>
						    	<input id="email" name="email" type="text" style="width:94%"/>
						    </p>
						    <p style="margin-top:20px">
						    	<label for="message">Message</label><br/>
						    	<textarea id="message" name="message" rows="7" cols="10" style="width:94%"></textarea>
						    </p>
						    <p style="margin-top:20px">
								<input type="submit" value="Send Message"/><br/>
							</p>
						</form>
						<div id="reponse_msg"></div>
						<br/><br/>
						
					</div>
					
					<div class="sidebar_wrapper">
						<div class="sidebar_top"></div>
						
						<div class="sidebar">
							
							<div class="content">
							
								<ul class="sidebar_widget">
								<?php dynamic_sidebar($page_sidebar); ?>
								</ul>
								
							</div>
						
						</div>
						<br class="clear"/>
					
						<div class="sidebar_bottom"></div>
					</div>
				
<?php
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
				
<?php
}

//if submit form
else
{

	/*
	|--------------------------------------------------------------------------
	| Mailer module
	|--------------------------------------------------------------------------
	|
	| These module are used when sending email from contact form
	|
	*/
	
	//Get your email address
	$contact_email = get_option('pp_contact_email');
	
	//Enter your email address, email from contact form will send to this addresss. Please enter inside quotes ('myemail@email.com')
	define('DEST_EMAIL', $contact_email);
	
	//Change email subject to something more meaningful
	define('SUBJECT_EMAIL', 'Email from contact form');
	
	//Thankyou message when message sent
	define('THANKYOU_MESSAGE', 'Thank you! We will get back to you as soon as possible');
	
	//Error message when message can't send
	define('ERROR_MESSAGE', 'Oops! something went wrong, please try to submit later.');
	
	
	/*
	|
	| Begin sending mail
	|
	*/
	
	$from_name = $_GET['your_name'];
	$from_email = $_GET['email'];
	
	$message = 'Name: '.$from_name.PHP_EOL;
	$message.= 'Email: '.$from_email.PHP_EOL.PHP_EOL;
	$message.= 'Message: '.PHP_EOL.$_GET['message'];
	    
	
	if(!empty($from_name) && !empty($from_email) && !empty($message))
	{
		mail(DEST_EMAIL, SUBJECT_EMAIL, $message);
	
		echo THANKYOU_MESSAGE;
		
		exit;
	}
	else
	{
		echo ERROR_MESSAGE;
		
		exit;
	}
	
	/*
	|
	| End sending mail
	|
	*/
}

?>