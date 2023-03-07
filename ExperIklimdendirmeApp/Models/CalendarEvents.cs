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
        public string start { get; set; }
        public string end { get; set; }
        public string color { get; set; }
        public bool allDay { get; set; }
        public int IsActive { get; set; }
    }
}