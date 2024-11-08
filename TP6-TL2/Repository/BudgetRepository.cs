using System.Globalization;
using Microsoft.Data.Sqlite;
using TP6.Models;

namespace TP5.Repository;

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

    public void updateBudget(int id, BudgetProductDetail budgetProductDetail)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var query = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)"+" VALUES (@idPresupuesto, @idProducto, @Cantidad);";
            
            SqliteCommand command = new SqliteCommand(query, connection);
            
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
            command.Parameters.Add(new SqliteParameter("@idProducto", budgetProductDetail.Product.IdProduct));
            command.Parameters.Add(new SqliteParameter("@Cantidad", budgetProductDetail.Quantity));
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

                var query = "SELECT P.idPresupuesto, P.NombreDestinatario, P.FechaCreacion, PR.idProducto, PR.Descripcion "
                               + "AS Producto, PR.Precio, PD.Cantidad, (PR.Precio * PD.Cantidad) AS Subtotal "
                               + "FROM Presupuestos P "
                               + "JOIN PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto "
                               + "JOIN Productos PR ON PD.idProducto = PR.idProducto "
                               + "WHERE P.idPresupuesto = (@idPresupuesto);";

                SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (budget.ClientName == null || budget.DateCreated == new DateOnly(1, 1, 1))
                    {
                        DateOnly date = new();
                        DateOnly.TryParseExact((string?)reader["FechaCreacion"], "yyyy-MM-dd", out date);
                        budget.IdBudget = Convert.ToInt32(reader["idPresupuesto"]);
                        budget.ClientName = reader["NombreDestinatario"].ToString();
                        budget.DateCreated = date;
                        budget.Details = new List<BudgetProductDetail>();
                    }

                    var product = new Product
                    {
                        IdProduct = Convert.ToInt32(reader["idProducto"]),
                        Description = reader["Producto"].ToString(),
                        Price = Convert.ToInt32(reader["Precio"])
                    };

                    var detail = new BudgetProductDetail()
                    {
                        Product = product,
                        Quantity = Convert.ToInt32(reader["Cantidad"]),
                    };

                    budget.Details.Add(detail);
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
}