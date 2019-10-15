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
    
    public class TestLogin
    {

        [Fact]
        public void TestLoginDetails()
        {
            GreetPageViewModel test = new GreetPageViewModel();
            string user1 = test._UserModel.Username = "Username";
            string pass1 = test._UserModel.Password = "Password";

            Assert.Equal("Username", user1);//Test values
            Assert.Equal("Password", pass1);//Test values
        }

        [Fact]
        public void TestLoginDetailsLogging()// Test initial values after login
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Users");

            string Character;
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//Get folder path
            
            GreetPageViewModel test = new GreetPageViewModel();
            UserModel.StoreInfo("a", "b", "c", new RegisterView(false),true);
            test._UserModel.Username = "a";
            test._UserModel.Password = "b";
            var filename = Path.Combine(documents + "/Users", test._UserModel.Username + "Login.dt");// File name is equal to the username+login.dt
            var Timer = Path.Combine(documents + "/Users", test._UserModel.Username + "Timer.dt");

            test.LoginCommand(true);
            Assert.Equal("a", test._UserModel.Username);//Test values
            Assert.Equal("b", test._UserModel.Password);//Test values
            Assert.Equal("c", test._UserModel.FullName);//Test values
            Assert.Equal("true", test._UserModel.Logged);//Test values
            Assert.Equal(filename, test._UserModel.file);
            Assert.Equal(Timer, test._UserModel.timer);
            Assert.Equal("(ง’̀-‘́)ง",test._UserModel.Character);//This is null as folder paths dont exit

            var Items = Path.Combine(documents + "/Users", "aInv.dt");
            var Stats = Path.Combine(documents + "/Users", "aStats.dt");

            File.Delete(filename);
            File.Delete(Items);
            File.Delete(Stats);
            File.Delete(Timer);
            Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Users");
        }

        [Fact]
        public void TestAccountNotFound()
        {
            string Character;

            GreetPageViewModel test = new GreetPageViewModel();

            test._UserModel.Username = "";
            test._UserModel.Password = "";

            try
            {
                test.LoginCommand(true);
            }
            catch(Exception msg)
            {
                Assert.Equal("Account not found", msg.Message);
            }

        }

    }
}

