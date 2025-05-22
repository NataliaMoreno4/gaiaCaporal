using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encriptacion
{
    public class EncriptadorUtility
    {
        private const string key = "3xc3ll3ntiam_2024!**@!";

        private string Decode(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str.Replace(",", "=").Replace("-", "+").Replace("/", "_"));
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        private string Encode(string input)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(input ?? "");
            return Convert.ToBase64String(encbuff).Replace("=", ",").Replace("+", "-").Replace("_", "/");
        }

        private string DictionaryToQuery(Dictionary<string,string> parametros)
        {
            string result = "";

            foreach(var value in parametros)
            {
                result += $"{value.Key}={value.Value}&";
            }

            if (!string.IsNullOrEmpty(result))
            {
                result = result.Remove(result.Length - 1);
            }

            return result;
        }

        private Dictionary<string,string> QueryToDictionary(string query)
        {
            Dictionary<string,string> dictionary = new Dictionary<string,string>();

            List<string> pairs = query.Split('&').ToList();

            foreach(var pair in pairs)
            {
                List<string> strings = pair.Split('=').ToList();

                dictionary.Add(strings[0], strings[1]);
            }

            return dictionary;
        }

        public string encriptarParametros(Dictionary<string, string> parametros)
        {
            string query = DictionaryToQuery(parametros);

            string encrypted = Encrypt(query);

            encrypted = Encode(encrypted);

            return encrypted;
        }


        public Dictionary<string, string> desencriptarParametros(string parametrosEncriptadosBase64)
        {
            parametrosEncriptadosBase64 = Decode(parametrosEncriptadosBase64);

            string decrypted = Decrypt(parametrosEncriptadosBase64);

            return QueryToDictionary(decrypted);
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = key;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = key;
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
