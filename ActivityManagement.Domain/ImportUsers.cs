using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Domain
{
    public class ImportUsers
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
