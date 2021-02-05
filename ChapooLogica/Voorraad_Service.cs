using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;

namespace ChapooLogica
{
    public class Voorraad_Service
    {
        Voorraad_DAO voorraad_db = new Voorraad_DAO();

        public List<Voorraad> AllesOphalen()
        {
            try
            {
                List<Voorraad> voorraad = voorraad_db.DB_Selecteer_Alle_Items();

                return voorraad;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (tafel) kan niet connecten met de database");
            }

        }
        public List<Voorraad> KrijgAllesVoorraadBar() // John Bond 649770
        {
            try
            {
                List<Voorraad> voorraad = voorraad_db.DB_Selecteer_Alle_Items_Bar();

                return voorraad;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (voorraad) kan niet connecten met de database");
            }
        }
        public List<Voorraad> KrijgAllesVoorraadKeuken() // John Bond 649770
        {
            try
            {
                List<Voorraad> voorraad = voorraad_db.DB_Selecteer_Alle_Items_Keuken();

                return voorraad;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (voorraad) kan niet connecten met de database");
            }

        }

        public void BewerkenVoorraadAantal(int VoorraadId, int nieuweVoorraadAantal) // Koen van Cromvoirt 647634
        {
            try
            {
                voorraad_db.DB_UpdateVoorraad(VoorraadId, nieuweVoorraadAantal);
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (reserveringen) kan niet connecten met de database");
            }

        }
    }
}