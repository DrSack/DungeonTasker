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
    public partial class ShopView : ContentPage
    {
        public ShopView(ShopModel items, ItemInfoModel Inv, WeaponInfoModel Weapon, CharacterInfoModel Characters, UserModel user)
        {
            InitializeComponent();
            BindingContext = new ShopViewModel(items, Inv, Weapon, Characters, user);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var delete = button.BindingContext as ItemModel;
            var vm = BindingContext as ShopViewModel;
            vm.Remove.Execute(delete);
        }
    }
}