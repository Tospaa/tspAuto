using MaterialDesignThemes.Wpf;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using tspAuto.Model;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for YeniDosyaEkle.xaml
    /// </summary>
    public partial class YeniDosyaEkle : UserControl
    {
        public YeniDosyaEkle()
        {
            InitializeComponent();
        }

        private WorkModes workMode;

        private void Getir_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == "Getir1") { workMode = WorkModes.Filling_1; }
            else if ((sender as Button).Name == "Getir1_Vekil") { workMode = WorkModes.Filling_1_Vekil; }
            else if ((sender as Button).Name == "Getir2") { workMode = WorkModes.Filling_2; }
            else if ((sender as Button).Name == "Getir2_Vekil") { workMode = WorkModes.Filling_2_Vekil; }

            Icerik.Content = new AramaYap();
        }

        private void YeniDosyaEkleDialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return; // çok coolum ya mk xD

            try
            {
                Doldur((Icerik.Content as AramaYap).MuvekkilSahis_tt.SelectedItem as MuvekkilSahis);
            }
            catch (NullReferenceException) { return; }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }

        private void Doldur(MuvekkilSahis sahis)
        {
            if (workMode == WorkModes.Filling_1)
            {
                IsimSoyisim_1.Tag = sahis.ID;
                IsimSoyisim_1.Text = sahis.IsimSoyisim;
                TCKimlikNo_1.Text = sahis.TCKimlik;
                Adres_1.Text = sahis.Adres;
            }
            else if (workMode == WorkModes.Filling_1_Vekil)
            {
                IsimSoyisim_1_Vekil.Tag = sahis.ID;
                IsimSoyisim_1_Vekil.Text = sahis.IsimSoyisim;
                TCKimlikNo_1_Vekil.Text = sahis.TCKimlik;
                Adres_1_Vekil.Text = sahis.Adres;
            }
            else if (workMode == WorkModes.Filling_2)
            {
                IsimSoyisim_2.Tag = sahis.ID;
                IsimSoyisim_2.Text = sahis.IsimSoyisim;
                TCKimlikNo_2.Text = sahis.TCKimlik;
                Adres_2.Text = sahis.Adres;
            }
            else if (workMode == WorkModes.Filling_2_Vekil)
            {
                IsimSoyisim_2_Vekil.Tag = sahis.ID;
                IsimSoyisim_2_Vekil.Text = sahis.IsimSoyisim;
                TCKimlikNo_2_Vekil.Text = sahis.TCKimlik;
                Adres_2_Vekil.Text = sahis.Adres;
            }
        }

        private void DosyaDavaEkle_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DbConnection())
            {
                DosyaDava dosyaDava = new DosyaDava
                {
                    DosyaTuru = DosyaTuru.Text,
                    DosyaNo = DosyaNo.Text,
                    ArsivNo = ArsivNo.Text,
                    Davaci = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_1.Tag),
                    DavaciVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_1_Vekil.Tag),
                    Davali = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_2.Tag),
                    DavaliVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_2_Vekil.Tag),
                    Durum = Durum.Text,
                    DavaTuru = DavaTuru.Text,
                    Mahkeme = Mahkeme.Text,
                    DavaKonusu = Konusu.Text,
                    Log = $"[{DateTime.Now.ToString("dd/MM/yyyy hh:mm")}] Dosya oluşturuldu."
                };

                db.DosyaDava_tt.Add(dosyaDava);
                db.SaveChanges();
            }
        }

        private void DosyaIcraEkle_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DbConnection())
            {
                DosyaIcra dosyaIcra = new DosyaIcra
                {
                    DosyaTuru = DosyaTuru.Text,
                    DosyaNo = DosyaNo.Text,
                    ArsivNo = ArsivNo.Text,
                    Alacakli = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_1.Tag),
                    AlacakliVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_1_Vekil.Tag),
                    Borclu = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_2.Tag),
                    BorcluVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_2_Vekil.Tag),
                    IcraDairesi = IcraDairesi.Text,
                    Log = $"[{DateTime.Now.ToString("dd/MM/yyyy hh:mm")}] Dosya oluşturuldu."
                };

                db.DosyaIcra_tt.Add(dosyaIcra);
                db.SaveChanges();
            }
        }
    }

    public enum WorkModes { Filling_1, Filling_1_Vekil, Filling_2, Filling_2_Vekil }
}
