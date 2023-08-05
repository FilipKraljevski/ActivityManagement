using ActivityManagement.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Repository.Inteface
{
    public interface IUserRepository
    {
        IEnumerable<ActivityUser> GetAll();
        ActivityUser Get(string id);
        void Insert(ActivityUser entity);
        void Update(ActivityUser entity);
        void Delete(ActivityUser entity);
    }
}
