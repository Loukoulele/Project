namespace AppAnnuaire
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    using DllLibrary;

    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var gestionAnnuaire = new Annuaire();
            Console.WriteLine("voulez vous importer les contacts ?\n1- Oui\n2- Non");
            var yOrNo = Convert.ToInt32(Console.ReadLine());

            if (yOrNo == 1)
            {
                Console.WriteLine("Quel type de fichier contenant les contact voulez-vous importer ?\n1- .xml\n2- .dat");
                var xOrD = Convert.ToInt32(Console.ReadLine());

                if (xOrD == 1)
                {
                    // On ouvre une boite de dialogue pour que l'utilisateur puisse chercher son fichier.
                    var dialog = new OpenFileDialog
                                     {
                                         Multiselect = false,
                                         Title = "Open xml document",
                                         Filter = "XML File|*.xml"
                                     };
                    using (dialog)
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            var path = dialog.FileName;
                            gestionAnnuaire.ImportXml(path);
                        }
                    }
                }
                else if (xOrD == 2)
                {
                    var dialog = new OpenFileDialog
                                     {
                                         Multiselect = false,
                                         Title = "Open dat file",
                                         Filter = "DAT File|*.dat"
                                     };
                    using (dialog)
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            var path = dialog.FileName;
                            gestionAnnuaire.ImportDat(path);
                        }
                    }
                }
            }

            // on récupère le nombre de contact dans la liste de conctact, pour définir l'id pour les futurs contacts.
            var id = gestionAnnuaire.ListeContact.Count;
            string choixMenu;
            do
            {
                Console.WriteLine(
                    "---------- MENU -----------\n" + "1 - Lister l'ensemble des contacts\n"
                    + "2 - Ajouter/Supprimer/Modifier un contact\n" + "3 - Rechercher un contact\n"
                    + "4 - Trier les contacts (Nom, date de naissance ou mail)\n" + "5 - Quitter");

                choixMenu = Console.ReadLine();

                switch (choixMenu)
                {
                    case "1":
                        Console.WriteLine(gestionAnnuaire.Lister());
                        break;
                    case "2":
                        Console.WriteLine(
                            "Que voulez-vous faire ?\n" + "1 - Ajouter un contact\n" + "2 - Supprimer un contact\n"
                            + "3 - Modifier un contact");

                        var choix2 = Console.ReadLine();

                        switch (choix2)
                        {
                            case "1":
                                Console.WriteLine(
                                    "Entrer le type de contact : Privé ou Professionnel (respecter bien la synthaxe)");
                                var typeContact = Console.ReadLine();
                                if ((typeContact != "Privé") && (typeContact != "Professionnel"))
                                {
                                    Console.WriteLine("Veuillez choisir entre privé et professionnel !");
                                    break;
                                }

                                Console.WriteLine("Entrer le nom :");
                                var nom = Console.ReadLine();
                                Console.WriteLine("Entrer le Prenom :");
                                var prenom = Console.ReadLine();
                                string telephone;
                                var count = 0;
                                do
                                {
                                    Console.WriteLine("Entrer le numéro de téléphone : (Format :00-00-00-00-00)");
                                    telephone = Console.ReadLine();

                                    // On vérifie si le numéro de téléphone à bien 4 "-" et contient 14 caractères.
                                    const char Verif = '-';

                                    if (telephone == null) continue;
                                    count += telephone.Count(t => t == Verif);

                                    if (count != 4 && telephone.Length != 14) Console.WriteLine("Mauvais format du numéro, retaper");
                                }
                                while ((count != 4) && (telephone.Length != 14));

                                if (typeContact == "Privé")
                                {
                                    Console.WriteLine("Entrer la date de naissance : ");
                                    var date = Console.ReadLine();
                                    Console.WriteLine("Entrer l'age :");
                                    var age = Convert.ToInt32(Console.ReadLine());

                                    // On ajoute le contact de type pivé dans les listes.
                                    gestionAnnuaire.Ajouter(
                                        new ContactPriv(id, "Privé", nom, prenom, telephone, date, age));
                                    Console.WriteLine("contact ajouté ! ");
                                    gestionAnnuaire.ListeContactPriv.Add(
                                        new ContactPriv(id, "Privé", nom, prenom, telephone, date, age));
                                    id++;
                                }
                                else if (typeContact == "Professionnel")
                                {
                                    Console.WriteLine("Entrer l'adresse email :");
                                    var adresseMail = Console.ReadLine();
                                    Console.WriteLine("Entrer le nom de l'entreprise : ");
                                    var nomEntr = Console.ReadLine();

                                    // On ajoute le contact de type pro dans les listes.
                                    gestionAnnuaire.Ajouter(
                                        new ContactPro(id, "Pro", nom, prenom, telephone, adresseMail, nomEntr));
                                    gestionAnnuaire.ListeContactPros.Add(
                                        new ContactPro(id, "Privé", nom, prenom, telephone, adresseMail, nomEntr));
                                    Console.WriteLine("contact ajouté");
                                    id++;
                                }

                                break;

                            case "2":
                                Console.WriteLine("Quel est le contact que vous voulez supprimer ? (entrer l'ID)");
                                var supp = Convert.ToInt32(Console.ReadLine());
                                gestionAnnuaire.Supprimer(supp);
                                Console.WriteLine("Suppression réussi !");
                                break;

                            case "3":
                                Console.WriteLine("Quel est le contact à modifier ? (Entrer l'ID)");
                                var mod = Convert.ToInt32(Console.ReadLine());
                                try
                                {
                                    gestionAnnuaire.Modifier(mod);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Aucun contact trouvé avec cette ID");
                                }

                                break;

                            default:
                                Console.WriteLine("Veuillez choisir un choix dispo");
                                break;
                        }

                        break;

                    case "3":
                        gestionAnnuaire.Rechercher();
                        break;

                    case "4":
                        gestionAnnuaire.Trier();
                        break;

                    case "5":
                        Console.WriteLine("Voulez-vous sauvegarder les contacts avant de quitter ?\n-OUI\n-NON");
                        var choiceQ = Console.ReadLine();
                        switch (choiceQ)
                        {
                            case "OUI":
                                Console.WriteLine(
                                    "Dans quel format voulez-vous enregistrer les contacts ?\n1- XML\n2- DAT\n3- LES DEUX");
                                var sauv = Convert.ToInt32(Console.ReadLine());
                                switch (sauv)
                                {
                                    case 1:
                                        gestionAnnuaire.ExportXML();
                                        break;

                                    case 2:
                                        gestionAnnuaire.ExportDat();
                                        break;

                                    case 3:
                                        gestionAnnuaire.ExportXML();
                                        gestionAnnuaire.ExportDat();
                                        break;

                                    default:
                                        Console.WriteLine("Veuillez choisir entre les 3 choix proposé");
                                        break;
                                }

                                break;

                            case "NON":
                                Environment.Exit(0);
                                break;

                            default:
                                Console.WriteLine("Veuillez choisir entre OUI et NON");
                                break;
                        }

                        break;

                    default:
                        Console.WriteLine("Veuillez entrer un choix correct !");
                        break;
                }
            }
            while (choixMenu != "5");
            Console.ReadKey();
        }
    }
}