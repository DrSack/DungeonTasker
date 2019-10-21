using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DungeonTasker.Models;
using System.IO;

namespace DungeonTasker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InventoryView : ContentPage
	{
        InventoryItemsModel items; // Store items information
        WeaponInfoModel weapon;  // The current weapon
        ItemInfoModel ItemInv;
        List<ItemModel> weapons = new List<ItemModel>(); // Store weapon item details
        List<ItemModel> pots = new List<ItemModel>(); // Store weapon item details
        List<ItemModel> characters = new List<ItemModel>();
        UserModel User;

        /*
         * Constructor for Inventory
         * PARAM items to be used by the class
         * RETURNS Nothing
         */
		public InventoryView(InventoryItemsModel items, WeaponInfoModel weapon, UserModel user, ItemInfoModel ItemInv, CharacterInfoModel characters)
		{
            this.items = items;
            this.weapon = weapon;
            this.User = user;
            this.ItemInv = ItemInv;
            this.pots = ItemInv.pots;
            this.weapons = weapon.weapons;
            this.characters = characters.Characters;
            InitializeComponent ();
            DisplayInventory();
            DisplayKey();
            DisplayGold();
            DisplayEquipped();
            DisplayNoWep();
            DisplayNoItem();
            Character.Text = User.Character;
        }

        /*
         * Create an array and store the weapon items in the weapons list 
         * and run through a foreach loop to display all available items within the UserModels inventory.
         * With each item in the foreach loop create a stacklayout with buttons and text details about
         * the item. These buttons allow you to sell or equip that weapon.
         * 
         * PARAM Nothing
         * RETURNS Nothing
         */
        private void DisplayInventory()
        {
            foreach (ItemModel weaponitem in weapons)
            {
                if (!string.IsNullOrEmpty(weaponitem.item))
                {
                    CreateDisplayItem(weaponitem,0, WeaponsList, weapons);
                }
            }

            foreach (ItemModel stash in pots)
            {
                if (!string.IsNullOrEmpty(stash.item))
                {
                    CreateDisplayItem(stash,1, ItemsList, pots);
                }
            }

            foreach(ItemModel Chars in characters)
            {
                if (!string.IsNullOrEmpty(Chars.item))
                {
                    CreateDisplayItem(Chars, 2, CharactersList, characters);
                }
            }
        }

        /*
        * 
        * This Method is responsible for taking the 
        * current selected ItemModel that contains a 
        * weapon and creates and displays details of 
        * that specific item. You can also equip and sell the weapon.
        * 
        * PARAM Nothing
        * RETURNS Nothing
        */
        private void CreateDisplayItem(ItemModel stash, int itemtype, StackLayout layout, List<ItemModel> list)
        {
            int sellcount = 0;
            var frame = new Frame();
            var LayoutItem = new StackLayout();

            var item = new Label();
            var extra = new Label();
            var equip = new Button();
            var sell = new Button(); // Initialize

            LayoutItem.HorizontalOptions = LayoutOptions.FillAndExpand;
            LayoutItem.Orientation = StackOrientation.Horizontal;
            LayoutItem.BackgroundColor = Color.White;

            item.Margin = new Thickness(5, 0, 0, 0);
            item.Text = stash.item;
            item.FontAttributes = FontAttributes.Bold;
            item.HorizontalTextAlignment = TextAlignment.Start;
            item.VerticalTextAlignment = TextAlignment.Center;
            item.TextColor = Color.FromHex("#212121");


            if(itemtype == 0)
            {
                extra.Text = string.Format("Damage: {0} - {1}",
                WeaponInfoModel.ObtainWeaponInfo(stash.item, true).ToString(),
                WeaponInfoModel.ObtainWeaponInfo(stash.item, false));
                extra.TextColor = Color.Red;
                extra.FontSize = 10;
                extra.Margin = new Thickness(15, 0, 0, 0);
                extra.HorizontalTextAlignment = TextAlignment.Start;
                extra.VerticalTextAlignment = TextAlignment.Center;

                equip.Text = "equip";
                equip.HorizontalOptions = LayoutOptions.End;
                equip.WidthRequest = 70;
                equip.HeightRequest = 50;
                equip.BackgroundColor = Color.FromHex("#00CC33");
                equip.TextColor = Color.White;
            }
            else if(itemtype == 1)
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
            else
            {
                item.FontSize = 30;
                item.HorizontalTextAlignment = TextAlignment.Center;
                item.HorizontalOptions = LayoutOptions.CenterAndExpand;
                extra.Text = "";
                equip.Text = "equip";
                equip.HorizontalOptions = LayoutOptions.End;
                equip.WidthRequest = 70;
                equip.HeightRequest = 50;
                equip.BackgroundColor = Color.FromHex("#00CC33");
                equip.TextColor = Color.White;
            }

            sell.Text = "sell";
            sell.HorizontalOptions = LayoutOptions.EndAndExpand;
            sell.FontSize = 10;
            sell.WidthRequest = 50;
            sell.HeightRequest = 50;
            sell.BackgroundColor = Color.White;
            sell.TextColor = Color.Gold;

            equip.Clicked += async (s, a) =>
            {
                if(itemtype == 0)
                {
                    if (await weapon.SetWeaponAsync(this, stash.item)) // If the weapon is not already equipped
                    {
                        await DisplayAlert("Equipped", string.Format("You have equipped: {0}", stash.item), "Close");
                    }
                    DisplayEquipped();
                }
                else if(itemtype == 2)
                {
                    if(Character.Text != stash.item)
                    {
                        UserModel.Rewrite("Character:", stash.item, User.LocalLogin);
                        User.Character = stash.item;
                        Character.Text = stash.item;
                        try
                        {
                            User.UserLogin.Object.Character = stash.item;
                            await User.RewriteDATA();
                        }
                        catch { }
                    }
                    else
                    {
                        await DisplayAlert("Equipped", "Character already equipped", "Close");
                    }
                }
            };

            sell.Clicked += async (s, a) =>
            {
                    if (CharactersList.Children.Count == 1 && itemtype == 2)
                {
                    await DisplayAlert("Error", "Cannot delete your only character", "Close");
                    return;
                }

                if (Character.Text == stash.item && itemtype == 2) //If there is no more of the same character then allow to delete.
                {
                    await DisplayAlert("Error", "Please unequip before selling", "Close");
                    return;
                }

                int Goldvalue = 0;
                if(itemtype == 0)
                    Goldvalue = WeaponInfoModel.ObtainWeaponValue(stash.item);
                if(itemtype == 1)
                    Goldvalue = ItemInfoModel.ObtainItemValue(stash.item);
                if (itemtype == 2)
                    Goldvalue = 50;

                int CurrentGold = Int32.Parse(UserModel.CheckForstring(items.Localfile,"Gold:"));
                int TotalGold = Goldvalue + CurrentGold; // Get your gold and add onto the gold you have recieved.

                sell.Text = Goldvalue.ToString() + " G";
                sellcount++;
                if (sellcount == 2) // If pressed twice
                {
                    list.Remove(stash); // Remove off list
                    string invetory = "";
                    foreach (ItemModel itemInv in list)
                    {
                        if (!String.IsNullOrEmpty(itemInv.item))
                        {
                            invetory += itemInv.item + ","; // Create string for file
                        }
                    }

                    if (itemtype == 0)
                    {
                        UserModel.Rewrite("Weapons:", invetory, items.Localfile);
                        try
                        {
                            items.Invfile.Object.Weapons = invetory;
                            await items.UpdateInv(); // Replace the number of weapons if the remaining weapons set by the weapons list.
                        }catch { }
                        weapon.weapons = this.weapons;
                    }
                    else if (itemtype == 1)
                    {
                        UserModel.Rewrite("Items:", invetory, items.Localfile);
                        try
                        {
                            items.Invfile.Object.Items = invetory;
                            await items.UpdateInv(); // Replace the number of weapons if the remaining weapons set by the weapons list.
                        }catch { }
                        ItemInv.pots = this.pots;
                    }
                    else
                    {
                        UserModel.Rewrite("Characters:", invetory, items.Localfile);
                        try
                        {
                            items.Invfile.Object.Characters = invetory;
                            await items.UpdateInv(); // Replace the number of weapons if the remaining weapons set by the weapons list.
                        }
                        catch { }
                        ItemInv.pots = this.pots;
                    }
                    UserModel.Rewrite("Gold:", TotalGold.ToString(), items.Localfile);
                    try
                    {
                        items.Invfile.Object.Gold = TotalGold.ToString();
                        await items.UpdateInv(); // Replace the number of weapons if the remaining weapons set by the weapons list.
                    }catch { }
                    
                    DisplayGold(); //Display the gold

                    string equipped = UserModel.CheckForstring(items.Localfile, "Equipped:");
                    if (!UserModel.CheckForstring(items.Localfile,"Weapons:").Contains(equipped) && itemtype == 0) //If the Weapons: section is empty replace with "Not Equipped"
                    {
                        UserModel.Rewrite("Equipped:", "Not Equipped", items.Localfile);
                        try
                        {
                            items.Invfile.Object.Equipped = "Not Equipped";
                            await items.UpdateInv(); // Replace the number of weapons if the remaining weapons set by the weapons list.
                        }catch { }
                        weapon.SetWeaponAsync(this, "Not Equipped"); //Rewrite and Set weapon to nothing
                    }
                    DisplayEquipped();
                    await Task.Run(async () =>
                    {
                        Animations.CloseStackLayout(LayoutItem, "CloseItem", 60, 250);
                    }); //Run stacklayout close animation.
                    
                    layout.Children.Remove(frame); //Remove stacklayout
                    DisplayNoWep(); // Check if stacklayout is empty and display "No Weapon"
                    DisplayNoItem();
                    await DisplayAlert("Sold", string.Format("You gained {0} gold", Goldvalue.ToString()), "Close");
                }
            };

            LayoutItem.Children.Add(item);
            LayoutItem.Children.Add(extra);
            LayoutItem.Children.Add(sell);
            if(itemtype == 0 || itemtype == 2)
                LayoutItem.Children.Add(equip);         

            frame.Padding = 3;
            frame.BorderColor = Color.Black;
            frame.Content = LayoutItem;
            layout.Children.Add(frame); // Add onto itemlist stacklayout
        }

        /*
        * Display WepsEmpty, if its the only child left in the stacklayout
        * 
        * PARAM Nothing
`       * RETURNS Nothing
        */
        private void DisplayNoWep()
        {
            if (WeaponsList.Children.Count == 1)
            {
                NoWeps.IsEnabled = true;
                NoWeps.IsVisible = true;
            }
            else
            {
                NoWeps.IsEnabled = false;
                NoWeps.IsVisible = false;
            }  
        }

        /*
        * Display WepsEmpty, if its the only child left in the stacklayout
        * 
        * PARAM Nothing
`       * RETURNS Nothing
        */
        private void DisplayNoItem()
        {
            if (ItemsList.Children.Count == 1)
            {
                NoItems.IsEnabled = true;
                NoItems.IsVisible = true;
            }
            else
            {
                NoItems.IsEnabled = false;
                NoItems.IsVisible = false;
            }
        }

        /*
        * Display the total key value.
        * 
        * PARAM Nothing
        * RETURNS Nothing
        */
        private void DisplayKey()
        {
            Keys.Text = UserModel.CheckForstring(items.Localfile,"Keys:");
            Keys.TextColor = Color.Gold;
        }

        /*
        * Display the total Gold value.
        * 
        * PARAM Nothing
        * RETURNS Nothing
        */
        private void DisplayGold()
        {
            Gold.Text = UserModel.CheckForstring(items.Localfile, "Gold:");
            Gold.TextColor = Color.Gold;
        }

        /*
        * Display the currently equipped item. 
        * If the item is 'Not Equipped' then disable the damage and display "Not Equipped on the Label"
        * 
        * PARAM Nothing
        * RETURNS Nothing
        */
        private void DisplayEquipped()
        {
            string equippedWep = UserModel.CheckForstring(items.Localfile, "Equipped:");
            if (equippedWep.Contains("Not Equipped"))
            {
                EquippedLabel.Text = UserModel.CheckForstring(items.Localfile, "Equipped:");
                EquippedLabel.TextColor = Color.Red;
                Damage.IsVisible = false;
                Damage.IsEnabled = false;
            }

            else {
                EquippedLabel.Text = UserModel.CheckForstring(items.Localfile, "Equipped:");
                EquippedLabel.TextColor = Color.Black;
                Damage.Text = string.Format("Damage: {0} - {1}", WeaponInfoModel.ObtainWeaponInfo(equippedWep, true).ToString(), WeaponInfoModel.ObtainWeaponInfo(equippedWep, false));
                Damage.IsVisible = true;
                Damage.IsEnabled = true;
            }         
        }
	}
}