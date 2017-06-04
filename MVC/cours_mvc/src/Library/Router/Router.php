<?php

	namespace Library\Router;

	class Router
	{
		private static $_instance=NULL;


		//Singleton
		public static function getInstance()
		{
			if (is_null(self::$_instance))
			{
				self::$_instance = new self();				
			}

			return self::$_instance;

		}




		public function  __construct() {}


		static private function getControllerPath($name)
		{

			return APP_ROOT.'Controllers'.DIRECTORY_SEPARATOR.
				ucfirst(strtolower($name)).'.php';
		}


		static private function getControllerClassName($name)
		{

			return '\Application\Controllers\\'.
				ucfirst(strtolower($name));
		}

		static private function getActionName($name)
		{
			return strtolower($name);
		}


		static public function dispatchPage($url)
		{

			//Controleur & Action  par défaut
			$controller = self::getControllerClassName("Home");
			$action = self::getActionName("index");


			if (!empty($url[0]))
			{
				//Nom du Controleur
				if (file_exists(self::getControllerPath($url[0])) && class_exists(self::getControllerClassName($url[0])))
				{
					$controller = self::getControllerClassName($url[0]);
					array_splice($url, 0, 1);

				}
				else
				{
					$controller = self::getControllerClassName('error');
				}
				$controller = new $controller();


				//Action
				if (!empty($url[0]))
				{
					if (method_exists($controller, $url[0]))
					{
						$action=self::getActionName($url[0]);
					}
					array_splice($url, 0, 1);
				}

				/*
					Executer l'action du Controleur
					Execute le Rendu de l'Action
				*/
				call_user_func_array(array($controller, $action), $url);
				call_user_func_array(array($controller, 'renderView'),
						array('controller ' => get_class($controller),
								'action' => $action));

				//Suppression des Variables
				unset($controller, $action);



			}






		}









	}




?>
