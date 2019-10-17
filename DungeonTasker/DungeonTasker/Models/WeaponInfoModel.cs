using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DungeonTasker.Views;
using Firebase.Database.Query;
using Xamarin.Forms;

namespace DungeonTasker.Models
{
    public class WeaponInfoModel
    {
        public int Minimumdmg { get; set; }
        public int Maximumdmg { get; set; }
        public string EquippedWeapon { get; set; }
        public List<ItemModel> weapons = new List<ItemModel>();

        InventoryItemsModel items;


        /*
         * The WeaponInfo Constructor that encapsulates the current weapon.
         *  @Param items: the inventory items class
         *  Returns Nothing
         */

        public WeaponInfoModel(InventoryItemsModel items)
        {
            this.items = items;
            Rebuild();
        }


        public void Rebuild()
        {
            weapons.Clear();
            string weaponsInv;
            string[] split;
            weaponsInv = items.Invfile.Object.Weapons;
            split = weaponsInv.Split(',');
            foreach (string item in split)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    weapons.Add(new ItemModel(item));
                }
            }
        }



        /*
        *  An int method that returns either the minimum or maximum damage of a given weapon.
        *  @Param weapon: the weapon item, minimum: true or false to check for minimum or maximum damage.
        *  Returns The minimum damage is bool minimum is true, else return total damage.
        */

        public static int ObtainWeaponInfo(string weapon, bool minimum)
        {
            int totaldmg = 0;
            if (weapon.Contains("Wooden"))
            {
                totaldmg += 1;
                if (minimum) { return totaldmg;}
                if (weapon.Contains("Spoon")) { totaldmg += 2; return totaldmg; }
                if (weapon.Contains("Bow")) { totaldmg += 3; return totaldmg; }
            }
            if (weapon.Contains("Iron"))
            {
                totaldmg += 2;
                if(minimum && weapon.Contains("Sword")) { totaldmg++; }
                if (minimum) {return totaldmg;}
                if (weapon.Contains("Dagger")) { totaldmg += 3;return totaldmg;}
                if (weapon.Contains("Bow")) { totaldmg += 4; return totaldmg;}
                if (weapon.Contains("Sword")) { totaldmg += 4; return totaldmg;}
            }
            if (weapon.Contains("Steel"))
            {
                totaldmg += 4;
                if (minimum && weapon.Contains("Sword")) { totaldmg++; }
                if (minimum && weapon.Contains("Axe")) { totaldmg+=2; }
                if (minimum) { return totaldmg; }
                if (weapon.Contains("Sword")) { totaldmg += 7; return totaldmg;}
                if (weapon.Contains("Axe")) { totaldmg += 8; return totaldmg; }
            }
            if (weapon.Contains("Diamond"))
            {
                totaldmg += 6;
                if (minimum && weapon.Contains("Bow")) { totaldmg += 4; }
                if (minimum) { return totaldmg; }
                if (weapon.Contains("Bow")) { totaldmg += 9; return totaldmg; }
            }
            return 0;
        }

        /*
       *  An int method that returns the gold value of a given weapon.
       *  @Param weapon: the weapon item
       *  Returns The gold value.
       */
        public static int ObtainWeaponValue(string weapon)
        {
            if (weapon.Contains("Wooden"))
            {
                if (weapon.Contains("Spoon")) {  return 1; }
                if (weapon.Contains("Bow")) { return 2; }
            }
            if (weapon.Contains("Iron"))
            {
                if (weapon.Contains("Dagger")) { return 4; }
                if (weapon.Contains("Bow")) {  return 5; }
                if (weapon.Contains("Sword")) { return 7; }
            }
            if (weapon.Contains("Steel"))
            {
                if (weapon.Contains("Sword")) { return 11; }
                if (weapon.Contains("Axe")) { return 14; }
            }
            if (weapon.Contains("Diamond"))
            {
                if (weapon.Contains("Bow")) { return 19; }
            }
            return 0;
        }


     /*
      *  A void method that sets the weapon and sets the class minimum and maximum damage.
      *  @Param page: parse the content page to display an alert if an error has occured, weapon: the weapon item parse.
      *  Returns Nothing.
      */
      
        public async Task<bool> SetWeaponAsync(ContentPage page, string weapon)
        {
            int totaldmg = 0;
            int minimum = 0;
            try
            {
                if(weapon.Contains("Not Equipped")) { await confirmWeaponAsync(totaldmg+2, minimum+0, weapon); return true;}
                if (EquippedWeapon == weapon) { throw new Exception("Already equipped"); }

                if (weapon.Contains("Wooden"))
                {
                    totaldmg += 1;
                    minimum = totaldmg;
                    if (weapon.Contains("Spoon")) { totaldmg += 2; await confirmWeaponAsync(totaldmg, minimum, weapon);}
                    if (weapon.Contains("Bow")) { totaldmg += 3; await confirmWeaponAsync(totaldmg, minimum, weapon);}
                }
                if (weapon.Contains("Iron"))
                {
                    totaldmg += 2;
                    minimum = totaldmg;
                    if (weapon.Contains("Dagger")){totaldmg += 3; await confirmWeaponAsync(totaldmg, minimum, weapon);}
                    if (weapon.Contains("Bow")) { totaldmg += 4; await confirmWeaponAsync(totaldmg, minimum, weapon);}
                    if (weapon.Contains("Sword")) { totaldmg += 5; await confirmWeaponAsync(totaldmg, minimum+1, weapon);}
                }
                if (weapon.Contains("Steel"))
                {
                    totaldmg += 4;
                    minimum = totaldmg;
                    if (weapon.Contains("Sword")) { totaldmg += 7; await confirmWeaponAsync(totaldmg, minimum + 1, weapon);}
                    if (weapon.Contains("Axe")) { totaldmg += 8; await confirmWeaponAsync(totaldmg, minimum + 2, weapon); }
                }
                if (weapon.Contains("Diamond"))
                {
                    totaldmg += 6;
                    minimum = totaldmg;
                    if (weapon.Contains("Bow")) { totaldmg += 9; await confirmWeaponAsync(totaldmg, minimum+4, weapon); }
                }
                return true;
            }
            catch(Exception e)
            {
                page.DisplayAlert("Error", e.Message, "Close");
                return false;
            }
            
        }

        private async Task confirmWeaponAsync(int totaldmg, int minimum, string weapon)
        {
            Maximumdmg = totaldmg;
            Minimumdmg = minimum;
            EquippedWeapon = weapon;

            items.Invfile.Object.Equipped = weapon;

            await items.Client
                .Child(string.Format("{0}Inv", items.Username))
                .Child(items.Invfile.Key).PutAsync(items.Invfile.Object);
        }
    }
}
