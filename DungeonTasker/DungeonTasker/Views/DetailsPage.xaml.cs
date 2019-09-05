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
            InitializeComponent();
            this.Currentuser = user;
            this.truthtime = truth;
            Name.Text = Currentuser.Username;
            Character.Text = Currentuser.Character;
            //Currentuser.Checktimer(this);
        }

        public async void Add_Time(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new DatePicker(this));
        }

        protected override void OnAppearing()
        {
            if (truth)
            {
                using (var sr = new StreamReader(Currentuser.timer))
                {
                    string line;
                    string[] split;
                    while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        split = line.Split(',');
                        DateTime nice = Convert.ToDateTime(split[1]);
                        TimeSpan Rem = nice - DateTime.Now;
                        Timer(split[0], nice, Rem);
                    }
                }
                truth = false;
            }


        }
        public void Timer(string Task, DateTime Trg, TimeSpan Rem)
        {
            var timerlads = new StackLayout();
            timerlads.Orientation = StackOrientation.Horizontal;
            timerlads.BackgroundColor = Color.White;
            timerlads.Margin = new Thickness(3, 1, 3, 1);

            var cool = new Label { Text = Task };
            var cool2 = new Label();
            TimerUpdatecs time = new TimerUpdatecs(Trg, Rem, Task);

            time.R = time.T - DateTime.Now;

            if (DateTime.Now >= time.T)
            {
                time.R = DateTime.Now - DateTime.Now;
            }

            cool2.Text = string.Format("{0}:{1}:{2}", time.R.TotalHours.ToString("00"),
            time.R.Minutes.ToString("00"), time.R.Seconds.ToString("00"));
            ListTimer.Add(time);
            Currentuser.UpdateCurrenttimes(ListTimer);

            if (DateTime.Now < Trg)
            {

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    time.R = time.T - DateTime.Now;
                    cool2.Text = string.Format("{0}:{1}:{2}", time.R.TotalHours.ToString("00"),
                    time.R.Minutes.ToString("00"), time.R.Seconds.ToString("00"));

                    if (truthtime.nice == false)
                    {
                        return false;
                    }

                    if (DateTime.Now >= Trg)
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
        

        

        public void DisplayRedeem(StackLayout timerlads, TimerUpdatecs times)
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
                ListTimer.Remove(times);
                Currentuser.UpdateCurrenttimes(ListTimer);
            };
            timerlads.Children.Add(redeembtn);
            timers.Children.Add(timerlads);
        }
    }
}