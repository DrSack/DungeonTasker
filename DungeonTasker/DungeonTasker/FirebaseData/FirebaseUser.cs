using DungeonTasker.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DungeonTasker.FirebaseData
{
    public class FirebaseUser
    {
        public string Username {get;set;}
        public FirebaseClient Client;
        public FirebaseObject<ItemDetails> UserItems;
        public FirebaseObject<LoginDetails> UserLogin;
        public FirebaseObject<StatDetails>  UserStats;
        public FirebaseObject<TimerDetails> UserTimes;
        public FirebaseUser()
        {
            Connect();
        }

        public async Task<int> Login(string Username, string Password, bool bypass = false)
        {
            try
            {
                if (!UserModel.checkinfo(Username, Password))
                {
                    return 2;
                }
                var details = (await Client
                .Child(string.Format("{0}Login", Username))
                .OnceAsync<LoginDetails>()).Where(item => item.Object.Username == Username).FirstOrDefault();
                if ((details.Object.Username == Username && details.Object.Password == Password) || bypass)
                {
                    if(details.Object.Logged == "False" || bypass)
                    {
                    var Stats = (await Client
                    .Child(string.Format("{0}Stats", Username))
                    .OnceAsync<StatDetails>()).FirstOrDefault();
                    var Items = (await Client
                    .Child(string.Format("{0}Inv", Username))
                    .OnceAsync<ItemDetails>()).FirstOrDefault();
                    var Timer = (await Client
                    .Child(string.Format("{0}Timer", Username))
                    .OnceAsync<TimerDetails>()).FirstOrDefault();

                    UserLogin = details; Username = details.Object.Username;
                    UserItems = Items;
                    UserStats = Stats;
                    UserTimes = Timer;
                    return 0;
                    }
                    else
                    {
                        return 4;
                    }
                }
                    if (details.Object.Username == Username && details.Object.Password != Password)
                {
                    return 3;
                }
            }
            catch(Exception es)
            {
                if(es is NullReferenceException)
                {
                    return 5;
                }
                if(es is FirebaseException)
                {
                    return 1;
                }
            }
            return 1;
        }
        private void Connect()
        {
            try
            {
                var auth = "NAVcHJeSSFjH8QaQ2UqwNku1JyDLIWqbb8ITu65u";
                Client = new FirebaseClient("https://dungeontasker.firebaseio.com/",
                    new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(auth)
                    });
            }
            catch
            {
                Application.Current.MainPage.DisplayAlert("Error", "Can't connect to internet", "Close");
            }
        }

        public async Task Register(string Fullname, string Username, string Password, INavigation Navigation)
        {
            if(await ValidateAsync(Username))
            {
                CreateData(Fullname, Username, Password);//If this passes through the check create new data.
                await Application.Current.MainPage.DisplayAlert("Success", string.Format("Welcome {0}", Fullname), "Close");
                await Navigation.PopModalAsync();
            }
            else
            {
                return;
            }
        }

        public async void CreateData(string Fullname, string Username, string Password)
        {
            await Client
                .Child(string.Format(Username + "Login"))
                .PostAsync(new LoginDetails()
                {
                    Updated = DateTime.Now.ToString(),
                    FullName = Fullname,
                    Username = Username,
                    Password = Password,
                    Character = "(ง’̀-‘́)ง",
                    Logged = "False",
                    Tutorial = "True"
                });

            await Client
                .Child(string.Format(Username + "Stats"))
                .PostAsync(new StatDetails()
                {
                    HEALTH = "100",
                    MANA = "40",
                    LEVEL = "1",
                    EXP = "0"
                });

            await Client
                .Child(string.Format(Username + "Inv"))
                .PostAsync(new ItemDetails()
                {
                    Weapons = "IronDagger,IronBow,",
                    Keys = "0",
                    Gold = "500",
                    Equipped = "IronDagger",
                    Items = ""
                });

            await Client
                .Child(string.Format(Username + "Timer"))
                .PostAsync(new TimerDetails()
                {
                    Timer = ""
                });
        }

        public async Task<bool> ValidateAsync(string Username)
        {
            try
            {
                var details = (await Client
                .Child(string.Format("{0}Login", Username))
                .OnceAsync<LoginDetails>()).Where(item => item.Object.Username == Username).FirstOrDefault();
                if (details.Object.Username == Username)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Account already exists", "Close");
                    return false;
                }
            }
            catch(Exception es)
            {
                if(es is NullReferenceException)
                {
                    return true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Can't connect to server", "Close");
                    return false;
                }
            }
            return true;
        }
    }
}
