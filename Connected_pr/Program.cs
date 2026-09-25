using Microsoft.Data.SqlClient;
using System.Xml.Linq;

namespace Connected_pr
{
    internal class Program
    {
        static public SqlConnection? conn = null;
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Storage;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            Menu.MainMenu();
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
