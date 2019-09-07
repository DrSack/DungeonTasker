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
        Dungeon dungeon;
        WeaponInfo weapon;
        Stats stats;
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

        public MasterPage (Page page, User user, InventoryItems items, WeaponInfo weapon, Stats stats,  logged truth)
		{
			InitializeComponent ();
            this.page = page;
            this.user = user;
            this.items = items;
            this.truth = truth;
            this.weapon = weapon;
            this.stats = stats;
            weapon.SetWeapon(this, User.CheckForstring(items.Invfile, "Equipped:"));
            dungeon = new Dungeon(this.user, this.items, this.weapon, this.stats);
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
            User.Rewrite("Logged:", "false", user.file);
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
            
            ((MasterDetailPage)Parent).Detail = new NavigationPage(new Inventory(items, this.weapon));
            ((MasterDetailPage)Parent).IsPresented = false;

        }

        /*
         * If this is selected change the detailspage to the Dungeonpage
         * 
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */

        private void Dungeon_Clicked(object sender, EventArgs e)
        {
            dungeon.selectKey();
            ((MasterDetailPage)Parent).Detail = new NavigationPage(dungeon);
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
            ((MasterDetailPage)Parent).Detail = new NavigationPage(new Inventory(items, this.weapon));
            ((MasterDetailPage)Parent).IsPresented = false;
        }
    }
}