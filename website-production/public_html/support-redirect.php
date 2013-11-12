<?php
$id = $_REQUEST["id"];
$url = "";

switch ($id)
{
	case "ErrorArchiving":
		$url = "http://support.highlighthunter.com/customer/portal/emails/new";
		break;

	case "ErrorStartingTrial":
		$url = "http://support.highlighthunter.com/customer/portal/emails/new";
		break;

	case "ErrorReinstating":
		$url = "http://support.highlighthunter.com/customer/portal/emails/new";
		break;

	case "Lockout":
		$url = "http://support.highlighthunter.com/customer/portal/emails/new";
		break;
	
	case "ErrorLoadingVideo":
	case "WMPErrorC00D1199":
		$url = "http://support.highlighthunter.com/customer/portal/articles/780756-missing-codecs-pc-only-";
		break;
	
	default:
		$url = "http://support.highlighthunter.com/customer/portal/emails/new";
		break;
}

header("Location: " . $url);

?>