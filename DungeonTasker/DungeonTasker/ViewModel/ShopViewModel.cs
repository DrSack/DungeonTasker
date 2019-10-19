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
        public ItemInfoModel Inv;
        public WeaponInfoModel Weapon;
        public CharacterInfoModel Characters;
        public Command<ItemModel> Remove
        {
            get
            {
                return new Command<ItemModel>(async delete =>
                {
                    if(delete.countCheck()) 
                    {
                        string RealItem = delete.notes;
                        switch (items.CheckItem(delete.notes))
                        {
                            case 0:
                                if(await BuyAsync(items.Inv, 0, RealItem))
                                {
                                    SoldDisplay(delete);
                                    foreach (ItemModel wep in items.BuyWeapons)
                                    {
                                        if (wep.item.Contains(RealItem)) { wep.item = "Sold"; }
                                    }
                                }
                                else
                                {
                                    Volumes[Volumes.IndexOf(delete)].buy = "Buy";
                                }
                                break;

                            case 1:
                                Volumes[Volumes.IndexOf(delete)].buy = "Buy";
                                await BuyAsync(items.Inv, 1, RealItem);
                                break;

                            case 2:
                                if(await BuyAsync(items.Inv, 2, RealItem))
                                {
                                    SoldDisplay(delete);
                                    foreach (ItemModel chars in items.BuyCharacter)
                                    {
                                        if (chars.item.Contains(RealItem)) { chars.item = "Sold"; }
                                    }
                                }
                                else
                                {
                                    Volumes[Volumes.IndexOf(delete)].buy = "Buy";
                                }
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
                            case 2:
                                Volumes[Volumes.IndexOf(delete)].buy = 50.ToString();
                                break;
                        }
                        
                    }
                });
            }
        }

        public ShopViewModel(ShopModel items, ItemInfoModel Inv,WeaponInfoModel Weapon, CharacterInfoModel Characters ,UserModel user, bool test = false)
        {
            Gold = UserModel.CheckForstring(items.Inv.Localfile, "Gold:");
            Keys = UserModel.CheckForstring(items.Inv.Localfile, "Keys:");
            Character = user.Character;
            this.items = items;
            this.Inv = Inv;
            this.Weapon = Weapon;
            this.Characters = Characters;
            if (!test)
            {
                Volumes = new ObservableCollection<ItemModel>();
                CreateLists(items, Volumes);
            }
        }

        public async System.Threading.Tasks.Task<bool> BuyAsync(InventoryItemsModel items, int typecase, string ChoItem, bool test = false)
        {
            foreach (ItemModel item in Characters.Characters)
            {
                if (ChoItem == item.item)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Already have this Character", "Close");
                    return false;
                }
            }
                
            int CurrentGold = CurrentGold = Int32.Parse(UserModel.CheckForstring(items.Localfile, "Gold:"));
            int Price = 0;
            int TotalGold;
            if (typecase == 0)
            {
                Price = WeaponInfoModel.ObtainWeaponValue(ChoItem);
            }
            else if(typecase == 1)
            {
                Price = ItemInfoModel.ObtainItemValue(ChoItem);
            }
            else
            {
                Price = 50;
            }
            TotalGold = CurrentGold - Price;
            if (CurrentGold - Price >= 0)
            {
                if (typecase == 0)
                {
                    UserModel.AddOntoLine("Weapons:", ChoItem + ",", items.Localfile);
                    try
                    {
                        items.Invfile.Object.Weapons += ChoItem + ",";
                    }catch { }
                    Weapon.Rebuild();
                }
                else if(typecase == 1)
                {
                    UserModel.AddOntoLine("Items:", ChoItem + ",", items.Localfile);
                    try
                    {
                        items.Invfile.Object.Items += ChoItem + ",";
                    }catch { }
                    Inv.Rebuild();
                }
                else
                {
                    UserModel.AddOntoLine("Characters:", ChoItem + ",", items.Localfile);
                    try
                    {
                        items.Invfile.Object.Characters += ChoItem + ",";
                    }
                    catch { }
                    Characters.Rebuild();
                }
                UserModel.Rewrite("Gold:", TotalGold.ToString(), items.Localfile); //Rewrite the gold values
                try
                {
                    items.Invfile.Object.Gold = TotalGold.ToString();
                    await items.UpdateInv();
                }
                catch { }
                Gold = UserModel.CheckForstring(items.Localfile, "Gold:");
                if (!test) { await Application.Current.MainPage.DisplayAlert("Success", string.Format("You bought a {0}.", ChoItem), "Close"); }
                return true;
            }
            else
            {
                int remainder = CurrentGold - Price;
                remainder = remainder * -1;
                if (!test) { await Application.Current.MainPage.DisplayAlert("Error", string.Format("You need {0} more gold to purchase this item", remainder.ToString()), "Close"); }
                return false;
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
            Volumes.Add(new ItemModel("Title") { Title = "Characters", frameOn = false, frameVis = false, titleTrue = true, titleVis = true });
            foreach (ItemModel item in items.BuyCharacter)
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
                        texthoz = TextAlignment.Center,
                        item = "",
                        isenabled = true,
                        isvisible = true,
                        frameOn = true,
                        frameVis = true,
                        titleTrue = false,
                        titleVis = false,
                        isvisItem = true,
                        isenbItem = true,
                        hozopnotes = LayoutOptions.CenterAndExpand,
                        buy = "Buy",
                    });
                }  
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
