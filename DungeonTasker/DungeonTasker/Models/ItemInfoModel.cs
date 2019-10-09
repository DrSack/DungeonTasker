using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public class ItemInfoModel
    {
        public string EquippedWeapon { get; set;}
        public List<ItemModel> pots = new List<ItemModel>(); // Store weapon item details
        InventoryItemsModel items;
        
        /*
         * The WeaponInfo Constructor that encapsulates the current weapon.
         *  @Param items: the inventory items class
         *  Returns Nothing
         */

        public ItemInfoModel(InventoryItemsModel items)
        {
            this.items = items;
            Rebuild();
        }

        /*
        *  An int method that returns either the minimum or maximum damage of a given weapon.
        *  @Param weapon: the weapon item, minimum: true or false to check for minimum or maximum damage.
        *  Returns The minimum damage is bool minimum is true, else return total damage.
        */

        public void Rebuild()
        {
            pots.Clear();
            string itemsInv;
            string[] split;
            itemsInv = UserModel.CheckForstring(items.Invfile, "Items:");

            split = itemsInv.Split(',');
            foreach (string item in split)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    pots.Add(new ItemModel(item));
                }
            }
        }

        /*
        *  An int method that returns either the minimum or maximum damage of a given weapon.
        *  @Param weapon: the weapon item, minimum: true or false to check for minimum or maximum damage.
        *  Returns The minimum damage is bool minimum is true, else return total damage.
        */

        public static int ObtainItemInfo(string invitem, bool minimum)
        {
            int total = 0;
            if (invitem.Contains("HealthPotion"))
            {
                total += 10;
                if (minimum) { return total; }
                else { total += 30; return total; }
            }
            if (invitem.Contains("MagicPotion"))
            {
                total += 20;
                if (minimum) { return total; }
                else { total += 40; return total; }
            }
            return 0;
        }

        /*
       *  An int method that returns either the minimum or maximum damage of a given weapon.
       *  @Param weapon: the weapon item, minimum: true or false to check for minimum or maximum damage.
       *  Returns The minimum damage is bool minimum is true, else return total damage.
       */

        public static string ObtainItemString(string invitem, bool extra)
        {
            if (extra)
            {
                if (invitem.Contains("HealthPotion")) { return "Heal"; }
                if (invitem.Contains("MagicPotion")) { return "Restore"; }
            }
            else
            {
                if (invitem.Contains("HealthPotion")) { return "HP"; }
                if (invitem.Contains("MagicPotion")) { return "MP"; }
            }
            return null;
        }

        /*
       *  An int method that returns the gold value of a given weapon.
       *  @Param weapon: the weapon item
       *  Returns The gold value.
       */
        public static int ObtainItemValue(string invitem)
        {
            if (invitem.Contains("HealthPotion")){return 15;}
            if (invitem.Contains("MagicPotion")){return 20;}
            return 0;
        }
    }
}
