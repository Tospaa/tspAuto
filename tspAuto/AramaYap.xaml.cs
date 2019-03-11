using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Data;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for AramaYap.xaml
    /// </summary>
    public partial class AramaYap : UserControl
    {
        public AramaYap()
        {
            InitializeComponent();
        }

        private void AramaKutusu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AramaKutusu.Text.Length >= 1)
            {
                Arama();
            }
        }

        public void Arama()
        {
            DataSet dataSet = new DataSet();

            try
            {
                using (SQLiteConnection con = new SQLiteConnection($@"Data Source={Properties.Settings.Default.DatabaseFilePath}"))
                {
                    using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter($"SELECT * FROM Muvekkil WHERE(MuvekkilTuru LIKE '%{AramaKutusu.Text}%' OR NoterIsmi LIKE '%{AramaKutusu.Text}%' OR VekaletTarihi LIKE '%{AramaKutusu.Text}%' OR VekYevmiyeNo LIKE '%{AramaKutusu.Text}%' OR Banka LIKE '%{AramaKutusu.Text}%' OR Sube LIKE '%{AramaKutusu.Text}%' OR IBANno LIKE '%{AramaKutusu.Text}%')", con))
                    {
                        dataAdapter.Fill(dataSet);
                    }
                }

                MuvekkilSonuc.ItemsSource = dataSet.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("kötü kötü olduk\n\n" + ex.Message);
            }
        }
    }
}
