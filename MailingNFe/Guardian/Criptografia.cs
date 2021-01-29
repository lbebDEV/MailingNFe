using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MailingNFe.Guardian
{
    class Criptografia
    {
        private static readonly byte[] _byte =
         { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18,
                0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };

        /// <summary>
        /// Chave de criptografia
        /// </summary>
        private const string chave = "Gd3X1qbjSyNkaiQJN3FJusdaI4rtyokC";

        /// <summary>
        /// Criptografar valor informado como parâmetro.
        /// </summary>
        /// <param name="valor">Valor para criptografia</param>
        /// <returns></returns>
        public static string Criptografar(string valor)
        {
            try
            {
                if (!string.IsNullOrEmpty(valor))
                {
                    byte[] _bChave = Convert.FromBase64String(chave);
                    byte[] _bValor = new UTF8Encoding().GetBytes(valor);

                    Rijndael rijndael = new RijndaelManaged
                    {
                        KeySize = 256
                    };

                    MemoryStream mS = new MemoryStream();
                    CryptoStream encryptor = new CryptoStream(mS, rijndael.CreateEncryptor(_bChave, _byte), CryptoStreamMode.Write);

                    encryptor.Write(_bValor, 0, _bValor.Length);
                    encryptor.FlushFinalBlock();

                    return Convert.ToBase64String(mS.ToArray());
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Descriptografar valor informado como parâmetro.
        /// </summary>
        /// <param name="valor">Valor para descriptografia</param>
        /// <returns></returns>
        public static string Descriptografar(string valor)
        {
            try
            {
                if (!string.IsNullOrEmpty(valor))
                {
                    byte[] _bChave = Convert.FromBase64String(chave);
                    byte[] _bValor = Convert.FromBase64String(valor);

                    Rijndael rijndael = new RijndaelManaged
                    {
                        KeySize = 256
                    };

                    MemoryStream mStream = new MemoryStream();
                    CryptoStream decryptor = new CryptoStream(mStream, rijndael.CreateDecryptor(_bChave, _byte), CryptoStreamMode.Write);

                    decryptor.Write(_bValor, 0, _bValor.Length);
                    decryptor.FlushFinalBlock();
                    UTF8Encoding utf8 = new UTF8Encoding();

                    return utf8.GetString(mStream.ToArray());
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
