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
    public class DeleteModel : PageModel
    {
        private readonly Learning_site.Data.Learning_siteContext _context;
        private readonly Client _supabase;

        public DeleteModel(Learning_site.Data.Learning_siteContext context, Client supabase)
        {
            _context = context;
            _supabase = supabase;
        }

        [BindProperty]
        public Management Management { get; set; } = new Management();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var session = _supabase.Auth.CurrentSession;
            if (session == null)
            {
                return RedirectToPage("/login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var management_from_db = await _context.Management.FirstOrDefaultAsync(m => m.Title == id);

            if (management_from_db == null)
            {
                return NotFound();
            }
            else
            {
                Management = management_from_db;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var management_to_delete = await _context.Management.FindAsync(id);
            if (management_to_delete != null)
            {
                _context.Management.Remove(management_to_delete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
