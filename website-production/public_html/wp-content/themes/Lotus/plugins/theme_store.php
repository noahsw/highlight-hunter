<?php

/*
	Begin Create Troubleshooting Options
*/

add_action('admin_menu', 'pp_theme_store');

function pp_theme_store() {

  //add_submenu_page('functions.php', 'Theme Store', 'Theme Store', 'manage_options', 'pp_theme_store', 'pp_theme_store_options');
  add_menu_page('Theme Store', 'Theme Store', 'administrator', basename(__FILE__), 'pp_theme_store_options', get_bloginfo( 'stylesheet_directory' ).'/functions/images/screen.png');

}

function pp_theme_store_options() {

  	if (!current_user_can('manage_options'))  {
    	wp_die( __('You do not have sufficient permissions to access this page.') );
  	}
  	
  	$plugin_url = get_bloginfo( 'stylesheet_directory' ).'/plugins/theme_store';
?>

<div class="wrap rm_wrap">
	<div class="header_wrap">
		<div style="float:left">
			<h2>Theme Store</h2>
		</div>
		<div id="icon-themes" class="icon32" style="float:right;margin:20px 0 0 0"><br></div>
		<br style="clear:both"/>
	</div>
	
	<br/>
	
	<div style="padding:40px 20px 0 20px;background:#fff">
		<?php
			
			// Get store data from API
			$per_page = 10;
			
			if(isset($_GET['start']))
			{
				$start = $_GET['start'];
			}
			else
			{
				$start = 0;
			}
			$end = $start+$per_page-1;
			$next = $end+1;
			$prev = $start-$per_page;
			
			$store_stat_url = 'http://www.gallyapp.com/tf_themes/store/stat.php';
			$store_data_url = 'http://www.gallyapp.com/tf_themes/themes_store.php';
			$data = file_get_contents( $store_data_url );
			$themes_arr = unserialize($data);
			
			$themes_arr = array_merge(array(),$themes_arr);
			$count_themes = count($themes_arr);
			
			if($next<=$count_themes)
			{
				$has_next = TRUE;
			}
			else
			{
				$has_next = FALSE;
			}
			
			if($start>$per_page-1)
			{
				$has_prev = TRUE;
			}
			else
			{
				$has_prev = FALSE;
			}
				
			//pp_debug($themes_arr);
			if(is_array($themes_arr) && !empty($themes_arr))
			{
				$current = 1;
				for($i=$start;$i<=$end;$i++)
				{
					if(isset($themes_arr[$i]))
					{
		?>
		
				<div class="one_half <?php if($current>1 && $current%3 ==0) { echo 'last'; } ?>">
					<img src="<?php echo $themes_arr[$i]['preview']; ?>" /><br/><br/>
					<div class="theme_detail_wrapper">
						<div class="theme_name">
							<?php 
								$theme_name_arr = explode('-', $themes_arr[$i]['name']);
								
								$theme_name = $theme_name_arr[0];
								$theme_desc = $theme_name_arr[1];
							?>
							<span class="theme_title"><strong><?php echo $theme_name; ?></strong></span><br/>
							<span class="theme_desc"><?php echo $theme_desc; ?></span>
						</div>
						<div class="theme_buy">
							<a href="<?php echo $store_stat_url.'?theme='.$theme_name.'&amp;method=buy&amp;redirect='.$themes_arr[$i]['url']; ?>" class="button-primary" target="_blank">$<?php echo $themes_arr[$i]['price']; ?> BUY</a>
							<a href="<?php echo $store_stat_url.'?theme='.$theme_name.'&amp;method=demo&amp;redirect='.$themes_arr[$i]['demo']; ?>" class="button" target="_blank">DEMO</a>
						</div>
					</div>
				</div>
		
		<?php
						$current++;
					}
				}
				
				echo '<br style="clear:both"/>';
			}
			else
			{
		?>
				<div style="margin:auto;text-align:center">
					<img src="<?php echo get_bloginfo( 'stylesheet_directory' ); ?>/functions/images/attention-alert-warning-icone-6427-48.png"/>
					<p>Themes Store is not available now. <br/>Please check your network connection and try again later.</p>
				</div>
				<br style="clear:both"/>
				
		<?php
			}
		?>
	</div>
	
	<?php
		if($has_next OR $has_prev)
		{
	?>
			<br style="clear:both"/>
			<div style="margin:0 0 0 20px">
			
			<?php
				if($has_prev)
				{
			?>
			<div style="float:left;margin:0 10px 15px 0">
				<a href="<?php echo get_admin_url(); ?>admin.php?page=theme_store.php&start=<?php echo $prev; ?>" class="button"><< PREVIOUS</a>
			</div>
			<?php 
				}
			?>
			
			<?php
				if($has_next)
				{
			?>
			<div style="float:left;margin:0 10px 15px 0">
				<a href="<?php echo get_admin_url(); ?>admin.php?page=theme_store.php&start=<?php echo $next; ?>" class="button">NEXT >></a>
			</div>
			<?php 
				}
			?>
			
			</div>
			<br style="clear:both"/>
	<?php
		}
	?>
	
</div>
<br style="clear:both"/>

<?php

}

/*
	End Create Troubleshooting Options
*/

?>