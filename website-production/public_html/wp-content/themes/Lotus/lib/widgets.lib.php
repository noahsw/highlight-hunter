<?php

/**
*	Begin Recent Posts Custom Widgets
**/

class Custom_Recent_Posts extends WP_Widget {
	function Custom_Recent_Posts() {
		$widget_ops = array('classname' => 'Custom_Recent_Posts', 'description' => 'The recent posts with thumbnails' );
		$this->WP_Widget('Custom_Recent_Posts', 'Custom Recent Posts', $widget_ops);
	}

	function widget($args, $instance) {
		extract($args, EXTR_SKIP);

		echo $before_widget;
		$items = empty($instance['items']) ? ' ' : apply_filters('widget_title', $instance['items']);
		
		if(!is_numeric($items))
		{
			$items = 3;
		}
		
		if(!empty($items))
		{
			pp_posts('recent', $items, TRUE);
		}
		
		echo $after_widget;
	}

	function update($new_instance, $old_instance) {
		$instance = $old_instance;
		$instance['items'] = strip_tags($new_instance['items']);

		return $instance;
	}

	function form($instance) {
		$instance = wp_parse_args( (array) $instance, array( 'items' => '') );
		$items = strip_tags($instance['items']);

?>
			<p><label for="<?php echo $this->get_field_id('items'); ?>">Items (default 3): <input class="widefat" id="<?php echo $this->get_field_id('items'); ?>" name="<?php echo $this->get_field_name('items'); ?>" type="text" value="<?php echo attribute_escape($items); ?>" /></label></p>
<?php
	}
}

register_widget('Custom_Recent_Posts');

/**
*	End Recent Posts Custom Widgets
**/


/**
*	Begin Category Posts Custom Widgets
**/

class Custom_Cat_Posts extends WP_Widget {
	function Custom_Cat_Posts() {
		$widget_ops = array('classname' => 'Custom_Cat_Posts', 'description' => 'Display category\'s post' );
		$this->WP_Widget('Custom_Cat_Posts', 'Custom Category Posts', $widget_ops);
	}

	function widget($args, $instance) {
		extract($args, EXTR_SKIP);

		echo $before_widget;
		$cat_id = empty($instance['cat_id']) ? 0 : $instance['cat_id'];
		$items = empty($instance['items']) ? 0 : $instance['items'];
		
		if(empty($items))
		{
			$items = 5;
		}
		
		if(!empty($cat_id))
		{
			pp_cat_posts($cat_id, $items);
		}

		echo $after_widget;
	}

	function update($new_instance, $old_instance) {
		$instance = $old_instance;
		$instance['cat_id'] = strip_tags($new_instance['cat_id']);
		$instance['items'] = strip_tags($new_instance['items']);

		return $instance;
	}

	function form($instance) {
		$instance = wp_parse_args( (array) $instance, array( 'cat_id' => '', 'items' => '') );
		$cat_id = strip_tags($instance['cat_id']);
		$items = strip_tags($instance['items']);
		
		$categories = get_categories('hide_empty=0&orderby=name');
		$wp_cats = array(
			0		=> "Choose a category"
		);
		foreach ($categories as $category_list ) {
			$wp_cats[$category_list->cat_ID] = $category_list->cat_name;
		}

?>
			
			<p><label for="<?php echo $this->get_field_id('cat_id'); ?>">Category: 
				<select  id="<?php echo $this->get_field_id('cat_id'); ?>" name="<?php echo $this->get_field_name('cat_id'); ?>">
				<?php
					foreach($wp_cats as $wp_cat_id => $wp_cat)
					{
				?>
						<option value="<?php echo $wp_cat_id; ?>" <?php if(attribute_escape($cat_id) == $wp_cat_id) { echo 'selected="selected"'; } ?>><?php echo $wp_cat; ?></option>
				<?php
					}
				?>
				</select>
			</label></p>
			
			<p><label for="<?php echo $this->get_field_id('items'); ?>">Items (default 5): <input class="widefat" id="<?php echo $this->get_field_id('items'); ?>" name="<?php echo $this->get_field_name('items'); ?>" type="text" value="<?php echo attribute_escape($items); ?>" /></label></p>
<?php
	}
}

register_widget('Custom_Cat_Posts');

/**
*	End Category Posts Custom Widgets
**/


/**
*	Begin Popular Posts Custom Widgets
**/

class Custom_Popular_Posts extends WP_Widget {
	function Custom_Popular_Posts() {
		$widget_ops = array('classname' => 'Custom_Popular_Posts', 'description' => 'The popular posts with thumbnails' );
		$this->WP_Widget('Custom_Popular_Posts', 'Custom Popular Posts', $widget_ops);
	}

	function widget($args, $instance) {
		extract($args, EXTR_SKIP);

		echo $before_widget;
		$items = empty($instance['items']) ? ' ' : apply_filters('widget_title', $instance['items']);
		
		if(!is_numeric($items))
		{
			$items = 3;
		}
		
		if(!empty($items))
		{
			pp_posts('popular', $items, TRUE);
		}
		
		echo $after_widget;
	}

	function update($new_instance, $old_instance) {
		$instance = $old_instance;
		$instance['items'] = strip_tags($new_instance['items']);

		return $instance;
	}

	function form($instance) {
		$instance = wp_parse_args( (array) $instance, array( 'items' => '') );
		$items = strip_tags($instance['items']);

?>
			<p><label for="<?php echo $this->get_field_id('items'); ?>">Items (default 3): <input class="widefat" id="<?php echo $this->get_field_id('items'); ?>" name="<?php echo $this->get_field_name('items'); ?>" type="text" value="<?php echo attribute_escape($items); ?>" /></label></p>
<?php
	}
}

register_widget('Custom_Popular_Posts');

/**
*	End Popular Posts Custom Widgets
**/

/**
*	Begin Portfolios Custom Widgets
**/

class Custom_Portfolios extends WP_Widget {
	function Custom_Portfolios() {
		$widget_ops = array('classname' => 'Custom_Portfolios', 'description' => 'Portfolio items widget' );
		$this->WP_Widget('Custom_Portfolios', 'Custom Portfolios', $widget_ops);
	}

	function widget($args, $instance) {
		extract($args, EXTR_SKIP);

		echo $before_widget;
		$items = empty($instance['items']) ? ' ' : apply_filters('widget_title', $instance['items']);
		
		if(!is_numeric($items))
		{
			$items = 3;
		}
		
		if(!empty($items))
		{
			$pp_portfolio_sort = get_option('pp_portfolio_sort'); 
			if(empty($pp_portfolio_sort))
			{
				$pp_portfolio_sort = 'ASC';
			}
				
			$portfolio_items = get_posts('numberposts='.$items.'&order='.$pp_portfolio_sort.'&orderby=date&post_type=portfolios');
			
			if(isset($portfolio_items) && !empty($portfolio_items))
			{
				echo '<h2 class="widgettitle">Portfolio</h2>';
			
				foreach($portfolio_items as $key => $portfolio_item)
				{
					$image_url = '';
								
					if(has_post_thumbnail($portfolio_item->ID, 'large'))
					{
					    $image_id = get_post_thumbnail_id($portfolio_item->ID);
					    $image_url = wp_get_attachment_image_src($image_id, 'large', true);
					}
					
					$permalink_url = get_permalink($portfolio_item->ID);
													
					$word_count = 400;
					if(!empty($image_url[0]))
					{
						$word_count = 100;
					}
?>

					<div class="portfolio_grid">

<?php					
					if(!empty($image_url))
					{
?>					
						<div class="portfolio_image">
							<a href="<?php echo $permalink_url; ?>">
								<img src="<?php bloginfo( 'stylesheet_directory' ); ?>/timthumb.php?src=<?php echo $image_url[0]?>&amp;h=200&amp;w=250&amp;zc=1" alt="" class="img_nofade alignleft"/>
							</a>
						</div>
<?php				
					}
?>
						<div class="portfolio_desc">
							<h6 class="cufon"><a href="<?php echo $permalink_url; ?>"><?php echo $portfolio_item->post_title?></a></h6>
							<?php echo pp_substr(strip_tags(strip_shortcodes($portfolio_item->post_content)), $word_count); ?>
						</div>
					</div>
					<br class="clear"/>

<?php
				}
			}
		}
		
		echo $after_widget;
	}

	function update($new_instance, $old_instance) {
		$instance = $old_instance;
		$instance['items'] = strip_tags($new_instance['items']);

		return $instance;
	}

	function form($instance) {
		$instance = wp_parse_args( (array) $instance, array( 'items' => '' ) );
		$items = strip_tags($instance['items']);
		$bg_color = strip_tags($instance['bg_color']);
		if(empty($bg_color))
		{
			$bg_color = 'black';
		}

?>
			<p><label for="<?php echo $this->get_field_id('items'); ?>">Items (default 3): <input class="widefat" id="<?php echo $this->get_field_id('items'); ?>" name="<?php echo $this->get_field_name('items'); ?>" type="text" value="<?php echo attribute_escape($items); ?>" /></label></p>
<?php
	}
}

register_widget('Custom_Portfolios');

/**
*	End Portfolios Posts Custom Widgets
**/

/**
*	Begin Twitter Feed Custom Widgets
**/

class Custom_Twitter extends WP_Widget {
	function Custom_Twitter() {
		$widget_ops = array('classname' => 'Custom_Twitter', 'description' => 'Display your recent Twitter feed' );
		$this->WP_Widget('Custom_Twitter', 'Custom Twitter', $widget_ops);
	}

	function widget($args, $instance) {
		extract($args, EXTR_SKIP);

		echo $before_widget;
		$twitter_username = empty($instance['twitter_username']) ? ' ' : apply_filters('widget_title', $instance['twitter_username']);
		$title = $instance['title'];
		$items = empty($instance['items']) ? ' ' : apply_filters('widget_title', $instance['items']);
		
		if(!is_numeric($items))
		{
			$items = 5;
		}
		
		if(empty($title))
		{
			$title = 'Recent Tweets';
		}
		
		if(!empty($items) && !empty($twitter_username))
		{
			// Begin get user timeline
			include_once (TEMPLATEPATH . "/lib/twitter.lib.php");
			$obj_twitter = new Twitter($twitter_username); 
			$tweets = $obj_twitter->get($items);

			if(!empty($tweets))
			{
				echo '<h2 class="widgettitle">'.$title.'</h2>';
				echo '<ul class="twitter">';
				
				foreach($tweets as $tweet)
				{
					echo '<li>';
					
					if(isset($tweet[0]))
					{
						echo '<a href="'.$tweet[2][0].'">'.$tweet[0].'</a>';
					}
					
					echo '</li>';
				}
				
				echo '</ul>';
			}
		}
		
		echo $after_widget;
	}

	function update($new_instance, $old_instance) {
		$instance = $old_instance;
		$instance['items'] = strip_tags($new_instance['items']);
		$instance['title'] = strip_tags($new_instance['title']);
		$instance['twitter_username'] = strip_tags($new_instance['twitter_username']);

		return $instance;
	}

	function form($instance) {
		$instance = wp_parse_args( (array) $instance, array( 'items' => '', 'twitter_username' => '', 'title' => '') );
		$items = strip_tags($instance['items']);
		$twitter_username = strip_tags($instance['twitter_username']);
		$title = strip_tags($instance['title']);

?>
			<p><label for="<?php echo $this->get_field_id('twitter_username'); ?>">Twitter Username: <input class="widefat" id="<?php echo $this->get_field_id('twitter_username'); ?>" name="<?php echo $this->get_field_name('twitter_username'); ?>" type="text" value="<?php echo attribute_escape($twitter_username); ?>" /></label></p>
			
			<p><label for="<?php echo $this->get_field_id('title'); ?>">Title: <input class="widefat" id="<?php echo $this->get_field_id('title'); ?>" name="<?php echo $this->get_field_name('title'); ?>" type="text" value="<?php echo attribute_escape($title); ?>" /></label></p>

			<p><label for="<?php echo $this->get_field_id('items'); ?>">Items (default 5): <input class="widefat" id="<?php echo $this->get_field_id('items'); ?>" name="<?php echo $this->get_field_name('items'); ?>" type="text" value="<?php echo attribute_escape($items); ?>" /></label></p>
<?php
	}
}

register_widget('Custom_Twitter');

/**
*	End Twitter Feed Custom Widgets
**/

/**
*	Begin Contact Form Custom Widgets
**/

class Custom_Contact_Form extends WP_Widget {
	function Custom_Contact_Form() {
		$widget_ops = array('classname' => 'Custom_Contact_Form', 'description' => 'Display contact form' );
		$this->WP_Widget('Custom_Contact_Form', 'Custom Contact Form', $widget_ops);
	}

	function widget($args, $instance) {
		extract($args, EXTR_SKIP);

		echo $before_widget;
		$title = empty($instance['title']) ? '' : apply_filters('widget_title', $instance['title']);
		
		if(empty($title))
		{
			$title = 'Contact Us';
		}
		
		$pp_contact_email = get_option('pp_contact_email');
		
		echo '<h2 class="widgettitle">'.$title.'</h2>';
?>

			<form id="contact_form" method="post" action="<?php bloginfo( 'stylesheet_directory' ); ?>/lib/api.lib.php">
				<input type="hidden" id="contact_email" name="contact_email" value="<?php echo get_option('pp_contact_email'); ?>"/>
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
				    <textarea id="message" name="message" rows="2" cols="10" style="width:94%;height:60px"></textarea>
				</p>
				<p style="margin-top:20px">
				    <input type="submit" value="Send Message"/><br/>
				</p>
			</form>
			<div id="reponse_msg"></div>
		
<?php
		echo $after_widget;
	}

	function update($new_instance, $old_instance) {
		$instance = $old_instance;
		$instance['title'] = strip_tags($new_instance['title']);

		return $instance;
	}

	function form($instance) {
		$instance = wp_parse_args( (array) $instance, array( 'items' => '') );
		$title = strip_tags($instance['title']);

?>
			
			<p><label for="<?php echo $this->get_field_id('title'); ?>">Title: <input class="widefat" id="<?php echo $this->get_field_id('title'); ?>" name="<?php echo $this->get_field_name('title'); ?>" type="text" value="<?php echo attribute_escape($title); ?>" /></label></p>
<?php
	}
}

register_widget('Custom_Contact_Form');

/**
*	End Contact Form Custom Widgets
**/

/**
*	Begin Map Custom Widgets
**/

class Custom_Map extends WP_Widget {
	function Custom_Map() {
		$widget_ops = array('classname' => 'Custom_Map', 'description' => 'Display map' );
		$this->WP_Widget('Custom_Map', 'Custom Map', $widget_ops);
	}

	function widget($args, $instance) {
		extract($args, EXTR_SKIP);

		echo $before_widget;
		$title = empty($instance['title']) ? 'Map' : $instance['title'];
		$width = empty($instance['width']) ? 240 : $instance['width'];
		$height = empty($instance['height']) ? 240 : $instance['height'];
		$lat = empty($instance['lat']) ? 0 : $instance['lat'];
		$long = empty($instance['long']) ? 0 : $instance['long'];
		
		$custom_id = time().rand();
		
		echo '<h2 class="widgettitle">'.$title.'</h2><br/>';
		
		$marker = '';
		if(!empty($lat) && !empty($long))
		{
			$marker = '{ zoom: 12, markers: [ { latitude: '.$lat.', longitude: '.$long.' } ] }';
		}
?>

			<div id="map<?php echo $custom_id; ?>" style="width:<?php echo $width; ?>px;height:<?php echo $height; ?>px;margin-bottom:15px"></div>
			<script>
				$j(document).ready(function(){ 
					$j("#map<?php echo $custom_id; ?>").gMap(<?php echo $marker; ?>);
				});
			</script>
		
<?php
		echo $after_widget;
	}

	function update($new_instance, $old_instance) {
		$instance = $old_instance;
		$instance['title'] = strip_tags($new_instance['title']);
		$instance['width'] = strip_tags($new_instance['width']);
		$instance['height'] = strip_tags($new_instance['height']);
		$instance['lat'] = strip_tags($new_instance['lat']);
		$instance['long'] = strip_tags($new_instance['long']);

		return $instance;
	}

	function form($instance) {
		$instance = wp_parse_args( (array) $instance, array( 'items' => '', 'title' => '', 'width' => '', 'height' => '', 'lat' => '', 'long' => '') );
		$title = strip_tags($instance['title']);
		$width = strip_tags($instance['width']);
		$height = strip_tags($instance['height']);
		$lat = strip_tags($instance['lat']);
		$long = strip_tags($instance['long']);

?>
			
			<p><label for="<?php echo $this->get_field_id('title'); ?>">Title: <input class="widefat" id="<?php echo $this->get_field_id('title'); ?>" name="<?php echo $this->get_field_name('title'); ?>" type="text" value="<?php echo attribute_escape($title); ?>" /></label></p>
			
			<p><label for="<?php echo $this->get_field_id('width'); ?>">Width: <input class="widefat" id="<?php echo $this->get_field_id('width'); ?>" name="<?php echo $this->get_field_name('width'); ?>" type="text" value="<?php echo attribute_escape($width); ?>" /></label></p>
			
			<p><label for="<?php echo $this->get_field_id('height'); ?>">Height: <input class="widefat" id="<?php echo $this->get_field_id('height'); ?>" name="<?php echo $this->get_field_name('height'); ?>" type="text" value="<?php echo attribute_escape($height); ?>" /></label></p>
			
			<p><label for="<?php echo $this->get_field_id('lat'); ?>">Latitude (<a href="http://www.tech-recipes.com/rx/5519/the-easy-way-to-find-latitude-and-longitude-values-in-google-maps/">Find here</a>): <input class="widefat" id="<?php echo $this->get_field_id('lat'); ?>" name="<?php echo $this->get_field_name('lat'); ?>" type="text" value="<?php echo attribute_escape($lat); ?>" /></label></p>
			
			<p><label for="<?php echo $this->get_field_id('long'); ?>">Longitude (<a href="http://www.tech-recipes.com/rx/5519/the-easy-way-to-find-latitude-and-longitude-values-in-google-maps/">Find here</a>): <input class="widefat" id="<?php echo $this->get_field_id('long'); ?>" name="<?php echo $this->get_field_name('long'); ?>" type="text" value="<?php echo attribute_escape($long); ?>" /></label></p>
<?php
	}
}

register_widget('Custom_Map');

/**
*	End Map Custom Widgets
**/


/**
*	Begin Flickr Feed Custom Widgets
**/

class Custom_Flickr extends WP_Widget {
	function Custom_Flickr() {
		$widget_ops = array('classname' => 'Custom_Flickr', 'description' => 'Display your recent Flickr photos' );
		$this->WP_Widget('Custom_Flickr', 'Custom Flickr', $widget_ops);
	}

	function widget($args, $instance) {
		extract($args, EXTR_SKIP);

		echo $before_widget;
		$flickr_id = empty($instance['flickr_id']) ? ' ' : apply_filters('widget_title', $instance['flickr_id']);
		$title = $instance['title'];
		$items = $instance['items'];
		
		if(!is_numeric($items))
		{
			$items = 9;
		}
		
		if(empty($title))
		{
			$title = 'Photostream';
		}
		
		if(!empty($items) && !empty($flickr_id))
		{
			$photos_arr = get_flickr(array('type' => 'user', 'id' => $flickr_id, 'items' => $items));
			
			if(!empty($photos_arr))
			{
				echo '<h2 class="widgettitle">'.$title.'</h2>';
				echo '<ul class="flickr">';
				
				foreach($photos_arr as $photo)
				{
					echo '<li>';
					echo '<a href="'.$photo['url'].'" title="'.$photo['title'].'"><img src="'.$photo['thumb_url'].'" alt="" class="img_nofade" /></a>';
					echo '</li>';
				}
				
				echo '</ul><br class="clear"/><br class="clear"/>';
			}
		}
		
		echo $after_widget;
	}

	function update($new_instance, $old_instance) {
		$instance = $old_instance;
		$instance['items'] = strip_tags($new_instance['items']);
		$instance['title'] = strip_tags($new_instance['title']);
		$instance['flickr_id'] = strip_tags($new_instance['flickr_id']);

		return $instance;
	}

	function form($instance) {
		$instance = wp_parse_args( (array) $instance, array( 'items' => '', 'flickr_id' => '', 'title' => '') );
		$items = strip_tags($instance['items']);
		$flickr_id = strip_tags($instance['flickr_id']);
		$title = strip_tags($instance['title']);

?>
			<p><label for="<?php echo $this->get_field_id('flickr_id'); ?>">Flickr ID <a href="http://idgettr.com/">Find your Flickr ID here</a>: <input class="widefat" id="<?php echo $this->get_field_id('flickr_id'); ?>" name="<?php echo $this->get_field_name('flickr_id'); ?>" type="text" value="<?php echo attribute_escape($flickr_id); ?>" /></label></p>
			
			<p><label for="<?php echo $this->get_field_id('title'); ?>">Title: <input class="widefat" id="<?php echo $this->get_field_id('title'); ?>" name="<?php echo $this->get_field_name('title'); ?>" type="text" value="<?php echo attribute_escape($title); ?>" /></label></p>

			<p><label for="<?php echo $this->get_field_id('items'); ?>">Items (default 9): <input class="widefat" id="<?php echo $this->get_field_id('items'); ?>" name="<?php echo $this->get_field_name('items'); ?>" type="text" value="<?php echo attribute_escape($items); ?>" /></label></p>
<?php
	}
}

register_widget('Custom_Flickr');

/**
*	End Flickr Feed Custom Widgets
**/

?>