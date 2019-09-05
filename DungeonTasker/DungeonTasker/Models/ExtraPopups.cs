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
        public static async 
        Task
            ShowMessage(string message, string title,string buttonText, ContentPage page, Action afterHideCallback)
        {
            await page.DisplayAlert(title,message,buttonText);
            afterHideCallback?.Invoke();
        }
        
        public static async void LoginWrite(GreetPage page, string file, string times, string items , string[] line)
        {
            User user;
            InventoryItems item;
            string stringfile;

            using (StreamReader sr = new StreamReader(file)) { stringfile = sr.ReadToEnd(); }
            using (var outputfile = new StreamWriter(file, false))
            {
                stringfile = stringfile.Replace("Logged:false", "Logged:true");
                outputfile.Write(stringfile);
            }

            string character = User.CheckForstring(file, "Character:");
            string logged = User.CheckForstring(file, "Logged:");
            
            user = new User(line[0], line[1], character, logged, file, times);
            item = new InventoryItems(items);

            await page.FadeTo(1, 300);
            await page.FadeTo(0, 300);

            Application.Current.MainPage = new NavigationPage(new Add(user,item));
        }

    }
}
