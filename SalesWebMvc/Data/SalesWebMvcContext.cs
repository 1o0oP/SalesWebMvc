﻿using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;

namespace SalesWebMvc.Data
{
  public class SalesWebMvcContext : DbContext
  {
    public DbSet<Department> Department { get; set; }
    public DbSet<Seller> Seller { get; set; }
    public DbSet<SalesRecord> SalesRecord { get; set; }

    public SalesWebMvcContext(DbContextOptions<SalesWebMvcContext> options)
        : base(options)
    {
    }
  }
}
