using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker
{
    
    public class logged
    {
        public bool TasksRun { get; set; }
        /*
         * Encapsulation class to set a boolean true and false, initialise as false
         * This is to tell all running async tasks to stop.
         */
        public logged()
        {
           this.TasksRun = true;
        }
    }
}
