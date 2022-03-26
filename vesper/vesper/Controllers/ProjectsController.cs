using Microsoft.AspNetCore.Authorization;
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
    public class ProjectsController : Controller
    {
        private readonly VesperDbContext db;
        public ProjectsController(VesperDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            ViewBag.Count = db.Projects.Count();

            ProjectsViewModel model = new ProjectsViewModel();
            model.Projects = db.Projects.Take(6).ToList();
            return View(model);
        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult Searchv2()
        {
            return View();
        }

        public IActionResult SearchProject(string query)
        {
            List<Project> result = db.Projects.Where(x => x.Name.ToLower().Contains(query.ToLower())).ToList();
            return PartialView("_ProjectsPartial", result);
        }

        public IActionResult SearchProjectv2(string query)
        {
            List<Project> result = db.Projects.Where(x => x.Name.ToLower().Contains(query.ToLower())).ToList();
            return Json(result);
        }



        public IActionResult Info(int id)
        {
            Project project = db.Projects.Include(x => x.ProjectImages).First(x => x.Id == id);

            Project nextProj = db.Projects.OrderBy(x => x.Id).FirstOrDefault(x => x.Id > project.Id);
            Project prevProj = db.Projects.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Id < project.Id);

            if (nextProj != null)
            {
                ViewBag.NextId = nextProj.Id;
            }
            else
            {
                ViewBag.NextId = db.Projects.OrderBy(x => x.Id).First().Id;
            }

            if (prevProj != null)
            {
                ViewBag.PrevId = prevProj.Id;
            }
            else
            {
                ViewBag.PrevId = db.Projects.OrderByDescending(x => x.Id).First().Id;
            }

            return View(project);
        }

        //public IActionResult LoadProjects(int skip = 0)
        //{
        //    return Json(db.Projects.Skip(skip).Take(6));
        //}

        public IActionResult LoadProjectsAsView(int skip = 0)
        {
            return PartialView("_ProjectsPartial", db.Projects.Skip(skip).Take(6).ToList());
        }

        [Authorize(Roles = "Member,Admin")]
        public IActionResult AddPost()
        {
            return View();
        }
    }
}
