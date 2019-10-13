using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace DungeonTasker.Models
{
    public class ItemModel:INotifyPropertyChanged
    {
        private string Notes;
        public string notes 
        { 
            get 
            { 
                return Notes; 
            } 

            set 
            { 
                Notes = value;
                OnPropertyChanged(this, "notes");
            } 
        }

        private bool IsEnabled;
        public bool isenabled
        {
            get
            {
                return IsEnabled;
            }

            set
            {
                IsEnabled = value;
                OnPropertyChanged(this, "isenabled");
            }
        }

        private bool IsVisible;
        public bool isvisible
        {
            get
            {
                return IsVisible;
            }

            set
            {
                IsVisible = value;
                OnPropertyChanged(this, "isvisible");
            }
        }

        private bool FrameOn;
        public bool frameOn
        {
            get
            {
                return FrameOn;
            }

            set
            {
                FrameOn = value;
                OnPropertyChanged(this, "frameOn");
            }
        }

        private bool FrameVis;
        public bool frameVis
        {
            get
            {
                return FrameVis;
            }

            set
            {
                FrameVis = value;
                OnPropertyChanged(this, "frameVis");
            }
        }

        private TextAlignment TextHoz;
        public TextAlignment texthoz
        {
            get
            {
                return TextHoz;
            }

            set
            {
                TextHoz = value;
                OnPropertyChanged(this, " texthoz");
            }
        }

        private bool IsVisItem;
        public bool isvisItem
        {
            get
            {
                return IsVisItem;
            }

            set
            {
                IsVisItem = value;
                OnPropertyChanged(this, "isvisItem");
            }
        }

        private bool IsEnbItem;
        public bool isenbItem
        {
            get
            {
                return IsEnbItem;
            }

            set
            {
                IsEnbItem = value;
                OnPropertyChanged(this, "isenbItem");
            }
        }

        private LayoutOptions HozOpNotes;
        public LayoutOptions hozopnotes
        {
            get
            {
                return HozOpNotes;
            }

            set
            {
                HozOpNotes = value;
                OnPropertyChanged(this, "hozopnotes");
            }
        }

        private string Buy;
        public string buy
        {
            get
            {
                return Buy;
            }

            set
            {
                Buy = value;
                OnPropertyChanged(this, "buy");
            }
        }
        public bool titleTrue { get; set; }
        public bool titleVis { get; set; }
        public string Title { get; set; }
        public string item { get; set; }
        public int count { get; set; }
        
        /*
         * Encapsulation class for Weapon items
         * Param
         * @weapon is the weapon string parsed
         * Returns Nothing
         */
        public ItemModel(string weapon)
        {
            this.item = weapon;
        }

        public bool countCheck()
        {
            if (count == 1)
            {
                count = 0;
                return true;
            }
            else
            {
                count++;
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // OnPropertyChanged will raise the PropertyChanged event passing the
        // source property that is being updated.
        private void OnPropertyChanged(object sender, string propertyName)
        {

            if (this.PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
