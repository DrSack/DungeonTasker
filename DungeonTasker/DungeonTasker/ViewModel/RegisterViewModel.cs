﻿using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    class RegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public Command RegisterBtn { get; set; }
        Register page;
        /*
         * A constructor for the RegisterViewModel
         * 
         * @Param page: parse the register page to display an alert page if an error occured.
         * Returns Nothing.
         */
        public RegisterViewModel(Register page)
        {
            this.page = page;
            Username = "";
            Password = "";
            FullName = "";
            RegisterBtn = new Command(async () => await RegisterAddAccount());
        }

        /*
        * Whenever the register button is called create a file with the details parsed through the Entry controls
        * 
        * PARAM 
        * sender: reference to the control object
        * eventargs: object data
        * RETURNS Nothing
        */
        private async Task RegisterAddAccount()
        {
            try
            {
                if ((!Username.Equals("") && !Password.Equals("") && !FullName.Equals("")))// check if both username and password fields are filled
                {
                    User.StoreInfo(Username, Password, FullName, page);// store and create new files based on the information given
                }
                else
                {
                    throw new Exception("Please enter all credentials... ");// throw exception
                }
            }
            catch (Exception es)
            {
                if (es != null) { await page.DisplayAlert("Error", es.Message, "Close"); }// display error message
                else { await page.DisplayAlert("Error", "Please delete current account", "Close"); }
            }
        }
    }
}
