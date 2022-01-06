using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace password_manager
{
    static class Sha256Hash
    {
        public static string ComputeHash(string plaintext)
        {
            SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static void SaveHash(string hashedText)
        {
            string fileName = "PasswordHash.txt";
            File.WriteAllText(fileName, hashedText);
            //files are saved in C:\Users\...\passwrod_manager\passwrod_manager\bin\Debug\net5.0
        }

        public static bool CheckHash(string hashedText)
        {
            string fileName = "PasswordHash.txt";
            string storedHash=File.ReadAllText(fileName);

            bool result = hashedText.Equals(storedHash);
            return result;
        }
    }
}
