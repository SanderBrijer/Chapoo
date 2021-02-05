using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;

namespace ChapooLogica
{
    public class Menu_Service
    {
        Menu_DAO menu_db = new Menu_DAO();

        public List<Menu> AllesOphalen()
        {
            try
            {
                List<Menu> menu = menu_db.DB_Selecteer_Alle_Items();

                return menu;
            }
            catch (Exception)
            {
                throw new Exception("Chapoo (menu) kan niet connecten met de database");
            }

        }
    }
}
