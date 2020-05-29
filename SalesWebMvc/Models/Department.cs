﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Models
{
  public class Department
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Seller> Sellers { get; set; } = new HashSet<Seller>();

    public Department()
    {
    }

    public Department(int id, string name, ICollection<Seller> sellers)
    {
      Id = id;
      Name = name;
    }

    public void AddSeller(Seller seller)
    {
      Sellers.Add(seller);
    }

    public double TotalSales(DateTime inicial, DateTime final)
    {
      return Sellers.Sum(seller => seller.TotalSales(inicial, final));
    }
  }
}
