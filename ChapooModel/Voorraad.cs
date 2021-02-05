using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapooModel
{
    public class Voorraad // Sander Brijer 646235
    {
        public int VoorraadId { get; set; }
        public string Naam { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public int VoorraadAantal { get; set; }
    }
}
