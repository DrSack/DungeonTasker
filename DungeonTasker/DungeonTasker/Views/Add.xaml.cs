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
        User nice;

        public Add(User user)
		{
            this.nice = user;
            InitializeComponent();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            this.Detail = new NavigationPage(new DetailsPage(user));
            this.Master = new MasterPage(Detail, user);
        }
    }
}