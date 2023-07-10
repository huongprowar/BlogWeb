using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_BlogWeb.Models;
using System.Reflection.Metadata.Ecma335;

namespace PRN221_BlogWeb.Pages
{
    [AllowAnonymous]
    public class RegisterPageModel : PageModel
    {
        private readonly ILogger<RegisterPageModel> _logger;
        public BlogWebContext context;
        public RegisterPageModel(ILogger<RegisterPageModel> logger)
        {
            _logger = logger;
            context = new BlogWebContext();
        }


        [BindProperty]
        public User user { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost(User user)
        {
            if (String.IsNullOrWhiteSpace(user.Username) || user.Username.Contains(' '))
            {
                ViewData["invalidUsername"] = "Tài khoản không được chứa dấu cách.";
                return Page();
            }
            if(ModelState.IsValid)
            {
                var tempUser = context.Users.FirstOrDefault(x => x.Username== user.Username);
                if(tempUser!=null)
                {
                    ViewData["invalidUsername"] = "Tài khoản đã tồn tại.";
                    return Page();
                }
                var gender = Request.Form["gender"];
                user.Gender = gender.Equals("Male");
                user.IsActive = true;
                context.Entry(user).State = EntityState.Added;
                context.SaveChanges();
                return LocalRedirect("/");
            }
            return LocalRedirect("/Error");
        }
    }
}
