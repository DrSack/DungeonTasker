using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using DungeonTasker;

    public class TimerUpdatecs
    {
    public DateTime T { get; set; }
    public TimeSpan R { get; set; }
    public string type { get; set; }

        /*
         * This is the Contructor, this is an encapsulation class that stores DateTimer, TimeSpan and string information.
         * PARAM
         * trig: the end date time
         * Rem: the remaining time
         * name: the title of the task
         * 
         * RETURNS Nothing
         */
    public TimerUpdatecs(DateTime trig, TimeSpan Rem, string name)
        {
          this.T = trig;
          this.R = Rem;
          this.type = name;
        }

        
    }

