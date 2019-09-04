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

        public TimerUpdatecs(DateTime trig, TimeSpan Rem, string name)
        {
          this.T = trig;
          this.R = Rem;
          this.type = name;
        }

        
    }

