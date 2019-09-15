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
    public partial class EditTask : ContentPage
    {
        TimerUpdatecs timeStats;
        List<TimerUpdatecs> ListTimers;
        User user;
        Label label;
        public EditTask(TimerUpdatecs time, List<TimerUpdatecs> listtimes, User user, Label label)
        {
            this.label = label;
            this.user = user;
            this.timeStats = time;
            this.ListTimers = listtimes;
            InitializeComponent();
        }

        private async void PopupEditing(object sender, EventArgs e)
        {
            ListTimers[ListTimers.IndexOf(timeStats)].type = TaskEditted.Text;
            user.UpdateCurrenttimes(ListTimers);
            label.Text = TaskEditted.Text;
            await Navigation.PopModalAsync();
        }
    }
}