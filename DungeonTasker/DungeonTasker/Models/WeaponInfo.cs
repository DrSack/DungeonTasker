using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    class WeaponInfo
    {
        int CurrentDmg { get; set; }
        string EquippedWeapon { get; set; }
        InventoryItems items;

        WeaponInfo(InventoryItems items)
        {
            this.items = items;
        }

        public static int ObtainWeaponInfo(string weapon)
        {
            int totaldmg = 0;
            if (weapon.Contains("Iron"))
            {
                totaldmg += 2;
                if (weapon.Contains("Dagger"))
                {
                    totaldmg += 1;
                    return totaldmg;
                }
            }
            return 0;
        }

        public void SetWeapon()
        {
            int totaldmg = 0;
            EquippedWeapon = User.CheckForstring(items.Invfile, "Equipped:");

            if (EquippedWeapon.Contains("Iron"))
            {
                totaldmg += 2;
                if (EquippedWeapon.Contains("Dagger"))
                {
                    totaldmg += 1;
                    CurrentDmg = totaldmg;
                }
            }
        }
    }
}
