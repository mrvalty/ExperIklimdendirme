using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Windows.Media.Imaging;

namespace ExperIklimdendirmeApp.Models
{
    public class QRCustomer
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string QRName { get; set; }
        public string QRImage { get; set; }

    }
}