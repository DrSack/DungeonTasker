using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using DungeonTasker;

    public class TimerUpdatecs
    {
     public int time { get; set; }
    public string type { get; set; }

    public Guid InstanceID { get; private set; }
        public TimerUpdatecs(int time, string name)
        {
          this.time = time;
          this.type = name;
        this.InstanceID = Guid.NewGuid();
        }

        
    }

