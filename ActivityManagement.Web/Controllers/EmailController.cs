using ActivityManagement.Domain.DTO;
using ActivityManagement.Domain.Model;
using ActivityManagement.Repository;
using ActivityManagement.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;

namespace ActivityManagement.Web.Controllers
{
    [AllowAnonymous]
    public class EmailController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly ILinkCodeService _linkCodeService;

        public EmailController(IActivityService activityService, ILinkCodeService linkCodeService)
        {
            _activityService = activityService;
            _linkCodeService = linkCodeService;
        }

        public IActionResult Index(int? pageNumber)
        {
            string check = HttpContext.Session.GetString("Validate");
            if (check == null || !check.Equals("Yes"))
            {
                return RedirectToAction("CodeInput");
            }
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

        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult CodeInput()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CodeInput(LinkCodeDto linkCodeDto)
        {
            LinkCode linkCode = _linkCodeService.CheckLinkCode(linkCodeDto);
            if (linkCode != null)
            {
                HttpContext.Session.SetString("UserId", linkCode.UserId);
                if (linkCode.DateFrom != null && linkCode.DateTo != null) 
                { 
                    HttpContext.Session.SetString("DateFromEmail", linkCode.DateFrom);
                    HttpContext.Session.SetString("DateToEmail", linkCode.DateTo);
                }
                HttpContext.Session.SetString("Validate", "Yes");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("message", "Invalid email or code or the link has expired");
            return View(linkCodeDto);
        }
    }
}
