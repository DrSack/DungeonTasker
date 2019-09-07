using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DungeonTasker.Models
{
    class ExtraPopups:ContentPage
    {
        /*
         *  Creates a display alert that has await assigned to it such that the user must close this before any action is taken afterwards
         *  PARAM
         *  message: message of the display alert
         *  title: the title for the display alert
         *  buttonText: the string for the button
         *  page: the current contentpage
         *  afterHideCallback: action to be take
         *  Returns Nothing
         */
        public static async Task ShowMessage(string message, string title,string buttonText, ContentPage page, Action afterHideCallback)
        {
            await page.DisplayAlert(title,message,buttonText);
            afterHideCallback?.Invoke();
        }
        
        /*
         * A static method when called intializes all strings and 
         * content pages to be parsed through the User class to be used 
         * throughout the entire program once logged in.
         * 
         * PARAM
         * page: the Greetpage content page
         * file: the user file that contains all user information
         * times: the timer file that contains current running timers that are still operational or havent been closed
         * items: the items file contains all of the items that the user currently has
         * line: this array contains the user information to be parsed within the initial User class
         * 
         * RETURNS Nothing
         */
        public static async void LoginWrite(GreetPage page, string file, string times, string items , string stats, string[] line)
        {
            string stringfile;
            using (StreamReader sr = new StreamReader(file)) { stringfile = sr.ReadToEnd(); }
            using (var outputfile = new StreamWriter(file, false))
            {
                stringfile = stringfile.Replace("Logged:false", "Logged:true");// change the logged status to true
                outputfile.Write(stringfile);
            }

            string character = User.CheckForstring(file, "Character:");
            string logged = User.CheckForstring(file, "Logged:");//obtain file information

            User user = new User(line[0], line[1], character, logged, file, times);
            InventoryItems item = new InventoryItems(items);
            Stats stat = new Stats(stats);

            await page.FadeTo(1, 300);
            await page.FadeTo(0, 300);//fade in and out to transition to next screen

            Application.Current.MainPage = new NavigationPage(new Add(user,item,stat));// replace the mainpage with Add()
        }

    }
}
