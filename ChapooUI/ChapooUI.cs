//      Chapoo Team 06

//      Sander Brijer	646235
//      John Bond   649770
//		Louëlla Creemers    641347
//		Koen van Cromvoirt	647634


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChapooDAL;
using ChapooModel;
using ChapooLogica;
using MenuItem = ChapooModel.MenuItem;

namespace ChapooUI
{
    public partial class BestelsysteemChapoo : Form
    {
        List<Log> logList = new List<Log>();
        private bool Ingelogd { get; set; }
        List<string> querystring { get; set; }
        private bool reedsGeklikt0TeServeren { get; set; }
        private bool reedsGeklikt1TeServeren { get; set; }
        private bool reedsGeklikt2TeServeren { get; set; }
        private bool reedsGeklikt3TeServeren { get; set; }
        private bool reedsGeklikt0ReserveringOverzicht { get; set; }
        private bool reedsGeklikt1ReserveringOverzicht { get; set; }
        private bool reedsGeklikt2ReserveringOverzicht { get; set; }
        private bool reedsGeklikt3ReserveringOverzicht { get; set; }

        private bool DoeHerstartTafels { get; set; }
        private List<int> tijdVerschilPerTafel { get; set; }
        private Medewerker IngelogdeMedewerker { get; set; }
        private int geselecteerdeTafelTafelOverzicht { get; set; }
        private Medewerker_Service medewerker_Service;
        private Tafel_Service tafel_Service;
        private List<Tafel> tafels;
        private List<Reservering> reserveringen;
        private List<Klant> klanten;
        List<Bestelling> bestellingenKlaar;
        private List<Bestelling> bestellingenOpen;
        private Klant_Service klant_Service;
        private Bestelling_Service bestelling_Service;
        private Reservering_Service reservering_Service;
        private BesteldeItem_Service besteldeItem_Service;
        private MenuItem_Service menuItem_Service;
        private Voorraad_Service voorraad_Service;
        List<MenuItem> items;

        private StatusBestelling StatusBestelling { get; set; }


        public BestelsysteemChapoo() //Sander Brijer 646235
        {
            querystring = new List<string>
            {
                "select",
                "*",
                "insert",
                "update",
                "remove",
                "update",
                "null"
            };
            reedsGeklikt0TeServeren = true;
            reedsGeklikt1TeServeren = true;
            reedsGeklikt2TeServeren = true;
            reedsGeklikt3TeServeren = true;
            tijdVerschilPerTafel = new List<int>();
            Ingelogd = false;
            medewerker_Service = new Medewerker_Service();
            bestelling_Service = new Bestelling_Service();
            tafel_Service = new Tafel_Service();
            reservering_Service = new Reservering_Service();
            klant_Service = new Klant_Service();
            besteldeItem_Service = new BesteldeItem_Service();
            voorraad_Service = new Voorraad_Service();
            menuItem_Service = new MenuItem_Service();
            items = new List<MenuItem>();
            reserveringen = new List<Reservering>();
            klanten = new List<Klant>();
            bestellingenKlaar = new List<Bestelling>();
            InitializeComponent();
            Program();
        }

        //GEDEELTE GEMAAKT DOOR SANDER BRIJER

        // Opstart methoden
        public void Program() //Sander Brijer 646235
        {
            //Tablet scherm
            TabletScherm();
            lblReserveringOverzichtFoutmelding.Text = string.Empty;
            AllePanelsOnzichtbaar();
            Inlog();
        }

        // - Inlog; kijk of er is ingelogd en ga hierop verder.
        private void Inlog() //Sander Brijer 646235
        {
            if (Ingelogd == false)
            {
                txtLoginWachtwoord.Text = string.Empty;
                if (txtLoginWachtwoord.Text == "")
                {
                    txtLoginWachtwoord.UseSystemPasswordChar = false;
                    txtLoginWachtwoord.ForeColor = Color.Gray;
                    txtLoginWachtwoord.Text = "wachtwoord";
                }
                txtLoginGebruikersnaam.Text = string.Empty;
                pnlInloggen.Show();
                lblLoginFoutmelding.Text = string.Empty;
            }
            else
            {
                IngelogdeMedewerker = new Medewerker();
                FunctieVerwerking();
            }
        }

        // -- Reservering email tekst
        private void txtInlogGebruikersnaam_Enter(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtLoginGebruikersnaam.Text == "gebruikersnaam")
            {
                txtLoginGebruikersnaam.ForeColor = Color.Black;
                txtLoginGebruikersnaam.Text = "";
            }
        }

        private void txtInlogGebruikersnaam_Leave(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtLoginGebruikersnaam.Text == "")
            {
                txtLoginGebruikersnaam.ForeColor = Color.Gray;
                txtLoginGebruikersnaam.Text = "gebruikersnaam";
            }
        }

        // -- Inlog wachtwoord tekst
        private void txtInlogWachtwoord_Enter(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtLoginWachtwoord.Text == "wachtwoord")
            {
                txtLoginWachtwoord.UseSystemPasswordChar = true;
                txtLoginWachtwoord.ForeColor = Color.Black;
                txtLoginWachtwoord.Text = "";
            }
        }

        private void txtInlogWachtwoord_Leave(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtLoginWachtwoord.Text == "")
            {
                txtLoginWachtwoord.UseSystemPasswordChar = false;
                txtLoginWachtwoord.ForeColor = Color.Gray;
                txtLoginWachtwoord.Text = "wachtwoord";
            }
        }

        // - Roep de methode aan die kijkt of de inloggegevens juist zijn.
        private void btnInloggen_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            CheckInlog();
        }

        // - Panel bekijk of inloggegevens juist zijn.
        void CheckInlog() //Sander Brijer 646235
        {
            try
            {
                string gebruikersnaam = txtLoginGebruikersnaam.Text.ToLower();
                if (gebruikersnaam == string.Empty)
                {
                    throw new Exception("Geen gebruikersnaam ingevoerd.");
                }
                ControleerOpQuery(gebruikersnaam);
                string wachtwoord = txtLoginWachtwoord.Text;
                if (wachtwoord == string.Empty)
                {
                    throw new Exception("Geen wachtwoord ingevoerd.");
                }
                ControleerOpQuery(wachtwoord);

                IngelogdeMedewerker = medewerker_Service.KrijgInloggegevensViaGebruikersnaam(gebruikersnaam, wachtwoord);
                if (IngelogdeMedewerker != null)
                {
                    this.Ingelogd = true;
                    pnlInloggen.Hide();
                    FunctieVerwerking();
                }
                else
                {
                    throw new Exception("Inloggegevens zijn onjuist.");
                }
            }
            catch (Exception e)
            {
                lblLoginFoutmelding.Text = "⚠ " + e.Message;
                lblLoginFoutmelding.Show();
            }
        }

        //Bekijk welke functie er bij de inlog hoort en geef rechten af.
        void FunctieVerwerking()
        {
            try
            {
                if (IngelogdeMedewerker.Functie == Functie.Barman || IngelogdeMedewerker.Functie == Functie.Kok)
                {
                    //COMPUTER
                    //scherm
                    ComputerScherm();
                    //logo
                    if (IngelogdeMedewerker.Functie == Functie.Kok)
                    {
                        MenuKok();
                    }
                    else if (IngelogdeMedewerker.Functie == Functie.Barman)
                    {
                        MenuBar();
                    }
                }
                if (IngelogdeMedewerker.Functie == Functie.Eigenaar || IngelogdeMedewerker.Functie == Functie.Bediening)
                {
                    //TABBLAD
                    //scherm
                    TabletScherm();
                    if (IngelogdeMedewerker.Functie == Functie.Bediening)
                    {
                        MenuBediening();
                    }
                    else if (IngelogdeMedewerker.Functie == Functie.Eigenaar)
                    {
                        MenuEigenaar();
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        //het systeem wordt nu een computer
        private void ComputerScherm()
        {
            pbChapooLogo.BringToFront();
            //b - h
            pbChapooLogo.Location = new Point(5, 5);

            this.Size = new System.Drawing.Size(1020, 600);
        }

        //het systeem wordt nu een tablet
        private void TabletScherm()
        {
            pbChapooLogo.BringToFront();
            //b - h
            pbChapooLogo.Location = new Point(500, 0);
            this.Size = new System.Drawing.Size(680, 850);
        }


        // Open het menu voor de bediening
        private void MenuBediening()
        {
            btnBarmanMenu.Hide();
            btnKeukenMenu.Hide();
            btnMenuVoorraadEigenaar.Hide();
            lblMenuFunctie.Text = "Bediening";
            pnlMenu.Show();
        }

        // Open het menu voor de barman

        private void btnBarmanMenu_Click(object sender, EventArgs e) //Sander Brijer 646235 
        {
            //maak het een computerscherm
            ComputerScherm();
            AllePanelsOnzichtbaar();
            pnlBarmanMenu.Show();
        }

        private void MenuBar()
        {
            pnlBarmanMenu.Show();
        }

        // Open het menu voor de kok
        private void MenuKok()
        {
            pnlKeukenMenu.Show();
        }

        // Open het menu voor de eigenaar
        private void MenuEigenaar()
        {
            TabletScherm();
            btnBarmanMenu.Show();
            btnKeukenMenu.Show();
            btnMenuVoorraadEigenaar.Show();
            lblMenuFunctie.Text = "Eigenaar";
            btnMenuVoorraadBediening.Hide();
            pnlMenu.Show();
        }

        //Ga naar het menu
        private void pbChapooLogo_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                if (pnlInloggen.Visible == false)
                {
                    AllePanelsOnzichtbaar();
                    if (IngelogdeMedewerker.Functie == Functie.Barman)
                    {
                        MenuBar();
                    }
                    if (IngelogdeMedewerker.Functie == Functie.Bediening)
                    {
                        MenuBediening();
                    }
                    if (IngelogdeMedewerker.Functie == Functie.Eigenaar)
                    {
                        MenuEigenaar();
                    }
                    if (IngelogdeMedewerker.Functie == Functie.Kok)
                    {
                        MenuKok();
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        //alle panelen onzichtbaar
        public void AllePanelsOnzichtbaar() //Sander Brijer 646235
        {
            foreach (Control c in this.Controls)
            {
                if (c is Panel) c.Visible = false;
            }
        }

        // - Controle methoden
        public void ControleerOpQuery(string teControlerenString)
        {
            if (querystring.Contains(teControlerenString))
            {
                teControlerenString.ToLower();
                throw new Exception($"Invoer mag geen SQL-query bevatten.");
            }
        }

        // - Panel Tafeloverzicht methoden
        private void btnVerversen_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            LaatTafeloverzicht();
        }

        private void btnReserveringOverzichtNaarTafelOverzicht_Click(object sender, EventArgs e)
        {
            LaatTafeloverzicht();
        }

        private void btnTafeloverzicht_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            LaatTafeloverzicht();
        }

        // - Panel Tafeloverzicht methoden 
        // -- Laat alle reserveringen
        private void LaatTafeloverzicht() //Sander Brijer 646235
        {
            try
            {

                pnlTafeloverzicht.BringToFront();
                pbChapooLogo.BringToFront();
                pnlTafeloverzicht.Show();

                tafels = tafel_Service.AllesOphalen();

                cbSelecteerEenTafel.Items.Clear();
                foreach (Tafel tafel in tafels)
                {
                    cbSelecteerEenTafel.Items.Add(tafel.TafelId);
                }

                cbTafeloverzichtAantalZitplaatsen.Items.Clear();
                for (int i = 1; i <= 10; i++)
                {
                    cbTafeloverzichtAantalZitplaatsen.Items.Add(i);
                }

                LaatListViewTafelOverzicht(true);

                cbSelecteerStatus.Items.Clear();
                cbSelecteerStatus.Items.Add("Vrij");
                cbSelecteerStatus.Items.Add("Bezet");

                if (tafels.Count > 0)
                {
                    DoeHerstartTafels = false;
                    cbSelecteerEenTafel.SelectedIndex = 0;
                    DoeHerstartTafels = true;
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- Laat de listview voor het tafeloverzicht
        private void LaatListViewTafelOverzicht(bool tijdookmeenemen) //Sander Brijer 646235
        {
            try
            {
                listvOverzichtTafels.Items.Clear();
                if (tijdookmeenemen)
                {
                    tijdVerschilPerTafel.Clear();
                }
                else
                {
                    tafels = tafel_Service.AllesOphalen();
                }
                for (int i = 0; i < tafels.Count; i++)
                {
                    if (tijdookmeenemen)
                    {
                        List<Bestelling> bestellingen = bestelling_Service.Selecteer_BestellingenViaTafelId(tafels[i].TafelId);
                        if (bestellingen.Count != 0)
                        {
                            DateTime date1 = bestellingen[0].TijdBestelling;
                            DateTime date2 = DateTime.Now;
                            TimeSpan tijdverschilInMinuten = date2 - date1;
                            double tijd = tijdverschilInMinuten.TotalMinutes;
                            int tijdInt = (int)tijd;
                            if (tijdInt > 0)
                            {
                                tijdVerschilPerTafel.Add(tijdInt);
                            }
                            else
                            {
                                tijdVerschilPerTafel.Add(0);
                            }
                        }
                        else
                        {
                            tijdVerschilPerTafel.Add(0);
                        }
                    }
                    ListViewItem item = new ListViewItem(tafels[i].TafelId.ToString());
                    item.SubItems.Add(tafels[i].Status.ToString());
                    item.SubItems.Add(tafels[i].Zitplekken.ToString());
                    item.SubItems.Add($"{tijdVerschilPerTafel[i]} minuten");
                    item.SubItems.Add(tafels[i].Gereserveerd.ToString());
                    listvOverzichtTafels.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- Indien aantal zitplaatsen veranderd > direct naar de database wegschrijven
        private void cbTafeloverzichtAantalZitplaatsen_SelectedIndexChanged(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                if (DoeHerstartTafels == true)
                {
                    geselecteerdeTafelTafelOverzicht = cbSelecteerEenTafel.SelectedIndex;
                    tafel_Service.BewerkenTafelZitplekken(geselecteerdeTafelTafelOverzicht + 1, cbTafeloverzichtAantalZitplaatsen.SelectedIndex + 1);
                    LaatListViewTafelOverzicht(false);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- Indien tafel veranderd > direct gegevens vanuit de database opvragen
        private void cbSelecteerEenTafel_SelectedIndexChanged(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                tafels = tafel_Service.AllesOphalen();
                geselecteerdeTafelTafelOverzicht = cbSelecteerEenTafel.SelectedIndex;
                cbSelecteerStatus.SelectedItem = tafels[geselecteerdeTafelTafelOverzicht].Status.ToString();
                cbTafeloverzichtAantalZitplaatsen.SelectedItem = tafels[geselecteerdeTafelTafelOverzicht].Zitplekken;
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- Indien status veranderd > direct naar de database wegschrijven
        private void cbSelecteerStatus_SelectedIndexChanged(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                if (DoeHerstartTafels == true)
                {
                    geselecteerdeTafelTafelOverzicht = cbSelecteerEenTafel.SelectedIndex + 1;
                    try
                    {
                        StatusTafel nieuweStatus = (StatusTafel)Enum.Parse(typeof(StatusTafel), cbSelecteerStatus.Text);
                        if (geselecteerdeTafelTafelOverzicht > 0)
                        {
                            tafel_Service.BewerkenTafelStatus(geselecteerdeTafelTafelOverzicht, nieuweStatus);
                        }
                    }
                    catch (Exception ex)
                    {
                        Catch(ex);
                    }
                    finally
                    {
                        LaatListViewTafelOverzicht(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // - Panel ReserveringOpnemen methoden
        private void cbReserveringSelecteerKlant_SelectedIndexChanged(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {

                if (klanten.Count > 0)
                {
                    txtReserveringNaamKlant.Text = klanten[cbReserveringSelecteerKlant.SelectedIndex].Naam;
                    txtReserveringTelefoonnummer.Text = klanten[cbReserveringSelecteerKlant.SelectedIndex].Telefoonnummer.ToString();
                    txtReserveringEmailadres.Text = klanten[cbReserveringSelecteerKlant.SelectedIndex].Email;
                }
                else
                {
                    txtReserveringNaamKlant.Text = string.Empty;
                    txtReserveringTelefoonnummer.Text = string.Empty;
                    txtReserveringEmailadres.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void rbReserveringBestaandeKlant_CheckedChanged(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                klanten = klant_Service.AllesOphalen();
                klanten = klanten.OrderBy(klant => klant.Naam).ToList();
                cbReserveringSelecteerKlant.Items.Clear();
                cbReserveringSelecteerKlant.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (Klant klant in klanten)
                {
                    cbReserveringSelecteerKlant.Items.Add($"{klant.Naam} ({klant.Telefoonnummer})");
                }
                if (klanten.Count > 0)
                {
                    cbReserveringSelecteerKlant.SelectedIndex = 0;
                }
                ReserveringZichtbaarheid(false);
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void rbReserveringNieuweKlant_CheckedChanged(object sender, EventArgs e) //Sander Brijer 646235
        {
            txtReserveringNaamKlant.ForeColor = Color.Gray;
            txtReserveringNaamKlant.Text = "Voornaam Achternaam";
            txtReserveringTelefoonnummer.ForeColor = Color.Gray;
            txtReserveringTelefoonnummer.Text = "31612345678";
            txtReserveringEmailadres.ForeColor = Color.Gray;
            txtReserveringEmailadres.Text = "voorbeeld@mail.com";

            ReserveringZichtbaarheid(true);
        }

        private void ReserveringZichtbaarheid(bool zichtbaar) //Sander Brijer 646235
        {
            lblReserveringEmailadres.Visible = zichtbaar;
            lblReserveringNaamKlant.Visible = zichtbaar;
            lblReserveringTelefoonnummer.Visible = zichtbaar;
            lblReserveringSelecteerKlant.Visible = !zichtbaar;
            cbReserveringSelecteerKlant.Visible = !zichtbaar;
            txtReserveringNaamKlant.Visible = zichtbaar;
            txtReserveringEmailadres.Visible = zichtbaar;
            txtReserveringTelefoonnummer.Visible = zichtbaar;
        }

        //Verstuur reservering naar database
        private void btnReserveringVersturen_Click(object sender, EventArgs e) // Sander Brijer 646235
        {
            try
            {
                //NAAM
                string klantNaam = txtReserveringNaamKlant.Text;
                if (klantNaam == string.Empty || klantNaam == "Voornaam Achternaam")
                {
                    throw new Exception("Geen klantnaam opgegeven");
                }
                ControleerOpQuery(klantNaam);

                //TELEFOONNUMMER
                string telefoonNummer = txtReserveringTelefoonnummer.Text;
                if (!txtReserveringTelefoonnummer.Text.StartsWith("316") || txtReserveringTelefoonnummer.Text.Length != 11)
                {
                    throw new Exception("Geen geldig telefoonnummer opgegeven");
                }
                ControleerOpQuery(telefoonNummer);

                //EMAIL
                string email = txtReserveringEmailadres.Text;
                try
                {
                    var mailadres = new System.Net.Mail.MailAddress(email);
                    bool geldigAdres = (mailadres.Address == email);
                    if (geldigAdres == false || mailadres.ToString() == "voorbeeld@mail.nl")
                    {
                        throw new Exception("Geen geldig emailadres opgegeven");
                    }
                }
                catch
                {
                    throw new Exception("Geen geldig emailadres opgegeven");
                }
                ControleerOpQuery(email);

                //AANKOMSTTIJD
                DateTime aankomstDatumTijd = dtpReserveringOpnemenDatumTijd.Value;

                //TAFEL
                int tafelId = cbReserveringSelecteerTafel.SelectedIndex;
                if (tafelId < 0 || tafelId > tafels.Count)
                {
                    throw new Exception("Geen geldige tafel ingevoerd");
                }
                else if (tafelId == 0)
                {
                    Random random = new Random();
                    tafelId = random.Next(1, tafels.Count + 1);
                }
                int aantalPersonen = int.Parse(nudReserveringAantalPersonen.Value.ToString());

                string opmerking = "" + txtReserveringOpmerking.Text;
                ControleerOpQuery(opmerking);

                //Hier controleren of het om opnemen of wijzigen gaat
                if (lblReserveringOpnemenOfWijzigen.Text == "Reservering wijzigen")
                {
                    //wijzigen reservering
                    int reserveringId = int.Parse(cbReserveringOverzichtSelecteerEenReservering.Text);
                    reservering_Service.BewerkenReservering(tafelId, aantalPersonen, aankomstDatumTijd, opmerking, reserveringId);
                }
                else
                {
                    //toevoegen reservering
                    if (rbReserveringNieuweKlant.Checked == true)
                    {
                        klant_Service.ToevoegenKlant(klantNaam, telefoonNummer, email);
                        Klant klant = klant_Service.KrijgLaatsteKlant(klantNaam, telefoonNummer, email);

                        reservering_Service.ToevoegenReservering(klant.KlantId, tafelId, aantalPersonen, aankomstDatumTijd, opmerking);
                    }
                    else
                    {
                        int klantId = klanten[cbReserveringSelecteerKlant.SelectedIndex].KlantId;
                        reservering_Service.ToevoegenReservering(klantId, tafelId, aantalPersonen, aankomstDatumTijd, opmerking);
                    }
                }
                //herstart en feedback
                LaatReserveringOverzichtPanel();
                lblReserveringOverzichtFoutmelding.Text = "✔ Reservering verwerkt.";
            }
            catch (Exception exception)
            {
                lblReserveringFoutmelding.Text = $"⚠ {exception.Message}";
                lblReserveringFoutmelding.Show();
            }

        }

        // -- Reservering opnemen
        private void btnReserveringOpnemen_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            //Alle textboxen moeten zichtbaar en worden ingevuld met voorbeeld tekst.
            AllePanelsOnzichtbaar();

            ReserveringOpnemen();

        }

        // -- Reservering opnemen
        public void ReserveringOpnemen() //Sander Brijer 646235
        {
            try
            {
                rbReserveringBestaandeKlant.Visible = true;
                rbReserveringNieuweKlant.Visible = true;
                lblReserveringOpnemenOfWijzigen.Text = "Reservering opnemen";
                txtReserveringEmailadres.Enabled = true;
                txtReserveringNaamKlant.Enabled = true;
                txtReserveringTelefoonnummer.Enabled = true;
                txtReserveringEmailadres.Text = "voorbeeld@mail.com";
                txtReserveringNaamKlant.Text = "Voornaam Achternaam";
                txtReserveringTelefoonnummer.Text = "31612345678";
                nudReserveringAantalPersonen.Value = 1;
                CbReserveringSelecteerTafelVullen();
                cbReserveringSelecteerTafel.SelectedIndex = 0;
                ReserveringClear();
                pnlTafeloverzicht.Hide();
                pnlReserveringoverzicht.Hide();
                pnlReserveringen.Show();
                ReserveringTextInvulling();
                nudReserveringAantalPersonen.Minimum = 1;
                nudReserveringAantalPersonen.Maximum = 10;
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- teksten clearen
        private void ReserveringClear() //Sander Brijer 646235
        {
            dateTimePickerReservering();
            txtReserveringEmailadres.Text = string.Empty;
            txtReserveringNaamKlant.Text = string.Empty;
            txtReserveringOpmerking.Text = string.Empty;
            txtReserveringTelefoonnummer.Text = string.Empty;
        }


        // -- Reservering email tekst
        private void txtReserveringEmailadres_Enter(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtReserveringEmailadres.Text == "voorbeeld@mail.com")
            {
                txtReserveringEmailadres.ForeColor = Color.Black;
                txtReserveringEmailadres.Text = "";
            }
        }

        private void txtReserveringEmailadres_Leave(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtReserveringEmailadres.Text == "")
            {
                txtReserveringEmailadres.ForeColor = Color.Gray;
                txtReserveringEmailadres.Text = "voorbeeld@mail.com";
            }
        }

        // -- Reservering naam klant tekst
        private void txtReserveringNaamKlant_Enter(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtReserveringNaamKlant.Text == "Voornaam Achternaam")
            {
                txtReserveringNaamKlant.ForeColor = Color.Black;
                txtReserveringNaamKlant.Text = "";
            }
        }

        private void txtReserveringNaamKlant_Leave(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtReserveringNaamKlant.Text == "")
            {
                txtReserveringNaamKlant.ForeColor = Color.Gray;
                txtReserveringNaamKlant.Text = "Voornaam Achternaam";
            }
        }

        // -- Reservering telefoonnummer tekst
        private void txtReserveringTelefoonnummer_Enter(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtReserveringTelefoonnummer.Text == "31612345678")
            {
                txtReserveringTelefoonnummer.ForeColor = Color.Black;
                txtReserveringTelefoonnummer.Text = "";
            }
        }

        private void txtReserveringTelefoonnummer_Leave(object sender, EventArgs e) //Sander Brijer 646235
        {
            if (txtReserveringTelefoonnummer.Text == "")
            {
                txtReserveringTelefoonnummer.ForeColor = Color.Gray;
                txtReserveringTelefoonnummer.Text = "31612345678";
            }
        }

        // -- Reservering tekst
        private void ReserveringTextInvulling() //Sander Brijer 646235
        {
            rbReserveringNieuweKlant.Checked = true;
            rbReserveringBestaandeKlant.Checked = false;

            lblReserveringFoutmelding.Text = string.Empty;

            txtReserveringEmailadres.ForeColor = Color.Gray;
            txtReserveringNaamKlant.ForeColor = Color.Gray;
            txtReserveringTelefoonnummer.ForeColor = Color.Gray;
        }

        // -- Reservering wijzigen
        private void btnReserveringWijzigen_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                int geselecteerdeReservering = int.Parse(cbReserveringOverzichtSelecteerEenReservering.Text.ToString());
                foreach (Reservering reservering in reserveringen)
                {
                    if (reservering.ReserveringId == geselecteerdeReservering)
                    {
                        txtReserveringEmailadres.Enabled = false;
                        txtReserveringNaamKlant.Enabled = false;
                        txtReserveringTelefoonnummer.Enabled = false;
                        CbReserveringSelecteerTafelVullen();
                        ReserveringClear();
                        AllePanelsOnzichtbaar();
                        pnlReserveringen.Show();
                        lblReserveringFoutmelding.Text = string.Empty;
                        lblReserveringOpnemenOfWijzigen.Text = "Reservering wijzigen";

                        nudReserveringAantalPersonen.Maximum = 10;
                        nudReserveringAantalPersonen.Minimum = 1;

                        rbReserveringBestaandeKlant.Hide();
                        rbReserveringNieuweKlant.Hide();
                        lblReserveringSelecteerKlant.Hide();
                        cbReserveringSelecteerKlant.Hide();
                        Reservering reserveringTeWijzigen = reservering_Service.KrijgReserveringBijId(geselecteerdeReservering);
                        txtReserveringOpmerking.Text = reserveringTeWijzigen.ReserveringOpmerking;
                        txtReserveringEmailadres.Text = reserveringTeWijzigen.Klant.Email;
                        nudReserveringAantalPersonen.Value = reserveringTeWijzigen.AantalPersonen;
                        txtReserveringNaamKlant.Text = reserveringTeWijzigen.Klant.Naam;
                        txtReserveringTelefoonnummer.Text = reserveringTeWijzigen.Klant.Telefoonnummer;
                        cbReserveringSelecteerTafel.Text = reserveringTeWijzigen.TafelId.ToString();
                        dtpReserveringOpnemenDatumTijd.Value = reserveringTeWijzigen.AankomstDatumTijd;

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- Reservering verwijderen
        private void btnReserveringVerwijderen_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            LaatReserveringOverzichtPanel();
        }

        private void btnMenuReserveringOverzicht_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            LaatReserveringOverzichtPanel();
        }

        // -- Reservering opnemen of wijzigen annuleren
        private void btnReserveringAnnuleren_Click(object sender, EventArgs e)
        {
            lblReserveringFoutmelding.Text = string.Empty;
            LaatReserveringOverzichtPanel();
            pnlReserveringen.Hide();
        }

        // -- Reservering opnemen of wijzigen datetimepicker
        private void dateTimePickerReservering() //Sander Brijer 646235
        {
            dtpReserveringOpnemenDatumTijd.Format = DateTimePickerFormat.Custom;
            //dtpReserveringOpnemenDatumTijd.CustomFormat = "dd-MM-yyyy HH:mm";
            dtpReserveringOpnemenDatumTijd.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpReserveringOpnemenDatumTijd.MinDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
            dtpReserveringOpnemenDatumTijd.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 00, 00);
        }

        // - Panel Reserveringoverzicht
        void LaatReserveringOverzichtPanel() //Sander Brijer 646235
        {
            try
            {

                reedsGeklikt0ReserveringOverzicht = true;
                reedsGeklikt1ReserveringOverzicht = true;
                reedsGeklikt2ReserveringOverzicht = true;
                reedsGeklikt3ReserveringOverzicht = true;

                //if (pnlReserveringoverzicht.Visible == false)
                //{
                AllePanelsOnzichtbaar();
                pnlReserveringoverzicht.Show();
                //}

                cbReserveringOverzichtSelecteerEenReservering.Items.Clear();
                lblReserveringoverzichtTijdNu.Text = "Tijd nu: " + DateTime.Now.ToString("HH:mm");

                reserveringen = reservering_Service.AllesOphalen();
                foreach (Reservering reservering in reserveringen)
                {
                    cbReserveringOverzichtSelecteerEenReservering.Items.Add(reservering.ReserveringId.ToString());
                }
                vulReserveringenListview(reserveringen);
                if (cbReserveringOverzichtSelecteerEenReservering.Items.Count > 0)
                    cbReserveringOverzichtSelecteerEenReservering.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void lstViewReserveringOverzicht_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                List<Reservering> reserveringenTemp = reserveringen.ToList();
                switch (e.Column)
                {
                    case 0:
                        {
                            if (reedsGeklikt0ReserveringOverzicht == false)
                            {
                                reserveringenTemp = reserveringenTemp.OrderBy(reservering => reservering.ReserveringId).ToList();
                            }
                            else
                            {
                                reserveringenTemp.Reverse(0, reserveringenTemp.Count);
                            }
                            reedsGeklikt0ReserveringOverzicht = !reedsGeklikt0ReserveringOverzicht;
                            break;
                        }
                    case 1:
                        {
                            if (reedsGeklikt1ReserveringOverzicht == false)
                            {
                                reserveringenTemp = reserveringenTemp.OrderBy(reservering => reservering.TafelId).ToList();
                            }
                            else
                            {
                                reserveringenTemp.Reverse(0, reserveringenTemp.Count);
                            }
                            reedsGeklikt1ReserveringOverzicht = !reedsGeklikt1ReserveringOverzicht;
                            break;
                        }
                    case 2:
                        {
                            if (reedsGeklikt2ReserveringOverzicht == false)
                            {
                                reserveringenTemp = reserveringenTemp.OrderBy(reservering => reservering.AankomstDatumTijd.Day).ToList();
                            }
                            else
                            {
                                reserveringenTemp.Reverse(0, reserveringenTemp.Count);
                            }
                            reedsGeklikt2ReserveringOverzicht = !reedsGeklikt2ReserveringOverzicht;
                            break;
                        }
                    case 3:
                        {
                            if (reedsGeklikt3ReserveringOverzicht == false)
                            {
                                reserveringenTemp = reserveringenTemp.OrderBy(reservering => reservering.Klant.Naam).ToList();
                            }
                            else
                            {
                                reserveringenTemp.Reverse(0, reserveringenTemp.Count);
                            }
                            reedsGeklikt3ReserveringOverzicht = !reedsGeklikt3ReserveringOverzicht;
                            break;
                        }
                }
                vulReserveringenListview(reserveringenTemp);
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        public void vulReserveringenListview(List<Reservering> reserveringen)
        {
            try
            {
                lstViewReserveringOverzicht.Items.Clear();
                foreach (Reservering reservering in reserveringen)
                {
                    ListViewItem item = new ListViewItem(reservering.ReserveringId.ToString());
                    item.SubItems.Add(reservering.TafelId.ToString());
                    item.SubItems.Add(reservering.AankomstDatumTijd.ToString());
                    item.SubItems.Add(reservering.Klant.Naam.ToString());
                    lstViewReserveringOverzicht.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // reservering aanmelden
        private void btnReserveringOverzichtReserveringAanmelden_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                Reservering reservering = reservering_Service.KrijgReserveringBijId(int.Parse(cbReserveringOverzichtSelecteerEenReservering.SelectedItem.ToString()));
                //reservering verwijderen
                ReserveringOverzichtReserveringVerwijderen();
                //tafel status aanpassen naar bezet.
                tafel_Service.BewerkenTafelStatus(reservering.TafelId, StatusTafel.Bezet);
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- Reservering selecteer een tafel vullen
        private void CbReserveringSelecteerTafelVullen()
        {
            cbReserveringSelecteerTafel.Items.Clear();
            cbReserveringSelecteerTafel.Items.Add("Willekeurig");
            for (int i = 1; i <= 10; i++)
            {
                cbReserveringSelecteerTafel.Items.Add(i);
            }
        }

        private void btnReserveringOverzichtVerversen_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            LaatReserveringOverzichtPanel();
        }


        // -- Reservering verwijderen
        private void btnReserveringOverzichtReserveringVerwijderen_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            ReserveringOverzichtReserveringVerwijderen();
        }

        private void ReserveringOverzichtReserveringVerwijderen()
        {
            try
            {
                int reserveringId = int.Parse(cbReserveringOverzichtSelecteerEenReservering.Text);
                reservering_Service.VerwijderReservering(reserveringId);
                lblReserveringOverzichtFoutmelding.Text = $"⚠ Verwijderen van reservering {reserveringId} succesvol.";
            }
            catch
            {
                lblReserveringOverzichtFoutmelding.Text = $"⚠ Verwijderen van reservering {cbReserveringOverzichtSelecteerEenReservering.Text} niet gelukt.";
            }
            finally
            {
                LaatReserveringOverzichtPanel();
            }
        }


        // - Uitlog methoden
        private void btnUitloggen_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            Uitloggen();
        }

        private void Uitloggen() //Sander Brijer 646235
        {
            //Reset ingelogde medewerker
            IngelogdeMedewerker = null;
            //Uitloggen
            this.Ingelogd = false;
            //Herstart programma
            Program();
        }


        public void Catch(Exception exception) //Sander Brijer 646235
        {
            MessageBox.Show(exception.Message.ToString(), "Foutmelding", MessageBoxButtons.OK);
            //Log log = new Log()
            //{
            //    Date = DateTime.Now,
            //    Message = exception.Message,
            //    Source = exception.Source,
            //    Method = exception.TargetSite.Name,
            //    Fullname = exception.TargetSite.DeclaringType.FullName.ToString(),
            //};
            //logList.Add(log);
        }

        // - Panel te serveren overzicht
        private void btnMenuTeServerenOverzicht_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                AllePanelsOnzichtbaar();
                pnlTeServerenOverzicht.Show();
                LaatTeServerenOverzicht();
                lblTeServerenOverzichtBestelling.MaximumSize = new Size(400, 0);
                lblTeServerenOverzichtBestelling.AutoSize = true;
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- Panel inladen 
        private void LaatTeServerenOverzicht() //Sander Brijer 646235
        {
            try
            {
                reedsGeklikt0TeServeren = false;
                reedsGeklikt1TeServeren = false;
                reedsGeklikt2TeServeren = false;
                reedsGeklikt3TeServeren = false;
                lblTeServerenOverzichtBestelling.Text = string.Empty;
                lblTeServerenOverzichtTijd.Text = string.Empty;

                bestellingenKlaar = bestelling_Service.KrijgAlleBestellingenStatusKlaar();
                cbTeServerenOverzichtSelecteerEenBestelling.Items.Clear();
                cbTeServerenOverzichtFilterOpTafel.Items.Clear();
                lstvTeServerenOverzichtOverzicht.Items.Clear();
                VulTeServerenListView(bestellingenKlaar);
                for (int i = 0; i < bestellingenKlaar.Count; i++)
                {
                    cbTeServerenOverzichtSelecteerEenBestelling.Items.Add(bestellingenKlaar[i].BestellingId);
                }
                tafels = tafel_Service.AllesOphalen();
                cbTeServerenOverzichtFilterOpTafel.Items.Add("Alles");
                for (int i = 1; i <= tafels.Count; i++)
                {
                    cbTeServerenOverzichtFilterOpTafel.Items.Add(i);
                }
                if (tafels.Count > 0)
                    cbTeServerenOverzichtFilterOpTafel.SelectedIndex = 0;
                else
                    cbTeServerenOverzichtFilterOpTafel.SelectedIndex = -1;

                if (bestellingenKlaar.Count > 0)
                    cbTeServerenOverzichtSelecteerEenBestelling.SelectedIndex = 0;
                else
                    cbTeServerenOverzichtSelecteerEenBestelling.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- Status wijzigen
        private void TeServerenOverzichtStatusWijzigen(StatusBestelling status) //Sander Brijer 646235
        {
            try
            {
                int geselecteerdeBestelling = int.Parse(cbTeServerenOverzichtSelecteerEenBestelling.SelectedItem.ToString());
                if (geselecteerdeBestelling > 0)
                {
                    bestelling_Service.VeranderStatus(geselecteerdeBestelling, status);
                    LaatTeServerenOverzicht();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Geen bestelling geselecteerd", "Foutmelding", MessageBoxButtons.OK);
            }
        }
        // --- Status naar 'Open'
        private void btnTeServerenOverzichtTerugMelden_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                TeServerenOverzichtStatusWijzigen(StatusBestelling.Open);
                LaatTeServerenOverzicht();
                lblTeServerenOverzichtTijd.Text = lblTeServerenOverzichtTijd.Text + "✔ Status veranderd.";
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // --- Status naar 'Gearchiveerd'
        private void btnTeServerenOverzichtGeserveerdMelden_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                TeServerenOverzichtStatusWijzigen(StatusBestelling.Gearchiveerd);
                LaatTeServerenOverzicht();
                lblTeServerenOverzichtTijd.Text = lblTeServerenOverzichtTijd.Text + "✔ Status veranderd.";
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- Filter op tafel
        private void cbTeServerenOverzichtFilterOpTafel_SelectedIndexChanged(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {
                int geselecteerdeTafel = cbTeServerenOverzichtFilterOpTafel.SelectedIndex;
                lstvTeServerenOverzichtOverzicht.Items.Clear();
                foreach (Bestelling bestelling in bestellingenKlaar)
                {
                    if (geselecteerdeTafel == 0 || bestelling.TafelId == geselecteerdeTafel)
                    {
                        ListViewItem item = new ListViewItem(bestelling.BestellingId.ToString());
                        item.SubItems.Add(bestelling.TafelId.ToString());
                        item.SubItems.Add(bestelling.Medewerker.VolledigeNaam.ToString());
                        string bestellingDetails = string.Empty;
                        for (int i = 0; i < bestelling.BesteldeItems.Count; i++)
                        {
                            if (i == 0)
                                bestellingDetails = $"{bestelling.BesteldeItems[i]}";
                            else
                                bestellingDetails = $"{bestellingDetails}, {bestelling.BesteldeItems[i]}";
                        }
                        item.SubItems.Add(bestellingDetails);
                        lstvTeServerenOverzichtOverzicht.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- verversen
        private void btnTeServerenOverzichtVerversen_Click(object sender, EventArgs e) //Sander Brijer 646235
        {
            LaatTeServerenOverzicht();
        }

        // -- selecteer op bestelling
        private void cbTeServerenOverzichtSelecteerEenBestelling_SelectedIndexChanged(object sender, EventArgs e) //Sander Brijer 646235
        {
            try
            {

                int bestelling = cbTeServerenOverzichtSelecteerEenBestelling.SelectedIndex;
                string bestellingDetails = "";
                if (bestelling > -1)
                {

                    for (int i = 0; i < bestellingenKlaar[bestelling].BesteldeItems.Count; i++)
                    {
                        if (i == 0)
                            bestellingDetails = $"{bestellingenKlaar[bestelling].BesteldeItems[i]}";
                        else
                            bestellingDetails = $"{bestellingDetails}, {bestellingenKlaar[bestelling].BesteldeItems[i]}";
                    }

                    lblTeServerenOverzichtBestelling.Text = bestellingDetails;
                    lblTeServerenOverzichtBestellingOpmerking.Text = bestellingenKlaar[bestelling].BestellingOpmerking;
                    lblTeServerenOverzichtTijd.Text = bestellingenKlaar[bestelling].TijdBestelling.ToString();
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }




        // -- Sorteren te reserveren overzicht
        private void lstvTeServerenOverzichtOverzicht_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                List<Bestelling> bestellingenKlaarTemp = bestellingenKlaar.ToList();
                switch (e.Column)
                {
                    case 0:
                        {
                            if (reedsGeklikt0TeServeren == false)
                            {
                                bestellingenKlaarTemp = bestellingenKlaarTemp.OrderBy(bestelling => bestelling.BestellingId).ToList();
                            }
                            else
                            {
                                bestellingenKlaarTemp.Reverse(0, bestellingenKlaarTemp.Count);
                            }
                            reedsGeklikt0TeServeren = !reedsGeklikt0TeServeren;
                            break;
                        }
                    case 1:
                        {
                            if (reedsGeklikt1TeServeren == false)
                            {
                                bestellingenKlaarTemp = bestellingenKlaarTemp.OrderBy(bestelling => bestelling.TafelId).ToList();
                            }
                            else
                            {
                                bestellingenKlaarTemp.Reverse(0, bestellingenKlaarTemp.Count);
                            }
                            reedsGeklikt1TeServeren = !reedsGeklikt1TeServeren;
                            break;
                        }
                    case 2:
                        {
                            if (reedsGeklikt2TeServeren == false)
                            {
                                bestellingenKlaarTemp = bestellingenKlaarTemp.OrderBy(bestelling => bestelling.Medewerker.VolledigeNaam).ToList();
                            }
                            else
                            {
                                bestellingenKlaarTemp.Reverse(0, bestellingenKlaarTemp.Count);
                            }
                            reedsGeklikt2TeServeren = !reedsGeklikt2TeServeren;
                            break;
                        }
                    case 3:
                        {
                            if (reedsGeklikt3TeServeren == false)
                            {
                                bestellingenKlaarTemp = bestellingenKlaarTemp.OrderBy(bestelling => bestelling.BesteldeItems.Count).ToList();
                            }
                            else
                            {
                                bestellingenKlaarTemp.Reverse(0, bestellingenKlaarTemp.Count);
                            }
                            reedsGeklikt3TeServeren = !reedsGeklikt3TeServeren;
                            break;
                        }
                }
                VulTeServerenListView(bestellingenKlaarTemp);
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        // -- list view vullen
        private void VulTeServerenListView(List<Bestelling> bestellingen)
        {
            try
            {
                lstvTeServerenOverzichtOverzicht.Items.Clear();
                foreach (Bestelling bestelling in bestellingen)
                {
                    ListViewItem item = new ListViewItem(bestelling.BestellingId.ToString());
                    item.SubItems.Add(bestelling.TafelId.ToString());
                    item.SubItems.Add(bestelling.Medewerker.VolledigeNaam.ToString());
                    string bestellingDetails = string.Empty;
                    for (int i = 0; i < bestelling.BesteldeItems.Count; i++)
                    {
                        if (i == 0)
                            bestellingDetails = $"{bestelling.BesteldeItems[i]}";
                        else
                            bestellingDetails = $"{bestellingDetails}, {bestelling.BesteldeItems[i]}";
                    }
                    item.SubItems.Add(bestellingDetails);
                    item.SubItems.Add(bestelling.BestellingOpmerking.ToString());
                    lstvTeServerenOverzichtOverzicht.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        //GEDEELTE GEMAAKT DOOR JOHN BOND

        // Kijkt naar de status van de bestelling en verandert daarmee de tekst op de statusknop (Keuken)
        private void listViewBestellingenKeuken_SelectedIndexChanged(object sender, EventArgs e) // John Bond 649770
        {
            try
            {
                string statusBestelling;
                int bestellingId;
                if (listViewBestellingenKeuken.SelectedIndices.Count > 0)
                {
                    bestellingId = int.Parse(listViewBestellingenKeuken.SelectedItems[0].SubItems[0].Text);
                    lbl_MomenteelGeselecteerdKeuken.Text = $"Momenteel geselecteerd\nBestelling {bestellingId}";
                    statusBestelling = listViewBestellingenKeuken.SelectedItems[0].SubItems[3].Text;
                    if (statusBestelling == "Open")
                    {
                        btn_KeukenVeranderStatus.Text = "Verander status naar Bezet";
                    }
                    else if (statusBestelling == "Bezet")
                    {
                        btn_KeukenVeranderStatus.Text = "Bestelling gereed melden";
                    }
                    else if (statusBestelling == "Klaar")
                    {
                        btn_KeukenVeranderStatus.Text = "Zet bestelling terug";
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }

        }
        // Kijkt naar de status van de bestelling en verandert daarmee de tekst op de statusknop (Bar)
        private void listViewBestellingenBar_SelectedIndexChanged(object sender, EventArgs e) // John Bond 649770
        {
            try
            {
                string statusBestelling;
                int bestellingId;
                if (listViewBestellingenBar.SelectedIndices.Count > 0)
                {
                    bestellingId = int.Parse(listViewBestellingenBar.SelectedItems[0].SubItems[0].Text);
                    lbl_MomenteelGeselecteerdBar.Text = $"Momenteel geselecteerd\nBestelling {bestellingId}";
                    statusBestelling = listViewBestellingenBar.SelectedItems[0].SubItems[3].Text;
                    if (statusBestelling == "Open")
                    {
                        btn_BarmanVeranderStatus.Text = "Verander status naar Bezet";
                    }
                    else if (statusBestelling == "Bezet")
                    {
                        btn_BarmanVeranderStatus.Text = "Bestelling gereed melden";
                    }
                    else if (statusBestelling == "Klaar")
                    {
                        btn_BarmanVeranderStatus.Text = "Zet bestelling terug";
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Roept de methode op om de status van een barbestelling te veranderen
        private void btn_BarmanVeranderStatus_Click(object sender, EventArgs e) //John Bond 649770
        {
            try
            {
                BarVeranderStatus();
            }
            catch
            {
                MessageBox.Show("ERROR: Er moet eerst bestelling worden geselecteerd!");
            }
        }
        // Laat een tabel zien met alle bestellingen die op 'klaar' staan (bar)
        private void btn_BarmanGeschiedenisBestellingen_Click(object sender, EventArgs e) // John Bond 649770
        {
            try
            {

                listViewBestellingenBar.Items.Clear();
                Bestelling_Service bestelling_Service = new Bestelling_Service();
                List<Bestelling> bestellingenBar = bestelling_Service.KrijgAlleBestellingenBarGeschiedenis();
                foreach (Bestelling bestelling in bestellingenBar)
                {
                    ListViewItem item = new ListViewItem(bestelling.BestellingId.ToString());
                    item.SubItems.Add(bestelling.TafelId.ToString());
                    item.SubItems.Add(bestelling.TijdBestelling.ToString("HH:mm"));
                    item.SubItems.Add(bestelling.Status.ToString());
                    listViewBestellingenBar.Items.Add(item);
                }
                btn_BarmanGeschiedenisTerug.Show();
                btn_BarmanGeschiedenisBestellingen.Hide();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btn_BarmanOverzichtVerniew_Click(object sender, EventArgs e) // John Bond 649770
        {
            OverzichtBar();
        }

        private void btn_BarmanTerugNaarBestellingen_Click(object sender, EventArgs e) // John Bond 649770
        {
            AllePanelsOnzichtbaar();
            pnlBarmanBesteloverzicht.Show();
        }
        // Vernieuwd de voorraad van de bar
        private void btn_BarmanVoorraadVernieuw_Click(object sender, EventArgs e) // John Bond 649770
        {
            try
            {
                OverzichtVoorraadBar();
                VoorraadBarKleurcode();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Vernieuwd de voorrraad van de keuken
        private void btn_KeukenVernieuwVoorraad_Click(object sender, EventArgs e)
        {
            try
            {
                OverzichtVoorraadKeuken();
                VoorraadKeukenKleurcode();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        private void btn_KeukenGeschiedenisTerug_Click(object sender, EventArgs e) // John Bond 649770
        {
            try
            {
                OverzichtKeuken();
                btn_KeukenGeschiedenisBestellingen.Show();
                btn_KeukenGeschiedenisTerug.Hide();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btn_BarmanGeschiedenisTerug_Click(object sender, EventArgs e) // John Bond 649770
        {
            try
            {
                OverzichtBar();
                btn_BarmanGeschiedenisBestellingen.Show();
                btn_BarmanGeschiedenisTerug.Hide();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Roept de methode op om de status van een keukenbestelling te veranderen
        private void btn_KeukenVeranderStatus_Click(object sender, EventArgs e) // John Bond 649770
        {
            try
            {
                KeukenVeranderStatus();
            }
            catch
            {
                MessageBox.Show("ERROR: Er moet eerst bestelling worden geselecteerd!");
            }
        }
        // Laat een tabel zien met alle bestellingen die op 'klaar' staan (keuken)
        private void btn_KeukenGeschiedenisBestellingen_Click(object sender, EventArgs e) // John Bond 649770
        {
            try
            {
                listViewBestellingenKeuken.Items.Clear();
                Bestelling_Service bestelling_Service = new Bestelling_Service();
                List<Bestelling> bestellingenKeuken = bestelling_Service.KrijgAlleBestellingenKeukenGeschiedenis();
                foreach (Bestelling bestelling in bestellingenKeuken)
                {
                    ListViewItem item = new ListViewItem(bestelling.BestellingId.ToString());
                    item.SubItems.Add(bestelling.TafelId.ToString());
                    item.SubItems.Add(bestelling.TijdBestelling.ToString("HH:mm"));
                    item.SubItems.Add(bestelling.Status.ToString());
                    listViewBestellingenKeuken.Items.Add(item);
                }
                btn_KeukenGeschiedenisTerug.Show();
                btn_KeukenGeschiedenisBestellingen.Hide();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Vernieuwd het besteloverzicht van de keuken
        private void btn_KeukenOverzichtVernieuw_Click(object sender, EventArgs e) // John Bond 649770
        {
            OverzichtKeuken();
        }
        // ontvangt data gebasseerd op de geselecteerde bestelling en roept daarmee de details methode aan (keuken)
        private void btnKeukenBekijkDetails_Click(object sender, EventArgs e) // John Bond 649770
        {
            try
            {
                string selectedBestellingId = listViewBestellingenKeuken.SelectedItems[0].SubItems[0].Text;
                AllePanelsOnzichtbaar();
                pnlKeukenBestellingDetails.Show();
                KeukenBestellingDetails(selectedBestellingId);
            }
            catch
            {
                MessageBox.Show("ERROR: Er moet eerst bestelling worden geselecteerd!");
            }
        }
        // ontvangt data gebasseerd op de geselecteerde bestelling en roept daarmee de details methode aan (bar)
        private void btnBarmanBekijkDetails_Click(object sender, EventArgs e) //John Bond 649770
        {
            try
            {
                string selectedBestellingId = listViewBestellingenBar.SelectedItems[0].SubItems[0].Text;
                AllePanelsOnzichtbaar();
                pnlBarmanBestellingDetails.Show();
                BarmanBestellingDetails(selectedBestellingId);
            }
            catch
            {
                MessageBox.Show("ERROR: Er moet eerst bestelling worden geselecteerd!");
            }
        }

        private void btnKeukenMenu_Click(object sender, EventArgs e) //John Bond 649770
        {
            //maak het een computerscherm
            ComputerScherm();
            AllePanelsOnzichtbaar();
            pnlKeukenMenu.Show();
        }
        private void btn_BarMenuNaarBestellingen_Click(object sender, EventArgs e) //John Bond 649770
        {
            OpenBarBestellingen();
        }

        private void btn_BarMenuNaarVoorraad_Click(object sender, EventArgs e) //John Bond 649770
        {
            OpenBarVoorraad();
        }

        private void btn_KeukenMenuNaarBestellingen_Click(object sender, EventArgs e) //John Bond 649770
        {
            OpenKeukenBestellingen();
        }

        private void btn_KeukenMenuNaarVoorraad_Click(object sender, EventArgs e) //John Bond 649770
        {
            OpenKeukenVoorraad();
        }
        private void btn_KeukenDetailsNaarKeukenBestelling_Click(object sender, EventArgs e) //John Bond 649770
        {
            AllePanelsOnzichtbaar();
            pnlKeukenBesteloverzicht.Show();
        }
        // Verandert de status in het detail panel (keuken)
        private void btn_KeukenVeranderStatusDetails_Click(object sender, EventArgs e) // John Bond 649770
        {
            KeukenVeranderStatus();
            MessageBox.Show($"Status is veranderd");
        }
        // Verandert de status in het detail panel (bar)
        private void btn_BarmanVeranderStatusDetails_Click(object sender, EventArgs e) // John Bond 649770
        {
            BarVeranderStatus();
            MessageBox.Show("Status is veranderd");
        }

        // HIER ALLE METHODES VAN JOHN
        // Zorgt voor het besteloverzicht van de bar
        private void OverzichtBar() //John Bond 649770
        {
            try
            {

                listViewBestellingenBar.Items.Clear();
                Bestelling_Service bestelling_Service = new Bestelling_Service();
                List<Bestelling> bestellingenBar = bestelling_Service.KrijgAlleBestellingenBar();
                foreach (Bestelling bestelling in bestellingenBar)
                {
                    ListViewItem item = new ListViewItem(bestelling.BestellingId.ToString());
                    item.SubItems.Add(bestelling.TafelId.ToString());
                    item.SubItems.Add(bestelling.TijdBestelling.ToString("HH:mm"));
                    item.SubItems.Add(bestelling.Status.ToString());
                    listViewBestellingenBar.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Zorgt voor de voorraad van de bar
        private void OverzichtVoorraadBar() //John Bond 649770
        {
            try
            {
                listViewVoorraadBar.Items.Clear();
                Voorraad_Service voorraad_Service = new Voorraad_Service();
                List<Voorraad> voorraadBar = voorraad_Service.KrijgAllesVoorraadBar();
                foreach (Voorraad drank in voorraadBar)
                {
                    ListViewItem item = new ListViewItem(drank.VoorraadId.ToString());
                    item.SubItems.Add(drank.Naam.ToString());
                    item.SubItems.Add(drank.VoorraadAantal.ToString());
                    listViewVoorraadBar.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Zorgt voor de voorraad van de keuken
        private void OverzichtVoorraadKeuken() //John Bond 649770
        {
            try
            {
                listViewVoorraadKeuken.Items.Clear();
                Voorraad_Service voorraad_Service = new Voorraad_Service();
                List<Voorraad> voorraadKeuken = voorraad_Service.KrijgAllesVoorraadKeuken();
                foreach (Voorraad gerecht in voorraadKeuken)
                {
                    ListViewItem item = new ListViewItem(gerecht.VoorraadId.ToString());
                    item.SubItems.Add(gerecht.Naam.ToString());
                    item.SubItems.Add(gerecht.VoorraadAantal.ToString());
                    listViewVoorraadKeuken.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Zorgt voor de kleuren van de voorraad van de bar
        private void VoorraadBarKleurcode() // John Bond 649770
        {
            try
            {
                foreach (ListViewItem lvw in listViewVoorraadBar.Items)
                {
                    lvw.UseItemStyleForSubItems = false;

                    for (int i = 0; i < listViewVoorraadBar.Columns.Count; i++)
                    {
                        if (int.Parse(lvw.SubItems[2].Text.ToString()) == 0)
                        {
                            lvw.SubItems[i].BackColor = Color.Red;
                            lvw.SubItems[i].ForeColor = Color.White;
                        }
                        else if (int.Parse(lvw.SubItems[2].Text.ToString()) <= 5)
                        {
                            lvw.SubItems[i].BackColor = Color.Orange;
                            lvw.SubItems[i].ForeColor = Color.Black;
                        }
                        else if (int.Parse(lvw.SubItems[2].Text.ToString()) <= 15)
                        {
                            lvw.SubItems[i].BackColor = Color.Yellow;
                            lvw.SubItems[i].ForeColor = Color.Black;
                        }
                        else
                        {
                            lvw.SubItems[i].BackColor = Color.Green;
                            lvw.SubItems[i].ForeColor = Color.White;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Zorgt voor de kleuren van de voorraad van de keuken
        private void VoorraadKeukenKleurcode() // John Bond 649770
        {
            try
            {
                foreach (ListViewItem lvw in listViewVoorraadKeuken.Items)
                {
                    lvw.UseItemStyleForSubItems = false;

                    for (int i = 0; i < listViewVoorraadKeuken.Columns.Count; i++)
                    {
                        if (int.Parse(lvw.SubItems[2].Text.ToString()) == 0)
                        {
                            lvw.SubItems[i].BackColor = Color.Red;
                            lvw.SubItems[i].ForeColor = Color.White;
                        }
                        else if (int.Parse(lvw.SubItems[2].Text.ToString()) <= 5)
                        {
                            lvw.SubItems[i].BackColor = Color.Orange;
                            lvw.SubItems[i].ForeColor = Color.Black;
                        }
                        else if (int.Parse(lvw.SubItems[2].Text.ToString()) <= 15)
                        {
                            lvw.SubItems[i].BackColor = Color.Yellow;
                            lvw.SubItems[i].ForeColor = Color.Black;
                        }
                        else
                        {
                            lvw.SubItems[i].BackColor = Color.Green;
                            lvw.SubItems[i].ForeColor = Color.White;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Zorgt voor het besteloverzicht van de keuken
        private void OverzichtKeuken() //John Bond 649770
        {
            try
            {
                listViewBestellingenKeuken.Items.Clear();
                Bestelling_Service bestelling_Service = new Bestelling_Service();
                List<Bestelling> bestellingenKeuken = bestelling_Service.KrijgAlleBestellingenKeuken();
                foreach (Bestelling bestelling in bestellingenKeuken)
                {
                    ListViewItem item = new ListViewItem(bestelling.BestellingId.ToString());
                    item.SubItems.Add(bestelling.TafelId.ToString());
                    item.SubItems.Add(bestelling.TijdBestelling.ToString("HH:mm"));
                    item.SubItems.Add(bestelling.Status.ToString());
                    listViewBestellingenKeuken.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }

        }

        private void OpenBarVoorraad() // John Bond 649770
        {
            AllePanelsOnzichtbaar();
            pnlBarmanVoorraad.Show();
            OverzichtVoorraadBar();
            VoorraadBarKleurcode();
        }
        private void OpenBarBestellingen() // John Bond 649770
        {
            AllePanelsOnzichtbaar();
            pnlBarmanBesteloverzicht.Show();
            btn_BarmanGeschiedenisTerug.Hide();
            btn_BarmanGeschiedenisBestellingen.Show();
            OverzichtBar();
        }
        private void OpenKeukenVoorraad() // John Bond 649770
        {
            AllePanelsOnzichtbaar();
            pnlKeukenVoorraad.Show();
            OverzichtVoorraadKeuken();
            VoorraadKeukenKleurcode();
        }
        private void OpenKeukenBestellingen() // John Bond 649770
        {
            AllePanelsOnzichtbaar();
            pnlKeukenBesteloverzicht.Show();
            btn_KeukenGeschiedenisBestellingen.Show();
            btn_KeukenGeschiedenisTerug.Hide();
            OverzichtKeuken();
        }
        // Zorgt voor de details van de geselecteerde keukenbestelling
        private void KeukenBestellingDetails(string selectedBestellingId) //John Bond 649770
        {
            try
            {
                lbl_KeukenDetailsBestelling.Text = $"Bestelling {selectedBestellingId} details";
                listViewKeukenDetails.Items.Clear();
                BesteldeItem_Service besteldeItem_Service = new BesteldeItem_Service();
                List<BesteldeItem> bestellingen = besteldeItem_Service.KrijgAlleDetailsKeuken(selectedBestellingId);
                foreach (BesteldeItem details in bestellingen)
                {
                    ListViewItem item = new ListViewItem(details.Naam.ToString());
                    item.SubItems.Add(details.Aantal.ToString());
                    item.SubItems.Add(details.BestellingOpmerking.ToString());
                    listViewKeukenDetails.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Zorgt voor de details van de geselecteerde barbestelling
        private void BarmanBestellingDetails(string selectedBestellingId) //John Bond 649770
        {
            try
            {
                lbl_BarmanDetailsBestelling.Text = $"Bestelling {selectedBestellingId} details";
                ListViewBarmanDetails.Items.Clear();
                BesteldeItem_Service besteldeItem_Service = new BesteldeItem_Service();
                List<BesteldeItem> bestellingen = besteldeItem_Service.KrijgAlleDetailsBarman(selectedBestellingId);
                foreach (BesteldeItem details in bestellingen)
                {
                    ListViewItem item = new ListViewItem(details.Naam.ToString());
                    item.SubItems.Add(details.Aantal.ToString());
                    item.SubItems.Add(details.BestellingOpmerking.ToString());
                    ListViewBarmanDetails.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Verandert de status van de geselecteerde keukenbestelling
        private void KeukenVeranderStatus() // John Bond 649770
        {
            try
            {
                int selectedBestellingId = int.Parse(listViewBestellingenKeuken.SelectedItems[0].SubItems[0].Text);
                string statusBestelling = listViewBestellingenKeuken.SelectedItems[0].SubItems[3].Text;
                StatusBestelling status = new StatusBestelling();
                if (statusBestelling == "Open")
                {
                    status = StatusBestelling.Open;
                }
                else if (statusBestelling == "Bezet")
                {
                    status = StatusBestelling.Bezet;
                }
                else if (statusBestelling == "Klaar")
                {
                    status = StatusBestelling.Klaar;
                    btn_KeukenGeschiedenisBestellingen.Show();
                    btn_KeukenGeschiedenisTerug.Hide();
                }
                Bestelling_Service bestelling_Service = new Bestelling_Service();
                bestelling_Service.VeranderStatus(selectedBestellingId, status);
                OverzichtKeuken();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        // Verandert de status van de geselecteerde barbestelling
        private void BarVeranderStatus() // John Bond 649770
        {
            try
            {
                int selectedBestellingId = int.Parse(listViewBestellingenBar.SelectedItems[0].SubItems[0].Text);
                string statusBestelling = listViewBestellingenBar.SelectedItems[0].SubItems[3].Text;
                StatusBestelling status = new StatusBestelling();
                if (statusBestelling == "Open")
                {
                    status = StatusBestelling.Open;
                }
                else if (statusBestelling == "Bezet")
                {
                    status = StatusBestelling.Bezet;
                }
                else if (statusBestelling == "Klaar")
                {
                    status = StatusBestelling.Klaar;
                    btn_BarmanGeschiedenisBestellingen.Show();
                    btn_BarmanGeschiedenisTerug.Hide();
                }
                Bestelling_Service bestelling_Service = new Bestelling_Service();
                bestelling_Service.VeranderStatus(selectedBestellingId, status);
                OverzichtBar();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }


        //Bestelling gedeelte: taak van Louella Creemers 641347

        //BESTELLINGOVERZICHT (Sander + Louella)

        private void btnBestellingsoverzicht_Click(object sender, EventArgs e) //Sander Brijer 646235 + Louella Creemers 641347
        {
            try
            {
                AllePanelsOnzichtbaar();
                pnlBestellingOverzicht.Show();
                LaadBestellingOverzicht();
                lbBestellingBO.MaximumSize = new Size(600, 0);
                lbBestellingBO.AutoSize = true;
                pbChapooLogo.BringToFront();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btnBestellingoverzichtVerversen_Click(object sender, EventArgs e)
        {
            LaadBestellingOverzicht();
        }

        private void LaadBestellingOverzicht() //Sander Brijer 646235 + Louella Creemers 641347
        {
            try
            {
                lbBestellingBO.Text = string.Empty;
                lbTijdBO.Text = string.Empty;

                bestellingenOpen = bestelling_Service.KrijgAlleBestellingenStatusOpen();

                cbIdBestellingOverzicht.Items.Clear();
                cbTafelBestellingOverzicht.Items.Clear();
                lvBestellingOverzicht.Items.Clear();

                VulBestellingOverzichtListView(bestellingenOpen);
                for (int i = 0; i < bestellingenOpen.Count; i++)
                {
                    cbIdBestellingOverzicht.Items.Add(bestellingenOpen[i].BestellingId);
                }
                tafels = tafel_Service.AllesOphalen();
                cbTafelBestellingOverzicht.Items.Add("Alles");
                for (int i = 1; i <= tafels.Count; i++)
                {
                    cbTafelBestellingOverzicht.Items.Add(i);
                }
                if (tafels.Count > 0)
                    cbTafelBestellingOverzicht.SelectedIndex = 0;
                else
                    cbTafelBestellingOverzicht.SelectedIndex = -1;

                if (bestellingenOpen.Count > 0)
                {
                    cbIdBestellingOverzicht.SelectedIndex = 0;
                }
                else
                {
                    cbIdBestellingOverzicht.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void cbTafelBestellingOverzicht_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int geselecteerdeTafel = cbTafelBestellingOverzicht.SelectedIndex;
                lvBestellingOverzicht.Items.Clear();
                foreach (Bestelling bestelling in bestellingenOpen)
                {
                    if (geselecteerdeTafel == 0 || bestelling.TafelId == geselecteerdeTafel)
                    {
                        ListViewItem item = new ListViewItem(bestelling.BestellingId.ToString());
                        item.SubItems.Add(bestelling.TafelId.ToString());
                        item.SubItems.Add(bestelling.Medewerker.VolledigeNaam);
                        string bestellingDetails = string.Empty;
                        for (int i = 0; i < bestelling.BesteldeItems.Count; i++)
                        {
                            if (i == 0)
                                bestellingDetails = $"{bestelling.BesteldeItems[i]}";
                            else
                                bestellingDetails = $"{bestellingDetails}, {bestelling.BesteldeItems[i]}";
                        }
                        item.SubItems.Add(bestellingDetails);
                        lvBestellingOverzicht.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }


        private void VulBestellingOverzichtListView(List<Bestelling> bestellingen) //Sander Brijer 646235 + Louella Creemers 641347
        {
            try
            {
                lvBestellingOverzicht.Items.Clear();
                foreach (Bestelling bestelling in bestellingen)
                {
                    ListViewItem item = new ListViewItem(bestelling.BestellingId.ToString());
                    item.SubItems.Add(bestelling.TafelId.ToString());
                    item.SubItems.Add(bestelling.Medewerker.VolledigeNaam);
                    string bestellingDetails = string.Empty;

                    for (int i = 0; i < bestelling.BesteldeItems.Count; i++)
                    {
                        if (i == 0)
                            bestellingDetails = $"{bestelling.BesteldeItems[i]}";
                        else
                            bestellingDetails = $"{bestellingDetails}, {bestelling.BesteldeItems[i]}";
                    }

                    item.SubItems.Add(bestellingDetails);
                    item.SubItems.Add(bestelling.BestellingOpmerking);
                    lvBestellingOverzicht.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
        private void btnBestellingVerwijderen_Click(object sender, EventArgs e) //Louëlla Creemers 641347 voor het verwijderen van een bestelling
        {
            try
            {
                if (cbIdBestellingOverzicht.Text == string.Empty)
                {
                    MessageBox.Show("Selecteer eerst een Bestelling", "Fout", MessageBoxButtons.OK);
                }

                else
                {
                    int bestellingId = int.Parse(cbIdBestellingOverzicht.Text);

                    bestelling_Service.VerwijderBestelling(bestellingId);

                    LaadBestellingOverzicht();
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void cbIdBestellingOverzicht_SelectedIndexChanged(object sender, EventArgs e)  //Sander Brijer 646235 + Louella Creemers 641347
        {
            try
            {
                int bestelling = cbIdBestellingOverzicht.SelectedIndex;

                if (bestelling > -1)
                {
                    string bestellingDetails = "";
                    for (int i = 0; i < bestellingenOpen[bestelling].BesteldeItems.Count; i++)
                    {
                        if (i == 0)
                            bestellingDetails = $"{bestellingenOpen[bestelling].BesteldeItems[i]}";
                        else
                            bestellingDetails = $"{bestellingDetails}, {bestellingenOpen[bestelling].BesteldeItems[i]}";
                    }

                    lbBestellingBO.Text = bestellingDetails;
                    lbOpmerkingBO.Text = bestellingenOpen[bestelling].BestellingOpmerking;
                    lbTijdBO.Text = bestellingenOpen[bestelling].TijdBestelling.ToString();
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        //BESTELLING OPNEEM SCHERM (Louella)
        private void btnBestellingOpnemen_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            AllePanelsOnzichtbaar();
            pnlBestellingOpnemen.Show();
            LabelFont();
            ComboBoxesLaden();
            MaakButtonAan();
        }

        private void LabelFont() //Louëlla Cremmers 641347
        {

            lblDiner.Font = new Font(lblLunch.Font, FontStyle.Regular);
            lblLunch.Font = new Font(lblLunch.Font, FontStyle.Bold);
            lblDrankjes.Font = new Font(lblLunch.Font, FontStyle.Regular);
            lblGerechten.Font = new Font(lblLunch.Font, FontStyle.Bold);
        }

        private void ComboBoxesLaden() //Louëlla Cremmers 641347
        {
            try
            {
                List<Tafel> tafels = tafel_Service.AllesOphalen();

                cbTafel.Items.Clear();
                foreach (Tafel tafel in tafels)
                {
                    cbTafel.Items.Add($"Tafel {tafel.TafelId}");
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        //MAAK BUTTONS AAN

        private void ButtonStyle(List<MenuItem> menuItems, int aantalKnoppenOver, int aantalKnoppenMax, Panel pnl) //Louella Creemers 641347
        {
            try
            {
                for (int i = 0; i < aantalKnoppenMax; i++)
                {
                    aantalKnoppenOver--;

                    if (i < menuItems.Count() && aantalKnoppenOver != 0)
                    {
                        int menuItemId = menuItems[i].MenuItemId;
                        ChapooModel.MenuItem menuitem = menuItem_Service.Selecteer_MenuItem_Bij_Naam(menuItemId);

                        Button button = new Button();
                        button.Text = menuitem.Naam;
                        button.Name = "button" + i;
                        button.Height = 110;
                        button.Width = 110;
                        button.FlatStyle = FlatStyle.Flat;
                        ButtonKlikEvent(button);

                        Switch(button, i);

                        int menuId = menuitem.MenuId;

                        if (menuitem.VoorraadAantal <= 0)
                        {
                            button.Enabled = false;
                            button.BackColor = Color.Gray;
                        }

                        else
                        {
                            KleurButton(menuId, button);
                        }

                        if (i < 16)
                        {
                            pnl.Controls.Add(button);
                        }

                        else
                        {
                            pnlDrank2.Controls.Add(button);
                        }


                    }

                    else
                    {
                        int menuId = 0;
                        Button button = new Button();
                        button.Height = 110;
                        button.Width = 110;
                        button.FlatStyle = FlatStyle.Flat;

                        Switch(button, i);
                        KleurButton(menuId, button);

                        if (i > 15)
                        {
                            pnlDrank2.Controls.Add(button);
                        }

                        else
                        {
                            pnl.Controls.Add(button);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void ButtonKlikEvent(Button button) //Louella Creemers 641347
        {
            if ((lblDrankjes.Font.Style & FontStyle.Bold) != 0)
            {
                pnlDrank1.BringToFront();
                button.Click += btnDrank_Click;
            }

            else if ((lblLunch.Font.Style & FontStyle.Bold) != 0)
            {
                pnlLunch.BringToFront();
                button.Click += btnLunch_Click;
            }

            else
            {
                pnlDiner.BringToFront();
                button.Click += btnDiner_Click;
            }

        }

        private void MaakButtonAan() //Louëlla Cremmers 641347
        {
            try
            {
                if ((lblDrankjes.Font.Style & FontStyle.Bold) != 0) //Als drank groupbox actief is
                {
                    ButtonStyle(menuItem_Service.Selecteer_Item_Drank(), 32, 32, pnlDrank1);
                }

                else if ((lblLunch.Font.Style & FontStyle.Bold) != 0) //Louëlla Cremmers 641347
                {
                    ButtonStyle(menuItem_Service.Selecteer_Item_Lunch(), 16, 16, pnlLunch);
                }

                else //Als dinerscherm actief is
                {
                    pnlDiner.BringToFront();
                    pnlDiner.Show();
                    ButtonStyle(menuItem_Service.Selecteer_Item_Diner(), 16, 16, pnlDiner);

                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void Switch(Button button, int i) //Louëlla Cremmers 641347 //Switch voor het positioneren van de buttons
        {
            try
            {
                if (i > 15) //Als i groter is dan 16, begin opnieuw bij 1 voor Drank schermen
                {
                    i = i - 16;
                }

                switch (i)
                {
                    case 0:
                        button.Location = new Point(80, 0);
                        break;
                    case 1:
                        button.Location = new Point(200, 0);
                        break;
                    case 2:
                        button.Location = new Point(320, 0);
                        break;
                    case 3:
                        button.Location = new Point(440, 0);
                        break;
                    case 4:
                        button.Location = new Point(80, 120);
                        break;
                    case 5:
                        button.Location = new Point(200, 120);
                        break;
                    case 6:
                        button.Location = new Point(320, 120);
                        break;
                    case 7:
                        button.Location = new Point(440, 120);
                        break;
                    case 8:
                        button.Location = new Point(80, 240);
                        break;
                    case 9:
                        button.Location = new Point(200, 240);
                        break;
                    case 10:
                        button.Location = new Point(320, 240);
                        break;
                    case 11:
                        button.Location = new Point(440, 240);
                        break;
                    case 12:
                        button.Location = new Point(80, 360);
                        break;
                    case 13:
                        button.Location = new Point(200, 360);
                        break;
                    case 14:
                        button.Location = new Point(320, 360);
                        break;
                    case 15:
                        button.Location = new Point(440, 360);
                        break;

                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }


        private void KleurButton(int menuId, Button button) //Louëlla Cremmers 641347 //Kleuren per menu 
        {
            try
            {
                if ((menuId == 1) || (menuId == 4)) //Voorgerecht 
                {
                    button.BackColor = Color.FromArgb(152, 179, 252);
                }

                else if ((menuId == 2) || (menuId == 6) || (menuId == 8)) //Hoofdgerecht + drank
                {
                    button.BackColor = Color.FromArgb(117, 234, 247);
                }

                else if ((menuId == 3) || (menuId == 7)) //Nagerecht
                {

                    button.BackColor = Color.FromArgb(206, 249, 255);
                }

                else if (menuId == 5) //Tussengerecht
                {
                    button.BackColor = Color.FromArgb(88, 138, 255);
                }

                else //Menu's die niet gelist zijn
                {
                    button.BackColor = Color.FromArgb(204, 204, 204);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        //BUTTON EVENTS

        private void btnLunch_Click(object sender, EventArgs e) //Louëlla Cremmers 641347 //Klik event voor elke knop in Lunch
        {
            Button btnLunch = sender as Button;
            ChapooModel.MenuItem menuitem = menuItem_Service.Selecteer_MenuItem_Bij_Id(btnLunch.Text);
            MenuButtonKlik(menuitem.MenuItemId, btnLunch, lvMenuItem);
        }

        private void btnDiner_Click(object sender, EventArgs e) //Klik event voor elke knop in Diner
        {
            Button btnDiner = sender as Button;
            ChapooModel.MenuItem menuitem = menuItem_Service.Selecteer_MenuItem_Bij_Id(btnDiner.Text);
            MenuButtonKlik(menuitem.MenuItemId, btnDiner, lvMenuItemDiner);

        }

        private void btnDrank_Click(object sender, EventArgs e) //Klik event voor elke knop in Drank
        {
            Button btnDrank = sender as Button;
            ChapooModel.MenuItem menuitem = menuItem_Service.Selecteer_MenuItem_Bij_Id(btnDrank.Text);
            MenuButtonKlik(menuitem.MenuItemId, btnDrank, lvDrank);
        }


        //BESTELDE ITEMS LV

        public void MenuButtonKlik(int id, Button button, ListView listView) //Louëlla Cremmers 641347 //Selecteert juiste menuitem
        {
            try
            {
                ChapooModel.MenuItem item = menuItem_Service.Selecteer_Een_MenuItem(id);
                ItemToevoegenAanListView(listView, item);
                btnVerstuur.Text = "➤";
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private bool CheckVoorraadPlus(string naam, int aantal) //Louëlla Creemers 641347 //Checkt voorraad bij duplicaten of bij klikken van plus knop 
        {
            try
            {
                MenuItem menuItem = menuItem_Service.Selecteer_MenuItem_Bij_Id(naam);

                int voorraadOver = menuItem.VoorraadAantal;

                if (aantal >= voorraadOver)
                {
                    MessageBox.Show("U zit aan de maximale voorraad", "Fout", MessageBoxButtons.OK);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Catch(ex);
                return false;
            }
        }


        private void ItemToevoegenAanListView(ListView lv, ChapooModel.MenuItem item) //Louëlla Cremmers 641347
        {
            try
            {
                ListViewItem listItem = lv.FindItemWithText(item.Naam);

                if (listItem == null)
                {
                    ListViewItem li = new ListViewItem(item.Naam);
                    int aantal = 1;
                    li.SubItems.Add(aantal.ToString());
                    lv.Items.Insert(0, li);
                }

                else
                {
                    int aantal = int.Parse(lv.Items[listItem.Index].SubItems[1].Text);
                    lv.Items[listItem.Index].SubItems[1].Text = aantal.ToString();

                    if (CheckVoorraadPlus(lv.Items[listItem.Index].SubItems[0].Text, int.Parse(lv.Items[listItem.Index].SubItems[1].Text)))
                    {
                        lv.Items[listItem.Index].SubItems[1].Text = aantal.ToString();
                    }

                    else
                    {
                        aantal++;
                        lv.Items[listItem.Index].SubItems[1].Text = aantal.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }


        //AANPASSENDE KNOPPEN 

        private void PlusProduct(ListView lv)
        {
            try
            {
                if (lv.FocusedItem == null)
                {
                    MessageBox.Show("Selecteer eerst een gerecht", "Fout", MessageBoxButtons.OK);
                }

                else
                {
                    int rijGeselecteerd = lv.FocusedItem.Index;
                    int aantal = int.Parse(lv.Items[rijGeselecteerd].SubItems[1].Text);

                    if (CheckVoorraadPlus(lv.Items[rijGeselecteerd].SubItems[0].Text, int.Parse(lv.Items[rijGeselecteerd].SubItems[1].Text)))
                    {
                        lv.Items[rijGeselecteerd].SubItems[1].Text = aantal.ToString();
                        lv.FocusedItem = null;
                    }

                    else
                    {
                        aantal++;
                        lv.Items[rijGeselecteerd].SubItems[1].Text = aantal.ToString();
                        lv.FocusedItem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }

        }

        private void MinProduct(ListView lv)
        {
            try
            {
                if (lv.FocusedItem == null)
                {
                    MessageBox.Show("Selecteer eerst een gerecht", "Fout", MessageBoxButtons.OK);
                }

                else
                {
                    int rijGeselecteerd = lv.FocusedItem.Index;

                    int aantal = int.Parse(lv.Items[rijGeselecteerd].SubItems[1].Text);
                    aantal--;
                    lv.Items[rijGeselecteerd].SubItems[1].Text = aantal.ToString();

                    if (aantal <= 0)
                    {
                        MessageBox.Show("U mag minimaal 1 per gerecht bestellen", "Fout", MessageBoxButtons.OK);
                        aantal++;
                        lv.Items[rijGeselecteerd].SubItems[1].Text = aantal.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void VerwijderProduct(ListView lv)
        {
            try
            {
                if (lv.FocusedItem == null)
                {
                    MessageBox.Show("Selecteer eerst een gerecht", "Fout", MessageBoxButtons.OK);
                }

                else
                {
                    int rijGeselecteerd = lv.FocusedItem.Index;

                    lv.Items.RemoveAt(rijGeselecteerd);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btnPlus_Click(object sender, EventArgs e)  //Louëlla Cremmers 641347 
        {

            try
            {
                if ((lblLunch.Font.Style & FontStyle.Bold) != 0) //Voor lunch LV
                {
                    PlusProduct(lvMenuItem);
                }

                else if ((lblDiner.Font.Style & FontStyle.Bold) != 0)  //Voor diner LV
                {
                    PlusProduct(lvMenuItemDiner);
                }

                else if ((lblDrankjes.Font.Style & FontStyle.Bold) != 0) //Voor drank LV
                {
                    PlusProduct(lvDrank);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btnMin_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            try
            {
                if ((lblLunch.Font.Style & FontStyle.Bold) != 0) //Voor lunch LV
                {
                    MinProduct(lvMenuItem);
                }

                else if ((lblDiner.Font.Style & FontStyle.Bold) != 0)  //Voor diner LV
                {
                    MinProduct(lvMenuItemDiner);
                }

                else if ((lblDrankjes.Font.Style & FontStyle.Bold) != 0) //Voor drank LV
                {
                    MinProduct(lvDrank);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btnVerwijder_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            try
            {

                if ((lblLunch.Font.Style & FontStyle.Bold) != 0) //Voor lunch LV
                {
                    VerwijderProduct(lvMenuItem);
                }

                else if ((lblDiner.Font.Style & FontStyle.Bold) != 0)  //Voor diner LV
                {
                    VerwijderProduct(lvMenuItemDiner);
                }

                else if ((lblDrankjes.Font.Style & FontStyle.Bold) != 0) //Voor drank LV
                {
                    VerwijderProduct(lvDrank);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }

        }

        private void btnOpmerking_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            pnlOpmerking.BringToFront();
            pnlOpmerking.Show();
        }

        private void btnTerugOpmerking_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            pnlOpmerking.Hide();

            if (tbOpmerking.Text != "")
            {
                btnOpmerking.Text = "Opmerking ✔";
            }
        }


        //VERSTUUR BESTELDEITEMS + OPMERKING

        Bestelling _nieuwBestelling = new Bestelling(); //Louella Creemers 641347 

        private void BesteldeItemsBestellen(ListView lv) //Louëlla Cremmers 641347
        {
            for (int i = 0; i < lv.Items.Count; i++)
            {
                ChapooModel.MenuItem menuItem = menuItem_Service.Selecteer_MenuItem_Bij_Id(lv.Items[i].SubItems[0].Text);
                int aantal = int.Parse(lv.Items[i].SubItems[1].Text);
                int bestellingId = _nieuwBestelling.BestellingId;

                besteldeItem_Service.AlleItemsToevoegen(bestellingId, menuItem.MenuItemId, aantal);
            }

            lvMenuItem.Items.Clear();
            lvMenuItemDiner.Items.Clear();
            lvDrank.Items.Clear();
        }

        private void VerstuurBestelling(ListView lv) //Louëlla Cremmers 641347
        {
            try
            {
                int medewerkerId = IngelogdeMedewerker.MedewerkerId;

                string[] tafelInput = cbTafel.Text.Split(' ');

                if (lv.Items.Count == 0)
                {
                    MessageBox.Show("Bestellijst is leeg. Selecteer eerst een gerecht/drankje", "Fout", MessageBoxButtons.OK);
                    btnVerstuur.Text = "✘";
                }

                else if (IngelogdeMedewerker == null)
                {
                    MessageBox.Show("`Ingelogde medewerker niet gevonden. Probeer het opnieuw of log opnieuw in", "Fout", MessageBoxButtons.OK);
                    btnVerstuur.Text = "✘";
                }

                else
                {
                    int tafel = int.Parse(tafelInput[1]);

                    string opmerking = tbOpmerking.Text;

                    _nieuwBestelling.BestellingId = bestelling_Service.Maak_Nieuwe_Bestelling(tafel, medewerkerId, opmerking);

                    btnVerstuur.Text = "✔";

                    if ((lblLunch.Font.Style & FontStyle.Bold) != 0) //Bestel Producten uit listview Lunch
                    {
                        BesteldeItemsBestellen(lvMenuItem);

                    }

                    else if ((lblDiner.Font.Style & FontStyle.Bold) != 0) //Bestel producten uit listview Avond
                    {
                        BesteldeItemsBestellen(lvMenuItemDiner);
                    }

                    else if ((lblDrankjes.Font.Style & FontStyle.Bold) != 0) //Bestel producten uit listview Drank
                    {
                        BesteldeItemsBestellen(lvDrank);
                    }

                    cbTafel.ResetText();
                    tbOpmerking.Text = "";
                    btnOpmerking.Text = "+ Opmerking";
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }

        }

        private void btnVerstuur_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            try
            {

                if (cbTafel.Text == "")
                {
                    MessageBox.Show("Selecteer een tafel", "Fout", MessageBoxButtons.OK);
                }

                else
                {

                    if ((lblLunch.Font.Style & FontStyle.Bold) != 0)
                    {
                        VerstuurBestelling(lvMenuItem);

                    }

                    else if ((lblDiner.Font.Style & FontStyle.Bold) != 0)
                    {
                        VerstuurBestelling(lvMenuItemDiner);
                    }

                    else if ((lblDrankjes.Font.Style & FontStyle.Bold) != 0)
                    {
                        VerstuurBestelling(lvDrank);
                    }
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }

        }


        //BUTTON STYLE AANPASSEN NA EEN KLIK EVENT
        private void lblDiner_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            pnlDiner.BringToFront();
            lblLunch.Font = new Font(lblLunch.Font, FontStyle.Regular);
            lblDiner.Font = new Font(lblLunch.Font, FontStyle.Bold);
            MaakButtonAan();

        }

        private void lblLunch_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            pnlLunch.BringToFront();
            lblLunch.Font = new Font(lblLunch.Font, FontStyle.Bold);
            lblDiner.Font = new Font(lblDiner.Font, FontStyle.Regular);
            MaakButtonAan();
        }
        private void lblDrankjes_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            pnlDrank1.BringToFront();
            pnlDrank1.Show();
            lblGerechten.Font = new Font(lblGerechten.Font, FontStyle.Regular);
            lblLunch.Font = new Font(lblLunch.Font, FontStyle.Regular);
            lblDiner.Font = new Font(lblDiner.Font, FontStyle.Regular);
            lblDrankjes.Font = new Font(lblDrankjes.Font, FontStyle.Bold);

            lblLunch.Hide();
            lblDiner.Hide();

            MaakButtonAan();
        }

        private void lblGerechten_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            pnlLunch.BringToFront();
            lblDiner.Show();
            lblLunch.Show();

            lblGerechten.Font = new Font(lblGerechten.Font, FontStyle.Bold);
            lblLunch.Font = new Font(lblGerechten.Font, FontStyle.Bold);
            lblDrankjes.Font = new Font(lblDrankjes.Font, FontStyle.Regular);

            MaakButtonAan();
        }

        //VOLGENDE - VORIGE BUTTONS DRANK
        private void btnVolgende_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            pnlDrank2.BringToFront();
        }

        private void btnVorige_Click(object sender, EventArgs e) //Louëlla Cremmers 641347
        {
            pnlDrank1.BringToFront();
        }

        //Logo click naar hoofdmenu

        private void button1_Click_1(object sender, EventArgs e)
        {
            pnlMenu.BringToFront();
            pnlMenu.Show();
        }

        //EINDE BESTELSCHERM GEDEELTE

        //GEDEELTE GEMAAKT DOOR Koen van Cromvoirt 647634

        private void LaatAlleVoorraadzien() // Koen van Cromvoirt 647634
        {
            try
            {
                listViewVoorraad.Items.Clear();
                Voorraad_Service voorraad_Service2 = new Voorraad_Service();
                List<Voorraad> voorraadKeuken = voorraad_Service2.KrijgAllesVoorraadKeuken();
                foreach (Voorraad gerecht in voorraadKeuken)
                {

                    ListViewItem item = new ListViewItem(gerecht.VoorraadId.ToString());
                    item.SubItems.Add(gerecht.Naam.ToString());
                    item.SubItems.Add(gerecht.VoorraadAantal.ToString());
                    if (gerecht.VoorraadAantal == 0)
                    {
                        item.BackColor = Color.Red;
                    }
                    else if (gerecht.VoorraadAantal > 0 && gerecht.VoorraadAantal <= 20)
                    {
                        item.BackColor = Color.Orange;
                    }
                    else if (gerecht.VoorraadAantal > 20 && gerecht.VoorraadAantal < 30)
                    {
                        item.BackColor = Color.Yellow;
                    }
                    else if (gerecht.VoorraadAantal >= 30)
                    {
                        item.BackColor = Color.Green;
                    }
                    listViewVoorraad.Items.Add(item);

                }
                Voorraad_Service voorraad_Service = new Voorraad_Service();
                List<Voorraad> voorraadBar = voorraad_Service.KrijgAllesVoorraadBar();
                foreach (Voorraad drank in voorraadBar)
                {
                    ListViewItem item = new ListViewItem(drank.VoorraadId.ToString());
                    item.SubItems.Add(drank.Naam.ToString());
                    item.SubItems.Add(drank.VoorraadAantal.ToString());
                    if (drank.VoorraadAantal == 0)
                    {
                        item.BackColor = Color.Red;
                    }
                    else if (drank.VoorraadAantal > 0 && drank.VoorraadAantal <= 20)
                    {
                        item.BackColor = Color.Orange;
                    }
                    else if (drank.VoorraadAantal > 20 && drank.VoorraadAantal < 30)
                    {
                        item.BackColor = Color.Yellow;
                    }
                    else if (drank.VoorraadAantal >= 30)
                    {
                        item.BackColor = Color.Green;
                    }

                    listViewVoorraad.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        //Managment | Koen van Cromvoirt 647634

        private int SelectedIdpublic = 0;
        private void btnAanpassenVoorraad_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            try
            {
                string selectedVoorraadId = listViewVoorraad.SelectedItems[0].SubItems[0].Text;
                string selectedVoorraadNaam = listViewVoorraad.SelectedItems[0].SubItems[1].Text;
                string selectedVoorraadAantal = listViewVoorraad.SelectedItems[0].SubItems[2].Text;
                SelectedIdpublic = int.Parse(selectedVoorraadId);
                AllePanelsOnzichtbaar();
                pnlVoorraadaanpassen.Show();
                KeukenBestellingDetails(selectedVoorraadId);
                ListViewItem item = new ListViewItem(selectedVoorraadId);
                item.SubItems.Add(selectedVoorraadNaam);
                item.SubItems.Add(selectedVoorraadAantal);
                listViewVoorraadaanpassen.Items.Add(item);
            }
            catch (Exception ex)
            {
                lblManagementVoorraad.Text = "⚠ " + ex.Message;
            }
        }

        private void btnMenuVoorraadEigenaar_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            AllePanelsOnzichtbaar();
            pnlManagmentMenu.Show();
        }

        private void btn_ok_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            try
            {

                string nieuweAantal = textBoxAantal.Text;
                int nieuwevoorraadAantal = int.Parse(nieuweAantal);
                AllePanelsOnzichtbaar();
                pnlManagmentVoorraad.Show();
                voorraad_Service.BewerkenVoorraadAantal(SelectedIdpublic, nieuwevoorraadAantal);
                LaatAlleVoorraadzien();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            AllePanelsOnzichtbaar();
            pnlManagmentVoorraad.Show();
            LaatAlleVoorraadzien();
        }

        private void btnMenuVoorraadBediening_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            try
            {
                AllePanelsOnzichtbaar();
                lblManagementVoorraad.Text = string.Empty;
                btnAanpassenVoorraad.Visible = false;
                pnlManagmentVoorraad.Show();
                LaatAlleVoorraadzien();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btn_VoorraadManagment_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            try
            {
                AllePanelsOnzichtbaar();
                this.listViewVoorraad.Columns[0].Width = 50;
                this.listViewVoorraad.Columns[1].Width = 500;
                this.listViewVoorraad.Columns[2].Width = 65;
                btnAanpassenVoorraad.Visible = true;
                lblManagementVoorraad.Text = string.Empty;
                pnlManagmentVoorraad.Show();
                LaatAlleVoorraadzien();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btnMedewerkerInfo_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            try
            {

                AllePanelsOnzichtbaar();
                pnlMedewerkers.Show();
                LaatAlleMedewerkersZien();
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void LaatAlleMedewerkersZien() // Koen van Cromvoirt 647634
        {
            try
            {
                listViewmedewerkers.Items.Clear();
                Medewerker2_Service medewerker_Service = new Medewerker2_Service();
                List<Medewerker2> medewerkers = medewerker_Service.AllesOphalen();
                foreach (Medewerker2 medewerker in medewerkers)
                {
                    ListViewItem item = new ListViewItem(medewerker.MedewerkerId2.ToString());
                    item.SubItems.Add(medewerker.VolledigeNaam2.ToString());
                    item.SubItems.Add(medewerker.Functie2.ToString());
                    item.SubItems.Add(medewerker.Email2.ToString());
                    listViewmedewerkers.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }


        private void btnFilterBediening_Click_1(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            try
            {
                listViewmedewerkers.Items.Clear();
                Medewerker2_Service medewerker_Service = new Medewerker2_Service();
                List<Medewerker2> medewerkers = medewerker_Service.BedieningOphalen();
                foreach (Medewerker2 medewerker in medewerkers)
                {
                    ListViewItem item = new ListViewItem(medewerker.MedewerkerId2.ToString());
                    item.SubItems.Add(medewerker.VolledigeNaam2.ToString());
                    item.SubItems.Add(medewerker.Functie2.ToString());
                    item.SubItems.Add(medewerker.Email2.ToString());
                    listViewmedewerkers.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btnFilterKok_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            try
            {
                listViewmedewerkers.Items.Clear();
                Medewerker2_Service medewerker_Service = new Medewerker2_Service();
                List<Medewerker2> medewerkers = medewerker_Service.KoksOphalen();
                foreach (Medewerker2 medewerker in medewerkers)
                {
                    ListViewItem item = new ListViewItem(medewerker.MedewerkerId2.ToString());
                    item.SubItems.Add(medewerker.VolledigeNaam2.ToString());
                    item.SubItems.Add(medewerker.Functie2.ToString());
                    item.SubItems.Add(medewerker.Email2.ToString());
                    listViewmedewerkers.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btnFilterEigenaar_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            try
            {
                listViewmedewerkers.Items.Clear();
                Medewerker2_Service medewerker_Service = new Medewerker2_Service();
                List<Medewerker2> medewerkers = medewerker_Service.EigenaarOphalen();
                foreach (Medewerker2 medewerker in medewerkers)
                {
                    ListViewItem item = new ListViewItem(medewerker.MedewerkerId2.ToString());
                    item.SubItems.Add(medewerker.VolledigeNaam2.ToString());
                    item.SubItems.Add(medewerker.Functie2.ToString());
                    item.SubItems.Add(medewerker.Email2.ToString());
                    listViewmedewerkers.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }

        private void btnFilterBarman_Click(object sender, EventArgs e) // Koen van Cromvoirt 647634
        {
            try
            {
                listViewmedewerkers.Items.Clear();
                Medewerker2_Service medewerker_Service = new Medewerker2_Service();
                List<Medewerker2> medewerkers = medewerker_Service.BarmanOphalen();
                foreach (Medewerker2 medewerker in medewerkers)
                {
                    ListViewItem item = new ListViewItem(medewerker.MedewerkerId2.ToString());
                    item.SubItems.Add(medewerker.VolledigeNaam2.ToString());
                    item.SubItems.Add(medewerker.Functie2.ToString());
                    item.SubItems.Add(medewerker.Email2.ToString());
                    listViewmedewerkers.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Catch(ex);
            }
        }
    }
}
