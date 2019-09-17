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
	public partial class Inventory : ContentPage
	{
        InventoryItems items;// Store items information
        WeaponInfo weapon;
        List<Item> weapons = new List<Item>();
        /*
         * Constructor for Inventory
         * PARAM items to be used by the class
         * RETURNS Nothing
         */
		public Inventory(InventoryItems items, WeaponInfo weapon)
		{
            this.items = items;
            this.weapon = weapon;
            InitializeComponent ();
            DisplayWeapons();
            DisplayKey();
            DisplayGold();
            DisplayEquipped();
            DisplayNoWep();
        }

        /*
         * create a list and run through a foreach and for loop to display all available items within the users inventory
         * PARAM Nothing
         * RETURNS Nothing
         */
        private void DisplayWeapons()
        {
            string line;
            string[] cool;
            line = User.CheckForstring(items.Invfile, "Weapons:");
            cool = line.Split(',');

            foreach(string item in cool)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    weapons.Add(new Item(item));
                }
            }

            int i = 0;
            foreach(Item weaponitem in weapons)
            {
                if (!string.IsNullOrEmpty(weaponitem.weapon))
                {
                    bool chck = true;
                    int nice = i;
                    var LayoutItem = new StackLayout();


                    var item = new Label();
                    var damage = new Label();
                    var equip = new Button();
                    var sell = new Button();

                    LayoutItem.HorizontalOptions = LayoutOptions.FillAndExpand;
                    LayoutItem.Orientation = StackOrientation.Horizontal;
                    LayoutItem.BackgroundColor = Color.White;

                    item.Margin = new Thickness(5, 0, 0, 0);
                    item.Text = weaponitem.weapon;
                    item.FontAttributes = FontAttributes.Bold;
                    item.HorizontalTextAlignment = TextAlignment.Start;
                    item.VerticalTextAlignment = TextAlignment.Center;


                    damage.Text = string.Format("Damage: {0} - {1}", WeaponInfo.ObtainWeaponInfo(weaponitem.weapon, true).ToString(), WeaponInfo.ObtainWeaponInfo(weaponitem.weapon, false));
                    damage.FontSize = 10;
                    damage.HorizontalTextAlignment = TextAlignment.Start;
                    damage.VerticalTextAlignment = TextAlignment.Center;

                    equip.Text = "equip";
                    equip.HorizontalOptions = LayoutOptions.EndAndExpand;
                    equip.WidthRequest = 70;
                    equip.HeightRequest = 50;

                    sell.Text = "sell";
                    sell.HorizontalOptions = LayoutOptions.End;
                    sell.WidthRequest = 60;
                    sell.HeightRequest = 50;

                    equip.Clicked += (s, a) =>
                    {
                        weapon.SetWeapon(this, weaponitem.weapon);
                        DisplayEquipped();
                    };

                    sell.Clicked += async (s, a) =>
                    {
                       chck = false;
                       weapons.Remove(weaponitem);
                        
                        string weaponlist = "";
                        foreach (Item weapon in weapons)
                        {
                            if (!String.IsNullOrEmpty(weapon.weapon))
                            {
                                weaponlist += weapon.weapon + ",";
                            }
                        }
                        User.Rewrite("Weapons:", weaponlist, items.Invfile);
                        string equipped = User.CheckForstring(items.Invfile, "Equipped:");
                        if (!User.CheckForstring(items.Invfile, "Weapons:").Contains(equipped))
                        {
                            User.Rewrite("Equipped:", "Not Equipped", items.Invfile);
                            weapon.SetWeapon(this, "Not Equipped");
                        }
                        await Task.Run(async () =>
                        {
                            Animations.CloseStackLayout(LayoutItem, "CloseItem", 60, 250);
                        });

                        ItemsList.Children.Remove(LayoutItem);
                        DisplayNoWep();
                        DisplayEquipped();
                    };

                    LayoutItem.Children.Add(item);
                    LayoutItem.Children.Add(damage);
                    LayoutItem.Children.Add(equip);
                    LayoutItem.Children.Add(sell);

                    ItemsList.Children.Add(LayoutItem);
                    i++;
                }
                
                

            }
        }
        
        private void DisplayNoWep()
        {
            if(ItemsList.Children.Count == 1)
            {
                WepsEmpty.IsEnabled = true;
                WepsEmpty.IsVisible = true;
            }
            else
            {
                WepsEmpty.IsEnabled = false;
                WepsEmpty.IsVisible = false;
            }
            
        }
        private void DisplayKey()
        {
            Keys.Text = User.CheckForstring(items.Invfile, "Keys:");
        }

        private void DisplayGold()
        {
            Gold.Text = User.CheckForstring(items.Invfile, "Gold:");
        }

        private void DisplayEquipped()
        {
            string equippedWep = User.CheckForstring(items.Invfile, "Equipped:");
            if (equippedWep.Contains("Not Equipped"))
            {
                EquippedLabel.Text = User.CheckForstring(items.Invfile, "Equipped:");
                Damage.IsVisible = false;
                Damage.IsEnabled = false;
            }
            else
            {
                EquippedLabel.Text = User.CheckForstring(items.Invfile, "Equipped:");
                Damage.Text = string.Format("Damage: {0} - {1}", WeaponInfo.ObtainWeaponInfo(equippedWep, true).ToString(), WeaponInfo.ObtainWeaponInfo(equippedWep, false));
                Damage.IsVisible = true;
                Damage.IsEnabled = true;
            }
            
        }

	}
}