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
    public class Medewerker_DAO : Base
    {
        public List<Medewerker> DB_Selecteer_Alle_Items()
        {
            string query = "SELECT MedewerkerId, Voornaam, Achternaam, Functie, Email, GebruikersNaam, Wachtwoord FROM Medewerker";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Medewerker> ReadTables(DataTable dataTable) // Sander Brijer 646235
        {
            List<Medewerker> medewerkers = new List<Medewerker>();

            foreach (DataRow r in dataTable.Rows)
            {
                Medewerker medewerker = new Medewerker()
                {
                    MedewerkerId = (int)r["MedewerkerId"],
                    Voornaam = (string)r["Voornaam"],
                    Achternaam = (string)r["Achternaam"],
                    Functie = (Functie)r["Functie"],
                    Email = (string)r["Email"],
                    GebruikersNaam = (string)r["GebruikersNaam"],
                    Wachtwoord = (string)r["Wachtwoord"]
                };
                medewerkers.Add(medewerker);
            }
            return medewerkers;
        }


        public Medewerker DB_Selecteer_Medewerker_Via_Gebruikersnaam_En_Wachtwoord(string gebruikersNaam, string wachtwoord) //Sander Brijer 646235
        {
            string query = $"SELECT MedewerkerId, Voornaam, Achternaam, Functie, Email, GebruikersNaam, Wachtwoord FROM Medewerker WHERE Gebruikersnaam = '{gebruikersNaam}' AND Wachtwoord = '{wachtwoord}'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTable(ExecuteSelectQuery(query, sqlParameters));
        }

        private Medewerker ReadTable(DataTable dataTable) //Sander Brijer 646235
        {
            try
            {

                DataRow r = dataTable.Rows[0];
                string functie = (string)r["Functie"];
                Medewerker medewerker = new Medewerker()
                {
                    Functie = (Functie)Enum.Parse(typeof(Functie), functie),
                    MedewerkerId = (int)r["MedewerkerId"],
                    Voornaam = (string)r["Voornaam"],
                    Achternaam = (string)r["Achternaam"],
                    Email = (string)r["Email"],
                    GebruikersNaam = (string)r["GebruikersNaam"],
                    Wachtwoord = (string)r["Wachtwoord"]
                };
                return medewerker;
            }
            catch
            {
                throw new Exception("Chapoo kan niet connecten met de database");
            }
        }

    }

    public class Medewerker2_DAO : Base
    {
        public List<Medewerker2> DB_Selecteer_Alle_Items()  // Sander Brijer 646235
        {
            string query = $"SELECT MedewerkerId, Voornaam, Achternaam, Functie, Email, GebruikersNaam, Wachtwoord FROM Medewerker";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Medewerker2> DB_Selecteer_Alle_Bediening() // Koen van Cromvoirt 647634
        {
            string query = $"SELECT MedewerkerId, Voornaam, Achternaam, Functie, Email, GebruikersNaam, Wachtwoord FROM Medewerker WHERE Functie = 'Bediening'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Medewerker2> DB_Selecteer_Alle_Koks() // Koen van Cromvoirt 647634
        {
            string query = $"SELECT MedewerkerId, Voornaam, Achternaam, Functie, Email, GebruikersNaam, Wachtwoord FROM Medewerker WHERE Functie = 'Kok'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }
        public List<Medewerker2> DB_Selecteer_Alle_Barman() // Koen van Cromvoirt 647634
        {
            string query = $"SELECT MedewerkerId, Voornaam, Achternaam, Functie, Email, GebruikersNaam, Wachtwoord FROM Medewerker WHERE Functie = 'Barman'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }
        public List<Medewerker2> DB_Selecteer_Alle_Eigenaar() // Koen van Cromvoirt 647634
        {
            string query = $"SELECT MedewerkerId, Voornaam, Achternaam, Functie, Email, GebruikersNaam, Wachtwoord FROM Medewerker WHERE Functie = 'Eigenaar'";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Medewerker2> ReadTables(DataTable dataTable) // Koen van Cromvoirt 647634
        {
            List<Medewerker2> medewerkers = new List<Medewerker2>();

            foreach (DataRow r in dataTable.Rows)
            {
                Medewerker2 medewerker = new Medewerker2()
                {
                    MedewerkerId2 = (int)r["MedewerkerId"],
                    Voornaam2 = (string)r["Voornaam"],
                    Achternaam2 = (string)r["Achternaam"],
                    Functie2 = (String)r["Functie"],
                    Email2 = (string)r["Email"],
                    GebruikersNaam = (string)r["GebruikersNaam"],
                    Wachtwoord2 = (string)r["Wachtwoord"]
                };
                medewerkers.Add(medewerker);
            }
            return medewerkers;
        }

    }

}
