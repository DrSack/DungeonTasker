using DungeonTasker;
using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DungeonTasker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Register : ContentPage
	{
        GreetPage nice;
		public Register(GreetPage cool)
		{
			InitializeComponent();
            this.Title = "Create Account";
            this.RegisterAddbtn.Text = "Create";
            this.nice = cool;
            this.EntryMrk2.IsPassword = true;
		}

        private async void RegisterAddAccount(object sender, EventArgs e)
        {
                try
                {
                    if ((!EntryMrk.Text.Equals("") && !EntryMrk2.Equals("")))
                    {
                            User.StoreInfo(EntryMrk.Text, EntryMrk2.Text, this);
                    }
                    else
                    {
                        throw new Exception("Please enter both credentials... ");
                    }
                }
                catch (Exception es)
                {
                    if (es != null) { await DisplayAlert("Error", es.Message, "Close"); }
                    else { await DisplayAlert("Error", "Please delete current account", "Close"); }
                }
        }
    }
}