using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_BlogWeb.Models;

namespace PRN221_BlogWeb.Pages
{
    [Authorize(Roles ="User")]
    public class MyBlogModel : PageModel
    {
        private readonly ILogger<MyBlogModel> _logger;
        private BlogWebContext _context;
        [BindProperty] public Blog Blog { get; set; }
        public MyBlogModel(ILogger<MyBlogModel> logger)
        {
            _logger = logger;
            _context = new BlogWebContext();
        }
        public List<Blog> blogList;

        public void OnGet()
        {
            blogList = _context.Blogs.Where(x => x.UserId == Convert.ToInt32(User.Claims.First().Value)).ToList();
        }
        public IActionResult OnPostDelete(Blog Blog)
        {
            if (ModelState.IsValid)
            {
                Blog deleteBlog = _context.Blogs.FirstOrDefault(x => x.BlogId == Blog.BlogId);
                _context.Entry<Blog>(deleteBlog).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            return LocalRedirect("/");
        }

        public IActionResult OnPostUpdate(Blog Blog)
        {
            if (ModelState.IsValid)
            {
                Blog updateBlog = _context.Blogs.FirstOrDefault(x => x.BlogId == Blog.BlogId);
                updateBlog.Title = Blog.Title;
                updateBlog.Content = Blog.Content;
                _context.Entry<Blog>(updateBlog).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return LocalRedirect("/");
        }
    }
}
