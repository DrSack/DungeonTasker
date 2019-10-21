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
    public class UserModelTest
    {
        [Fact]
        public void CheckUserModelCharacterTask()
        {
            UserModel user = new UserModel();
            user.Character = "(0_0)";
            user.LocalLogin = Path.GetTempFileName();
            TasksViewModel test = new TasksViewModel(user, new TasksView());
            Assert.Equal(user.Character, test.Currentuser.Character);//Test for character check
            Assert.Equal("Current Tasks", test.Tasks);//Test for initiation
        }

        [Fact]
        public void CheckUserModelCharacterShop()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:\nCharacters:");
            InventoryItemsModel info = new InventoryItemsModel(tempFile);
            UserModel user = new UserModel();

            user.Character = "(0_0)";

            ShopViewModel test = new ShopViewModel(new ShopModel(info), new ItemInfoModel(info), new WeaponInfoModel(info), new CharacterInfoModel(info, tempFile), user, true);
            Assert.Equal(user.Character, test.Character);//Test for character check
        }

        [Fact]
        public void CheckInitialValues()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "Weapons:IronDagger,IronBow,\nKeys:0\nGold:500\nEquipped:IronDagger\nItems:\nCharacters:");
            InventoryItemsModel info = new InventoryItemsModel(tempFile);
            UserModel user = new UserModel();

            user.Character = "(0_0)";

            ShopViewModel test = new ShopViewModel(new ShopModel(info), new ItemInfoModel(info), new WeaponInfoModel(info), new CharacterInfoModel(info,tempFile),user, true);
            Assert.Equal(user.Character, test.Character);//Test for character check
            Assert.Equal("500", test.Gold);//Test for character check
            Assert.Equal("0", test.Keys);//Test for character check
        }
    }
}
