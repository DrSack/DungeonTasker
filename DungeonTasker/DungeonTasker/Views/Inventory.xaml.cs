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
        /*
         * Constructor for Inventory
         * PARAM items to be used by the class
         * RETURNS Nothing
         */
		public Inventory(InventoryItems items)
		{
            this.items = items;
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
            List<string[]> ListItems = new List<string[]>();
            List<string> Types = new List<string>();// Initialize variables and lists

            line = User.CheckForstring(items.Invfile, "Weapons:");
            Weapons = line.Split(',');
            line = User.CheckForstring(items.Invfile, "Keys:");
            Keys = line.Split(',');// Retrieve data

            ListItems.Add(Weapons);
            ListItems.Add(Keys);
            Types.Add("Weapon");
            Types.Add("Keys");// Add to lists

            foreach (string[] type in ListItems)
            {//Runs through each array within the List
                var ItemTitle = new Label();
                ItemTitle.Text = Types[0];
                ItemTitle.FontSize = 30;
                ItemTitle.FontAttributes = FontAttributes.Bold;
                ItemTitle.HorizontalTextAlignment = TextAlignment.Center;
                Types.RemoveAt(0);
                ItemsList.Children.Add(ItemTitle);//add type of item to stacklayout

                foreach (string item in type)
                {// for each array run through each individual item and display as a Label
                    var Label = new Label()
                    {
                        Text = item,
                        HorizontalTextAlignment = TextAlignment.Center
                    };
                    ItemsList.Children.Add(Label);//add item name to stacklayout
                }
            }
                
           
        }

	}
}