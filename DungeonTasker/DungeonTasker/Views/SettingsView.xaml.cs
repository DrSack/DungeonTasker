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

namespace DungeonTasker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class SettingsView : ContentPage
    {
        /*
         * Constructor for Settings Class
         * @PARAMS
         * user: parse through to binding context
         * truth: parse through to  binding context
         * 
         * @RETURNS Nothing
         */
        public SettingsView(UserModel user, logged truth)
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel(user,truth,this);
        }
    }
}