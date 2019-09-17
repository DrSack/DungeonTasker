using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public class Item
    {
        public string weapon { get; set; }
        public Item(string weapon)
        {
            this.weapon = weapon;
        }
    }
}
