using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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

    public async Task<IActionResult> Index() // O nome IndexAsync é proibido no Controller
    { // Resposta ao clicar em "Seller" no menu Nav
      List<Seller> sellers = await _sellerService.FindAllAsync();
      return View(sellers);
    }

    public async Task<IActionResult> Create() // Resposta ao clicar em "Create New" na página "/Sellers"
    {
      var departments = await _departmentService.FindAllAsync();
      var viewModel = new SellerFormViewModel { Departments = departments };
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Evita envio de dados maliciosos
    public async Task<IActionResult> Create(Seller seller) // Resposta eo clicar em "Create" na página "/Sellers/Create"
    {
      if (!ModelState.IsValid) // Valida as entradas sem precisar de JavaScript
      {
        var departments = await _departmentService.FindAllAsync();
        var viewModel = new SellerFormViewModel
        {
          Seller = seller,
          Departments = departments
        };
        return View(viewModel);
      }
      // Caso contrário
      await _sellerService.InsertAsync(seller);
      return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id) // Resposta ao clicar em "Delete" na página "/Sellers"
    {
      if (id == null)
      {
        return RedirectToAction(nameof(Error), new { message = "Id not provided" });
      }
      else
      {
        var sl = await _sellerService.FindByIdAsync(id.Value);
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
    public async Task<IActionResult> Delete(int id) // Resposta ao clicar em "Delete" na página "/Sellers/Delete"
    {
      try
      {
        await _sellerService.RemoveAsync(id); // Remove o vendedor do banco de dados
        return RedirectToAction(nameof(Index)); // Retorna para a página anterior
      }
      catch (IntegrityException e) // Erro da chave extrangeira em nível de controlador
      { // Trantando o erro que surge ao tentarmos deletar dados interligados do banco de dados
        return RedirectToAction(nameof(Error), new { message = e.Message });
      }
    }

    public async Task<IActionResult> Details(int? id) // Resposta ao clicar em "Details" na página "/Sellers"
    {
      if (id == null)
      {
        return RedirectToAction(nameof(Error), new { message = "Id not provided" });
      }
      else
      {
        var sl = await _sellerService.FindByIdAsync(id.Value);
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

    public async Task<IActionResult> Edit(int? id) // Resposta ao clicar em "Edit" na página "/Sellers"
    {
      if (id == null)
      {
        return RedirectToAction(nameof(Error), new { message = "Id not provided" });
      }
      else
      {
        var seller = await _sellerService.FindByIdAsync(id.Value);
        if (seller == null)
        {
          return RedirectToAction(nameof(Error), new { message = "Id not found" });
        }
        else
        {
          List<Department> departments = await _departmentService.FindAllAsync();
          SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
          return View(viewModel);
        }
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Evita envio de dados maliciosos
    public async Task<IActionResult> Edit(int id, Seller seller) // Resposta ao clicar em "Save" na página "/Sellers/Edit"
    {
      if (!ModelState.IsValid) // Valida as entradas sem precisar de JavaScript
      {
        var departments = await _departmentService.FindAllAsync();
        var viewModel = new SellerFormViewModel
        {
          Seller = seller,
          Departments = departments
        };
        return View(viewModel);
      }
      // Caso contrário
      if (id != seller.Id)
      {
        return RedirectToAction(nameof(Error), new { message = "Ids mismatch" });
      }
      else
      {
        try
        {
          await _sellerService.UpdateAsync(seller); // Pode lançar excessões
          return RedirectToAction(nameof(Index));
        }
        catch (ApplicationException e)
        {
          return RedirectToAction(nameof(Error), new { message = e.Message });
        }
      }
    }

    public IActionResult Error(string message)
    { // Só precisa de async quando for acessar o banco de dados
      var viewModel = new ErrorViewModel
      {
        Message = message,
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
      };
      return View(viewModel);
    }
  }
}