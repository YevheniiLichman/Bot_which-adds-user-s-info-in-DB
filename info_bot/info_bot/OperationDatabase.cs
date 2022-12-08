using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;


namespace dark
{
    public class OperationDatabase
    {
        public void insertUser(int id, string name, string lang) // додавання користувача 
        {
            string connectionString =
                ("server = localhost; user = root; port = 3306; password = root; database = bot_user_info");
            MySqlConnection connection = new MySqlConnection(connectionString);

            string addUser =
                $"INSERT IGNORE user_info(ID, Name, Lang) VALUES ({id}, '{name}', '{lang}')";
            MySqlCommand cmd = new MySqlCommand(addUser, connection);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("DONE!\n");
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Error -> {e}");
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeleteUser(string name) // видалення користувача по імені
        {
            string connectionString =
                ("server = localhost; user = root; port = 3306; password = root; database = user_info");
            string query = $"DELETE FROM bot_info WHERE Name = ('{name}')";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand(query, connection);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Succes");
            }
            catch (SqlException e)
            {
                Console.WriteLine("error");
            }
            finally
            {
                connection.Close();
            }
        }


        public void connectDatabase() // з'єднання до бази даних та виведення всіх користувачів 
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "bot_user_info";

            if (dbCon.IsConnect())
            {
                Console.WriteLine("Connect!\n");
                string query = "SELECT * FROM bot_info";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string[] array = { reader.GetString(0), reader.GetString(1), reader.GetString(2) };

                    for (int i = 0; i < array.Length; i++)
                    {
                        Console.WriteLine($"{array[i]}");
                    }
                }

                dbCon.Close();
            }
        }
    }
}

// унікальне Name
// CREATE TABLE user_info (
//      ID int,
//      Name varchar(255),
//      Lang varchar(255),
//      CONSTRAINT UC_user_info UNIQUE (Name)
//  );