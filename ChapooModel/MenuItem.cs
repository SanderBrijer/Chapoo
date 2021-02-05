using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapooModel
{
    public class MenuItem // Sander Brijer 646235
    {
        public int MenuItemId { get; set; }
        public int MenuId { get; set; }
        public string Naam { get; set; }
        public double Prijs { get; set; }
        public int VoorraadAantal { get; set;}
        
        
    }
}
