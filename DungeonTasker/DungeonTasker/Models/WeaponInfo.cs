using System;
using System.Collections.Generic;
using System.Text;
using DungeonTasker.Views;
using Xamarin.Forms;

namespace DungeonTasker.Models
{
    public class WeaponInfo
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public string EquippedWeapon { get; set; }
        InventoryItems items;

        public WeaponInfo(InventoryItems items)
        {
            this.items = items;
        }

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
                if (minimum) { return totaldmg; }
                if (weapon.Contains("Sword")) { totaldmg += 7; return totaldmg;}
            }
            return 0;
        }

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
            }
            return 0;
        }

        public void SetWeapon(ContentPage page, string weapon)
        {
            int totaldmg = 0;
            int minimum = 0;
            try
            {
                if(weapon.Contains("Nothing Equipped")) { confirmWeapon(totaldmg+2, minimum+0, weapon); }
                if (EquippedWeapon == weapon) { throw new Exception("Already equipped"); }

                if (weapon.Contains("Wooden"))
                {
                    totaldmg += 1;
                    minimum = totaldmg;
                    if (weapon.Contains("Spoon")) { totaldmg += 2; confirmWeapon(totaldmg, minimum, weapon);}
                    if (weapon.Contains("Bow")) { totaldmg += 3; confirmWeapon(totaldmg, minimum, weapon);}
                }
                if (weapon.Contains("Iron"))
                {
                    totaldmg += 2;
                    minimum = totaldmg;
                    if (weapon.Contains("Dagger")){totaldmg += 3; confirmWeapon(totaldmg, minimum, weapon);}
                    if (weapon.Contains("Bow")) { totaldmg += 4; confirmWeapon(totaldmg, minimum, weapon);}
                    if (weapon.Contains("Sword")) { totaldmg += 5; confirmWeapon(totaldmg, minimum+1, weapon);}
                }
                if (weapon.Contains("Steel"))
                {
                    totaldmg += 4;
                    minimum = totaldmg;
                    if (weapon.Contains("Sword")) { totaldmg += 7; confirmWeapon(totaldmg, minimum + 1, weapon);}
                }
            }
            catch(Exception e)
            {
                page.DisplayAlert("Error", e.Message, "Close");
            }
            
        }

        private void confirmWeapon(int totaldmg, int minimum, string weapon)
        {
            Maximum = totaldmg;
            Minimum = minimum;
            EquippedWeapon = weapon;
            User.Rewrite("Equipped:", EquippedWeapon, items.Invfile);
        }
    }
}
