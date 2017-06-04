<?php


namespace Library\Controller;


abstract class Controller implements iController
{

	private $dataView = array(
								'siteName' => "BTS - MVC",
								'pageTitle' => 'Titre de la Page'

							);



	public function __construct()
	{}


	public function setDataView(array $data)
	{
		$this->dataView = array_merge($this->dataView, $data);
	}



	public function getDataView()
	{
		return $this->dataView;
	}


	public function renderView($controller , $action)
	{
		$pathView = APP_ROOT.'Views/Controllers/'.str_replace('Application\\Controllers\\', '', $controller).'/'
			.str_replace('Action','', $action).".phtml";

			if (file_exists($pathView))
			{
				extract($this->getDataView());

				ob_start();
				include_once($pathView);

				$viewContent = ob_get_clean();






			}



	}











}






?>



