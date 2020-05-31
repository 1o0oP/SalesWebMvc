using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        return RedirectToAction(nameof(Error), new { message = "Id not provided" });
      }
      else
      {
        var sl = _sellerService.FindById(id.Value);
        if (sl == null)
        {
          return RedirectToAction(nameof(Error), new { message = "Id not found" });
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
        return RedirectToAction(nameof(Error), new { message = "Id not provided" });
      }
      else
      {
        var sl = _sellerService.FindById(id.Value);
        if (sl == null)
        {
          return RedirectToAction(nameof(Error), new { message = "Id not found" });
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
        return RedirectToAction(nameof(Error), new { message = "Id not provided" });
      }
      else
      {
        var seller = _sellerService.FindById(id.Value);
        if (seller == null)
        {
          return RedirectToAction(nameof(Error), new { message = "Id not found" });
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
        return RedirectToAction(nameof(Error), new { message = "Ids mismatch" });
      }
      else
      {
        try
        {
          _sellerService.Update(seller); // Pode lançar excessões
          return RedirectToAction(nameof(Index));
        }
        catch (ApplicationException e)
        {
          return RedirectToAction(nameof(Error), new { message = e.Message });
        }
      }
    }

    public IActionResult Error(string message)
    {
      var viewModel = new ErrorViewModel
      {
        Message = message,
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
      };
      return View(viewModel);
    }
  }
}