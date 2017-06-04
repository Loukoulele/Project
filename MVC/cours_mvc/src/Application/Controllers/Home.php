<?php
	namespace Application\Controllers;
	class Home extends \Library\Controller\Controller
	{

		public function __construct()
		{
			parent::__construct();
		}

		public function index()
		{
			$this->setDataView(array('pageTitle' => 'Home - Index'));
		}




	}
	



?>