#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.Controllers.Bases;
using BLL.Services;
using BLL.Models;
using BLL.DAL;

// Generated from Custom Template.

namespace MVC.Controllers
{
    public class CommentsController : MvcController
    {
        // Service injections:
        private readonly ICommentService _commentService;
        private readonly IBlogService _blogService;
        private readonly IUsersService _userService;

        /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
        //private readonly IManyToManyRecordService _ManyToManyRecordService;

        public CommentsController(
            ICommentService commentService
            , IBlogService blogService
            , IUsersService userService

        /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
        //, IManyToManyRecordService ManyToManyRecordService
        )
        {
            _commentService = commentService;
            _blogService = blogService;
            _userService = userService;

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //_ManyToManyRecordService = ManyToManyRecordService;
        }

        // GET: Comments for a specific blog
        public IActionResult Index(int blogId)
        {
            var blog = _blogService.Query().SingleOrDefault(b => b.Record.Id == blogId);
            if (blog == null)
            {
                return NotFound(); // 404 if blog does not exist
            }

            ViewBag.BlogId = blogId;
            ViewBag.BlogTitle = blog.Title;

            var comments = _commentService.Query()
                .Where(c => c.Record.BlogId == blogId)
                .ToList();
            return View(comments);
        }

        // GET: Comments/Details/5
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _commentService.Query().SingleOrDefault(q => q.Record.Id == id);
            return View(item);
        }

        protected void SetViewData()
        {
            // Related items service logic to set ViewData (Record.Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            ViewData["BlogId"] = new SelectList(_blogService.Query().ToList(), "Record.Id", "Name");
            ViewData["UserId"] = new SelectList(_userService.Query().ToList(), "Record.Id", "Name");

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //ViewBag.ManyToManyRecordIds = new MultiSelectList(_ManyToManyRecordService.Query().ToList(), "Record.Id", "Name");
        }

        // GET: Create a new comment for a specific blog
        public IActionResult Create(int blogId)
        {
            var blog = _blogService.Query().SingleOrDefault(b => b.Record.Id == blogId);
            if (blog == null)
            {
                return NotFound(); // 404 if blog does not exist
            }

            var comment = new CommentModel
            {
                Record = new Comment
                {
                    BlogId = blogId // Set the BlogId for the comment
                }
            };

            ViewBag.BlogTitle = blog.Title;

            // Populate User dropdown
            ViewBag.UserId = _userService.Query().Select(u => new SelectListItem
            {
                Value = u.Record.Id.ToString(),
                Text = u.Record.UserName
            }).ToList();

            return View(comment);
        }

        // POST: Save a new comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommentModel comment)
        {
            // Set default values if necessary
            if (comment.Record.PublishDate == DateTime.MinValue)
            {
                comment.Record.PublishDate = DateTime.Now; // Set a default publish date
            }

            if (ModelState.IsValid)
            {
                var result = _commentService.Create(comment.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction("Index", new { blogId = comment.Record.BlogId });
                }

                ModelState.AddModelError("", result.Message);
            }

            // Repopulate dropdowns and other data in case of failure
            var blog = _blogService.Query().SingleOrDefault(b => b.Record.Id == comment.Record.BlogId);
            ViewBag.BlogTitle = blog?.Title;

            ViewBag.UserId = _userService.Query().Select(u => new SelectListItem
            {
                Value = u.Record.Id.ToString(),
                Text = u.Record.UserName
            }).ToList();

            return View(comment);
        }

        // GET: Comments/Edit/5
        public IActionResult Edit(int id)
        {
            // Fetch the comment by ID
            var comment = _commentService.Query().SingleOrDefault(c => c.Record.Id == id);
            if (comment == null)
            {
                return NotFound(); // 404 if comment does not exist
            }

            // Populate the Blog dropdown (optional, as BlogId is fixed for a comment)
            ViewBag.BlogId = new List<SelectListItem>
            {
                new SelectListItem { Value = comment.Record.BlogId.ToString(), Text = _blogService.Query().SingleOrDefault(b => b.Record.Id == comment.Record.BlogId)?.Title }
            };

            // Populate the User dropdown
            ViewBag.UserId = _userService.Query().Select(u => new SelectListItem
            {
                Value = u.Record.Id.ToString(),
                Text = u.Record.UserName
            }).ToList();

            return View(comment);
        }

        // POST: Comments/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CommentModel comment)
        {
            if (ModelState.IsValid)
            {
                var result = _commentService.Update(comment.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction("Index", new { blogId = comment.Record.BlogId });
                }

                ModelState.AddModelError("", result.Message);
            }

            // Repopulate dropdowns in case of validation errors
            ViewBag.BlogId = new List<SelectListItem>
            {
                new SelectListItem { Value = comment.Record.BlogId.ToString(), Text = _blogService.Query().SingleOrDefault(b => b.Record.Id == comment.Record.BlogId)?.Title }
            };

            ViewBag.UserId = _userService.Query().Select(u => new SelectListItem
            {
                Value = u.Record.Id.ToString(),
                Text = u.Record.UserName
            }).ToList();

            return View(comment);
        }

        // GET: Comments/Delete/5
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _commentService.Query().SingleOrDefault(q => q.Record.Id == id);
            return View(item);
        }

        // POST: Comments/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var result = _commentService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
