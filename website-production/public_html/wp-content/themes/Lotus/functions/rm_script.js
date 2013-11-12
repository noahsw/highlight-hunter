    jQuery(document).ready(function(){
		/*jQuery('.rm_options').slideUp();
		
		jQuery('.rm_section h3').click(function(){		
			if(jQuery(this).parent().next('.rm_options').css('display')=='none')
				{	jQuery(this).removeClass('inactive');
					jQuery(this).addClass('active');
					jQuery(this).children('img').removeClass('inactive');
					jQuery(this).children('img').addClass('active');
					
				}
			else
				{	jQuery(this).removeClass('active');
					jQuery(this).addClass('inactive');		
					jQuery(this).children('img').removeClass('active');			
					jQuery(this).children('img').addClass('inactive');
				}
				
			jQuery(this).parent().next('.rm_options').slideToggle('slow');	
		});*/
		
		jQuery('#current_sidebar li a.sidebar_del').click(function(){
			if(confirm('Are you sure you want to delete this sidebar? (this can not be undone)'))
			{
				sTarget = jQuery(this).attr('href');
				sSidebar = jQuery(this).attr('rel');
				objTarget = jQuery(this).parent('li');
				
				jQuery.ajax({
  		    		type: 'POST',
  		    		url: sTarget,
  		    		data: 'sidebar_id='+sSidebar,
  		    		success: function(msg){ 
  		    			objTarget.fadeOut();
  		    		}
		    	});
			}
			
			return false;
		});
		
		jQuery('a.image_del').click(function(){
			if(confirm('Are you sure you want to delete this image? (this can not be undone)'))
			{
				sTarget = jQuery(this).attr('href');
				sFieldId = jQuery(this).attr('rel');
				objTarget = jQuery('#'+sFieldId+'_wrapper');
				
				jQuery.ajax({
  		    		type: 'POST',
  		    		url: sTarget,
  		    		data: 'field_id='+sFieldId,
  		    		success: function(msg){ 
  		    			objTarget.fadeOut();
  		    		}
		    	});
			}
			
			return false;
		});
		
		jQuery('#pp_panel a').click(function(){
			jQuery('#pp_panel a').removeClass('nav-tab-active');
			jQuery(this).addClass('nav-tab-active');
			
			jQuery('.rm_section').css('display', 'none');
			jQuery(jQuery(this).attr('href')).css('display', 'block');
			jQuery('#current_tab').val(jQuery(this).attr('href'));
			
			return false;
		});
		
		jQuery('.color_picker').each(function()
		{	
			var inputID = jQuery(this).attr('id');
			
			jQuery(this).ColorPicker({
				color: jQuery(this).val(),
				onShow: function (colpkr) {
					jQuery(colpkr).fadeIn(500);
					return false;
				},
				onHide: function (colpkr) {
					jQuery(colpkr).fadeOut(500);
					return false;
				},
				onChange: function (hsb, hex, rgb, el) {
					jQuery('#'+inputID).val('#' + hex);
					jQuery('#'+inputID+'_bg').css('backgroundColor', '#' + hex);
				}
			});	
			
			jQuery(this).css('visibility', 'hidden');
		});
		
		jQuery('.iphone_checkboxes').iphoneStyle({
  			checkedLabel: 'YES',
  			uncheckedLabel: 'NO'
		});
		
		jQuery('.rm_section').css('display', 'none');
		
		if(self.document.location.hash == '')
		{
			jQuery('#pp_panel_general_a').click();
		}
		else
		{
			jQuery(self.document.location.hash+'_a').click();
		}
		
		jQuery( ".pp_sortable" ).sortable({
			placeholder: "ui-state-highlight",
			create: function(event, ui) { 
				myCheckRel = jQuery(this).attr('rel');
			
				var order = jQuery(this).sortable('toArray');
            	jQuery('#'+myCheckRel).val(order);
			},
			update: function(event, ui) {
				myCheckRel = jQuery(this).attr('rel');
			
				var order = jQuery(this).sortable('toArray');
            	jQuery('#'+myCheckRel).val(order);
			}
		});
		jQuery( ".pp_sortable" ).disableSelection();
		
		jQuery(".pp_checkbox input").change(function(){
			myCheckId = jQuery(this).val();
			myCheckRel = jQuery(this).attr('rel');
			myCheckTitle = jQuery(this).attr('alt');
			
			if (jQuery(this).is(':checked')) { 
				jQuery('#'+myCheckRel).append('<li id="'+myCheckId+'_sort" class="ui-state-default">'+myCheckTitle+'</li>');
			} 
			else
			{
				jQuery('#'+myCheckId+'_sort').remove();
			}

			var order = jQuery('#'+myCheckRel).sortable('toArray');

            jQuery('#'+myCheckRel+'_data').val(order);
		});
				
});