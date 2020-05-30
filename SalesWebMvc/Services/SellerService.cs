using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;

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
      _context.Add(sl);
      _context.SaveChanges();
    }
  }
}
