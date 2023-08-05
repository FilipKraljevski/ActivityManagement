using ActivityManagement.Domain.DTO;
using ActivityManagement.Domain.Identity;
using ActivityManagement.Domain.Model;
using ActivityManagement.Repository.Inteface;
using ActivityManagement.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityManagement.Service.Implementation
{
    public class ActivityService : IActivityService
    {
        private readonly IRepository<Activity> _activityRepository;
        private readonly IUserRepository _userRepository;

        public ActivityService(IRepository<Activity> activityRepository, IUserRepository userRepository)
        {
            _activityRepository = activityRepository;
            _userRepository = userRepository;
        }

        public void CreateActivity(ActivityDto activityDto, string userId)
        {
            ActivityUser user = _userRepository.Get(userId);
            Activity activity = new Activity
            {
                Id = Guid.NewGuid(),
                Date = activityDto.Date,
                TimeSpent = activityDto.TimeSpent,
                Description = activityDto.Description,
                User = user
            };
            _activityRepository.Insert(activity);
        }

        public List<Activity> GetAllActivities(string userId)
        {
            ActivityUser user = _userRepository.Get(userId);
            List<Activity> activities = _activityRepository.GetAll().ToList();
            return activities.FindAll(a => a.User == user);
        }

        public List<Activity> GetActivitiesByTimeInterval(string userId, DateTime from, DateTime to)
        {
            List<Activity> activities = GetAllActivities(userId);
            return activities.FindAll(a => a.Date >= from && a.Date <= to);
        }
    }
}
