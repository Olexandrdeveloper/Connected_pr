using System;
using System.Collections.Generic;
using System.Text;

namespace Connected_pr
{
    internal static class Menu
    {
        private static bool IsConnected() => Program.conn != null && Program.conn.State == System.Data.ConnectionState.Open;

        public static void MainMenu()
        {
            while (true)
            {
                Console.Write(
                    "====== MENU ======\n" +
                    " 0. Вихід\n" +
                    " 1. Під'єднатись до БД\n" +
                    " 2. Від'єднатись від БД\n" +
                    " 3. Показати\n" +
                    " 4. Вставити\n" +
                    " 5. Оновити\n" +
                    " 6. Видалити\n" +
                    " > ");
                byte choice = byte.TryParse(Console.ReadLine(), out byte result) ? result : (byte)0;
                Console.Clear();

                if (choice == 1)
                {
                    DatabaseOperations.ConnectToDatabase();
                }
                else if (choice == 2)
                {
                    DatabaseOperations.DisconnectFromDatabase();
                }
                else if (choice == 3)
                {
                    if (IsConnected()) SelectMenu();
                    else
                    {
                        Console.WriteLine("Спочатку підключіться");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else if (choice == 4)
                {
                    if (IsConnected()) InsertMenu();
                    else
                    {
                        Console.WriteLine("Спочатку підключіться");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else if (choice == 5)
                {
                    if (IsConnected()) UpdateMenu();
                    else
                    {
                        Console.WriteLine("Спочатку підключіться");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else if (choice == 6)
                {
                    if (IsConnected()) DeleteMenu();
                    else
                    {
                        Console.WriteLine("Спочатку підключіться");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else if (choice == 0)
                {
                    break;
                }
            }
        }

        private static void SelectMenu()
        {
            while (true)
            {
                Console.Write(
                    "====== SELECT MENU ======\n" +
                    " 0. Назад\n" +
                    " 1. Відображення інформації про товар\n" +
                    " 2. Відображення типів товарів\n" +
                    " 3. Відображення постачальників\n" +
                    " 4. Показати товар з максимальною кількістю\n" +
                    " 5. Показати товар з мінімальною кількістю\n" +
                    " 6. Показати товар з мінімальною собівартістю\n" +
                    " 7. Показати товар з максимальною собівартістю\n" +
                    " 8. Показати товари заданої категорії\n" +
                    " 9. Показати товари заданого постачальника\n" +
                    " 10. Показати товар, який знаходиться на складі найдовше з усіх\n" +
                    " 11. Показати середню кількість товарів за кожним типом\n" +
                    " 12. Показати інформацію про постачальника з найбільшою кількістю товарів\n" +
                    " 13. Показати інформацію про постачальника з найменшою кількістю товарів\n" +
                    " 14. Показати інформацію про тип товару з найбільшою кількістю одиниць\n" +
                    " 15. Показати інформацію про тип товарів з найменшою кількістю товарів\n" +
                    " 16. Показати товари, з постачання яких минула задана кількість днів\n" +
                    " > ");
                byte choice = byte.TryParse(Console.ReadLine(), out byte result) ? result : (byte)0;
                Console.Clear();

                if (choice == 0)
                {
                    break;
                }
                else
                {
                    DatabaseOperations.ExecuteSelectCommand(choice);
                }
            }
        }

        private static void InsertMenu()
        {
            while (true)
            {
                Console.Write(
                    "====== INSERT MENU ======\n" +
                    " 0. Назад\n" +
                    " 1. Вставити новий товар\n" +
                    " 2. Вставити новий тип товару\n" +
                    " 3. Вставити нового постачальника\n" +
                    " > ");
                byte choice = byte.TryParse(Console.ReadLine(), out byte result) ? result : (byte)0;
                Console.Clear();

                if (choice == 0)
                {
                    break;
                }
                else
                {
                    DatabaseOperations.ExecuteInsertCommand(choice);
                }
            }
        }

        private static void UpdateMenu()
        {
            while (true)
            {
                Console.Write(
                    "====== UPDATE MENU ======\n" +
                    " 0. Назад\n" +
                    " 1. Оновити інформацію про товар\n" +
                    " 2. Оновити інформацію про тип товару\n" +
                    " 3. Оновити інформацію про постачальника\n" +
                    " > ");
                byte choice = byte.TryParse(Console.ReadLine(), out byte result) ? result : (byte)0;
                Console.Clear();

                if (choice == 0)
                {
                    break;
                }
                else
                {
                    DatabaseOperations.ExecuteUpdateCommand(choice);
                }
            }
        }

        private static void DeleteMenu()
        {
            while (true)
            {
                Console.Write(
                    "====== DELETE MENU ======\n" +
                    " 0. Назад\n" +
                    " 1. Видалити товар\n" +
                    " 2. Видалити тип товару\n" +
                    " 3. Видалити постачальника\n" +
                    " > ");
                byte choice = byte.TryParse(Console.ReadLine(), out byte result) ? result : (byte)0;
                Console.Clear();

                if (choice == 0)
                {
                    break;
                }
                else
                {
                    DatabaseOperations.ExecuteDeleteCommand(choice);
                }
            }
        }
    }
}
