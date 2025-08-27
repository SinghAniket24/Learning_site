using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Learning_site.Data;
using Learning_site.Models;
using Supabase;

namespace Learning_site.Pages.Managements
{
    public class IndexModel : PageModel
    {
        private readonly Learning_site.Data.Learning_siteContext _context;
        private readonly Client _supabase;

        public IndexModel(Learning_site.Data.Learning_siteContext context, Client supabase)
        {
            _context = context;
            _supabase = supabase;
        }

        public IList<Management> Management { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var session = _supabase.Auth.CurrentSession;
            if (session == null)
            {
                return RedirectToPage("/login");
            }

            Management = await _context.Management.ToListAsync();
            return Page();
        }
    }
}