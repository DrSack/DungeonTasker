using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    

    public class InventoryItems
    {
        public string Invfile { get; set; }//Initialize variables 

        /*
         * Contructor for Inventory items, encapsulates the inventory file to be used throughout the entire program whenever called.
         * PARAM
         * file: the file of the inventory items.
         * RETURNS Nothing
         */
        public InventoryItems(string file)
        {
            this.Invfile = file;
        }

    }
}
