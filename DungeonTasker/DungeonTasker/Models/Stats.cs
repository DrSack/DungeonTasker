using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    
    public class Stats
    {
        public int Health { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public string file { get; set; }

        //Empty constructor for boss
        public Stats()
        {

        }
        //For player
        public Stats(string file)
        {
            this.file = file;
            SetStats();
        }

        public void SetStats()
        {
           Health = Int32.Parse(User.CheckForstring(file, "HEALTH:"));
           Experience = Int32.Parse(User.CheckForstring(file, "EXP:"));
           Level = Int32.Parse(User.CheckForstring(file, "LEVEL:"));
        }

        public bool StatsCheck()
        {
            int LevelPass = (Level * 21) + 15;
            if (Experience >= LevelPass)
            {
                Level++;
                Health += 20;
                User.Rewrite("HEALTH:", Health.ToString(), file);
                User.Rewrite("LEVEL:", Level.ToString(), file);
                return true;
            }

            else
            {
                return false;
            }
        }

        public string ExpLeft()
        {
            int LevelPass = (Level * 21) + 15;
            int left = LevelPass - Experience;
            if(left < 0)
            {
                return "LEVEL UP";
            }
            return left.ToString();
        }

        public void ExpEnter(int exp)
        {
            Experience += exp;
            User.Rewrite("EXP:", Experience.ToString(), file);
        }
    }
}
