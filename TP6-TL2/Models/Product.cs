namespace TP6.Models;

public class Product
{
    private int idProduct;
    private string description;
    private double price;

    public Product(int idProduct, string description, double price)
    {
        this.idProduct = idProduct;
        this.description = description;
        this.price = price;
    }

    public Product()
    {
    }

    public int IdProduct
    {
        get => idProduct;
        set => idProduct = value;
    }

    public string Description
    {
        get => description;
        set => description = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double Price
    {
        get => price;
        set => price = value;
    }
}