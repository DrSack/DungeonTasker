using DungeonTasker.Models;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    public class GreetPageViewModel
    {
        public INavigation Navigation;
        public UserModel _UserModel { get; set; }
        public double FadeOut { get; set; }
        public Command Login { get; set; }
        public Command RegisterCommand { get; set; }

        AddView main;

        /*
        * Contructor forGreetPageViewModel. 
        * 
        */
        public GreetPageViewModel()
        {
            Login = new Command(async () => await LoginCommand());
            RegisterCommand = new Command(async () => await Navigation.PushModalAsync(new RegisterView()));
            _UserModel = new UserModel();
            FadeOut = 100;
        }

        /*
        * The login algorithm that decides whenver the UserModel trying to login is genuine
        * PARAM Nothing
        * RETURNS Nothing
        */
        private async Task LoginCommand()
        {
            try
            {
                string[] line;
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Users";//Get folder path
                
                var files = Directory.GetFiles(documents);
                bool hit = false;
                foreach (var file in files)
                {
                    string nice2;
                    using (StreamReader sr = new StreamReader(file)) { nice2 = sr.ReadToEnd(); }
                    line = nice2.Split(',');

                    if (line[0].Contains("ID:")) { line[0] = line[0].Replace("ID:", ""); }// obtain UserModelname and password information

                    if (_UserModel.Username == line[0] && _UserModel.Password == line[1])// checks if entrytext is the same as file information
                    {
                        hit = true;
                        var Timers = Path.Combine(documents,line[0] + "Timer.dt");
                        var Items = Path.Combine(documents, line[0] + "Inv.dt");
                        var Stats = Path.Combine(documents, line[0] + "Stats.dt");

                        UserModel.Rewrite("Logged:", "true", file);
                        string character = UserModel.CheckForstring(file, "Character:");
                        string logged = UserModel.CheckForstring(file, "Logged:");//obtain file information
                        
                        UserModel user = new UserModel(line[0], line[1], line[2], character, logged, file, Timers);
                        InventoryItemsModel item = new InventoryItemsModel(Items);
                        StatsModel stat = new StatsModel(Stats);
                        Application.Current.MainPage = new NavigationPage(new AddView(user, item, stat));
                        
                        MessagingCenter.Send(this, "Animation");
                    }

                    else if (_UserModel.Username == line[0] && _UserModel.Password != line[1])// if the password is incorrect
                    {
                        hit = true;
                        throw new Exception("Incorrect Password");
                    }
                }
                if (!UserModel.checkinfo(_UserModel.Password, _UserModel.Password))// if both entry's are empty
                {
                    throw new Exception("Please enter both Username and password");
                }
                else if (!hit)// if entrytext found no accounts corresponding to that UserModelname.
                {
                    throw new Exception("Account not found");
                }
            }
            catch (Exception es)//catch exception
            {
                if (es.GetType().FullName == "System.IndexOutOfRangeException") { await Application.Current.MainPage.DisplayAlert("Error", "Please enter both Username and password", "Close"); }
                else if (es != null) { await Application.Current.MainPage.DisplayAlert("Error", es.Message, "Close"); }
                else { await Application.Current.MainPage.DisplayAlert("Error", "Account not found", "Close"); }
            }
        }

        /*/
         * Check all files within local folder and see if a login file is still logged in.
         * If the login file is still logged:true then proceed to bypass login screen and go into
         * app.
         * 
         * PARAM begin: checks whenever the UserModel has only accessed the page once.
         * RETURNS Nothing
         */
        public static bool OnAppearing(bool begin)
        {
            string[] line;
            string all;

           var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//Get folder path
           Directory.CreateDirectory(documents+ "/Users");
            
            var files = Directory.GetFiles(documents+ "/Users");

            foreach (var file in files)
            {
                var textfile = File.ReadAllText(file);
                line = textfile.Split(',');
                using (StreamReader sr = new StreamReader(file)) { all = sr.ReadToEnd(); }
                if (line[0].Contains("ID:")) { line[0] = line[0].Replace("ID:", ""); }

                string character = UserModel.CheckForstring(file, "Character:");
                string logged = UserModel.CheckForstring(file, "Logged:");
                if (all.Contains("Logged:true"))
                {
                    var Timer = Path.Combine(documents + "/Users", line[0] + "Timer.dt");
                    var Items = Path.Combine(documents + "/Users", line[0] + "Inv.dt");
                    var Stats = Path.Combine(documents+ "/Users", line[0] + "Stats.dt");
                    UserModel UserModel = new UserModel(line[0], line[1],line[2], character, logged, file, Timer);
                    InventoryItemsModel items = new InventoryItemsModel(Items);
                    StatsModel stat = new StatsModel(Stats);
                    Application.Current.MainPage = new NavigationPage(new AddView(UserModel, items, stat));
                }
            }
            if (begin && !CheckAccounts(files))
            {
                Application.Current.MainPage.DisplayAlert("Welcome", "Welcome to Dungeon Tasker new User", "close");
                return false;
            }
            return false;

        }


        /*
         * Check to see if there are anyfiles that exist within the device
         * PARAM
         * files: the file directory
         * RETURNS 
         * true: whenever a files exist
         * false: whenever it doesn't
         */
        public static bool CheckAccounts(string[] files)
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
