using DungeonTasker.FirebaseData;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DungeonTasker.Models
{
    

    public class InventoryItemsModel
    {
        public FirebaseObject<ItemDetails> Invfile { get; set; }//Initialize variables 
        public FirebaseClient Client;
        public string Username;

        /*
         * Contructor for Inventory items, encapsulates the inventory file to be used throughout the entire program whenever called.
         * PARAM
         * file: the file of the inventory items.
         * RETURNS Nothing
         */
        public InventoryItemsModel(FirebaseObject<ItemDetails> file, FirebaseClient Client, string Username)
        {
            this.Invfile = file;
            this.Client = Client;
            this.Username = Username;
        }

        /*
         * Add keys to the Inventoryfile
         * PARAM
         * key: the number of keys to be added.
         * RETURNS Nothing
         */
        public async Task GiveKeyAsync(int key)
        {
            string keys = Invfile.Object.Keys;
            int realkey = Int32.Parse(keys);
            realkey += key; Invfile.Object.Keys = realkey.ToString();

            await UpdateInv();
        }

        public async Task UpdateInv()
        {
            await Client
                .Child(string.Format("{0}Inv", Username))
                .Child(Invfile.Key).PutAsync(Invfile.Object);
        }

    }
}
