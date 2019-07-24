using MaterialDesignThemes.Wpf;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

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

            DataRowView row = (Icerik.Content as AramaYap).MuvekkilSahis_tt.SelectedItem as DataRowView;

            try
            {
                Doldur(row["IsimSoyisim"].ToString(), row["TCKimlik"].ToString(), row["Adres"].ToString());
            }
            catch (NullReferenceException) { return; }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }

        private void Doldur(string isimsoyisim, string tckimlikno, string adres)
        {
            if (workMode == WorkModes.Filling_1)
            {
                IsimSoyisim_1.Text = isimsoyisim;
                TCKimlikNo_1.Text = tckimlikno;
                Adres_1.Text = adres;
            }
            else if (workMode == WorkModes.Filling_1_Vekil)
            {
                IsimSoyisim_1_Vekil.Text = isimsoyisim;
                TCKimlikNo_1_Vekil.Text = tckimlikno;
                Adres_1_Vekil.Text = adres;
            }
            else if (workMode == WorkModes.Filling_2)
            {
                IsimSoyisim_2.Text = isimsoyisim;
                TCKimlikNo_2.Text = tckimlikno;
                Adres_2.Text = adres;
            }
            else if (workMode == WorkModes.Filling_2_Vekil)
            {
                IsimSoyisim_2_Vekil.Text = isimsoyisim;
                TCKimlikNo_2_Vekil.Text = tckimlikno;
                Adres_2_Vekil.Text = adres;
            }
        }
    }

    public enum WorkModes { Filling_1, Filling_1_Vekil, Filling_2, Filling_2_Vekil }
}
