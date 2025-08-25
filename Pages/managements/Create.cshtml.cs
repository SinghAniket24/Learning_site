using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Learning_site.Data;
using Learning_site.Models;

namespace Learning_site.Pages.managements
{
    public class CreateModel : PageModel
    {
        private readonly Learning_site.Data.Learning_siteContext _context;

        public CreateModel(Learning_site.Data.Learning_siteContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public management management { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.management.Add(management);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
