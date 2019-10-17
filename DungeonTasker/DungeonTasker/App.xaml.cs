using DungeonTasker.Models;
using Firebase.Database.Query;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DungeonTasker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new SpashScreen();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            try
            {
                Globals.LOGGED.Object.Logged = "False";
                Globals.CLIENT
                    .Child(string.Format("{0}Login", Globals.LOGGED.Object.Username))
                    .Child(Globals.LOGGED.Key).PutAsync(Globals.LOGGED.Object);
            }
            catch { }
            
        }

        protected override void OnResume()
        {
            try
            {
                Globals.LOGGED.Object.Logged = "True";
                Globals.CLIENT
                    .Child(string.Format("{0}Login", Globals.LOGGED.Object.Username))
                    .Child(Globals.LOGGED.Key).PutAsync(Globals.LOGGED.Object);
            }
            catch { }
        }
    }
}
