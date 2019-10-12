using DungeonTasker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DungeonTasker.ViewModel
{
    public class StatViewModel
    {
        public ObservableCollection<ItemModel> Volumes { get; set; }

        private List<ItemModel> oof;
        private ItemInfoModel nice;
        public Command<ItemModel> Remove
        {
            get
            {
                return new Command<ItemModel>(delete =>
                {
                    Volumes[Volumes.IndexOf(delete)].notes = "Sold";
                    Volumes[Volumes.IndexOf(delete)].isenabled = false;
                    Volumes[Volumes.IndexOf(delete)].isvisible = false;
                });
            }
        }
        public StatViewModel(ItemInfoModel items)
        {
            Volumes = new ObservableCollection<ItemModel>();
            Volumes.Add(new ItemModel("Title") { Title = "Potions", frameOn = false, frameVis = false , titleTrue = true, titleVis = true});
            foreach (ItemModel item in items.pots)
            {
                Volumes.Add(new ItemModel(item.item)
                {
                    notes = string.Format("{0} {1}: {2} - {3} {4}",item.item,
                ItemInfoModel.ObtainItemString(item.item, true),
                ItemInfoModel.ObtainItemInfo(item.item, true),
                ItemInfoModel.ObtainItemInfo(item.item, false),
                ItemInfoModel.ObtainItemString(item.item, false)),
                item = item.item,
                isenabled = true,
                isvisible = true,
                frameOn = true,
                frameVis = true,
                titleTrue = false,
                titleVis = false,
                });
            }
            Volumes.Add(new ItemModel("Title") { Title = "Weapons", frameOn = false, frameVis = false, titleTrue = true, titleVis = true });

            this.nice = items;
        }
    }
}
