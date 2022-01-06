using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace password_manager
{
    static class JsonManager
    {

        public static void JsonSerialize(List<Entry> listOfEntries)
        {

            string filename = "myDatabase.json";
            string jsonstring = JsonSerializer.Serialize(listOfEntries);
            File.WriteAllText(filename, jsonstring);
        }

        public static List<Entry> JsonDeserialize() 
        {
            string fileName = "myDatabase.json";

            if (new FileInfo(fileName).Length == 0)
            {
                // file is empty
                List<Entry> ListOfEntries=new List<Entry>();
                return ListOfEntries;
            }
            else
            {
                string jsonString = File.ReadAllText(fileName);
                List<Entry> ListOfEntries = JsonSerializer.Deserialize<List<Entry>>(jsonString);
                return ListOfEntries;
            }
            

        }

        public static void ShowDatabase(List<Entry> listOfEntries)
        {
            if (!listOfEntries.Any())
            {
                Console.WriteLine("The database appears to be empty, try adding some entries");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                return;

            }

            for (int i = 0; i < listOfEntries.Count; i++)
            {
                Console.WriteLine($"Site:     {listOfEntries[i].Site}");
                Console.WriteLine($"Username: {listOfEntries[i].Username}");
                Console.WriteLine($"Password: {listOfEntries[i].Password}");
                Console.WriteLine($"---------------------------------------");
                
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();


        }

        public static void ShowEntry(Entry entry)
        {
           
                Console.WriteLine($"Site:     {entry.Site}");
                Console.WriteLine($"Username: {entry.Username}");
                Console.WriteLine($"Password: {entry.Password}");


        }



    }
}
