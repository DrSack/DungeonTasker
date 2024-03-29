﻿using DungeonTasker.Models;
using DungeonTasker.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DungeonTasker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class GreetPageView : ContentPage
    {
        bool begin = true;
        
        /*
         * Contructor for GreetPage, Initialize all controls. 
         * 
         */

        public GreetPageView()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (begin)
            {
                GreetPageViewModel VM = new GreetPageViewModel
                {
                    Navigation = Navigation
                };
                BindingContext = VM;
                begin = await VM.OnAppearingAsync(begin); 
            }
        }
    }
}