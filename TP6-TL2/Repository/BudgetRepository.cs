using System.Globalization;
using Microsoft.Data.Sqlite;
using TP6.Models;

namespace TP6.Repository;

public class BudgetRepository : IBudgetRepository
{
    private readonly string connectionString= "Data Source=Database/Tienda.db;Cache=Shared";
        
    public void createBudget(Budget budget)
    {
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var query = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion)\" + \" VALUES (@NombreDestinatario, @FechaCreacion);";
            
            SqliteCommand command = new SqliteCommand(query, connection);

            command.Parameters.Add(new SqliteParameter("@NombreDestinatario", budget.ClientName));
            command.Parameters.Add(new SqliteParameter("@FechaCreacion", budget.DateCreated));
            
            command.ExecuteNonQuery();
            
            connection.Close();

        }
        
    }

    public List<Budget> getBudgets()
    {
        List<Budget> budgets = new();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            
            var query = "SELECT * FROM Presupuestos;";
            SqliteCommand command = new SqliteCommand(query, connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                DateOnly dateP = new();
                DateOnly.TryParseExact((string?)reader["FechaCreacion"],"yyyy-MM-dd",out dateP);

                var budget = new Budget()
                {
                    IdBudget = Convert.ToInt32(reader["idPresupuesto"]),
                    ClientName = reader["NombreDestinatario"].ToString(),
                    DateCreated = dateP,
                };
                
                budgets.Add(budget);
            }
            connection.Close();
        }

        return budgets;
    }

    public void updateBudget(int id, Budget budget)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var query = "UPDATE Presupuestos "
                               + "SET NombreDestinatario = (@NuevoDestinatario) "
                               + "WHERE idPresupuesto = (@idPresupuesto);;";
            
            SqliteCommand command = new SqliteCommand(query, connection);
            
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
            command.Parameters.Add(new SqliteParameter("@NuevoDestinatario", budget.ClientName));
            
            command.ExecuteNonQuery();
            
            connection.Close();
            
        }
    }
    
    public Budget getBudgetById(int id)
    {
        Budget budget = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Presupuestos WHERE idPresupuesto = (@idPresupuesto);";

                SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    budget.IdBudget = Convert.ToInt32(reader["idPresupuesto"]);
                    budget.ClientName = reader["NombreDestinatario"].ToString();
                    if (reader["FechaCreacion"] != DBNull.Value &&
                        DateTime.TryParse(reader["FechaCreacion"].ToString(), out DateTime dateCreated))
                    {
                        budget.DateCreated = DateOnly.FromDateTime(dateCreated); 
                    }
                    
                }

                connection.Close();
            }

            return budget;
    }

    public void deleteBudgetById(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var query = "DELETE FROM Presupuestos WHERE idPresupuesto = (@idPresupuesto);";

            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    
    public int GetQuantityOfProduct(int idBudget, int idProduct)
    {
        int quantity = 0;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var query = "SELECT cantidad "
                           + "FROM PresupuestosDetalle "
                           + "WHERE idPresupuesto = (@idPresupuesto) AND idProducto = (@idProducto);";

            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", idBudget));
            command.Parameters.Add(new SqliteParameter("@idProducto", idProduct));
            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                quantity = Convert.ToInt32(reader["Cantidad"]);
            }

            connection.Close();
        }

        return quantity;
    }

    public void AddProduct(int idBudget, BudgetProductDetail detail)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            string query;

            int quantity = GetQuantityOfProduct(idBudget, detail.Product.IdProduct);

            if (quantity > 0)
            {
                query = "UPDATE PresupuestosDetalle "
                        + "SET Cantidad = (@Cantidad) "
                        + "WHERE idPresupuesto = (@idPresupuesto) AND idProducto = (@idProducto);";
            }
            else
            {
                query = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) "
                        + "VALUES (@idPresupuesto, @idProducto, @Cantidad);";
            }

            quantity += detail.Quantity;

            connection.Open();
            SqliteCommand command = new(query, connection);
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", idBudget));
            command.Parameters.Add(new SqliteParameter("@idProducto", detail.Product.IdProduct));
            command.Parameters.Add(new SqliteParameter("@Cantidad", quantity));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }

   

        

        
    
}