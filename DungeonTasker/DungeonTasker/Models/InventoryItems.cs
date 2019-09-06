﻿using System;
using System.Collections.Generic;
using System.IO;
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

        /*
         * Add keys to the Inventoryfile
         * PARAM
         * key: the number of keys to be added.
         * RETURNS Nothing
         */
        public void GiveKey(int key)
        {
            string keys = User.CheckForstring(Invfile, "Keys:");
            keys = keys.Replace(",", "");
            int realkey = Int32.Parse(keys);
            realkey += key;
            User.Rewrite("Keys:", realkey.ToString(), Invfile);
        }

    }
}
