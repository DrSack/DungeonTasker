using DungeonTasker;
using DungeonTasker.Models;
using DungeonTasker.ViewModel;
using DungeonTasker.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xunit;

namespace UnitTest
{
    public class WeaponTest
    {
        //[Fact]
        //public void TestWeaponInitiate()
        //{
        //    string tempFile = Path.GetTempFileName();//create a temporary file
        //    File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
        //    WeaponInfoModel test = new WeaponInfoModel(new InventoryItemsModel(tempFile));
            
        //    Assert.Equal("IronDagger", test.weapons[0].item);
        //    Assert.Equal("IronBow", test.weapons[1].item);//Test if method parses to list.
        //}

        //[Fact]
        //public void TestWeaponInfoMinimum()
        //{
        //    string tempFile = Path.GetTempFileName();//create a temporary file
        //    File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
        //    WeaponInfoModel test = new WeaponInfoModel(new InventoryItemsModel(tempFile));

        //    string IronDagger = test.weapons[0].item;
        //    string IronBow = test.weapons[1].item;

        //    int minimumDagger = WeaponInfoModel.ObtainWeaponInfo(IronDagger, true);
        //    int minimumBow = WeaponInfoModel.ObtainWeaponInfo(IronBow, true);

        //    Assert.Equal(2, minimumDagger);
        //    Assert.Equal(2, minimumBow);//Test if minimum values
        //}

        //[Fact]
        //public void TestWeaponInfoMaximum()
        //{
        //    string tempFile = Path.GetTempFileName();//create a temporary file
        //    File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
        //    WeaponInfoModel test = new WeaponInfoModel(new InventoryItemsModel(tempFile));

        //    string IronDagger = test.weapons[0].item;
        //    string IronBow = test.weapons[1].item;

        //    int MaximumDagger = WeaponInfoModel.ObtainWeaponInfo(IronDagger, false);
        //    int MaximumBow = WeaponInfoModel.ObtainWeaponInfo(IronBow, false);

        //    Assert.Equal(5, MaximumDagger);
        //    Assert.Equal(6, MaximumBow);//Test Max hit values
        //}

        //[Fact]
        //public void TestWeaponEquipReal()
        //{
        //    string tempFile = Path.GetTempFileName();//create a temporary file
        //    File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
        //    WeaponInfoModel test = new WeaponInfoModel(new InventoryItemsModel(tempFile));

        //    Assert.True(test.SetWeapon(new ContentPage(), UserModel.CheckForstring(tempFile, "Equipped:")));
        //    Assert.Equal(5, test.Maximumdmg);
        //    Assert.Equal(2, test.Minimumdmg);
        //    Assert.Equal("IronDagger", test.EquippedWeapon);
        //}

        //[Fact]
        //public void TestWeaponEquipNotEquipped()
        //{
        //    string tempFile = Path.GetTempFileName();//create a temporary file
        //    File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
        //    WeaponInfoModel test = new WeaponInfoModel(new InventoryItemsModel(tempFile));

        //    Assert.True(test.SetWeapon(new ContentPage(), "Not Equipped"));
        //    Assert.Equal(2, test.Maximumdmg);
        //    Assert.Equal(0, test.Minimumdmg);
        //    Assert.Equal("Not Equipped", test.EquippedWeapon);
        //}

        //[Fact]
        //public void TestWeaponEquipSameWeapon()
        //{
        //    string tempFile = Path.GetTempFileName();//create a temporary file
        //    File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
        //    WeaponInfoModel test = new WeaponInfoModel(new InventoryItemsModel(tempFile));
        //    test.SetWeapon(new ContentPage(), UserModel.CheckForstring(tempFile, "Equipped:"));
        //    Assert.False(test.SetWeapon(new ContentPage(), UserModel.CheckForstring(tempFile, "Equipped:")));//Test if false meaning same weapon cant be equipped
        //}

        //[Fact]
        //public void TestWeaponGoldValues()
        //{
        //    string tempFile = Path.GetTempFileName();//create a temporary file
        //    File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:");
        //    WeaponInfoModel test = new WeaponInfoModel(new InventoryItemsModel(tempFile));

        //    int dagger = WeaponInfoModel.ObtainWeaponValue(test.weapons[0].item);
        //    int bow = WeaponInfoModel.ObtainWeaponValue(test.weapons[1].item);
        //    Assert.Equal(4,dagger);//Test weapon value
        //    Assert.Equal(5,bow);//Test weapon value
        //}

        //[Fact]
        //public void TestWeaponGoldValueInvalid()
        //{
        //    int test = WeaponInfoModel.ObtainWeaponValue("FlamingRocketGrenadeLauncherSwordBowNukeBomb");
        //    Assert.Equal(0, test);//if invalid return 0.
        //}
    }
}
