using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapooDAL;
using ChapooModel;

namespace ChapooLogica
{
    public class MenuItem_Service
    {
        MenuItem_DAO menuItem_db = new MenuItem_DAO();


        public List<MenuItem> AllesOphalen() // Sander Brijer 646235
        {
            try
            {
                List<MenuItem> menuItems = menuItem_db.DB_Selecteer_Alle_Items();

                return menuItems;
            }
            catch
            {
                throw new ArgumentException("Chapoo (menu item) kan niet connecten met de database");
            }

        }
        public MenuItem Selecteer_Een_MenuItem(int menuItem) //Louella Creemers 641347
        {
            try
            {
                return menuItem_db.DB_Selecteer_Een_Item(menuItem);
            }

            catch
            {
                throw new Exception();
            }
        }

        public List<MenuItem> Selecteer_Item_Lunch() //Louella Creemers 641347
        {
            try
            {
                return menuItem_db.DB_Selecteer_Een_Item_Lunch();
            }

            catch
            {
                throw new Exception();
            }
        }

        public List<MenuItem> Selecteer_Item_Diner() //Louella Creemers 641347
        {
            try
            {
                return menuItem_db.DB_Selecteer_Een_Item_Diner();
            }

            catch
            {
                throw new Exception();
            }
        }

        public List<MenuItem> Selecteer_Item_Drank() //Louella Creemers 641347
        {
            try
            {
                return menuItem_db.DB_Selecteer_Een_Item_Drank();
            }

            catch
            {
                throw new Exception();
            }
        }

        public MenuItem Selecteer_MenuItem_Bij_Naam(int menuItemId) //Louella Creemers 641347
        {
            try
            {
                return menuItem_db.DB_Selecteer_Een_Item_Bij_Naam(menuItemId);
            }
            catch
            {
                throw new Exception();
            }

        }

        public MenuItem Selecteer_MenuItem_Bij_Id(string naam) //Louella Creemers 641347
        {
            try
            {
                return menuItem_db.DB_Selecteer_Een_Item_Bij_Id(naam);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
