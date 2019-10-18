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
    public class StatsTest
    {

        [Fact]
        public void TestUserStatsInitiate()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "HEALTH:100\nMANA:40\nLEVEL:1\nEXP:0");//Create values
            StatsModel test = new StatsModel(tempFile);
            Assert.Equal(100, test.Health);
            Assert.Equal(40, test.Mana);
            Assert.Equal(1, test.Level);
            Assert.Equal(0, test.Experience);//Obtain and check.
        }

        [Fact]
        public async Task TestUserStatsExpCheckAsync()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "HEALTH:100\nMANA:40\nLEVEL:1\nEXP:36");//Create values
            StatsModel test = new StatsModel(tempFile);
            Assert.True(await test.StatsCheckAsync());//Check if its time for a levelup
        }

        [Fact]
        public async Task TestUserStatsExpCheckFalseAsync()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "HEALTH:100\nMANA:40\nLEVEL:1\nEXP:0");//Create values
            StatsModel test = new StatsModel(tempFile);
            Assert.False(await test.StatsCheckAsync());//Check if not ready to lvl up
        }

        [Fact]
        public void TestUserStatsExpLeft()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "HEALTH:100\nMANA:40\nLEVEL:1\nEXP:0");//Create values
            StatsModel test = new StatsModel(tempFile);
            Assert.Equal("36", test.ExpLeft());//Check the remaining exp left
        }

        [Fact]
        public void TestUserStatsExpLeftMAX()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "HEALTH:100\nMANA:40\nLEVEL:1\nEXP:36");//Create values
            StatsModel test = new StatsModel(tempFile);
            Assert.Equal("LEVEL UP", test.ExpLeft());//Check if user has leveled up
        }

        [Fact]
        public void TestUserStatsExpENTER()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "HEALTH:100\nMANA:40\nLEVEL:1\nEXP:0");//Create values
            StatsModel test = new StatsModel(tempFile);
            test.ExpEnterAsync(10);
            Assert.Equal(10, test.Experience);// Check if total exp is 10.
        }

        [Fact]
        public async Task TestUserStatsExpCOMBOAsync()
        {
            string tempFile = Path.GetTempFileName();//create a temporary file
            File.WriteAllText(tempFile, "HEALTH:100\nMANA:40\nLEVEL:1\nEXP:0");//Create values
            StatsModel test = new StatsModel(tempFile);
            test.ExpEnterAsync(36);
            Assert.Equal(36, test.Experience);// Check if total exp is 10.
            Assert.Equal("LEVEL UP", test.ExpLeft());//Check if user has leveled up
            Assert.True(await test.StatsCheckAsync());//Check if its time for a levelup

            Assert.Equal(2, test.Level);//Check if level has updated
            Assert.Equal(120, test.Health);//Check if Health has updated
            Assert.Equal(50, test.Mana);//Check if Mana has updated
        }
    }
}
