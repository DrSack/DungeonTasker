﻿using DungeonTasker.Models;
using DungeonTasker.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace DungeonTasker.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
        /*
         * Contructor for Masterpage, initialize all components and BindingContext 
         * 
         * PARAM
         * page: encapsulate page
         * user: encapsulate user
         * items: encapsulate items
         * weapon: encapsulate weapon
         * truth: encapsulate truth
         * 
         * RETURNS Nothing
         */

        public MasterPage (Page page, User user, InventoryItems items, WeaponInfo weapon, Stats stats,  logged truth)
		{
			InitializeComponent ();
            BindingContext = new MasterPageViewModel(page,user,items,weapon,stats,truth,this,this);
        }
    }
}