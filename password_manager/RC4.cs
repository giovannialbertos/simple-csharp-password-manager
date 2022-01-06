using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;


namespace password_manager
{
    static class RC4
    {
        public static string Encrypt(string text, string key)
        {
            StringBuilder result = new StringBuilder();
            int temp_variable, y, j = 0;
            int[] S = new int[256];

            //initialization of S
            for (int i = 0; i < 256; i++)
            {
                S[i] = i;
            }


            //KSA algorithm
            //this scrambles the array S using the key 
            for (int i = 0; i < 256; i++)
            {
                j = (key[i % key.Length] + S[i] + j) % 256;
                temp_variable = S[i];
                S[i] = S[j];
                S[j] = temp_variable;
            }

            j = 0;
            //PRGA(Pseudo Random Generation Algorithm)
            for (int i = 0; i < text.Length; i++)
            {
                y = (i + 1) % 256;
                j = (S[y] + j) % 256;
                temp_variable = S[y];
                S[y] = S[j];
                S[j] = temp_variable;

                result.Append((char)(text[i] ^ S[(S[y] + S[j]) % 256]));
                //Console.WriteLine(result);
            }
            return result.ToString();




        }

        public static List<Entry> ComputeDatabase(List<Entry> listOfEntries, string masterPassword)
        {
            for (int i = 0; i < listOfEntries.Count; i++)
            {
                listOfEntries[i].Site = RC4.Encrypt(listOfEntries[i].Site, masterPassword);
                listOfEntries[i].Username = RC4.Encrypt(listOfEntries[i].Username, masterPassword);
                listOfEntries[i].Password = RC4.Encrypt(listOfEntries[i].Password, masterPassword);

            }

            return listOfEntries;

        }

    }
}
