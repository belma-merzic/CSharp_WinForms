using cSharpIntroWinForms.P10;
using cSharpIntroWinForms.P8;
using cSharpIntroWinForms.P9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cSharpIntroWinForms
{
    public partial class KorisniciAdmin : Form
    {

        KonekcijaNaBazu konekcijaNaBazu = DLWMS.DB;

        public KorisniciAdmin()
        {
            InitializeComponent();
            dgvKorisnici.AutoGenerateColumns = false;
        }

        private void KorisniciAdmin_Load(object sender, EventArgs e)
        {
            LoadData();
        }



        private void LoadData(List<Korisnik> korisnici = null)
        {
            try
            {
                List<Korisnik> rezultati = korisnici ?? konekcijaNaBazu.Korisnici.ToList();

                dgvKorisnici.DataSource = null;
                dgvKorisnici.DataSource = rezultati;
                lblProsjek.Text = IzracunajProsjek(rezultati).ToString();

            }
            catch (Exception ex)
            {
                MboxHelper.PrikaziGresku(ex);
            }
        }

        private float IzracunajProsjek(List<Korisnik> korisnici)
        {
            float suma = 0;

            if (korisnici.Count() == 0)
                return suma;
            else
            {
                foreach (var korisnik in korisnici)
                {
                    suma += korisnik.ProsjekKorisnika();
                }
                return suma / korisnici.Count();
            }
        }

        private void txtPretraga_TextChanged(object sender, EventArgs e)
        {
           var filter = txtPretraga.Text;
            var rezultat = new List<Korisnik>();

            foreach (var korisnik in konekcijaNaBazu.Korisnici)
            {
                if (korisnik.Ime.ToLower().Contains(filter.ToLower()) || korisnik.Prezime.ToLower().Contains(filter.ToLower()))
                    rezultat.Add(korisnik);
            }
            LoadData(rezultat);
       }

        private void dgvKorisnici_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var korisnik = dgvKorisnici.SelectedRows[0].DataBoundItem as Korisnik;

            if(korisnik != null)
            {
                var forma = new KorisniciPolozeniPredmeti(korisnik);   ////////////////////////////////////////ovo je popravilo greskuuuuuuuuuuuuu
                forma.ShowDialog();
            }
        }
    }
}
