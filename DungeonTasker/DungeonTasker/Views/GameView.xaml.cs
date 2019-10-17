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
    public partial class GameView : ContentPage
    {
        private bool battlesequence { get; set; }
        private bool WON { get; set; }
        private bool ANNOUNCING { get; set; }
        private int CharacterHP { get; set; }
        private int CharacterMP {get; set; }
        private int BossHP { get; set; }

        private bool MagicPage { get; set; }
        private int lowestdamage { get; set; }
        private int highestdamage { get; set; }
        private int MagicDamage = 0;
        private string message = "";
        private List<ItemModel> pots;
        DungeonView dungeon;

        /*
         * The contructor for the game class/page
         * Param dungeon: parse the dungeon class object.
         * Returns Nothing.
         */
        public GameView(DungeonView dungeon)
        {
            this.dungeon = dungeon;
            this.BossHP = this.dungeon.boss.Health;
            this.CharacterHP = dungeon.stats.Health;
            this.CharacterMP = dungeon.stats.Mana;
            this.MagicPage = false;
            this.lowestdamage = Convert.ToInt32((this.dungeon.boss.Health) * .1);
            this.highestdamage = Convert.ToInt32((this.dungeon.boss.Health) * .2);
            pots = dungeon.itemInv.pots;
            InitializeComponent();
            InitializeStats();
            InitializeItems();
        }

        /*
         * Create a display alert and pop the current page if yes is chosen
         * Param Nothing
         * Returns true(Dont back) if the result is No.
         */
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?(Lose keys)", "No", "Yes");
                if (!result) await this.Navigation.PopModalAsync(); // or anything else
            });

            return true;
        }

        /*
         * Set all the Label.Text variables to its assosiated element.
         * Param Nothing.
         * Returns Nothing.
         */
        private void InitializeStats()
        {
            Character.Text = dungeon.user.Character;
            CharacterName.Text = dungeon.user.Username;
            CharacterHealth.Text = CharacterHP.ToString();
            CharacterMana.Text = CharacterMP.ToString();

            Boss.Text = dungeon.CurrentBoss;
            BossName.Text = dungeon.CurrentName;
            BossHealth.Text = BossHP.ToString();
        }

        /*
         * When the page appears Sway the Boss and User Characters side to side.
         * Initiate the Battle Sequnce.
         * Param Nothing.
         * Returns Nothing.
         */
        protected override async void OnAppearing()
        {
            MoveCharBossAsync(Character, true);
            MoveCharBossAsync(Boss, false);
            await InitializeBattleSequqnce();
            BossAttack();
        }

        /*
        *
        * Create a random int which determines if the player of boss go first to attack. Then display the label
        * 
        * PARAM Nothing
        * RETURN Nothing
        */
        private async Task InitializeBattleSequqnce()
        {
            ANNOUNCING = true;
            Announce.IsVisible = true;
            Announce.IsEnabled = true;
            var label = new Label();
            label.Opacity = 0;
            label.HorizontalTextAlignment = TextAlignment.Center;
            label.VerticalTextAlignment = TextAlignment.Center;
            label.FontSize = 50;
            label.FontAttributes = FontAttributes.Bold;

            Random rand = new Random();
            if (rand.Next(0, 2) == 0)
            {
                label.Text = "BOSS HAS INITIATION";
                label.TextColor = Color.FromHex("#F44336");
                battlesequence = false;
            }
            else
            {
                label.Text = "PLAYER HAS INITIATION";
                label.TextColor = Color.Accent;
                battlesequence = true;
            }

            Announce.Children.Add(label);
            await label.FadeTo(1, 1000, Easing.Linear);
            await label.FadeTo(0, 1000, Easing.Linear);
            Announce.Children.Remove(label);
            Announce.IsVisible = false;
            Announce.IsEnabled = false;
            ANNOUNCING = false;
        }

        /*
        *
        * Display a message in the middle of the screen
        * 
        * PARAM Nothing
        * RETURN Nothing
        */
        private async Task Announcer(string message, bool battlesequence)
        {
            ANNOUNCING = true;
            Announce.IsVisible = true;
            Announce.IsEnabled = true;
            var label = new Label();
            if (!battlesequence)
            {
                label.TextColor = Color.FromHex("#F44336");
            }
            else if (battlesequence == true)
            {
                label.TextColor = Color.Accent;
            }
            label.Opacity = 0;
            label.HorizontalTextAlignment = TextAlignment.Center;
            label.VerticalTextAlignment = TextAlignment.Center;
            label.FontSize = 50;
            label.FontAttributes = FontAttributes.Bold;
            label.Text = message;
            Announce.Children.Add(label);
            await label.FadeTo(1, 500, Easing.Linear);
            await label.FadeTo(0, 500, Easing.Linear);
            Announce.Children.Remove(label);
            Announce.IsVisible = false;
            Announce.IsEnabled = false;
            ANNOUNCING = false;
        }
        /*
        *
        * Checks whenever the Player has won or lost. If so give the appropriate Exp and give loot to the player, followed by a congrats display alert.
        * Pop the current page back to the dungeon if display alert is closed, and rest the dungeon boss character and its HP.
        * Param Nothing.
        * Returns Nothing.
        */
        private async Task CheckHP()
        {
            if (WON)
            {
                Random rnd = new Random();
                int lootindx = rnd.Next(0, 2);
                int expgained = Convert.ToInt32(dungeon.boss.Health * .3);
                string[] loot = { "SteelSword", "IronSword", "WoodenSpoon" };
                string currentloot = loot[lootindx];
                UserModel.AddOntoLine("Weapons:", currentloot + ",", dungeon.items.Localfile);
                try
                {
                    dungeon.items.Invfile.Object.Weapons += currentloot + ",";
                    dungeon.weapon.UpdateInv();
                }
                catch { }
                
                dungeon.weapon.Rebuild();
                await dungeon.stats.ExpEnterAsync(expgained);
                await DisplayAlert("YOU WIN", string.Format("Loot: {0}\nExp gained: {1}\nExp left: {2}", currentloot, expgained, dungeon.stats.ExpLeft()), "Close");
                if (await dungeon.stats.StatsCheckAsync())
                {
                    await DisplayAlert("Congrats", string.Format("You are now Level: {0}\nCurrent Health: {1}", UserModel.CheckForstring(dungeon.stats.Localfile, "LEVEL:"), UserModel.CheckForstring(dungeon.stats.Localfile, "HEALTH:")), "Close");
                }
            }
            else
            {
                Random rnd = new Random();
                int lootindx = rnd.Next(0, 1);
                int expgained = Convert.ToInt32(dungeon.boss.Health * .1);
                string[] loot = { "WoodenSpoon", "WoodenBow" };
                string currentloot = loot[lootindx];
                UserModel.AddOntoLine("Weapons:", currentloot + ",", dungeon.items.Localfile);
                try
                {
                    dungeon.items.Invfile.Object.Weapons += currentloot + ",";
                    dungeon.weapon.UpdateInv();
                }
                catch { }
                dungeon.weapon.Rebuild();
                await dungeon.stats.ExpEnterAsync(expgained);
                await DisplayAlert("YOU LOSE", string.Format("Loot: {0}\nExp gained: {1}\nExp left: {2}", currentloot, expgained, dungeon.stats.ExpLeft()), "Close");
                if (await dungeon.stats.StatsCheckAsync())
                {
                    await DisplayAlert("Congrats", string.Format("You are now Level: {0}\nCurrent Health: {1}", UserModel.CheckForstring(dungeon.stats.Localfile, "LEVEL:"), UserModel.CheckForstring(dungeon.stats.Localfile, "HEALTH:")), "Close");
                }
            }
            UserModel.Rewrite("Updated:", DateTime.Now.ToString(), dungeon.user.LocalLogin);
            dungeon.Shop.Rebuild();
            dungeon.clearBoss();
            await this.Navigation.PopModalAsync();
        }

        /*
        *
        * Animate a Label to go from the character location to the other end of the screen
        * Param Nothing.
        * Returns Nothing.
        */
        private async Task AttackPixelCharacter()
        {
            CharacterAttacking.IsEnabled = true;
            CharacterAttacking.IsVisible = true;
            CharacterAttacking.Opacity = 100;
            await CharacterAttacking.TranslateTo(Application.Current.MainPage.Width - 26, -Application.Current.MainPage.Height + BossStats.Height + CharacterStats.Height, 1000);
            InitializeStats();
            CharacterAttacking.Opacity = 0;
            CharacterAttacking.TranslationY += Application.Current.MainPage.Height - BossStats.Height - CharacterStats.Height;
            CharacterAttacking.TranslationX -= Application.Current.MainPage.Width + 26;
            CharacterAttacking.IsEnabled = false;
            CharacterAttacking.IsVisible = false;
        }

        /*
        *
        * Animate a Label to go from the boss location to the other end of the screen
        * Param Nothing.
        * Returns Nothing.
        */
        private async Task AttackPixelBoss()
        {
            BossAttacking.IsEnabled = true;
            BossAttacking.IsVisible = true;
            BossAttacking.Opacity = 100;
            await BossAttacking.TranslateTo(-Application.Current.MainPage.Width + 10, Application.Current.MainPage.Height - BossStats.Height - 36, 1000);
            InitializeStats();
            BossAttacking.Opacity = 0;
            BossAttacking.TranslationY -= Application.Current.MainPage.Height - BossStats.Height - 36;
            BossAttacking.TranslationX += Application.Current.MainPage.Width - 10;
            BossAttacking.IsEnabled = false;
            BossAttacking.IsVisible = false;
        }

        /*
        *
        * A basic animation which moves Labels side to side
        * Param Nothing.
        * Returns Nothing.
        */
        private async void MoveCharBossAsync(Label move, bool truth)
        {
            await Task.Run(async () =>
            {
                while (CharacterHP >= 0 || BossHP >= 0)
                {
                    if (truth)
                    {
                        await move.TranslateTo(5, 0, 500);
                        await move.TranslateTo(-5, 0, 500);
                    }

                    else
                    {
                        await move.TranslateTo(-5, 0, 500);
                        await move.TranslateTo(5, 0, 500);
                    }
                }
            });


        }

        /*
        *
        * The boss attack sequence, this combines all animations, health checks and damage counters.
        * Param Nothing.
        * Returns Nothing.
        */

        private async void BossAttack()
        {
            if (!battlesequence)
            {
                Random rand = new Random();
                int damage = rand.Next(lowestdamage, highestdamage);
                await Announcer(string.Format("BOSS Dealt {0} Damage", damage.ToString()), false);
                CharacterHP -= damage;
                await AttackPixelBoss();
                CharacterHealth.RelRotateTo(360, 500);
                await CharacterHealth.ScaleTo(5, 300);
                await CharacterHealth.ScaleTo(1, 300);

                if (CharacterHP <= 0) { WON = false; await CheckHP(); }
                await Announcer("PLAYER TURN", true);
                battlesequence = true;
            }

        }

        /*
      *
      * The Character attack sequence, this combines all animations, health checks and damage counters.
      * Param Nothing.
      * Returns Nothing.
      */

        private async void AttackBtn(object sender, EventArgs e)
        {
            DisableorEnableFrameLayouts(false, MagicAbility);
            DisableorEnableFrameLayouts(false, ItemAbility);
            if (battlesequence && !ANNOUNCING)
            {
                battlesequence = false;
                Random rand = new Random();
                int damage = rand.Next(dungeon.weapon.Minimumdmg, dungeon.weapon.Maximumdmg + 1);
                await Announcer(string.Format("PLAYER Dealt {0} Damage", damage), true);
                BossHP -= damage;
                await AttackPixelCharacter();
                BossHealth.RelRotateTo(360, 500);
                await BossHealth.ScaleTo(5, 300);
                await BossHealth.ScaleTo(1, 300);

                if (BossHP <= 0) { WON = true; await CheckHP(); }
                await Announcer("BOSS TURN", false);
                BossAttack();
            }
        }

        /*
         * The dodge sequence, this combines all animations, health checks and damage counters.
         *  Param Nothing
         *  Returns Nothing.
         */

        private async void DodgeBtn(object sender, EventArgs e)
        {
            DisableorEnableFrameLayouts(false, MagicAbility);
            DisableorEnableFrameLayouts(false, ItemAbility);
            if (battlesequence && !ANNOUNCING)
            {
                battlesequence = false;
                var rand = new Random();
                int dodge = rand.Next(0, 10);
                await Announcer(string.Format("Dodging..."), true);
                if (dodge >= 5)
                {
                    await AttackPixelBoss();
                    await Announcer(string.Format("Player Dodge Success"), true);


                }
                else
                {
                    await Announcer(string.Format("Player Dodge failure"), false);
                    BossAttack();

                }

                if (CharacterHP <= 0) { WON = false; await CheckHP(); }
                battlesequence = true;
            }

        }

        /*
         * The dodge sequence, this combines all animations, health checks and damage counters.
         *  Param Nothing
         *  Returns Nothing.
         */

        private async void MagicBtn(object sender, EventArgs e)
        {
            if(MagicAbility.IsEnabled != true)
            {
                DisableorEnableFrameLayouts(false, ItemAbility);

            if (!ItemAbility.IsEnabled && !ANNOUNCING && battlesequence)
            {
                    DisableorEnableFrameLayouts(true, MagicAbility);
                    MagicAbility.Opacity = 0;
                    await MagicAbility.FadeTo(1, 200);

                    if (!MagicPage)
                    {
                        MagicPage = true;
                        ButtonFire.Clicked += async (s, a) =>
                        {
                            if (CheckMana(CharacterMP, 15))
                            {
                                Random rand = new Random();
                                CharacterMP -= 15;
                                MagicDamage = rand.Next(15, 30 + 1);
                                message = "Casted Fireball";
                                await MagicAbility.FadeTo(0, 200);
                                DisableorEnableFrameLayouts(false, MagicAbility);
                            }
                            else
                            {
                                DisplayAlert("Error", "Not enough mana", "Close");
                            }
                        };

                        ButtonLightning.Clicked += async (s, a) =>
                        {
                            if (CheckMana(CharacterMP, 30))
                            {
                                Random rand = new Random();
                                CharacterMP -= 30;
                                MagicDamage = rand.Next(25, 45 + 1);
                                message = "Casted Lightning";
                                await MagicAbility.FadeTo(0, 200);
                                DisableorEnableFrameLayouts(false, MagicAbility);
                            }
                            else
                            {
                                DisplayAlert("Error", "Not enough mana", "Close");
                            }
                        };

                        ButtonVoid.Clicked += async (s, a) =>
                        {
                            if (CheckMana(CharacterMP, 50))
                            {
                                Random rand = new Random();
                                CharacterMP -= 50;
                                MagicDamage = rand.Next(35, 55 + 1);
                                message = "Casted Voidball";
                                await MagicAbility.FadeTo(0, 200);
                                DisableorEnableFrameLayouts(false, MagicAbility);
                            }
                            else
                            {
                                DisplayAlert("Error", "Not enough mana", "Close");
                            }
                        };
                    }
               

                while (MagicDamage==0) { await Task.Delay(25); if (!battlesequence) {return;} if(ItemAbility.IsEnabled) {return;} }
                    await Task.Delay(200);
                await Announcer(message, true);
                await Announcer(string.Format("Dealt {0} damage", MagicDamage.ToString()), true);

                battlesequence = false;
                BossHP -= MagicDamage;

                MagicDamage = 0;
                message = "";

                CharacterMana.Text = CharacterMP.ToString();
                await AttackPixelCharacter();
                BossHealth.RelRotateTo(360, 500);
                await BossHealth.ScaleTo(5, 300);
                await BossHealth.ScaleTo(1, 300);

                if (BossHP <= 0) { WON = true; await CheckHP(); }
                await Announcer("BOSS TURN", false);
                BossAttack();
            }
            }

        }

        private async void ItemsBtn(object sender, EventArgs e)
        {
            if(ItemAbility.IsEnabled != true)
            {
                DisableorEnableFrameLayouts(false, MagicAbility);
                if (!MagicAbility.IsEnabled && !ANNOUNCING && battlesequence)
                {
                    DisableorEnableFrameLayouts(true, ItemAbility);
                    ItemAbility.Opacity = 0;
                    await ItemAbility.FadeTo(1, 200);
                    
                }
            }
        }

        private void InitializeItems()
        {
            foreach (ItemModel item in pots)
            {
                CreateItemDisplay(item, pots);
            }
        }

        private bool CheckMana(int MP, int SubMP)
        {
            if(MP - SubMP < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CreateItemDisplay(ItemModel item, List<ItemModel> list)
        {
            var layout = new StackLayout();
            layout.Orientation = StackOrientation.Horizontal;

            var Name = new Label();
            Name.Text = item.item;
            Name.VerticalTextAlignment = TextAlignment.Center;

            var extras = new Label();
            extras.Text = string.Format("{0}: {1} - {2} {3}",
            ItemInfoModel.ObtainItemString(item.item, true),
            ItemInfoModel.ObtainItemInfo(item.item, true),
            ItemInfoModel.ObtainItemInfo(item.item, false),
            ItemInfoModel.ObtainItemString(item.item, false));
            extras.VerticalTextAlignment = TextAlignment.Center;

            var button = new Button();
            button.HorizontalOptions = LayoutOptions.EndAndExpand;
            button.Text = "USE";

            button.Clicked += async (s, e) =>
            {
                battlesequence = false;
                Animations.CloseStackLayout(layout, "closing", 30, 200);
                Items.Children.Remove(layout);
                pots.Remove(item); // Remove off list
                string invetory = "";
                foreach (ItemModel itemInv in pots)
                {
                    if (!String.IsNullOrEmpty(itemInv.item))
                    {
                        invetory += itemInv.item + ","; // Create string for file
                    }
                }
                UserModel.Rewrite("Items:", invetory, dungeon.items.Localfile);

                try
                {
                    dungeon.items.Invfile.Object.Items = invetory;
                    await dungeon.items.UpdateInv();
                }catch { }
                
                dungeon.itemInv.pots = this.pots;

                await ItemAbility.FadeTo(0, 200);
                DisableorEnableFrameLayouts(false, ItemAbility);

                if (item.item.Contains("HealthPotion"))
                {
                    int buff = Obtainbuff(item.item);
                    CharacterHP += buff;
                    CharacterHealth.Text = CharacterHP.ToString();
                    await Announcer(string.Format("Healed for {0}", buff.ToString()),true);
                    CharacterHealth.RelRotateTo(360, 500);
                    await CharacterHealth.ScaleTo(5, 300);
                    await CharacterHealth.ScaleTo(1, 300);
                }
                else if (item.item.Contains("MagicPotion"))
                {
                    int buff = Obtainbuff(item.item);
                    CharacterMP += buff;
                    CharacterMana.Text = CharacterMP.ToString();
                    await Announcer(string.Format("Restored {0} Mana", buff.ToString()), true);
                    CharacterMana.RelRotateTo(360, 500);
                    await CharacterMana.ScaleTo(5, 300);
                    await CharacterMana.ScaleTo(1, 300);

                }

                await Announcer("BOSS TURN", false);
                BossAttack();
            };

            layout.Children.Add(Name);
            layout.Children.Add(extras);
            layout.Children.Add(button);

            Items.Children.Add(layout);
        }

        private void DisableorEnableFrameLayouts(bool truth, Frame layout)
        {
            layout.IsEnabled = truth;
            layout.IsVisible = truth;
        }

        private int Obtainbuff(string item)
        {
            Random rand = new Random();
            int minimum = ItemInfoModel.ObtainItemInfo(item, true);
            int maximum = ItemInfoModel.ObtainItemInfo(item, false);
            return rand.Next(minimum, maximum + 1);
        }
    }

}