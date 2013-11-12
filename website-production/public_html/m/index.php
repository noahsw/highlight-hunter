<?php
function getResourcePath($relativePath)
{
    $isTest = !(strpos($_SERVER["SERVER_NAME"], "test") === false);

    if ($isTest)
        return $relativePath;

    $extension = substr($relativePath, strrpos($relativePath, '.') + 1);

    if ($extension == 'css')
        return "http://static4.highlighthunter.com/m/$relativePath";
    else if ($extension == 'js')
        return "http://static2.highlighthunter.com/m/$relativePath";
    else
        return "http://static.highlighthunter.com/m/$relativePath";
}
?>
<!DOCTYPE html>
<!--[if IEMobile 7 ]>    <html class="no-js iem7"> <![endif]-->
<!--[if (gt IEMobile 7)|!(IEMobile)]><!--> <html class="no-js"> <!--<![endif]-->
    <head>
        <meta charset="utf-8">
        <title>Highlight Hunter</title>
        <meta name="description" content="">
        <meta name="HandheldFriendly" content="True">
        <meta name="MobileOptimized" content="320">
        <meta name="viewport" content="width=device-width">
        <meta http-equiv="cleartype" content="on">
<!--Commenting out custom touch icons
        <link rel="apple-touch-icon-precomposed" sizes="144x144" href="img/touch/apple-touch-icon-144x144-precomposed.png">
        <link rel="apple-touch-icon-precomposed" sizes="114x114" href="img/touch/apple-touch-icon-114x114-precomposed.png">
        <link rel="apple-touch-icon-precomposed" sizes="72x72" href="img/touch/apple-touch-icon-72x72-precomposed.png">
        <link rel="apple-touch-icon-precomposed" href="img/touch/apple-touch-icon-57x57-precomposed.png">
        <link rel="shortcut icon" href="img/touch/apple-touch-icon.png">
-->
        <!-- For iOS web apps. Delete if not needed. https://github.com/h5bp/mobile-boilerplate/issues/94 -->
        <!--
        <meta name="apple-mobile-web-app-capable" content="yes">
        <meta name="apple-mobile-web-app-status-bar-style" content="black">
        -->

        <!-- This script prevents links from opening in Mobile Safari. https://gist.github.com/1042026 -->
        <!--
        <script>(function(a,b,c){if(c in b&&b[c]){var d,e=a.location,f=/^(a|html)$/i;a.addEventListener("click",function(a){d=a.target;while(!f.test(d.nodeName))d=d.parentNode;"href"in d&&(d.href.indexOf("http")||~d.href.indexOf(e.host))&&(a.preventDefault(),e.href=d.href)},!1)}})(document,window.navigator,"standalone")</script>
        -->

        <link rel="stylesheet" href="<?php echo getResourcePath('css/normalize.css'); ?>">
        <link rel="stylesheet" href="<?php echo getResourcePath('css/main.css'); ?>">
        <script src="<?php echo getResourcePath('js/vendor/modernizr-2.6.1.min.js'); ?>"></script>
        
        <!--Google Analytics-->
        <script type="text/javascript">//<![CDATA[ 

            var _gaq = _gaq || []; 
  
            _gaq.push(['_setAccount','UA-28160719-1']); 
            
            _gaq.push(['_setCampSourceKey', 'utm_source']); 
            _gaq.push(['_setCampMediumKey', 'utm_medium']); 
            _gaq.push(['_setCampContentKey', 'utm_content']); 
            _gaq.push(['_setCampTermKey', 'utm_keyword']); 
            _gaq.push(['_setCampNameKey', 'utm_campaign']);

            _gaq.push(['_setDomainName','highlighthunter.com'],['_trackPageview'],['_trackPageLoadTime']);

            (function() { 
            
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true; 
            
            ga.src = ('https:' == document.location.protocol ? 'https://ssl&#39; : 'http://www&#39;) + '.google-analytics.com/ga.js'; 
            
            
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s); 
            
            })(); 
            
            //]]>
        </script> 
        
    </head>
    <body>
        
            <div id="logobar">
                <img src="http://static.highlighthunter.com/images/hh-50px-lockup.png">
                <div id="logo"></div>
            </div>
            <div class="video-container">
                <iframe src="http://www.youtube.com/embed/e0xRpRbLb_0?controls=0&showinfo=0&showsearch=0&modestbranding=1" frameborder="0" width="560" height="315"></iframe>
            </div>
            
            <div class="greenbutton">
                <a href="mailto:?subject=Reminder to download Highlight Hunter&body=Remember to download Highlight Hunter for Mac or PC. It's a free download at http://www.highlighthunter.com.">DOWNLOAD FREE (MAC&#43;PC)</a>
            </div>
            
            <ol>
                <li><b>1. Record &#43; Mark</b><br/>Record with your camera as usual. After something cool happens, cover the lens with your hand for 1 second.</li>
                <li><b>2. Scan</b><br/>Sit back and relax while Highlight Hunter scans your videos for the highlights you marked.</li>
                <li><b>3. Share &#43; Save</b><br/>Share the best highlights on Facebook, save them as videos, or open them in your favorite video editor.</li>
            </ol>
            
            <div class="darkgray">
                Record all day. Never miss a highlight. Save time editing. No big deal.
            </div>
            
            <div class="greenbutton">
                <a href="mailto:?subject=Reminder to download Highlight Hunter&body=Remember to download Highlight Hunter for Mac or PC. It's a free download at http://www.highlighthunter.com.">Send yourself a download reminder</a>
            </div>
            
            <div id="footer"> 
                <a href="http://www.highlighthunter.com?desktop=1">full site</a>  &#124;  <a href="http://www.highlighthunter.com/blog">blog</a>  &#124;  <a href="http://support.highlighthunter.com">support</a>
            </div>
        

        <script src="js/vendor/zepto.min.js"></script>
        <script src="js/helper.js"></script>

   </body>
</html>
