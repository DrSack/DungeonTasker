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

    public partial class GreetPage : ContentPage
    {
        /*
         * Contructor for GreetPage, Initialize all controls. 
         * 
         */
        public GreetPage()
        {
            InitializeComponent();
            BindingContext = new GreetPageViewModel(this);
        }
      
    }
}