using DungeonTasker.FirebaseData;
using DungeonTasker.Models;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        /*
        * Contructor forGreetPageViewModel. 
        * 
        */
        public GreetPageViewModel()
        {
            Login = new Command(async () => await LoginCommandDatabase());
            RegisterCommand = new Command(async () => await Navigation.PushModalAsync(new RegisterView(Navigation)));
            _UserModel = new UserModel();
            FadeOut = 100;
        }

        public async Task LoginCommandDatabase()
        {
            FirebaseUser client = new FirebaseUser();
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Users";//Get folder path
            try
            {
                switch (await client.Login(_UserModel.Username, _UserModel.Password))
                {
                    case 0:
                        var Timers = Path.Combine(documents, client.UserLogin.Object.Username + "Timer.dt");
                        var Logged = Path.Combine(documents, "Logged.dt");
                        UserModel newuser = new UserModel(client.UserLogin, client.UserStats, client.UserItems, client.Client, client.UserTimes); newuser.file = Logged;
                        InventoryItemsModel item = new InventoryItemsModel(newuser.UserItems, newuser.Token, newuser.UserLogin.Object.Username);
                        StatsModel stat = new StatsModel(newuser.UserStats, newuser.Token, newuser.UserLogin.Object.Username);
                        UserModel.Rewrite("Username:", client.UserLogin.Object.Username, newuser.file);
                        UserModel.Rewrite("Password:", client.UserLogin.Object.Password, newuser.file);

                        newuser.UserLogin.Object.Logged = "True";
                        newuser.RewriteDATA();

                        MessagingCenter.Send(this, "Animation");
                        await Task.Delay(600);
                        Application.Current.MainPage = new NavigationPage(new AddView(newuser, item, stat));
                        break;
                    case 1:
                        throw new Exception("Incorrect password");
                    case 2:
                        throw new Exception("No account found");
                    case 3:
                        throw new Exception("Account already in use");
                    case 4:
                        throw new Exception("Can't connect to server");
                }
            }
            catch(Exception es)
            {
                if (es != null) { await Application.Current.MainPage.DisplayAlert("Error", es.Message, "Close"); }
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
        public static async Task<bool> OnAppearingAsync(bool begin)
        {

           var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//Get folder path
           Directory.CreateDirectory(documents+ "/Users"); 
           FirebaseUser client = new FirebaseUser();

            var files = Directory.GetFiles(documents+ "/Users");
            try
            {
                foreach (var file in files)
                {
                    if (Path.GetFileName(file).Contains("Logged"))
                    {
                        switch (await client.Login(UserModel.CheckForstring(file, "Username:"), UserModel.CheckForstring(file, "Password:"), true))
                        {
                            case 0:
                                var Timers = Path.Combine(documents+"/Users", client.UserLogin.Object.Username + "Timer.dt");
                                var Logged = Path.Combine(documents+"/Users", "Logged.dt");
                                UserModel newuser = new UserModel(client.UserLogin, client.UserStats, client.UserItems, client.Client, client.UserTimes); newuser.file = Logged;
                                InventoryItemsModel item = new InventoryItemsModel(newuser.UserItems, newuser.Token, newuser.UserLogin.Object.Username);
                                StatsModel stat = new StatsModel(newuser.UserStats, newuser.Token, newuser.UserLogin.Object.Username);
                                Application.Current.MainPage = new NavigationPage(new AddView(newuser, item, stat));
                                return false;
                        }
                    }
                }
            }
            catch (Exception es)
            {
                if (es != null) { Application.Current.MainPage.DisplayAlert("Error", es.Message, "Close"); return false; }
            }
            
            if (begin && !CheckAccounts(files))
            {
                var documentz = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Users";//Get folder path
                var Logged = Path.Combine(documentz,"Logged.dt");
                File.WriteAllText(Logged, "Username:\nPassword:");
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
