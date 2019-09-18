using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public class Item
    {
        public string weapon { get; set; }
        /*
         * Encapsulation class for Weapon items
         * Param
         * @weapon is the weapon string parsed
         * Returns Nothing
         */
        public Item(string weapon)
        {
            this.weapon = weapon;
        }
    }
}
