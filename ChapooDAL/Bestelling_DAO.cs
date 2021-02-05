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
    public class Bestelling_DAO : Base
    {
        public List<Bestelling> DB_Selecteer_Alle_BestellingenViaTafelId(int tafelId) // Sander Brijer 646235
        {
            string query = $"SELECT BestellingId, TafelId, MedewerkerId, TijdBestelling, [Status] FROM Bestelling WHERE TafelId = {tafelId} AND (Status = 'Open' OR Status = 'Bezet') AND TijdBestelling = (SELECT MIN(TijdBestelling) FROM Bestelling WHERE TafelId = {tafelId} AND(Status = 'Open' OR Status = 'Bezet'))";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesList(ExecuteSelectQuery(query, sqlParameters));
        }

        public int DB_Maak_Bestelling_Aan(int tafelId, int medewerkerId, string Opmerking) //Louella Creemers 641347
        {
            string queryMaken = "INSERT INTO Bestelling VALUES (@Tafel, @Medewerker, @Tijd, 'Open', @Opmerking);";
            SqlParameter[] sqlParametersMaken = new SqlParameter[]
            {
               new SqlParameter("@Tafel", tafelId),
               new SqlParameter("@Medewerker", medewerkerId),
               new SqlParameter("@Tijd", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
               new SqlParameter("@Opmerking", Opmerking)
            };
            ExecuteEditQuery(queryMaken, sqlParametersMaken);

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ChapooDB"].ConnectionString);
            conn.Open();
            string queryZoekenId = "SELECT MAX(BestellingId) FROM Bestelling";
            SqlCommand command = new SqlCommand(queryZoekenId, conn);
            return (int)command.ExecuteScalar();

        }

        public void DB_Verwijder_Bestelling(int bestellingId) //Louella Creemers
        {
            string query = "DELETE FROM Bestelling WHERE BestellingId = @BestellingId";

            SqlParameter[] sql = new SqlParameter[]
            {
                new SqlParameter("@BestellingId", bestellingId),
            };
            ExecuteEditQuery(query, sql);
        }


        public List<Bestelling> DB_Selecteer_Alle_Bestellingen_Bar() // John Bond 649770
        {
            string query = "SELECT DISTINCT be.BestellingId, be.TafelId, be.TijdBestelling, be.[Status]" +
                " FROM BesteldeItem AS bi " +
                "JOIN bestelling AS be ON bi.BestellingId = be.BestellingId " +
                "JOIN MenuItem AS mi ON bi.MenuItemId = mi.MenuItemId " +
                "JOIN Menu AS m ON mi.MenuId = m.MenuId " +
                "WHERE m.MenuType = 'Drankmenu' AND be.[Status] != 'Klaar' " +
                "ORDER BY be.TijdBestelling;";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesBestellingen(ExecuteSelectQuery(query, sqlParameters));
        }
        public List<Bestelling> DB_Selecteer_Alle_Bestellingen_Bar_Geschiedenis() // John Bond 649770
        {
            string query = "SELECT DISTINCT be.BestellingId, be.TafelId, be.TijdBestelling, be.[Status]" +
                " FROM BesteldeItem AS bi " +
                "JOIN bestelling AS be ON bi.BestellingId = be.BestellingId " +
                "JOIN MenuItem AS mi ON bi.MenuItemId = mi.MenuItemId " +
                "JOIN Menu AS m ON mi.MenuId = m.MenuId " +
                "WHERE m.MenuType = 'Drankmenu' AND be.[Status] = 'Klaar' " +
                "ORDER BY be.TijdBestelling;";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesBestellingen(ExecuteSelectQuery(query, sqlParameters));
        }
        public List<Bestelling> DB_Selecteer_Alle_Bestellingen_Keuken() // John Bond 649770
        {
            string query = "SELECT DISTINCT be.BestellingId, be.TafelId, be.TijdBestelling, be.[Status]" +
                " FROM BesteldeItem AS bi " +
                "JOIN bestelling AS be ON bi.BestellingId = be.BestellingId " +
                "JOIN MenuItem AS mi ON bi.MenuItemId = mi.MenuItemId " +
                "JOIN Menu AS m ON mi.MenuId = m.MenuId " +
                "WHERE m.MenuType != 'Drankmenu' AND be.[Status] != 'Klaar' " +
                "ORDER BY be.TijdBestelling;";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesBestellingen(ExecuteSelectQuery(query, sqlParameters));
        }
        public List<Bestelling> DB_Selecteer_Alle_Bestellingen_Keuken_Geschiedenis() // John Bond 649770
        {
            string query = "SELECT DISTINCT be.BestellingId, be.TafelId, be.TijdBestelling, be.[Status]" +
                " FROM BesteldeItem AS bi " +
                "JOIN bestelling AS be ON bi.BestellingId = be.BestellingId " +
                "JOIN MenuItem AS mi ON bi.MenuItemId = mi.MenuItemId " +
                "JOIN Menu AS m ON mi.MenuId = m.MenuId " +
                "WHERE m.MenuType != 'Drankmenu' AND be.[Status] = 'Klaar' " +
                "ORDER BY be.TijdBestelling;";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesBestellingen(ExecuteSelectQuery(query, sqlParameters));
        }
        private List<Bestelling> ReadTablesBestellingen(DataTable dataTable) // John Bond 649770
        {
            List<Bestelling> bestellingen = new List<Bestelling>();
            foreach (DataRow r in dataTable.Rows)
            {
                string status = (string)r["Status"];
                Bestelling bestelling = new Bestelling()
                {
                    BestellingId = (int)r["BestellingId"],
                    TafelId = (int)r["TafelId"],
                    TijdBestelling = Convert.ToDateTime(r["TijdBestelling"].ToString()),
                    Status = (StatusBestelling)Enum.Parse(typeof(StatusBestelling), status),
                };
                bestellingen.Add(bestelling);
            }
            return bestellingen;
        }
        public void DB_Verander_Status(int bestellingId, StatusBestelling statusBestelling) // John Bond 649770
        {
            if (statusBestelling == StatusBestelling.Open)
            {
                string query = $"UPDATE Bestelling SET[Status] = 'Bezet' WHERE BestellingId = {bestellingId};";
                SqlParameter[] sqlParameters = new SqlParameter[0];
                ExecuteSelectQueryVoid(query, sqlParameters);
            }
            else if (statusBestelling == StatusBestelling.Bezet)
            {
                string query = $"UPDATE Bestelling SET[Status] = 'Klaar' WHERE BestellingId = {bestellingId};";
                SqlParameter[] sqlParameters = new SqlParameter[0];
                ExecuteSelectQueryVoid(query, sqlParameters);
            }
            else if (statusBestelling == StatusBestelling.Klaar)
            {
                string query = $"UPDATE Bestelling SET[Status] = 'Open' WHERE BestellingId = {bestellingId};";
                SqlParameter[] sqlParameters = new SqlParameter[0];
                ExecuteSelectQueryVoid(query, sqlParameters);
            }
        }

        private List<Bestelling> ReadTablesList(DataTable dataTable) // Sander Brijer 646235
        {
            List<Bestelling> bestellingen = new List<Bestelling>();

            foreach (DataRow r in dataTable.Rows)
            {
                Bestelling bestelling = new Bestelling()
                {
                    BestellingId = (int)r["BestellingId"],
                    TafelId = (int)r["TafelId"],
                    MedewerkerId = (int)r["MedewerkerId"],
                    TijdBestelling = (DateTime)r["TijdBestelling"]
                };
                bestellingen.Add(bestelling);
            }
            return bestellingen;
        }

        private List<Bestelling> ReadTablesBestellingenKlaar(DataTable dataTable) //Sander Brijer 646235
        {
            List<Bestelling> bestellingen = new List<Bestelling>();
            int i = 0;
            foreach (DataRow r in dataTable.Rows)
            {
                if (bestellingen.Count == 0 || (int)r["BestellingId"] != bestellingen[i - 1].BestellingId)
                {
                    string functie = (string)r["Functie"];

                    Medewerker medewerker = new Medewerker()
                    {
                        MedewerkerId = (int)r["MedewerkerId"],
                        Voornaam = (string)r["Voornaam"],
                        Achternaam = (string)r["Achternaam"],
                        Functie = (Functie)Enum.Parse(typeof(Functie), functie),
                        Email = (string)r["Email"],
                        GebruikersNaam = (string)r["GebruikersNaam"],
                        Wachtwoord = (string)r["Wachtwoord"]
                    };

                    string opmerking;
                    List<string> besteldeItems = new List<string>();
                    try
                    {
                        opmerking = (string)r["BestellingOpmerking"];
                    }
                    catch
                    {
                        opmerking = "";
                    }
                    besteldeItems.Add((string)r["Naam"]);
                    Bestelling bestelling = new Bestelling()
                    {
                        BestellingId = (int)r["BestellingId"],
                        TafelId = (int)r["TafelId"],
                        MedewerkerId = (int)r["MedewerkerId"],
                        Medewerker = medewerker,
                        Status = StatusBestelling.Klaar,
                        TijdBestelling = (DateTime)r["TijdBestelling"],
                        BestellingOpmerking = opmerking,
                        BesteldeItems = besteldeItems
                    };
                    bestellingen.Add(bestelling);
                    i++;
                }
                else
                {
                    bestellingen[bestellingen.Count - 1].BesteldeItems.Add((string)r["Naam"]);
                }
            }
            return bestellingen;
        }

        private List<Bestelling> ReadTablesBestellingenOpen(DataTable dataTable) //Sander Brijer 646235 + Louella Creemers 641347
        {
            List<Bestelling> bestellingen = new List<Bestelling>();
            int i = 0;
            foreach (DataRow r in dataTable.Rows)
            {
                if (bestellingen.Count == 0 || (int)r["BestellingId"] != bestellingen[i - 1].BestellingId)
                {
                    string functie = (string)r["Functie"];

                    Medewerker medewerker = new Medewerker()
                    {
                        MedewerkerId = (int)r["MedewerkerId"],
                        Voornaam = (string)r["Voornaam"],
                        Achternaam = (string)r["Achternaam"],
                        Functie = (Functie)Enum.Parse(typeof(Functie), functie),
                        Email = (string)r["Email"],
                        GebruikersNaam = (string)r["GebruikersNaam"],
                        Wachtwoord = (string)r["Wachtwoord"]
                    };

                    string opmerking;
                    List<string> besteldeItems = new List<string>();
                    try
                    {
                        opmerking = (string)r["BestellingOpmerking"];
                    }
                    catch
                    {
                        opmerking = "";
                    }
                    besteldeItems.Add((string)r["Naam"]);
                    Bestelling bestelling = new Bestelling()
                    {
                        BestellingId = (int)r["BestellingId"],
                        TafelId = (int)r["TafelId"],
                        MedewerkerId = (int)r["MedewerkerId"],
                        Medewerker = medewerker,
                        Status = StatusBestelling.Open,
                        TijdBestelling = (DateTime)r["TijdBestelling"],
                        BestellingOpmerking = opmerking,
                        BesteldeItems = besteldeItems
                    };
                    bestellingen.Add(bestelling);
                    i++;
                }
                else
                {
                    bestellingen[bestellingen.Count - 1].BesteldeItems.Add((string)r["Naam"]);
                }
            }
            return bestellingen;
        }

        public List<Bestelling> DB_Krijg_Alle_Bestellingen_Status_Klaar() //Sander Brijer 646235
        {
            string query = "Select b.BestellingId, b.TafelId, b.MedewerkerId, b.TijdBestelling, b.BestellingOpmerking, mi.Naam, m.MedewerkerId, m.Voornaam, m.Achternaam, m.Functie, m.Email, m.Gebruikersnaam, m.Wachtwoord FROM bestelling AS b JOIN Medewerker AS m ON b.MedewerkerId = m.MedewerkerId JOIN BesteldeItem AS bi ON b.BestellingId = bi.BestellingId JOIN MenuItem AS mi ON mi.MenuItemId = bi.MenuItemId WHERE b.MedewerkerId = m.MedewerkerId AND Status = 'Klaar'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesBestellingenKlaar(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Bestelling> DB_Krijg_Alle_Bestellingen_Status_Open() //Sander Brijer 646235 + Louella Creemers 641347
        {
            string query = "Select b.BestellingId, b.TafelId, b.MedewerkerId, b.TijdBestelling, b.BestellingOpmerking, mi.Naam, m.MedewerkerId, m.Voornaam, m.Achternaam, m.Functie, m.Email, m.Gebruikersnaam, m.Wachtwoord FROM bestelling AS b " +
                           "JOIN Medewerker AS m ON b.MedewerkerId = m.MedewerkerId JOIN BesteldeItem AS bi ON b.BestellingId = bi.BestellingId JOIN MenuItem AS mi ON mi.MenuItemId = bi.MenuItemId " +
                           "WHERE b.MedewerkerId = m.MedewerkerId AND Status = 'Open'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesBestellingenOpen(ExecuteSelectQuery(query, sqlParameters));
        }
    }
}
