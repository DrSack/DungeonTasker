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
        public SpashScreen()
        {
            createSplash("Welcome");
        }

        protected override async void OnAppearing()
        {

            base.OnAppearing();
            await this.FadeTo(1, 1500);
            await this.FadeTo(0, 1500);
            Application.Current.MainPage = new NavigationPage(new GreetPage());

        }

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
