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

        public User(string Username, string Password, string Character, string Logged, string file)
        {
            this.Username = Username;
            this.Password = Password;
            this.Character = Character;
            this.Logged = Logged;
            this.file = file;
        }

        public void Rewrite()
        {
            string newfile = string.Format("ID:{0},{1},{2},{3},", Username, Password, "trueB", "checked");
            newfile += string.Format("\nCharacter:{0}", Character);
            newfile += string.Format("\nLogged:{0}", "false");
            File.WriteAllText(file,newfile);
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, Username + "Login.dt");
            this.file = filename;
        }

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

        public static async void StoreInfo(string User, string Pass, RegisterAdd Rego)
        {
            if(!checkinfo(User, Pass)) { throw new Exception("Please enter both username and password"); }
            string line = string.Format("ID:{0},{1},{2}", User, Pass, "falseB");
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, User+"Login.dt");
            ;
            try
            {
                if (File.Exists(filename))
                {
                    throw new Exception("Please delete current account");
                }
                else
                {
                    File.WriteAllText(filename, line);
                    await ExtraPopups.ShowMessage("Account Succefully Created", "Create", "Close", Rego, async () =>
                    {
                        await Rego.Navigation.PopAsync();
                    });
                }
            }
            catch (Exception e)
            {
                await Rego.DisplayAlert("Error", e.Message, "Close");
            }
            
        }

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
