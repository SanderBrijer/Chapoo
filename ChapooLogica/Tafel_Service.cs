using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;

namespace ChapooLogica
{
    public class Tafel_Service
    {
        Tafel_DAO tafel_db = new Tafel_DAO();

        public List<Tafel> AllesOphalen() // Sander Brijer 646235
        {
            try
            {
                List<Tafel> tafels = tafel_db.DB_Selecteer_Alle_Tafels();

                return tafels;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (tafel) kan niet connecten met de database");
            }

        }


        public void BewerkenTafelStatus(int tafelId, StatusTafel nieuweStatus) // Sander Brijer 646235
        {
            try
            {
                tafel_db.DB_BewerkenTafelStatus(tafelId, nieuweStatus);
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (reserveringen) kan niet connecten met de database");
            }

        }

        public void BewerkenTafelZitplekken(int tafelId, int nieuweZitplekken) // Sander Brijer 646235
        {
            try
            {
                tafel_db.DB_BewerkenTafelZitplekken(tafelId, nieuweZitplekken);
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (reserveringen) kan niet connecten met de database");
            }

        }
    }
}