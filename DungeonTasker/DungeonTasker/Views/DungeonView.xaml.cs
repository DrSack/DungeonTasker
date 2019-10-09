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
    public partial class DungeonView : ContentPage
    {
        private string[] Names = { "Fighter", "Mage", "Rogue", "Demon", "Zombie", "Vampire", "Ugandan Warlord", "Knight", "Thief", "Battle Mage", "Death Robot", "Ogre" };
        private string[] Currentears = { "<>", "||", "!!", "~~", "^^", "{}", "[]", "++" };
        private string Currenteyes = "0^#@.Xx-";
        private string Currentnose = ".*@:!O0IVvXxw";
        public string CurrentBoss { get; set; }
        public string CurrentName { get; set; }

        public bool tut { get; set; }
        public UserModel user;
        public InventoryItemsModel items;
        public WeaponInfoModel weapon;
        public ItemInfoModel itemInv;
        public StatsModel stats;
        public StatsModel boss = new StatsModel();
        
        /*
         * Constructor for Dungeon
         * Encapsulates UserModel,InventoryItems,WeaponInfo,Stats objects + the bool variable
         * 
         * PARAM
         * UserModel:parse UserModel,
         * items: parse items,
         * weapon: parse weapon ,
         * stats: parse stats,
         * tut: true of false if the tutorial display alert is wished to be displayed
         * 
         * RETURN Nothing
         */
        public DungeonView(UserModel user, InventoryItemsModel items, WeaponInfoModel weapon, ItemInfoModel itemInv, StatsModel stats, bool tut)
        {
            this.user = user;
            this.items = items;
            this.weapon = weapon;
            this.stats = stats;
            this.tut = tut;
            this.itemInv = itemInv;
            InitializeComponent();
            selectBoss();
            selectBossHP();
        }
        /*
         * Obtains how keys there are within the Invfile and display it on the KetsLeft label text
         * 
         * PARAM Nothing
         * RETURN Nothing
         */
        public void selectKey()
        {
            string keys = UserModel.CheckForstring(items.Invfile, "Keys:");
            keys = keys.Replace(",", "");
            KeysLeft.Text = keys;
        }

        /*
         * Set current boss and name to NULL, then reselect the boss hp and character.
         * 
         * PARAM Nothing
         * RETURN Nothing
         */
        public void clearBoss()
        {
            CurrentBoss = null;
            CurrentName = null;
            selectBossHP();
            selectBoss();
        }

        /*
         * Randomly select the BossHP
         * 
         * PARAM Nothing
         * RETURN Nothing
         */
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
         * RETURN Nothing
         */

        private void selectBoss()
        {
            if (CurrentBoss == null && CurrentName == null)
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
                Character.HorizontalTextAlignment = TextAlignment.Center;

                CharName.Text = Name;

                CurrentBoss = Boss;
                CurrentName = Name;
                
            }
        }

        /*
        * When the button is pressed check whenever the UserModel has any keys available, then open the Game page
        * 
        * PARAM sender, e
        * RETURN Nothing
        */
        private async void BattleBtn(object sender, EventArgs e)
        {
            int realKeys;
            string keys = UserModel.CheckForstring(items.Invfile, "Keys:");
            keys = keys.Replace(",", "");
            realKeys = Int32.Parse(keys);

            if (realKeys > 0) {
                await this.Navigation.PushModalAsync(new GameView(this));
                items.GiveKey(-1);
                selectKey();
            }

            else {
                await DisplayAlert("Error", "Not enough keys to battle", "cancel");
            }
        }

        /*
        * When the page appears display the tutorial screen if the tut bool is active
        * 
        * PARAM Nothing
        * RETURN Nothing
        */
        protected override async void OnAppearing()
        {
            if (tut)
            {
                await UserModel.ShowMessage("This is Dungeon Menu\nThis is where you can see the boss you are about to Battle!\n", "Dungeon", "Close", this, async () =>
                {
                    await this.Navigation.PopModalAsync();
                });
            }
        }
    }
}