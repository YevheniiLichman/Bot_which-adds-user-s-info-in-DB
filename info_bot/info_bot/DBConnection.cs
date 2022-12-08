using MySql.Data.MySqlClient;
using System;

namespace dark
{
    public class DBConnection
    {
        private DBConnection()
        {
        }

        private string databaseName = string.Empty;

        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }
        
        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }
        
        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            try
            {
                if (Connection == null)
                {
                    if (String.IsNullOrEmpty(databaseName))
                        return false;
                    string connstring =
                        string.Format("server = localhost; user = root; port = 3306; password = root; database = {0}",
                            databaseName);
                    connection = new MySqlConnection(connstring);
                    connection.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex}");
                return false;
            }
        }

        public void Close()
        {
            connection.Close();
        }
    }
}