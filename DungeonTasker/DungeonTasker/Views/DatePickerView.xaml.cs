using DungeonTasker.Models;
using DungeonTasker.ViewModel;
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
    public partial class DatePickerView : ContentPage
    {
        TasksView page; // Initialize variables
        bool tut = false;


        public DatePickerView()
        {
            this.tut = true;
            InitializeComponent();
        }
        /*
         * Contructor for DatePicker, initialize all components
         * 
         * PARAM
         * page: obtain and store variable page to be used within the class
         * 
         * RETURN Nothing
         */
        public DatePickerView(TasksView page, bool tut)
        {
            // Initialize the componenets and add the content page.
            this.page = page;
            this.tut = tut;
            InitializeComponent();
            _timePicker.Time = DateTime.Now.AddMinutes(15).TimeOfDay; // Set the default date for _timerpicked to the current time.
        }

        /*
         * When the add button is pressed add the time to the Tasks page.
         * 
         * RETURN NULL
         */
        private async void _switch_Clicked(object sender, EventArgs e)
        {
            DateTime _triggerTime = _datePicker.Date + _timePicker.Time; // Add both the date picked and the time picked to a datetime variable
            TimeSpan _remainderTime = _triggerTime - DateTime.Now; // Take the date of the time and the trigger time to be left with the remainder time

            try // Try clause to detect error
            {
                // If the task name entry is empty
                if (string.IsNullOrEmpty(_entry.Text)) {
                    throw new Exception("Please enter the name of the task you need to do");
                }

                // When the date selected is lower than the current date throw an exception
                else if (_triggerTime <= DateTime.Now.AddMinutes(15)) { 
                    throw new Exception("Please add a time above 15 Minutes\nbefore you can finish the task.");
                }

                else {
                    
                    page.Timer(_entry.Text, _triggerTime, _remainderTime);
                    await this.Navigation.PopModalAsync();
                }
            }

            catch (Exception es)
            {
                await DisplayAlert("Error", es.Message, "Close"); // Throw exception
            }
        }

        /*
         * If 5:00 is pressed display 5:00 on the timer
         */
        private async void btn5(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_entry.Text)) {
                    DateTime _triggerTime = DateTime.Now.AddMinutes(5);
                    TimeSpan _remainderTime = _triggerTime - DateTime.Now; // Take the date of the time and the trigger time to be left with the remainder time
                    page.Timer(_entry.Text, _triggerTime, _remainderTime);
                    await page.Navigation.PopModalAsync();
                }

                else {
                    throw new Exception("Please enter the name of the task you need to do");
                }
            }

            catch (Exception es)
            {   
                await DisplayAlert("Error", es.Message, "Close"); // Throw exception
            }
        }

        /*
         * If 10:00 is pressed display 10:00 on the timer
         */
        private async void btn10(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_entry.Text))
                {
                    DateTime _triggerTime = DateTime.Now.AddMinutes(10);
                    TimeSpan _remainderTime = _triggerTime - DateTime.Now; // Take the date of the time and the trigger time to be left with the remainder time
                    page.Timer(_entry.Text, _triggerTime, _remainderTime);
                    await page.Navigation.PopModalAsync();
                }

                else {
                    throw new Exception("Please enter the name of the task you need to do");
                }
            }

            catch (Exception es)
            {
                await DisplayAlert("Error", es.Message, "Close"); // Throw exception
            }
        }

        /*
         * If 15:00 is pressed display 15:00 on the timer
         */
        private async void btn15(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_entry.Text)) {
                    DateTime _triggerTime = DateTime.Now.AddMinutes(15);
                    TimeSpan _remainderTime = _triggerTime - DateTime.Now; // Take the date of the time and the trigger time to be left with the remainder time
                    page.Timer(_entry.Text, _triggerTime, _remainderTime);
                    await page.Navigation.PopModalAsync();
                }

                else {
                    throw new Exception("Please enter the name of the task you need to do");
                }
            }
            catch (Exception es)
            {             
                await DisplayAlert("Error", es.Message, "Close"); // Throw exception
            }
        }

        protected override async void OnAppearing()
        {
            if (tut) // Run this if the tutorial is active.
            {
                await UserModel.ShowMessage("This is the DatePicker tool.\nThis is where you can set tasks", "DatePicker", "Close", this, async () =>
                {
                    await this.Navigation.PopModalAsync();
                });
            }
        }
    }
}