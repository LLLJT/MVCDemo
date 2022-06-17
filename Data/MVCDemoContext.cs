using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCDemo.Models;

namespace MVCDemo.Data
{
    public class MVCDemoContext : DbContext
    {
        public MVCDemoContext (DbContextOptions<MVCDemoContext> options)
            : base(options)
        {
        }

        public DbSet<MVCDemo.Models.Movie> Movie { get; set; }
    }
}
