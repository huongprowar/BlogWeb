using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_BlogWeb.Models;
using System.Security.Claims;

namespace PRN221_BlogWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private BlogWebContext _context;

        [BindProperty]
        public List<Blog> listBlogs { get; set; }

        [BindProperty]
        public Comment comment { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = new BlogWebContext();
        }

        public IActionResult OnGet()
        {
            var identityValue = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityValue.Claims;
            if (claims.Any())
            {
                HttpContext.Session.SetString("UserId", claims.First().Value);
                HttpContext.Session.SetString("Username", claims.ElementAt(1).Value);
                var role = claims.ElementAt(2).Value;
            }
            listBlogs = _context.Blogs.Include(x => x.Comments).Include(x => x.User).ToList();
            return Page();
        }
        public IActionResult OnPostComment(string commentContent, string blogid)
        {            
            if (User.Claims.First().Value != null)
            {
                int userId = Convert.ToInt32(User.Claims.First().Value);
                Comment comment = new Comment()
                {
                    User = _context.Users.FirstOrDefault(x => x.UserId == userId),
                    Blog = _context.Blogs.FirstOrDefault(x => x.BlogId == Convert.ToInt32(blogid)),
                    Content = commentContent
                };
                _context.Entry<Comment>(comment).State= EntityState.Added;
                _context.SaveChanges();
            }
            return LocalRedirect("/");
        }
    }
}