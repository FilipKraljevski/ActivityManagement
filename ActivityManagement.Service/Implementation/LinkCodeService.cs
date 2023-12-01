using ActivityManagement.Domain.DTO;
using ActivityManagement.Domain.Model;
using ActivityManagement.Repository.Inteface;
using ActivityManagement.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityManagement.Service.Implementation
{
    public class LinkCodeService : ILinkCodeService
    {
        private readonly IRepository<LinkCode> _linkCodeRepository;

        public LinkCodeService(IRepository<LinkCode> linkCodeRepository)
        {
            _linkCodeRepository = linkCodeRepository;
        }

        public LinkCode Create(string email, string userId, string from, string to)
        {
            LinkCode linkCode = new LinkCode
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserId = userId,
                DateFrom = from,
                DateTo = to,
                Expire = DateTime.Now.AddDays(2)
            };
            _linkCodeRepository.Insert(linkCode);
            return linkCode;
        }

        public LinkCode CheckLinkCode(LinkCodeDto linkCodeDto)
        {
            List<LinkCode> linkCodes = _linkCodeRepository.GetAll().ToList();
            return linkCodes.Find(lc => lc.Email.Equals(linkCodeDto.Email) &&
                lc.Id.ToString().Equals(linkCodeDto.Code) && lc.Expire >= DateTime.Now);
        }
    }
}
