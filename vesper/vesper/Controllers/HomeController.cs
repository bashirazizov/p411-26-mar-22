using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vesper.DAL;
using vesper.Models;

namespace vesper.Controllers
{
    public class HomeController : Controller
    {
        private readonly VesperDbContext db;
        private readonly UserManager<AppUser> userManager;

        public HomeController(VesperDbContext _db, UserManager<AppUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            HomeViewModel hvm = new HomeViewModel {
                Banner = db.Banners.FirstOrDefault(),
                Partners = db.Partners.ToList(),
                Stats = db.Stats.ToList(),
                HomeAboutSection = db.HomeAboutSections.FirstOrDefault(),
                Categories = db.Categories.ToList(),
                Projects = db.Projects.OrderByDescending(x=>x.Id).Include(x => x.ProjectToCategories).Take(6).ToList(),
                Users = userManager.Users.Where(x => x.IsActive == true).ToList()
            };

            return View(hvm);
        }
    }
}
