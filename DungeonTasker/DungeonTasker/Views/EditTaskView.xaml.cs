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
    public partial class EditTaskView : ContentPage
    {
        TimerUpdatecs timeStats;
        List<TimerUpdatecs> ListTimers;
        StackLayout timerlads;
        StackLayout outer;
        UserModel user;
        Label label;

        public bool Deleted { get; set; }
        /*
         * Constructor for Edit Task which encapsulates TierUpdatecs, List<TimerUpdatecs>, User and the Label.
         * Param
         * time: Parse the current timer statistics
         * listtime: Parse a list of all timerUpdatecs timers
         * user: the user parsed
         * label: the label id parsed
         * Returns Nothing
         */
        public EditTaskView(TimerUpdatecs time, List<TimerUpdatecs> listtimes, UserModel user, Label label, StackLayout inner, StackLayout outer)
        {
            Deleted = false;
            this.label = label;
            this.user = user;
            this.timeStats = time;
            this.ListTimers = listtimes;
            this.timerlads = inner;
            this.outer = outer;
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
            user.UpdateCurrenttimesAsync(ListTimers);
            label.Text = TaskEditted.Text;
            await Navigation.PopModalAsync();
        }

        private async void PopupDelete(object sender, EventArgs e)
        {
            ListTimers.Remove(timeStats);
            outer.Children.Remove(timerlads);
            user.UpdateCurrenttimesAsync(ListTimers);
            Deleted = true;
            await Navigation.PopModalAsync();
        }
    }
}