using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vesper.DAL;
using vesper.Models;
using vesper.Utils;

namespace vesper.Controllers
{
    public class AccountController : Controller
    {
        private readonly VesperDbContext db;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IWebHostEnvironment env;

        public AccountController(VesperDbContext _db, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager, IWebHostEnvironment _env)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            env = _env;
            db = _db;
        }

        [Route("/account")]
        [Route("/account/index")]
        [Route("/account/index/{username?}")]
        public async Task<IActionResult> Index(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login");
                }

                AppUser loggedUser = await db.Users.Include(x=>x.Posts).FirstOrDefaultAsync(x=>x.UserName == User.Identity.Name);
                return View(loggedUser);
            }

            AppUser userToView = await db.Users.Include(x => x.Posts).FirstOrDefaultAsync(x => x.UserName == username);
            return View(userToView);
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return Content("No Access");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (!ModelState.IsValid) return View();

            AppUser loggingUser = await userManager.FindByNameAsync(lvm.UserName);
            if (loggingUser == null)
            {
                ModelState.AddModelError("", "Email or password is wrong!");
                return View(lvm);
            };
            if (!loggingUser.IsActive)
            {
                ModelState.AddModelError("", "Account is disabled. Please contact admin!");
                return View(lvm);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(loggingUser, lvm.Password, lvm.KeepMeLoggedIn, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "You are locked out!");
                return View(lvm);
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong!");
                return View(lvm);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if (!ModelState.IsValid) return View();

            if (rvm.Image != null)
            {
                if (!rvm.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Fayl sekil olmalidir.");
                    return View();
                }

                if (!rvm.Image.IsValidSize(500))
                {
                    ModelState.AddModelError("Image", "Fayl maksimum 500kb ola biler.");
                    return View();
                }
            }

            AppUser newUser = new AppUser
            {
                FullName = rvm.Name + " " + rvm.Surname,
                Email = rvm.Email,
                UserName = rvm.UserName,
                IsActive = true
            };

            if (rvm.Image != null)
            {
                newUser.ProfilePhoto = await rvm.Image.Upload(env.WebRootPath, @"assets\img\users");
            }

            IdentityResult identityResult = await userManager.CreateAsync(newUser, rvm.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(rvm);
            }

            await userManager.AddToRoleAsync(newUser, "Member");

            await signInManager.SignInAsync(newUser, true);


            if ((await userManager.GetRolesAsync(newUser))[0] == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }
        //public async Task<IActionResult> InitRoles()
        //{
        //    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

        //}
    }
}
