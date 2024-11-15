using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using TP6.Repository;

namespace TP6_TL2.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public ActionResult Index()
    {
        var products = _productRepository.getAllProducts();
        return View(products);
    }

    [HttpGet]
    public ActionResult CreateProduct()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateProduct(Product product)
    {
        _productRepository.createProduct(product);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult EditProduct(int idProduct)
    {
        return View(_productRepository.getProductById(idProduct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditProduct(Product product)
    {
        _productRepository.updateProduct(product.IdProduct, product);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult DeleteProduct(int idProduct)
    {
        return View(_productRepository.getProductById(idProduct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int idProduct)
    {
        _productRepository.deleteProduct(idProduct);
        return RedirectToAction("Index");
    }
    
}