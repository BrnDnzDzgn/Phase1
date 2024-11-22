#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.Controllers.Bases;
using BLL.Services;
using BLL.Models;
using System.Linq;

namespace MVC.Controllers
{
    public class BlogsController : MvcController
    {
        private readonly IBlogService _blogService;
        private readonly IUsersService _userService;

        public BlogsController(
            IBlogService blogService,
            IUsersService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }

        // GET: Blogs
        public IActionResult Index()
        {
            var list = _blogService.Query().ToList();
            return View(list);
        }

        // GET: Blogs/Details/5
        public IActionResult Details(int id)
        {
            var item = _blogService.Query().SingleOrDefault(q => q.Record.Id == id);
            if (item == null)
            {
                return NotFound(); // 404 if not found
            }
            return View(item);
        }

        // Set ViewData for dropdown lists
        protected void SetViewData()
        {
            ViewData["UserId"] = new SelectList(
                _userService.Query().Select(u => u.Record).ToList(),
                "Id", "UserName");
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        // POST: Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogModel blog)
        {
            if (blog?.Record == null)
            {
                ModelState.AddModelError("", "Blog data is required.");
                SetViewData();
                return View(blog);
            }

            if (ModelState.IsValid)
            {
                var result = _blogService.Create(blog.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = blog.Record.Id });
                }
                ModelState.AddModelError("", result.Message);
            }

            SetViewData();
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _blogService.Query().SingleOrDefault(q => q.Record.Id == id);
            if (item == null)
            {
                return NotFound(); // 404 if not found
            }

            SetViewData();
            return View(item);
        }

        // POST: Blogs/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BlogModel blog)
        {
            if (blog?.Record == null)
            {
                ModelState.AddModelError("", "Blog data is required.");
                SetViewData();
                return View(blog);
            }

            if (ModelState.IsValid)
            {
                var result = _blogService.Update(blog.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = blog.Record.Id });
                }
                ModelState.AddModelError("", result.Message);
            }

            SetViewData();
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _blogService.Query().SingleOrDefault(q => q.Record.Id == id);
            if (item == null)
            {
                return NotFound(); // 404 if not found
            }

            return View(item);
        }

        // POST: Blogs/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _blogService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}