using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    
    public class StatsModel
    {
        public int Health { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
        public string file { get; set; }

        /*
        * Stats Constructor for Boss
        * Param Nothing
        * Returns Nothing
        */
        public StatsModel()
        {

        }

        /*
         * Stats Constructor for player
         * Param
         * @file the file path to the Stats file.
         * Returns Nothing
         */
        public StatsModel(string file)
        {
            this.file = file;
            SetStats();
        }

        /*
         * A void method that sets the class variables to the contents of the stats file.
         * 
         */
        public void SetStats()
        {
           Health = Int32.Parse(UserModel.CheckForstring(file, "HEALTH:"));
           Experience = Int32.Parse(UserModel.CheckForstring(file, "EXP:"));
           Level = Int32.Parse(UserModel.CheckForstring(file, "LEVEL:"));
        }

        /*
         * A bool method that checks if the player is eligible for a level up
         * 
         * Param Nothing
         * Returns 
         * true if equal to or over levelpass
         * false if under levelpass
         */
        public bool StatsCheck()
        {
            int LevelPass = (Level * 21) + 15;
            if (Experience >= LevelPass)
            {
                Level++;
                Health += 20;
                UserModel.Rewrite("HEALTH:", Health.ToString(), file);
                UserModel.Rewrite("LEVEL:", Level.ToString(), file);
                return true;
            }

            else
            {
                return false;
            }
        }

        /*
         * A string method that returns LEVEL UP or the remainder exp.
         * 
         * Param Nothing
         * Returns 
         * LEVEL UP if the difference is less than 0 
         * The amount of experience left to level up
         */
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

        /*
         * A void method that adds on top of the current UserModel experience.
         * 
         * Param 
         * exp Is an integer of how much exp was gained.
         * 
         * Returns Nothing
         */
        public void ExpEnter(int exp)
        {
            Experience += exp;
            UserModel.Rewrite("EXP:", Experience.ToString(), file);
        }
    }
}
