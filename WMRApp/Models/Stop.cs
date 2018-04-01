﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMRApp.Models
{
    class Stop
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }

        public string toString()
        {
            return "Lat: " + Lat + ", Lng: " + Lng + ", Address: " + Address;
        }
    }
}
