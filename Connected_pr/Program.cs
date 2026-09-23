using Microsoft.Data.SqlClient;
using System.Xml.Linq;

namespace Connected_pr
{
    internal class Program
    {
        static SqlConnection? conn = null;
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Storage;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            while (true)
            {
                Console.WriteLine("" +
                    "1. Під'єднатись до БД\n" +
                    "2. Від'єднатись від БД\n" +
                    "3. Відображення інформації про товар\n" +
                    "4. Відображення типів товарів\n" +
                    "5. Відображення постачальників\n" +
                    "6. Показати товар з максимальною кількістю\n" +
                    "7. Показати товар з мінімальною кількістю\n" +
                    "8. Показати товар з мінімальною собівартістю\n" +
                    "9. Показати товар з максимальною собівартістю\n" +
                    "10. Показати товари заданої категорії\n" +
                    "11. Показати товари заданого постачальника\n" +
                    "12. Показати товар, який знаходиться на складі найдовше з усіх\n" +
                    "13. Показати середню кількість товарів за кожним типом\n" +
                    "товару.");
                byte choice = byte.TryParse(Console.ReadLine(), out byte result) ? result : (byte)0;
                Console.Clear();

                if (choice == 1)
                {
                    ConnectToDatabase();
                }
                else if (choice == 2)
                {
                    DisconnectFromDatabase();
                }
                else
                {
                    if (conn == null || conn.State != System.Data.ConnectionState.Open)
                    {
                        Console.WriteLine("Будь ласка, спочатку під'єднайтесь до БД.");
                        continue;
                    }
                    try
                    {
                        SqlCommand command = choice switch
                        {
                            3 => new SqlCommand("SELECT * FROM Product", conn),
                            4 => new SqlCommand("SELECT * FROM ProductType", conn),
                            5 => new SqlCommand("SELECT * FROM Suplier", conn),
                            6 => new SqlCommand("SELECT TOP 1 * FROM Product ORDER BY Quantity DESC", conn),
                            7 => new SqlCommand("SELECT TOP 1 * FROM Product ORDER BY Quantity ASC", conn),
                            8 => new SqlCommand("SELECT TOP 1 * FROM Product ORDER BY Price ASC", conn),
                            9 => new SqlCommand("SELECT TOP 1 * FROM Product ORDER BY Price DESC", conn),
                            10 => new SqlCommand("SELECT * FROM Product WHERE ProductTypeId = @ProductTypeId", conn),
                            11 => new SqlCommand("SELECT * FROM Product WHERE SuplierId = @SuplierId", conn),
                            12 => new SqlCommand("SELECT TOP 1 * FROM Product ORDER BY CreatedDate DESC", conn),
                            13 => new SqlCommand("SELECT ProductTypeId, AVG(Quantity) as AverageQuantity FROM Product GROUP BY ProductTypeId", conn),
                            _ => null
                        };

                        if (command != null)
                        {
                            if (choice == 10)
                            {
                                Console.Write("Введіть ID типу продукту (ProductTypeId): ");
                                int productTypeId = int.Parse(Console.ReadLine());
                                command.Parameters.AddWithValue("@ProductTypeId", productTypeId);
                            }
                            else if (choice == 11)
                            {
                                Console.Write("Введіть ID постачальника (SuplierId): ");
                                int suplierId = int.Parse(Console.ReadLine());
                                command.Parameters.AddWithValue("@SuplierId", suplierId);
                            }

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                int line = 0;
                                while (reader.Read())
                                {
                                    if (line == 0)
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            Console.Write($"{reader.GetName(i)}\t");
                                        }
                                        Console.WriteLine();
                                        line++;
                                    }
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        Console.Write(reader[i] + "\t");
                                    }
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Помилка виконання запиту: {ex.Message}");
                    }
                }
            }
        }

        static void ConnectToDatabase()
        {
            try
            {
                conn?.Open();
                Console.WriteLine("Під'єднання до БД успішне.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під'єднання до БД: {ex.Message}");
            }
        }

        static void DisconnectFromDatabase()
        {
            try
            {
                conn?.Close();
                Console.WriteLine("Від'єднання від БД успішне.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка від'єднання від БД: {ex.Message}");
            }
        }
    }
}


    //CREATE TABLE [dbo].[ProductType]
    //(
    //	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
    //	[Name] NVARCHAR(100) NOT NULL
    //)
    //GO
    //CREATE TABLE [dbo].[Suplier]
    //(
    //	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
    //	[Name] NVARCHAR(100) NOT NULL
    //)
    //GO
    //CREATE TABLE [dbo].[Product]
    //(
    //	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
    //	[Name] NVARCHAR(200) NOT NULL,
    //	[ProductTypeId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[ProductType]([Id]),
    //	[SuplierId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Suplier]([Id]),
    //	[Quantity] INT NOT NULL DEFAULT 0,
    //	[Price] DECIMAL(18, 2) NOT NULL,
    //	[CreatedDate] DATETIME NOT NULL DEFAULT GETDATE()
    //)
