using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using System.Collections.Generic;

namespace SalesWebMvc.Controllers
{
  public class SellersController : Controller
  {
    private readonly SellerService _sellerService;

    public SellersController(SellerService sellerService)
    {
      _sellerService = sellerService;
    }

    public IActionResult Index()
    {
      HashSet<Seller> ss = _sellerService.FindAll();
      return View(ss);
    }
  }
}