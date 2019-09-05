using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    

    public class InventoryItems
    {
        public string Invfile { get; set; }

        public InventoryItems(string file)
        {
            this.Invfile = file;
        }


    }
}
