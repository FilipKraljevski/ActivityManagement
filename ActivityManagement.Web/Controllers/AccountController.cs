using ActivityManagement.Domain.DTO;
using ActivityManagement.Domain.Identity;
using ActivityManagement.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ActivityManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ActivityUser> userManager;
        private readonly SignInManager<ActivityUser> signInManager;
        private readonly IUserService _userService;

        public AccountController(UserManager<ActivityUser> userManager, SignInManager<ActivityUser> signInManager, IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (!_userService.CheckAdminUserInDatabase())
            {
                var user = new ActivityUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "testprojectsemail4@gmail.com",
                    NormalizedUserName = "testprojectsemail4@gmail.com",
                    Email = "testprojectsemail4@gmail.com",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(user, "Admin=1").Result;
                userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
            }
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);
                }
                if(await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid Credentials");
                    return View(model);
                }
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Activity");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
