using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DungeonTasker
{
    public partial class AboutUsView : ContentPage
    {
        String[] SS = new string[] { "This", "Is", "A", "Test" };
        int i = 1;

        public AboutUsView()
        {
            InitializeComponent();
            NewSlideBoi.Value = 1;
            

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                Quotes.Text = SS[i];
            }catch(IndexOutOfRangeException){
                i = 0;
                Quotes.Text = SS[i];
            }
                i++;
        }

        private void NewSlideBoi_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double nice;
            nice = NewSlideBoi.Value * 100;
            i = Convert.ToInt32(nice)/4;

            if (i == 0)
            {
                i = 1;
            }

            FontSize.Text = "FontSize = " + i.ToString();

            Quotes.FontSize = i;
        }
    }
}
