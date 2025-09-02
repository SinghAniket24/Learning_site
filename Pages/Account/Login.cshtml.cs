using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Learning_site.Pages.Account
{
    public class LoginModel : PageModel
    {
        public IActionResult OnPost()
        {
            // Trigger Auth0 login only when button is clicked
            return Challenge(
                new AuthenticationProperties { RedirectUri = "/dashboard" },
                "OpenIdConnect"
            );
        }
    }
}
