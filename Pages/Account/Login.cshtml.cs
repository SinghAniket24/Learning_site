using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Learning_site.Pages.Account
{
    public class LoginModel : PageModel
    {
        public IActionResult OnGet(string returnUrl = "/dashboard")
        {
            // Challenge the user with OpenID Connect (Auth0)
            return Challenge(
                new AuthenticationProperties { RedirectUri = returnUrl },
                "OpenIdConnect"
            );
        }
    }
}
