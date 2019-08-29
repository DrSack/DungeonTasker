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
        public static async void Login(string[]line, GreetPage Greet, string file)
        {
                var lblTitle = new Label { Text = "Title", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
                var lblMessage = new Label { Text = "Enter Password:" };
                var txtInput = new Entry { Text = "" };
                var page = new ContentPage();

                var btnOk = new Button
                {
                    Text = "Ok",
                    WidthRequest = 100,
                    BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
                };

                btnOk.Clicked += async (s, e) =>
                {
                    if (line[1] == txtInput.Text)
                    {
                        User user;
                        string nice;

                        using (StreamReader sr = new StreamReader(file)) { nice = sr.ReadToEnd();}
                        using (var outputfile = new StreamWriter(file, false))
                        {
                            if (nice.Contains("Character:"))
                            {
                                nice = nice.Replace("Logged:false", "Logged:true");
                            }
                            else
                            {
                                nice+= "\nCharacter:<0-0>";
                                nice+= "\nLogged:true";
                            }
                            outputfile.Write(nice);
                        }

                        string character = User.CheckForstring(file, "Character:");
                        string logged = User.CheckForstring(file, "Logged:");

                        user = new User(line[0], line[1], character, logged, file);
                        
                        await page.FadeTo(1, 300);
                        await page.FadeTo(0, 300);
                        await Greet.FadeTo(1, 300);
                        await Greet.FadeTo(0, 300);
                        
                        Application.Current.MainPage = new NavigationPage(new Add(user));
                    }
                    else
                    {
                        lblMessage.Text = "Incorrect Password";
                    }

                };

                var btnCancel = new Button
                {
                    Text = "Cancel",
                    WidthRequest = 100,
                    BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
                };

                btnCancel.Clicked += async (s, e) =>
                {
                    await Greet.Navigation.PopModalAsync();
                };

                var slButtons = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { btnOk, btnCancel },
                };

                var layout = new StackLayout
                {
                    Padding = new Thickness(0, 40, 0, 0),
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = { lblTitle, lblMessage, txtInput, slButtons },
                };


                page.Content = layout;
                await Greet.Navigation.PushModalAsync(page);
                txtInput.Focus();

        }

        public static async 
        Task
            ShowMessage(string message, string title,string buttonText, ContentPage page, Action afterHideCallback)
        {
            await page.DisplayAlert(title,message,buttonText);
            afterHideCallback?.Invoke();
        }
        
        public static async void LoginWrite(GreetPage page, string file, string[] line)
        {
            User user;
            string stringfile;

            using (StreamReader sr = new StreamReader(file)) { stringfile = sr.ReadToEnd(); }
            using (var outputfile = new StreamWriter(file, false))
            {
                if (stringfile.Contains("Character:"))
                {
                    stringfile = stringfile.Replace("Logged:false", "Logged:true");
                }
                else
                {
                    stringfile += "\nCharacter:<0-0>";
                    stringfile += "\nLogged:true";
                }
                outputfile.Write(stringfile);
            }

            string character = User.CheckForstring(file, "Character:");
            string logged = User.CheckForstring(file, "Logged:");

            user = new User(line[0], line[1], character, logged, file);

            await page.FadeTo(1, 300);
            await page.FadeTo(0, 300);

            Application.Current.MainPage = new NavigationPage(new Add(user));
        }

    }
}
