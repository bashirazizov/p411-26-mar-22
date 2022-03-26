using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vesper.DAL;
using vesper.Models;

namespace vesper.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectToCategoriesController : Controller
    {
        private readonly VesperDbContext _context;

        public ProjectToCategoriesController(VesperDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProjectToCategories
        public async Task<IActionResult> Index()
        {
            var vesperDbContext = _context.ProjectToCategories.Include(p => p.Category).Include(p => p.Project);
            return View(await vesperDbContext.ToListAsync());
        }

        // GET: Admin/ProjectToCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectToCategory = await _context.ProjectToCategories
                .Include(p => p.Category)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectToCategory == null)
            {
                return NotFound();
            }

            return View(projectToCategory);
        }

        // GET: Admin/ProjectToCategories/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        // POST: Admin/ProjectToCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,ProjectId,Id")] ProjectToCategory projectToCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectToCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projectToCategory.CategoryId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", projectToCategory.ProjectId);
            return View(projectToCategory);
        }

        // GET: Admin/ProjectToCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectToCategory = await _context.ProjectToCategories.FindAsync(id);
            if (projectToCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projectToCategory.CategoryId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", projectToCategory.ProjectId);
            return View(projectToCategory);
        }

        // POST: Admin/ProjectToCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,ProjectId,Id")] ProjectToCategory projectToCategory)
        {
            if (id != projectToCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectToCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectToCategoryExists(projectToCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", projectToCategory.CategoryId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", projectToCategory.ProjectId);
            return View(projectToCategory);
        }

        // GET: Admin/ProjectToCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectToCategory = await _context.ProjectToCategories
                .Include(p => p.Category)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectToCategory == null)
            {
                return NotFound();
            }

            return View(projectToCategory);
        }

        // POST: Admin/ProjectToCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectToCategory = await _context.ProjectToCategories.FindAsync(id);
            _context.ProjectToCategories.Remove(projectToCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectToCategoryExists(int id)
        {
            return _context.ProjectToCategories.Any(e => e.Id == id);
        }
    }
}
