using System;
using System.Data.SqlClient;

namespace Library
{
    class Program
    {      
        static void StudentsChoose()
        {
            var connectionString = @"Data Source = JONATHAN-KENT\SQLEXPRESS; 
                                     Initial Catalog = LibrarySQL;
                                     Trusted_Connection=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = @"select FirstName + ' ' + LastName as Name
                              FROM Students";

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine($"{reader.GetName(0)}");

                    while (reader.Read())
                    {
                        var name = reader.GetValue(0);
                        Console.Write(name);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Not found!");
                }
            }
        }
        static void BooksChoose()
        {
            var connectionString = @"Data Source = JONATHAN-KENT\SQLEXPRESS; 
                                     Initial Catalog = LibrarySQL;
                                     Trusted_Connection=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = @"SELECT * 
                              FROM Press";

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine($"{reader.GetName(0)}\t{reader.GetName(1)}");

                    while (reader.Read())
                    {
                        var id = reader.GetValue(0);
                        Console.Write(id);

                        Console.Write('\t');

                        var name = reader.GetValue(1);
                        Console.Write(name);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Not found!");
                }
            }
        }

        static void Main(string[] args)
        {
            bool exit = true;
            while (exit)
            {
                Console.WriteLine("Приветствую вас в Sam Library\nВыберите что хотите: ");
                Console.Write("1) Вывод всех книг\n2) Вывод всех студентов\n9) Выход\nВыбор: ");
                int choose = int.Parse(Console.ReadLine());
                if (choose == 1)
                {
                    Console.Clear();
                    BooksChoose();
                    Console.Write("Нажмите любую кнопку для продолжения.");
                    Console.ReadKey();
                    Console.Clear();

                }
                else if (choose == 2)
                {
                    Console.Clear();
                    StudentsChoose();
                    Console.Write("Нажмите любую кнопку для продолжения.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (choose == 9)
                {
                    exit = false;
                }
            }            
        }
    }
}
