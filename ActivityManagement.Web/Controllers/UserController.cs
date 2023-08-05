using ActivityManagement.Domain;
using ActivityManagement.Domain.Identity;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;

namespace ActivityManagement.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ActivityUser> _userManager;

        public UserController(UserManager<ActivityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImportUsers(IFormFile file)
        {
            if (file != null)
            {
                string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";
                using (FileStream fileStream = System.IO.File.Create(pathToUpload))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }
                List<ImportUsers> users = getUsersFromExcelFile(file.FileName);
                foreach (var item in users)
                {
                    var userCheck = _userManager.FindByEmailAsync(item.Email).Result;
                    if (userCheck == null)
                    {
                        var user = new ActivityUser
                        {
                            UserName = item.Email,
                            NormalizedUserName = item.Email,
                            Email = item.Email,
                            EmailConfirmed = true,
                            FirstName = item.FirstName,
                            LastName = item.LastName
                        };
                        var result = _userManager.CreateAsync(user, item.Password).Result;
                        _userManager.AddClaimAsync(user, new Claim("UserRole", "User"));
                        _userManager.UpdateAsync(user);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return RedirectToAction("Index", "Activity");
        }

        private List<ImportUsers> getUsersFromExcelFile(string fileName)
        {
            string pathToFile = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            List<ImportUsers> userList = new List<ImportUsers>();
            using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        userList.Add(new ImportUsers
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            FirstName = reader.GetValue(2).ToString(),
                            LastName = reader.GetValue(3).ToString(),
                        });
                    }
                }
            }
            return userList;
        }
    }
}
