using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System.Collections.Generic;

namespace SalesWebMvc.Controllers
{
  public class SellersController : Controller
  {
    private readonly SellerService _sellerService;
    private readonly DepartmentService _departmentService;

    public SellersController(SellerService sellerService, DepartmentService departmentService)
    {
      _sellerService = sellerService;
      _departmentService = departmentService;
    }

    public IActionResult Index() // Resposta ao clicar em "Seller" no menu Nav
    {
      HashSet<Seller> sellers = _sellerService.FindAll();
      return View(sellers);
    }

    public IActionResult Create() // Resposta ao clicar em "Create New" na página "/Sellers"
    {
      var departments = _departmentService.FindAll();
      var viewModel = new SellerFormViewModel { Departments = departments };
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Evita envio de dados maliciosos
    public IActionResult Create(Seller seller) // Resposta eo clicar em "Create" na página "/Sellers/Create"
    {
      _sellerService.Insert(seller);
      return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id) // Resposta ao clicar em "Delete" na página "/Sellers"
    {
      if (id == null)
      {
        return NotFound();
      }
      else
      {
        var sl = _sellerService.FindById(id.Value);
        if (sl == null)
        {
          return NotFound();
        }
        else
        {
          return View(sl);
        }
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id) // Resposta ao clicar em "Delete" na página "/Sellers/Delete"
    {
      _sellerService.Remove(id); // Remove o vendedor do banco de dados
      return RedirectToAction(nameof(Index)); // Retorna para a página anterior
    }

    public IActionResult Details(int? id) // Resposta ao clicar em "Details" na página "/Sellers"
    {
      if (id == null)
      {
        return NotFound();
      }
      else
      {
        var sl = _sellerService.FindById(id.Value);
        if (sl == null)
        {
          return NotFound();
        }
        else
        {
          return View(sl);
        }
      }
    }

    public IActionResult Edit(int? id) // Resposta ao clicar em "Edit" na página "/Sellers"
    {
      if (id == null)
      {
        return NotFound();
      }
      else
      {
        var seller = _sellerService.FindById(id.Value);
        if (seller == null)
        {
          return NotFound();
        }
        else
        {
          List<Department> departments = _departmentService.FindAll();
          SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
          return View(viewModel);
        }
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Evita envio de dados maliciosos
    public IActionResult Edit(int id, Seller seller) // Resposta ao clicar em "Save" na página "/Sellers/Edit"
    {
      if (id != seller.Id)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          _sellerService.Update(seller); // Pode lançar excessões
          return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException e)
        {
          return NotFound();
        }
        catch (DbConcurrencyException e)
        {
          return BadRequest();
        }
      }
    }
  }
}