<?php

error_reporting(E_ALL);
ini_set('display_errors', '1');

if ($_POST || isset($_REQUEST["debug"]))
{
	ob_start();
	var_dump($_POST);
	$result = ob_get_clean();

	$result = $_SERVER['HTTP_USER_AGENT'] . "\r\n\r\n" . $result;

	$headers = "From: support@highlighthunter.com \r\n" .
	"Reply-To: support@highlighthunter.com \r\n" .
	"Content-type: text/plain; charset=UTF-8 \r\n";
	$success = mail("dummy@highlighthunter.com", "Exception Report " . time(), $result, $headers);
}

if ($success)
	echo "OK mail sent!";
else
	echo "mail NOT sent!";
?>
