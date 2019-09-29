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

        /*
         * Constructor for PageOpenModel, this is an encapsulation boolean class
         * 
         */
        public PageOpenModel()
        {
            Tasks = false;
            Inventory = false;
            Dungeon = false;
            AboutUs = false;
            Settings = false;
        }

        /*
         * A void method that is responsible for resetting all values to false.
         * 
         * 
         */
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
