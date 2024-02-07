using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp_note_THOMAZET_Remi
{
    class Program
    {
        // Methode principale du programme, qui va permettre l'intéraction avec l'utilisateur
        static void Main(string[] args)
        {
            ContactManager contactManager = new ContactManager();


            //Affichage des commandes disponibles 

            Console.WriteLine("Liste des commandes : ");
            Console.WriteLine("  - afficher");
            Console.WriteLine("  - sortir");
            Console.WriteLine("  - cd nomDuFichier pour descendre et cd ../ pour remonter");
            Console.WriteLine("  - enregistrer ");
            Console.WriteLine("  - charger");
            Console.WriteLine("  - ajouterdossier nomDossier");
            Console.WriteLine("  - ajoutercontact Nom Prenom Email Société Lien (sans espace a la fin)");
            Console.WriteLine("  - deleteall");

            // Boucle infinie tant que l'utilisateur ne tape pas la commande "sortir"

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(">");
                string commande = Console.ReadLine();
                Console.ResetColor();

                string[] mots = commande.Split(' ');

                switch (mots[0])
                {
                    case "afficher":
                        contactManager.Afficher();
                        break;
                    case "sortir":
                        Environment.Exit(0);
                        break;
                    case "cd":
                        contactManager.Cd(mots[1]);
                        break;
                    case "enregistrer":
                        contactManager.EnregisterData();
                        break;
                    case "charger":
                        contactManager.ChargerData();
                        break;
                    case "ajouterdossier":
                        contactManager.NouveauDossier(mots[1]);
                        break;
                    case "ajoutercontact":
                        Liens l = new Liens();
                        switch (mots[mots.Length - 1])
                        {
                            case "ami":
                                l = Liens.ami;
                                break;
                            case "collegue":
                                l = Liens.collegue;
                                break;
                            case "relation":
                                l = Liens.relation;
                                break;
                            case "reseau":
                                l = Liens.reseau;
                                break;
                            default:
                                Console.WriteLine("Relation qui n'existe pas, par défault elle sera reseau");
                                l = Liens.reseau;
                                break;
                        }
                        string entreprise = "";
                        for (int i = 4; i < mots.Length - 1; i++)
                        {
                            entreprise = entreprise + mots[i] + " ";
                        }
                        contactManager.NouveauContact(mots[1], mots[2], mots[3], entreprise, l);
                        break;
                    case "deleteall":
                        contactManager.deleteAll();
                        break;
                    default:
                        Console.WriteLine("Commande inconnue. Veuillez entrer une commande connue");
                        break;
                }
            }
        }
    }
}
