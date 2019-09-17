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
    public partial class Tasks : ContentPage
    {
        List<TimerUpdatecs> ListTimer = new List<TimerUpdatecs>();
        User Currentuser;
        InventoryItems items;
        logged truthtime;
        bool truth = true;//Initialize all variables

        /*
         * Contructor for DetailsPage to encapsulate current user information and truth value.
         * PARAM
         * user: parse the user to be used within this class
         * truth: parse truth to notify Device.StartTimer to stop whenever truthtime is off
         * RETURN Nothing
         */
        public Tasks(User user, InventoryItems items, logged truth)
        {
            InitializeComponent();
            this.Currentuser = user;
            this.truthtime = truth;
            this.items = items;
            Name.Text = Currentuser.Username;
            Character.Text = Currentuser.Character;// Initialize all components
        }

        /*
         * Simple button even to open another page for selecting a date and time for a new task
         * PARAM 
         * sender: reference to the control object
         * eventargs: object data
         * RETURNS Nothing
         */
        public async void Add_Time(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new DatePicker(this));// open DatePicker
        }

        /*
         * A method that is called whenever this page appears and 
         * systematically scans through each singleline of the timer file within the user class 
         * and checks whenever timer information has been found. Where the timer information and 
         * its corresponding name is taken and parsed through the Timer method to display each 
         * current operating timers still active according to the timer file
         * 
         * PARAM void.
         * RETURN Nothing
         */
        protected override void OnAppearing()
        {
            if (truth)// if this is the first time opening the content page
            {
                using (var sr = new StreamReader(Currentuser.timer))
                {
                    string line;
                    string[] split;
                    while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        split = line.Split(',');
                        DateTime nice = Convert.ToDateTime(split[1]);
                        TimeSpan Rem = nice - DateTime.Now;// store information into variables
                        Timer(split[0], nice, Rem);//call Timer and parse variables
                    }
                }
                truth = false;// declare false so timers wont be added again
            }


        }

        /*
         * This method is responsible for creating timers based on DateTime and TimeSpan 
         * information which is parse from the DatePicker page
         * 
         * PARAM
         * Task: A string which contains the title of the task
         * Trg: The end date of the task 
         * Rem: The leftover time remaining
         * 
         * RETURNS Nothing
         */
        public void Timer(string Original, DateTime Trg, TimeSpan Rem)
        {
            string TaskName = Original;
            bool TimerStop = false;
            var timerlads = new StackLayout();
            timerlads.Orientation = StackOrientation.Horizontal;
            timerlads.BackgroundColor = Color.White;
            timerlads.Margin = new Thickness(3, 1, 3, 1); // Initialize Stacklayout and its properties

            var cool = new Label { Text = TaskName };
            var cool2 = new Label();
            var Edit = new Button();
            var Delete = new Button();

            Edit.Text = "Edit";
            Edit.HorizontalOptions = LayoutOptions.EndAndExpand;
            Delete.Text = "Delete";
            Delete.HorizontalOptions = LayoutOptions.End;
            TimerUpdatecs time = new TimerUpdatecs(Trg, Rem, TaskName); // Initialize labels and TimerUpdatecs object

            time.R = time.T - DateTime.Now;

            if (DateTime.Now >= time.T)
            { // If the current time is overdue then time remainging is 00:00:00
                time.R = DateTime.Now - DateTime.Now;
            }

            cool2.Text = string.Format("{0}:{1}:{2}", time.R.TotalHours.ToString("00"),
            time.R.Minutes.ToString("00"), time.R.Seconds.ToString("00"));

            Edit.Clicked += async (s, a) =>
            {
                await this.Navigation.PushModalAsync(new EditTask(time,ListTimer,Currentuser, cool));
            };

            Delete.Clicked += async (s, a) =>
            {
                await Task.Run(async () =>
                {
                    Animations.CloseStackLayout(timerlads, "Timer", 30, 500);
                });

                TimerStop = true;
                timers.Children.Remove(timerlads);
                ListTimer.Remove(time);
                Currentuser.UpdateCurrenttimes(ListTimer);
            };

            ListTimer.Add(time); // Add the TimerUpdatecs time variable to the ListTimer list
            Currentuser.UpdateCurrenttimes(ListTimer); // Update the current times on the file

            if (DateTime.Now < Trg) // Check whenever the current date is still under the end date of the task.
            {

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                { // Start timer to be run by a background thread
                    time.R = time.T - DateTime.Now; // Update remaining time
                    cool2.Text = string.Format("{0}:{1}:{2}", time.R.TotalHours.ToString("00"),
                    time.R.Minutes.ToString("00"), time.R.Seconds.ToString("00"));

                    if (truthtime.TasksRun == false) // Check whenever the truthtime boolean encapsulation class is false
                    {
                        return false;
                    }

                    if (TimerStop == true)
                    {
                        return false;
                    }

                    if (DateTime.Now >= Trg)// if the timer is equal to the end date or over
                    {
                        timerlads.Children.Remove(Delete);
                        timerlads.Children.Remove(Edit);
                        DisplayRedeem(timerlads, time, cool.Text);
                        return false;// 
                    }

                    return true;// return true to continue thread and timer operation
                });

                timerlads.Children.Add(cool);
                timerlads.Children.Add(cool2);
                timerlads.Children.Add(Edit);
                timerlads.Children.Add(Delete);
                timers.Children.Add(timerlads);// add all controls to stack layouts
            }

            else// if over due display redeem button
            {
                timerlads.Children.Add(cool);
                timerlads.Children.Add(cool2);
                timers.Children.Add(timerlads);// add all controls to stack layouts
                DisplayRedeem(timerlads, time, cool.Text);
            }

        }



        /*
         * This method is responsible for displaying the redeem button and updating the 
         * current timer file with new timer information and removing the current element from the ListTimer
         * 
         * PARAM
         * timerlads: the stacklayout that is to be updated with the redeem button
         * times: the current TimerUpdatecs to be removed from the ListTimer list 
         * RETURNS Nothing
         */

        public void DisplayRedeem(StackLayout timerlads, TimerUpdatecs times, string task)
        {
            Application.Current.MainPage.DisplayAlert("Reminder Alert", string.Format("Assigned Task:{0} Timer is Done", task), "Close");
            var redeembtn = new Button { Text = "Redeem" };
            redeembtn.HorizontalOptions = LayoutOptions.EndAndExpand;
            redeembtn.Clicked += async (s, a) =>
            {
                await Task.Run(async () =>
                {
                    Animations.CloseStackLayout(timerlads, "Timer", 30, 500);
                });

                timers.Children.Remove(timerlads);
                ListTimer.Remove(times);
                Currentuser.UpdateCurrenttimes(ListTimer);
                items.GiveKey(1);
            };
            timerlads.Children.Add(redeembtn);
            timers.Children.Add(timerlads);
        }

    }
}