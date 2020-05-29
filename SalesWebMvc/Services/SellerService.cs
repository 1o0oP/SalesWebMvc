﻿using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
  public class SellerService
  {
    private readonly SalesWebMvcContext _context;

    public SellerService(SalesWebMvcContext context)
    {
      _context = context;
    }
    
    public HashSet<Seller> FindAll()
    {
      return _context.Seller.ToHashSet();
    }

    public void Insert(Seller sl)
    {
      sl.Department = _context.Department.First();
      _context.Add(sl);
      _context.SaveChanges();
    }
  }
}
