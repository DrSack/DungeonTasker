using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.ViewModel
{
    public class StatsViewModel
    {
        UserModel user;
        ItemInfoModel items;
        StatsModel stats;
        public string Character { get; set; }
        public string Health { get; set; }
        public string Mana { get; set; }
        public string BossDefeated { get; set; }
        public string Keys { get; set; }
        public string Exp {get; set;}
        public string Name { get; set; }
        public string Levels { get; set; }
        public StatsViewModel(ItemInfoModel item, UserModel user, StatsModel stats)
        {
            this.items = item;
            this.user = user;
            this.stats = stats;
            int Level = Int32.Parse(UserModel.CheckForstring(stats.Localfile, "LEVEL:"));
            int expInt = Int32.Parse(UserModel.CheckForstring(stats.Localfile, "EXP:"));
            Name = UserModel.CheckForstring(user.LocalLogin, "Fullname:");
            Character = user.Character;
            BossDefeated = UserModel.CheckForstring(stats.Localfile, "TOTAL_BOSSES:");
            Keys = UserModel.CheckForstring(item.items.Localfile, "TOTAL_KEYS:");
            Health = String.Format("{0}",UserModel.CheckForstring(stats.Localfile, "HEALTH:"));
            Mana = String.Format("{0}",UserModel.CheckForstring(stats.Localfile, "MANA:"));
            int Remaining = (((Level * 21) + 15) - expInt);
            Levels = UserModel.CheckForstring(stats.Localfile, "LEVEL:");
            Exp = String.Format("{0}",Remaining.ToString());
        }
    }
}
