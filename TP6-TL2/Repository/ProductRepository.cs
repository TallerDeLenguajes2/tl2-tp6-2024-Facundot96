using System.Data;
using TP6.Models;
using Microsoft.Data.Sqlite;

namespace TP5.Repository;

public class ProductRepository : IProductRepository
{
    private readonly string connectionString= "Data Source=Database/Tienda.db;Cache=Shared";
    
    public void createProduct(Product product)
    {
        var query = $"INSERT INTO Productos (Descripcion, Precio) \nVALUES (@Descripcion, @Precio)";
        
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = new SqliteCommand(query, connection);
            
            command.Parameters.Add(new SqliteParameter("@Descripcion", product.Description));
            command.Parameters.Add(new SqliteParameter("@Precio", product.Price));
            

            command.ExecuteNonQuery();

            connection.Close();
        }
        
        
    }

    public void deleteProduct(int idProduct)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            
            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Productos WHERE IdProduct = {idProduct}";
            command.Parameters.Add(new SqliteParameter("@IdProduct", idProduct));
            
            connection.Open();
            
            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void updateProduct(int idProduct, Product product)
    {
        var query = $"UPDATE producto SET descripcion = @descripcion, precio = @precio WHERE IdProduct = {idProduct}";
        
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = new SqliteCommand(query, connection);
            
            command.Parameters.Add(new SqliteParameter("@Descripcion", product.Description));
            command.Parameters.Add(new SqliteParameter("@Precio", product.Price));
            

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public List<Product> getAllProducts()
    {
        var query = $"SELECT * FROM Productos";
        
        List<Product> products = new List<Product>();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(query, connection);
            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var product = new Product();
                    product.IdProduct = Convert.ToInt32(reader["IdProducto"]);
                    product.Description = reader["Descripcion"].ToString();
                    product.Price = (double)Convert.ToDecimal(reader["Precio"]);
                    products.Add(product);
                }
            }
            connection.Close();
        }

        return products;
    }

    public Product getProductById(int idProduct)
    {
        SqliteConnection connection = new SqliteConnection(connectionString);
        
        var query = $"SELECT * FROM Productos WHERE IdProducto = @idProduct";
        
        Product product = new Product();

        SqliteCommand command = connection.CreateCommand();
        
        command.CommandText = query;
        command.Parameters.Add(new SqliteParameter("@idProduct", idProduct));
        
        connection.Open();

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                product.IdProduct = Convert.ToInt32(reader["IdProducto"]);
                product.Description = reader["Descripcion"].ToString();
                product.Price = (double)Convert.ToDecimal(reader["Precio"]);
            }
        }
        
        connection.Close();

        return product;
    }
}