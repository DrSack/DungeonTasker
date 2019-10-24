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
        public string Localfile { get; set; }
        public FirebaseClient Client;
        public string Username;

        /*
         * Contructor for Inventory items, encapsulates the inventory file to be used throughout the entire program whenever called.
         * PARAM
         * file: the file of the inventory items, Client to update the client database, Username of current user.
         * RETURNS Nothing
         */
        public InventoryItemsModel(FirebaseObject<ItemDetails> file, FirebaseClient Client, string Username, string Localfile)
        {
            this.Localfile = Localfile;
            this.Invfile = file;
            this.Client = Client;
            this.Username = Username;
        }

        /*
         * FOR LOCAL USER ONLY
         * PARAM
         * file: the file of the inventory items.
         * RETURNS Nothing
         */
        public InventoryItemsModel(string Localfile)
        {
            this.Localfile = Localfile;
        }


        /*
         * Add keys to the Inventoryfile
         * PARAM
         * key: the number of keys to be added.
         * RETURNS Nothing
         */
        public async Task GiveKeyAsync(int key)
        {
            string keys = UserModel.CheckForstring(Localfile, "Keys:");
            int realkey = Int32.Parse(keys);
            realkey += key;
            UserModel.Rewrite("Keys:", realkey.ToString(), Localfile);
            try
            {
                Invfile.Object.Keys = realkey.ToString();
                await UpdateInv();
            }catch { }
        }

        public async Task UpdateInv()
        {
            try
            {
                await Client
                .Child(string.Format("{0}Inv", Username))
                .Child(Invfile.Key).PutAsync(Invfile.Object);
            }catch{ }
        }

    }
}
