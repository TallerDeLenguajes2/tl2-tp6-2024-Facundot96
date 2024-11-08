namespace TP6.Models;

public class BudgetProductDetail
{
    private Product product;
    private int quantity;

    public BudgetProductDetail(Product product, int quantity)
    {
        this.product = product;
        this.quantity = quantity;
    }

    public BudgetProductDetail()
    {
    }

    public Product Product
    {
        get => product;
        set => product = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Quantity
    {
        get => quantity;
        set => quantity = value;
    }
}