using ActivityManagement.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ActivityManagement.Domain.Model
{
    public class Activity : BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double TimeSpent { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ActivityUser User { get; set; } 
    }
}
