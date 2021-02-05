using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooModel;


namespace ChapooDAL
{
    public class Menu_DAO : Base
    {
        public List<Menu> DB_Selecteer_Alle_Items()
        {
            string query = "SELECT mi.MenuItemId, m.MenuType, mi.Naam, mi.Prijs FROM MenuItem AS mi" +
                "JOIN Menu AS m ON mi.MenuId = m.MenuId";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Menu> ReadTables(DataTable dataTable)
        {
            List<Menu> menus = new List<Menu>();

            foreach (DataRow r in dataTable.Rows)
            {
                Menu menu = new Menu()
                {
                    MenuId = (int)r["MenuId"],
                    Type = (string)r["Type"],
                };
                menus.Add(menu);
            }
            return menus;
        }
    }
}
