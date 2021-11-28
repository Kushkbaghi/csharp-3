using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Gust
{

    //===================CLASS================
    //
    // Set and get gustlist element
    //
    //========================================
    [Serializable]
    class FileElement
    {

        public FileElement() { } //   Create to handle deserializing
        public string Gust { get; set; }
        public string Post { get; set; }


    }

    //===================CLASS================
    //
    // Read, write and delete item in a file
    // 
    //
    //========================================
    class GustList : FileElement
    {

        // class fildes
        private string filePath;
        private List<FileElement> GustsList;
        string[] stArray;


        // constructor
        public GustList(string fileName)
        {
            filePath = fileName;
            GustsList = new List<FileElement>();


        }

        /// <summary>
        /// Copy file lines in th elist
        /// </summary>
        /// <returns>
        /// if it was sucssesfull return true
        /// </returns>
        public bool SetGustList()
        {
            string s = "";
            if (!File.Exists(filePath))
            {
                return false;
            }
            else
            {
                // Read the file
                string jsonString = File.ReadAllText(filePath);

                // set file element In the list
                GustsList = JsonConvert.DeserializeObject<List<FileElement>>(jsonString);


                return true;

            }

        }

        /// <summary>
        /// Get post 
        /// </summary>
        /// <returns>
        /// Return List of file items
        /// </returns>
        public List<FileElement> GetList()
        {
            return GustsList;
        }

        /// <summary>
        /// Set in new gust in the list/file
        /// </summary>
        /// <param string text></param>

        public void SetNewGust(string newGust, string newPost)
        {
            GustsList.Add(new FileElement
            {
                Gust = newGust,
                Post = newPost
            }
              );

            // update file
            File.WriteAllText(filePath, JsonConvert.SerializeObject(GustsList, Formatting.Indented));

        }

        /// <summary>
        /// Delete an item 
        /// </summary>
        /// <returns>
        /// Return true if its done
        /// </returns>
        /// 
      
        public bool DeleteItem(int id)
        {
            int before = GustsList.Count();

            // controll idand list length
            if (id <= before)
            {
                GustsList.RemoveAt(id);

                int after = GustsList.Count;

                // controllera listan
                if (before > after && id <= GustsList.Count())
                {
                    // Update the file
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(GustsList, Formatting.Indented));

                    return true;

                }
            }
            return false;
        }
       
    }

    internal class Program
    {
        static void Main(string[] args)
        {

            int userInput = -1;
            string userInputSt = "";
            List<FileElement> GustsList = new List<FileElement>();


            // file path debug
            string dir
                 = Directory.GetCurrentDirectory();

            // file path current folder
            string path
                = Directory.GetParent(dir).Parent.FullName.ToString();
            path = path + @"\list.json";

            // create an object
            GustList gustObj = new GustList(path);


            // menu
            while (userInput != 4)
            {

                Console.WriteLine("\n==============MENUY============\n");
                Console.WriteLine("\t1- Skriva ut Listan:");
                Console.WriteLine("\t2- Lägga till en gäst:");
                Console.WriteLine("\t3- Ta bort en gäst :");
                Console.WriteLine("\t4- Avsluta!\n");
                Console.WriteLine("=================================\n");

                // Is user input int
                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    Console.Clear();
                    switch (userInput)
                    {

                        // Print the list
                        case 1:
                            if (gustObj.SetGustList())
                            {
                                // create an object 
                                GustsList = gustObj.GetList();

                                int temp = 1;
                                Console.WriteLine(GustsList.Count);
                                foreach (FileElement s in GustsList)
                                {
                                    if (s.Gust != "" && s.Post != "")
                                    {
                                        Console.WriteLine("|\t" + temp + "- " + s.Gust + "- " + s.Post);
                                        temp++;
                                    }
                                }

                            }
                            else
                            {
                                Console.WriteLine("Fel inmatning!!");
                            }

                            break;

                        // Set new guest
                        case 2:
                            bool gustName = true;
                            bool post = true;
                            string postText = "";
                            string gustText = "";
                            // run untill input is not correct
                            while (gustName)
                            {
                                Console.WriteLine("\tSkriv in gest namn:");
                                gustText = Console.ReadLine();
                                if (gustText == "")
                                {
                                    Console.Clear();
                                    Console.WriteLine("Gäst namn är  obligatoriskt!\n");

                                }
                                else
                                {
                                    gustName = false;
                                }
                            }

                            // run untill input is not correct
                            while (post)
                            {
                                Console.WriteLine("\tSkriv in ett inlägg:");
                                postText = Console.ReadLine();

                                if (postText == "")
                                {
                                    Console.Clear();
                                    Console.WriteLine("Gäst namn är  obligatoriskt!\n");
                                }
                                else
                                {


                                    post = false;
                                }
                            }

                            // Add to the file
                            gustObj.SetNewGust(gustText, postText);

                            break;


                        // Delete
                        case 3:
                            bool indexToDel = true;
                            int toDelete = -1;

                            // run untill input is not correct
                            while (indexToDel)
                            {
                                Console.WriteLine("\tSkriv in gäst nummer:");
                                if (int.TryParse(Console.ReadLine(), out toDelete) && toDelete > 0)
                                {



                                    if (!gustObj.DeleteItem(toDelete - 1))
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Fel inmatning!\n");

                                    }
                                    else
                                    {
                                        indexToDel = false;
                                    }
                                }
                            }

                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("Välkommen åter!");

                            break;
                    }



                }
                else
                {
                    Console.WriteLine("vänligen välja från meny!");
                }
            }
        }
    }

}
