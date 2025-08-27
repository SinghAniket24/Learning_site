using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Learning_site.Data;
using Learning_site.Models;
using Supabase;

namespace Learning_site.Pages.Managements
{
    public class CreateModel : PageModel
    {
        private readonly Learning_site.Data.Learning_siteContext _context;
        private readonly Client _supabase;

        public CreateModel(Learning_site.Data.Learning_siteContext context, Client supabase)
        {
            _context = context;
            _supabase = supabase;
        }

        public IActionResult OnGet()
        {
            var session = _supabase.Auth.CurrentSession;
            if (session == null)
            {
                return RedirectToPage("/login");
            }
            return Page();
        }

        [BindProperty]
        public Management Management { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Management.Add(Management);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}