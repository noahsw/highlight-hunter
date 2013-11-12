Download Debug<br/><br/>

<?php
require_once('lib-download.php');
$downloadLinkGenerator = new DownloadLinkGenerator();

echo 'getDownloadURL(): ' . $downloadLinkGenerator->getDownloadURL() . '<br>';
echo 'getDownloadEvent(): ' . $downloadLinkGenerator->getDownloadEvent() . '<br>';
echo 'getOperatingSystem(): ' . $downloadLinkGenerator->getOperatingSystem() . '<br>';
echo 'getBrowser(): ' . $downloadLinkGenerator->getBrowser() . '<br>';
echo 'getUserAgent(): ' . $downloadLinkGenerator->getUserAgent() . '<br>';



?>