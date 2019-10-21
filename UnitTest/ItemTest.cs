using DungeonTasker;
using DungeonTasker.FirebaseData;
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
        public void TestItemsCreation()// Test if the items generated at all.
        {
            FirebaseUser client = new FirebaseUser();
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));
            Assert.Empty(test.pots);//Default value is nothing
        }
        [Fact]
        public void TestItemsInfo()// Test if constructor gets file items
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:HealthPotion,MagicPotion");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));

            Assert.Equal("HealthPotion", test.pots[0].item);
            Assert.Equal("MagicPotion", test.pots[1].item);//Test if values appear
        }

        [Fact]
        public void TestItemInfoMinimum()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:HealthPotion,MagicPotion");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));

            string HealthPot = test.pots[0].item;
            string MagicPot = test.pots[1].item;

            int healthvalue = ItemInfoModel.ObtainItemInfo(HealthPot, true);
            int manavalue = ItemInfoModel.ObtainItemInfo(MagicPot, true);

            Assert.Equal(10, healthvalue);
            Assert.Equal(20, manavalue);//Test Minimum values
        }

        [Fact]
        public void TestItemInfoMaximum()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:HealthPotion,MagicPotion");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));

            string HealthPot = test.pots[0].item;
            string MagicPot = test.pots[1].item;

            int healthvalue = ItemInfoModel.ObtainItemInfo(HealthPot, false);
            int manavalue = ItemInfoModel.ObtainItemInfo(MagicPot, false);

            Assert.Equal(40, healthvalue);
            Assert.Equal(60, manavalue);//Test Maximum values
        }

        [Fact]
        public void TestItemInfoIncorrect()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:SICKONEPALL");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));

            string incorrect = test.pots[0].item;

            int value = ItemInfoModel.ObtainItemInfo(incorrect, false);
            int value2 = ItemInfoModel.ObtainItemInfo(incorrect, true);

            Assert.Equal(0, value);
            Assert.Equal(0, value2);//Test Wrong values
        }

        [Fact]
        public void TestItemInfoString()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:HealthPotion,MagicPotion");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));

            string HealthPot = test.pots[0].item;
            string MagicPot = test.pots[1].item;

            string healthextra = ItemInfoModel.ObtainItemString(HealthPot, false);
            string health = ItemInfoModel.ObtainItemString(HealthPot, true);
            string magicextra = ItemInfoModel.ObtainItemString(MagicPot, false);
            string magic = ItemInfoModel.ObtainItemString(MagicPot, true);

            Assert.Equal("Heal", health);
            Assert.Equal("HP", healthextra);
            Assert.Equal("Restore", magic);
            Assert.Equal("MP", magicextra);//Test string values
        }

        [Fact]
        public void TestItemInfoStringIncorrect()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:Cool");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));

            string Incorrect= test.pots[0].item;
            string wrong = ItemInfoModel.ObtainItemString(Incorrect, false);
            string wrongextra = ItemInfoModel.ObtainItemString(Incorrect, true);

            Assert.Null(wrongextra);
            Assert.Null(wrong);
        }

        [Fact]
        public void TestItemInfoValue()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:HealthPotion,MagicPotion");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));

            string HealthPot = test.pots[0].item;
            string MagicPot = test.pots[1].item;

            int healthvalue = ItemInfoModel.ObtainItemValue(HealthPot);
            int manavalue = ItemInfoModel.ObtainItemValue(MagicPot);

            Assert.Equal(15, healthvalue);
            Assert.Equal(20, manavalue);
        }

        [Fact]
        public void TestItemInfoValueIncorrect()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:SOMEWEAPON");
            ItemInfoModel test = new ItemInfoModel(new InventoryItemsModel(tempFile));

            string Incorrect = test.pots[0].item;
            int incorrectvalue = ItemInfoModel.ObtainItemValue(Incorrect);

            Assert.Equal(0, incorrectvalue);
        }
    }
}
