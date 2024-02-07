using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Tp_note_THOMAZET_Remi
{
    // Définition de la classe ContactManager
    public class ContactManager
    {
        private Dossier root;
        private Dossier courant;
        private DataFactory dataFactory;
        private Serialized serialized;
        private List<Dossier> cheminparcouru;

        private string cleCryptage;
        private string cleDecryptage;

        public ContactManager()
        {
            cheminparcouru = new List<Dossier>();

            dataFactory = new DataFactory();
            serialized = new Serialized();

            cleCryptage = getCleCryptage();
            Charger();
        }

        // Méthode pour créer un nouveau dossier
        public void NouveauDossier(string nom)
        {
            // Vérification et création de la liste des sous-dossiers si elle n'existe pas encore
            if (courant.SousDossier == null)
            {
                courant.SousDossier = new List<Dossier>();
            }

            Dossier newDossier = dataFactory.NouveauDossier(nom);

            courant.SousDossier.Add(newDossier);

            Console.WriteLine($"Dossier '{newDossier.Nom}' ajouté sous {courant.Nom} en position {courant.SousDossier.Count}.");
        }

        // Méthode pour créer un nouveau contact
        public void NouveauContact(string nom, string prenom, string couriel, string societe, Liens lien)
        {
            // Vérification et création de la liste des contacts si elle n'existe pas encore
            if (courant.Contacts == null)
            {
                courant.Contacts = new List<Contact>();
            }

            Contact newContact = dataFactory.NouveauContact(nom, prenom, couriel, societe, lien);

            courant.Contacts.Add(newContact);

            Console.WriteLine($"Contact '{nom}' ajouté sous '{courant.Nom}' en position '{courant.Contacts.Count}'.");
        }

        // Méthode pour obtenir la clé de cryptage basée sur l'identité de l'utilisateur
        public string getCleCryptage()
        {
            // Récupération de l'identité de l'utilisateur actuel afin de l'utiliser comme clé de cryptage
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            string name = windowsIdentity.Name;
            byte[] inputBytes = Encoding.UTF8.GetBytes(name);

            // Utilisation de l'algorithme SHA256 pour générer une clé de cryptage
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Méthode pour charger les données
        public void Charger()
        {
            // Initialisation du dossier racine et du dossier courant
            root = dataFactory.NouveauDossier("Root");
            courant = root;
        }

        // Méthode pour charger les données à partir d'un fichier
        public void ChargerData()
        {
            // Nom du fichier de données
            string fichierData = "listeContact.xml";

            // Vérification de l'existence du fichier de données
            if (File.Exists(fichierData))
            {
                try
                {
                    // Vérification et récupération de la clé de décryptage
                    if (string.IsNullOrEmpty(cleDecryptage))
                    {
                        cleDecryptage = getCleCryptage();
                    }

                    // Désérialisation des données à partir du fichier
                    root = serialized.DeserializedToFile<Dossier>(fichierData, cleDecryptage);
                    courant = root;

                    Console.WriteLine("Chargement de data effectué");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur lors du chargement " + e.ToString());
                }
            }
            else
            {
                Console.WriteLine("Fichier inexistant");
            }
        }

        // Méthode pour afficher la structure des dossiers et contacts
        public void Afficher()
        {
            // Appel de la méthode récursive pour afficher la structure à partir du dossier racine
            AfficherStructure(root, 0);
        }

        // Méthode récursive pour afficher la structure des dossiers et contacts
        public void AfficherStructure(Dossier dossier, int espace)
        {
            // Création d'une chaîne de caractères représentant l'espacement
            string tab = new string(' ', espace * 3);

            // Affichage du dossier actuel avec sa date de création
            Console.WriteLine($"{tab}[D] {dossier.Nom} (création {dossier.DateCreation})");

            // Affichage des contacts du dossier actuel s'ils existent
            if (dossier.Contacts != null)
            {
                foreach (Contact contact in dossier.Contacts)
                {
                    // Affichage des informations du contact
                    Console.WriteLine($"{tab}| [C] {contact.Prenom} {contact.Nom} ({contact.Societe}) Email: {contact.Couriel} Lien: {contact.Lien} ");
                }
            }

            // Appel récursif pour afficher la structure des sous-dossiers du dossier actuel
            if (dossier.SousDossier != null)
            {
                foreach (Dossier d in dossier.SousDossier)
                {
                    AfficherStructure(d, espace + 1);
                }
            }
        }

        // Méthode pour changer de dossier (se déplacer dans la hiérarchie)
        public void Cd(string nomFichier)
        {
            // Vérification si l'utilisateur souhaite remonter d'un niveau
            if (nomFichier == "../")
            {
                // Vérification de la présence de dossiers parcourus
                if (cheminparcouru.Count() >= 1)
                {
                    // Retour au dossier précédent et mise à jour de la liste des dossiers parcourus
                    courant = cheminparcouru.Last();
                    cheminparcouru.RemoveAt(cheminparcouru.Count - 1);
                    Console.WriteLine("Retour en arrière");
                }
                else
                {
                    Console.WriteLine("Impossible de revenir en arrière");
                }
            }
            else
            {
                Dossier dossier = null;

                // Recherche du dossier correspondant au nom spécifié par l'utilisateur
                foreach (Dossier d in courant.SousDossier)
                {
                    if (d.Nom == nomFichier)
                    {
                        dossier = d;
                    }
                }

                // Vérification de l'existence du dossier
                if (dossier != null)
                {
                    // Enregistrement du dossier courant dans la liste des dossiers parcourus
                    cheminparcouru.Add(courant);

                    // Déplacement vers le nouveau dossier
                    courant = dossier;
                    Console.WriteLine("Déplacement effectué");
                }
                else
                {
                    Console.WriteLine("Dossier inexistant");
                }
            }
        }

        // Méthode pour enregistrer les données dans un fichier
        public void EnregisterData()
        {
            string fichierSave = "listeContact.xml";

            // Vérification de l'existence du fichier de sauvegarde
            if (!File.Exists(fichierSave))
            {
                // Création du fichier s'il n'existe pas encore
                using (FileStream nouvFichier = File.Create(fichierSave))
                {
                    nouvFichier.Close();
                }
            }

            // Sérialisation des données dans le fichier de sauvegarde
            serialized.SerializedToFile(root, fichierSave, cleCryptage);

            Console.WriteLine("Data enregistrées");
        }

        // Méthode pour tout supprimer et revenir à un fichier racine uniquement
        public void deleteAll()
        {
            Charger();
        }
    }
}
