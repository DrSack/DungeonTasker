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
        List<ItemModel> weapons = new List<ItemModel>(); // Store weapon item details
        UserModel User;

        /*
         * Constructor for Inventory
         * PARAM items to be used by the class
         * RETURNS Nothing
         */
		public InventoryView(InventoryItemsModel items, WeaponInfoModel weapon, UserModel user)
		{
            this.items = items;
            this.weapon = weapon;
            this.User = user;
            InitializeComponent ();
            DisplayWeapons();
            DisplayKey();
            DisplayGold();
            DisplayEquipped();
            DisplayNoWep();
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
        private void DisplayWeapons()
        {
            string line;
            string[] cool;
            line = UserModel.CheckForstring(items.Invfile, "Weapons:");
            cool = line.Split(',');

            foreach(string item in cool)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    weapons.Add(new ItemModel(item));
                }
            }

            foreach(ItemModel weaponitem in weapons)
            {
                if (!string.IsNullOrEmpty(weaponitem.weapon))
                {
                    CreateDisplayWep(weaponitem);
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
        private void CreateDisplayWep(ItemModel weaponitem)
        {
            int sellcount = 0;
            var frame = new Frame();
            var LayoutItem = new StackLayout();

            var item = new Label();
            var damage = new Label();
            var equip = new Button();
            var sell = new Button(); // Initialize

            LayoutItem.HorizontalOptions = LayoutOptions.FillAndExpand;
            LayoutItem.Orientation = StackOrientation.Horizontal;
            LayoutItem.BackgroundColor = Color.White;

            item.Margin = new Thickness(5, 0, 0, 0);
            item.Text = weaponitem.weapon;
            item.FontAttributes = FontAttributes.Bold;
            item.HorizontalTextAlignment = TextAlignment.Start;
            item.VerticalTextAlignment = TextAlignment.Center;
            item.TextColor = Color.FromHex("#212121");

            damage.Text = string.Format("Damage: {0} - {1}",
            WeaponInfoModel.ObtainWeaponInfo(weaponitem.weapon, true).ToString(),
            WeaponInfoModel.ObtainWeaponInfo(weaponitem.weapon, false));
            damage.TextColor = Color.Red;
            damage.HorizontalTextAlignment = TextAlignment.Start;
            damage.VerticalTextAlignment = TextAlignment.Center;

            sell.Text = "sell";
            sell.HorizontalOptions = LayoutOptions.EndAndExpand;
            sell.FontSize = 10;
            sell.WidthRequest = 50;
            sell.HeightRequest = 50;
            sell.BackgroundColor = Color.White;
            sell.TextColor = Color.Gold;

            equip.Text = "equip";
            equip.HorizontalOptions = LayoutOptions.End;
            equip.WidthRequest = 70;
            equip.HeightRequest = 50;
            equip.BackgroundColor = Color.FromHex("#00CC33");
            equip.TextColor = Color.White;

            equip.Clicked += (s, a) =>
            {
                if(weapon.SetWeapon(this, weaponitem.weapon)) // If the weapon is not already equipped
                {
                    DisplayAlert("Equipped", string.Format("You have equipped: {0}", weaponitem.weapon), "Close");
                }
                DisplayEquipped();
            };

            sell.Clicked += async (s, a) =>
            {
                int Goldvalue = WeaponInfoModel.ObtainWeaponValue(weaponitem.weapon);
                int CurrentGold = Int32.Parse(UserModel.CheckForstring(items.Invfile, "Gold:"));
                int TotalGold = Goldvalue + CurrentGold; // Get your gold and add onto the gold you have recieved.

                sell.Text = Goldvalue.ToString() + " G";
                sellcount++;
                if (sellcount == 2) // If pressed twice
                {
                    weapons.Remove(weaponitem); // Remove off list

                    string weaponlist = "";
                    foreach (ItemModel weapon in weapons)
                    {
                        if (!String.IsNullOrEmpty(weapon.weapon))
                        {
                            weaponlist += weapon.weapon + ","; // Create string for file
                        }
                    }
                    UserModel.Rewrite("Weapons:", weaponlist, items.Invfile); // Replace the number of weapons if the remaining weapons set by the weapons list.
                    UserModel.Rewrite("Gold:", TotalGold.ToString(), items.Invfile); //Rewrite the gold values
                    DisplayGold(); //Display the gold

                    string equipped = UserModel.CheckForstring(items.Invfile, "Equipped:");
                    if (!UserModel.CheckForstring(items.Invfile, "Weapons:").Contains(equipped)) //If the Weapons: section is empty replace with "Not Equipped"
                    {
                        UserModel.Rewrite("Equipped:", "Not Equipped", items.Invfile);
                        weapon.SetWeapon(this, "Not Equipped"); //Rewrite and Set weapon to nothing
                    }
                    DisplayEquipped();
                    await Task.Run(async () =>
                    {
                        Animations.CloseStackLayout(LayoutItem, "CloseItem", 60, 250);
                    }); //Run stacklayout close animation.
                    
                    ItemsList.Children.Remove(frame); //Remove stacklayout
                    DisplayNoWep(); // Check if stacklayout is empty and display "No Weapon"
                    await DisplayAlert("Sold", string.Format("You gained {0} gold", Goldvalue.ToString()), "Close");
                }
            };

            LayoutItem.Children.Add(item);
            LayoutItem.Children.Add(damage);
            LayoutItem.Children.Add(sell);
            LayoutItem.Children.Add(equip);         

            frame.Padding = 3;
            frame.BorderColor = Color.Black;
            frame.Content = LayoutItem;
            ItemsList.Children.Add(frame); // Add onto itemlist stacklayout
        }

        /*
        * Display WepsEmpty, if its the only child left in the stacklayout
        * 
        * PARAM Nothing
`       * RETURNS Nothing
        */
        private void DisplayNoWep()
        {
            if (ItemsList.Children.Count == 1)
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
        * Display the total key value.
        * 
        * PARAM Nothing
        * RETURNS Nothing
        */
        private void DisplayKey()
        {
            Keys.Text = UserModel.CheckForstring(items.Invfile, "Keys:");
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
            Gold.Text = UserModel.CheckForstring(items.Invfile, "Gold:");
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
            string equippedWep = UserModel.CheckForstring(items.Invfile, "Equipped:");
            if (equippedWep.Contains("Not Equipped"))
            {
                EquippedLabel.Text = UserModel.CheckForstring(items.Invfile, "Equipped:");
                EquippedLabel.TextColor = Color.Red;
                Damage.IsVisible = false;
                Damage.IsEnabled = false;
            }

            else {
                EquippedLabel.Text = UserModel.CheckForstring(items.Invfile, "Equipped:");
                Damage.Text = string.Format("Damage: {0} - {1}", WeaponInfoModel.ObtainWeaponInfo(equippedWep, true).ToString(), WeaponInfoModel.ObtainWeaponInfo(equippedWep, false));
                Damage.IsVisible = true;
                Damage.IsEnabled = true;
            }         
        }
	}
}