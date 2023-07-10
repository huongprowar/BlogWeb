using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_BlogWeb.Models;

namespace PRN221_BlogWeb.Pages
{
    public class CommentPageModel : PageModel
    {
        private ILogger<CommentPageModel> _logger;
        private BlogWebContext _context;
        [BindProperty]
        public Comment comment { get; set; }
        public CommentPageModel(ILogger<CommentPageModel> logger)
        {
            _logger = logger;
            _context = new BlogWebContext();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(Comment newComment)
        {
            if (ModelState.IsValid)
            {

            }
            return Page();
        }
    }
}
