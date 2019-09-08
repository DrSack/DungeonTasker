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
    public partial class Game : ContentPage
    {
        private bool battlesequence { get; set; }
        private bool WON { get; set; }
        private bool ANNOUNCING { get; set; }
        private int CharacterHP { get; set; }
        private int BossHP { get; set; }

        Dungeon dungeon;
        Stats boss;
        
        public Game(Dungeon dungeon)
        {
            this.dungeon = dungeon;
            this.boss = new Stats();
            this.boss.Health = 100;

            this.BossHP = this.boss.Health;
            this.CharacterHP = dungeon.stats.Health;

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
            await InitializeBattleSequqnce();
            await BossAttack();
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
                battlesequence = false;
            }
            else
            {
                label.Text = "PLAYER HAS INITIATION";
                battlesequence = true;
            }

            Announce.Children.Add(label);
            await label.FadeTo(1, 1000, Easing.Linear);
            await label.FadeTo(0, 1000, Easing.Linear);
            Announce.Children.Remove(label);
            ANNOUNCING = false;
        }

        private async Task Announcer(string message)
        {
            ANNOUNCING = true;
            var label = new Label();
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

        private async void checkHP()
        {
            if (WON)
            {
                await DisplayAlert("Congrats", "YOU WIN", "close");
            }
            else
            {
                await DisplayAlert("Congrats", "YOU LOSE", "close");
            }
            dungeon.clearBoss();
            await this.Navigation.PopModalAsync();
        }

        private async Task BossAttack()
        {
            if (!battlesequence)
            {
                Random rand = new Random();
                int damage = rand.Next(5, 25);
                await Announcer(string.Format("BOSS Dealt {0} Damage", damage.ToString()));
                CharacterHP -= damage;
                InitializeStats();
                CharacterHealth.RelRotateTo(360, 500);
                await CharacterHealth.ScaleTo(5, 300);
                await CharacterHealth.ScaleTo(1, 300);

                if (CharacterHP <= 0) { WON = false; checkHP(); }
                await Announcer("PLAYER TURN");
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
                await Announcer(string.Format("PLAYER Dealt {0} Damage", damage));
                BossHP -= damage;
                
                InitializeStats();

                if (BossHP <= 0) { WON = true; checkHP(); }
                await Announcer("BOSS TURN");
                await BossAttack();
            }
        }
    }
}