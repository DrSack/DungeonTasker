using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace DungeonTasker.Models
{
    class Animations
    {
        /*
         * This method is responsible for creating an animation that closes that particular stack layout full
         * 
         * Param 
         * layout: the stack layout from a particular page, 
         * name: the name of the specific animation, 
         * frames: the amount of frames for the animation to play, 
         * milisecs: how long the animation will last for
         * 
         * Returns NULL
         */
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
