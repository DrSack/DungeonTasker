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
    public partial class Game : ContentPage
    {
        Dungeon dungeon;
        public Game(Dungeon dungeon)
        {
            this.dungeon = dungeon;
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?(Lose keys)", "Yes", "No");
                if (result) await this.Navigation.PopModalAsync(); // or anything else
            });

            return true;
        }
    }
}