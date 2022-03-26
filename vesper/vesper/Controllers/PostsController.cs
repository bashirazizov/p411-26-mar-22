using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vesper.DAL;
using vesper.Models;
using vesper.Utils;

namespace vesper.Controllers
{
    public class PostsController : Controller
    {

        private readonly VesperDbContext db;
        private readonly UserManager<AppUser> userManager;
        private readonly IWebHostEnvironment env;

        public PostsController(VesperDbContext _db, UserManager<AppUser> _userManager, IWebHostEnvironment _env)
        {
            userManager = _userManager;
            env = _env;
            db = _db;
        }

        public IActionResult Add()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login","Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Post post)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid) return View();
            
            if (!post.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Fayl sekil olmalidir.");
                return View();
            }

            if (!post.Image.IsValidSize(500))
            {
                ModelState.AddModelError("Image", "Fayl maksimum 500kb ola biler.");
                return View();
            }

            post.Img = await post.Image.Upload(env.WebRootPath, @"assets\img\posts");
            post.AppUserId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Account");
        }
    }
}
