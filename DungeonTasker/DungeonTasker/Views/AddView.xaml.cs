using DungeonTasker.Models;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DungeonTasker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddView : MasterDetailPage
	{
        logged truth = new logged();// Used for telling threads of the application to stop running whenever this is false
      
        /*
         * Initialize all components and classes for traversing through both details page and masterpage
         * PARAM 
         * user: the user data
         * items: the items data
         * RETURNS Nothing
         */
        public AddView(UserModel user, InventoryItemsModel items, StatsModel stats)
		{
            InitializeComponent();
            WeaponInfoModel weapon = new WeaponInfoModel(items);
            ItemInfoModel Invitem = new ItemInfoModel(items);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            DungeonView dungeon = new DungeonView(user, items, weapon, Invitem, stats, false);
            this.Detail = new NavigationPage(new TasksView(user, items, truth, dungeon));// Set Detailspage arguments with user information and truth value.
            this.Master = new MasterPageView(Detail, user, items, weapon, truth, dungeon, Invitem);// set the masterpage information with user, items, and truth values.
            
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "No", "Yes");
                if (!result) System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow(); // or anything else
            });

            return true;
        }
    }
}