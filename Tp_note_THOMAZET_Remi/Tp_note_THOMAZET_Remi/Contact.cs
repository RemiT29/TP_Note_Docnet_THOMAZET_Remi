using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp_note_THOMAZET_Remi
{
    public class Contact
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Couriel { get; set; }
        public string Societe { get; set; }
        public Liens Lien { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateModification { get; set; }
        public Contact() { }

        public Contact(string nom, string prenom, string couriel, string societe, Liens lien)
        {
            Nom = nom;
            Prenom = prenom;
            Couriel = couriel;
            Societe = societe;
            Lien = lien;
            DateCreation = DateTime.Now;
            DateModification = DateTime.Now;
        }
    }
}
