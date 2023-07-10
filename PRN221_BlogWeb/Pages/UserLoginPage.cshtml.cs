using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_BlogWeb.Models;
using System.Security.Claims;

namespace PRN221_BlogWeb.Pages
{
    [AllowAnonymous]
    public class LoginPageModel : PageModel
    {
        private readonly ILogger<LoginPageModel> _logger;
        public BlogWebContext context;
        public LoginPageModel(ILogger<LoginPageModel> logger)
        {
            _logger = logger;
            context = new BlogWebContext();
        }


        [BindProperty]
        public User user { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost(User user)
        {
            if (ModelState.IsValid)
            {
                User _user = context.Users.FirstOrDefault(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password));
                if (_user != null)
                {
                    int UserId = _user.UserId;
                    var claims = new List<Claim>()
                    {
                        new Claim("Id", UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, "User"),
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {                        
                    });                    
                    HttpContext.Session.SetInt32("UserId", UserId);
                    HttpContext.Session.SetString("Username", _user.Username);
                    return LocalRedirect("/");
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return LocalRedirect("/Error");
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;

            var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Id");
            await HttpContext.SignOutAsync(scheme);
            return RedirectToPage("/Index");

        }
    }
}
