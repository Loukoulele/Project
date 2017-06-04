// modeleImage.js
// MODELE "Image"

function Image(fichier, categorie, legende){
	this.fichier = fichier;
	this.categorie = categorie;
	this.legende = legende;
	this.showInfos = function(){console.log("fichier:" + this.fichier + "categorie:" + this.categorie + "legende:" + this.legende);}
}
