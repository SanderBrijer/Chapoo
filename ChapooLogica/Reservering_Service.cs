using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;


namespace ChapooLogica
{
    public class Reservering_Service
    {
        Reservering_DAO reservering_db = new Reservering_DAO();

        public List<Reservering> AllesOphalen() // Sander Brijer 646235
        {
            try
            {
                List<Reservering> reserveringen = reservering_db.DB_Selecteer_Alle_Items();

                return reserveringen;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (reserveringen) kan niet connecten met de database");
            }

        }

        public Reservering KrijgReserveringBijId(int reserveringNummer) // Sander Brijer 646235
        {
            try
            {
                Reservering reservering = reservering_db.DB_Selecteer_Via_Id(reserveringNummer);

                return reservering;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (reserveringen) kan niet connecten met de database");
            }

        }

        public void ToevoegenReservering(int klantId, int tafelId, int aantalPersonen, DateTime aankomstDatumTijd, string opmerking) // Sander Brijer 646235
        {
            try
            {
                reservering_db.Db_ToevoegenReservering(klantId, tafelId, aantalPersonen, aankomstDatumTijd, opmerking);
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (reserveringen) kan niet connecten met de database");
            }

        }

        public void BewerkenReservering(int tafelId, int aantalPersonen, DateTime aankomstDatumTijd, string opmerking, int reserveringId) // Sander Brijer 646235
        {
            try
            {
                reservering_db.Db_BewerkenReservering(tafelId, aantalPersonen, aankomstDatumTijd, opmerking, reserveringId);
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (reserveringen) kan niet connecten met de database");
            }

        }

        public void VerwijderReservering(int reserveringId) // Sander Brijer 646235
        {
            try
            {
                reservering_db.Db_VerwijderReservering(reserveringId);
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (reserveringen) kan niet connecten met de database");
            }

        }
    }
}
