using DungeonTasker.FirebaseData;
using DungeonTasker.Models;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    public class GreetPageViewModel:INotifyPropertyChanged
    {
        public INavigation Navigation;
        public UserModel _UserModel { get; set; }

        private bool isrunning;
        public bool IsRunning
        {
            get{return isrunning;}
            set{isrunning = value; OnPropertyChanged(this, "IsRunning");}
        }

        private bool isvisible;
        public bool IsVisible
        {
            get{ return isvisible;}
            set{ isvisible = value; OnPropertyChanged(this, "IsVisible");}
        }

        private double fadeout;
        public double FadeOut
        {
            get{ return fadeout;}
            set{ fadeout = value; OnPropertyChanged(this, "FadeOut");}
        }

        private bool loggedisrunning;
        public bool LoggedIsRunning
        {
            get { return loggedisrunning; }
            set { loggedisrunning = value; OnPropertyChanged(this, "LoggedIsRunning"); }
        }

        
        public Command Login { get; set; }
        public Command RegisterCommand { get; set; }

        /*
        * Contructor forGreetPageViewModel. 
        * 
        */
        public GreetPageViewModel()
        {
            IsVisible = false;
            IsRunning = false;
            Login = new Command(async () => await LoginCommandDatabase());
            RegisterCommand = new Command(async () => await Navigation.PushModalAsync(new RegisterView(Navigation)));
            _UserModel = new UserModel(); _UserModel.Username = ""; _UserModel.Password = "";
            FadeOut = 100.0;
        }

        /*/
         * Log into the Database and check if user is either online or offline, 
         * if offline then use local data from last used account
         * 
         * PARAM begin: checks whenever the UserModel has only accessed the page once.
         * RETURNS Nothing
         */

        public async Task LoginCommandDatabase()
        {
            IsVisible = true;
            IsRunning = true;
            FirebaseUser client = new FirebaseUser();
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Users";//Get folder path
            try
            {
                switch (await client.Login(_UserModel.Username, _UserModel.Password))
                {
                    case 0:
                        await LoadUser(client, documents);
                        break;
                    case 1:
                        await OfflineLogin(documents);
                        break;
                    case 2:
                        throw new Exception("Please input both Username and Password");
                    case 3:
                        throw new Exception("Incorrect password");
                    case 4:
                        throw new Exception("Account already in use");
                    case 5:
                        NoAccount(documents);
                        break;
                }
            }
            catch(Exception es)
            {
                if (es != null) {
                    try
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", es.Message, "Close");
                    }catch { throw new Exception(es.Message); }
                }
            }
            IsVisible = false;
            IsRunning = false;

        }

        /*/
         * Check all files within local folder and see if a login file is still logged in.
         * If the login file is still logged:true then proceed to bypass login screen and go into
         * app.
         * 
         * PARAM begin: checks whenever the UserModel has only accessed the page once.
         * RETURNS Nothing
         */
        public async Task<bool> OnAppearingAsync(bool begin)
        {
           var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//Get folder path
           Directory.CreateDirectory(documents+ "/Users"); 
           FirebaseUser client = new FirebaseUser();
            var files = Directory.GetFiles(documents+ "/Users");
            var Logged = Path.Combine(documents + "/Users", "Logged.dt");
            FadeOut = 0.0;
            LoggedIsRunning = true;
            try
            {
                foreach (var file in files)
                {
                    if (Path.GetFileName(file).Contains("Logged"))
                    {
                        switch (await client.Login(UserModel.CheckForstring(file, "Username:"), UserModel.CheckForstring(file, "Password:"), true))
                        {
                            case 0:
                                await LoadUserLogged(client, documents, file, Logged);
                                return false;
                            case 1:
                                await OfflineLoginLogged(documents,file,Logged);
                                return false;
                            case 2:
                                break;
                            case 3:
                                throw new Exception("Incorrect Password");
                            case 5:
                                NoAccountLogged(documents,file,Logged);
                                break;
                        }
                    }
                }
            }
            catch (Exception es)
            {
                if (es != null) { await Application.Current.MainPage.DisplayAlert("Error", es.Message, "Close"); FadeOut = 100.0; LoggedIsRunning = false; return false;}
            }
            FadeOut = 100.0;
            LoggedIsRunning = false;
            if (begin && !CheckAccounts(files))
            {
                var documentz = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Users";//Get folder path
                File.WriteAllText(Logged, "Username:\nPassword:");
                await Application.Current.MainPage.DisplayAlert("Welcome", "Welcome to Dungeon Tasker new User", "close");
                return false;
            }
            return false;

        }

        private async Task LoadUser(FirebaseUser client, string documents)// Load user from login mainscreen
        {
            bool skip = false;
            var Logged = Path.Combine(documents, "Logged.dt");
            var Login = Path.Combine(documents, _UserModel.Username + "Login.dt");
            var Timers = Path.Combine(documents, _UserModel.Username + "Timer.dt");
            var Items = Path.Combine(documents, _UserModel.Username + "Inv.dt");
            var Stats = Path.Combine(documents, _UserModel.Username + "Stats.dt");
            UserModel newuser = new UserModel(client.UserLogin, client.UserStats, client.UserItems, client.Client, client.UserTimes); newuser.file = Logged;
            await Task.Run(async () =>
            {
                if (!File.Exists(Login) || !File.Exists(Items) || !File.Exists(Stats) || !File.Exists(Timers))
                {
                    skip = true;
                    File.Delete(Login);
                    File.Delete(Items);
                    File.Delete(Stats);
                    File.Delete(Timers);
                    File.WriteAllText(Login, string.Format("Updated:\nUsername:\nPassword:\nFullname:\nCharacter:(ง’̀-‘́)ง\nLogged:false\nTutorial:True"));
                    File.WriteAllText(Items, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:\nCharacters:(ง’̀-‘́)ง,\nTOTAL_KEYS:0");
                    File.WriteAllText(Stats, "HEALTH:100\nMANA:40\nLEVEL:1\nEXP:0\nTOTAL_BOSSES:0");
                    File.WriteAllText(Timers, "");
                }
                newuser.Getfile(Login, Items, Stats, Timers);
                newuser.Character = UserModel.CheckForstring(newuser.LocalLogin, "Character:");
                newuser.UserLogin.Object.Logged = "True";
                await newuser.RewriteDATA();
                await newuser.UpdateAll(skip);
                UserModel.Rewrite("Username:", _UserModel.Username, newuser.file);
                UserModel.Rewrite("Password:", _UserModel.Password, newuser.file);
                Globals.LOGGED = client.UserLogin;
                Globals.CLIENT = client.Client;
            });
            InventoryItemsModel item = new InventoryItemsModel(newuser.UserItems, newuser.Token, newuser.UserLogin.Object.Username, Items);
            StatsModel stat = new StatsModel(newuser.UserStats, newuser.Token, newuser.UserLogin.Object.Username, Stats);
            MessagingCenter.Send(this, "Animation");
            await Task.Delay(700);
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new NavigationPage(new AddView(newuser, item, stat));
            });
        }

        private async Task LoadUserLogged(FirebaseUser client, string documents, string file, string Logged)// This automatically logs in user.
        {
            var Login = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Login.dt");
            var Timers = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Timer.dt");
            var Items = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Inv.dt");
            var Stats = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Stats.dt");
            UserModel newuser = new UserModel(client.UserLogin, client.UserStats, client.UserItems, client.Client, client.UserTimes); newuser.file = Logged;
            InventoryItemsModel item = new InventoryItemsModel(newuser.UserItems, newuser.Token, newuser.UserLogin.Object.Username, Items);
            StatsModel stat = new StatsModel(newuser.UserStats, newuser.Token, newuser.UserLogin.Object.Username, Stats);
            await Task.Run(async () =>
             {
                 newuser.Getfile(Login, Items, Stats, Timers);
                 UserModel.Rewrite("Username:", UserModel.CheckForstring(file, "Username:"), newuser.file);
                 UserModel.Rewrite("Password:", UserModel.CheckForstring(file, "Password:"), newuser.file);
                 newuser.Character = UserModel.CheckForstring(newuser.LocalLogin, "Character:");
                 newuser.UserLogin.Object.Logged = "True";
                 await newuser.RewriteDATA();
                 Globals.LOGGED = client.UserLogin;
                 Globals.CLIENT = client.Client;
                 await newuser.UpdateAll();
             });
            MessagingCenter.Send(this, "Animation");
            await Task.Delay(700);
            Device.BeginInvokeOnMainThread(async () =>
            {
                Application.Current.MainPage = new NavigationPage(new AddView(newuser, item, stat));
            });
            
            IsRunning = false;
            FadeOut = 100.0;
        }

        private async Task OfflineLogin(string documents)
        {
            var localLogged = Path.Combine(documents, "Logged.dt");
            var localLogin = Path.Combine(documents, _UserModel.Username + "Login.dt");
            var localTimers = Path.Combine(documents, _UserModel.Username + "Timer.dt");
            var localItems = Path.Combine(documents, _UserModel.Username + "Inv.dt");
            var localStats = Path.Combine(documents, _UserModel.Username + "Stats.dt");
            UserModel local = new UserModel(); local.file = localLogged;
            await Task.Run(() =>
            {
                try
                {
                    File.ReadAllText(localLogin);
                    File.ReadAllText(localTimers);
                    File.ReadAllText(localItems);
                    File.ReadAllText(localStats);
                }
                catch { throw new Exception("Local Account doesn't exist"); }//if logging in and files dont exist then log in
                local.Getfile(localLogin, localItems, localStats, localTimers);
                local.Character = UserModel.CheckForstring(localLogin, "Character:");
                UserModel.Rewrite("Username:", _UserModel.Username, local.file);
                UserModel.Rewrite("Password:", _UserModel.Password, local.file);
            });
            InventoryItemsModel localitem = new InventoryItemsModel(localItems);
            StatsModel localstat = new StatsModel(localStats);
            MessagingCenter.Send(this, "Animation");
            await Task.Delay(700);// Allow time for all threads to finish
            Device.BeginInvokeOnMainThread(async () =>
            {
                Application.Current.MainPage = new NavigationPage(new AddView(local, localitem, localstat));
            });
        }

        private async Task OfflineLoginLogged(string documents, string file, string Logged)
        {
            var localLogin = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Login.dt");
            var localTimers = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Timer.dt");
            var localItems = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Inv.dt");
            var localStats = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Stats.dt");
            UserModel local = new UserModel(); local.file = Logged;
            await Task.Run(() =>
            {
                try
                {
                    File.ReadAllText(localLogin);
                }
                catch { throw new Exception("Error reading local data"); }
                local.Getfile(localLogin, localItems, localStats, localTimers);
                local.Character = UserModel.CheckForstring(localLogin, "Character:");
                UserModel.Rewrite("Username:", UserModel.CheckForstring(file, "Username:"), local.file);
                UserModel.Rewrite("Password:", UserModel.CheckForstring(file, "Password:"), local.file);
            });
            InventoryItemsModel localitem = new InventoryItemsModel(localItems);
            StatsModel localstat = new StatsModel(localStats);
            MessagingCenter.Send(this, "Animation");
            await Task.Delay(700);
            Device.BeginInvokeOnMainThread(async () =>
            {
                Application.Current.MainPage = new NavigationPage(new AddView(local, localitem, localstat));
            });
            FadeOut = 100.0;
            IsRunning = false;
        }

        private void NoAccount(string documents)
        {
            var FakeLogin = Path.Combine(documents, _UserModel.Username + "Login.dt");
            var FakeTimers = Path.Combine(documents, _UserModel.Username + "Timer.dt");
            var FakeItems = Path.Combine(documents, _UserModel.Username + "Inv.dt");
            var FakeStats = Path.Combine(documents, _UserModel.Username + "Stats.dt");

            if (File.Exists(FakeLogin))
            {
                File.Delete(FakeLogin);
                File.Delete(FakeTimers);
                File.Delete(FakeItems);
                File.Delete(FakeStats);
                throw new Exception("No online account found, Deleting Local Data.");
            }
            throw new Exception("No account found");
        }
        private void NoAccountLogged(string documents, string file, string Logged)
        {
            var FakeLogin = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Login.dt");
            var FakeTimers = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Timer.dt");
            var FakeItems = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Inv.dt");
            var FakeStats = Path.Combine(documents + "/Users", UserModel.CheckForstring(file, "Username:") + "Stats.dt");
            if (File.Exists(FakeLogin))
            {
                File.Delete(FakeLogin);
                File.Delete(FakeTimers);
                File.Delete(FakeItems);
                File.Delete(FakeStats);
                UserModel.Rewrite("Username:", "", Logged);
                UserModel.Rewrite("Password:", "", Logged);
                throw new Exception("No online account found, Deleting Local Data." + documents + "/Users" + UserModel.CheckForstring(file, "Username:") + "Login.dt");
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

        public event PropertyChangedEventHandler PropertyChanged;

        // OnPropertyChanged will raise the PropertyChanged event passing the
        // source property that is being updated.
        private void OnPropertyChanged(object sender, string propertyName)
        {

            if (this.PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
