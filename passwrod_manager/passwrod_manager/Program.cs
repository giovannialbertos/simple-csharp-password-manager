using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace password_manager
{

        class Program
        {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Welcome to my RC4 Password manager");
            Console.ResetColor();

            if (!File.Exists("PasswordHash.txt"))
            {
                
                Console.WriteLine("It seems you don't have a database yet!:");
                ChangeMasterPassword();
            }

            string masterPassword = CheckPassword();
            

            List<Entry> listOfEntries = new List<Entry>();

            string fileName = "myDatabase.json";
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "");
            }

         
                
             listOfEntries = JsonManager.JsonDeserialize();
             listOfEntries = RC4.ComputeDatabase(listOfEntries, masterPassword);
           
            

            int choice = 0;
            while (choice!=6)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Choose what you want to do:");
                Console.ResetColor();
                Console.WriteLine("1. show database");
                Console.WriteLine("2. show specific entry");
                Console.WriteLine("3. add entry");
                Console.WriteLine("4. delete entry");
                Console.WriteLine("5. change master password");
                Console.WriteLine("6. save and exit");
                choice = ValidateInputInt(1,6);
                Console.Clear();

                switch (choice)
                {
                    case 1:
                    JsonManager.ShowDatabase(listOfEntries);
                    break;
    
                    case 2:
                        ShowSpecificEntry(listOfEntries);
                        break;
                    
                    case 3:
                        listOfEntries = AddEntry(listOfEntries);
                        break;
                    
                    case 4:
                        listOfEntries = DeleteEntry(listOfEntries);
                        break;
                    
                    case 5:
                        masterPassword= ChangeMasterPassword();
                        break;
                    

                }

              


            }

            //save encypted database and exit
            listOfEntries = RC4.ComputeDatabase(listOfEntries, masterPassword);
            JsonManager.JsonSerialize(listOfEntries);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Saved and exited with success");
            Console.ResetColor();
           
           
                   }

        public static string ChangeMasterPassword()
        {
            Console.WriteLine("Choose a strong password:");
            string newPassword = Console.ReadLine();
            string myHash = Sha256Hash.ComputeHash(newPassword);
            Sha256Hash.SaveHash(myHash);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Password changed!");
            Console.ResetColor();
            Console.WriteLine("Press a key to continue");
            Console.ReadKey(); 
            Console.Clear();
            return newPassword;
        }

        public static List<Entry> AddEntry(List<Entry> listOfEntries)
        {
            Console.WriteLine("site:");
            string site = Console.ReadLine();
            Console.WriteLine("username:");
            string username = Console.ReadLine();
            Console.WriteLine("password:");
            string password = GeneratePassword();
            Entry newEntry = new Entry(site, username, password);
            listOfEntries.Add(newEntry);
            //sort list in alphabetical order
            listOfEntries.Sort((x, y) => string.Compare(x.Site, y.Site));
            Console.Clear();
            return listOfEntries;
        }

        public static string CheckPassword()
        {
            Console.WriteLine("Please enter your master password:");
            bool exit = false;
            string masterPassword = "";
            while (exit == false)
            {
                masterPassword = Console.ReadLine();
                if (Sha256Hash.CheckHash(Sha256Hash.ComputeHash(masterPassword)) != true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Wrong password, please try again");
                    Console.ResetColor();
                }
                else
                {
                    exit = true;
                    Console.Clear();
                }

            }
            return masterPassword;
        }

        public static List<Entry> SearchEntry(List<Entry> listOfEntries)
        {
            Console.WriteLine("Do you want to search by site(1) or by username(2)?:");

            int choice = ValidateInputInt(1,2);

            switch (choice)
            {
                //by site
                case 1:
                    Console.WriteLine("Write the name of the site:");
                    string site = Console.ReadLine();
                    listOfEntries = listOfEntries.Where(x => String.Equals(x.Site, site, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;

                //username
                case 2:
                    Console.WriteLine("Write the name of the username:");
                    string username = Console.ReadLine();
                    listOfEntries = listOfEntries.Where(x => String.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    break;
            }
            Console.Clear();

            return listOfEntries;

        }



        public static void ShowSpecificEntry(List<Entry> listOfEntries)
        {

            listOfEntries = SearchEntry(listOfEntries);

            
            if (!listOfEntries.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No entry found with these search terms");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                return;
                
            }

            JsonManager.ShowDatabase(listOfEntries);

            Console.Clear();

        }

        public static List<Entry> DeleteEntry(List<Entry> listOfEntries)
        {


           
            List<Entry> listOfEntriesToRemove = SearchEntry(listOfEntries);
            if (!listOfEntriesToRemove.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No entry found with these search terms");
                Console.ResetColor();
               Console.WriteLine("Press any key to continue");
               Console.ReadKey();
               Console.Clear();
               return listOfEntries;
            }

            if (listOfEntriesToRemove.Count()>=1)
            {
                Console.WriteLine("Entries that match search terms");
                Console.WriteLine();
                int i = 1;
                foreach (var entry in listOfEntriesToRemove)
                {
                    Console.WriteLine($"---------{i}----------");
                    i++;
                    JsonManager.ShowEntry(entry);
                }

                Console.WriteLine();
                Console.WriteLine("Enter the number of the entry  you want to remove (0 to not remove any) ");
          
                int choice = ValidateInputInt(-1, listOfEntriesToRemove.Count());
                if (choice!=0)
                {
                    listOfEntries.Remove(listOfEntriesToRemove[choice - 1]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Entry deleted");
                    Console.ResetColor();
                }
                
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                return listOfEntries;
            }

            return listOfEntries;
            }

        public static int ValidateInputInt(int minNumber, int maxNumber)
        {
            int choice=-1;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice<minNumber || choice>maxNumber )
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("That was invalid. Enter a valid number.");
                Console.ResetColor();
            }
            return choice;
        }

        public static string GeneratePassword()
        {
            Console.WriteLine("Do you want to generate a random password(1) or write your own(2)?");
            int choice = ValidateInputInt(1,2);
            string password = "";

            if (choice==2)
            {
                Console.WriteLine("Write your password");
                password = Console.ReadLine();
                return password;
            }


            Random res = new Random();
            String str = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!£$%&/()=?^-_;,:.#@+";

            Console.WriteLine("Input password lenght (max 128):");
            int size = ValidateInputInt(1,128);

          

            for (int i = 0; i < size; i++)
            {

                // Selecting a index randomly
                int x = res.Next(82);

                // Appending the character at the 
                // index to the random string.
                password = password + str[x];
            }

         
        
            Console.WriteLine("Generated password: " + password);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            
            return password;

        }



        }
}
