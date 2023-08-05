using ActivityManagement.Domain.DTO;
using ActivityManagement.Domain.Model;
using ActivityManagement.Service.Interface;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

namespace ActivityManagement.Web.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Activity> activities = _activityService.GetAllActivities(userId);
            return View(activities);
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

        [HttpPost]
        public IActionResult FilterByTimeInterval(DateTime from, DateTime to)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Activity> activities = _activityService.GetActivitiesByTimeInterval(userId, from, to);
            return View("Index", activities);
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
    }
}
