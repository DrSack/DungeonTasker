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
        public string Localfile { get; set; }
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
        * Stats Constructor for local only users
        * Param Nothing
        * Returns Nothing
        */

        public StatsModel(string Localfile)
        {
            this.Localfile = Localfile;
            SetStats();
        }
        /*
         * Stats Constructor for player
         * Param
         * @file the file path to the Stats file.
         * Returns Nothing
         */
        public StatsModel(FirebaseObject<StatDetails> file, FirebaseClient Client, string Username, string Localfile)
        {
            this.file = file;
            this.Client = Client;
            this.Username = Username;
            this.Localfile = Localfile;
            SetStats();
        }

        /*
         * A void method that sets the class variables to the contents of the stats file.
         * 
         */
        public void SetStats()
        {
           Health = Int32.Parse(UserModel.CheckForstring(Localfile, "HEALTH:"));
           Mana = Int32.Parse(UserModel.CheckForstring(Localfile, "MANA:"));
           Experience = Int32.Parse(UserModel.CheckForstring(Localfile, "EXP:"));
           Level = Int32.Parse(UserModel.CheckForstring(Localfile, "LEVEL:"));
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
                Level++; 
                UserModel.Rewrite("LEVEL:", Level.ToString(), Localfile);
                Health += 20;
                UserModel.Rewrite("HEALTH:", Health.ToString(), Localfile);
                Mana += 10; 
                UserModel.Rewrite("MANA:", Mana.ToString(), Localfile);

                try
                {
                    file.Object.LEVEL = Level.ToString();
                    file.Object.HEALTH = Health.ToString();
                    file.Object.MANA = Mana.ToString();
                    await Client
                    .Child(string.Format("{0}Stats", Username))
                    .Child(file.Key).PutAsync(file.Object);
                }
                catch { }
                
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
            Experience += exp; 
            UserModel.Rewrite("EXP:", Experience.ToString(), Localfile);
            try
            {
                file.Object.EXP = Experience.ToString();
                await Client
                    .Child(string.Format("{0}Stats", Username))
                    .Child(file.Key).PutAsync(file.Object);
            }catch { }
        }

        public async Task AddBossDefeated()
        {
            string bosses = UserModel.CheckForstring(Localfile, "TOTAL_BOSSES:");
            int totalbosses = Int32.Parse(bosses);
            totalbosses += 1;
            UserModel.Rewrite("TOTAL_BOSSES:", totalbosses.ToString(), Localfile);
            try
            {
                file.Object.TOTAL_BOSSES = totalbosses.ToString();
                await Client
                    .Child(string.Format("{0}Stats", Username))
                    .Child(file.Key).PutAsync(file.Object);
            }catch { }
            
        }
    }
}
