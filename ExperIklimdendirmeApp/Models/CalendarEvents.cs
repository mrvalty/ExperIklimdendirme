using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExperIklimdendirmeApp.Models
{
    public class CalendarEvents
    {
        public int id { get; set; }
        public int Customerid { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}