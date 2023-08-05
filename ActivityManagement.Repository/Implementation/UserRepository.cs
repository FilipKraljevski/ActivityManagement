using ActivityManagement.Domain.Identity;
using ActivityManagement.Repository.Inteface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityManagement.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<ActivityUser> entities;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<ActivityUser>();
        }
        public IEnumerable<ActivityUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public ActivityUser Get(string id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(ActivityUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity is null");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(ActivityUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity is null");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(ActivityUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity is null");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
