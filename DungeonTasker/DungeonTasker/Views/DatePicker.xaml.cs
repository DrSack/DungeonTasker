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
        DetailsPage page;//Initialize variables

        /*
         * Contructor for DatePicker, initialize all components
         * PARAM
         * page: obtain and store variable page to be used within the class
         * RETURN Nothing
         */
        public DatePicker(DetailsPage page)
        {
            //Initialize the componenets and add the content page.
            this.page = page;
            InitializeComponent();
            _timePicker.Time = DateTime.Now.TimeOfDay;// set the default date for _timerpicked to the current time.
            
        }

        /*
         * When the add button is pressed add the time to the Tasks page.
         * returns NULL
         */
        private async void _button_Toggled(object sender, ToggledEventArgs e)
        {

            DateTime _triggerTime = _datePicker.Date + _timePicker.Time;// Add both the date picked and the time picked to a datetime variable
            TimeSpan _remainderTime = _triggerTime - DateTime.Now;//Take the date of the time and the trigger time to be left with the remainder time
            try// Try clause to detect error
            {
                if (_triggerTime <= DateTime.Now)
                {// When the date selected is lower than the current date throw an exception
                    throw new Exception("Date Invalid");
                }
                else
                {
                    //Add a timer to the Tasks page and close the current page.
                    page.Timer(_entry.Text, _triggerTime, _remainderTime);
                    await page.Navigation.PopModalAsync();
                }
            }
            catch (Exception es)
            {
                //throw exception
               await DisplayAlert("Error", es.Message, "Close");
            }
            

            
        }
    }
}