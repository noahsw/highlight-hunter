<?php

// [dropcap foo="foo-value"]
function dropcap_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'style' => 1
	), $atts));
	
	//get first char
	$first_char = substr($content, 0, 1);
	$text_len = strlen($content);
	$rest_text = substr($content, 1, $text_len);

	$return_html = '<span class="dropcap'.$style.'">'.$first_char.'</span>';
	$return_html.= do_shortcode($rest_text);
	
	return $return_html;
}
add_shortcode('dropcap', 'dropcap_func');




// [quote foo="foo-value"]
function quote_func($atts, $content) {
	
	$return_html = '<blockquote>'.do_shortcode($content).'</blockquote>';
	
	return $return_html;
}
add_shortcode('quote', 'quote_func');


function pre_func($atts, $content) {
	
	$return_html = '<pre>'.strip_tags($content).'</pre>';
	
	return $return_html;
}
add_shortcode('pre', 'pre_func');



// [button foo="foo-value"]
function button_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'href' => '',
		'align' => 'left',
		'bg_color' => '#cccccc',
		'text_color' => '#444444',
		'size' => 'small',
		'style' => '',
		'color' => '',
		'target' => '_self',
	), $atts));
	
	if(!empty($color))
	{
		switch(strtolower($color))
		{
			case 'black':
				$bg_color = '#000000';
				$text_color = '#ffffff';
			break;
			case 'gray':
				$bg_color = '#666666';
				$text_color = '#ffffff';
			break;
			case 'white':
				$bg_color = '#f5f5f5';
				$text_color = '#444444';
			break;
			case 'blue':
				$bg_color = '#004a80';
				$text_color = '#ffffff';
			break;
			case 'yellow':
				$bg_color = '#f9b601';
				$text_color = '#ffffff';
			break;
			case 'red':
				$bg_color = '#9e0b0f';
				$text_color = '#ffffff';
			break;
			case 'orange':
				$bg_color = '#fe7201';
				$text_color = '#ffffff';
			break;
			case 'green':
				$bg_color = '#7aad34';
				$text_color = '#ffffff';
			break;
			case 'pink':
				$bg_color = '#d2027d';
				$text_color = '#ffffff';
			break;
			case 'purple':
				$bg_color = '#582280';
				$text_color = '#ffffff';
			break;
		}
	}
	
	$bg_color_light = '#'.hex_lighter(substr($bg_color, 1), 35);
	$border_color = '#'.hex_darker(substr($bg_color, 1), 10);
	
	$return_html = '<input type="button" class="button '.$size.' '.$align.'" style="background: -webkit-gradient(linear, left top, left bottom, from('.$bg_color_light.'), to('.$bg_color.'));background: -moz-linear-gradient(top,  '.$bg_color_light.',  '.$bg_color.');filter:  progid:DXImageTransform.Microsoft.gradient(startColorstr=\''.$bg_color_light.'\', endColorstr=\''.$bg_color.'\');border:1px solid '.$border_color.';color:'.$text_color.';'.$style.'" value="'.$content.'"';
	
	if(!empty($href))
	{
		$return_html.= ' onclick="window.open(\''.$href.'\', \''.$target.'\')"';
	}
	
	$return_html.= '/>';
	
	return $return_html;
}
add_shortcode('button', 'button_func');



function lightbox_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'title' => '',
		'href' => '',
		'type' => 'image',
		'youtube_id' => '',
		'vimeo_id' => '',
		'dailymotion_id' => '',
	), $atts));
	
	$class = 'lightbox';
	
	if($type != 'image')
	{
		$class.= '_'.$type;
	}
	
	if($type == 'youtube')
	{
		$href = '#video_'.$youtube_id;
	}
	
	if($type == 'vimeo')
	{
		$href = '#video_'.$vimeo_id;
	}
	
	if($type == 'dailymotion')
	{
		$href = '#video_'.$dailymotion_id;
	}
	
	$return_html.= '<a href="'.$href.'" class="'.$class.'">'.do_shortcode($content).'</a>';
	
	if(!empty($youtube_id))
	{
		$return_html.= '<div style="display:none;">
							    <div id="video_'.$youtube_id.'" style="width:640px;height:385px">
							        
							        <object type="application/x-shockwave-flash" data="http://www.youtube.com/v/'.$youtube_id.'" style="width:640px;height:385px">
			        		    		<param name="movie" value="http://www.youtube.com/v/'.$youtube_id.'" />
			    			    	</object>
							        
							    </div>	
							</div>';
	}
	
	if(!empty($vimeo_id))
	{
		$return_html.= '<div style="display:none;">
							    <div id="video_'.$vimeo_id.'" style="width:601px;height:338px">
							    
							        <object width="601" height="338" data="http://vimeo.com/moogaloop.swf?clip_id='.$vimeo_id.'&amp;server=vimeo.com&amp;show_title=0&amp;show_byline=0&amp;show_portrait=0&amp;color=ffffff&amp;fullscreen=1" type="application/x-shockwave-flash">
			  				    		<param name="allowfullscreen" value="true" />
			  				    		<param name="allowscriptaccess" value="always" />
			  				    		<param name="movie" value="http://vimeo.com/moogaloop.swf?clip_id='.$vimeo_id.'&amp;server=vimeo.com&amp;show_title=0&amp;show_byline=0&amp;show_portrait=0&amp;color=ffffff&amp;fullscreen=1" />
							    	</object>
							        
							    </div>	
							</div>';
	}
	
	if(!empty($dailymotion_id))
	{
		$return_html.= '<div style="display:none;">
							    <div id="video_'.$dailymotion_id.'" style="width:601px;height:338px">
							    
							        <iframe frameborder="0" width="601" height="338" src="http://www.dailymotion.com/embed/video/'.$dailymotion_id.'?width=560&theme=default&foreground=%23F7FFFD&highlight=%23FFC300&background=%23171D1B&start=&animatedTitle=&iframe=1&additionalInfos=0&autoPlay=0&hideInfos=0"></iframe>
							        
							    </div>	
							</div>';
	}
	
	return $return_html;
}
add_shortcode('lightbox', 'lightbox_func');


function styled_box_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'title' => '',
		'width' => '95%',
		'style' => '',
		'color' => '',
	), $atts));
	
	switch(strtolower($color))
		{
			case 'black':
				$bg_color = '#000000';
				$text_color = '#ffffff';
			break;
			default:
			case 'gray':
				$bg_color = '#666666';
				$text_color = '#ffffff';
			break;
			case 'white':
				$bg_color = '#f5f5f5';
				$text_color = '#444444';
			break;
			case 'blue':
				$bg_color = '#004a80';
				$text_color = '#ffffff';
			break;
			case 'yellow':
				$bg_color = '#f9b601';
				$text_color = '#ffffff';
			break;
			case 'red':
				$bg_color = '#9e0b0f';
				$text_color = '#ffffff';
			break;
			case 'orange':
				$bg_color = '#fe7201';
				$text_color = '#ffffff';
			break;
			case 'green':
				$bg_color = '#7aad34';
				$text_color = '#ffffff';
			break;
			case 'pink':
				$bg_color = '#d2027d';
				$text_color = '#ffffff';
			break;
			case 'purple':
				$bg_color = '#582280';
				$text_color = '#ffffff';
			break;
		}
	
	$bg_color_light = '#'.hex_lighter(substr($bg_color, 1), 20);
	$border_color = '#'.hex_lighter(substr($bg_color, 1), 10);
	
	$return_html = '<div class="styled_box_title" style="background: -webkit-gradient(linear, left top, left bottom, from('.$bg_color_light.'), to('.$bg_color.'));background: -moz-linear-gradient(top,  '.$bg_color_light.',  '.$bg_color.');filter:  progid:DXImageTransform.Microsoft.gradient(startColorstr=\''.$bg_color_light.'\', endColorstr=\''.$bg_color.'\');border:1px solid '.$border_color.';color:'.$text_color.';width:'.$width.';'.$style.'">'.$title.'</div>';
	$return_html.= '<div class="styled_box_content" style="border:1px solid '.$border_color.';border-top:0;width:'.$width.'">'.html_entity_decode(do_shortcode($content)).'</div>';
	
	return $return_html;
}
add_shortcode('styled_box', 'styled_box_func');


function frame_left_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'src' => '',
		'href' => '',
	), $atts));
	
	$return_html = '<div class="frame_left">';
	
	if(!empty($href))
	{
		$return_html.= '<a href="'.$href.'" class="img_frame">';
	}
	
	$return_html.= '<img src="'.$src.'" alt=""/>';
	
	if(!empty($href))
	{
		$return_html.= '</a>';
	}
	
	if(!empty($content))
	{
		$return_html.= '<span class="caption">'.$content.'</span>';
	}
	
	$return_html.= '</div>';
	
	return $return_html;
}
add_shortcode('frame_left', 'frame_left_func');




function frame_right_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'src' => '',
		'href' => '',
	), $atts));
	
	$return_html = '<div class="frame_right">';
	
	if(!empty($href))
	{
		$return_html.= '<a href="'.$href.'" class="img_frame">';
	}
	
	$return_html.= '<img src="'.$src.'" alt=""/>';
	
	if(!empty($href))
	{
		$return_html.= '</a>';
	}
	
	if(!empty($content))
	{
		$return_html.= '<span class="caption">'.$content.'</span>';
	}
	
	$return_html.= '</div>';
	
	return $return_html;
}
add_shortcode('frame_right', 'frame_right_func');



function frame_center_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'src' => '',
		'href' => '',
	), $atts));
	
	$return_html = '<div class="frame_center">';
	
	if(!empty($href))
	{
		$return_html.= '<a href="'.$href.'" class="img_frame">';
	}
	
	$return_html.= '<img src="'.$src.'" alt=""/>';
	
	if(!empty($href))
	{
		$return_html.= '</a>';
	}
	
	if(!empty($content))
	{
		$return_html.= '<span class="caption">'.$content.'</span>';
	}
	
	$return_html.= '</div>';
	
	return $return_html;
}
add_shortcode('frame_center', 'frame_center_func');



function list_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'type' => '',
	), $atts));
	
	$return_html = '
		<style>
			.pp_list.'.$type.' ul li {
				display: block;
				background: url("'.get_bloginfo( 'stylesheet_directory' ).'/images/icon/'.$type.'_16x16.png") no-repeat top left;
			}
		</style>
	';
	
	$return_html.= '<div class="pp_list '.$type.'">'.strip_tags($content,'<ul><li><a>').'</div>';
	
	return $return_html;
}
add_shortcode('list', 'list_func');



function highlight_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'type' => 'yellow',
	), $atts));
	
	$return_html.= '<span class="highlight_'.$type.'">'.strip_tags($content).'</span>';
	
	return $return_html;
}
add_shortcode('highlight', 'highlight_func');



function tagline_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'title' => '',
		'button' => '',
		'href' => '',
	), $atts));
	
	$return_html.= '
		<div class="tagline" style="width:92%">
			<div class="tagline_text">
			    <h2 class="cufon">'.$title.'</h2>
			    <p>'.strip_tags(strip_shortcodes($content)).'</p>
			</div>
			<div class="tagline_button">
			    <a href="'.$href.'" class="button medium">'.$button.'</a>
			</div>
			<br class="clear"/>
		</div>
	';
	
	return $return_html;
}
add_shortcode('tagline', 'tagline_func');



function arrow_list_func($atts, $content) {
	
	$return_html = '<ul class="arrow_list">'.html_entity_decode(strip_tags($content,'<li><a>')).'</ul>';
	
	return $return_html;
}
add_shortcode('arrow_list', 'arrow_list_func');




function check_list_func($atts, $content) {
	
	$return_html = '<ul class="check_list">'.html_entity_decode(strip_tags($content,'<li><a>')).'</ul>';
	
	return $return_html;
}
add_shortcode('check_list', 'check_list_func');




function star_list_func($atts, $content) {
	
	$return_html = '<ul class="star_list">'.html_entity_decode(strip_tags($content,'<li><a>')).'</ul>';
	
	return $return_html;
}
add_shortcode('star_list', 'star_list_func');



function one_half_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'class' => '',
	), $atts));
	
	$return_html = '<div class="one_half '.$class.'">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_half', 'one_half_func');




function one_half_last_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'class' => '',
	), $atts));
	
	$return_html = '<div class="one_half last '.$class.'">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_half_last', 'one_half_last_func');



function one_third_func($atts, $content) {
	
	$return_html = '<div class="one_third">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_third', 'one_third_func');




function one_third_last_func($atts, $content) {
	
	$return_html = '<div class="one_third last">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_third_last', 'one_third_last_func');



function two_third_func($atts, $content) {
	
	$return_html = '<div class="two_third">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('two_third', 'two_third_func');




function two_third_last_func($atts, $content) {
	
	$return_html = '<div class="two_third last">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('two_third_last', 'two_third_last_func');




function one_fourth_func($atts, $content) {
	
	$return_html = '<div class="one_fourth">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_fourth', 'one_fourth_func');




function one_fourth_last_func($atts, $content) {
	
	$return_html = '<div class="one_fourth last">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_fourth_last', 'one_fourth_last_func');



function one_fifth_func($atts, $content) {
	
	$return_html = '<div class="one_fifth">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_fifth', 'one_fifth_func');




function one_fifth_last_func($atts, $content) {
	
	$return_html = '<div class="one_fifth last">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_fifth_last', 'one_fifth_last_func');



function one_sixth_func($atts, $content) {
	
	$return_html = '<div class="one_sixth">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_sixth', 'one_sixth_func');




function one_sixth_last_func($atts, $content) {
	
	$return_html = '<div class="one_sixth last">'.do_shortcode($content).'</div>';
	
	return $return_html;
}
add_shortcode('one_sixth_last', 'one_sixth_last_func');



function pp_gallery_func($atts, $content) {

	//extract short code attr
	/*extract(shortcode_atts(array(
		'src' => '',
	), $atts));*/
	
	$return_html = '<div class="pp_gallery">'.html_entity_decode(strip_tags($content,'<img><a>')).'</div>';
	
	return $return_html;
}
add_shortcode('pp_gallery', 'pp_gallery_func');



function accordion_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'title' => '',
		'close' => 0,
	), $atts));
	
	$close_class = '';
	
	if(!empty($close))
	{
		$close_class = 'pp_accordion_close';
	}
	
	$return_html = '<div class="pp_accordion '.$close_class.'"><h3><a href="#">'.$title.'</a></h3>';
	$return_html.= '<div><p>';
	$return_html.= do_shortcode($content);
	$return_html.= '</p></div></div><br class="clear"/>';
	
	return $return_html;
}
add_shortcode('accordion', 'accordion_func');



function tabs_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'tab1' => '',
		'tab2' => '',
		'tab3' => '',
		'tab4' => '',
		'tab5' => '',
		'tab6' => '',
		'tab7' => '',
		'tab8' => '',
		'tab9' => '',
		'tab10' => '',
	), $atts));
	
	$tab_arr = array(
		$tab1,
		$tab2,
		$tab3,
		$tab4,
		$tab5,
		$tab6,
		$tab7,
		$tab8,
		$tab9,
		$tab10,
	);
	
	$return_html = '<div class="tabs"><ul>';
	
	foreach($tab_arr as $key=>$tab)
	{
		//display title1
		if(!empty($tab))
		{
			$return_html.= '<li><a href="#tabs-'.($key+1).'">'.$tab.'</a></li>';
		}
	}
	
	$return_html.= '</ul>';
	$return_html.= do_shortcode($content);
	$return_html.= '</div>';
	
	return $return_html;
}
add_shortcode('tabs', 'tabs_func');


function tab_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'id' => '',
	), $atts));
	
	$return_html.= '<div id="tabs-'.$id.'" class="tab_wrapper"><br class="clear"/>'.do_shortcode($content).'<br class="clear"/></div>';
	
	return $return_html;
}
add_shortcode('tab', 'tab_func');



function recent_posts_func($atts) {
	//extract short code attr
	extract(shortcode_atts(array(
		'items' => 3,
	), $atts));

	$return_html = pp_posts('recent', $items, FALSE, 'black', FALSE);
	
	return $return_html;
}
add_shortcode('recent_posts', 'recent_posts_func');



function popular_posts_func($atts) {
	//extract short code attr
	extract(shortcode_atts(array(
		'items' => 3,
	), $atts));

	$return_html = pp_posts('poopular', $items, FALSE, 'black', FALSE);
	
	return $return_html;
}
add_shortcode('popular_posts', 'popular_posts_func');


/**
*	Begin Portfolio slider shortcodes
**/

function slide_img_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'src' => '',
	), $atts));

	if(empty($pp_slider_height))
	{
		$pp_slider_height = 405;
	}
	
	$pp_slider_height_offset = $pp_slider_height - 405;
	
	$return_html = '<li>';
	$return_html.= '<img src="'.get_bloginfo( 'stylesheet_directory' ).'/timthumb.php?src='.$src.'&h='.intval(400+$pp_slider_height_offset).'&w=939&zc=1" alt=""/>';
	$return_html.= '</li>'. PHP_EOL;
	
	return $return_html;
}
add_shortcode('slide_img', 'slide_img_func');


function slide_vimeo_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'video_id' => '',
	), $atts));

	if(empty($pp_slider_height))
	{
		$pp_slider_height = 405;
	}
	
	$pp_slider_height_offset = $pp_slider_height - 405;
	
	$return_html = '<li>';
	$return_html.= '<object width="939" height="'.intval(400+$pp_slider_height_offset).'"><param name="allowfullscreen" value="true" /><param name="wmode" value="opaque"><param name="allowscriptaccess" value="always" /><param name="movie" value="http://vimeo.com/moogaloop.swf?clip_id='.$video_id.'&amp;server=vimeo.com&amp;show_title=0&amp;show_byline=0&amp;show_portrait=0&amp;color=00ADEF&amp;fullscreen=1" /><embed src="http://vimeo.com/moogaloop.swf?clip_id='.$video_id.'&amp;server=vimeo.com&amp;show_title=0&amp;show_byline=0&amp;show_portrait=0&amp;color=00ADEF&amp;fullscreen=1" type="application/x-shockwave-flash" allowfullscreen="true" allowscriptaccess="always" width="939" height="'.intval(400+$pp_slider_height_offset).'" wmode="transparent"></embed></object>';
	$return_html.= '</li>'. PHP_EOL;
	
	return $return_html;
}
add_shortcode('slide_vimeo', 'slide_vimeo_func');


function slide_youtube_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'video_id' => '',
	), $atts));

	if(empty($pp_slider_height))
	{
		$pp_slider_height = 405;
	}
	
	$pp_slider_height_offset = $pp_slider_height - 405;
	
	$return_html = '<li>';
	$return_html.= '<object type="application/x-shockwave-flash" data="http://www.youtube.com/v/'.$video_id.'&hd=1" style="width:939px;height:'.intval(400+$pp_slider_height_offset).'px"><param name="wmode" value="opaque"><param name="movie" value="http://www.youtube.com/v/'.$video_id.'&hd=1" /></object>';
	$return_html.= '</li>'. PHP_EOL;
	
	return $return_html;
}
add_shortcode('slide_youtube', 'slide_youtube_func');

/**
*	End Portfolio slider shortcodes
**/


function pricing_func($atts, $content) {
	
	//extract short code attr
	extract(shortcode_atts(array(
		'size' => '',
		'title' => '',
		'column' => 3,
	), $atts));
	
	$width_class = 'three';
	switch($column)
	{
		case 3:
			$width_class = 'three';
		break;
		case 4:
			$width_class = 'four';
		break;
		case 5:
			$width_class = 'five';
		break;
	}
	
	$return_html = '<div class="pricing_box '.$size.' '.$width_class.'">';
	
	if(!empty($title))
	{
		$return_html.= '<div class="header">';
		$return_html.= '<span>'.$title.'</span>';
		$return_html.= '</div><br/>';
	}
	
	$return_html.= do_shortcode($content);
	$return_html.= '</div>';
	
	return $return_html;
}
add_shortcode('pricing', 'pricing_func');

function map_func($atts) {

	//extract short code attr
	extract(shortcode_atts(array(
		'width' => 400,
		'height' => 300,
		'lat' => 0,
		'long' => 0,
		'zoom' => 12,
		'type' => '',
		'popup' => '',
		'address' => '',
	), $atts));
	
	$custom_id = time().rand();
	
	$marker = '{';
	
	if((!empty($lat) && !empty($long)) OR (!empty($address)))
	{
		if(!empty($lat) && !empty($long))
		{
			$marker.= 'markers: [ { latitude: '.$lat.', longitude: '.$long;
		}
		elseif(!empty($address))
		{
			$marker.= 'markers: [ { address: "'.$address.'"';
		}
		
		if(!empty($popup))
		{
			$marker.= ', html: "'.$popup.'", popup: false';
		}
		
		$marker.= '} ], ';
	}
	
	if(!empty($type))
	{
		$marker.= 'maptype: '.$type.',';
	}
	
	$marker.= 'zoom: '.$zoom;
	$marker.= '}';
	
	$return_html = '<div id="map'.$custom_id.'" style="width:'.$width.'px;height:'.$height.'px"></div>';
	$return_html.= '<script>';
	$return_html.= 'jQuery(document).ready(function(){ jQuery("#map'.$custom_id.'").gMap('.$marker.'); });';
	$return_html.= '</script>';
	
	return $return_html;
}
add_shortcode('map', 'map_func');


function youtube_func($atts) {

	//extract short code attr
	extract(shortcode_atts(array(
		'width' => 640,
		'height' => 385,
		'video_id' => '',
	), $atts));
	
	$custom_id = time().rand();
	
	$return_html = '<object type="application/x-shockwave-flash" data="http://www.youtube.com/v/'.$video_id.'&hd=1" style="width:'.$width.'px;height:'.$height.'px"><param name="wmode" value="opaque"><param name="movie" value="http://www.youtube.com/v/'.$video_id.'&hd=1" /></object>';
	
	return $return_html;
}
add_shortcode('youtube', 'youtube_func');


function vimeo_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'width' => 640,
		'height' => 385,
		'video_id' => '',
	), $atts));
	
	$custom_id = time().rand();
	
	$return_html = '<object width="'.$width.'" height="'.$height.'"><param name="allowfullscreen" value="true" /><param name="wmode" value="opaque"><param name="allowscriptaccess" value="always" /><param name="movie" value="http://vimeo.com/moogaloop.swf?clip_id='.$video_id.'&amp;server=vimeo.com&amp;show_title=0&amp;show_byline=0&amp;show_portrait=0&amp;color=00ADEF&amp;fullscreen=1" /><embed src="http://vimeo.com/moogaloop.swf?clip_id='.$video_id.'&amp;server=vimeo.com&amp;show_title=0&amp;show_byline=0&amp;show_portrait=0&amp;color=00ADEF&amp;fullscreen=1" type="application/x-shockwave-flash" allowfullscreen="true" allowscriptaccess="always" width="'.$width.'" height="'.$height.'" wmode="transparent"></embed></object>';
	
	return $return_html;
}
add_shortcode('vimeo', 'vimeo_func');


function dailymotion_func($atts) {

	//extract short code attr
	extract(shortcode_atts(array(
		'width' => 640,
		'height' => 385,
		'video_id' => '',
	), $atts));
	
	$custom_id = time().rand();
	
	$return_html = '<iframe frameborder="0" width="'.$width.'" height="'.$height.'" src="http://www.dailymotion.com/embed/video/'.$video_id.'?width=560&theme=default&foreground=%23F7FFFD&highlight=%23FFC300&background=%23171D1B&start=&animatedTitle=&iframe=1&additionalInfos=0&autoPlay=0&hideInfos=0"></iframe>';
	
	return $return_html;
}
add_shortcode('dailymotion', 'dailymotion_func');


function html5video_func($atts) {

	//extract short code attr
	extract(shortcode_atts(array(
		'width' => 640,
		'height' => 385,
		'poster' => '',
		'mp4' => '',
		'webm' => '',
		'ogg' => '',
	), $atts));
	
	$custom_id = time().rand();
	
	$return_html = '<div class="video-js-box vim-css"> 
    <video id="example_video_1" class="video-js" width="'.$width.'" height="'.$height.'" controls="controls" preload="auto" poster="'.$poster.'"> 
      <source src="'.$mp4.'" type=\'video/mp4; codecs="avc1.42E01E, mp4a.40.2"\' /> 
      <source src="'.$webm.'" type=\'video/webm; codecs="vp8, vorbis"\' /> 
      <source src="'.$ogg.'" type=\'video/ogg; codecs="theora, vorbis"\' /> 
      <object id="flash_fallback_1" class="vjs-flash-fallback" width="640" height="264" type="application/x-shockwave-flash"
        data="http://releases.flowplayer.org/swf/flowplayer-3.2.1.swf"> 
        <param name="movie" value="http://releases.flowplayer.org/swf/flowplayer-3.2.1.swf" /> 
        <param name="allowfullscreen" value="true" /> 
        <param name="flashvars" value=\'config={"playlist":["'.$poster.'", {"url": "'.$mp4.'","autoPlay":false,"autoBuffering":true}]}\' /> 
        <img src="'.$poster.'" width="640" height="264" alt="Poster Image"
          title="No video playback capabilities." /> 
      </object> 
    </video> 
  </div> ';
	
	return $return_html;
}
add_shortcode('html5video', 'html5video_func');


function slideshow_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'width' => 400,
		'height' => 300,
	), $atts));
	
	$content = trim($content);
	$image_arr = preg_split("/(\r?\n)/", $content);

	$return_html = '<div class="slideshow" style="width:'.$width.'px;height:'.$height.'px"><div class="wrapper" style="width:'.$width.'px;height:'.intval($height+25).'px"><ul>';

	if(!empty($image_arr) && is_array($image_arr))
	{
		foreach($image_arr as $image)
		{
			$image = trim(strip_tags($image));
			
			if(!empty($image))
			{
				$return_html.= '<li>';
				$return_html.= '<img src="'.get_bloginfo( 'stylesheet_directory' ).'/timthumb.php?src='.strip_tags($image).'&amp;h='.$height.'&amp;w='.$width.'&amp;zc=1" alt=""/>';
				$return_html.= '</li>'. PHP_EOL;
			}
		}
	}
	
	$return_html.= '</ul></div></div><br class="clear"/><br class="clear"/>';
	
	return $return_html;
}
add_shortcode('slideshow', 'slideshow_func');


function nivoslide_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'width' => 400,
		'height' => 300,
		'effect' => 'sliceDown',
		'pauseTime' => 5,
	), $atts));
	
	$content = trim($content);
	$image_arr = preg_split("/(\r?\n)/", $content);

	$rand_id = mt_rand();
	$return_html = '<div class="nivo_border" style="width:'.$width.'px;height:'.$height.'px;"><div id="'.$rand_id.'" class="nivoslide" style="width:'.$width.'px;height:'.$height.'px; visibility: hidden">';

	if(!empty($image_arr) && is_array($image_arr))
	{
		foreach($image_arr as $image)
		{
			$image = trim(strip_tags($image));
			
			if(!empty($image))
			{
				$return_html.= '<img src="'.get_bloginfo( 'stylesheet_directory' ).'/timthumb.php?src='.strip_tags($image).'&amp;h='.$height.'&amp;w='.$width.'&amp;zc=1" alt=""/>';
			}
		}
	}
	
	$return_html.= '</div></div><br class="clear"/>';
	
	$return_html.= "<script>jQuery(window).load(function() { jQuery('#".$rand_id."').nivoSlider({ pauseTime: ".intval($pauseTime*1000).", pauseOnHover: true, effect: '".$effect."', controlNav: true, captionOpacity: 1, directionNavHide: false, controlNavThumbs: true, controlNavThumbsFromRel:false, afterLoad: function(){ 
		jQuery('#".$rand_id."').css('visibility', 'visible');
	} }); });</script>";
	
	return $return_html;
}
add_shortcode('nivoslide', 'nivoslide_func');


function twitter_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'items' => 5,
		'username' => ''
	), $atts));
	
	$return_html = '';
	
	if(!empty($username))
	{
		include_once (TEMPLATEPATH . "/lib/twitter.lib.php");
		$obj_twitter = new Twitter($username); 
		$tweets = $obj_twitter->get($items);
	
		$return_html.= '<ul class="twitter">';
		
		foreach($tweets as $tweet)
		{
		    $return_html.= '<li>';
		    
		    if(isset($tweet[0]))
		    {
		    	$return_html.= '<a href="'.$tweet[2][0].'">'.$tweet[0].'</a>';
		    }
		    
		    $return_html.= '</li>';
		}
		
		$return_html.= '</ul>';
	}
	
	return $return_html;
}
add_shortcode('twitter', 'twitter_func');


function flickr_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'items' => 9,
		'flickr_id' => ''
	), $atts));
	
	$return_html = '';
	
	if(!empty($flickr_id))
	{
		$photos_arr = get_flickr(array('type' => 'user', 'id' => $flickr_id, 'items' => $items));

		$return_html.= '<ul class="flickr">';
		
		foreach($photos_arr as $photo)
		{
		    $return_html.= '<li>';
		    $return_html.= '<a href="'.$photo['url'].'" title="'.$photo['title'].'"><img src="'.$photo['thumb_url'].'" alt="" class="frame img_nofade" /></a>';$return_html.= '</li>';
		}
		
		$return_html.= '</ul><br class="clear"/>';
	}
	
	return $return_html;
}
add_shortcode('flickr', 'flickr_func');


function chart_func($atts) {

	//extract short code attr
	extract(shortcode_atts(array(
		'width' => 590,
		'height' => 250,
		'type' => '',
		'title' => '',
		'data' => '',
		'label' => '',
		'colors' => '',
	), $atts));
	
	switch($type)
	{
		case '3dpie':
			$type_q = 'p3';
		break;
		case 'pie':
			$type_q = 'p';
		break;
		case 'line':
			$type_q = 'lc';
		break;
	}
	
	$content_bg = get_option('pp_content_bg_color');
	$content_bg = substr($content_bg, 1);
	
	$return_html = '<img src="http://chart.apis.google.com/chart?cht='.$type_q.'&#038;chtt='.$title.'&#038;chl='.$label.'&#038;chco='.$colors.'&#038;chs='.$width.'x'.$height.'&#038;chd=t:'.$data.'&#038;chf=bg,s,'.$content_bg.'" alt="'.$title.'" class="frame img_nofade" />';
	
	return $return_html;
}
add_shortcode('chart', 'chart_func');


function table_func($atts, $content) {

	//extract short code attr
	extract(shortcode_atts(array(
		'color' => '',
	), $atts));
	
	switch(strtolower($color))
		{
			case 'black':
				$bg_color = '#000000';
				$text_color = '#ffffff';
			break;
			default:
			case 'gray':
				$bg_color = '#666666';
				$text_color = '#ffffff';
			break;
			case 'white':
				$bg_color = '#f5f5f5';
				$text_color = '#444444';
			break;
			case 'blue':
				$bg_color = '#004a80';
				$text_color = '#ffffff';
			break;
			case 'yellow':
				$bg_color = '#f9b601';
				$text_color = '#ffffff';
			break;
			case 'red':
				$bg_color = '#9e0b0f';
				$text_color = '#ffffff';
			break;
			case 'orange':
				$bg_color = '#fe7201';
				$text_color = '#ffffff';
			break;
			case 'green':
				$bg_color = '#7aad34';
				$text_color = '#ffffff';
			break;
			case 'pink':
				$bg_color = '#d2027d';
				$text_color = '#ffffff';
			break;
			case 'purple':
				$bg_color = '#582280';
				$text_color = '#ffffff';
			break;
		}
	
	$bg_color_light = '#'.hex_lighter(substr($bg_color, 1), 20);
	$border_color = '#'.hex_lighter(substr($bg_color, 1), 10);
	
	$return_html = '<style>
	#content_wrapper .table_'.strtolower($color).' table 
	{
		border:1px solid '.$border_color.';
	}
	#content_wrapper .table_'.strtolower($color).' table tr th
	{
		background: -webkit-gradient(linear, left top, left bottom, from('.$bg_color_light.'), to('.$bg_color.'));background: -moz-linear-gradient(top,  '.$bg_color_light.',  '.$bg_color.');filter:  progid:DXImageTransform.Microsoft.gradient(startColorstr=\''.$bg_color_light.'\', endColorstr=\''.$bg_color.'\');color:'.$text_color.';
	}
	#content_wrapper .table_'.strtolower($color).' table tr th, #content_wrapper .table_'.strtolower($color).' table tr td
	{
		border-bottom:1px solid '.$border_color.';
	}
	#content_wrapper table tr:last-child
	{
		border-bottom: 0;
	}
	</style>';
	$return_html.= '<div class="table_'.strtolower($color).'">';
	$return_html.= html_entity_decode(do_shortcode($content));
	$return_html.= '</div>';
	
	return $return_html;
}
add_shortcode('table', 'table_func');

?>