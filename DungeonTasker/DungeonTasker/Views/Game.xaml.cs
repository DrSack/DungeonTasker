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
        private bool game { get; set; }
        private bool attack { get; set; }
        private bool WON { get; set; }

        Dungeon dungeon;
        Stats boss;
        
        public Game(Dungeon dungeon)
        {
            this.dungeon = dungeon;
            this.boss = new Stats();
            this.boss.Health = 100;
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
            CharacterHealth.Text = dungeon.stats.Health.ToString();

            Boss.Text = dungeon.CurrentBoss;
            BossName.Text = dungeon.CurrentName;
            BossHealth.Text = boss.Health.ToString();
        }

        protected override async void OnAppearing()
        {
            await InitializeBattleSequqnce();
            await BossAttack();
        }

            private async Task InitializeBattleSequqnce()
        {
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

            game = true;
            Announce.Children.Add(label);
            await label.FadeTo(1, 1000, Easing.Linear);
            await label.FadeTo(0, 1000, Easing.Linear);
            Announce.Children.Remove(label);
        }

        private async Task Announcer(string message)
        {
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
        }

        private async void checkHP()
        {
            if (WON)
            {
                await DisplayAlert("Congrats", "YOU WIN", "close");
                dungeon.stats.Health = 100;
                boss.Health = 100;
            }
            else
            {
                await DisplayAlert("Congrats", "YOU LOSE", "close");
                dungeon.stats.Health = 100;
                boss.Health = 100;
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
                dungeon.stats.Health -= damage;
                InitializeStats();
                CharacterHealth.RelRotateTo(360, 500);
                await CharacterHealth.ScaleTo(5, 300);
                await CharacterHealth.ScaleTo(1, 300);

                if (dungeon.stats.Health <= 0) { WON = false; game = false; checkHP(); }
                await Announcer("PLAYER TURN");
                battlesequence = true;
            }
            
        }

        private async void AttackBtn(object sender, EventArgs e)
        {
            if (battlesequence)
            {
                battlesequence = false;
                await Announcer("Player Dealt 3 Damage");
                boss.Health -= dungeon.weapon.CurrentDmg;
                
                InitializeStats();

                if (boss.Health <= 0) { WON = true; game = false; checkHP(); }
                await Announcer("BOSS TURN");
                await BossAttack();
            }
        }
    }
}