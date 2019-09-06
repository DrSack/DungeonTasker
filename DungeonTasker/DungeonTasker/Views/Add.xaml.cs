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
	public partial class Add : MasterDetailPage
	{
        User nice;//Initialize and store information of corresponding variables
        InventoryItems Inv;

        logged truth = new logged();// Used for telling threads of the application to stop running whenever this is false
      
        /*
         * Initialize all components and classes for traversing through both details page and masterpage
         * PARAM 
         * user: the user data
         * items: the items data
         * RETURNS Nothing
         */
        public Add(User user, InventoryItems items)
		{
            this.nice = user;
            this.Inv = items;
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            this.Detail = new NavigationPage(new Tasks(user, items, truth));// Set Detailspage arguments with user information and truth value.
            this.Master = new MasterPage(Detail, user, items, truth);// set the masterpage information with user, items, and truth valus.
        }
    }
}