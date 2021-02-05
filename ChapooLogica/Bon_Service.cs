using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;

namespace ChapooLogica
{
    public class Bon_Service
    {
        Bon_DAO bon_db = new Bon_DAO();

        public List<Bon> AllesOphalen()
        {
            try
            {
                List<Bon> bon = bon_db.DB_Selecteer_Alle_Items();

                return bon;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (bonnen) kan niet connecten met de database");
            }

        }
    }
}
