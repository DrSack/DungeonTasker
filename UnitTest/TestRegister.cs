using DungeonTasker;
using DungeonTasker.Models;
using DungeonTasker.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class TestRegister
    {
        [Fact]
        public void TestRegisterDetails()
        {
            RegisterViewModel test = new RegisterViewModel();
            string user1 = test.Username = "Username";
            string pass1 = test.Password = "Password";

            Assert.Equal("Username", user1);//Test values
            Assert.Equal("Password", pass1);//Test values
        }

        [Fact]
        public async Task TestRegisterCreateAccountError()
        {
            RegisterViewModel test = new RegisterViewModel();
            var msg = await Assert.ThrowsAsync<Exception>(async () => await test.RegisterAddAccountFIREBASE());
            Assert.Equal("Please enter all credentials... ", msg.Message);
        }

        [Fact]
        public void TestRegisterCheckValidity()
        {
            RegisterViewModel test = new RegisterViewModel();
            test.Username = "Username";
            test.Password = "Password";
            Assert.True(UserModel.checkinfo(test.Username,test.Password));
        }

        [Fact]
        public void TestRegisterCheckIncorrectValidity()
        {
            RegisterViewModel test = new RegisterViewModel();
            test.Username = "";
            test.Password = "";
            Assert.False(UserModel.checkinfo(test.Username, test.Password));
        }
    }
}
