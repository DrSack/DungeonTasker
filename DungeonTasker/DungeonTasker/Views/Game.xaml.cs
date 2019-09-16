﻿using DungeonTasker.Models;
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
    public partial class Game : ContentPage
    {
        private bool battlesequence { get; set; }
        private bool WON { get; set; }
        private bool ANNOUNCING { get; set; }
        private int CharacterHP { get; set; }
        private int BossHP { get; set; }

        private int lowestdamage { get; set; }
        private int highestdamage { get; set; }
        Dungeon dungeon;
        
        
        public Game(Dungeon dungeon)
        {
            this.dungeon = dungeon;
            this.BossHP = this.dungeon.boss.Health;
            this.CharacterHP = dungeon.stats.Health;
            this.lowestdamage = Convert.ToInt32((this.dungeon.boss.Health) * .1);
            this.highestdamage = Convert.ToInt32((this.dungeon.boss.Health) * .2);
            InitializeComponent();
            InitializeStats();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?(Lose keys)", "No", "Yes");
                if (!result) await this.Navigation.PopModalAsync(); // or anything else
            });

            return true;
        }

        private void InitializeStats()
        {
            Character.Text = dungeon.user.Character;
            CharacterName.Text = dungeon.user.Username;
            CharacterHealth.Text = CharacterHP.ToString();

            Boss.Text = dungeon.CurrentBoss;
            BossName.Text = dungeon.CurrentName;
            BossHealth.Text = BossHP.ToString();
        }

        protected override async void OnAppearing()
        {
            MoveCharBossAsync(Character, true);
            MoveCharBossAsync(Boss, false);
            await InitializeBattleSequqnce();
            BossAttack();
        }

            private async Task InitializeBattleSequqnce()
        {
            ANNOUNCING = true;
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
            ANNOUNCING = false;
        }

        private async Task Announcer(string message, bool battlesequence)
        {
            ANNOUNCING = true;
            var label = new Label();
            if (!battlesequence)
            {
                label.TextColor = Color.FromHex("#F44336");
            }
            else if(battlesequence == true)
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
            ANNOUNCING = false;
        }

        private async Task CheckHP()
        {
            if (WON)
            {
                Random rnd = new Random();
                int lootindx = rnd.Next(0, 2);
                int expgained = Convert.ToInt32(dungeon.boss.Health * .3);
                string[] loot = { "SteelSword", "IronSword", "WoodenSpoon" };
                string currentloot = loot[lootindx];
                User.AddOntoLine("Weapons:", currentloot+",", dungeon.items.Invfile);
                dungeon.stats.ExpEnter(expgained);
                await DisplayAlert("YOU WIN", string.Format("Loot: {0}\nExp gained: {1}\nExp left: {2}", currentloot, expgained, dungeon.stats.ExpLeft()), "Close");
                if (dungeon.stats.StatsCheck())
                {
                    await DisplayAlert("Congrats", string.Format("You are now Level: {0}\nCurrent Health: {1}", User.CheckForstring(dungeon.stats.file, "LEVEL:"), User.CheckForstring(dungeon.stats.file, "HEALTH:")), "Close");
                }
            }
            else
            {
                Random rnd = new Random();
                int lootindx = rnd.Next(0, 1);
                int expgained = Convert.ToInt32(dungeon.boss.Health * .1);
                string[] loot = { "WoodenSpoon", "WoodenBow"};
                string currentloot = loot[lootindx];
                User.AddOntoLine("Weapons:", currentloot+",", dungeon.items.Invfile);
                dungeon.stats.ExpEnter(expgained);
                await DisplayAlert("YOU LOSE", string.Format("Loot: {0}\nExp gained: {1}\nExp left: {2}", currentloot,expgained, dungeon.stats.ExpLeft()), "Close");
                if (dungeon.stats.StatsCheck())
                {
                    await DisplayAlert("Congrats", string.Format("You are now Level: {0}\nCurrent Health: {1}", User.CheckForstring(dungeon.stats.file,"LEVEL:"), User.CheckForstring(dungeon.stats.file,"HEALTH:")), "Close");
                }
            }
            dungeon.clearBoss();
            await this.Navigation.PopModalAsync();
        }


        private async Task AttackPixelCharacter()
        {
            CharacterAttacking.Opacity = 100;
            await CharacterAttacking.TranslateTo(Application.Current.MainPage.Width - 10, -Application.Current.MainPage.Height + BossStats.Height + CharacterStats.Height, 1000);
            InitializeStats();
            CharacterAttacking.Opacity = 0;
            CharacterAttacking.TranslationY += Application.Current.MainPage.Height - BossStats.Height - CharacterStats.Height;
            CharacterAttacking.TranslationX -= Application.Current.MainPage.Width + 10;
        }

        private async Task AttackPixelBoss()
        {
            BossAttacking.Opacity = 100;
            await BossAttacking.TranslateTo(-Application.Current.MainPage.Width + 10, Application.Current.MainPage.Height - BossStats.Height - 36, 1000);
            InitializeStats();
            BossAttacking.Opacity = 0;
            BossAttacking.TranslationY -= Application.Current.MainPage.Height - BossStats.Height - 36;
            BossAttacking.TranslationX += Application.Current.MainPage.Width - 10;
        }

        private async void MoveCharBossAsync(Label move, bool nice)
        {
            await Task.Run(async () =>
            {
                while (CharacterHP >= 0 || BossHP >= 0)
                {
                    if (nice)
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

        private async void BossAttack()
        {
            if (!battlesequence)
            {
                Random rand = new Random();
                int damage = rand.Next(lowestdamage, highestdamage);
                await Announcer(string.Format("BOSS Dealt {0} Damage", damage.ToString()),false);
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

        private async void AttackBtn(object sender, EventArgs e)
        {
            if (battlesequence && !ANNOUNCING)
            {
                battlesequence = false;
                Random rand = new Random();
                int damage = rand.Next(dungeon.weapon.Minimum, dungeon.weapon.Maximum+1);
                await Announcer(string.Format("PLAYER Dealt {0} Damage",damage),true);
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
    }
}