using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Working_with_SQL
{
    class CSharpWithSql
    {
        static void Main()
        {
            InsertData();
            SelectData();
            SelectData2();
            SelectData3();
            AggregateFunction();
            AvoidSqlInjection();
            OutputParameter();
            GetInsertedId();
            CallStoredProcedure();
            Transaction();
        }

        static void InsertData()
        {
            var connectionString = @"Data Source = JONATHAN-KENT\SQLEXPRESS; 
                                     Initial Catalog = LibrarySQL;
                                     Trusted_Connection=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connected...");

                var query = @"INSERT INTO Press 
                              VALUES ('SamPress')";
                var command = new SqlCommand(query, connection);
                var result = command.ExecuteNonQuery();
                Console.WriteLine(result);
            }
            Console.WriteLine("Disconnected...");
        }

        static void SelectData()
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

        static void SelectData2()
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
                        Console.Write(reader.GetValue(0));
                        Console.Write('\t');
                        Console.Write(reader.GetValue(1));
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Not found!");
                }
            }
        }

        static void SelectData3()
        {
            var connectionString = @"Data Source = JONATHAN-KENT\SQLEXPRESS; 
                                     Initial Catalog = LibrarySQL;
                                     Trusted_Connection=True";
            var books = new List<Book>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = @"SELECT Books.Id, Books.Name, Pages, YearPress, Comment, Quantity, Press.Name
                              FROM Books
                              JOIN Press ON Press.Id = Id_Press";

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var book = new Book();

                        book.Id = reader.GetInt32(0);
                        book.Name = reader.GetString(1);
                        book.Pages = reader.GetInt32(2);
                        book.YearPress = reader.GetInt32(3);
                        book.Comment = reader.GetString(4);
                        book.Quantity = reader.GetInt32(5);
                        book.PressName = reader.GetString(6);

                        books.Add(book);
                    }
                }
                else
                {
                    Console.WriteLine("Not found!");
                }
            }

            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        static void AggregateFunction()
        {
            var connectionString = @"Data Source = JONATHAN-KENT\SQLEXPRESS; 
                                     Initial Catalog = LibrarySQL;
                                     Trusted_Connection=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = @"SELECT COUNT(*)
                              FROM Books";

                var command = new SqlCommand(query, connection);
                var result = (int)command.ExecuteScalar();
                Console.WriteLine(result);
            }
        }

        static void AvoidSqlInjection()
        {
            var connectionString = @"Data Source = JONATHAN-KENT\SQLEXPRESS; 
                                     Initial Catalog = LibrarySQL;
                                     Trusted_Connection=True";
            Console.Write("Enter press name: ");
            var pressName = Console.ReadLine();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = "INSERT INTO Press VALUES (@pressName)";
                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@pressName", pressName));

                var result = command.ExecuteNonQuery();
                Console.WriteLine(result);
            }
        }

        static void OutputParameter()
        {
            var connectionString = @"Data Source = JONATHAN-KENT\SQLEXPRESS; 
                                     Initial Catalog = LibrarySQL;
                                     Trusted_Connection=True";
            Console.Write("Enter press name: ");
            var pressName = Console.ReadLine();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = "INSERT INTO Press VALUES (@pressName); SET @id=SCOPE_IDENTITY();";
                var command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@pressName", pressName));
                var idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(idParam);

                var result = command.ExecuteNonQuery();
                Console.WriteLine(result);

                Console.WriteLine(idParam.Value);
            }

        }

        static void GetInsertedId()
        {
            var connectionString = @"Data Source = JONATHAN-KENT\SQLEXPRESS; 
                                     Initial Catalog = LibrarySQL;
                                     Trusted_Connection=True";
            Console.Write("Enter press name: ");
            var pressName = Console.ReadLine();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = "INSERT INTO Press VALUES (@pressName); SELECT SCOPE_IDENTITY();";
                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("@pressName", pressName));

                var result = (decimal)command.ExecuteScalar();
                Console.WriteLine(result);
            }
        }

        static void CallStoredProcedure()
        {
            var connectionString = @"Data Source = JONATHAN-KENT\SQLEXPRESS; 
                                     Initial Catalog = LibrarySQL;
                                     Trusted_Connection=True";
            Console.Write("Enter press name: ");
            var pressName = Console.ReadLine();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("IncrementQuantityByPressName", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@pressName", pressName));

                var result = command.ExecuteNonQuery();
                Console.WriteLine(result);
            }
        }

        static void Transaction()
        {
            var bankConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BankDb";
            using (var connection = new SqlConnection(bankConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                var query1 = @"UPDATE Account
            				   SET Amount = Amount - 1000
            				   WHERE Id = 1";
                var command1 = new SqlCommand(query1, connection);
                command1.Transaction = transaction;

                var query2 = @"UPDATE Account
            				   SET Amount = Amount + 1000
            				   WHERE Id = 2";
                var command2 = new SqlCommand(query2, connection);
                command2.Transaction = transaction;

                try
                {
                    command1.ExecuteNonQuery();
                    command2.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                }
            }
        }
    }

}

class Book
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Pages { get; set; }
    public int YearPress { get; set; }
    public string Comment { get; set; }
    public int Quantity { get; set; }
    public string PressName { get; set; }

    public override string ToString()
    {
        return $"{Id}\t{Name}\t{Pages}\t{YearPress}\t{Comment}\t{Quantity}\t{PressName}";
    }
}

