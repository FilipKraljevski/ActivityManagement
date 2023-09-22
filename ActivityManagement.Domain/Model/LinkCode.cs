using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Domain.Model
{
    public class LinkCode : BaseEntity
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public DateTime Expire { get; set; }
    }
}
