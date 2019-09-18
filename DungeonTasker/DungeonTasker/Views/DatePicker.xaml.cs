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
    public partial class DatePicker : ContentPage
    {
        Tasks page; // Initialize variables
        bool tut = false;
        /*
         * Contructor for DatePicker, initialize all components
         * 
         * PARAM
         * page: obtain and store variable page to be used within the class
         * RETURN Nothing
         */
        public DatePicker(Tasks page, bool tut)
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
         * returns NULL
         */
        private async void _switch_Clicked(object sender, EventArgs e)
        {
            DateTime _triggerTime = _datePicker.Date + _timePicker.Time; // Add both the date picked and the time picked to a datetime variable
            TimeSpan _remainderTime = _triggerTime - DateTime.Now; // Take the date of the time and the trigger time to be left with the remainder time
            try // Try clause to detect error
            {
                if (_triggerTime <= DateTime.Now.AddMinutes(15))
                { // When the date selected is lower than the current date throw an exception
                    throw new Exception("Please add a time before you can finish the task");
                }

                // If the task name entry is empty
                if (_entry.Text == null)
                {
                    throw new Exception("Please enter the name of the task you need to do");
                }

                else
                {
                    page.Timer(_entry.Text, _triggerTime, _remainderTime);
                }
            }

            catch (Exception es)
            {
                // Throw exception
                await DisplayAlert("Error", es.Message, "Close");
            }
        }

        private async void btn5(object sender, EventArgs e)
        {
            DateTime _triggerTime = DateTime.Now.AddMinutes(5);
            TimeSpan _remainderTime = _triggerTime - DateTime.Now; // Take the date of the time and the trigger time to be left with the remainder time
            page.Timer(_entry.Text, _triggerTime, _remainderTime);
            await page.Navigation.PopModalAsync();
        }

        private async void btn10(object sender, EventArgs e)
        {
            DateTime _triggerTime = DateTime.Now.AddMinutes(10);
            TimeSpan _remainderTime = _triggerTime - DateTime.Now; // Take the date of the time and the trigger time to be left with the remainder time
            page.Timer(_entry.Text, _triggerTime, _remainderTime);
            await page.Navigation.PopModalAsync();
        }

        private async void btn15(object sender, EventArgs e)
        {
            DateTime _triggerTime = DateTime.Now.AddMinutes(15);
            TimeSpan _remainderTime = _triggerTime - DateTime.Now; // Take the date of the time and the trigger time to be left with the remainder time
            page.Timer(_entry.Text, _triggerTime, _remainderTime);
            await page.Navigation.PopModalAsync();
        }

        protected override async void OnAppearing()
        {
            if (tut)
            {
                await ExtraPopups.ShowMessage("This is the DatePicker tool.\nThis is where you can set tasks", "DatePicker", "Close", this, async () =>
                {
                    await this.Navigation.PopModalAsync();
                 });

            }
        }
    }
}