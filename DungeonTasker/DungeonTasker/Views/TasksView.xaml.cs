using DungeonTasker.Models;
using DungeonTasker.ViewModel;
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
    public partial class TasksView : ContentPage
    {
        List<TimerUpdatecs> ListTimer = new List<TimerUpdatecs>();
        UserModel Currentuser;
        InventoryItemsModel items;
        logged truthtime;
        public DungeonView dungeon;
        bool truth = true; // Initialize all variables

        /*
         * Contructor for DetailsPage to encapsulate current user information and truth value
         * 
         * PARAM
         * user: parse the user to be used within this class
         * truth: parse truth to notify Device.StartTimer to stop whenever truthtime is off
         * 
         * RETURN Nothing
         */
        public TasksView(UserModel user, InventoryItemsModel items, logged truth, DungeonView dungeon)
        {
            this.Currentuser = user;
            this.items = items;
            this.truthtime = truth;
            this.dungeon = dungeon;
            TasksViewModel VM = new TasksViewModel(user,this)
            {
                Navigation = Navigation
            };
            BindingContext = VM;
            InitializeComponent();
        }


        /*
         * A method that is called whenever this page appears and 
         * systematically scans through each singleline of the timer file within the user class 
         * and checks whenever timer information has been found. Where the timer information and 
         * its corresponding name is taken and parsed through the Timer method to display each 
         * current operating timers still active according to the timer file
         * 
         * PARAM void
         * RETURN Nothing
         */
        protected override void OnAppearing()
        {
            if (UserModel.CheckForstring(Currentuser.file, "Tutorial:").Contains("True"))// This is the tutorial.
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await UserModel.ShowMessage(string.Format("Welcome to DungeonTasker {0}\nStart adding Tasks with the (+) button below\nGain Keys by completing Tasks!", Currentuser.Username), "Welcome!", "Close", this, async () =>
                    {
                        DatePickerView Tutorial = new DatePickerView();
                        Tutorial.Disappearing += async (s, e) =>
                        {
                            dungeon.Disappearing += (s2, e2) =>
                            {
                                UserModel.Rewrite("Tutorial:", "False", Currentuser.file);
                                this.DisplayAlert("Ready?", "You're all set!\nComplete those tasks and get some loot!.", "Close");
                            };

                            dungeon.tut = true;
                            await Navigation.PushModalAsync(dungeon);
                            dungeon.tut = false;
                        };
                        await Navigation.PushModalAsync(Tutorial);
                    });
                });
            }

            if (truth) // If this is the first time opening the content page
            {
                using (var sr = new StreamReader(Currentuser.timer))
                {
                    string line;
                    string[] split;
                    while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        split = line.Split(',');
                        DateTime nice = Convert.ToDateTime(split[1]);
                        TimeSpan Rem = nice - DateTime.Now; // Store information into variables
                        Timer(split[0], nice, Rem); // Call Timer and parse variables
                    }
                }

                truth = false; // Declare false so timers wont be added again
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
         * RETURN Nothing
         */
        public void Timer(string Original, DateTime Trg, TimeSpan Rem)
        {
            string TaskName = Original;
            bool TimerStop = false;
            var rand = new Random();

            // Initialize Stacklayout and its properties
            var timerlads = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.White,
                HeightRequest = 38,
                Margin = new Thickness(0, 0, 6, 0)
            };

            var colorTag = new Label
            {
                Text = "",
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 8,
            };

            var taskName = new Label
            {
                Text = TaskName,
                Margin = new Thickness(10, 10),
                TextColor = Color.FromHex("#212121")
            };

            var editButton = new Button
            {
                Text = "Edit",
                FontSize = 10,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.White,
                WidthRequest = 45
            };

            var countdownFinish = new Button
            {
                IsEnabled = false,
                BackgroundColor = Color.FromHex("#00CC33"),
                HorizontalOptions = LayoutOptions.End,
                FontSize = 12,
                WidthRequest = 80,
                CornerRadius = 16
            };

            // Initialize labels and TimerUpdatecs object
            TimerUpdatecs time = new TimerUpdatecs(Trg, Rem, TaskName);
            time.R = time.T - DateTime.Now;

            // If the current time is overdue then time remainging is 00:00:00
            if (DateTime.Now >= time.T)
            {
                time.R = DateTime.Now - DateTime.Now;
            }

            countdownFinish.Text = string.Format("{0}:{1}:{2}", time.R.TotalHours.ToString("00"),
            time.R.Minutes.ToString("00"), time.R.Seconds.ToString("00"));

            // Set random color tags for tasks
            int num = rand.Next(1, 5);

            if (num == 1)
            {
                colorTag.BackgroundColor = Color.Accent;
            }
            if (num == 2)
            {
                colorTag.BackgroundColor = Color.FromHex("#F44336");
            }
            if (num == 3)
            {
                colorTag.BackgroundColor = Color.FromHex("#212121");
            }
            if (num == 4)
            {
                colorTag.BackgroundColor = Color.FromHex("#FFCA28");
            }

            // When the user clicks on the edit button
            editButton.Clicked += async (s, a) =>
            {
                EditTaskView EditCurrent = new EditTaskView(time, ListTimer, Currentuser, taskName, timerlads, timers);
                EditCurrent.Disappearing += (s2, e2) =>
                {
                    TimerStop = EditCurrent.Deleted;
                };
                await this.Navigation.PushModalAsync(EditCurrent);
            };

            ListTimer.Add(time); // Add the TimerUpdatecs time variable to the ListTimer list
            Currentuser.UpdateCurrenttimes(ListTimer); // Update the current times on the file

            if (DateTime.Now < Trg) // Check whenever the current date is still under the end date of the task.
            {

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                { // Start timer to be run by a background thread
                    time.R = time.T - DateTime.Now; // Update remaining time
                    countdownFinish.Text = string.Format("{0}:{1}:{2}", time.R.TotalHours.ToString("00"),
                    time.R.Minutes.ToString("00"), time.R.Seconds.ToString("00"));

                    if (truthtime.TasksRun == false) // Check whenever the truthtime boolean encapsulation class is false
                    {
                        return false;
                    }

                    if (TimerStop == true)
                    {
                        return false;
                    }

                    if (DateTime.Now >= Trg) // If the timer is equal to the end date or over
                    {
                        timerlads.Children.Remove(countdownFinish);
                        DisplayFinishButton(timerlads, time, taskName.Text);
                        return false;
                    }
                    return true; // Return true to continue thread and timer operation
                });

                timerlads.Children.Add(colorTag);
                timerlads.Children.Add(taskName);
                timerlads.Children.Add(editButton);
                timerlads.Children.Add(countdownFinish);
                timers.Children.Add(timerlads); // Add all controls to stack layouts
            }

            else // If over due display finish button
            {
                timerlads.Children.Add(colorTag);
                timerlads.Children.Add(taskName);
                timerlads.Children.Add(editButton);
                timers.Children.Add(timerlads); // Add all controls to stack layouts
                DisplayFinishButton(timerlads, time, taskName.Text);
            }
        }

        /*
         * This method is responsible for displaying the finish button and updating the 
         * current timer file with new timer information and removing the current element from the ListTimer
         * 
         * PARAM
         * timerlads: the stacklayout that is to be updated with the finish button
         * times: the current TimerUpdatecs to be removed from the ListTimer list 
         * 
         * RETURN Nothing
         */
        public void DisplayFinishButton(StackLayout timerlads, TimerUpdatecs times, string task)
        {
            var finishButton = new Button
            {
                Text = "Finish",
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = Color.FromHex("#00CC33"),
                TextColor = Color.White,
                FontSize = 12,
                WidthRequest = 70,
                CornerRadius = 16
            };

            finishButton.Clicked += async (s, a) =>
            {
                await Task.Run(async () =>
                {
                    Animations.CloseStackLayout(timerlads, "Timer", 30, 500);
                });

                timers.Children.Remove(timerlads);
                ListTimer.Remove(times);
                Currentuser.UpdateCurrenttimes(ListTimer);
                items.GiveKey(1);
                await this.DisplayAlert("Congratulations", "You finished a task!\n\nHere's a key", "Receive");
            };

            timerlads.Children.Add(finishButton);
            timers.Children.Add(timerlads);
        }
    }
}