using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DungeonTasker.Models;

namespace DungeonTasker.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { private get; set; }
        public string Character { get; set; }
        public string Logged { get; set; }
        public string file {get; set;}

        public User()
        {
            
        }

        /*
         * The User constructor 
         * @para 
         * string Username: sets the username, 
         * string Password:sets the username, 
         * string Character: sets the Character string, 
         * string Logged: sets if logged in, 
         * string file: set the file path.
         */
        public User(string Username, string Password, string Character, string Logged, string file)
        {
            this.Username = Username;
            this.Password = Password;
            this.Character = Character;
            this.Logged = Logged;
            this.file = file;
        }
        /*
         *This method is responsible for rewriting all variables back into a file.
         *@para NONE
         * @returns Is Void
         */ 
        public void Rewrite()
        {
            string newfile = string.Format("ID:{0},{1},", Username, Password);
            newfile += string.Format("\nCharacter:{0}", Character);
            newfile += string.Format("\nLogged:{0}", "false");
            File.WriteAllText(file,newfile);
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, Username + "Login.dt");
            this.file = filename;
        }

        /*
         * This method is responsible for finding particular section associated with any variable word within a file and  
         * returning that particular word as a string.
         * @para string file: the file location, string word: the particular command word to find.
         * @return return Find when the word is found, return NULL if nothing is found.
         */

        public static string CheckForstring(string file, string word)
        {
            string Find;
            using (StreamReader sr = new StreamReader(file))
            {
                while ((Find = sr.ReadLine()) != null)
                {
                    if (Find.Contains(word))
                    {
                        Find = Find.Replace(word, "");
                        return Find;
                    }
                }
                return null;
            }
        }
        /*
         * This method is responsible for creating a file and writing the information of the textfields onto it
         *  @para string User: the username, string Pass: the password, RegisterAdd Rego: the page class. 
         *  @returns is Void
         *  
         */
        public static async void StoreInfo(string User, string Pass, Register Rego)
        {
            try
            {
                // if the username and password are not filled in throw an exception
                if (!checkinfo(User, Pass)) { throw new Exception("Please enter both credentials... ");}
            string line = string.Format("ID:{0},{1},", User, Pass);// format file structure
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//Get folder path
            var filename = Path.Combine(documents, User+"Login.dt");// File name is equal to the username+login.dt
            
            
                if (File.Exists(filename))// If the file already exists throw and exception
                {
                    throw new Exception("Please delete current account");
                }
                else
                {
                    //Write onto file and save onto device
                    File.WriteAllText(filename, line);
                    // Show display alert then close current page and go back to previous opened window.
                    await ExtraPopups.ShowMessage("Account Succefully Created", "Create", "Close", Rego, async () =>
                    {
                        await Rego.Navigation.PopAsync();
                    });
                }
            }
            catch (Exception e)
            {   //What an exception is caught, display alert message
                await Rego.DisplayAlert("Error", e.Message, "Close");
            }
            
        }

        /*
         * A boolean check method for checking whether both username and passwords
         * text fields are filled in.
         * 
         * @para string user: the username, string pass: the password
         * @returns: true if username and password are both not blank, false if one or both fields are blank.
         */ 
        public static bool checkinfo(string user, string pass)
        {
            if (!user.Equals("") && !pass.Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
