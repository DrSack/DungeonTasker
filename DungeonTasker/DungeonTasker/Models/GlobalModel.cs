using DungeonTasker.FirebaseData;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{
    public static class Globals
    {
        public static FirebaseObject<LoginDetails> LOGGED; // Modifiable
        public static FirebaseClient CLIENT;
    }
}
