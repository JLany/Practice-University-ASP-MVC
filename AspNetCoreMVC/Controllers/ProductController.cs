using AspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC.Controllers;

public class ProductController : Controller
{
    private readonly ProductRepository _productRepository;

    public ProductController(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IActionResult All()
    {
        return View(_productRepository.GetAll());
    }

    public IActionResult Details(int id)
    {
        return View(_productRepository.Get(id));
    }
}
