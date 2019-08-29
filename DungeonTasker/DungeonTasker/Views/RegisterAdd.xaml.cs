using DungeonTasker;
using DungeonTasker.Models;
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
	public partial class RegisterAdd : ContentPage
	{
        GreetPage nice;
        bool checkers;
		public RegisterAdd(GreetPage cool, bool check)
		{
			InitializeComponent();
            this.checkers = check;
            this.Title = "Create Account";
            this.RegisterAddbtn.Text = "Create";
            this.nice = cool;
            this.EntryMrk2.IsPassword = true;
            if (check == true)
            {
            this.Title = "Add Account";
            this.RegisterAddbtn.Text = "Add";
            }
		}

        private async void RegisterAddAccount(object sender, EventArgs e)
        {
            if (this.checkers)
            {
                try
                {
                    string[] line;
                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var files = System.IO.Directory.GetFiles(documents);
                    bool hit = false;
                    foreach(var file in files)
                    {


                    string nice2;
                    using (StreamReader sr = new StreamReader(file)) { nice2 = sr.ReadToEnd(); }

                    line = nice2.Split(',');

                    if (line[0].Contains("ID:")) { line[0] = line[0].Replace("ID:", ""); }

                        if (EntryMrk.Text == line[0] && EntryMrk2.Text == line[1])
                    {
                            hit = true;
                            if (line[2] != "trueB")
                        {
                            string lines = string.Format("ID:{0},{1},{2},{3}", line[0], line[1], "trueB","incorrect");
                            File.WriteAllText(file, lines);
                            nice.Update(files);

                            await ExtraPopups.ShowMessage("Added", "New Account Added", "Close", this, async () =>
                            {
                                    await this.Navigation.PopAsync();
                            });
                                
                                
                        }
                        else
                        {
                                throw new Exception("Account already added");
                        }
                    }
                    else if(EntryMrk.Text == line[0] && EntryMrk2.Text != line[1])
                        {
                            hit = true;
                            throw new Exception("Incorrect Password");
                        }
                    }
                    if (!hit)
                    {
                        throw new Exception("Account not found");
                    }
                }
                catch (Exception es)
                {
                    if(es != null){ await DisplayAlert("Error", es.Message, "Close");}
                    else { await DisplayAlert("Error", "Account not found", "Close"); }
                }
            }
            else
            {
                try
                {
                    if ((!EntryMrk.Text.Equals("") && !EntryMrk2.Equals("")))
                    {
                            User.StoreInfo(EntryMrk.Text, EntryMrk2.Text, this);
                    }
                    else
                    {
                        throw new Exception("Please enter both names.. ");
                    }
                }
                catch (Exception es)
                {
                    if (es != null) { await DisplayAlert("Error", es.Message, "Close"); }
                    else { await DisplayAlert("Error", "Please delete current account", "Close"); }
                }
            }
            
            

        }
    }
}