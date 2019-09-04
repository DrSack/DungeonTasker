using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonTasker.Models
{

    class StringtoInt
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
        public DateTimeKind kind { get; set; }
        public StringtoInt(string year, string month, string day, string hour, string minute, string second, string kind)
        {
            this.year = Int32.Parse(year);
            this.month = Int32.Parse(month);
            this.day = Int32.Parse(day);
            this.hour = Int32.Parse(hour);
            this.minute = Int32.Parse(minute);
            this.second = Int32.Parse(second);
            this.kind = new DateTimeKind();
        }
    }
}
