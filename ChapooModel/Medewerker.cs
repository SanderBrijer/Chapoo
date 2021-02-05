using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapooModel
{
    public class Medewerker // Sander Brijer 646235
    {
        private string _gebruikersNaam;
        public int MedewerkerId { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public Functie Functie { get; set; }
        public string Email { get; set; }

        public string GebruikersNaam //Louella Creemers 641347
        {
            get => _gebruikersNaam;
            set => _gebruikersNaam = value;
        }

        public string Wachtwoord { get; set; }
        public string VolledigeNaam { get { return Voornaam + " " + Achternaam; } }
    }

    public class Medewerker2
    {
        private string _gebruikersNaam2;
        public int MedewerkerId2 { get; set; }
        public string Voornaam2 { get; set; }
        public string Achternaam2 { get; set; }
        public string Functie2 { get; set; }
        public string Email2 { get; set; }

        public string GebruikersNaam //Louella Creemers 641347
        {
            get => _gebruikersNaam2;
            set => _gebruikersNaam2 = value;
        }

        public string Wachtwoord2 { get; set; }
        public string VolledigeNaam2 { get { return Voornaam2 + " " + Achternaam2; } }
    }


}
