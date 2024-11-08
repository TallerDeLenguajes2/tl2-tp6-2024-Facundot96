using TP6.Models;

namespace TP5.Repository;

public interface IBudgetRepository
{
    public void createBudget(Budget budget);
    public List<Budget> getBudgets();
    public void updateBudget(int id, BudgetProductDetail budgetProductDetail);
    public Budget getBudgetById(int id);
    public void deleteBudgetById(int id);
}