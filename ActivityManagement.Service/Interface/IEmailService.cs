using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Service.Interface
{
    public interface IEmailService
    {
        void Send(string toEmail, string dateFrom, string dateTo, string userId);
    }
}
