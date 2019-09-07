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
            BossHeatlh.Text = boss.Health.ToString();
        }

        private void InitializeBattleSequqnce()
        {
            Random rand = new Random();
            if (rand.Next(0, 2) == 0)
            {
                var label = new Label();
            }
            else
            {

            }
                
        }
    }
}