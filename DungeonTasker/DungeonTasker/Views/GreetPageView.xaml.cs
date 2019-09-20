using DungeonTasker.Models;
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
            GreetPageViewModel VM = new GreetPageViewModel
            {
                Navigation = Navigation
            };
            BindingContext = VM;
            MessagingCenter.Subscribe<GreetPageViewModel>(this, "Animation", async (sender) =>{
                await this.FadeTo(1, 300);
                await this.FadeTo(0, 300);
                MessagingCenter.Send(this, "Done");
                MessagingCenter.Unsubscribe<GreetPageViewModel>(this, "Animation");
            });
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            begin = GreetPageViewModel.OnAppearing(begin);
        }

    }
}