using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;

namespace ChapooLogica
{
    public class Klant_Service
    {
        Klant_DAO klant_db = new Klant_DAO();

        public List<Klant> AllesOphalen() // Sander Brijer 646235
        {
            try
            {
                List<Klant> klanten = klant_db.DB_Selecteer_Alle_Items();

                return klanten;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (klant) kan niet connecten met de database");
            }

        }

        public void ToevoegenKlant(string klantNaam, string telefoonnummer, string emailadres) // Sander Brijer 646235
        {
            try
            {
                klant_db.Db_ToevoegenKlant(klantNaam, telefoonnummer, emailadres);
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (klant) kan niet connecten met de database");
            }

        }

        public Klant KrijgLaatsteKlant(string klantNaam, string telefoonNummer, string email) // Sander Brijer 646235
        {
            try
            {
                Klant klant = klant_db.Db_KrijgLaatsteKlant(klantNaam, telefoonNummer, email);
                return klant;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (klant) kan niet connecten met de database");
            }

        }
    }
}
