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
    public class Tafel_DAO : Base
    {
        public List<Tafel> DB_Selecteer_Alle_Tafels() // Sander Brijer 646235
        {
            string query = "SELECT TafelId, Status, Gereserveerd, Zitplekken FROM Tafel";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Tafel> ReadTables(DataTable dataTable) // Sander Brijer 646235
        {
            List<Tafel> tafels = new List<Tafel>();

            foreach (DataRow r in dataTable.Rows)
            {
                string status = (string)r["Status"];
                Tafel tafel = new Tafel()
                {
                    TafelId = (int)r["TafelId"],
                    Status = (StatusTafel)Enum.Parse(typeof(StatusTafel), status),
                    Gereserveerd = (bool)r["Gereserveerd"],
                    Zitplekken = (int)r["Zitplekken"]
                };
                tafels.Add(tafel);
            }
            return tafels;
        }


        public void DB_BewerkenTafelStatus(int tafelId, StatusTafel nieuweStatus) // Sander Brijer 646235
        {
            string query = $"UPDATE Tafel SET Status = '{nieuweStatus.ToString()}' WHERE TafelId='{tafelId}'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            ExecuteSelectQueryVoid(query, sqlParameters);
        }

        public void DB_BewerkenTafelZitplekken(int tafelId, int nieuweZitplekken) // Sander Brijer 646235
        {
            string query = $"UPDATE Tafel SET Zitplekken = '{nieuweZitplekken}' WHERE TafelId='{tafelId}'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            ExecuteSelectQueryVoid(query, sqlParameters);
        }

        public Tafel DB_Selecteer_Alle_Tafels_Bij_Id(int tafelId) //Lou
        {
            string query = "SELECT Status, Gereserveerd, Zitplekken FROM Tafel WHERE TafelId = @TafelId";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("TafelId", tafelId)
            };
            return ReadTables(ExecuteSelectQuery(query, sqlParameters))[0];

        }
    }

}