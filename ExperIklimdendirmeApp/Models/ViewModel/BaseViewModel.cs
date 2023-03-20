using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExperIklimdendirmeApp.Models.ViewModel
{
    public class BaseViewModel
    {
        public int sonuc { get; set; }
        public string message { get; set; }

        public List<CalendarEventsViewModel> eventList { get; set; }
    }
}