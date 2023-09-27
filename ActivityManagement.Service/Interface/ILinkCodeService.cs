using ActivityManagement.Domain.DTO;
using ActivityManagement.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Service.Interface
{
    public interface ILinkCodeService
    {
        void Create(string email, string code, string userId, string from, string to);
        LinkCode CheckLinkCode(LinkCodeDto linkCodeDto);
    }
}
