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
    public partial class StatsView : ContentPage
    {
        public StatsView(ItemInfoModel Inv, UserModel user, StatsModel stats)
        {
            BindingContext = new StatsViewModel(Inv, user, stats);
            InitializeComponent();
        }
    }
}