<?php
require_once("lib-download.php");

if (isset($_SERVER["QUERY_STRING"]))
	$query_string = $_SERVER["QUERY_STRING"];

$downloadLinkGenerator = new DownloadLinkGenerator();
if ($downloadLinkGenerator->getDownloadURL() != '') // make sure we have valid download link for this browser and OS
	header("Location: " + $downloadLinkGenerator->getDownloadURL() . "?" . $query_string);
else
	header("Location: http://www.highlighthunter.com?" . $query_string);
	
?>