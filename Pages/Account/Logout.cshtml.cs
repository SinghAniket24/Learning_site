using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Learning_site.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            // Logout from both Cookie and OpenIdConnect
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = "http://localhost:5136/dashboard" 
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                "OpenIdConnect"
            );
        }
    }
}
