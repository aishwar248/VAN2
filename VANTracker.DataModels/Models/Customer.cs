using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VANTracker.DataModels.Models
{
    public class Customer : UserBase
    {
        public string RouteNumber { get; set; }
    }
}
