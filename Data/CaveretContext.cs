using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Caveret.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Caveret.Data
{
    public class CaveretContext : IdentityDbContext<IdentityUser>
    {
        public CaveretContext (DbContextOptions<CaveretContext> options) : base(options)
        {
        }

        public DbSet<Caveret.Models.Catagories> Catagories { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<Caveret.Models.Stock> Stock { get; set; }
        public DbSet<Shops> Shops { get; set; }
    }
}
