using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;
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
    
    public async Task<List<Seller>> FindAllAsync() 
    { // Busca a tabela Sellers no banco de dados de forma assíncrona
      return await _context.Seller.ToListAsync();
    }

    public async Task InsertAsync(Seller sl)
    {
      _context.Add(sl);
      await _context.SaveChangesAsync();
    }

    public async Task<Seller> FindByIdAsync(int id)
    {
      return await _context.Seller.Include(sl => sl.Department).FirstOrDefaultAsync(sl => sl.Id == id);
      // Outra forma: _context.Seller.Include(sl => sl.Department).Where(sl => sl.Id == id).Single();
      // Include: Olha o DepartmentId -> Abre a tabela departments -> Encontra pelo DepartmentId -> Retorna para sl?
    }

    public async Task RemoveAsync(int id)
    {
      try
      {
        _context.Seller.Remove(await _context.Seller.FindAsync(id)); // Encontra pela chave primária
        await _context.SaveChangesAsync(); // Efetiva mudança no banco de dados
      }
      catch (DbUpdateException e) // Erro da chave estrangeira em nível de serviço
      { // Trata a excessão quando tentamos apagar dados interligados do banco de dados
        throw new IntegrityException(e.Message);
      }
    }

    public async Task UpdateAsync(Seller seller)
    {
      if (! await _context.Seller.AnyAsync(sl => sl.Id == seller.Id))
      {
        throw new NotFoundException("Id not found");
      }
      else
      {
        try
        {
          _context.Update(seller);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
          throw new DbConcurrencyException(e.Message);
          // Trata excessões da camada de dados na camada de serviços
        }
      }
    }
  }
}
