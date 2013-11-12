<?php
/**
 * The template for displaying the footer.
 *
 * @package WordPress
 */
?>

    </div> <!-- End wrapper div -->

    <div class="bottom_shadow"></div>

    <!-- Begin footer -->
    <div id="footer">
      <div id="footer_shadow"></div> <!-- End footer shadow -->

      <ul class="sidebar_widget">
        <?php dynamic_sidebar('Footer Sidebar'); ?>
      </ul>

      <br class="clear"/>

      <div id="copyright">
        <?php
          /**
           * Get footer text
           */

          $pp_footer_text = get_option('pp_footer_text');

          if(empty($pp_footer_text))
          {
            $pp_footer_text = 'Copyright © 2010 Peerapong. Remove this once after purchase from the ThemeForest.net';
          }

          echo stripslashes(html_entity_decode($pp_footer_text));
        ?>
      </div> <!-- End copyright -->

    </div> <!-- End footer -->


<?php
    /**
      * Setup Google Analyric Code
      **/
      include (TEMPLATEPATH . "/google-analytic.php");
?>

</div></div>

<?php
  /* Always have wp_footer() just before the closing </body>
   * tag of your theme, or you will break many plugins, which
   * generally use this hook to reference JavaScript files.
   */

  wp_footer();
?>

<!-- Begin FastSpring Source Link Script -->
<?php
if (is_home() || is_page("home") || is_page("download") || is_page("home2"))
{
?>
<script type="text/javascript">
<!--
try {
if (window.location.search != null) {
  var cookie = ''; var params = ['source','campaign'];
  for (var i = 0; i < params.length; i++){
    var srcPos = window.location.search.indexOf(params[i]);
    if (srcPos != -1) {
      var srcStr = window.location.search.substr(srcPos + params[i].length + 1);
      var srcAmpPos = srcStr.indexOf('&');
      var srcVal = srcAmpPos != -1 ? srcStr.substring(0, srcAmpPos) : srcStr;
      var now = new Date(); now.setDate(now.getDate() + 60);
      document.cookie = params[i] + '=' + escape(srcVal) + ';' + 'expires=' + now.toGMTString() + '; path=/;';
    }
  }
}
} catch (e){}
//-->
</script>
<?php
} // if home page or download page (where our affiliates send us traffic)

if (is_page("store"))
{
?>
<script type="text/javascript">
<!--
try {
if (document.cookie != null){
  var cookiearray = document.cookie.split(';');
  var append = '';
  for(var i=0; i<cookiearray.length; i++){
     var name = cookiearray[i].split('=')[0]; while (name.charAt(0)==' ') name = name.substring(1,name.length);
     var value = unescape(cookiearray[i].split('=')[1]);
     if (name == 'source' || name == 'campaign') append = append + (append.length > 0 ? '&' : '') + name + '=' + value;
  }
  if (append.length > 0){
    var links = document.getElementsByTagName("a");
    for (var i = 0; i < links.length; i++){
      var dom = links[i];
      if (dom.href.indexOf('sites.fastspring.com') != -1) {
        var ch = dom.href.indexOf('?') != -1 ? '&' : '?';
        dom.href = dom.href + ch + append;
      }
    }
    var forms = document.getElementsByTagName("form");
    for (var i = 0; i < forms.length; i++){
      var dom = forms[i];
      if (dom.action.indexOf('sites.fastspring.com') != -1) {
        var ch = dom.action.indexOf('?') != -1 ? '&' : '?';
        dom.action = dom.action + ch + append;
      }
    }
  }
}
} catch (e){}
//-->
</script>
<?php
} // if store page, where we link off to fast spring
?>
<!-- End Source Link Script -->

</body>
</html>
