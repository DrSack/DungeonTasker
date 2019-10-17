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
using DungeonTasker.FirebaseData;
using Firebase.Database;
using Firebase.Database.Query;

namespace DungeonTasker.Models
{
    public class UserModel
    {
        public FirebaseObject<ItemDetails> UserItems;
        public FirebaseObject<LoginDetails> UserLogin;
        public FirebaseObject<StatDetails> UserStats;
        public FirebaseObject<TimerDetails> UserTimes;
        public FirebaseClient Token;
        public string Username { get; set; } 
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Character { get; set; }
        public string Logged { get; set; }

        public string LocalLogin { get; set; }
        public string LocalStats { get; set; }
        public string LocalItem { get; set; }
        public string LocalTimer { get; set; }

        public string file {get; set;}
        public string timer { get; set; }// Intialize all variables

        /*
         * An empty User constructor 
         * @para Nothing 
         * @returns Nothing
         */
        public UserModel()
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
        public UserModel(FirebaseObject<LoginDetails> User, FirebaseObject<StatDetails> Stats,
            FirebaseObject<ItemDetails> Inv, FirebaseClient token, FirebaseObject<TimerDetails> timer)
        {
            this.Token = token;
            this.UserLogin = User;
            this.UserStats = Stats;
            this.UserItems = Inv;
            this.Username = UserLogin.Object.Username;
            this.FullName = UserLogin.Object.FullName;
            this.Character = UserLogin.Object.Character;
            this.UserTimes = timer;
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
        public UserModel(string Username, string Password, string FullName, string Character, string Logged, string file, string timer)
        {
            this.Username = Username;
            this.Password = Password;
            this.FullName = FullName;
            this.Character = Character;
            this.Logged = Logged;
            this.file = file;
            this.timer = timer;
        }


        /*
         *  Creates a display alert that has await assigned to it such that the user must close this before any action is taken afterwards
         *  PARAM
         *  message: message of the display alert
         *  title: the title for the display alert
         *  buttonText: the string for the button
         *  page: the current contentpage
         *  afterHideCallback: action to be take
         *  Returns Nothing
         */
        public static async Task ShowMessage(string message, string title, string buttonText, Page page, Action afterHideCallback)
        {
            await page.DisplayAlert(title, message, buttonText);
            afterHideCallback?.Invoke();
        }

        /*
         * Update the current operating timers on to the timer file 
         * 
         * PARAM timer a list that uses the TimerUpdatecs object class
         * RETURNS Nothing
         */
        public void UpdateCurrenttimes(List<TimerUpdatecs> timer)
        {
            string tempTimes="";//create a temporary file
            string tempFile = Path.GetTempFileName();

            foreach (TimerUpdatecs timeboi in timer)// for each timer within the timer list
            {
                tempTimes += string.Format("Name:{0}Time:{1}\n", timeboi.type, timeboi.T.ToString());// write onto the temporary string the name and time information 
            }

            File.WriteAllText(tempFile,tempTimes);
            File.Delete(LocalTimer);
            File.Move(tempFile, LocalTimer);
            try
            {
                UserTimes.Object.Timer = tempTimes;
                RewriteDATATimes();
            } catch { }
        }

        /*
         *This method is responsible for adding onto an already existing line.
         *@para command: the line that contains that specific text, truth: the text to be added, file: the file path
         * @returns Nothing
         */
        public static void AddOntoLine(string command, string truth, string file)
        {
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(file))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;

                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    if (line.Contains(command)) { sw.WriteLine(line + truth); }
                    else { sw.WriteLine(line); }
                }
            }

            File.Delete(file);
            File.Move(tempFile, file);
        }

        /*
         *This method is responsible for rewriting all variables back into a file.
         *@para command: the text to start from, truth: the text to replace after the command: line, file: the file path.
         * @returns Nothing
         */
        public static void Rewrite(string command,string truth, string file)
        {
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(file))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;

                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    if (line.Contains(command)) { sw.WriteLine(command + truth);}
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
         * This method is responsible for deleting the account of the user
         * 
         * @para nothing
         * @return nothing
         */
        public async void DeleteAccount()
        {
            await Token
                .Child(string.Format("{0}Login", Username))
                .Child(UserLogin.Key).DeleteAsync();
            await Token
                .Child(string.Format("{0}Stats", Username))
                .Child(UserStats.Key).DeleteAsync();
            await Token
                .Child(string.Format("{0}Inv", Username))
                .Child(UserItems.Key).DeleteAsync();
            await Token
                .Child(string.Format("{0}Timer", Username))
                .Child(UserTimes.Key).DeleteAsync();
        }

        /*
        * Encapsulate localfiles
        * 
        * @para nothing
        * @return nothing
        */
        public void Getfile(string login, string items, string stats, string timer)
        {
            this.LocalLogin = login;
            this.LocalItem = items;
            this.LocalStats = stats;
            this.LocalTimer = timer;
        }

            /*
             * This updates everything based on which file was last updated.
             * 
             * @para nothing
             * @return nothing
             */
            public async void UpdateAll(bool skip = false)
        {
            DateTime Server = DateTime.Now;
            DateTime Local = DateTime.Now.AddDays(1);
            try
            {
                Server = Convert.ToDateTime(UserLogin.Object.Updated);
                Local = Convert.ToDateTime(UserModel.CheckForstring(LocalLogin, "Updated:"));
            }catch { }
            
            if (Server > Local || skip == true)
            {
                //Login
                UserModel.Rewrite("Updated:", DateTime.Now.ToString(), LocalLogin);
                UserModel.Rewrite("Username:", UserLogin.Object.Username, LocalLogin);
                UserModel.Rewrite("Password:", UserLogin.Object.Password, LocalLogin);
                UserModel.Rewrite("Fullname:", UserLogin.Object.FullName, LocalLogin);
                UserModel.Rewrite("Character:", UserLogin.Object.Character, LocalLogin);
                UserModel.Rewrite("Logged:", UserLogin.Object.Logged, LocalLogin);
                UserModel.Rewrite("Tutorial:", UserLogin.Object.Tutorial, LocalLogin);
                //Inv
                UserModel.Rewrite("Weapons:", UserItems.Object.Weapons, LocalItem);
                UserModel.Rewrite("Keys:", UserItems.Object.Keys, LocalItem);
                UserModel.Rewrite("Gold:", UserItems.Object.Gold, LocalItem);
                UserModel.Rewrite("Equipped:", UserItems.Object.Equipped, LocalItem);
                UserModel.Rewrite("Items:", UserItems.Object.Items, LocalItem);
                //Stats
                UserModel.Rewrite("HEALTH:", UserStats.Object.HEALTH, LocalStats);
                UserModel.Rewrite("MANA:", UserStats.Object.MANA, LocalStats);
                UserModel.Rewrite("LEVEL:", UserStats.Object.LEVEL, LocalStats);
                UserModel.Rewrite("EXP:", UserStats.Object.EXP, LocalStats);
                //Timer
                File.WriteAllText(LocalTimer, UserTimes.Object.Timer);
            }
            else
            {
                //Login
                UserLogin.Object.Updated = DateTime.Now.ToString();// GOTTA ADD ITEMS HERE
                UserLogin.Object.Username = UserModel.CheckForstring(LocalLogin, "Username:");
                UserLogin.Object.Password = UserModel.CheckForstring(LocalLogin, "Password:");
                UserLogin.Object.FullName = UserModel.CheckForstring(LocalLogin, "Fullname:");
                UserLogin.Object.Character = UserModel.CheckForstring(LocalLogin, "Character:");
                UserLogin.Object.Logged = UserModel.CheckForstring(LocalLogin, "Logged:");
                UserLogin.Object.Tutorial = UserModel.CheckForstring(LocalLogin, "Tutorial:");
                await RewriteDATA();

                //Inv
                UserItems.Object.Weapons = UserModel.CheckForstring(LocalItem, "Weapons:");
                UserItems.Object.Keys = UserModel.CheckForstring(LocalItem, "Keys:");
                UserItems.Object.Gold = UserModel.CheckForstring(LocalItem, "Gold:");
                UserItems.Object.Equipped = UserModel.CheckForstring(LocalItem, "Equipped:");
                UserItems.Object.Items = UserModel.CheckForstring(LocalItem, "Items:");
                await RewriteDATAItems();

                //Stats
                UserStats.Object.HEALTH = UserModel.CheckForstring(LocalStats, "HEALTH:");
                UserStats.Object.MANA = UserModel.CheckForstring(LocalStats, "MANA:");
                UserStats.Object.LEVEL = UserModel.CheckForstring(LocalStats, "LEVEL:");
                UserStats.Object.EXP = UserModel.CheckForstring(LocalStats, "EXP:");
                await RewriteDATAStats();

                //Timer
                UserTimes.Object.Timer = File.ReadAllText(LocalTimer);
                await RewriteDATATimes();
            }

            Character = UserModel.CheckForstring(LocalLogin, "Character:");
        }

        /*
         * This method is responsible for creating a file and writing the information of the textfields onto it
         *  @para string User: the username, string Pass: the password, RegisterAdd Rego: the page class. 
         *  @returns is Void
         *  
         */
        public static async void StoreInfo(string User, string Pass, string FullName, RegisterView Rego, bool test = false)
        {
            try
            {
                // if the username and password are not filled in throw an exception
                if (!checkinfo(User, Pass)) { throw new Exception("Please enter all credentials... ");}
            string line = string.Format("Username:{0}\nPassword:{1}\nFullname:{2}\nCharacter:(ง’̀-‘́)ง\nLogged:false\nTutorial:True", User, Pass, FullName);// format file structure
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//Get folder path
            var filename = Path.Combine(documents + "/Users", User+"Login.dt");// File name is equal to the username+login.dt
            var Items = Path.Combine(documents + "/Users", User+"Inv.dt");
            var Stats = Path.Combine(documents + "/Users", User+"Stats.dt");
            var Timer = Path.Combine(documents + "/Users", User+"Timer.dt");

                if (File.Exists(filename))// If the file already exists throw and exception
                {
                    throw new Exception("Please delete current account");
                }
                else
                {
                    //Write onto file and save onto device
                    File.WriteAllText(filename, line);
                    File.WriteAllText(Items, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
                    File.WriteAllText(Stats, "HEALTH:100\nMANA:40\nLEVEL:1\nEXP:0");
                    File.WriteAllText(Timer, "");
                    if (!test)
                    {
                        // Show display alert then close current page and go back to previous opened window.
                        await UserModel.ShowMessage("Account Succefully Created", "Create", "Close", Rego, async () =>
                        {
                            await Rego.Navigation.PopModalAsync();
                        });
                    }
                }
            }
            catch (Exception e)
            {   //What an exception is caught, display alert message
              await Rego.DisplayAlert("Error", e.Message, "Close");
            }
            
        }

        

        public async Task RewriteDATA()
        {
            await Token
                .Child(string.Format("{0}Login", Username))
                .Child(UserLogin.Key).PutAsync(UserLogin.Object);
        }

        public async Task RewriteDATAItems()
        {
            await Token
                .Child(string.Format("{0}Inv", Username))
                .Child(UserItems.Key).PutAsync(UserItems.Object);
        }

        public async Task RewriteDATAStats()
        {
            await Token
                .Child(string.Format("{0}Stats", Username))
                .Child(UserStats.Key).PutAsync(UserStats.Object);
        }

        public async Task RewriteDATATimes()
        {
            await Token
                .Child(string.Format("{0}Timer", Username))
                .Child(UserTimes.Key).PutAsync(UserTimes.Object);
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
