using ActivityManagement.Domain.DTO;
using ActivityManagement.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityManagement.Service.Interface
{
    public interface IActivityService
    {
        void CreateActivity(ActivityDto activityDto, string userId);
        List<Activity> GetAllActivities(string userId);
        List<Activity> GetActivitiesByTimeInterval(string userId, DateTime from, DateTime to);
    }
}
