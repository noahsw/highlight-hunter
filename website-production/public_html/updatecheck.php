<?php
header("Content-type: text/xml");
echo "<?xml version=\"1.0\"?>";

$platform = $_REQUEST['platform'];
switch ($platform)
{
	case "pc":
		$latestVersion = "2.1";
		$downloadPage = "http://www.highlighthunter.com/download-redirect.php";
		$whatsNew = 
			"- Brand new design\n" .
			"- Ability to trim clips and upload them to Facebook from within the app\n" .
			"- Scanning engine is twice as accurate\n" .
			"- QuickTime player enables smoother playback";
		break;

	case "mac":
		$latestVersion = "2.0";
		$downloadPage = "http://www.highlighthunter.com/download-redirect.php";
		$whatsNew = 
			"- Brand new design\n" .
			"- Ability to trim clips and upload them to Facebook from within the app\n" .
			"- Scanning engine is twice as accurate\n";
		break;
		
	default:
		$latestVersion = "error";
		$downloadPage = "http://www.highlighthunter.com";
		$whatsNew = "";
		break;
}

$version = $_REQUEST['version'];
// PC example: 1.2.3.0
// Mac example: 1.2.3

$isUpdateAvailable = "false"; // default
$versionSplit = explode('.', $version);
$latestVersionSplit = explode('.', $latestVersion);
for ($i = 0; $i < count($latestVersionSplit); $i++) // use count($latestVersionSplit) beause it's smaller
{
	if (intval($latestVersionSplit[$i]) > intval($versionSplit[$i]) )
	{
		$isUpdateAvailable = "true";
		break;
	}
	if (intval($latestVersionSplit[$i]) < intval($versionSplit[$i]) )
	{
		$isUpdateAvailable = "false";
		break;
	}
}

$oXMLout = new XMLWriter();
$oXMLout->openMemory();
$oXMLout->startElement("updateResults");
$oXMLout->writeElement("isUpdateAvailable", $isUpdateAvailable);
$oXMLout->writeElement("latestVersion", $latestVersion);
$oXMLout->writeElement("whatsNew", $whatsNew);
$oXMLout->writeElement("downloadPage", $downloadPage);
$oXMLout->endElement();
print $oXMLout->outputMemory();
 



?>