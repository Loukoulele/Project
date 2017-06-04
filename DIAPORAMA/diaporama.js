// galerie.js

	onload=init;

//Métier
	var listeFinale = [];// la collection finale d'objets de type 'image'

//une reference vers le miniteur
	var zeTimer = null;
	var nbImages = null;
	var imageLue = 0;
	
	
/***********************/
 /* fonction init*/
/**********************/

//Gerer la fonction init
function init(){
	// pour initialiser des evenements...
	
		document.getElementById("bt_precedent").onclick = previous;
		//document.getElementById("bt_play").onclick = play;
		//document.getElementById("bt_stop").onclick = stop;
		document.getElementById("bt_suivant").onclick = next;
		ajaxCall();
}


//Gerer la fonction ajaxCall
function ajaxCall(){
	//gere la requete http
	
	//on instancie un objet XMLHttpRequest
		var xhr = new XMLHttpRequest();
	
	// on ouvre une connexion
		xhr.open("GET", "galerie_images.xml", true); // xhr: objet ajax
	
	//On envoie la requete
		xhr.send();//renvoi la valeur 4 readyState la reponse est prete 
	
	// on ecoute tout changement de readyState
		xhr.onreadystatechange = function() { 
	
			if(xhr.readyState == 4 && (xhr.status == 200 || xhr.status == 0)){ // statut de serveur ok a 200
				var data = xhr.responseXML; // fichier responseText   data: float donné
			
				// on traite la reponse 
					process(data);
		}//FIN IF
		
	}// FIN onreadystatechange
	
	//alert(xhr);	
}


//Gerer la fonction process
function process(data){
	//on decortique(parse/analyse) le flux xml
	// pour chaque tag 'image' on crée un objet (js) 'image' et on le nourrit, et on l'enregistre finalement dans une liste
	
		var listeTagsImages = data.getElementsByTagName("image");
	
	// pour chaque noeud image, on crée un objet Image
		for (var i = 0; i<listeTagsImages.length; i++){
	
	//On récupère la valeur de l'attribut categorie
		var categorie = listeTagsImages[i].getAttribute("categorie");
	
	// Stockage de la valeur du contenu du tag fichier
		var fichier = listeTagsImages[i].getElementsByTagName("fichier")[0].firstChild.nodeValue;
	
	// Stockage de la valeur du contenu du tag legende
		var legende = listeTagsImages[i].getElementsByTagName("legende")[0].firstChild.nodeValue;
	
	//On crée un objet Image à partir du modèle Image
		var objetImage = new Image(fichier, categorie, legende);
		console.log(objetImage);
	
	// on ajoute l'objet à la liste finale
		listeFinale.push(objetImage);
	}	
	// connaitre le nombre d'image mais avec le compteur à 0 donc length -1
		nbImages = listeFinale.length -1;
		showImage();
	
}//FIN process


// Gere la fonction showImage
function showImage(){
	
	//
		var image = listeFinale[imageLue];

	//creation de la string representant le chemin de l'image
		var chemin="galeries/" + image.categorie + "/big/"+ image.fichier;
	
	
		document.getElementById("screen").src = chemin;


}//FIN showImage



// Gere la fonction next
function next(){
	
	//On incrémente imageLue
	imageLue++;
	
	//On teste si ce n'est pas la dernière
		if(imageLue > nbImages){
			imageLue = 0;
		}
		
	// on affiche la suivante
		showImage();

}//FIN next

// Gere la fonction previous
function previous(){
	
	//On décrémente imageLue
	imageLue--;
	
	//On teste si ce n'est pas la dernière
		if(imageLue == 0){
			imageLue = nbImages;
		}
		
	// on affiche la suivante
		showImage();
		
}//FIN previous


