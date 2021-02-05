using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapooModel
{
    public class Tafel // Sander Brijer 646235
    {
        public int TafelId { get; set; }
        //public string Status { get; set; }
        public StatusTafel Status { get; set; }
        public bool Gereserveerd { get; set; }
        public int Zitplekken { get; set; }
    }
}
