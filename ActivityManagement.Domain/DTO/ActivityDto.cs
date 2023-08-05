using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Domain.DTO
{
    public class ActivityDto
    {
        public DateTime Date { get; set; }
        public double TimeSpent { get; set; }
        public string Description { get; set; }
    }
}
