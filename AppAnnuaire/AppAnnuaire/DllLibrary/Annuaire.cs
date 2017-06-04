namespace DllLibrary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    /// <summary>
    ///     La classe annuaire gérant les contacts.
    /// </summary>
    public class Annuaire
    {
        /// <summary>
        ///     La list qui stock les contacts.
        /// </summary>
        public readonly List<Contact> ListeContact = new List<Contact>();

        /// <summary>
        ///     The list contact priv.
        /// </summary>
        public readonly List<ContactPriv> ListeContactPriv = new List<ContactPriv>();

        /// <summary>
        ///     The list contact pros.
        /// </summary>
        public readonly List<ContactPro> ListeContactPros = new List<ContactPro>();

        #region Fonctions Import/Export XML -- Import/Export DAT
        /// <summary>
        ///     Exporter les contacts dans le fichier XML.
        /// </summary>
        public void ExportXML()
        {
            var count = 0;

            // Création du fichier XML et insertion des contacts.
            using (var writer = XmlWriter.Create("contact.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Annuaire");

                foreach (var contact in this.ListeContact)
                {
                    var contactPriv = this.ListeContact[count] as ContactPriv;
                    var contactPro = this.ListeContact[count] as ContactPro;

                    writer.WriteStartElement("Contact_" + count);
                    writer.WriteElementString("ID", contact.Id.ToString());
                    writer.WriteElementString("TypeDeContact", contact.TypeContact);
                    writer.WriteElementString("Nom", contact.Nom);
                    writer.WriteElementString("Prénom", contact.Prenom);
                    writer.WriteElementString("Telephone", contact.Telephone);

                    // Si le contact est de type Privé, on créer les noeuds DateDeNaissance et Age, si il est de type pro, on créer les noeuds AdresseMail et NomEntreprise
                    if (this.ListeContact[count].TypeContact == "Privé")
                    {
                        writer.WriteElementString("DateDeNaissance", contactPriv.DateDeNaissance);
                        writer.WriteElementString("Age", contactPriv.Age.ToString());
                    }
                    else if (this.ListeContact[count].TypeContact == "Pro")
                    {
                        writer.WriteElementString("AdresseMail", contactPro.AdresseMail);
                        writer.WriteElementString("NomEntreprise", contactPro.NomEntreprise);
                    }

                    writer.WriteEndElement();
                    count++;
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        ///     Importer les contacts d'un fichier XML.
        /// </summary>
        public void ImportXml(string chemin)
        {
            var doc = new XmlDocument();
            doc.Load(chemin);

            // condition if avec i < que tous les noeuds enfant du noeud Annuaire dans le document XML.
            for (var i = 0; i < doc["Annuaire"].ChildNodes.Count; i++)
            {
                // on selectionne un noeud spécifique que l'on met dans une variable string.

                // récupère la donnée dans le noeud "Annuaire/Contact_" + i + "/ID" (ici l'id).
                var xmlId = Convert.ToInt32(doc.SelectSingleNode("Annuaire/Contact_" + i + "/ID").InnerText);
                var xmlType = doc.SelectSingleNode("Annuaire/Contact_" + i + "/TypeDeContact").InnerText;
                var xmlNom = doc.SelectSingleNode("Annuaire/Contact_" + i + "/Nom").InnerText;
                var xmlPrenom = doc.SelectSingleNode("Annuaire/Contact_" + i + "/Prénom").InnerText;
                var xmlTelephone = doc.SelectSingleNode("Annuaire/Contact_" + i + "/Telephone").InnerText;

                // Si le type de contact est privé, on cherche les noeuds "DateDeNaissance" et Age sinon si le contact est pro, on cherche les noeuds AdresseMail et Age.
                if (xmlType == "Privé")
                {
                    var xmlDate = doc.SelectSingleNode("Annuaire/Contact_" + i + "/DateDeNaissance").InnerText;
                    var xmlAge = Convert.ToInt32(doc.SelectSingleNode("Annuaire/Contact_" + i + "/Age").InnerText);

                    // Ajoute le contact de type Privée dans la liste annuaire.
                    this.Ajouter(
                        new ContactPriv(xmlId, xmlType, xmlNom, xmlPrenom, xmlTelephone, xmlDate, xmlAge));
                    this.ListeContactPriv.Add(
                        new ContactPriv(xmlId, xmlType, xmlNom, xmlPrenom, xmlTelephone, xmlDate, xmlAge));
                }
                else if (xmlType == "Pro")
                {
                    var xmlMail = doc.SelectSingleNode("Annuaire/Contact_" + i + "/AdresseMail").InnerText;
                    var xmlEntreprise = doc.SelectSingleNode("Annuaire/Contact_" + i + "/NomEntreprise").InnerText;

                    // Ajoute le contact de type Pro dans la liste annuaire.   
                    this.Ajouter(
                        new ContactPro(xmlId, xmlType, xmlNom, xmlPrenom, xmlTelephone, xmlMail, xmlEntreprise));
                    this.ListeContactPros.Add(
                        new ContactPro(xmlId, xmlType, xmlNom, xmlPrenom, xmlTelephone, xmlMail, xmlEntreprise));
                }
            }
        }

        /// <summary>
        ///     Exporter les contacts dans un fichier DAT
        /// </summary>
        public void ExportDat()
        {
            var count = 0;

            using (var sw = new StreamWriter("contactDat.txt"))
            {
                foreach (var contact in this.ListeContact)
                {
                    var contactPriv = this.ListeContact[count] as ContactPriv;
                    var contactPro = this.ListeContact[count] as ContactPro;

                    sw.WriteLine(contact.Id);
                    sw.WriteLine(contact.TypeContact);
                    sw.WriteLine(contact.Nom);
                    sw.WriteLine(contact.Prenom);
                    sw.WriteLine(contact.Telephone);
                    if (contact.TypeContact == "Privé")
                    {
                        sw.WriteLine(contactPriv.DateDeNaissance);
                        sw.WriteLine(contactPriv.Age);
                    }
                    else if (contact.TypeContact == "Pro")
                    {
                        sw.WriteLine(contactPro.AdresseMail);
                        sw.WriteLine(contactPro.NomEntreprise);
                    }

                    count++;
                }
            }

            if (File.Exists("contactDat.dat"))
            {
                File.Delete("contactDat.dat");
                File.Move("contactDat.txt", "contactDat.dat");
            }
            else
            {
                File.Move("contactDat.txt", "contactDat.dat");
            }
        }

        /// <summary>
        ///     Importer les contacts d'un fichier DAT.
        /// </summary>
        /// <param name="chemin">
        ///     Chemin du fichier.
        /// </param>
        public void ImportDat(string chemin)
        {
            var i = 0;
            var reader = new StreamReader(chemin);
            var strAllFile = reader.ReadToEnd().Replace("\r\n", "\n").Replace("\n\r", "\n");
            var arrLines = strAllFile.Split('\n');

            foreach (var t in arrLines)
            {
                // On verifie si la chaine de caractère est vide, si c'est le cas on break.
                if (arrLines[i] == string.Empty)
                {
                    break;
                }

                var id = Convert.ToInt32(arrLines[i]);
                var datType = arrLines[i + 1];
                var datNom = arrLines[i + 2];
                var datPrenom = arrLines[i + 3];
                var datTelephone = arrLines[i + 4];

                if (datType == "Privé")
                {
                    var datDate = arrLines[i + 5];
                    var datAge = Convert.ToInt32(arrLines[i + 6]);
                    this.Ajouter(new ContactPriv(id, datType, datNom, datPrenom, datTelephone, datDate, datAge));
                    this.ListeContactPriv.Add(
                        new ContactPriv(id, datType, datNom, datPrenom, datTelephone, datDate, datAge));
                    i += 7;
                }
                else if (datType == "Pro")
                {
                    var datMail = arrLines[i + 5];
                    var datEntreprise = arrLines[i + 6];
                    this.Ajouter(
                        new ContactPro(id, datType, datNom, datPrenom, datTelephone, datMail, datEntreprise));
                    this.ListeContactPros.Add(
                        new ContactPro(id, datType, datNom, datPrenom, datTelephone, datMail, datEntreprise));
                    i += 7;
                }
            }

            reader.Close();
        }
        #endregion

        #region Fonctions pour gérer les contacts
        /// <summary>
        ///     lister les contacts.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string Lister()
        {
            var ch = string.Empty;
            foreach (var t in this.ListeContact)
            {
                ch += Environment.NewLine;
                ch += t.Affiche() + "\n";
                ch += Environment.NewLine + "---------------------\n";
            }

            return ch;
        }

        /// <summary>
        ///     Ajouter un contact.
        /// </summary>
        /// <param name="contact">
        ///     Le contact.
        /// </param>
        public void Ajouter(Contact contact)
        {
            this.ListeContact.Add(contact);
        }

        /// <summary>
        ///     Modifier un contact.
        /// </summary>
        /// <param name="modif">
        ///     La valeur int ou se situe le contact dans la list.
        /// </param>
        /// <param name="contact">
        ///     The contact.
        /// </param>
        public void Modifier(int modif)
        {
            // On cherche le contact avec l'id entré par l'utilisateur.
            var change = this.ListeContact.FirstOrDefault(d => d.Id == modif);
            var changePriv = this.ListeContactPriv.FirstOrDefault(d => d.Id == modif);
            var changePro = this.ListeContactPros.FirstOrDefault(d => d.Id == modif);

            var contactPrivMod = change as ContactPriv;
            var contactProMod = change as ContactPro;

            Console.Write("que voulez vous changer ?\n" + "1 - Nom\n" + "2 - Prénom\n" + "3 - Téléphone\n");
            if (this.ListeContact[modif].TypeContact == "Privé")
            {
                Console.WriteLine("4 - Date de naissance\n5 - Age");
            }
            else if (this.ListeContact[modif].TypeContact == "Pro")
            {
                Console.WriteLine("4 - Adresse mail\n5- Nom de l'entreprise");
            }

            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    Console.WriteLine("Entrer le nouveau nom : ");
                    var newName = Console.ReadLine();
                    change.Nom = newName;
                    changePriv.Nom = newName;
                    changePro.Nom = newName;
                    break;

                case "2":
                    Console.WriteLine("Entrer le nouveau prénom : ");
                    var newPre = Console.ReadLine();
                    change.Prenom = newPre;
                    changePriv.Prenom = newPre;
                    changePro.Prenom = newPre;
                    break;

                case "3":
                    var count = 0;
                    string newPhone;
                    do
                    {
                        Console.WriteLine("Entrer le numéro de téléphone : (Format :00-00-00-00-00)");
                        newPhone = Console.ReadLine();
                        const char Verif = '-';

                        count += newPhone.Count(t => t == Verif);

                        if (count != 4 && newPhone.Length != 14) Console.WriteLine("Mauvais format du numéro, retaper");
                        else
                        {
                            change.Telephone = newPhone;
                            changePriv.Telephone = newPhone;
                            changePro.Telephone = newPhone;
                        }
                    }
                    while ((count != 4) && (newPhone.Length != 14));

                    break;

                case "4":
                    if (this.ListeContact[modif].TypeContact == "Privé")
                    {
                        Console.WriteLine("Entrez la nouvelle date de naissance : ");
                        var newDate = Console.ReadLine();
                        contactPrivMod.DateDeNaissance = newDate;
                        changePriv.DateDeNaissance = newDate;
                    }
                    else if (this.ListeContact[modif].TypeContact == "Pro")
                    {
                        Console.WriteLine("Entrez la nouvelle adresse email");
                        var newMail = Console.ReadLine();
                        contactProMod.AdresseMail = newMail;
                        changePro.AdresseMail = newMail;
                    }

                    break;

                case "5":
                    if (this.ListeContact[modif].TypeContact == "Privé")
                    {
                        Console.WriteLine("Entrez le nouvel age :");
                        var newAge = Convert.ToInt32(Console.ReadLine());
                        contactPrivMod.Age = newAge;
                        changePriv.Age = newAge;
                    }
                    else if (this.ListeContact[modif].TypeContact == "Pro")
                    {
                        Console.WriteLine("Entrez le nouveau nom de l'entreprise");
                        var newEntr = Console.ReadLine();
                        contactProMod.NomEntreprise = newEntr;
                        changePro.NomEntreprise = newEntr;
                    }

                    break;

                default:
                    Console.WriteLine("Veuillez selectionner un choix possible");
                    break;
            }
        }


        /// <summary>
        ///     Recherche des contact.
        /// </summary>
        public void Rechercher()
        {
            Console.WriteLine(
                "Comment voulez-vous effectuer votre recherche ?\n" + "1- Par nom\n" + "2- Par nom d'entreprise\n"
                + "3- Par téléphone\n" + "4- Par type de contact\n");
            var choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    Console.WriteLine("Entrer le nom à rechercher :");
                    var nom = Console.ReadLine();

                    // On recherche le contact avec le nom et on l'affiche sinon on retourne une erreur.
                    try
                    {
                        Console.WriteLine(this.ListeContact.Find(x => x.Nom.Equals(nom)).Affiche());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Erreur: Aucun contact avec ce nom trouvé");
                    }

                    break;

                case "2":
                    Console.WriteLine("Entrez le nom de l'entreprise :");
                    var nameEntrise = Console.ReadLine();
                    try
                    {
                        Console.WriteLine(
                            this.ListeContactPros.Find(x => x.NomEntreprise.Equals(nameEntrise)).Affiche());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Erreur: Aucun contact trouvé avec le nom d'entreprise entré");
                    }

                    break;

                case "3":
                    Console.WriteLine("Entrez le n° de téléphone :");
                    var numéro = Console.ReadLine();
                    try
                    {
                        Console.WriteLine(this.ListeContact.Find(x => x.Telephone.Equals(numéro)).Affiche());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Erreur: Aucun contact trouvé avec ce numéro");
                    }

                    break;

                case "4":
                    Console.WriteLine(
                        "Quel type de contact voulez-vous lister ?\n" + "1- Privé\n" + "2- Professionnel\n");
                    choix = Console.ReadLine();
                    if (choix == "1")
                    {
                        // On liste tous les contacts qui sont de type Privé.
                        foreach (var j in this.ListeContactPriv)
                        {
                            Console.WriteLine(Environment.NewLine + j.Affiche());
                            Console.WriteLine(Environment.NewLine + "---------------------");
                        }
                    }
                    else if (choix == "2")
                    {
                        // On liste tous les contacts qui sont de type Pro.
                        foreach (var j in this.ListeContactPros)
                        {
                            Console.WriteLine(Environment.NewLine + j.Affiche());
                            Console.WriteLine(Environment.NewLine + "---------------------");
                        }
                    }

                    break;

                default:
                    Console.WriteLine("Veuillez selectionner un choix proposé");
                    break;
            }
        }

        /// <summary>
        ///     Trier les contacts.
        /// </summary>
        public void Trier()
        {
            Console.WriteLine("Comment voulez-vous trier les contacts ?:\n1- Par nom\n2- Par date de naissance\n3- Par adresse mail");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    var trierParNom = this.ListeContact.OrderBy(o => o.Nom).ToList();
                    foreach (var t in trierParNom)
                    {
                        Console.WriteLine(Environment.NewLine + t.Affiche());
                        Console.WriteLine(Environment.NewLine + "---------------------");
                    }

                    break;

                case "2":
                    var trierParN = this.ListeContactPriv.OrderBy(o => o.DateDeNaissance).ToList();
                    foreach (var t in trierParN)
                    {
                        Console.WriteLine(Environment.NewLine + t.Affiche());
                        Console.WriteLine(Environment.NewLine + "---------------------");
                    }

                    break;

                case "3":
                    var trierParM = this.ListeContactPros.OrderBy(o => o.AdresseMail).ToList();
                    foreach (var t in trierParM)
                    {
                        Console.WriteLine(Environment.NewLine + t.Affiche());
                        Console.WriteLine(Environment.NewLine + "---------------------");
                    }

                    break;

                default:
                    Console.WriteLine("Veuillez selectionner un choix proposé");
                    break;
            }
        }

        /// <summary>
        ///     Supprimmer un contact de l'annuaire.
        /// </summary>
        /// <param name="supp">
        ///     L'id du contact à supprimmer.
        /// </param>
        public void Supprimer(int supp)
        {
            this.ListeContact.RemoveAt(supp);
            if (this.ListeContact[supp].TypeContact == "Privé")
            {
                this.ListeContactPriv.Remove(this.ListeContactPriv.Find(o => o.Id.Equals(supp)));
            }
            else if (this.ListeContact[supp].TypeContact == "Pro")
            {
                this.ListeContactPros.Remove(this.ListeContactPros.Find(o => o.Id.Equals(supp)));
            }
        }
        #endregion
    }
}