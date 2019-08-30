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
        logged truth;
		public MasterPage (Page page, User user, logged truth)
		{
			InitializeComponent ();
            this.page = page;
            this.user = user;
            this.truth = truth;
            
		}

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

        private void Tasks_Clicked(object sender, EventArgs e)
        {
            ((MasterDetailPage)Parent).Detail = page;
            ((MasterDetailPage)Parent).IsPresented = false;
        }

        private void Store_Clicked(object sender, EventArgs e)
        {
            
            ((MasterDetailPage)Parent).Detail = new NavigationPage(new Inventory());
            ((MasterDetailPage)Parent).IsPresented = false;

        }

        private void Terminate_Clicked(object sender, EventArgs e)
        {
            if(Device.RuntimePlatform == Device.iOS){
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else if(Device.RuntimePlatform == Device.Android)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            
        }
    }
}