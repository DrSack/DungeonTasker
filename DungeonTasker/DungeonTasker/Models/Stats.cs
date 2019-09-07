using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    
    public class Stats
    {
        public int Health { get; set; }
        public string file { get; set; }

        //Empty constructor for boss
        public Stats()
        {

        }
        //For player
        public Stats(string file)
        {
            this.file = file;
            SetHealth();
        }

        public void SetHealth()
        {
           Health = Int32.Parse(User.CheckForstring(file, "HEALTH:"));
        }
    }
}
