using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using TP6.Repository;

namespace TP6_TL2.Controllers;

public class BudgetController : Controller
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IProductRepository _productRepository;

    public BudgetController(IBudgetRepository budgetRepository, IProductRepository productRepository)
    {
        _budgetRepository = budgetRepository;
        _productRepository = productRepository;
    }

    public ActionResult Index()
    {
        var budgets = _budgetRepository.getBudgets();
        return View(budgets);
    }
    
    [HttpGet]
    public ActionResult CreateBudget()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateBudget(Budget budget)
    {
        _budgetRepository.createBudget(budget);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult EditBudget(int idBudget)
    {
        return View(_budgetRepository.getBudgetById(idBudget));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditBudget(Budget budget)
    {
        _budgetRepository.updateBudget(budget.IdBudget, budget);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public ActionResult AddProducto(int idBudget)
    {
        return View(_budgetRepository.getBudgetById(idBudget));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddProduct(int idBudget, [FromForm] int quantity, [FromForm] int idProduct)
    {
        var product = _productRepository.getProductById(idProduct);
        BudgetProductDetail detail = new(product, quantity);            
        _budgetRepository.AddProduct(idBudget, detail);

        return RedirectToAction("AddProducto", _budgetRepository.getBudgetById(idBudget));
    }
    
    
}