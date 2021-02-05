using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapooModel
{
    public class Reservering // Sander Brijer 646235
    {
        public int ReserveringId { get; set; }

        public Klant Klant { get; set; }
        public int TafelId { get; set; }
        public int AantalPersonen { get; set; }
        public DateTime AankomstDatumTijd { get; set; }

        public string ReserveringOpmerking { get; set; }
    }
}
