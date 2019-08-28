using DungeonTasker.Models;
using System;
using System.Collections.Generic;
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
        private int Seconds = 0;
        User Currentuser;
		public DetailsPage(User user)
		{
            InitializeComponent ();
            this.Currentuser = user;
            Name.Text = Currentuser.Username;
            Character.Text = Currentuser.Character;
        }

        public async void Add_Time(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Set time: ", "Cancel", null, "3 Seconds", "1 hour", "1 day");

            if (action != "Cancel")
            {
                if (action == "3 Seconds")
                {
                    Seconds = 3;

                    var timerlads = new StackLayout();
                    timerlads.Orientation = StackOrientation.Horizontal;
                    timerlads.BackgroundColor = Color.White;
                    timerlads.Margin = new Thickness(3, 1, 3, 1);

                    var cool = new Label { Text = action };
                    var cool2 = new Label { Text = Seconds.ToString() };
                    TimerUpdatecs time = new TimerUpdatecs(Seconds);
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                    time.time -= 1;
                    cool2.Text = time.time.ToString();

                    if (time.time == 0)
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
                            };
                            timerlads.Children.Add(redeembtn);
                            timers.Children.Add(timerlads);
                            return false;
                        }
                        return true;
                    });

                    timerlads.Children.Add(cool);
                    timerlads.Children.Add(cool2);
                    timers.Children.Add(timerlads);
                }
            }
        }
    }
}