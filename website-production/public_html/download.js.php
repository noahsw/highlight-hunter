<?php
header('Content-type: text/javascript');

require_once("lib-download.php");

$downloadLinkGenerator = new DownloadLinkGenerator();

if (isset($_REQUEST["force_noappstore"])) // so users on Mac that are redeeming code, don't go to the app store
    $force_noappstore = "?force_noappstore=1";
else
    $force_noappstore = "";


?>

jQuery(window).load(function() {

	var newHref = '<?php echo $downloadLinkGenerator->getDownloadURL(); ?>';
	var newTarget = '_blank';
	var newOnClick = RedirectToDownloadingPage;

	if (newHref == '')
	{
		newHref = 'mailto:?subject=Reminder%20to%20download%20Highlight%20Hunter&body=Highlight%20Hunter%20requires%20a%20Mac%20or%20PC%20-%20http%3A%2F%2Fwww.highlighthunter.com';
		newTarget = '';
		newOnClick = TrackEmailReminder;
	}

	for (var i = 0; i < $j('#main_menu a').length; i++) {
		var link = $j('#main_menu a')[i];
		if (link.href.indexOf("downloading") == -1)
			continue;

		link.href = newHref;
		link.target = newTarget;
		link.onclick = newOnClick;
	}

	for (var i = 0; i < $j('.download_link').length; i++) {
		var link = $j('.download_link')[i];
		if (link.href.indexOf("downloading") == -1)
			continue;

		link.href = newHref;
		link.target = newTarget;
		link.onclick = newOnClick;
	}

	var link = $j('#slider_download_link a')[0];
	if (link)
	{
		link.href = newHref;
		link.target = newTarget;
		link.onclick = newOnClick;
	}
});

function RedirectToDownloadingPage()
{ // Needed because Analytics wordpress plugin was screwing up our onclick events. It was inserting code to track outbound links and that screwed us up.
	// Analytics are done on /downloading page
	setTimeout(function()
    { // This timeout is for Safari
        location.href="/downloading/<?php echo $force_noappstore; ?>";
    }, 100);

	return true;
}

function TrackEmailReminder()
{
	setTimeout(function()
    { // This timeout is for Safari
        var _gaq = _gaq || [];
   		_gaq.push(['_setAccount','UA-28160719-1']);
   		_gaq.push(['_trackEvent','download','reminder']);
    }, 100);

	return true;

}
