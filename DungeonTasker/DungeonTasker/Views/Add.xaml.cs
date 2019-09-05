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
        User nice;// Store the user information
        logged truth = new logged();// Used for telling threads of the application to stop running whenever this is false
        InventoryItems Inv;

        //Initialize all components and classes for traversing through both details page and masterpage
        public Add(User user, InventoryItems items)
		{
            this.nice = user;
            this.Inv = items;
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            this.Detail = new NavigationPage(new DetailsPage(user, truth));
            this.Master = new MasterPage(Detail, user, items, truth);
        }
    }
}