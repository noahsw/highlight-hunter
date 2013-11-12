<?php

error_reporting(E_ALL);
ini_set('display_errors', '1');

if ($_GET || isset($_REQUEST["debug"]))
{
	ob_start();
	var_dump($_GET);
	$result = ob_get_clean();

	$result = $_SERVER['HTTP_USER_AGENT'] . "\r\n\r\n" . $result;

	$headers = "From: support@highlighthunter.com \r\n" .
	"Reply-To: support@highlighthunter.com \r\n" .
	"Content-type: text/plain; charset=UTF-8 \r\n";
	$success = mail("dummy@highlighthunter.com", "ClickOnce Error Report " . time(), $result, $headers);
}

if ($success)
	echo "We're sorry for the inconvenience! Our engineers have been notified.";
else
	echo "We're sorry for the inconvenience!";
?>
