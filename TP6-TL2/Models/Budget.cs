namespace TP6.Models;

public class Budget
{
    private int idBudget;
    private string clientName;
    private List<BudgetProductDetail> details;
    private DateOnly dateCreated;

    public Budget(string clientName, List<BudgetProductDetail> details, DateOnly date)
    {
        this.clientName = clientName;
        this.details = details;
        dateCreated = date;
    }

    public Budget()
    {
    }

    
    public int IdBudget
    {
        get => idBudget;
        set => idBudget = value;
    }

    public string ClientName
    {
        get => clientName;
        set => clientName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<BudgetProductDetail> Details
    {
        get => details;
        set => details = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DateOnly DateCreated
    {
        get => dateCreated;
        set => dateCreated = value;
    }

    public float BudgetAmount()
    {
        float amount = 0;

        foreach (var detail in details)
        {
            amount = (float)(detail.Product.Price * detail.Quantity);
        }

        return amount;
    }

    public float TotalAmountWithTaxes()
    {
        return (float)(BudgetAmount() * 1.21);
    }

    public int quantityOfProducts()
    {
        return details.Count;
    }
}