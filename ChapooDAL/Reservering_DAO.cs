using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooModel;

namespace ChapooDAL
{
    public class Reservering_DAO : Base
    {
        public List<Reservering> DB_Selecteer_Alle_Items() // Sander Brijer 646235
        {
            string query = "SELECT k.Email, k.Naam, k.Telefoonnummer, r.ReserveringId, r.KlantId, r.TafelId, r.AantalPersonen, r.AankomstTijdDatum, r.Opmerking FROM Reservering AS r JOIN Klant AS k ON r.KlantId = k.KlantId";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Reservering> ReadTables(DataTable dataTable) // Sander Brijer 646235
        {
            List<Reservering> reserveringen = new List<Reservering>();
            if (dataTable.Rows.Count == 0)
                return null;
            foreach (DataRow r in dataTable.Rows)
            {
                Klant klant = new Klant()
                {
                    KlantId = (int)r["KlantId"],
                    Naam = (string)r["Naam"],
                    Telefoonnummer = (string)r["Telefoonnummer"],
                    Email = (string)r["Email"]
                };

                Reservering reservering = new Reservering()
                {
                    ReserveringId = (int)r["ReserveringId"],
                    Klant = klant,
                    TafelId = (int)r["TafelId"],
                    AantalPersonen = (int)r["AantalPersonen"],
                    AankomstDatumTijd = (DateTime)r["AankomstTijdDatum"],
                    ReserveringOpmerking = (string)r["Opmerking"]
                };
                reserveringen.Add(reservering);
            }
            return reserveringen;
        }

        private Reservering ReadTable(DataTable dataTable) // Sander Brijer 646235
        {
            Klant klant = new Klant()
            {
                KlantId = (int)dataTable.Rows[0]["KlantId"],
                Naam = (string)dataTable.Rows[0]["Naam"],
                Telefoonnummer = (string)dataTable.Rows[0]["Telefoonnummer"],
                Email = (string)dataTable.Rows[0]["Email"]
            };

            Reservering reservering = new Reservering()
            {
                ReserveringId = (int)dataTable.Rows[0]["ReserveringId"],
                Klant = klant,
                TafelId = (int)dataTable.Rows[0]["TafelId"],
                AantalPersonen = (int)dataTable.Rows[0]["AantalPersonen"],
                AankomstDatumTijd = (DateTime)dataTable.Rows[0]["AankomstTijdDatum"],
                ReserveringOpmerking = (string)dataTable.Rows[0]["Opmerking"]
            };
            return reservering;

        }

        public void Db_BewerkenReservering(int tafelId, int aantalPersonen, DateTime aankomstDatumTijd, string opmerking, int reserveringId) // Sander Brijer 646235
        {
            string aankomstDatumTijdS = aankomstDatumTijd.ToString("yyyy-MM-dd HH:mm:00.000");
            string query = $"UPDATE Reservering SET TafelId= '{tafelId}', AantalPersonen = '{aantalPersonen}', AankomstTijdDatum = '{aankomstDatumTijdS}', Opmerking = '{opmerking}' WHERE ReserveringId = {reserveringId}";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            ExecuteSelectQueryVoid(query, sqlParameters);
        }

        public void Db_ToevoegenReservering(int klantId, int tafelId, int aantalPersonen, DateTime aankomstDatumTijd, string opmerking) // Sander Brijer 646235
        {
            string aankomstDatumTijdS = aankomstDatumTijd.ToString("yyyy-MM-dd HH:mm:00.000");
            string query = $"INSERT INTO Reservering VALUES ('{klantId}', '{tafelId}', '{aantalPersonen}', '{aankomstDatumTijdS}', '{opmerking}')";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            ExecuteSelectQueryVoid(query, sqlParameters);
        }

        public void Db_VerwijderReservering(int reserveringId) // Sander Brijer 646235
        {
            string query = $"DELETE FROM Reservering WHERE ReserveringId = '{reserveringId}'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            ExecuteSelectQueryVoid(query, sqlParameters);
        }


        public Reservering DB_Selecteer_Via_Id(int reserveringNummer) // Sander Brijer 646235
        {
            string query = $"SELECT k.Email, k.Naam, k.Telefoonnummer, r.ReserveringId, r.KlantId, r.TafelId, r.AantalPersonen, r.AankomstTijdDatum, r.Opmerking FROM Reservering AS r JOIN Klant AS k ON r.KlantId = k.KlantId WHERE r.ReserveringId = '{reserveringNummer}'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTable(ExecuteSelectQuery(query, sqlParameters));
        }
    }
}
