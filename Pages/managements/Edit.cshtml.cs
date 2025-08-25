using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Learning_site.Data;
using Learning_site.Models;

namespace Learning_site.Pages.managements
{
    public class EditModel : PageModel
    {
        private readonly Learning_site.Data.Learning_siteContext _context;

        public EditModel(Learning_site.Data.Learning_siteContext context)
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

            var management =  await _context.management.FirstOrDefaultAsync(m => m.Title == id);
            if (management == null)
            {
                return NotFound();
            }
            management = management;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(management).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!managementExists(management.Title))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool managementExists(string id)
        {
            return _context.management.Any(e => e.Title == id);
        }
    }
}
