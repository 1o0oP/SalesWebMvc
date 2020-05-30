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

    public IActionResult Index()
    {
      HashSet<Seller> ss = _sellerService.FindAll();
      return View(ss);
    }

    public IActionResult Create()
    {
      var departments = _departmentService.FindAll();
      var viewModel = new SellerFormViewModel { Departments = departments };
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Evita envio de dados maliciosos
    public IActionResult Create(SellerFormViewModel sl)
    {
      _sellerService.Insert(sl.Seller);
      return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
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
    public IActionResult Delete(int id)
    {
      _sellerService.Remove(id); // Remove o vendedor do banco de dados
      return RedirectToAction(nameof(Index)); // Retorna para a página anterior
    }

    public IActionResult Details(int? id)
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

    //public IActionResult Edit()
  }
}