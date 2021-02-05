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
    public class Bon_DAO : Base
    {
        public List<Bon> DB_Selecteer_Alle_Items()
        {
            string query = "SELECT * BON";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Bon> ReadTables(DataTable dataTable)
        {
            List<Bon> bonnen = new List<Bon>();

            foreach (DataRow r in dataTable.Rows)
            {
                Bon bon = new Bon()
                {
                    BonId = (int)r["BonId"],
                    BestellingId = (int)r["BestellingId"],
                    TotaalPrijs = (double)r["TotaalPrijs"],
                    Tip = (int)r["Tip"],
                    Betaald = (bool)r["Betaald"],
                    Betaalwijze = (string)r["BetaalWijze"],
                    BonOpmerking = (string)r["BonOpmerking"]
                };
                bonnen.Add(bon);
            }
            return bonnen;
        }
    }
}
