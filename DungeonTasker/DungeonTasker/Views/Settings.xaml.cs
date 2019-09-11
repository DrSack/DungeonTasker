using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DungeonTasker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    /**
     * username hi password w
     * 
     * 
     * 
     */
    public partial class Settings : ContentPage
    {
        User user;
        logged truth;

        public Settings(User user, logged truth)
        {
            InitializeComponent();
            this.user = user;
            this.truth = truth;


        }

        /*
         * If user decides to logout set the word "logged:true" to false such that the user isnt logged 
         * automatically back onto their account next time they open the app
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */
        private void Button_Clicked(object sender, EventArgs e)
        {
            user.Logged = "false";
            User.Rewrite("Logged:", "false", user.file);
            truth.nice = false;
            if (User.CheckForstring(user.file, "Logged:") == "false")
            {
                File.WriteAllText(user.file, File.ReadAllText(user.file));
                Application.Current.MainPage = new NavigationPage(new SpashScreen());
            }

        }
    }
}