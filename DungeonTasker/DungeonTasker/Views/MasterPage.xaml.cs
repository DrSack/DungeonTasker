using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace DungeonTasker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
        Page page;
        User user;
        InventoryItems items;
        logged truth;

        /*
         * Contructor for Masterpage, initialize all variables 
         * 
         * PARAM
         * page: encapsulate page
         * user: encapsulate user
         * items: encapsulate items
         * truth: encapsulate truth
         * 
         * RETURNS Nothing
         */

        public MasterPage (Page page, User user, InventoryItems items, logged truth)
		{
			InitializeComponent ();
            this.page = page;
            this.user = user;
            this.items = items;
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
        private void Exit_Clicked(object sender, EventArgs e)
        {
            user.Logged = "false";
            user.Rewrite("false");
            truth.nice = false;
            if (User.CheckForstring(user.file, "Logged:") == "false" )
            {
                File.WriteAllText(user.file, File.ReadAllText(user.file));
                Application.Current.MainPage = new NavigationPage(new SpashScreen());
            }
            
            
        }

        /*
         * If this is selected change the detailspage to the taskpage
         * 
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */

        private void Tasks_Clicked(object sender, EventArgs e)
        {
            ((MasterDetailPage)Parent).Detail = page;
            ((MasterDetailPage)Parent).IsPresented = false;
        }

        /*
         * If this is selected change the detailspage to the storepage
         * 
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */
        private void Store_Clicked(object sender, EventArgs e)
        {
            
            ((MasterDetailPage)Parent).Detail = new NavigationPage(new Inventory(items));
            ((MasterDetailPage)Parent).IsPresented = false;

        }

        /*
         * If this is selected change the detailspage to the settings
         * 
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */

        private void Settings_Clicked(object sender, EventArgs e)
        {
            ((MasterDetailPage)Parent).Detail = new NavigationPage(new Inventory(items));
            ((MasterDetailPage)Parent).IsPresented = false;
        }
    }
}