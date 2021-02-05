using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapooModel
{
    public class Bon // Sander Brijer 646235
    {
        public int BonId { get; set; }
        public int BestellingId { get; set; }
        public double TotaalPrijs { get; set; }
        public double Tip { get; set; }
        public bool Betaald { get; set; }
        public string Betaalwijze { get; set; }
        public string BonOpmerking { get; set; }
    }
}
