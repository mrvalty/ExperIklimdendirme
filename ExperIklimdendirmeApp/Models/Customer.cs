using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExperIklimdendirmeApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string FaultReason { get; set; }
        public string UsedMaterial { get; set; }
        public string Explanation { get; set; }
        public int IsActive { get; set; }
    }
}