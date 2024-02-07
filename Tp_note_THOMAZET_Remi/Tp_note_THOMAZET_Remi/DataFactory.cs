using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp_note_THOMAZET_Remi
{
    public interface IDataFactory
    {
        Dossier NouveauDossier(string nom);
        Contact NouveauContact(string nom, string prenom, string couriel, string societe, Liens lien);
    }
    public class DataFactory : IDataFactory
    {
        public Dossier NouveauDossier(string nom)
        {
            return new Dossier(nom);
        }

        public Contact NouveauContact(string nom, string prenom, string couriel, string societe, Liens lien)
        {
            return new Contact(nom, prenom, couriel, societe, lien);
        }
    }
}
