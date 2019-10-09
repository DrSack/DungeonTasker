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
    public partial class ShopView : ContentPage
    {
        InventoryItemsModel items; // Store items information
        ItemInfoModel ItemInv;
        UserModel user;
        List<ItemModel> BuyWeapons = new List<ItemModel>(); // Store weapon item details
        List<ItemModel> BuyItem = new List<ItemModel>(); // Store weapon item details
        public ShopView(InventoryItemsModel items, UserModel user, ItemInfoModel ItemInv)
        {
            this.items = items;
            this.user = user;
            this.ItemInv = ItemInv;
            InitializeComponent();
            CreateWeaponPool();
            CreateItemPool();
            DisplayWeapons();
            InitializeValues();
        }

        private void InitializeValues()
        {
            Character.Text = user.Character;
            Keys.Text = UserModel.CheckForstring(items.Invfile, "Keys:");
            Keys.TextColor = Color.Gold;
            Gold.Text = UserModel.CheckForstring(items.Invfile, "Gold:");
            Gold.TextColor = Color.Gold;
        }

        private void CreateWeaponPool()
        {
            Random rnd = new Random();
            List<int> added = new List<int>();
            string[] list = { "SteelSword", "SteelAxe", "DiamondBow" };
            int weapon1 = rnd.Next(0,3);
            int weapon2 = rnd.Next(0, 3);
            while (weapon2 == weapon1)
                weapon2 = rnd.Next(0, 3);

            BuyWeapons.Add(new ItemModel(list[weapon1]));
            BuyWeapons.Add(new ItemModel(list[weapon2]));
        }

        private void CreateItemPool()
        {
            BuyItem.Add(new ItemModel("HealthPotion"));
            BuyItem.Add(new ItemModel("MagicPotion"));
        }

        private void DisplayWeapons()
        {

            foreach (ItemModel weaponitem in BuyWeapons)
            {
                if (!string.IsNullOrEmpty(weaponitem.item))
                {
                    CreateDisplayWep(weaponitem,true, WeaponList);
                }
            }
            foreach (ItemModel stashitem in BuyItem)
            {
                if (!string.IsNullOrEmpty(stashitem.item))
                {
                    CreateDisplayWep(stashitem, false, ItemsList);
                }
            }
        }

        private void CreateDisplayWep(ItemModel stash, bool isWep, StackLayout layout)
        {
            int count = 0;
            var frame = new Frame();
            var LayoutItem = new StackLayout();

            var item = new Label();
            var extra = new Label();
            var buy = new Button();

            LayoutItem.HorizontalOptions = LayoutOptions.FillAndExpand;
            LayoutItem.Orientation = StackOrientation.Horizontal;
            LayoutItem.BackgroundColor = Color.White;

            item.Margin = new Thickness(5, 0, 0, 0);
            item.Text = stash.item;
            item.FontAttributes = FontAttributes.Bold;
            item.HorizontalTextAlignment = TextAlignment.Start;
            item.VerticalTextAlignment = TextAlignment.Center;
            item.TextColor = Color.FromHex("#212121");

            if(isWep)
            {
                extra.Text = string.Format("Damage: {0} - {1}",
                WeaponInfoModel.ObtainWeaponInfo(stash.item, true).ToString(),
                WeaponInfoModel.ObtainWeaponInfo(stash.item, false));
                extra.TextColor = Color.Red;
                extra.HorizontalTextAlignment = TextAlignment.Start;
                extra.VerticalTextAlignment = TextAlignment.Center;
            }
            else
            {
                extra.Text = string.Format("{0}: {1} - {2} {3}",
                ItemInfoModel.ObtainItemString(stash.item, true),
                ItemInfoModel.ObtainItemInfo(stash.item, true),
                ItemInfoModel.ObtainItemInfo(stash.item, false),
                ItemInfoModel.ObtainItemString(stash.item, false));
                extra.TextColor = Color.Red;
                extra.HorizontalTextAlignment = TextAlignment.Start;
                extra.VerticalTextAlignment = TextAlignment.Center;
            }
            
            buy.Text = "Buy";
            buy.HorizontalOptions = LayoutOptions.EndAndExpand;
            buy.WidthRequest = 70;
            buy.HeightRequest = 50;
            buy.BackgroundColor = Color.FromHex("#00CC33");
            buy.TextColor = Color.White;

            buy.Clicked += (s, a) =>
            {
                if (isWep)
                    buy.Text = WeaponInfoModel.ObtainWeaponValue(stash.item).ToString();
                if (!isWep)
                    buy.Text = ItemInfoModel.ObtainItemValue(stash.item).ToString();

                if(count == 1)
                {
                    int CurrentGold = CurrentGold = Int32.Parse(UserModel.CheckForstring(items.Invfile, "Gold:"));
                    int Price = 0;
                    int TotalGold;
                    if (isWep)
                    {
                        Price = WeaponInfoModel.ObtainWeaponValue(stash.item);
                    }
                    if (!isWep)
                    {
                        Price = ItemInfoModel.ObtainItemValue(stash.item);
                    }
                    TotalGold = CurrentGold - Price;
                    if (CurrentGold - Price >= 0)
                        {
                            if (isWep)
                            {
                                LayoutItem.Children.Remove(extra);
                                LayoutItem.Children.Remove(buy);
                                item.Text = "Sold";
                                item.HorizontalTextAlignment = TextAlignment.Center;
                                item.HorizontalOptions = LayoutOptions.CenterAndExpand;
                                UserModel.AddOntoLine("Weapons:", stash.item + ",", items.Invfile);
                            }
                            else
                            {
                                UserModel.AddOntoLine("Items:", stash.item + ",", items.Invfile);
                                ItemInv.Rebuild();
                            }
                        UserModel.Rewrite("Gold:", TotalGold.ToString(), items.Invfile); //Rewrite the gold values
                        InitializeValues();
                        DisplayAlert("Bought", string.Format("You have bought a {0}", stash.item), "Close");
                        }
                        else
                        {
                            int remainder = CurrentGold - Price;
                            remainder = remainder * -1;
                            DisplayAlert("Error", string.Format("You need {0} more gold to purchase this item",remainder.ToString() ), "Close");
                            buy.Text = "Buy";
                        }
                             
                    count = 0; return;
                }
                
                count++;
            };

            LayoutItem.Children.Add(item);
            LayoutItem.Children.Add(extra);
            LayoutItem.Children.Add(buy);

            frame.Padding = 3;
            frame.BorderColor = Color.Black;
            frame.Content = LayoutItem;
            layout.Children.Add(frame);
            
        }
    }
}