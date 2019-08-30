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
        bool begin = true;
        public GreetPage()
        {
            InitializeComponent();
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var files = System.IO.Directory.GetFiles(documents);
            
            RegisterBtn();
        }


        public void RegisterBtn()
        {
            var RegisterClicked = new TapGestureRecognizer();
            RegisterClicked.Tapped += (s, e) =>
            {
               Navigation.PushAsync(new Register());
            };
            Register.GestureRecognizers.Add(RegisterClicked);
        }

        public async void Login()
        {
            try
            {
                string[] line;
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var files = Directory.GetFiles(documents);
                bool hit = false;
                foreach (var file in files)
                {
                    string nice2;
                    using (StreamReader sr = new StreamReader(file)) { nice2 = sr.ReadToEnd(); }
                    line = nice2.Split(',');

                    if (line[0].Contains("ID:")) { line[0] = line[0].Replace("ID:", ""); }

                    if (EntryMrk.Text == line[0] && EntryMrk2.Text == line[1])
                    {
                        hit = true;
                        ExtraPopups.LoginWrite(this, file, line);

                    }
                    else if (EntryMrk.Text == line[0] && EntryMrk2.Text != line[1])
                    {
                        hit = true;
                        throw new Exception("Incorrect Password");
                    }
                }
                if(!User.checkinfo(EntryMrk.Text, EntryMrk2.Text))
                {
                    throw new Exception("Please enter both username and password");
                }
                else if (!hit)
                {
                    throw new Exception("Account not found");
                }
            }
            catch (Exception es)
            {
                if (es != null) { await DisplayAlert("Error", es.Message, "Close"); }
                else { await DisplayAlert("Error", "Account not found", "Close"); }
            }
        }

        public bool CheckAccounts(string[] files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
        protected override void OnAppearing()
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
            if (begin && !CheckAccounts(files))
            {
                Application.Current.MainPage.DisplayAlert("Welcome", "Welcome to Dungeon Tasker new user", "close");
                begin = false;
            }
            
            
        }


        private void LoginBtn(object sender, EventArgs e)
        {
            Login();
        }
      
    }
}