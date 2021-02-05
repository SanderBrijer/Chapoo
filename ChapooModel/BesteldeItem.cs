using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapooModel
{
    public class BesteldeItem // Sander Brijer 646235
    {
        public int BesteldeItemId { get; set; }
        public int BestellingId { get; set; }
        public Bestelling Bestelling { get; set; }
        public int MenuItemId { get; set; }
        public int VoorraadId { get; set; }
        public string Naam { get; set; }
        public int Aantal { get; set; }
        public string BestellingOpmerking { get; set; }
    }
}
