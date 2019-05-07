using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoodworkBerserk.Controllers
{
    class DatabaseConnector
    {

        SqlConnection connection;

        public DatabaseConnector(SqlConnection connection, String hostname, String databaseName, String userID, String userPassword)
        {
            this.connection = connection;
            initializeDatabase(hostname, databaseName, userID, userPassword);
            connect();
        }

        public void initializeDatabase(String hostname, String databaseName, String userID, String userPassword)
        {
            string connectionString;
            connectionString = @"Data Source=" + hostname + ";Initial Catalog=" + databaseName + ";User ID=" + userID + ";Password="+ userPassword;
            connection = new SqlConnection(connectionString);
            Console.WriteLine("Database initialized succesfully...");
        }
        public void connect()
        {
            connection.Open();
            Console.WriteLine("Database connected succesfully...");
        }

        public void disconnect()
        {
            connection.Close();
            Console.WriteLine("Database disconnected succesfully...");
        }

        public bool Authenticate(String username, String password)
        {
            SqlCommand command;
            SqlDataReader dataReader;
            String sql;

            sql = "SELECT COUNT(*) FROM player WHERE player_name='" + username + "' AND player_password='" + password + "'";
            command = new SqlCommand(sql, connection);
            bool result;
            dataReader = command.ExecuteReader();

            if(Convert.ToInt32(dataReader.GetValue(0)) != 1)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            
            dataReader.Close();
            command.Dispose();
            return result;
        }

        public void createPlayer(String username, String password)
        {
            SqlCommand command;
            String sql;

            sql = "INSERT INTO player (player_id, player_name, player_password) VALUES (" + generateId() + ", '" + username + "', '" + password + "')";
            command = new SqlCommand(sql, connection);
            Console.WriteLine("Created player with username: " + username + ", and password: " + password);
            command.Dispose();
        }

        public uint generateId()
        {
            SqlCommand command;
            SqlDataReader dataReader;
            String sql;

            sql = "SELECT COUNT(*) FROM player";
            command = new SqlCommand(sql, connection);
            dataReader = command.ExecuteReader();
            uint result = Convert.ToUInt32(dataReader.GetInt32(0));
            dataReader.Close();
            command.Dispose();
            return result + 1;
        }


    }
}
