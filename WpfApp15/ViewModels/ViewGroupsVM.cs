using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp15.DTO;

namespace WpfApp15.ViewModels
{
    class ViewGroupsVM
    {
        public List<Group> Groups { get; set; }

        public ViewGroupsVM()
        {
            Groups = new List<Group>();
            MySqlDB mySqlDB = MySqlDB.GetDB();
            string query = "select * from `group`";
            if (mySqlDB.OpenConnection())
            {
                using (MySqlCommand mc = new MySqlCommand(query, mySqlDB.sqlConnection))
                using (MySqlDataReader dr = mc.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Groups.Add(new Group
                        {
                            ID = dr.GetInt32("id"),
                            Title = dr.GetString("title"),
                            Year = dr.GetInt32("year")
                        }) ;
                    }
                }
                mySqlDB.CloseConnection();
            }
        }
    }
}
