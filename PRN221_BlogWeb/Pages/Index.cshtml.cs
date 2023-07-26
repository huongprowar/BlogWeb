using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        [BindProperty]
        public string SearchResult { get; set; }
        [BindProperty]
        public List<Category> categories { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = new BlogWebContext();
        }

        public IActionResult OnGet(string SearchResult, string CategoryId)
        {
            var identityValue = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityValue.Claims;
            if (claims.Any())
            {
                HttpContext.Session.SetString("UserId", claims.First().Value);
                HttpContext.Session.SetString("Username", claims.ElementAt(1).Value);
                var role = claims.ElementAt(2).Value;
            }
            var testListComment = _context.Comments.Include(x => x.User).ToList();
            if (SearchResult != null || !String.IsNullOrEmpty(SearchResult))
            {
                listBlogs = _context.Blogs.Include(x => x.Comments).Include(x => x.User).Include(x => x.Category).Where(x => x.User.Username.Equals(SearchResult) || x.Title.Equals(SearchResult) || x.Category.CategoryName.Equals(SearchResult)).ToList();
            }
            else
            {
                
                listBlogs = _context.Blogs.Include(x => x.Comments).Include(x => x.User).Include(x => x.Category).ToList();
            }
            if (!String.IsNullOrEmpty(CategoryId))
            {
                int categoryId = Convert.ToInt32(CategoryId);
                if (categoryId != 0) listBlogs = listBlogs.Where(x => x.CategoryId == categoryId).ToList();
            }
            categories = _context.Categories.ToList();
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
                _context.Entry<Comment>(comment).State = EntityState.Added;
                _context.SaveChanges();
            }
            return LocalRedirect("/");
        }
        public IActionResult OnPostDeleteComment(string commentId)
        {
            Comment deleteComment = _context.Comments.FirstOrDefault(x => x.CommentId == Convert.ToInt32(commentId));
            _context.Comments.Remove(deleteComment);
            _context.SaveChanges();
            return LocalRedirect("/");
        }
    }
}