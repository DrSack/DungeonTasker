using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    public class SettingsViewModel
    {
        public bool CanCloseDelete { get; set; }
        public bool CanCloseLogout { get; set; }
        User user;
        logged truth;
        ContentPage page;
        public Command DeleteAccountBtn { get; set; }
        public Command LogoutBtn { get; set; }

        public SettingsViewModel(User user, logged truth, ContentPage page)
        {
            CanCloseDelete = true;
            CanCloseLogout = true;
            this.user = user;
            this.truth = truth;
            this.page = page;
            DeleteAccountBtn = new Command(async () => await DeleteAccount());
            LogoutBtn = new Command(async() => await Logout());
        }

        /*
         * If user decides to logout set the word "logged:true" to false such that the user isnt logged 
         * automatically back onto their account next time they open the app
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */
        private async Task Logout()
        {
            CanCloseLogout = false;
            user.Logged = "false";
            User.Rewrite("Logged:", "false", user.file);
            truth.TasksRun = false;
            if (User.CheckForstring(user.file, "Logged:") == "false")
            {
                File.WriteAllText(user.file, File.ReadAllText(user.file));
                await page.FadeTo(0, 600);
                Application.Current.MainPage = new NavigationPage(new SpashScreen());
            }

        }

        private async Task DeleteAccount()
        {
            CanCloseDelete = false;
            truth.TasksRun = false;
            user.DeleteAccount();
            await page.FadeTo(0, 600);
            Application.Current.MainPage = new NavigationPage(new SpashScreen());
        }
    }
}
