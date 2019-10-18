using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public class DungeonModel
    {
        private string[] Names = { "Fighter", "Mage", "Rogue", "Demon", "Zombie", "Vampire", "Ugandan Warlord", "Knight", "Thief", "Battle Mage", "Death Robot", "Ogre" };
        private string[] Currentears = { "<>", "||", "!!", "~~", "^^", "{}", "[]", "++" };
        private string Currenteyes = "0^#@.Xx-";
        private string Currentnose = ".*@:!O0IVvXxw";
        public string EasyBoss { get; set; }
        public string EasyName { get; set; }
        public string MediumBoss { get; set; }
        public string MediumName { get; set; }
        public string HardBoss { get; set; }
        public string HardName { get; set; }

        public bool RESET = true;
        public StatsModel Easyboss = new StatsModel();
        public StatsModel Mediumboss = new StatsModel();
        public StatsModel Hardboss = new StatsModel();

        public ShopModel Shop;
        public DungeonModel()
        {
            selectBossesHP();
            SelectBosses();
        }

        public void Reselect()
        {
            RESET = true;
            selectBossesHP();
            SelectBosses();
        }

        private void selectBossesHP()
        {
            Random rnd = new Random();
            int EasyHEALTH = rnd.Next(25, 51);
            int MediumHEALTH = rnd.Next(65, 112);
            int HardHEALTH = rnd.Next(90, 170);
            Easyboss.Health = EasyHEALTH;
            Mediumboss.Health = MediumHEALTH;
            Hardboss.Health = HardHEALTH;
        }

        private void SelectBosses()
        {
            if (RESET)
            {
                RESET = false;
                EasyBoss = SelectBoss();
                EasyName = SelectName();

                MediumBoss = SelectBoss();
                MediumName = SelectName();
                while (EasyName == MediumBoss || EasyName == MediumName)
                    MediumBoss = SelectBoss();
                    MediumName = SelectName();

                HardBoss = SelectBoss();
                HardName = SelectName();
                while ((HardBoss == MediumBoss || HardName == MediumName) && (HardBoss == EasyBoss || HardName == EasyName))//Make sure that values are unique
                    HardBoss = SelectBoss();
                    HardName = SelectName();
            }
        }

        private string SelectBoss()
        {
            int num;
            string Boss;

            char leftear;
            char rightear;
            char Eyes;
            char Nose;
            Random rand = new Random();

            num = rand.Next(0, Currentears.Length - 1);
            leftear = Currentears[num][0];
            rightear = Currentears[num][1];

            num = rand.Next(0, Currenteyes.Length - 1);
            Eyes = Currenteyes[num];

            num = rand.Next(0, Currentnose.Length - 1);
            Nose = Currentnose[num];

            Boss = string.Format("{0}{1}{2}{3}{4}", leftear, Eyes, Nose, Eyes, rightear);
            return Boss;
        }

        private string SelectName()
        {
            int num;
            string Name;
            Random rand = new Random();

            num = rand.Next(0, Names.Length - 1);
            Name = Names[num];
            return Name;
        }

    }
}
