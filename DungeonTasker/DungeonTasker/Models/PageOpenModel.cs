using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public class PageOpenModel
    {
        public bool Tasks { get; set; }
        public bool Inventory { get; set; }
        public bool Dungeon { get; set; }
        public bool AboutUs { get; set; }
        public bool Settings { get; set; } 

        public PageOpenModel()
        {
            Tasks = false;
            Inventory = false;
            Dungeon = false;
            AboutUs = false;
            Settings = false;
        }

        public void ResetAll()
        {
            Tasks = false;
            Inventory = false;
            Dungeon = false;
            AboutUs = false;
            Settings = false;
        }
}
}
