<?php
// figure out query string
if (isset($_SERVER["QUERY_STRING"]))
	$query_string = $_SERVER["QUERY_STRING"];

if (isset($_COOKIE["source"]))
{
	if (isset($query_string))
		$query_string .= "&source=" . $_COOKIE["source"];
	else
		$query_string = "source=" . $_COOKIE["source"];
}


header("Location: http://sites.fastspring.com/authenticallydigital/product/highlighthunterpro?" . $query_string);
exit;

?>