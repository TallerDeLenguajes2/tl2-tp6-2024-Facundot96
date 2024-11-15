using TP6.Models;

namespace TP6.Repository;

public interface IBudgetRepository
{
    public void createBudget(Budget budget);
    public List<Budget> getBudgets();
    public void updateBudget(int id, Budget budget);
    public Budget getBudgetById(int id);
    public void deleteBudgetById(int id);
    
    void AddProduct(int idBudget, BudgetProductDetail detail);
    
    
    
    public int GetQuantityOfProduct(int idBudget, int idProduct);
}