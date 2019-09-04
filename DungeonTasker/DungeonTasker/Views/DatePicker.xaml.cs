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
        DetailsPage page;
        public DatePicker(DetailsPage page)
        {
            this.page = page;
            InitializeComponent();
            _timePicker.Time = DateTime.Now.TimeOfDay;
            
        }


        private async void _button_Toggled(object sender, ToggledEventArgs e)
        {
            DateTime _triggerTime = DateTime.Today + _timePicker.Time;
            TimeSpan _remainderTime = _triggerTime - DateTime.Now;

            if (_triggerTime <= DateTime.Now)
            {
                _triggerTime = DateTime.Now.AddMinutes(1);
            }

            page.Timer(_entry.Text, _triggerTime, _remainderTime);
            await page.Navigation.PopModalAsync();
        }
    }
}