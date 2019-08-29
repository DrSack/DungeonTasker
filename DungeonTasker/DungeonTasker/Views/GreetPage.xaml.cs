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

    public partial class GreetPage : ContentPage
    {
        bool startup = true;

        public GreetPage()
        {
            InitializeComponent();
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var files = System.IO.Directory.GetFiles(documents);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            if (!CheckAccounts(files))
            {
                NonUsers();
            }
            else
            {
                Update(files);
            }
            RegisterBtn();
            startup = false;
        }

        public async void Update(string[] files)
        {
            try
            {
                foreach (var file in files)
                {
                    string[] lines;
                    var checking = File.ReadAllText(file);
                    lines = checking.Split(',');
                    if ((lines[2] == "trueB" && lines[3] != "checked") || (lines[2] == "trueB" && startup))
                    {
                        AddUsers(file);
                    }
                }
                if (UsersStack.Children.Count == 0)
                {
                    NonUsers();
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Welcome", "Create an account using register", "Close");
            }
        }

        public void RegisterBtn()
        {
            var RegisterClicked = new TapGestureRecognizer();
            RegisterClicked.Tapped += (s, e) =>
            {
               Navigation.PushAsync(new RegisterAdd(this, false));
            };
            Register.GestureRecognizers.Add(RegisterClicked);
        }

        private void AddUsers(string file)
        {
            string[] line;
            string nice;
            using (StreamReader sr = new StreamReader(file)) { nice = sr.ReadToEnd(); }

            line = nice.Split(',');
            if (line[0].Contains("ID:")) { line[0] = line[0].Replace("ID:", ""); }

            using (var outputfile = new StreamWriter(file, false))
            {
                if (nice.Contains("falseB")){nice = nice.Replace("falseB", "trueB");}
                if (!nice.Contains("checked")){nice = nice.Replace("incorrect", "checked,");}
                outputfile.Write(nice);
            }
            try
            {
                Label nonuser = (Label)UsersStack.Children[0];
                if(nonuser.Text == "No Users Added...")
                {
                    UsersStack.Children.RemoveAt(0);
                }
            }
            catch (Exception)
            {

            }
            var lul = new StackLayout();
            lul.Orientation = StackOrientation.Horizontal;
            lul.HorizontalOptions = LayoutOptions.Center;
            lul.VerticalOptions = LayoutOptions.Center;
            var name = new Label
            {
                Text = line[0],
                FontSize = 10,
                VerticalTextAlignment = TextAlignment.Center,
            };
            
            var btn = new Button { FontSize = 10, Text = "Login" };
            var btn2 = new Button { FontSize = 10, Text = "Delete" };

            btn.Clicked += async (sender, args) => ExtraPopups.Login(line, this, file);
            btn2.Clicked += async (sender, args) =>
            {
                Delete(file, lul);
            };

            lul.Children.Add(name);
            lul.Children.Add(btn);
            lul.Children.Add(btn2);
            UsersStack.Children.Add(lul);
        }
        public bool CheckAccounts(string[] files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (!File.Exists(file))
                    {
                        return false;
                    }
                    else
                    {
                        
                        return true;
                        
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
    }
        /*/
         * Check all files within local folder and see if a login file is still logged in.
         * If the login file is still logged:true then proceed to bypass login screen and go into
         * app.
         **/
        protected override async void OnAppearing()
        {
            string[] line;
            string all;
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var files = System.IO.Directory.GetFiles(documents);

            foreach (var file in files)
            {
                var textfile = File.ReadAllText(file);
                line = textfile.Split(',');
                using (StreamReader sr = new StreamReader(file)) { all = sr.ReadToEnd(); }
                if (line[0].Contains("ID:")) { line[0] = line[0].Replace("ID:", ""); }

                string character = User.CheckForstring(file, "Character:");
                string logged = User.CheckForstring(file, "Logged:");
                if (all.Contains("Logged:true"))
                {
                    User user = new User(line[0], line[1], character, logged, file);
                    Application.Current.MainPage = new NavigationPage(new Add(user));
                }
            }
        }


        private void Button_ClickedAdd(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterAdd(this, true));
        }

        public async void Delete(string file, StackLayout layout)
        {
            string[] ok;
            string nice3;

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var files = System.IO.Directory.GetFiles(documents);
            using (StreamReader sr = new StreamReader(file)) { nice3 = sr.ReadToEnd(); }

            ok = nice3.Split(',');
            if (ok[0].Contains("ID:")) { ok[0] = ok[0].Replace("ID:", ""); }
            File.Delete(file);

            Label nice2 = (Label)layout.Children[0];
                if(nice2.Text == ok[0])
                {
                    await Task.Run(async () =>
                    {
                       Animations.CloseStackLayout(layout, "Users", 30, 300);
                    });
                    UsersStack.Children.Remove(layout);
                if (UsersStack.Children.Count == 0)
                    {
                        NonUsers();
                    }
                }
            
            
        }

        public void NonUsers()
        {
            
            var nice = new Label()
            {
                Text = "No Users Added...",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            UsersStack.Children.Add(nice);
        }

      
    }
}