using DungeonTasker;
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
	public partial class RegisterView : ContentPage
	{
        /*
         * This is the Contructor, Initialize all controls
         * 
         */
		public RegisterView()
		{
			InitializeComponent();
            BindingContext = new RegisterViewModel(this);
		}
       

    }
}