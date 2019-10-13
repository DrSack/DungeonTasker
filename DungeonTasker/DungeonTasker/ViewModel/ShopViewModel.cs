using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    public class ShopViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<ItemModel> Volumes { get; set; }
        private string gold;
        public string Gold
        {
            get
            {
                return gold;
            }

            set
            {
                gold = value;
                OnPropertyChanged(this, "Gold");
            }
        }
        public string Keys { get; set; }
        public string Character { get; set; }

        private ShopModel items;
        private ItemInfoModel Inv;
        private WeaponInfoModel Weapon;
        public Command<ItemModel> Remove
        {
            get
            {
                return new Command<ItemModel>(delete =>
                {
                    if(delete.countCheck()) 
                    {
                        string RealItem = delete.notes;
                        switch (items.CheckItem(delete.notes))
                        {
                            case 0:
                                SoldDisplay(delete);
                                Buy(items.Inv, 0, RealItem);
                                foreach (ItemModel wep in items.BuyWeapons) 
                                {
                                    if (wep.item.Contains(RealItem)) { wep.item = "Sold"; }
                                }

                                break;
                            case 1:
                                Volumes[Volumes.IndexOf(delete)].buy = "Buy";
                                Buy(items.Inv, 1, RealItem);
                                break;
                        }
                    }
                    else
                    {
                        switch (items.CheckItem(delete.notes))
                        {
                            case 0:
                                Volumes[Volumes.IndexOf(delete)].buy = WeaponInfoModel.ObtainWeaponValue(delete.notes).ToString();
                                break;
                            case 1:
                                Volumes[Volumes.IndexOf(delete)].buy = ItemInfoModel.ObtainItemValue(delete.notes).ToString();
                                break;
                        }
                        
                    }
                });
            }
        }
        public ShopViewModel(ShopModel items, ItemInfoModel Inv, WeaponInfoModel Weapon, UserModel user)
        {
            Gold = UserModel.CheckForstring(items.Inv.Invfile, "Gold:");
            Keys = UserModel.CheckForstring(items.Inv.Invfile, "Keys:");
            Character = user.Character;
            this.items = items;
            this.Inv = Inv;
            this.Weapon = Weapon;
            Volumes = new ObservableCollection<ItemModel>();
            CreateLists(items, Volumes);
        }

        private void Buy(InventoryItemsModel items, int typecase, string ChoItem)
        {
            int CurrentGold = CurrentGold = Int32.Parse(UserModel.CheckForstring(items.Invfile, "Gold:"));
            int Price = 0;
            int TotalGold;
            if (typecase == 0)
            {
                Price = WeaponInfoModel.ObtainWeaponValue(ChoItem);
            }
            else
            {
                Price = ItemInfoModel.ObtainItemValue(ChoItem);
            }
            TotalGold = CurrentGold - Price;
            if (CurrentGold - Price >= 0)
            {
                if (typecase == 0)
                {
                    UserModel.AddOntoLine("Weapons:", ChoItem + ",", items.Invfile);
                    Weapon.Rebuild();
                }
                else
                {
                    UserModel.AddOntoLine("Items:", ChoItem + ",", items.Invfile);
                    Inv.Rebuild();
                }
                UserModel.Rewrite("Gold:", TotalGold.ToString(), items.Invfile); //Rewrite the gold values
                Gold = UserModel.CheckForstring(items.Invfile, "Gold:");
                Application.Current.MainPage.DisplayAlert("Success", string.Format("You bought a {0}.", ChoItem), "Close");
            }
            else
            {
                int remainder = CurrentGold - Price;
                remainder = remainder * -1;
                Application.Current.MainPage.DisplayAlert("Error", string.Format("You need {0} more gold to purchase this item", remainder.ToString()), "Close");
            }
        }

        private void CreateLists(ShopModel items, ObservableCollection<ItemModel> Volumes)
        {
            Volumes.Add(new ItemModel("Title") { Title = "Weapons", frameOn = false, frameVis = false, titleTrue = true, titleVis = true });
            foreach (ItemModel item in items.BuyWeapons)
            {
                if (item.item.Contains("Sold"))
                {
                    Volumes.Add(new ItemModel(item.item)
                    {
                        notes = string.Format("{0}", item.item),
                        item = string.Format("{0} - {1} dmg", WeaponInfoModel.ObtainWeaponInfo(item.item, true), WeaponInfoModel.ObtainWeaponInfo(item.item, false)),
                        texthoz = TextAlignment.Center,
                        hozopnotes = LayoutOptions.CenterAndExpand,
                        isenabled = false,
                        isvisible = false,
                        isvisItem = false,
                        isenbItem = false,
                        frameOn = true,
                        frameVis = true,
                        titleTrue = false,
                        titleVis = false,
                        buy = "Buy",
                        
                    });
                }
                else
                {
                    Volumes.Add(new ItemModel(item.item)
                    {
                        notes = string.Format("{0}", item.item),
                        texthoz = TextAlignment.Start,
                        item = string.Format("{0} - {1} dmg", WeaponInfoModel.ObtainWeaponInfo(item.item, true), WeaponInfoModel.ObtainWeaponInfo(item.item, false)),
                        isenabled = true,
                        isvisible = true,
                        frameOn = true,
                        frameVis = true,
                        titleTrue = false,
                        titleVis = false,
                        isvisItem = true,
                        isenbItem = true,
                        hozopnotes = LayoutOptions.Start,
                        buy = "Buy",
                    });
                }
                
            }
            Volumes.Add(new ItemModel("Title") { Title = "Potions", frameOn = false, frameVis = false, titleTrue = true, titleVis = true });
            foreach (ItemModel item in items.BuyItem)
            {
                Volumes.Add(new ItemModel(item.item)
                {
                    notes = string.Format("{0}", item.item),
                    texthoz = TextAlignment.Start,
                    item = string.Format("{0}: {1} - {2} {3}", ItemInfoModel.ObtainItemString(item.item, true),
                    ItemInfoModel.ObtainItemInfo(item.item, true),
                    ItemInfoModel.ObtainItemInfo(item.item, false),
                    ItemInfoModel.ObtainItemString(item.item, false)),
                    isenabled = true,
                    isvisible = true,
                    frameOn = true,
                    frameVis = true,
                    titleTrue = false,
                    titleVis = false,
                    isvisItem = true,
                    isenbItem = true,
                    hozopnotes = LayoutOptions.Start,
                    buy = "Buy",
                });
            }
        }

        private void SoldDisplay(ItemModel delete)
        {
            Volumes[Volumes.IndexOf(delete)].notes = "Sold";
            Volumes[Volumes.IndexOf(delete)].isenabled = false;
            Volumes[Volumes.IndexOf(delete)].isvisible = false;
            Volumes[Volumes.IndexOf(delete)].isvisItem = false;
            Volumes[Volumes.IndexOf(delete)].isenbItem = false;
            Volumes[Volumes.IndexOf(delete)].texthoz = TextAlignment.Center;
            Volumes[Volumes.IndexOf(delete)].hozopnotes = LayoutOptions.CenterAndExpand;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // OnPropertyChanged will raise the PropertyChanged event passing the
        // source property that is being updated.
        private void OnPropertyChanged(object sender, string propertyName)
        {

            if (this.PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
