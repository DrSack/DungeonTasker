using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public class ItemInfoModel
    {
        public string EquippedWeapon { get; set;}
        public List<ItemModel> pots = new List<ItemModel>(); // Store weapon item details
        public InventoryItemsModel items;
        
        /*
         * The ItemInfo Constructor that encapsulates the current item.
         *  @Param items: the inventory items class
         *  Returns Nothing
         */

        public ItemInfoModel(InventoryItemsModel items)
        {
            this.items = items;
            Rebuild();
        }

        /*
        *  This is called outside of the class and is used by the inventory/shop/ 
        *  and game as this method users the providied item.
        *  @Param Nothing
        *  Returns Nothing.
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
        *  An int method that returns either the minimum or maximum number of a given item.
        *  @Param weapon: the item, minimum: true or false to check for minimum or maximum numbers.
        *  Returns The minimum number is bool minimum if true, else return total damage.
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
        *  A string method that returns either the action, or Initials of the item.
        *  @Param weapon: the item, extra: the extra information.
        *  Returns the string to be returned.
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
       *  An int method that returns the gold value of a given item.
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
