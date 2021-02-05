using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;

namespace ChapooLogica
{
    public class Medewerker_Service
    {
        Medewerker_DAO medewerker_db = new Medewerker_DAO();

        public List<Medewerker> AllesOphalen() // Sander Brijer 646235
        {
            try
            {
                List<Medewerker> medewerker = medewerker_db.DB_Selecteer_Alle_Items();

                return medewerker;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (medewerker) kan niet connecten met de database");
            }

        }

        public Medewerker KrijgInloggegevensViaGebruikersnaam(string gebruikersNaam, string wachtwoord) // Sander Brijer 646235
        {
            try
            {
                Medewerker medewerkerLogin =
                    medewerker_db.DB_Selecteer_Medewerker_Via_Gebruikersnaam_En_Wachtwoord(gebruikersNaam, wachtwoord);

                return medewerkerLogin;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class Medewerker2_Service
    {
        Medewerker2_DAO medewerker_db = new Medewerker2_DAO();

        public List<Medewerker2> AllesOphalen()
        {
            List<Medewerker2> medewerker = medewerker_db.DB_Selecteer_Alle_Items();

            return medewerker;

        }

        public List<Medewerker2> BedieningOphalen()
        {
            List<Medewerker2> medewerker = medewerker_db.DB_Selecteer_Alle_Bediening();

            return medewerker;

        }

        public List<Medewerker2> KoksOphalen()
        {
            List<Medewerker2> medewerker = medewerker_db.DB_Selecteer_Alle_Koks();

            return medewerker;

        }

        public List<Medewerker2> BarmanOphalen()
        {
            List<Medewerker2> medewerker = medewerker_db.DB_Selecteer_Alle_Barman();

            return medewerker;

        }

        public List<Medewerker2> EigenaarOphalen()
        {
            List<Medewerker2> medewerker = medewerker_db.DB_Selecteer_Alle_Eigenaar();


            return medewerker;

        }
    }
}
