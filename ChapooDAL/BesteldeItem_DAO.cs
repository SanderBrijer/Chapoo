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
    public class BesteldeItem_DAO : Base
    {

        public void BesteldeItemToevoegen(int bestellingId, int menuItemId, int aantal) //Louella Creemers 641347
        {

            string bestellingQuery = "INSERT INTO BesteldeItem VALUES (@BestellingId, @MenuItemId, @Aantal)";
            SqlParameter[] sqlParametersBestelling = new SqlParameter[]
            {
                new SqlParameter("@BestellingId", bestellingId),
                new SqlParameter("@MenuItemId", menuItemId),
                new SqlParameter("@Aantal", aantal)
            };
            ExecuteEditQuery(bestellingQuery, sqlParametersBestelling);

            string voorraadQuery = "UPDATE Voorraad SET VoorraadAantal = VoorraadAantal - @Aantal WHERE MenuItemId = @MenuItemId";
            SqlParameter[] sqlParametersVoorraad = new SqlParameter[]
            {
                new SqlParameter("@Aantal", aantal),
                new SqlParameter("@MenuItemId", menuItemId)

            };
            ExecuteEditQuery(voorraadQuery, sqlParametersVoorraad);
        }

        public List<BesteldeItem> DB_Selecteer_Bestelling_Details_Op_BestellingId_Bar(string bestellingId) // John Bond 649770
        {
            string query = "SELECT mi.Naam, BI.Aantal, be.BestellingOpmerking " +
                "FROM BesteldeItem AS BI " +
                "JOIN Bestelling AS BE ON BI.BestellingId = be.BestellingId " +
                "JOIN MenuItem AS mi ON bi.MenuItemId = mi.MenuItemId " +
                "JOIN Menu AS m ON mi.MenuId = m.MenuId  " +
                $"WHERE be.BestellingId = {bestellingId} AND m.MenuType = 'Drankmenu'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesDetails(ExecuteSelectQuery(query, sqlParameters));
        }
        public List<BesteldeItem> DB_Selecteer_Bestelling_Details_Op_BestellingId_Keuken(string bestellingId) // John Bond 649770
        {
            string query = "SELECT mi.Naam, BI.Aantal, be.BestellingOpmerking " +
                "FROM BesteldeItem AS BI " +
                "JOIN Bestelling AS BE ON BI.BestellingId = be.BestellingId " +
                "JOIN MenuItem AS mi ON bi.MenuItemId = mi.MenuItemId " +
                "JOIN Menu AS m ON mi.MenuId = m.MenuId  " +
                $"WHERE be.BestellingId = {bestellingId} AND m.MenuType != 'Drankmenu'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesDetails(ExecuteSelectQuery(query, sqlParameters));
        }
        private List<BesteldeItem> ReadTablesDetails(DataTable dataTable) // John Bond 649770
        {
            List<BesteldeItem> bestellingen = new List<BesteldeItem>();

            foreach (DataRow r in dataTable.Rows)
            {
                BesteldeItem details = new BesteldeItem()
                {
                    Naam = (string)r["Naam"],
                    Aantal = (int)r["Aantal"],

                };
                if (!(r["BestellingOpmerking"].Equals(System.DBNull.Value)))
                {
                    details.BestellingOpmerking = (string)r["BestellingOpmerking"];
                }
                else
                {
                    details.BestellingOpmerking = "-";
                }
                bestellingen.Add(details);
            }
            return bestellingen;
        }
        
    }
}
