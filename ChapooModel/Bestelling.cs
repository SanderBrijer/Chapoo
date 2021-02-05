using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapooModel
{
    public class Bestelling // Sander Brijer 646235
    {
        public int BestellingId { get; set; }
        public int TafelId { get; set; }
        public int MedewerkerId { get; set; }
        public Medewerker Medewerker { get; set; }
        public DateTime TijdBestelling { get; set; }
        public StatusBestelling Status { get; set; }
        public string BestellingOpmerking { get; set; }
        public List<string> BesteldeItems { get; set; }
    }
}
