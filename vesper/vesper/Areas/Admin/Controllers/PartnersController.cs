using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using vesper.DAL;
using vesper.Models;
using vesper.Utils;

namespace vesper.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class PartnersController : Controller
    {
        private readonly VesperDbContext db;
        private readonly IWebHostEnvironment env;
        public PartnersController(VesperDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }
        public async Task<IActionResult> Index(string sort, int? page = 1)
        {
            if (string.IsNullOrEmpty(sort)) sort = "id";

            ViewBag.PageCount = Math.Ceiling((decimal)db.Partners.Count()/10);
            ViewBag.page = page;
            ViewBag.sort = sort;

            if (sort == "id")
            {
                return View(await db.Partners.OrderBy(x=>x.Id).Skip(((int)page-1)*10).Take(10).ToListAsync());
            }
            else if(sort == "az")
            {
                return View(await db.Partners.OrderBy(x => x.Name).Skip(((int)page - 1) * 10).Take(10).ToListAsync());
            }
            else
            {
                return View(await db.Partners.OrderByDescending(x => x.Name).Skip(((int)page - 1) * 10).Take(10).ToListAsync());
            }
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Partner partner)
        {
            if (!ModelState.IsValid) return View();
            Partner duplicate = await db.Partners.FirstOrDefaultAsync(x => x.Name == partner.Name);
            if (duplicate != null)
            {
                ModelState.AddModelError("Name", "Name unique olmalidir.");
                return View();
            }

            if (!partner.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Fayl sekil olmalidir.");
                return View();
            }

            if (!partner.Image.IsValidSize(500))
            {
                ModelState.AddModelError("Image", "Fayl maksimum 500kb ola biler.");
                return View();
            }

            partner.Img = await partner.Image.Upload(env.WebRootPath, @"assets\img\clients");

            await db.Partners.AddAsync(partner);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Partners");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Partners");
            Partner partner = await db.Partners.FindAsync(id);
            if (partner == null) return RedirectToAction("Index", "Partners");
            return View(partner);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Partners");
            Partner partner = await db.Partners.FindAsync(id);
            if (partner == null) return RedirectToAction("Index", "Partners");
            return View(partner);
        }
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Partners");
            Partner partnerToDelete = await db.Partners.FindAsync(id);
            if (partnerToDelete == null) return RedirectToAction("Index", "Partners");

            string filePath = Path.Combine(env.WebRootPath, @"assets\img\clients", partnerToDelete.Img);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            db.Partners.Remove(partnerToDelete);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "Partners");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Partners");
            Partner partner = await db.Partners.FindAsync(id);
            if (partner == null) return RedirectToAction("Index", "Partners");
            return View(partner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Partner partner) {


            if (!ModelState.IsValid) return View();

            if (partner.Image != null)
            {
                if (!partner.Image.IsImage())
                {
                    ModelState.AddModelError("Image", "Fayl sekil olmalidir.");
                    return View(partner);
                }

                if (!partner.Image.IsValidSize(500))
                {
                    ModelState.AddModelError("Image", "Fayl maksimum 500kb ola biler.");
                    return View(partner);
                }

                string filePath = Path.Combine(env.WebRootPath, @"assets\img\clients", partner.Img);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                partner.Img = await partner.Image.Upload(env.WebRootPath, @"assets\img\clients");
            }

            db.Partners.Update(partner);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Partners");
        }
    }
}
