using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public class ItemModel
    {
        public string weapon { get; set; }
        /*
         * Encapsulation class for Weapon items
         * Param
         * @weapon is the weapon string parsed
         * Returns Nothing
         */
        public ItemModel(string weapon)
        {
            this.weapon = weapon;
        }
    }
}
