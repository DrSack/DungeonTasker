using DungeonTasker.FirebaseData;
using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    public class RegisterViewModel
    {
        public INavigation Navigation;
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public Command RegisterBtn { get; set; }
        RegisterView page;

        //Empty parameter contructor for testing
        public RegisterViewModel()
        {
            Username = "";
            Password = "";
            FullName = "";
            RegisterBtn = new Command(async () => await RegisterAddAccount());
        }


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
                    FirebaseUser client = new FirebaseUser();
                    await client.Register(FullName, Username, Password, Navigation);
                }
                else
                {
                    throw new Exception("Please enter all credentials... ");// throw exception
                }
            }
            catch (Exception es)
            {
                if (es != null) { await page.DisplayAlert("Error", "a" , "Close"); }// display error message
                else { await page.DisplayAlert("Error", "Please delete current account", "Close"); }
            }
        }


        /*
        * Whenever the register button is called create a file with the details parsed through the Entry controls
        * 
        * PARAM 
        * sender: reference to the control object
        * eventargs: object data
        * RETURNS Nothing
        */
        public async Task RegisterAddAccount(bool test = false)
        {
            try
            {
                if ((!Username.Equals("") && !Password.Equals("") && !FullName.Equals("")))// check if both username and password fields are filled
                {
                    if (test) 
                    {
                        UserModel.StoreInfo(Username, Password, FullName, new RegisterView(Navigation,false), true);// store and create new files based on the information given
                    }
                    else
                    {
                        UserModel.StoreInfo(Username, Password, FullName, page);// store and create new files based on the information given
                    }
                }
                else
                {
                    throw new Exception("Please enter all credentials... ");// throw exception
                }
            }
            catch (Exception es)
            {
                if (test)
                {
                    if (es != null)
                    {
                        throw new Exception(es.Message);
                    }
                    else
                    {
                        throw new Exception("Please delete current account");
                    }
                }
                else if (es != null) { await page.DisplayAlert("Error", es.Message, "Close"); }// display error message
                else { await page.DisplayAlert("Error", "Please delete current account", "Close"); }
            }
        }
    }
}
