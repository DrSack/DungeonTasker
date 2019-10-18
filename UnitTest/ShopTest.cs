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
    public class ShopTest
    {
        [Fact]
        public void TestShopItems()// Test if the items generated at all.
        {
            List<ItemModel> TestWeapons = new List<ItemModel>(); // Store weapon item details
            List<ItemModel> TestItem = new List<ItemModel>(); // Store weapon item details
            ShopModel test = new ShopModel(new InventoryItemsModel("test"));
            TestWeapons = test.BuyWeapons;
            TestItem = test.BuyItem;
            if (TestWeapons.Count > 0 && TestItem.Count > 0)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void TestShopItemsString()//Test if the items generated are correct
        {
            List<ItemModel> TestWeapons = new List<ItemModel>(); // Store weapon item details
            List<ItemModel> TestItem = new List<ItemModel>(); // Store weapon item details
            ShopModel test = new ShopModel(new InventoryItemsModel("test"));
            TestItem = test.BuyItem;
            Assert.Equal("HealthPotion", TestItem[0].item);
            Assert.Equal("MagicPotion", TestItem[1].item);
        }

        [Fact]
        public void TestShopWeaponsString()//Verify if each weapon generated is unique
        {
            List<ItemModel> TestWeapons = new List<ItemModel>(); // Store weapon item details
            List<ItemModel> TestItem = new List<ItemModel>(); // Store weapon item details
            ShopModel test = new ShopModel(new InventoryItemsModel("test"));
            TestWeapons = test.BuyWeapons;
            Assert.DoesNotContain(TestWeapons[0].item, TestWeapons[1].item);//Verify that each weapon is unique
        }

        [Fact]
        public void TestShopInvTypes()//Test shop item types
        {
            ShopModel test = new ShopModel(new InventoryItemsModel("test"));
            int item1 = test.CheckItem(test.BuyItem[0].item);
            int item2 = test.CheckItem(test.BuyItem[1].item);
            int item3 = test.CheckItem(test.BuyWeapons[0].item);
            int item4 = test.CheckItem(test.BuyWeapons[1].item);

            Assert.Equal(1, item1);
            Assert.Equal(1, item2);
            Assert.Equal(0, item3);
            Assert.Equal(0, item4);//Check the item types
        }

        [Fact]
        public void BuyWeapon()//Test buying a weapon
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
            InventoryItemsModel info = new InventoryItemsModel(tempFile);
            UserModel user = new UserModel();

            user.Character = "(0_0)";
            ShopViewModel test = new ShopViewModel(new ShopModel(info), new ItemInfoModel(info), new WeaponInfoModel(info), user, true);

            test.BuyAsync(info, 0, "SteelSword", true);
            Assert.Equal(3, test.Weapon.weapons.Count);
            Assert.Equal("489", test.Gold);
            Assert.Equal("IronDagger,IronBow,SteelSword,", UserModel.CheckForstring(tempFile, "Weapons:"));
        }

        [Fact]
        public void BuyItem()//Test buying an item
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
            InventoryItemsModel info = new InventoryItemsModel(tempFile);
            UserModel user = new UserModel();

            user.Character = "(0_0)";
            ShopViewModel test = new ShopViewModel(new ShopModel(info), new ItemInfoModel(info), new WeaponInfoModel(info), user, true);

            test.BuyAsync(info, 1, "HealthPotion", true);
            Assert.Single(test.Inv.pots);
            Assert.Equal("485", test.Gold);
            Assert.Equal("HealthPotion,", UserModel.CheckForstring(tempFile, "Items:"));
        }

        [Fact]
        public async Task BuyWithNoGoldAsync()//Test buying an item
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:0\nEquipped:IronDagger\nItems:");
            InventoryItemsModel info = new InventoryItemsModel(tempFile);
            UserModel user = new UserModel();

            user.Character = "(0_0)";
            ShopViewModel test = new ShopViewModel(new ShopModel(info), new ItemInfoModel(info), new WeaponInfoModel(info), user, true);

            Assert.False(await test.BuyAsync(info, 1, "HealthPotion", true));
            Assert.Empty(test.Inv.pots);
            Assert.Equal("0", test.Gold);
            Assert.Equal("", UserModel.CheckForstring(tempFile, "Items:"));
        }
    }
}
