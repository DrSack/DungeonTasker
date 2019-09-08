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
            if (weapon.Contains("Iron"))
            {
                totaldmg += 2;
                if (minimum) { return totaldmg; }
                if (weapon.Contains("Dagger")) { totaldmg += 3;return totaldmg;}
                if (weapon.Contains("Bow")) { totaldmg += 4; return totaldmg; }
            }
            return 0;
        }

        public void SetWeapon(ContentPage page, string weapon)
        {
            int totaldmg = 0;
            int minimum = 0;
            try
            {
                if (EquippedWeapon == weapon) { throw new Exception("Already equipped"); }

                if (weapon.Contains("Iron"))
                {
                    totaldmg += 2;
                    minimum = totaldmg;
                    if (weapon.Contains("Dagger")){totaldmg += 3;Maximum = totaldmg; Minimum = minimum; EquippedWeapon = weapon;User.Rewrite("Equipped:", EquippedWeapon, items.Invfile);}
                    if (weapon.Contains("Bow")) { totaldmg += 4; Maximum = totaldmg; Minimum = minimum; EquippedWeapon = weapon; User.Rewrite("Equipped:", EquippedWeapon, items.Invfile);}
                }
            }
            catch(Exception e)
            {
                page.DisplayAlert("Error", e.Message, "Close");
            }
            
        }
    }
}
