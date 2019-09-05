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
        InventoryItems items;
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
            List<string> Types = new List<string>();

            line = User.CheckForstring(items.Invfile, "Weapons:");
            Weapons = line.Split(',');
            line = User.CheckForstring(items.Invfile, "Keys:");
            Keys = line.Split(',');

            ListItems.Add(Weapons);
            ListItems.Add(Keys);
            Types.Add("Weapon");
            Types.Add("Keys");

            foreach (string[] type in ListItems)
            {
                var ItemTitle = new Label();
                ItemTitle.Text = Types[0];
                ItemTitle.HorizontalTextAlignment = TextAlignment.Center;
                Types.RemoveAt(0);
                ItemsList.Children.Add(ItemTitle);

                foreach (string item in type)
                {
                    var Label = new Label()
                    {
                        Text = item,
                        HorizontalTextAlignment = TextAlignment.Center
                    };
                    ItemsList.Children.Add(Label);
                }
            }
                
           
        }

	}
}