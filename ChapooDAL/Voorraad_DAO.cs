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
    public class Voorraad_DAO : Base
    {
        public List<Voorraad> DB_Selecteer_Alle_Items()
        {
            string query = "SELECT mi.MenuItemId, m.MenuType, mi.Naam, mi.Prijs FROM MenuItem AS mi" +
                "JOIN Menu AS m ON mi.MenuId = m.MenuId";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Voorraad> DB_Selecteer_Alle_Items_Bar() // John Bond 649770
        {
            string query = "SELECT V.VoorraadId, MI.Naam, VoorraadAantal " +
                "FROM Voorraad AS V " +
                "JOIN MenuItem AS MI ON V.MenuItemId = MI.MenuItemId " +
                "JOIN Menu AS M ON MI.MenuId = M.MenuId " +
                "WHERE m.MenuType = 'DrankMenu';";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesVoorraad(ExecuteSelectQuery(query, sqlParameters));
        }
        public List<Voorraad> DB_Selecteer_Alle_Items_Keuken() // John Bond 649770
        {
            string query = "SELECT V.VoorraadId, MI.Naam, VoorraadAantal " +
                "FROM Voorraad AS V " +
                "JOIN MenuItem AS MI ON V.MenuItemId = MI.MenuItemId " +
                "JOIN Menu AS M ON MI.MenuId = M.MenuId " +
                "WHERE m.MenuType != 'DrankMenu';";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesVoorraad(ExecuteSelectQuery(query, sqlParameters));
        }
        private List<Voorraad> ReadTablesVoorraad(DataTable dataTable) // John Bond 649770
        {
            List<Voorraad> voorraad = new List<Voorraad>();

            foreach (DataRow r in dataTable.Rows)
            {
                Voorraad voorraadItem = new Voorraad()
                {
                    VoorraadId = (int)r["VoorraadId"],
                    Naam = (string)r["Naam"],
                    VoorraadAantal = (int)r["VoorraadAantal"]
                };
                voorraad.Add(voorraadItem);
            }
            return voorraad;
        }

        private List<Voorraad> ReadTables(DataTable dataTable)
        {
            List<Voorraad> voorraad = new List<Voorraad>();

            foreach (DataRow r in dataTable.Rows)
            {
                Voorraad voorraadItem = new Voorraad()
                {
                    VoorraadId = (int)r["VoorraadId"],
                    MenuItemId = (int)r["MenuItemId"],
                    VoorraadAantal = (int)r["VoorraadAantal"]
                };
                voorraad.Add(voorraadItem);
            }
            return voorraad;
        }

        public void DB_UpdateVoorraad(int VoorraadId, int nieuweVoorraadAantal) // Koen van Cromvoirt 647634
        {
            string query = $"UPDATE Voorraad SET VoorraadAantal = '{nieuweVoorraadAantal}' WHERE VoorraadId='{VoorraadId}'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            ExecuteSelectQueryVoid(query, sqlParameters);
        }
    }


}

