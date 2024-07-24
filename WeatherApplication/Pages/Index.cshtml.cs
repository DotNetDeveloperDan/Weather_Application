using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WeatherApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty(SupportsGet = true)]
        public string username { get; set; }

        [BindProperty(SupportsGet = true)]
        public string password { get; set; }

        public string ErrorMessage { get; set; }
        public bool LoginAttempted { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            LoginAttempted = false;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
                if (result.Succeeded)
                {
                    return Redirect("/cities");
                }
                else
                {
                    ErrorMessage = "Invalid login attempt.";
                    LoginAttempted = true;
                }
            }

            return Page();
        }
    }
}