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

        public IActionResult Save(string userId, string from, string to)
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("Validate", "False");
            if(userId == null)
            {
                return RedirectToAction("Error");
            }
            HttpContext.Session.SetString("UserId", userId);
            HttpContext.Session.SetString("Link", HttpContext.Request.GetDisplayUrl());
            if (from != null && to != null)
            {
                HttpContext.Session.SetString("DateFromEmail", from);
                HttpContext.Session.SetString("DateToEmail", to);
            }
            return RedirectToAction("CodeInput");
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult CodeInput()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CodeInput(LinkCodeDto linkCodeDto)
        {
            string link =  HttpContext.Session.GetString("Link");
            if(link == null)
            {
                ModelState.AddModelError("message", "Please access the site from the link in your mail");
                return View(linkCodeDto);
            }
            if (_linkCodeService.CheckLinkCode(linkCodeDto, link))
            {
                HttpContext.Session.SetString("Validate", "Yes");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("message", "Invalid email or code or the link has expired");
            return View(linkCodeDto);
        }
    }
}
