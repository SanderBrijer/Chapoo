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
    public class Klant_DAO : Base
    {
        public List<Klant> DB_Selecteer_Alle_Items() // Sander Brijer 646235
        {
            string query = "SELECT KlantId, Naam, Telefoonnummer, Email FROM Klant";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Klant> ReadTables(DataTable dataTable) // Sander Brijer 646235
        {
            List<Klant> klanten = new List<Klant>();

            foreach (DataRow r in dataTable.Rows)
            {
                Klant klant = new Klant()
                {
                    KlantId = (int)r["KlantId"],
                    Naam = (string)r["Naam"],
                    Telefoonnummer = (string)r["Telefoonnummer"],
                    Email = (string)r["Email"]
                };
                klanten.Add(klant);
            }
            return klanten;
        }

        private Klant ReadTable(DataTable dataTable) // Sander Brijer 646235
        {
            Klant klant = new Klant()
            {
                KlantId = (int)dataTable.Rows[0]["KlantId"],
                Naam = (string)dataTable.Rows[0]["Naam"],
                Telefoonnummer = (string)dataTable.Rows[0]["Telefoonnummer"],
                Email = (string)dataTable.Rows[0]["Email"]
            };
            return klant;
        }


        public void Db_ToevoegenKlant(string klantNaam, string telefoonnummer, string emailadres) // Sander Brijer 646235
        {
            string query = $"INSERT INTO Klant VALUES ('{klantNaam}', '{telefoonnummer}', '{emailadres}')";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            ExecuteSelectQueryVoid(query, sqlParameters);
        }

        public Klant Db_KrijgLaatsteKlant(string klantNaam, string telefoonNummer, string email) // Sander Brijer 646235
        {
            string query = $"SELECT KlantId, Naam, Telefoonnummer, Email FROM Klant WHERE Naam = {klantNaam} AND Telefoonnummer = {telefoonNummer}, Email = {email}";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTable(ExecuteSelectQuery(query, sqlParameters));
        }
    }
}
