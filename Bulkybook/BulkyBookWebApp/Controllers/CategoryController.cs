using BulkyBookWebApp.Data;
using BulkyBookWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoriesList = _context.Categories.ToList();
            return View(categoriesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(obj);
        }


        public IActionResult Edit(int? cid)
        {
            if(cid == null || cid == 0)
            {
                return NotFound(); 
            }
            var category = _context.Categories.SingleOrDefault(c => c.Id == cid);
            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Update(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }


        public IActionResult Delete(int cid)
        {
            var obj = _context.Categories.SingleOrDefault(c => c.Id == cid);
            if(obj != null)
            {
                _context.Categories.Remove(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return NotFound();
        }

    }
}
