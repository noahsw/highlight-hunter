/*
	Code to configure AddThis
*/

   var addthis_config = {
      data_ga_property: 'UA-28160719-1',
      data_ga_social: true,
	  "data_track_addressbar": true
   };
   var addthis_share = {
      url_transforms: {
         clean: true    /* removes Google Analytics utm_* parameters */
      },
	  url: "http://www.highlighthunter.com"
   }
   
   
   
/*
	Easy plugin to get element index position
	Author: Peerapong Pulpipatnan
	http://themeforest.net/user/peerapong
*/

var $j = jQuery.noConflict();

$j.fn.getIndex = function(){
	var $jp=$j(this).parent().children();
    return $jp.index(this);
}

$j.fn.setNav = function(){
	$j('#main_menu li ul').css({display: 'none'});

	$j('#main_menu li').each(function()
	{	
		var $jsublist = $j(this).find('ul:first');
		
		$j(this).hover(function()
		{	
			$jsublist.css({opacity: 1});
			
			$jsublist.stop().css({overflow:'hidden', height:'auto', display:'none'}).fadeIn(200, function()
			{
				$j(this).css({overflow:'visible', height:'auto', display: 'block'});
			});	
		},
		function()
		{	
			$jsublist.stop().css({overflow:'hidden', height:'auto', display:'none'}).fadeOut(200, function()
			{
				$j(this).css({overflow:'hidden', display:'none'});
			});	
		});	
		
	});
	
	$j('#main_menu li').each(function()
	{
		
		$j(this).hover(function()
		{	
			$j(this).find('a:first').addClass('hover');
		},
		function()
		{	
			$j(this).find('a:first').removeClass('hover');
		});	
		
	});
	
	$j('#menu_wrapper .nav ul li ul').css({display: 'none'});

	$j('#menu_wrapper .nav ul li').each(function()
	{	
		
		var $jsublist = $j(this).find('ul:first');
		
		$j(this).hover(function()
		{	
			$jsublist.css({opacity: 1});
			
			$jsublist.stop().css({overflow:'hidden', height:'auto', display:'none'}).fadeIn(200, function()
			{
				$j(this).css({overflow:'visible', height:'auto', display: 'block'});
			});	
		},
		function()
		{	
			$jsublist.stop().css({overflow:'hidden', height:'auto', display:'none'}).fadeOut(200, function()
			{
				$j(this).css({overflow:'hidden', display:'none'});
			});	
		});	
		
	});
	
	$j('#menu_wrapper .nav ul li').each(function()
	{
		
		$j(this).hover(function()
		{	
			$j(this).find('a:first').addClass('hover');
		},
		function()
		{	
			$j(this).find('a:first').removeClass('hover');
		});	
		
	});
}

/* REMED out because it's not used
$j(function () {

    	$j('.slideshow').anythingSlider({
    	        easing: "easeInOutExpo",
    	        autoPlay: false,
    	        startStopped: false,
    	        animationTime: 600,
    	        hashTags: false,
    	        buildNavigation: true,
    	        buildArrows: false,
    			pauseOnHover: true,
    			startText: "Go",
    	        stopText: "Stop"
    	    });
    	    
    });
*/

$j(document).ready(function(){ 

	$j(document).setNav();
	
	$j('.img_frame').fancybox({ 
		padding: 10,
		overlayColor: '#000',
		transitionIn: 'elastic',
		transitionOut: 'elastic',
		overlayOpacity: .8
	});
	
	$j('.pp_gallery a').fancybox({ 
		padding: 0,
		overlayColor: '#000', 
		transitionIn: 'elastic',
		transitionOut: 'elastic',
		overlayOpacity: .8
	});
	
	$j('.flickr li a').fancybox({ 
		padding: 0,
		overlayColor: '#000', 
		transitionIn: 'elastic',
		transitionOut: 'elastic',
		overlayOpacity: .8
	});
	
	$j('.lightbox').fancybox({ 
		padding: 0,
		overlayColor: '#000', 
		transitionIn: 'fade',
		transitionOut: 'fade',
		overlayOpacity: .8
	});
	
	$j('.lightbox_youtube').fancybox({ 
		padding: 10,
		overlayColor: '#000', 
		transitionIn: 'fade',
		transitionOut: 'fade',
		overlayOpacity: .8
	});
	
	$j('.lightbox_vimeo').fancybox({ 
		padding: 10,
		overlayColor: '#000', 
		transitionIn: 'fade',
		transitionOut: 'fade',
		overlayOpacity: .8
	});
	
	$j('.lightbox_dailymotion').fancybox({ 
		padding: 10,
		overlayColor: '#000', 
		transitionIn: 'fade',
		transitionOut: 'fade',
		overlayOpacity: .8
	});
	
	$j('.lightbox_iframe').fancybox({ 
		padding: 0,
		type: 'iframe',
		overlayColor: '#000', 
		transitionIn: 'fade',
		transitionOut: 'fade',
		overlayOpacity: .8,
		width: 900,
		height: 650
	});
	
	$j('a.one_fourth_img[rel=gallery]').fancybox({ 
		padding: 0,
		overlayColor: '#000', 
		overlayOpacity: .8
	});
	
	$j('#video_gallery_wrapper .one_fourth .portfolio_image .portfolio4_hover a').fancybox({ 
		padding: 10,
		overlayColor: '#000', 
		overlayOpacity: .8
	});
	
/* REMED out because it's not used
	$j.validator.setDefaults({
		submitHandler: function() { 
		    var actionUrl = $j('#contact_form').attr('action');
		    
		    $j.ajax({
  		    	type: 'GET',
  		    	url: actionUrl,
  		    	data: $j('#contact_form').serialize(),
  		    	success: function(msg){
  		    		$j('#contact_form').hide();
  		    		$j('#reponse_msg').html(msg);
  		    	}
		    });
		    
		    return false;
		}
	});
		    
		
	$j('#contact_form').validate({
		rules: {
		    your_name: "required",
		    email: {
		    	required: true,
		    	email: true
		    },
		    message: "required"
		},
		messages: {
		    your_name: "Please enter your name",
		    email: "Please enter a valid email address",
		    agree: "Please enter some message"
		}
	});	
*/
	
	if(BrowserDetect.browser == 'Explorer' && BrowserDetect.version < 8)
	{
		var zIndexNumber = 1000;
		$j('div').each(function() {
			$j(this).css('zIndex', zIndexNumber);
			zIndexNumber -= 10;
		});

		$j('#thumbNav').css('zIndex', 1000);
		$j('#thumbLeftNav').css('zIndex', 1000);
		$j('#thumbRightNav').css('zIndex', 1000);
		$j('#fancybox-wrap').css('zIndex', 1001);
		$j('#fancybox-overlay').css('zIndex', 1000);
	}
	
	$j(".pp_accordion").accordion({ collapsible: true });
	
	$j(".pp_accordion_close").find('.ui-accordion-header a').click();
	
	$j(".tabs").tabs();
	
	$j('.portfolio1_hover').hide();
	$j('.two_third').hover(function(){  
 			$j(this).find('.portfolio1_hover').show();
 			$j(this).find('img').animate({top: '10px'}, 300);
 			
 			$j(this).click(function(){
 				$j(this).find('a').click();
 			});
 		}  
  		, function(){  
  		
  			$j(this).find('img').animate({top: '20px'}, 300);
  			$j(this).find('.portfolio1_hover').hide();
  		}  
  		
	);
	
	$j('.portfolio2_hover').hide();
	$j('.one_half .portfolio_image').hover(function(){  
 			$j(this).find('.portfolio2_hover').show();
 			$j(this).find('img').animate({top: '11px'}, 300);
 			
 			$j(this).click(function(){
 				$j(this).find('a').click();
 			});
 		}  
  		, function(){  
  		
  			$j(this).find('img').animate({top: '21px'}, 300);
  			$j(this).find('.portfolio2_hover').hide();
  		}  
  		
	);
	
	$j('.portfolio3_hover').hide();
	$j('.one_third .portfolio_image').hover(function(){  
 			$j(this).find('.portfolio3_hover').show();
 			$j(this).find('img').animate({top: '13px'}, 300);
 			
 			$j(this).click(function(){
 				$j(this).find('a').click();
 			});
 		} 
  		, function(){  
  		
  			$j(this).find('img').animate({top: '20px'}, 300);
  			$j(this).find('.portfolio3_hover').hide();
  		}  
  		
	);
	
	$j('.portfolio4_hover').hide();
	$j('.one_fourth .portfolio_image').hover(function(){  
 			$j(this).find('.portfolio4_hover').show();
 			$j(this).find('img').animate({top: '3px'}, 300);
 			
 			$j(this).click(function(){
 				$j(this).find('a').click();
 			});
 		}  
  		, function(){  
  		
  			$j(this).find('img').animate({top: '10px'}, 300);
  			$j(this).find('.portfolio4_hover').hide();
  		}  
  		
	);
	
	$j('.post_img').hover(function(){  
 			$j(this).find('img').animate({top: '0px'}, 300);
 			
 			$j(this).click(function(){
 				$j(this).find('a').click();
 			});
 		}  
  		, function(){  
  		
  			$j(this).find('img').animate({top: '15px'}, 300);
  		}  
  		
	);
	
	$j('.pp_gallery a img').hover(function(){  
 			$j(this).animate({top: '-3px'}, 100);
 		}  
  		, function(){  
  		
  			$j(this).animate({top: '0px'}, 100);
  		}  
  		
	);
	
	$j('.home_portfolio_grid').hover(function(){  
 			$j(this).animate({top: '-5px'}, 300);
 		}  
  		, function(){  
  		
  			$j(this).animate({top: '5px'}, 300);
  		}  
  		
	);
	
	$j('.card_portfolio_grid').hover(function(){  
 			$j(this).animate({top: '-10px'}, 300);
 		}  
  		, function(){  
  		
  			$j(this).animate({top: '0px'}, 300);
  		}  
  		
	);
	
	$j('.img_nofade').hover(function(){  
			$j(this).animate({opacity: .5}, 300);
 		}  
  		, function(){  
  			$j(this).animate({opacity: 1}, 300);
  		}  
  		
	);
	
/* REMED out because it's not used
	$j('.tipsy').tipsy({fade: false, gravity: 's'});
*/
	
	/*$j('.one_sixth_img').tipsy({fade: false, gravity: 'n'});
	
	$j('.one_third_img').tipsy({fade: false, gravity: 'n'});
	
	$j('.one_fourth_img').tipsy({fade: false, gravity: 'n'});*/
	
	/*Cufon.replace('h1.cufon');
	Cufon.replace('h2.cufon');
	Cufon.replace('h2.widgettitle');
	Cufon.replace('h3.cufon');
	Cufon.replace('h4.cufon');
	Cufon.replace('h5.cufon');
	Cufon.replace('h6.cufon');
	Cufon.replace('.tagline');
	Cufon.replace('.pricing_box h2');
	Cufon.replace('.pricing_box .header span');
	Cufon.replace('.dropcap1');
	Cufon.replace('.circle_date a');
	Cufon.replace('.page_caption h1.cufon');
	Cufon.replace('.tagline h2.cufon');
	Cufon.replace('.tagline p');
	Cufon.replace('.ui-accordion-header');*/
	
	var footerLi = 0;
	$j('#footer .sidebar_widget li.widget').each(function()
	{
		footerLi++;
		
		if(footerLi%4 == 0)
		{ 
			$j(this).addClass('widget-four');
		}
	});
	
	/* REMED out because it's not used
	VideoJS.setupAllWhenReady({
      controlsBelow: false, // Display control bar below video instead of in front of
      controlsHiding: true, // Hide controls when mouse is not over the video
      defaultVolume: 0.85, // Will be overridden by user's last volume if available
      flashVersion: 9, // Required flash version for fallback
      linksHiding: true // Hide download links when video is supported
    });
	*/
	
	$j('.home_portfolio img.frame').each(function()
	{
		$j(this).hover(function()
		{	
			$j(this).animate({top: '-10px'}, 300);
		},
		function()
		{	
			$j(this).animate({top: 0}, 300);
		});	
	});
	
	$j('.html5_wrapper').hide();
	
	$j('input[title!=""]').hint();
	
	/* REMED because not used
	$j('.portfolio_title').tipsy({fade: true, gravity: 's'});
	
	$j('a.portfolio_image.gallery').tipsy({fade: true, gravity: 's'});
	*/
	
	$j('.tagline').css('visibility', 'visible');
	
});