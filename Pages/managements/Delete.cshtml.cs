using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Learning_site.Data;
using Learning_site.Models;

namespace Learning_site.Pages.managements
{
    public class DeleteModel : PageModel
    {
        private readonly Learning_site.Data.Learning_siteContext _context;

        public DeleteModel(Learning_site.Data.Learning_siteContext context)
        {
            _context = context;
        }

        [BindProperty]
        public management management { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var management = await _context.management.FirstOrDefaultAsync(m => m.Title == id);

            if (management == null)
            {
                return NotFound();
            }
            else
            {
                management = management;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var management = await _context.management.FindAsync(id);
            if (management != null)
            {
                management = management;
                _context.management.Remove(management);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
