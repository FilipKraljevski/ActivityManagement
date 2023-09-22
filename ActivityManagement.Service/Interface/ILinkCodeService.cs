using ActivityManagement.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Service.Interface
{
    public interface ILinkCodeService
    {
        void Create(string email, string code, string url);
        bool CheckLinkCode(LinkCodeDto linkCodeDto, string link);
    }
}
