using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DungeonTasker.Models;
using DungeonTasker.Views;

namespace DungeonTasker.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { private get; set; }
        public string Character { get; set; }
        public string Logged { get; set; }
        public string file {get; set;}
        public string timer { get; set; }
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
        public User(string Username, string Password, string Character, string Logged, string file, string timer)
        {
            this.Username = Username;
            this.Password = Password;
            this.Character = Character;
            this.Logged = Logged;
            this.file = file;
            this.timer = timer;
        }

        public void UpdateCurrenttimes(List<TimerUpdatecs> timer)
        {
            string tempFile = Path.GetTempFileName();

            using (var sw = new StreamWriter(tempFile))
            {
                foreach(TimerUpdatecs timeboi in timer)
                {
                    sw.WriteLine(string.Format("{0},{1}",timeboi.type, timeboi.T.ToString()));
                }
            }
            File.Delete(this.timer);
            File.Move(tempFile, this.timer);
        }

        /*
         *This method is responsible for rewriting all variables back into a file.
         *@para NONE
         * @returns Is Void
         */
        public void Rewrite(string truth)
        {
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(file))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;

                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    if (line.Contains("Logged")) { sw.WriteLine("Logged:" + truth); }
                    else { sw.WriteLine(line); }
                }
            }
            File.Delete(file);
            File.Move(tempFile, file);

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
            string line = string.Format("ID:{0},{1},\nCharacter:<0-0>\nLogged:false", User, Pass);// format file structure
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//Get folder path
            var filename = Path.Combine(documents, User+"Login.dt");// File name is equal to the username+login.dt
            var Items = Path.Combine(documents, User+ "Inv.dt");
            var Stats = Path.Combine(documents, User+ "Stats.dt");
            var Timer = Path.Combine(documents, User + "Timer.dt");

                if (File.Exists(filename))// If the file already exists throw and exception
                {
                    throw new Exception("Please delete current account");
                }
                else
                {
                    //Write onto file and save onto device
                    File.WriteAllText(filename, line);
                    File.WriteAllText(Items, "Weapons:IronDagger,\nKeys:0,");
                    File.WriteAllText(Stats, "Add later");
                    File.WriteAllText(Timer, "");
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
