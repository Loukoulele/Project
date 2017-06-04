<?php
namespace Application\Configs;

	class Settings
	{

		private static $_instance = NULL;


		public function __construct()
		{


			define('WEB_ROOT', str_replace('index.php','', $_SERVER['SCRIPT_NAME']));

			define('LINK_ROOT', str_replace('src/Public/index.php', '', 
				'http://localhost'.$_SERVER['SCRIPT_NAME']));

			define('APP_ROOT', str_replace('Public/index.php', 'Application/', 
				$_SERVER['SCRIPT_FILENAME']));

			define('LIB_ROOT', 
				str_replace('Public/index.php', 'Library/', $_SERVER['SCRIPT_FILENAME']));

			//Parametres de la BDD
			//define('DB_HOST', 'localhost');
		}

		//Singleton
		public static function getInstance()
		{
			if (is_null(self::$_instance))
			{
				self::$_instance = new self();				
			}

			return self::$_instance;

		}

	}

?>
