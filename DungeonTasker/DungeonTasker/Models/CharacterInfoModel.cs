using DungeonTasker.FirebaseData;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public class CharacterInfoModel
    {
        public List<ItemModel> Characters = new List<ItemModel>();
        public InventoryItemsModel items;
        public string Localuser;
        public CharacterInfoModel(InventoryItemsModel items, string user)
        {
            this.items = items;
            this.Localuser = user;
            Rebuild();
        }

        public void Rebuild()
        {
            Characters.Clear();
            string itemsInv;
            string[] split;
            itemsInv = UserModel.CheckForstring(items.Localfile, "Characters:");
            split = itemsInv.Split(',');
            foreach (string item in split)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Characters.Add(new ItemModel(item));
                }
            }
        }

        public void EquipCharacter(string character)
        {
            UserModel.Rewrite("Character:", character, Localuser);
        }
    }
}
