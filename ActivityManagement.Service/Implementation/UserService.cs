using ActivityManagement.Domain.Identity;
using ActivityManagement.Repository.Inteface;
using ActivityManagement.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityManagement.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool CheckAdminUserInDatabase()
        {
            List<ActivityUser> users = userRepository.GetAll().ToList();
            return users.Exists(u => u.FirstName.Equals("Admin") && u.LastName.Equals("Admin"));
        }
    }
}
