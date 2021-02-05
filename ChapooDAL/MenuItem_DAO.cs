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
    public class MenuItem_DAO : Base
    {
        public List<MenuItem> DB_Selecteer_Alle_Items()
        {
            string query = "SELECT mi.MenuItemId, m.MenuType, mi.Naam, mi.Prijs FROM MenuItem AS mi" +
                "JOIN Menu AS m ON mi.MenuId = m.MenuId";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public MenuItem DB_Selecteer_Een_Item(int menuItemId) //Louella Creemers 641347
        {
            string query = "SELECT MenuItemId, Naam FROM MenuItem " +
                "WHERE MenuItemId = @MenuItemId";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@MenuItemId", menuItemId)
            };
            return ReadTable(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<MenuItem> ReadTables(DataTable dataTable) //Louella Creemers 641347
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            foreach (DataRow r in dataTable.Rows)
            {
                MenuItem menuItem = new MenuItem()
                {
                    MenuItemId = (int)r["MenuItemId"],
                    MenuId = (int)r["MenuId"],
                    Naam = (string)r["Naam"],
                    Prijs = (Double)r["Prijs"]
                };
                menuItems.Add(menuItem);
            }
            return menuItems;
        }

        public List<MenuItem> DB_Selecteer_Een_Item_Lunch() //Louella Creemers 641347
        {
            string query = "SELECT m.MenuItemId FROM MenuItem AS m " +
                "WHERE MenuId IN(1, 2, 3); ";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadMenu(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<MenuItem> DB_Selecteer_Een_Item_Diner() //Louella Creemers 641347
        {
            string query = "SELECT m.MenuItemId FROM MenuItem AS m " +
                "WHERE MenuId IN(4, 5, 6, 7); ";

            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadMenu(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<MenuItem> DB_Selecteer_Een_Item_Drank() //Louella Creemers 641347
        {
            string query = "SELECT m.MenuItemId FROM MenuItem AS m " +
                "WHERE MenuId = '8'; ";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadMenu(ExecuteSelectQuery(query, sqlParameters));
        }

        private MenuItem ReadTable(DataTable dataTable) //Louella Creemers 641347
        {

            DataRow r = dataTable.Rows[0];
            MenuItem menuItem = new MenuItem()
            {
                Naam = (string)r["Naam"]
            };
            return menuItem;

        }

        public MenuItem DB_Selecteer_Een_Item_Bij_Naam(int menuItemId) //Louella Creemers 641347
        {
            string query = "SELECT m.Naam, m.MenuId , v.VoorraadAantal FROM MenuItem AS m " +
                           "JOIN Voorraad AS v ON m.MenuItemId = v.MenuItemId " +
                           "WHERE v.MenuItemId = @MenuItemId";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@MenuItemId", menuItemId)
            };
            return ReadNaam(ExecuteSelectQuery(query, sqlParameters));
        }

        public MenuItem DB_Selecteer_Een_Item_Bij_Id(string naam) //Louella Creemers 641347
        {
            string query = "SELECT m.MenuItemId, v.VoorraadAantal FROM MenuItem AS m " +
                           "JOIN Voorraad AS v ON m.MenuItemId = v.MenuItemId " +
                           "WHERE Naam = @Naam";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Naam", naam)
            };
            return ReadId(ExecuteSelectQuery(query, sqlParameters));
        }

        private MenuItem ReadNaam(DataTable dataTable) //Louella Creemers 641347
        {

            DataRow r = dataTable.Rows[0];
            MenuItem menuItem = new MenuItem()
            {
                MenuId = (int)r["MenuId"],
                Naam = (string)r["Naam"],
                VoorraadAantal = (int)r["VoorraadAantal"]
            };
            return menuItem;

        }

        private MenuItem ReadId(DataTable dataTable) //Louella Creemers 641347
        {

            DataRow r = dataTable.Rows[0];
            MenuItem menuItem = new MenuItem()
            {
                MenuItemId = (int)r["MenuItemId"],
                VoorraadAantal = (int)r["VoorraadAantal"]
            };
            return menuItem;

        }

        private MenuItem ReadItemMenu(DataTable dataTable) //Louella Creemers 641347
        {

            DataRow r = dataTable.Rows[0];
            MenuItem menuItem = new MenuItem()
            {
                MenuId = (int)r["MenuId"]
            };
            return menuItem;
        }

        private List<MenuItem> ReadMenu(DataTable dataTable) //Louella Creemers 641347
        {

            List<MenuItem> menuItems = new List<MenuItem>();

            foreach (DataRow r in dataTable.Rows)
            {
                MenuItem menuItem = new MenuItem()
                {
                    MenuItemId = (int)r["MenuItemId"]
                };
                menuItems.Add(menuItem);
            }

            return menuItems;

        }

    }
}