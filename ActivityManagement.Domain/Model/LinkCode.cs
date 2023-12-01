using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Domain.Model
{
    public class LinkCode : BaseEntity
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public DateTime Expire { get; set; }
    }
}
