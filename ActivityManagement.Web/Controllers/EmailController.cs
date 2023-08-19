using ActivityManagement.Domain.Model;
using ActivityManagement.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ActivityManagement.Web.Controllers
{
    [AllowAnonymous]
    public class EmailController : Controller
    {
        private readonly IActivityService _activityService;

        public EmailController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        public IActionResult Index(string userId, string? from, string? to)
        {
            List<Activity> activities;
            if (from == null || to == null)
            {
                activities = _activityService.GetAllActivities(userId);
            }
            else
            {
                activities = _activityService.GetActivitiesByTimeInterval(userId, DateTime.Parse(from), 
                    DateTime.Parse(to));
            }
            return View(activities);
        }
    }
}
