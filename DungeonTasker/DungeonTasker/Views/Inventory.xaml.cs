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

            Types.Add("Weapon");
            Types.Add("Keys");
            Types.Add("Equipped");// Add to lists

            foreach (string[] type in ListItems)
            {//Runs through each array within the List
                var ItemTitle = new Label();
                ItemTitle.Text = Types[0];
                ItemTitle.FontSize = 30;
                ItemTitle.FontAttributes = FontAttributes.Bold;
                ItemTitle.HorizontalTextAlignment = TextAlignment.Center;
                ItemsList.Children.Add(ItemTitle);//add type of item to stacklayout

                foreach (string item in type)
                {// for each array run through each individual item and display as a Label
                    var Label = new Label();
                    Label.Text = item;
                    Label.HorizontalTextAlignment = TextAlignment.Center;
                    Label.VerticalTextAlignment = TextAlignment.Center;

                    if (string.IsNullOrEmpty(item))
                    {
                        break;
                    }

                    else if (Types[0] == "Weapon")
                    {
                        var LayoutItem = new StackLayout();
                        var button = new Button();
                        var Label2 = new Label();
                        LayoutItem.HorizontalOptions = LayoutOptions.Center;
                        LayoutItem.Orientation = StackOrientation.Horizontal;
                        Label2.Text = string.Format("Damage: {0}", WeaponInfo.ObtainWeaponInfo(item).ToString());
                        Label2.HorizontalTextAlignment = TextAlignment.Center;
                        Label2.VerticalTextAlignment = TextAlignment.Center;
                        button.Text = "equip";

                        button.Clicked += (s, a) =>
                        {
                            weapon.SetWeapon(this, item);
                            ItemsList.Children.Clear();
                            displayitems();
                        };

                        LayoutItem.Children.Add(Label);
                        LayoutItem.Children.Add(Label2);
                        LayoutItem.Children.Add(button);
                        ItemsList.Children.Add(LayoutItem);

                    }
                    else
                    {
                        ItemsList.Children.Add(Label);
                    }
                    
                    
                }
                Types.RemoveAt(0);
            }
                
           
        }

	}
}