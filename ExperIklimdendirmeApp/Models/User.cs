﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExperIklimdendirmeApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
    }
}