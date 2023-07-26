using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace PRN221_BlogWeb.Pages
{
    public class CategoryManagerModel : PageModel
    {
        private ILogger<CommentPageModel> _logger;
        private BlogWebContext _context;
        public CategoryManagerModel(ILogger<CommentPageModel> logger)
        {
            _logger = logger;
            _context = new BlogWebContext();
        }

        public List<Category> Categories { get; set; }
        public Category category { get; set; }
        public IActionResult OnGet()
        {
            Categories = _context.Categories.ToList();
            return Page();
        }
        public IActionResult OnPostCreate(string createCategoryName)
        {
            Category category = new Category()
            {
                CategoryName = createCategoryName,
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return LocalRedirect("/");
        }
        public IActionResult OnPostDelete(string deleteCategoryId)
        {
            if (ModelState.IsValid)
            {
                Category cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Convert.ToInt32(deleteCategoryId));
                foreach (var blog in _context.Blogs.Include("Comments").Where(x => x.CategoryId==cat.CategoryId))
                {
                    _context.Comments.RemoveRange(blog.Comments);
                    _context.Blogs.Remove(blog);
                }
                _context.SaveChanges();
                _context.Categories.Remove(cat);
                _context.SaveChanges();
            }
            return LocalRedirect("/");
        }
        public IActionResult OnPostUpdate(string updateCategoryId, string updateCategoryName)
        {
            if (ModelState.IsValid)
            {
                Category cat = _context.Categories.FirstOrDefault(x => x.CategoryId == Convert.ToInt32(updateCategoryId));
                cat.CategoryName = updateCategoryName;
                _context.SaveChanges();
            }
            return LocalRedirect("/");
        }
    }
}
