using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VANTracker.DataModels.Models
{
    public class Driver : UserBase
    {
        public string RouteNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
