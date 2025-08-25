using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Learning_site.Models;

namespace Learning_site.Data
{
    public class Learning_siteContext : DbContext
    {
        public Learning_siteContext (DbContextOptions<Learning_siteContext> options)
            : base(options)
        {
        }

        public DbSet<Learning_site.Models.management> management { get; set; } = default!;
    }
}
