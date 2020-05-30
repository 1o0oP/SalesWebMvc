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

    public Seller FindById(int id)
    {
      return _context.Seller.Where(sl => sl.Id == id).Single();
      // Forma mais curta: _context.Seller.FirstOrDefault(sl => sl.Id == id);
    }

    public void Remove(int id)
    {
      _context.Seller.Remove(_context.Seller.Find(id)); // Encontra pela chave primária
      _context.SaveChanges(); // Efetiva mudança no banco de dados
    }
  }
}
