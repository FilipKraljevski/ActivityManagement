﻿using ActivityManagement.Domain.DTO;
using ActivityManagement.Domain.Model;
using ActivityManagement.Repository;
using ActivityManagement.Service.Interface;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ActivityManagement.Web.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IEmailService _emailService;
        private readonly ILinkCodeService _linkCodeService;

        public ActivityController(IActivityService activityService, IEmailService emailService, ILinkCodeService linkCodeService)
        {
            _activityService = activityService;
            _emailService = emailService;
            _linkCodeService = linkCodeService;
        }

        public IActionResult Index(string from, string to, int? pageNumber)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Activity> activities;
            if (from != null && to != null)
            {
                HttpContext.Session.SetString("DateFilterFrom", from);
                HttpContext.Session.SetString("DateFilterTo", to);
                activities = _activityService.GetActivitiesByTimeInterval(userId, DateTime.Parse(from), DateTime.Parse(to));
            }
            else if(HttpContext.Session.GetString("DateFilterFrom") != null && 
                HttpContext.Session.GetString("DateFilterTo") != null)
            {
                string f = HttpContext.Session.GetString("DateFilterFrom");
                string t = HttpContext.Session.GetString("DateFilterTo");
                activities = _activityService.GetActivitiesByTimeInterval(userId, DateTime.Parse(f), DateTime.Parse(t));
            }
            else
            {
                activities = _activityService.GetAllActivities(userId);
            }
            return View(PaginatedList<Activity>.Create(activities.OrderBy(a => a.Date).ToList(), 
                pageNumber ?? 1, 5));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ActivityDto activityDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (activityDto != null)
            {
                _activityService.CreateActivity(activityDto, userId);
                return RedirectToAction("Index");
            }
            return View(activityDto);
        }

        public IActionResult ResetFilter()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PrintReport(DateTime from, DateTime to)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Activity> activities = _activityService.GetActivitiesByTimeInterval(userId, from, to);
            string fileName = "PrintReport.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("Reports");
                var days = to - from;
                for(int i = 0; i < days.Days; i++)
                {
                    worksheet.Cell(1, i+1).Value = from.AddDays(i).Date;
                }
                worksheet.Cell(1, days.Days + 1).Value = "Total Time Spent";
                double totalTimeSpent = 0;
                for(int i = 0; i < activities.Count; i++)
                {
                    for(int j = 1; j <= days.Days; j++)
                    {
                        if(worksheet.Cell(1, j).Value.GetDateTime().Date.Equals(activities[i].Date.Date))
                        {
                            worksheet.Cell(2, j).Value += "*" + activities[i].Description + "\n";
                            totalTimeSpent += activities[i].TimeSpent;
                        }
                    }
                }
                worksheet.Cell(2, days.Days + 1).Value =totalTimeSpent + "hours";
                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        [HttpPost]
        public IActionResult SendEmail(string toEmail)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string from = HttpContext.Session.GetString("DateFilterFrom");
            string to = HttpContext.Session.GetString("DateFilterTo");
            LinkCode linkcode = _linkCodeService.Create(toEmail, userId, from, to);
            string url = "https://localhost:44361/Email/Index?code=" + linkcode.Id.ToString();
            _emailService.Send(toEmail, /*linkcode.Id.ToString(),*/ url);
            return View();
        }
    }
}
