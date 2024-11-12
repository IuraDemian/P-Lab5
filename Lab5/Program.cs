using Microsoft.Data.Sqlite;
using System;
using System.Data.SQLite;

namespace AutoDatabaseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=auto_database.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Auto (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    car_number TEXT NOT NULL,
                    mark TEXT NOT NULL,
                    price REAL NOT NULL,
                    home_address TEXT NOT NULL
                );";
                ExecuteNonQuery(connection, createTableQuery);

                string insertDataQuery = @"
                INSERT INTO Auto (name, car_number, mark, price, home_address)
                VALUES 
                    ('Юра', 'AA167BB', 'Toyota', 15000, 'Київ, вул. Лицовка, 10'),
                    ('Іван', 'AB5077CC', 'Honda', 12000, 'Одеса, вул. Іван Франка, 23'),
                    ('Софія', 'CA1234DD', 'Toyota', 18000, 'Львів, вул. Миру, 6'),
                    ('Міша', 'AB7177CC', 'BMW', 26000, 'Харків, вул. Лесі Українки, 5');
                ";
                ExecuteNonQuery(connection, insertDataQuery);

                Console.WriteLine("Дані в таблиці Auto:");
                string selectQuery = "SELECT * FROM Auto";
                using (var command = new SQLiteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["name"]}, {reader["car_number"]}, {reader["mark"]}, {reader["price"]}, {reader["home_address"]}");
                    }
                }

                string countQuery = "SELECT COUNT(*) FROM Auto WHERE mark = 'Honda' AND car_number LIKE '%7%'";
                using (var command = new SQLiteCommand(countQuery, connection))
                {
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Кількість власників машин марки 'Honda' з цифрою '7' у номері: {count}");
                }

                string sumQuery = "SELECT SUM(price) FROM Auto WHERE mark = 'Honda'";
                using (var command = new SQLiteCommand(sumQuery, connection))
                {
                    double totalCost = Convert.ToDouble(command.ExecuteScalar());
                    Console.WriteLine($"Загальна вартість усіх машин марки 'Honda': {totalCost}");
                }

                connection.Close();
            }
        }

        static void ExecuteNonQuery(SQLiteConnection connection, string query)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
