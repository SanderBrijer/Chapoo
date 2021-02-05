using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;

namespace ChapooLogica
{
    public class Bestelling_Service
    {
        Bestelling_DAO bestellingDao = new Bestelling_DAO();
        public List<Bestelling> Selecteer_BestellingenViaTafelId(int tafelId) //Louella Creemers 641337
        {
            try
            {
                return bestellingDao.DB_Selecteer_Alle_BestellingenViaTafelId(tafelId);
            }

            catch
            {
                throw new Exception("Kan bestellingen niet selecteren in database");
            }
        }

        public int Maak_Nieuwe_Bestelling(int tafelId, int medewerkerId, string Opmerking) //Louella Creemers 641347
        {
            try
            {
                return bestellingDao.DB_Maak_Bestelling_Aan(tafelId, medewerkerId, Opmerking);

            }

            catch
            {
                throw new Exception("Kan deze bestelling niet selecteren in database");
            }
        }

        public void VerwijderBestelling(int bestellingId) // Sander Brijer 646235
        {
            try
            {
                bestellingDao.DB_Verwijder_Bestelling(bestellingId);
            }
            catch (Exception e)
            {
                throw new Exception("Kan deze bestelling niet selecteren in database");
            }
        }

        public List<Bestelling> KrijgAlleBestellingenBar() // John Bond 649770
        {
            try
            {
                List<Bestelling> bestellingenBar = bestellingDao.DB_Selecteer_Alle_Bestellingen_Bar();
                return bestellingenBar;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bestellingen) kan niet connecten met de database");
            }
        }

        public List<Bestelling> KrijgAlleBestellingenBarGeschiedenis() // John Bond 649770
        {
            try
            {
                List<Bestelling> bestellingenBar = bestellingDao.DB_Selecteer_Alle_Bestellingen_Bar_Geschiedenis();
                return bestellingenBar;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bestellingen) kan niet connecten met de database");
            }
        }
        public List<Bestelling> KrijgAlleBestellingenKeuken() // John Bond 649770
        {
            try
            {
                List<Bestelling> bestellingenKeuken = bestellingDao.DB_Selecteer_Alle_Bestellingen_Keuken();
                return bestellingenKeuken;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bestellingen) kan niet connecten met de database");
            }
        }
        public List<Bestelling> KrijgAlleBestellingenKeukenGeschiedenis() // John Bond 649770
        {
            try
            {
                List<Bestelling> bestellingenKeuken = bestellingDao.DB_Selecteer_Alle_Bestellingen_Keuken_Geschiedenis();
                return bestellingenKeuken;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bestellingen) kan niet connecten met de database");
            }
        }
        public void VeranderStatus(int bestellingId, StatusBestelling bestellingStatus) // John Bond 649770
        {
            try
            {
                bestellingDao.DB_Verander_Status(bestellingId, bestellingStatus);
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bestellingen) kan niet connecten met de database");
            }
        }

        public List<Bestelling> KrijgAlleBestellingenStatusKlaar() // Sander Brijer 646235
        {
            try
            {
                List<Bestelling> bestellingenDieKlaarZijn = bestellingDao.DB_Krijg_Alle_Bestellingen_Status_Klaar();
                return bestellingenDieKlaarZijn;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bestellingen) kan niet connecten met de database");
            }
        }

        public List<Bestelling> KrijgAlleBestellingenStatusOpen() // Sander Brijer 646235 + Louella Creemers 641347
        {
            try
            {
                List<Bestelling> bestellingenDieOpenZijn = bestellingDao.DB_Krijg_Alle_Bestellingen_Status_Open();
                return bestellingenDieOpenZijn;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bestellingen) kan niet connecten met de database");
            }
        }
    }
}
