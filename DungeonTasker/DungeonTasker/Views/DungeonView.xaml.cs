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
        public string CurrentBoss { get; set; }
        public string CurrentName { get; set; }

        public bool tut { get; set; }
        public UserModel user;
        public InventoryItemsModel items;
        public WeaponInfoModel weapon;
        public ItemInfoModel itemInv;
        public StatsModel stats;
        public StatsModel boss = new StatsModel();
        public ShopModel Shop;
        public DungeonModel realdungeon;
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
        public DungeonView(UserModel user, InventoryItemsModel items, WeaponInfoModel weapon, ItemInfoModel itemInv, StatsModel stats, bool tut, DungeonModel realdungeon)
        {
            this.realdungeon = realdungeon;
            this.user = user;
            this.items = items;
            this.weapon = weapon;
            this.stats = stats;
            this.tut = tut;
            this.itemInv = itemInv;
            InitializeComponent();
            InitializeBoss();
        }

        public void InitializeBoss()
        {
            boss.Health = realdungeon.Easyboss.Health;
            Character.Text = realdungeon.EasyBoss;
            Character.HorizontalTextAlignment = TextAlignment.Center;
            CharName.Text = ""; CharName.Text = realdungeon.EasyName;
            TypeBoss.Text = "Easy";
            TypeBoss.TextColor = Color.Accent;
            CurrentBoss = realdungeon.EasyBoss;
            CurrentName = realdungeon.EasyName;
        }

        private void Easy(object sender, EventArgs e)
        {
            boss.Health = realdungeon.Easyboss.Health;
            Character.Text = realdungeon.EasyBoss;
            Character.HorizontalTextAlignment = TextAlignment.Center;
            CharName.Text = ""; CharName.Text = realdungeon.EasyName;
            TypeBoss.Text = "Easy";
            TypeBoss.TextColor = Color.Accent;
            CurrentBoss = realdungeon.EasyBoss;
            CurrentName = realdungeon.EasyName;
        }

        private void Medium(object sender, EventArgs e)
        {
            boss.Health = realdungeon.Mediumboss.Health;
            Character.Text = realdungeon.MediumBoss;
            Character.HorizontalTextAlignment = TextAlignment.Center;
            CharName.Text = ""; CharName.Text = realdungeon.MediumName;
            TypeBoss.Text = "Medium";
            TypeBoss.TextColor = Color.Orange;
            CurrentBoss = realdungeon.MediumBoss;
            CurrentName = realdungeon.MediumName;
        }

        private void Hard(object sender, EventArgs e)
        {
            boss.Health = realdungeon.Hardboss.Health;
            Character.Text = realdungeon.HardBoss;
            Character.HorizontalTextAlignment = TextAlignment.Center;
            CharName.Text = ""; CharName.Text = realdungeon.HardName;
            TypeBoss.Text = "Hard";
            TypeBoss.TextColor = Color.FromHex("#F44336");
            CurrentBoss = realdungeon.HardBoss;
            CurrentName = realdungeon.HardName;
        }
        /*
         * Obtains how keys there are within the Invfile and display it on the KetsLeft label text
         * 
         * PARAM Nothing
         * RETURN Nothing
         */
        public void selectKey()
        {
            string keys = UserModel.CheckForstring(items.Localfile, "Keys:");
            keys = keys.Replace(",", "");
            KeysLeft.Text = keys;
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
            string keys = UserModel.CheckForstring(items.Localfile, "Keys:");
            keys = keys.Replace(",", "");
            realKeys = Int32.Parse(keys);

            if (realKeys >= 0) {
                await this.Navigation.PushModalAsync(new GameView(this));
                items.GiveKeyAsync(0);
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