using cSharpIntroWinForms.P10;
using cSharpIntroWinForms.P9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cSharpIntroWinForms.P8
{
    public partial class KorisniciPolozeniPredmeti : Form
    {
        private Korisnik korisnik;

        KonekcijaNaBazu konekcijaNaBazu = DLWMS.DB;

        public KorisniciPolozeniPredmeti()
        {
            InitializeComponent();
            dgvPolozeniPredmeti.AutoGenerateColumns = false;
        }

        public KorisniciPolozeniPredmeti(Korisnik korisnik) : this()
        {
            this.korisnik = korisnik;
        }

        private void KorisniciPolozeniPredmeti_Load(object sender, EventArgs e)
        {
            UcitajPredmete();
            UcitajPolozene();
        }


        private void UcitajPredmete()
        {
                cmbPredmeti.DataSource = konekcijaNaBazu.Predmeti.ToList();
                cmbPredmeti.ValueMember = "Id";
                cmbPredmeti.DisplayMember = "Naziv";
        }



        private bool ValidirajUnos()
        {
            return Validator.ValidirajKontrolu(cmbPredmeti, err, "Ovo polje je obavezno.")
                && Validator.ValidirajKontrolu(cmbOcjene, err, "Ovo polje je obavezno.");
        }

        private void btnDodajPolozeni_Click(object sender, EventArgs e)
        {
            if(ValidirajUnos())
            {

                var odabraniPredmet = cmbPredmeti.SelectedItem as Predmeti;

                var predmet = cmbPredmeti.SelectedItem as Predmeti;
                var ocjena = cmbOcjene.SelectedItem.ToString();
                var datum = dtpDatumPolaganja.Text;

                if (ProvjeriDaLiPostojiPredmet(odabraniPredmet))
                    MessageBox.Show("Predmet je vec dodan.");

                else
                {
                    korisnik.Uspjeh.Add(new KorisniciPredmeti()
                    {
                        Predmet = predmet,
                        Ocjena = int.Parse(ocjena),
                        Datum = datum
                    });
                    konekcijaNaBazu.SaveChanges();
                    MessageBox.Show("Predmet uspjesno dodan.");
                    UcitajPolozene();
                }
            }
        }

        private bool ProvjeriDaLiPostojiPredmet(Predmeti odabraniPredmet)
        {
            if (korisnik.Uspjeh.Where(x => x.Predmet.Id == odabraniPredmet.Id).Count() > 0)
                return true;
            else
                return false;
        }

        private void UcitajPolozene()
        {
            dgvPolozeniPredmeti.DataSource = null;
            dgvPolozeniPredmeti.DataSource = korisnik.Uspjeh;
        }

        private void cbUcitajNepolozene_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUcitajNepolozene.Checked)
                UcitajNepolozene();
            else
                UcitajPredmete();
        }

        private void UcitajNepolozene()
        {
            var predmeti = konekcijaNaBazu.Predmeti.ToList();
            var nepolozeniPredmeti = new List<Predmeti>();

            foreach (var predmet in predmeti)
            {
                if (korisnik.Uspjeh.Count(x => x.Predmet.Id == predmet.Id) == 0)
                    nepolozeniPredmeti.Add(predmet);
            }
            cmbPredmeti.Text = "";
            cmbPredmeti.DataSource = nepolozeniPredmeti;
        }

    }
}
