using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Connected_pr
{
    internal static class DatabaseOperations
    {
        private static readonly Dictionary<string, string> selectCommands = new Dictionary<string, string>
        {
            { "SELECT_1", "SELECT * FROM Product" },
            { "SELECT_2", "SELECT * FROM ProductType" },
            { "SELECT_3", "SELECT * FROM Suplier" },
            { "SELECT_4", "SELECT TOP 1 * FROM Product ORDER BY Quantity DESC" },
            { "SELECT_5", "SELECT TOP 1 * FROM Product ORDER BY Quantity ASC" },
            { "SELECT_6", "SELECT TOP 1 * FROM Product ORDER BY Price ASC" },
            { "SELECT_7", "SELECT TOP 1 * FROM Product ORDER BY Price DESC" },
            { "SELECT_8", "SELECT * FROM Product WHERE ProductTypeId = @ProductTypeId" },
            { "SELECT_9", "SELECT * FROM Product WHERE SuplierId = @SuplierId" },
            { "SELECT_10", "SELECT TOP 1 * FROM Product ORDER BY CreatedDate DESC" },
            { "SELECT_11", "SELECT ProductTypeId, AVG(Quantity) as AverageQuantity FROM Product GROUP BY ProductTypeId" },
            { "SELECT_12", "SELECT SuplierId, COUNT(*) as ProductCount FROM Product GROUP BY SuplierId ORDER BY ProductCount DESC" },
            { "SELECT_13", "SELECT SuplierId, COUNT(*) as ProductCount FROM Product GROUP BY SuplierId ORDER BY ProductCount ASC" },
            { "SELECT_14", "SELECT ProductTypeId, SUM(Quantity) as TotalQuantity FROM Product GROUP BY ProductTypeId ORDER BY TotalQuantity DESC" },
            { "SELECT_15", "SELECT ProductTypeId, SUM(Quantity) as TotalQuantity FROM Product GROUP BY ProductTypeId ORDER BY TotalQuantity ASC" },
            { "SELECT_16", "SELECT * FROM Product WHERE DATEDIFF(day, CreatedDate, GETDATE()) > @DaysPassed" }
        };

        public static void ConnectToDatabase()
        {
            try
            {
                Program.conn?.Open();
                Console.WriteLine("Успішно");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }

        public static void DisconnectFromDatabase()
        {
            try
            {
                Program.conn?.Close();
                Console.WriteLine("Відключено");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }

        public static void ExecuteSelectCommand(int id)
        {
            string key = $"SELECT_{id}";
            if (!selectCommands.ContainsKey(key)) return;

            Dictionary<string, object> p = null;
            if (id == 8)
            {
                Console.Write("ID типу: ");
                p = new Dictionary<string, object> { { "@ProductTypeId", int.Parse(Console.ReadLine()) } };
            }
            else if (id == 9)
            {
                Console.Write("ID постачальника: ");
                p = new Dictionary<string, object> { { "@SuplierId", int.Parse(Console.ReadLine()) } };
            }
            else if (id == 16)
            {
                Console.Write("Днів: ");
                p = new Dictionary<string, object> { { "@DaysPassed", int.Parse(Console.ReadLine()) } };
            }

            Exec(selectCommands[key], p, true);
            Console.ReadKey();
            Console.Clear();
        }

        public static void ExecuteInsertCommand(int choice)
        {
            string q = "";
            var p = new Dictionary<string, object>();

            if (choice == 1)
            {
                Console.Write("Назва: ");
                p.Add("@Name", Console.ReadLine());
                Console.Write("ID типу: ");
                p.Add("@ProductTypeId", int.Parse(Console.ReadLine()));
                Console.Write("ID постачальника: ");
                p.Add("@SuplierId", int.Parse(Console.ReadLine()));
                Console.Write("Кількість: ");
                p.Add("@Quantity", int.Parse(Console.ReadLine()));
                Console.Write("Ціна: ");
                p.Add("@Price", decimal.Parse(Console.ReadLine()));
                q = "INSERT INTO Product (Name, ProductTypeId, SuplierId, Quantity, Price) VALUES (@Name, @ProductTypeId, @SuplierId, @Quantity, @Price)";
            }
            else if (choice == 2)
            {
                Console.Write("Назва типу: ");
                p.Add("@Name", Console.ReadLine());
                q = "INSERT INTO ProductType (Name) VALUES (@Name)";
            }
            else if (choice == 3)
            {
                Console.Write("Назва постачальника: ");
                p.Add("@Name", Console.ReadLine());
                q = "INSERT INTO Suplier (Name) VALUES (@Name)";
            }

            Exec(q, p, false);
            Console.WriteLine("Додано");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ExecuteUpdateCommand(int choice)
        {
            string q = "";
            var p = new Dictionary<string, object>();

            if (choice == 1)
            {
                Console.Write("ID: ");
                p.Add("@Id", int.Parse(Console.ReadLine()));
                Console.Write("Назва: ");
                p.Add("@Name", Console.ReadLine());
                Console.Write("ID типу: ");
                p.Add("@ProductTypeId", int.Parse(Console.ReadLine()));
                Console.Write("ID постачальника: ");
                p.Add("@SuplierId", int.Parse(Console.ReadLine()));
                Console.Write("Кількість: ");
                p.Add("@Quantity", int.Parse(Console.ReadLine()));
                Console.Write("Ціна: ");
                p.Add("@Price", decimal.Parse(Console.ReadLine()));
                q = "UPDATE Product SET Name = @Name, ProductTypeId = @ProductTypeId, SuplierId = @SuplierId, Quantity = @Quantity, Price = @Price WHERE Id = @Id";
            }
            else if (choice == 2)
            {
                Console.Write("ID: ");
                p.Add("@Id", int.Parse(Console.ReadLine()));
                Console.Write("Назва: ");
                p.Add("@Name", Console.ReadLine());
                q = "UPDATE ProductType SET Name = @Name WHERE Id = @Id";
            }
            else if (choice == 3)
            {
                Console.Write("ID: ");
                p.Add("@Id", int.Parse(Console.ReadLine()));
                Console.Write("Назва: ");
                p.Add("@Name", Console.ReadLine());
                q = "UPDATE Suplier SET Name = @Name WHERE Id = @Id";
            }

            Exec(q, p, false);
            Console.WriteLine("Оновлено");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ExecuteDeleteCommand(int choice)
        {
            Console.Write("ID: ");
            var p = new Dictionary<string, object> { { "@Id", int.Parse(Console.ReadLine()) } };
            string q = "";
            if (choice == 1) q = "DELETE FROM Product WHERE Id = @Id";
            else if (choice == 2) q = "DELETE FROM ProductType WHERE Id = @Id";
            else if (choice == 3) q = "DELETE FROM Suplier WHERE Id = @Id";

            Exec(q, p, false);
            Console.WriteLine("Видалено");
            Console.ReadKey();
            Console.Clear();
        }

        private static void Exec(string query, Dictionary<string, object> parameters, bool isRead)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, Program.conn))
                {
                    if (parameters != null)
                        foreach (var kv in parameters) cmd.Parameters.AddWithValue(kv.Key, kv.Value);

                    if (isRead)
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++) Console.Write($"{reader.GetName(i)}: {reader[i]} \t");
                                Console.WriteLine();
                            }
                        }
                    }
                    else cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}