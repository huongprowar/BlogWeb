using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace PRN221_BlogWeb.Pages
{        
    [Authorize(Roles = "User")]
    public class CreateBlogModel : PageModel
    {
        private readonly ILogger<CreateBlogModel> _logger;
        private BlogWebContext _context;
        public CreateBlogModel(ILogger<CreateBlogModel> logger) 
        {
            _logger = logger;
            _context = new BlogWebContext();
        }
        [BindProperty]
        public Blog blog { get; set; }
        [BindProperty]
        public List<Category> categories { get; set; }
        public void OnGet()
        {
            categories = _context.Categories.ToList();
        }
        public async Task<IActionResult> OnPost(Blog blog)
        {
            if(ModelState.IsValid)
            {
                string userId = User.Claims.First().Value;                                
                blog.User = _context.Users.FirstOrDefault(x => x.UserId == Convert.ToInt32(userId));
                blog.CreatedAt= DateTime.Now;                
                _context.Entry<Blog>(blog).State = EntityState.Added;
                _context.SaveChanges();
            }
            return LocalRedirect("/");
        }
    }
}
