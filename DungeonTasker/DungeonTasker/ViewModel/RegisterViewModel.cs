using DungeonTasker.FirebaseData;
using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation;
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public Command RegisterBtn { get; set; }

        private bool isrunning;
        public bool IsRunning
        {
            get { return isrunning; }
            set { isrunning = value; OnPropertyChanged(this, "IsRunning"); }
        }

        RegisterView page;

        /*
         * A constructor for the RegisterViewModel
         * 
         * @Param page: parse the register page to display an alert page if an error occured.
         * Returns Nothing.
         */
        public RegisterViewModel(RegisterView page)
        {
            this.page = page;
            Username = "";
            Password = "";
            FullName = "";
            RegisterBtn = new Command(async () => await RegisterAddAccountFIREBASE());
        }

        /*
        * Whenever the register button is called create a file with the details parsed through the Entry controls
        * 
        * PARAM 
        * sender: reference to the control object
        * eventargs: object data
        * RETURNS Nothing
        */
        public async Task RegisterAddAccountFIREBASE(bool test = false)
        {
            try
            {
                if ((!Username.Equals("") && !Password.Equals("") && !FullName.Equals("")))// check if both username and password fields are filled
                {
                    IsRunning = true;
                    FirebaseUser client = new FirebaseUser();
                    await client.Register(FullName, Username, Password, Navigation);
                    IsRunning = false;
                }
                else
                {
                    throw new Exception("Please enter all credentials... ");// throw exception
                }
            }
            catch (Exception es)
            {
                if (es != null) { await page.DisplayAlert("Error", es.Message , "Close"); }// display error message
                else { await page.DisplayAlert("Error", "Please delete current account", "Close"); }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // OnPropertyChanged will raise the PropertyChanged event passing the
        // source property that is being updated.
        private void OnPropertyChanged(object sender, string propertyName)
        {

            if (this.PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
