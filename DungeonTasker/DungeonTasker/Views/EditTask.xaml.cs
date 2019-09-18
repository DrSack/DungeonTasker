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

        /*
         * Constructor for Edit Task which encapsulates TierUpdatecs, List<TimerUpdatecs>, User and the Label.
         * Param
         * time: Parse the current timer statistics
         * listtime: Parse a list of all timerUpdatecs timers
         * user: the user parsed
         * label: the label id parsed
         * Returns Nothing
         */
        public EditTask(TimerUpdatecs time, List<TimerUpdatecs> listtimes, User user, Label label)
        {
            this.label = label;
            this.user = user;
            this.timeStats = time;
            this.ListTimers = listtimes;
            InitializeComponent();
        }


        /*
         * Edit the specific timers task name
         * Param sender, e
         * Returns Nothing
         */
        private async void PopupEditing(object sender, EventArgs e)
        {
            ListTimers[ListTimers.IndexOf(timeStats)].type = TaskEditted.Text;
            user.UpdateCurrenttimes(ListTimers);
            label.Text = TaskEditted.Text;
            await Navigation.PopModalAsync();
        }
    }
}