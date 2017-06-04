namespace DllLibrary
{
    /// <summary>
    ///     La classe privée.
    /// </summary>
    public class ContactPriv : Contact
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ContactPriv" /> class.
        /// </summary>
        /// <param name="id">
        ///     l'id.
        /// </param>
        /// <param name="n">
        ///     Le nom.
        /// </param>
        /// <param name="p">
        ///     Le prénom.
        /// </param>
        /// <param name="t">
        ///     Le téléphone.
        /// </param>
        /// <param name="c">
        ///     Le type de contact.
        /// </param>
        /// <param name="d">
        ///     La Date de naissance.
        /// </param>
        /// <param name="a">
        ///     L'âge.
        /// </param>
        public ContactPriv(int id, string c, string n, string p, string t, string d, int a)
            : base(id, n, p, t, c)
        {
            this.DateDeNaissance = d;
            this.Age = a;
        }

        /// <summary>
        ///     Gets or sets the age.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        ///     Gets or sets the date de naissance.
        /// </summary>
        public string DateDeNaissance { get; set; }

        /// <summary>
        ///     Affiche les infos de Contact + les infos ContactsPriv.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string Affiche()
        {
            return this.Infos() + $" Date de naissance : {this.DateDeNaissance} \n Age : {this.Age}";
        }
    }
}