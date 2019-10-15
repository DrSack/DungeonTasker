using DungeonTasker;
using DungeonTasker.Models;
using DungeonTasker.ViewModel;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class ItemTest
    {
        [Fact]
        public void TestShopItems()// Test if the items generated at all.
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));

            Assert.Empty(test.pots);//Default value is nothing
        }
    }
}
