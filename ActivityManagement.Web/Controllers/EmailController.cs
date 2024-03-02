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

        public IActionResult Index(string code, int? pageNumber)
        {
            if (code == null)
            {
                return RedirectToAction("Error");
            }
            HttpContext.Session.SetString("Code", code);
            LinkCode linkCode = _linkCodeService.CheckLinkCode(code);
            List<Activity> activities;
            if (linkCode != null)
            {
                if (linkCode.DateFrom != null && linkCode.DateTo != null)
                {
                    activities = _activityService.GetActivitiesByTimeInterval(linkCode.UserId,
                        DateTime.Parse(linkCode.DateFrom), DateTime.Parse(linkCode.DateTo));
                }
                else
                {
                    activities = _activityService.GetAllActivities(linkCode.UserId);
                }
                return View(PaginatedList<Activity>.Create(activities.OrderBy(a => a.Date).ToList(),
                    pageNumber ?? 1, 5));
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Paginate(int pageNumber)
        {
            string code = HttpContext.Session.GetString("Code");
            return RedirectToAction("Index", new { code, pageNumber });
        }

        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }

       /* [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CodeInput(LinkCodeDto linkCodeDto)
        {
           *LinkCode linkCode = _linkCodeService.CheckLinkCode(linkCodeDto);
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
        }*/
    }
}
