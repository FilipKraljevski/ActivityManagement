using ActivityManagement.Domain.Model;
using ActivityManagement.Repository;
using ActivityManagement.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IActionResult Index(int? pageNumber)
        {
            List<Activity> activities;
            string userId = HttpContext.Session.GetString("UserId");
            string from = HttpContext.Session.GetString("DateFromEmail");
            string to = HttpContext.Session.GetString("DateToEmail");
            if (from == null || to == null)
            {
                activities = _activityService.GetAllActivities(userId);
            }
            else
            {
                activities = _activityService.GetActivitiesByTimeInterval(userId, DateTime.Parse(from), 
                    DateTime.Parse(to));
            }
            return View(PaginatedList<Activity>.Create(activities.OrderBy(a => a.Date).ToList(), 
                pageNumber ?? 1, 5));
        }

        public IActionResult Save(string userId, string from, string to)
        {
            HttpContext.Session.SetString("UserId", userId);
            if (from != null && to != null)
            {
                HttpContext.Session.SetString("DateFromEmail", from);
                HttpContext.Session.SetString("DateToEmail", to);
            }
            return RedirectToAction("Index");
        }
    }
}
