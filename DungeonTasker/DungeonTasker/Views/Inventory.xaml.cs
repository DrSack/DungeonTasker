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
            displayitems();

        }
        /*
         * create a list and run through a foreach and for loop to display all available items within the users inventory
         * PARAM Nothing
         * RETURNS Nothing
         */
        private void displayitems()
        {
            string line;
            string[] Weapons;
            string[] Keys;
            string[] Equipped = new string[1];
            List<string[]> ListItems = new List<string[]>();
            List<string> Types = new List<string>();// Initialize variables and lists

            line = User.CheckForstring(items.Invfile, "Weapons:");
            Weapons = line.Split(',');
            line = User.CheckForstring(items.Invfile, "Keys:");
            Keys = line.Split(',');// Retrieve data
            line = User.CheckForstring(items.Invfile, "Equipped:");
            Equipped[0] = line;// Retrieve data

            ListItems.Add(Weapons);
            ListItems.Add(Keys);
            ListItems.Add(Equipped);

            Types.Add("Weapons");
            Types.Add("Keys");
            Types.Add("Equipped");// Add to lists

            foreach (string[] type in ListItems)
            {//Runs through each array within the List
                var ItemTitle = new Label();
                var LayoutWeapon = new StackLayout();
                ItemTitle.Text = Types[0];
                ItemTitle.FontSize = 30;
                ItemTitle.FontAttributes = FontAttributes.Bold;
                ItemTitle.HorizontalTextAlignment = TextAlignment.Center;
                LayoutWeapon.BackgroundColor = Color.Black;
                LayoutWeapon.Padding = new Thickness(2, 2, 2, 2);
                LayoutWeapon.Spacing = 2;
                ItemsList.Children.Add(ItemTitle);//add type of item to stacklayout

                foreach (string item in type)
                {// for each array run through each individual item and display as a Label
                    var Label = new Label();
                    var LayoutItem = new StackLayout();

                    Label.Text = item;
                    Label.FontSize = 15;
                    Label.FontAttributes = FontAttributes.Bold;
                    
                    Label.HorizontalTextAlignment = TextAlignment.Start;
                    Label.VerticalTextAlignment = TextAlignment.Center;
                    LayoutItem.BackgroundColor = Color.White;

                    if (string.IsNullOrEmpty(item))
                    {
                        break;
                    }

                    else if (Types[0] == "Weapons")
                    {
                        var button = new Button();
                        var Label2 = new Label();
                        Label.Margin = new Thickness(5, 0, 0, 0);
                        LayoutItem.HorizontalOptions = LayoutOptions.FillAndExpand;
                        LayoutItem.Orientation = StackOrientation.Horizontal;
                        Label2.Text = string.Format("Damage: {0} - {1}", WeaponInfo.ObtainWeaponInfo(item, true).ToString(), WeaponInfo.ObtainWeaponInfo(item, false));
                        Label2.HorizontalTextAlignment = TextAlignment.Start;
                        Label2.VerticalTextAlignment = TextAlignment.Center;
                        button.Text = "equip";
                        button.HorizontalOptions = LayoutOptions.EndAndExpand;

                        button.Clicked += (s, a) =>
                        {
                            weapon.SetWeapon(this, item);
                            ItemsList.Children.Clear();
                            displayitems();
                        };

                        LayoutItem.Children.Add(Label);
                        LayoutItem.Children.Add(Label2);
                        LayoutItem.Children.Add(button);
                        LayoutWeapon.Children.Add(LayoutItem);
                        ItemsList.Children.Add(LayoutWeapon);

                    }

                    else if (Types[0] == "Equipped")
                    {
                        var equipped = new StackLayout();
                        var Label2 = new Label();
                        Label.HorizontalTextAlignment = TextAlignment.Center;
                        Label.Text = string.Format("{0}", item);
                        Label2.Text = string.Format("Damage: {0} - {1}", WeaponInfo.ObtainWeaponInfo(item, true).ToString(), WeaponInfo.ObtainWeaponInfo(item, false));
                        equipped.HorizontalOptions = LayoutOptions.CenterAndExpand;
                        equipped.Orientation = StackOrientation.Horizontal;
                        LayoutItem.HorizontalOptions = LayoutOptions.FillAndExpand;

                        equipped.Children.Add(Label);
                        equipped.Children.Add(Label2);
                        LayoutItem.Children.Add(equipped);
                        LayoutWeapon.Children.Add(LayoutItem);
                        ItemsList.Children.Add(LayoutWeapon);
                    }
                    else
                    {
                        Label.HorizontalTextAlignment = TextAlignment.Center;
                        LayoutItem.Children.Add(Label);
                        LayoutWeapon.Children.Add(LayoutItem);
                        ItemsList.Children.Add(LayoutWeapon);
                    }
                    
                    
                }
                Types.RemoveAt(0);
            }
                
           
        }

	}
}