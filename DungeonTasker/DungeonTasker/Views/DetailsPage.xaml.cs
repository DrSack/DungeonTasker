using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DungeonTasker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailsPage : ContentPage
    {
        List<TimerUpdatecs> ListTimer = new List<TimerUpdatecs>();
        User Currentuser;
        bool truth = true;
        logged truthtime;
		public DetailsPage(User user, logged truth)
		{
            InitializeComponent ();
            this.Currentuser = user;
            this.truthtime = truth;
            Name.Text = Currentuser.Username;
            Character.Text = Currentuser.Character;
            //Currentuser.Checktimer(this);
        }

        public async void Add_Time(object sender, EventArgs e)
        {
            int i = 0;
            string action = await DisplayActionSheet("Set time: ", "Cancel", null, "10 Seconds", "15 seconds", "20 seconds");


            if (action != "Cancel")
            {
                if (action == "10 Seconds")
                {
                    i = 10;
                    Timer(action, i);
                }
                else if (action == "15 seconds")
                {
                    i = 15;
                    Timer(action, i);
                }
                else if (action == "20 seconds")
                {
                    i = 20;
                    Timer(action, i);
                }
            }
        }
        protected override void OnAppearing()
        {
            if (truth)
            {
                using (var sr = new StreamReader(Currentuser.file))
                {
                    string line;
                    string[] split;
                    while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        if (line.Contains("Timer"))
                        {
                            split = line.Split(':');
                            int result = Int32.Parse(split[2]);
                            Timer(split[1], result);
                        }
                    }
                }
                truth = false;
            }

            
        }
            public void Timer(string action, int i)
        {

            var timerlads = new StackLayout();
            timerlads.Orientation = StackOrientation.Horizontal;
            timerlads.BackgroundColor = Color.White;
            timerlads.Margin = new Thickness(3, 1, 3, 1);

            var cool = new Label { Text = action };
            var cool2 = new Label { Text = i.ToString() };
            TimerUpdatecs time = new TimerUpdatecs(i, action);
            ListTimer.Add(time);
            if(i != 0)
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    time.time -= 1;
                    cool2.Text = time.time.ToString();
                    Currentuser.UpdateCurrenttimes(ListTimer);

                    if (truthtime.nice == false)
                    {
                        return false;
                    }

                    if (time.time <= 0)
                    {
                        DisplayRedeem(timerlads, time);
                        return false;
                    }
                    return true;
                });
                timerlads.Children.Add(cool);
                timerlads.Children.Add(cool2);
                timers.Children.Add(timerlads);
            }

            

            else
            {
                timerlads.Children.Add(cool);
                timerlads.Children.Add(cool2);
                timers.Children.Add(timerlads);
                DisplayRedeem(timerlads, time);
            }

            
        }

        public void DisplayRedeem(StackLayout timerlads, TimerUpdatecs time)
        {
            Application.Current.MainPage.DisplayAlert("stuff", "stuff", "stuff");
            var redeembtn = new Button { Text = "Redeem" };
            redeembtn.Clicked += async (s, a) =>
            {
                await Task.Run(async () =>
                {
                    Animations.CloseStackLayout(timerlads, "Timer", 30, 500);
                });

                timers.Children.Remove(timerlads);
                ListTimer.Remove(time);
                Currentuser.UpdateCurrenttimes(ListTimer);
            };
            timerlads.Children.Add(redeembtn);
            timers.Children.Add(timerlads);
        }
    }
}