using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DungeonTasker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dungeon : ContentPage
    {
        private string[] Names = { "Fighter", "Mage", "Rogue", "Demon", "Zombie", "Vampire", "Ugandan Warlord", "Knight", "Thief", "BattleMage", "Death Robot" };
        private string[] Currentears = { "<>", "||", "!!" };
        private string Currenteyes = "0^#@.Xx";
        private string Currentnose = ".*@:!";
        private string CurrentBoss { get; set; }
        private string CurrentName { get; set; }
        User user;
        InventoryItems items;
        public Dungeon(User user, InventoryItems items)
        {
            this.user = user;
            this.items = items;
            InitializeComponent();
            selectBoss();
        }

        public void selectKey()
        {
            string keys = User.CheckForstring(items.Invfile, "Keys:");
            keys = keys.Replace(",", "");
            KeysLeft.Text = keys;
        }

        private void selectBoss()
        {
            if(CurrentBoss == null && CurrentName == null)
            {
                int num;
                string Boss;
                string Name;

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

                num = rand.Next(0, Names.Length - 1);
                Name = Names[num];

                Boss = string.Format("{0}{1}{2}{3}{4}", leftear, Eyes, Nose, Eyes, rightear);
                Character.Text = Boss;
                CharName.Text = Name;

                CurrentBoss = Boss;
                CurrentName = Name;
            }
        }
    }
}