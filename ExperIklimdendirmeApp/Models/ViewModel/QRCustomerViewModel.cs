using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ExperIklimdendirmeApp.Models.ViewModel
{
    public class QRCustomerViewModel
    {
        public int Id { get; set; }
        public Bitmap QrImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string FaultReason { get; set; }
        public string Explanation { get; set; }
        public string UsedMaterial { get; set; }


    }
}