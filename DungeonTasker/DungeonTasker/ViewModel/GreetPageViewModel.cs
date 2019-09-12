using DungeonTasker.Models;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    class GreetPageViewModel
    {
        bool begin = true;
        public string Username { get; set; }
        public string Password { get; set; }

        public Command Login { get; set; }
        public Command RegisterCommand { get; set; }

        private GreetPage page;


        /*
        * Contructor forGreetPageViewModel. 
        * 
        */
        public GreetPageViewModel(GreetPage page)
        {
            Username = "";
            Password = "";
            Login = new Command(async () => await LoginCommand());
            RegisterCommand = new Command(async () => await page.Navigation.PushModalAsync(new Register()));
            this.page = page;
            OnAppearing();
        }

        /*
        * The login algorithm that decides whenver the user trying to login is genuine
        * PARAM Nothing
        * RETURNS Nothing
        */
        async Task LoginCommand()
        {
            try
            {
                string[] line;
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var files = Directory.GetFiles(documents);
                bool hit = false;
                foreach (var file in files)
                {
                    string nice2;
                    using (StreamReader sr = new StreamReader(file)) { nice2 = sr.ReadToEnd(); }
                    line = nice2.Split(',');

                    if (line[0].Contains("ID:")) { line[0] = line[0].Replace("ID:", ""); }// obtain username and password information

                    if (Username == line[0] && Password == line[1])// checks if entrytext is the same as file information
                    {
                        hit = true;
                        var Timers = Path.Combine(documents, line[0] + "Timer.dt");
                        var Items = Path.Combine(documents, line[0] + "Inv.dt");
                        var Stats = Path.Combine(documents, line[0] + "Stats.dt");
                        ExtraPopups.LoginWrite(page, file, Timers, Items, Stats, line);

                    }
                    else if (Username == line[0] && Password != line[1])// if the password is incorrect
                    {
                        hit = true;
                        throw new Exception("Incorrect Password");
                    }
                }
                if (!User.checkinfo(Username, Password))// if both entry's are empty
                {
                    throw new Exception("Please enter both username and password");
                }
                else if (!hit)// if entrytext found no accounts corresponding to that username.
                {
                    throw new Exception("Account not found");
                }
            }
            catch (Exception es)//catch exception
            {
                if (es.GetType().FullName == "System.IndexOutOfRangeException") { await page.DisplayAlert("Error", "Please enter both username and password", "Close"); }
                else if (es != null) { await page.DisplayAlert("Error", es.Message, "Close"); }
                else { await page.DisplayAlert("Error", "Account not found", "Close"); }
            }
        }

        /*/
         * Check all files within local folder and see if a login file is still logged in.
         * If the login file is still logged:true then proceed to bypass login screen and go into
         * app.
         * 
         * PARAM Nothing
         * RETURNS Nothing
         */
        private void OnAppearing()
        {
            string[] line;
            string all;
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var files = System.IO.Directory.GetFiles(documents);

            foreach (var file in files)
            {
                var textfile = File.ReadAllText(file);
                line = textfile.Split(',');
                using (StreamReader sr = new StreamReader(file)) { all = sr.ReadToEnd(); }
                if (line[0].Contains("ID:")) { line[0] = line[0].Replace("ID:", ""); }

                string character = User.CheckForstring(file, "Character:");
                string logged = User.CheckForstring(file, "Logged:");
                if (all.Contains("Logged:true"))
                {
                    var Timer = Path.Combine(documents, line[0] + "Timer.dt");
                    var Items = Path.Combine(documents, line[0] + "Inv.dt");
                    var Stats = Path.Combine(documents, line[0] + "Stats.dt");
                    User user = new User(line[0], line[1], character, logged, file, Timer);
                    InventoryItems items = new InventoryItems(Items);
                    Stats stat = new Stats(Stats);
                    Application.Current.MainPage = new NavigationPage(new Add(user, items, stat));
                }
            }
            if (begin && !CheckAccounts(files))
            {
                Application.Current.MainPage.DisplayAlert("Welcome", "Welcome to Dungeon Tasker new user", "close");
                begin = false;
            }


        }

        /*
         * Check to see if there are anyfiles that exist within the device
         * PARAM
         * files: the file directory
         * RETURNS 
         * true: whenever a files exist
         * false: whenever it doesn't
         */
        public bool CheckAccounts(string[] files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
