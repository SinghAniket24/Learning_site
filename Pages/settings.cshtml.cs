using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Supabase;
using System.Threading.Tasks;

namespace Learning_site.Pages
{
    public class settingsModel : PageModel
    {
        private readonly Client _supabase;

        public settingsModel(Client supabase)
        {
            _supabase = supabase;
        }

        public Task<IActionResult> OnGetAsync()
        {
            var session = _supabase.Auth.CurrentSession;
            if (session == null)
            {
                return Task.FromResult<IActionResult>(RedirectToPage("/login"));
            }
            return Task.FromResult<IActionResult>(Page());
        }
    }
}