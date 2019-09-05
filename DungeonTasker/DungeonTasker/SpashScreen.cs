using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using Xamarin.Forms;
using System.Threading.Tasks;
using DungeonTasker.Models;
using System.IO;

namespace DungeonTasker
{
    class SpashScreen : ContentPage
    {
        /*
         * This is the Contructor, which calls the splash screen
         * 
         */

        public SpashScreen()
        {
            createSplash("Welcome");
        }
        /*
         * Call this whenever the screen appears
         * This fades in and out hen changes the mainpage to the GreetPage for smoother design.
         */
        protected override async void OnAppearing()
        {

            base.OnAppearing();
            await this.FadeTo(1, 1500);
            await this.FadeTo(0, 1500);
            Application.Current.MainPage = new NavigationPage(new GreetPage());

        }

        /*
         * Creates a splashscreen to display text in the middle of the screen 
         * PARAM
         * text: the splashscreen name
         * RETURNS Nothing
         */

        public void createSplash(string text)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            this.BackgroundColor = Color.Black;
            var layout = new StackLayout
            {
                BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.Center

            };

            var label = new Label
            {
                Text = text,
                TextColor = Color.White,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center
            };

            layout.Children.Add(label);
            this.Content = layout;
        }
    }
}
