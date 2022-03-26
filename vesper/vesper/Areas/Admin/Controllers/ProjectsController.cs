using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vesper.DAL;
using vesper.Models;
using vesper.Utils;

namespace vesper.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ProjectsController : Controller
    {
        private readonly VesperDbContext db;
        private readonly IWebHostEnvironment env;

        public ProjectsController(VesperDbContext _db, IWebHostEnvironment _env)
        {
            db = _db;
            env = _env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Projects.ToListAsync());
        }

        public async Task<IActionResult> Add()
        {
            ViewData["Categories"] = await db.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Project project,string categoryIds)
        {
            ViewData["Categories"] = await db.Categories.ToListAsync();

            if (!ModelState.IsValid) return View();
            Project duplicate = await db.Projects.FirstOrDefaultAsync(x => x.Name == project.Name);
            if (duplicate != null)
            {
                ModelState.AddModelError("Name", "Name unique olmalidir.");
                return View();
            }

            if (!project.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Fayl sekil olmalidir.");
                return View();
            }

            if (!project.ImageFile.IsValidSize(500))
            {
                ModelState.AddModelError("ImageFile", "Fayl maksimum 500kb ola biler.");
                return View();
            }

            project.Image = await project.ImageFile.Upload(env.WebRootPath, @"assets\img\portfolio");

            await db.Projects.AddAsync(project);
            await db.SaveChangesAsync();

            if (project.ImageList!=null && project.ImageList.Length>0)
            {
                foreach (IFormFile item in project.ImageList)
                {
                    if (!item.IsImage())
                    {
                        ModelState.AddModelError("ImageList", item.FileName + "fayli sekil deyil.");
                        db.Projects.Remove(db.Projects.Find(project.Id));
                        await db.SaveChangesAsync();
                        return View();
                    }

                    if (!item.IsValidSize(500))
                    {
                        ModelState.AddModelError("ImageList", item.FileName + "faylinin olcusu boyukdur.");
                        db.Projects.Remove(db.Projects.Find(project.Id));
                        await db.SaveChangesAsync();
                        return View();
                    }

                    ProjectImage pi = new ProjectImage();
                    pi.Img = await item.Upload(env.WebRootPath, @"assets\img\portfolio");
                    pi.ProjectId = project.Id;

                    await db.ProjectImages.AddAsync(pi);
                }
                await db.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(categoryIds))
            {
                string[] cIds = categoryIds.Split(",");
                Array.Resize(ref cIds, cIds.Length - 1);
                foreach (string cId in cIds)
                {
                    ProjectToCategory ptc = new ProjectToCategory();
                    ptc.CategoryId = Convert.ToInt32(cId);
                    ptc.ProjectId = project.Id;
                    await db.ProjectToCategories.AddAsync(ptc);
                }
                await db.SaveChangesAsync();
            }
            //if (category1!=null)
            //{
            //    ProjectToCategory ptc = new ProjectToCategory();
            //    ptc.CategoryId = (int)category1;
            //    ptc.ProjectId = project.Id;
            //    db.ProjectToCategories.Add(ptc);
            //}
            //if (category2 != null)
            //{
            //    ProjectToCategory ptc = new ProjectToCategory();
            //    ptc.CategoryId = (int)category2;
            //    ptc.ProjectId = project.Id;
            //    db.ProjectToCategories.Add(ptc);
            //}
            //if (category3 != null)
            //{
            //    ProjectToCategory ptc = new ProjectToCategory();
            //    ptc.CategoryId = (int)category3;
            //    ptc.ProjectId = project.Id;
            //    db.ProjectToCategories.Add(ptc);
            //}
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Projects");
        }
    }
}
