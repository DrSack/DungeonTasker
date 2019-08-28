using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace DungeonTasker.Models
{
    class Animations
    {

        Animations()
        {

        }

        public static void CloseStackLayout(StackLayout layout, string name, object frames, object milisecs)
        {

            uint uframe = (uint) (int) frames;
            uint umilisecs = (uint)(int)milisecs;
            var animate = new Animation(d => layout.HeightRequest = d, layout.Height, 0);
            animate.Commit(layout, name, uframe, umilisecs);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (layout.Height > 0)
            {
                if(stopwatch.ElapsedMilliseconds == umilisecs)
                {
                    break;
                }
            }
        }
    }
}
