using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp_note_THOMAZET_Remi
{
    public class Dossier
    {
        public string Nom { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateModification { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Dossier> SousDossier { get; set; }
        public Dossier() { }

        public Dossier(string nom)
        {
            Nom = nom;
            DateCreation = DateTime.Now;
            DateModification = DateTime.Now;
            Contacts = new List<Contact>();
            SousDossier = new List<Dossier>();
        }
    }
}
