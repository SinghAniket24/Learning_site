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
    public class IndexModel : PageModel
    {
        private readonly Learning_site.Data.Learning_siteContext _context;

        public IndexModel(Learning_site.Data.Learning_siteContext context)
        {
            _context = context;
        }

        public IList<management> management { get;set; } = default!;

        public async Task OnGetAsync()
        {
            management = await _context.management.ToListAsync();
        }
    }
}
