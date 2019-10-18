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
        public async Task TestLoginDetailsTestNoInputAsync()// Test initial values after login
        {
            GreetPageViewModel test = new GreetPageViewModel();
            test._UserModel.Username = "";// Input nothing
            test._UserModel.Password = "";
            var msg = await Assert.ThrowsAsync<Exception>(async () => await test.LoginCommandDatabase());
            Assert.Equal("Please input both Username and Password", msg.Message);
        }

        [Fact]
        public async Task testaccountnotfoundAsync()
        {
            GreetPageViewModel test = new GreetPageViewModel();
            test._UserModel.Username = "9999999";// Input invalid name
            test._UserModel.Password = "9999999";
            var msg = await Assert.ThrowsAsync<Exception>(async () => await test.LoginCommandDatabase());
            Assert.Equal("No account found", msg.Message);
        }

    }
}

