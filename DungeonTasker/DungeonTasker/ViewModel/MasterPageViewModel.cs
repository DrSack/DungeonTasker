﻿using DungeonTasker.Models;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    public class MasterPageViewModel
    {
        public Command Tasks_Clicked { get; set; }
        public Command Inventory_Clicked { get; set; }
        public Command Dungeon_Clicked { get; set; }
        public Command Settings_Clicked { get; set; }
        public Command About_Clicked { get; set; }

        Page page;
        UserModel user;
        InventoryItemsModel items;
        public DungeonView dungeon;
        WeaponInfoModel weapon;
        logged truth;
        MasterPageView mainpage;
        PageOpenModel PageOn;
        /*
         * Contructor for Masterpage, initialize all components and BindingContext 
         * 
         * PARAM
         * page: encapsulate page
         * user: encapsulate user
         * items: encapsulate items
         * weapon: encapsulate weapon
         * truth: encapsulate truth
         * mainpage: encapsulate mainpage
         * ContentPage: encapsulate display
         * 
         * RETURNS Nothing
         */
        public MasterPageViewModel(Page page, UserModel user, InventoryItemsModel items, WeaponInfoModel weapon, logged truth, MasterPageView mainpage, ContentPage display, DungeonView dungeon)
        {
            this.page = page;
            this.user = user;
            this.items = items;
            this.truth = truth;
            this.weapon = weapon;
            this.mainpage = mainpage;
            this.dungeon = dungeon;
            weapon.SetWeapon(display, UserModel.CheckForstring(items.Invfile, "Equipped:"));
            Tasks_Clicked = new Command(async () => await TaskNavAsync());
            Inventory_Clicked = new Command(async () => await InventoryNavAsync());
            Dungeon_Clicked = new Command(async () => await DungeonNavAsync());
            About_Clicked = new Command(async () => await AboutNavAsync());
            Settings_Clicked = new Command(async () => await SettingsNavAsync());
            PageOn = new PageOpenModel();
            PageOn.Tasks = true;
        }

        /*
         * If this is selected change the detailspage to the taskpage
         * 
         * PARAM Nothing
         * RETURNS Nothing
         */
        private async Task TaskNavAsync()
        {
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;
            if (!PageOn.Tasks)
            {
                PageOn.ResetAll();
                PageOn.Tasks = true;
                await SetPageAsync(page);
            } 
        }

        /*
         * If this is selected change the detailspage to the storepage
         * 
         * PARAM Nothing
         * RETURNS Nothing
         */
        private async Task InventoryNavAsync()
        {
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;
            if (!PageOn.Inventory)
            {
                PageOn.ResetAll();
                PageOn.Inventory = true;
                await SetPageAsync(new NavigationPage(new InventoryView(items, this.weapon, user)));
            }
        }

        /*
        * If this is selected change the detailspage to the Dungeonpage
        * 
        * PARAM Nothing
        * RETURNS Nothing
        */
        private async Task DungeonNavAsync()
        {
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;
            if (!PageOn.Dungeon)
            {
                PageOn.ResetAll();
                PageOn.Dungeon = true;
                dungeon.selectKey();
                await SetPageAsync(new NavigationPage(dungeon));
            }
        }


        /*
       * If this is selected change the detailspage to the AboutUs page.
       * 
       * PARAM Nothing
       * RETURNS Nothing
       */
        private async Task AboutNavAsync()
        {
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;
            if (!PageOn.AboutUs)
            {
                PageOn.ResetAll();
                PageOn.AboutUs = true;
                await SetPageAsync(new NavigationPage(new AboutUsView()));
            }
        }
        /*
         * If this is selected change the detailspage to the settings
         * 
         * PARAM Nothing
         * RETURNS Nothing
         */
        private async Task SettingsNavAsync()
        {
            ((MasterDetailPage)mainpage.Parent).IsPresented = false;
            if (!PageOn.Settings)
            {
                PageOn.ResetAll();
                PageOn.Settings = true;
                await SetPageAsync(new NavigationPage(new SettingsView(user, truth)));
            }
        }

        private async Task SetPageAsync(Page page)
        {
            ((MasterDetailPage)mainpage.Parent).Detail.FadeTo(0, 100);
            await Task.Delay(400);
            ((MasterDetailPage)mainpage.Parent).Detail = page;
            ((MasterDetailPage)mainpage.Parent).Detail.Opacity = 0;
            await ((MasterDetailPage)mainpage.Parent).Detail.FadeTo(1, 100);
        }
    }
}
