<?php
/**
 * The main template file for display error page.
 *
 * @package WordPress
*/


get_header(); 

?>

		<div class="page_caption">
			<div class="caption_inner">
				<div class="caption_header">
					<h1 class="cufon">Error 404 Page Not Found</h1>
				</div>
			</div>
		</div>
		
		<div id="header_pattern"></div>
		
		</div>

		<!-- Begin content -->
		<div id="content_wrapper">
		
			<div class="inner">
				
				<!-- Begin main content -->
				<div class="inner_wrapper">
					
						<h2 class="cufon"><?php _e( 'Oops!', '' ); ?></h2>
						<p><?php _e( 'Apologies, but the page you requested could not be found. Perhaps searching will help.', 'Soon' ); ?></p>
					
				</div>
				<!-- End main content -->
				
				<br class="clear"/><br/><br/><br/><br/><br/><br/>
			</div>
			
		</div>
		<!-- End content -->

<?php get_footer(); ?>