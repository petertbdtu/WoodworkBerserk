using Npgsql;
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

        NpgsqlConnection connection;

        public DatabaseConnector()
        {
            initializeDatabase();
            connect();
        }

        public void initializeDatabase()
        {
            //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            //builder.DataSource = "balarama.db.elephantsql.com,5432";
            //builder.UserID = "onyyvcau";
            //builder.Password = "uceLAbGSTVO4HyOLNczJinlQGWiKzVRR";
            //builder.InitialCatalog = "woodworkberserk";
            //string connectionString;
            var connString = "Host=balarama.db.elephantsql.com;Username=onyyvcau;Password=uceLAbGSTVO4HyOLNczJinlQGWiKzVRR;Database=onyyvcau";
            //connectionString = Properties.Settings.Default.Setting;
            connection = new NpgsqlConnection(connString);
            //connection = new SqlConnection(builder.ConnectionString);
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

        public int Authenticate(String username, String password)
        {
            //NpgsqlCommand command;
            //NpgsqlDataReader dataReader;
            String sql;
           

            sql = "SELECT COUNT(*) FROM player WHERE player_name='" + username + "' AND player_password='" + password + "';";
            using (var command = new NpgsqlCommand(sql, connection))
            {
                int result;
                //dataReader = command.ExecuteReader();
                object cmd = command.ExecuteScalar();
                int cmd2 = int.Parse(string.Format("{0}", cmd));
                Console.WriteLine(cmd);
                if (cmd2 == 0)
                {
                    result = 1;
                }
                else
                {
                    result = 2;
                }

                //dataReader.Close();
                command.Dispose();
                return result;
            }
        }
        //doesnt work? dunno why use auto_increment for player_id in SQL database
        public void createPlayer(String username, String password)
        {
            String sql;

            sql = "INSERT INTO player (player_id, player_name, player_password) VALUES (@id, '@name', '@pass')";
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", generateId());
                command.Parameters.AddWithValue("@name", username);
                command.Parameters.AddWithValue("@pass", password);
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Created player with username: " + username + ", and password: " + password);
                command.Dispose();
            }
            
        }

        public int generateId()
        {
            NpgsqlCommand command;
            NpgsqlDataReader dataReader;
            String sql;

            sql = "SELECT COUNT(*) FROM player;";
            command = new NpgsqlCommand(sql, connection);
            //dataReader = command.ExecuteReader();
            object cmd = command.ExecuteScalar();
            int result = int.Parse(string.Format("{0}", cmd));
            //dataReader.Close();
            command.Dispose();
            return result + 1;
        }


    }
}
