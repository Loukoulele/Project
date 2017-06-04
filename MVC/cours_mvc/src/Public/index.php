<?php

	echo $_GET['page'];
	echo "<br/>";



	require_once('../Library/Loader/Autoloader.php');

	$autoload=\Library\Loader\Autoloader::getInstance(); 

	//echo __DIR__;
	$autoload::setBasePath(str_replace('Public', '', __DIR__));


	//CVhargement des SEttings (constantes)
	\Application\Configs\Settings::getInstance();

	$router= \Library\Router\Router::getInstance();

	$router::dispatchPage(explode('/', $_GET['page']));



?>