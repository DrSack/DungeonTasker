using DungeonTasker.FirebaseData;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DungeonTasker.Models
{
    
    public class StatsModel
    {
        public FirebaseObject<StatDetails> file { get; set; }//Initialize variables 
        public FirebaseClient Client;
        public string Username;
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }

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
        public StatsModel(FirebaseObject<StatDetails> file, FirebaseClient Client, string Username)
        {
            this.file = file;
            this.Client = Client;
            this.Username = Username;
            SetStats();
        }

        /*
         * A void method that sets the class variables to the contents of the stats file.
         * 
         */
        public void SetStats()
        {
           Health = Int32.Parse(file.Object.HEALTH);
           Mana = Int32.Parse(file.Object.MANA);
           Experience = Int32.Parse(file.Object.EXP);
           Level = Int32.Parse(file.Object.LEVEL);
        }

        /*
         * A bool method that checks if the player is eligible for a level up
         * 
         * Param Nothing
         * Returns 
         * true if equal to or over levelpass
         * false if under levelpass
         */
        public async Task<bool> StatsCheckAsync()
        {
            int LevelPass = (Level * 21) + 15;
            if (Experience >= LevelPass)
            {
                Level++; file.Object.LEVEL = Level.ToString();
                Health += 20; file.Object.HEALTH = Health.ToString();
                Mana += 10; file.Object.MANA = Mana.ToString();

                await Client
                    .Child(string.Format("{0}Stats", Username))
                    .Child(file.Key).PutAsync(file.Object);
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
            if(left <= 0)
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
        public async Task ExpEnterAsync(int exp)
        {
            Experience += exp; file.Object.EXP = Experience.ToString();
            await Client
                    .Child(string.Format("{0}Stats", Username))
                    .Child(file.Key).PutAsync(file.Object);
        }
    }
}
