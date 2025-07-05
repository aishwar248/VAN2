using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VANTracker.DataModels.Models
{
    public abstract class UserBase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
