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

        public void Create(string email, string code, string url)
        {
            LinkCode linkCode = new LinkCode
            {
                Id = Guid.NewGuid(),
                Email = email,
                Code = code,
                Link = url,
                Expire = DateTime.Now.AddDays(2)
            };
            _linkCodeRepository.Insert(linkCode);
        }

        public bool CheckLinkCode(LinkCodeDto linkCodeDto, string link)
        {
            List<LinkCode> linkCodes = _linkCodeRepository.GetAll().ToList();
            return linkCodes.Exists(lc => lc.Email.Equals(linkCodeDto.Email) &&
                lc.Code.Equals(linkCodeDto.Code) && lc.Link.Equals(link) && lc.Expire >= DateTime.Now);
        }
    }
}
