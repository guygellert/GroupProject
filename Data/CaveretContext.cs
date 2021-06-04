﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Caveret.Models;

namespace Caveret.Data
{
    public class CaveretContext : DbContext
    {
        public CaveretContext (DbContextOptions<CaveretContext> options) : base(options)
        {
        }

        public DbSet<Caveret.Models.Catagories> Catagories { get; set; }

        public DbSet<Products> Products { get; set; }
    }
}
