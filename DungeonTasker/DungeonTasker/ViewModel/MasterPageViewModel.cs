using DungeonTasker.Models;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    public class MasterPageViewModel
    {
        public Command Tasks_Clicked { get; set; }
        public Command Inventory_Clicked { get; set; }
        public Command Dungeon_Clicked { get; set; }
        public Command Settings_Clicked { get; set; }
        public Command About_Clicked { get; set; }

        Page page;// This is the tasks page set to be binded to the Tasks_clicked command
        User user;
        InventoryItems items;
        Dungeon dungeon;
        WeaponInfo weapon;
        Stats stats;
        logged truth;
        MasterPage mainpage;
        ContentPage display;

        /*
         * Contructor for Masterpage, initialize all components and BindingContext 
         * 
         * PARAM
         * page: encapsulate page
         * user: encapsulate user
         * items: encapsulate items
         * weapon: encapsulate weapon
         * truth: encapsulate truth
         * mainpage: encapsulate mainpage
         * ContentPage: encapsulate display
         * 
         * RETURNS Nothing
         */
        public MasterPageViewModel(Page page, User user, InventoryItems items, WeaponInfo weapon, Stats stats, logged truth, MasterPage mainpage, ContentPage display)
        {
            this.page = page;
            this.user = user;
            this.items = items;
            this.truth = truth;
            this.weapon = weapon;
            this.stats = stats;
            this.mainpage = mainpage;
            this.display = display;
            weapon.SetWeapon(display, User.CheckForstring(items.Invfile, "Equipped:"));
            dungeon = new Dungeon(this.user, this.items, this.weapon, this.stats);
            Tasks_Clicked = new Command(() => TaskNav());
            Inventory_Clicked = new Command(() => InventoryNav());
            Dungeon_Clicked = new Command(() => DungeonNav());
            About_Clicked = new Command(() => AboutNav());
            Settings_Clicked = new Command(() => SettingsNav());
        }

        /*
         * If this is selected change the detailspage to the taskpage
         * 
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */
        private void TaskNav()
        {
            ((MasterDetailPage)mainpage.Parent).Detail = page;
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;   
        }

        /*
         * If this is selected change the detailspage to the storepage
         * 
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */
        private void InventoryNav()
        {

            ((MasterDetailPage)mainpage.Parent).Detail = new NavigationPage(new Inventory(items, this.weapon));
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;

        }

        /*
        * If this is selected change the detailspage to the Dungeonpage
        * 
        * PARAM 
        * sender: reference to the control object
        * eventargs: object data
        * RETURNS Nothing
        */
        private void DungeonNav()
        {
            dungeon.selectKey();
            ((MasterDetailPage)mainpage.Parent).Detail = new NavigationPage(dungeon);
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;

        }


        private void AboutNav()
        {
            ((MasterDetailPage)mainpage.Parent).Detail = new NavigationPage(new AboutUs());
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;
        }
        /*
         * If this is selected change the detailspage to the settings
         * 
         * PARAM 
         * sender:reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */
        private void SettingsNav()
        {

            ((MasterDetailPage)mainpage.Parent).Detail = new NavigationPage(new Settings(user, truth));
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;
        }
    }
}
