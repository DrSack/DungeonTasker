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
        /*
         * This is the Contructor, Initialize all controls
         * 
         */
		public Register()
		{
			InitializeComponent();
            this.Title = "Create Account";
            this.RegisterAddbtn.Text = "Create";
            this.EntryMrk2.IsPassword = true;
		}
        /*
         * Whenever the register button is called create a file with the details parsed through the Entry controls
         * 
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */
        private async void RegisterAddAccount(object sender, EventArgs e)
        {
                try
                {
                    if ((!EntryMrk.Text.Equals("") && !EntryMrk2.Equals("")))// check if both username and password fields are filled
                    {
                            User.StoreInfo(EntryMrk.Text, EntryMrk2.Text, this);// store and create new files based on the information given
                    }
                    else
                    {
                        throw new Exception("Please enter both credentials... ");// throw exception
                    }
                }
                catch (Exception es)
                {
                    if (es != null) { await DisplayAlert("Error", es.Message, "Close"); }// display error message
                    else { await DisplayAlert("Error", "Please delete current account", "Close"); }
                }
        }
    }
}