﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExperIklimdendirmeApp.Models.ViewModel
{
    public class CalendarEventsViewModel : BaseViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int calendarid { get; set; }
        public int customerid { get; set; }

    }
}