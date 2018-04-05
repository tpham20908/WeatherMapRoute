using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMRApp.Models
{
    public class Stop
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return "Lat: " + Lat + ", Lng: " + Lng + ", Address: " + Address;
        }
    }
}
