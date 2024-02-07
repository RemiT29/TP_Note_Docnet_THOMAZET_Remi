using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tp_note_THOMAZET_Remi
{
    public class Serialized
    {
        // Méthode pour sérialiser un objet de type T vers un fichier chiffré
        public void SerializedToFile<T>(T data, string fichier, string cleCryptage)
        {
            // Création d'un objet XmlSerializer pour la sérialisation XML
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            // Création d'un objet Aes pour le chiffrement AES
            using (Aes aesAlg = Aes.Create())
            {
                // Conversion de la clé de cryptage en tableau de bytes et ajustement à la taille requise
                byte[] keyBytes = Encoding.UTF8.GetBytes(cleCryptage.PadRight(32));
                aesAlg.Key = keyBytes.Take(aesAlg.KeySize / 8).ToArray();
                aesAlg.IV = new byte[16];  // Initialisation du vecteur d'initialisation (IV) à zéro

                // Création d'un transformateur de chiffrement en mode chiffrement
                ICryptoTransform cryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Ouverture d'un flux de fichier en mode création
                using (FileStream filestream = new FileStream(fichier, FileMode.Create))
                {
                    // Création d'un flux de chiffrement pour écrire dans le fichier
                    using (CryptoStream cryptostream = new CryptoStream(filestream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        // Création d'un objet TextWriter pour écrire dans le flux de chiffrement
                        using (TextWriter writter = new StreamWriter(cryptostream))
                        {
                            // Sérialisation de l'objet data dans le fichier chiffré
                            xmlSerializer.Serialize(writter, data);
                        }
                    }
                }
            }
        }

        // Méthode pour désérialiser un objet de type T depuis un fichier chiffré
        public T DeserializedToFile<T>(string fichier, string cleDecryptage)
        {
            // Création d'un objet XmlSerializer pour la désérialisation XML
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            // Création d'un objet Aes pour le déchiffrement AES
            using (Aes aesAlg = Aes.Create())
            {
                // Conversion de la clé de décryptage en tableau de bytes et ajustement à la taille requise
                byte[] keyBytes = Encoding.UTF8.GetBytes(cleDecryptage.PadRight(32));
                aesAlg.Key = keyBytes.Take(aesAlg.KeySize / 8).ToArray();
                aesAlg.IV = new byte[16];  // Initialisation du vecteur d'initialisation (IV) à zéro

                // Création d'un transformateur de chiffrement en mode déchiffrement
                ICryptoTransform cryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Ouverture d'un flux de fichier en mode lecture
                using (FileStream filestream = new FileStream(fichier, FileMode.Open))
                {
                    // Création d'un flux de chiffrement pour lire depuis le fichier
                    using (CryptoStream cryptostream = new CryptoStream(filestream, cryptoTransform, CryptoStreamMode.Read))
                    {
                        // Création d'un objet TextReader pour lire depuis le flux de chiffrement
                        using (TextReader reader = new StreamReader(cryptostream))
                        {
                            // Désérialisation de l'objet depuis le fichier chiffré
                            return (T)xmlSerializer.Deserialize(reader);
                        }
                    }
                }
            }
        }
    }
}
