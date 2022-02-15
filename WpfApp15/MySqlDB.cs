using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp15
{
    public class MySqlDB
    {
        private MySqlDB() { }
        static MySqlDB db;
        public static MySqlDB GetDB()
        {
            if (db == null)
                db = new MySqlDB();
            return db;
        }

        protected MySqlConnection sqlConnection = null;

        internal void InitConnection()
        {
            InitConnection(Properties.Settings.Default.server, Properties.Settings.Default.user,
                Properties.Settings.Default.db, Properties.Settings.Default.pass);
        }

        internal void InitConnection(string server, string user, string db, string password)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.UserID = user;
            builder.Password = password;
            builder.Database = db;
            builder.Server = server;
            builder.CharacterSet = "utf8";
            builder.ConnectionTimeout = 5;

            sqlConnection = new MySqlConnection(builder.GetConnectionString(true));
        }

        internal bool OpenConnection()
        {
            try
            {
                if (sqlConnection == null)
                    InitConnection();
                sqlConnection.Open();
                return true;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            return false;
        }

        internal void CloseConnection()
        {
            try
            {
                sqlConnection.Close(); // закрытие соединения
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal void ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            if (OpenConnection())
            {
                using (MySqlCommand mc = new MySqlCommand(query, sqlConnection))
                {
                    if (parameters != null)
                        mc.Parameters.AddRange(parameters);
                    mc.ExecuteNonQuery();
                }
                CloseConnection();
            }
        }
    }
}
