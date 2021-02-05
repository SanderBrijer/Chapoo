using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;

namespace ChapooLogica
{
    public class BesteldeItem_Service
    {
        BesteldeItem_DAO besteldeItem_db = new BesteldeItem_DAO();

        public void AlleItemsToevoegen(int bestellingId, int menuItemId, int aantal) //Louella Creemers 641347
        {
            try
            {
                besteldeItem_db.BesteldeItemToevoegen(bestellingId, menuItemId, aantal);
            }

            catch
            {
                throw new Exception("Kan item niet toevoegen aan bestelling");
            }
        }
        public List<BesteldeItem> KrijgAlleDetailsBarman(string bestellingId) // John Bond 649770
        {
            try
            {
                List<BesteldeItem> bestelling = besteldeItem_db.DB_Selecteer_Bestelling_Details_Op_BestellingId_Bar(bestellingId);

                return bestelling;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bestelde items) kan niet connecten met de database");
            }

        }
        public List<BesteldeItem> KrijgAlleDetailsKeuken(string bestellingId) // John Bond 649770
        {
            try
            {
                List<BesteldeItem> bestelling = besteldeItem_db.DB_Selecteer_Bestelling_Details_Op_BestellingId_Keuken(bestellingId);

                return bestelling;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bestelde items) kan niet connecten met de database");
            }

        }
    }
}
