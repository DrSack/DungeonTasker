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
        public string CurrentBoss { get; set; }
        public string CurrentName { get; set; }

        public User user;
        public InventoryItems items;
        public WeaponInfo weapon;
        public Stats stats;
        public Stats boss = new Stats();

        public Dungeon(User user, InventoryItems items, WeaponInfo weapon, Stats stats)
        {
            this.user = user;
            this.items = items;
            this.weapon = weapon;
            this.stats = stats;
            InitializeComponent();
            selectBoss();
            selectBossHP();
        }
        /*
         * Obtains how keys there are within the Invfile and display it on the KetsLeft label text
         * 
         * PARAM Nothing
         * RETURNS Nothing
         */

        public void selectKey()
        {
            string keys = User.CheckForstring(items.Invfile, "Keys:");
            keys = keys.Replace(",", "");
            KeysLeft.Text = keys;
        }

        public void clearBoss()
        {
            CurrentBoss = null;
            CurrentName = null;
            selectBossHP();
            selectBoss();
        }

        private void selectBossHP()
        {
            Random rnd = new Random();
            int HEALTH = rnd.Next(25, 101);
            boss.Health = HEALTH;
        }

        /*
         * Randomly selects the features of what the boss character will look like
         * 
         * PARAM Nothing
         * RETURNS Nothing
         */

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

        private async void BattleBtn(object sender, EventArgs e)
        {
            int realKeys;
            string keys = User.CheckForstring(items.Invfile, "Keys:");
            keys = keys.Replace(",", "");
            realKeys = Int32.Parse(keys);
            if(realKeys > 0)// Made this = 0 for debugging purposes, didnt want to wait for a key cause lazy 
            {
                await this.Navigation.PushModalAsync(new Game(this));
                items.GiveKey(-1);
                selectKey();
            }
            else
            {
                await DisplayAlert("Error", "Not enough keys to battle", "cancel");
            }
        }
    }
}