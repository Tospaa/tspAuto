using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.IO;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for YeniMuvekkilEkle.xaml
    /// </summary>
    public partial class YeniMuvekkilEkle : UserControl
    {
        public YeniMuvekkilEkle()
        {
            InitializeComponent();

            VekTarihi.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
        }

        private void Muvekkil_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.DatabaseFilePath != "" && File.Exists(Properties.Settings.Default.DatabaseFilePath))
                {
                    string sql = @"CREATE TABLE IF NOT EXISTS Muvekkil(
                            ID   INTEGER PRIMARY KEY AUTOINCREMENT,
                            MuvekkilTuru        TEXT      NOT NULL,
                            NoterIsmi           TEXT      NOT NULL,
                            VekaletTarihi       TEXT      NOT NULL,
                            VekYevmiyeNo        TEXT      NOT NULL,
                            AhzuKabza           BOOLEAN   NOT NULL,
                            Feragat             BOOLEAN   NOT NULL,
                            Ibra                BOOLEAN   NOT NULL,
                            Sulh                BOOLEAN   NOT NULL,
                            Banka               TEXT      NOT NULL,
                            Sube                TEXT      NOT NULL,
                            IBANno              TEXT      NOT NULL
                            );";

                    string sql2 = $@"INSERT INTO Muvekkil(
                            MuvekkilTuru,NoterIsmi,VekaletTarihi,VekYevmiyeNo,AhzuKabza,Feragat,Ibra,Sulh,Banka,Sube,IBANno) VALUES(
                            '{MuvekkilTuru.Text}','{NoterIsmi.Text}','{String.Format("{0:dd.MM.yyyy}", VekTarihi.SelectedDate)}','{VekYevNo.Text}',
                            {AhzuKabza.SelectedIndex},{Feragat.SelectedIndex},{Ibra.SelectedIndex},{Sulh.SelectedIndex},
                            '{Banka.Text}','{Sube.Text}','{IBANno.Text}');";
                    
                    bool basarili = false;

                    try
                    {
                        using (SQLiteConnection con = new SQLiteConnection($"Data Source={Properties.Settings.Default.DatabaseFilePath};"))
                        {
                            con.Open();
                            new SQLiteCommand(sql + sql2, con).ExecuteNonQuery();

                            basarili = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanı işlemi sırasında bir hata oluştu.\n\n" + ex.Message);
                    }
                    finally
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        if (basarili)
                        {
                            MessageBox.Show("Veritabanı girdisi başarılı.");
                            MuvekkilTuru.Text = string.Empty;
                            NoterIsmi.Text = string.Empty;
                            VekTarihi.SelectedDate = null;
                            VekYevNo.Text = string.Empty;
                            AhzuKabza.SelectedIndex = 1;
                            Feragat.SelectedIndex = 1;
                            Ibra.SelectedIndex = 1;
                            Sulh.SelectedIndex = 1;
                            Banka.Text = string.Empty;
                            Sube.Text = string.Empty;
                            IBANno.Text = string.Empty;
                        }
                    }
                }
                else if (Properties.Settings.Default.DatabaseFilePath == "")
                {
                    MessageBox.Show("Veritabanı seçilmemiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                }
                else if (!File.Exists(Properties.Settings.Default.DatabaseFilePath))
                {
                    MessageBox.Show("Veritabanı silinmiş ya da erişim engellenmiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Bazı dosyalar silinmiş ya da erişim engellenmiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
            }
        }

        private void Sirket_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            if (VekTarihi.SelectedDate != null)
            {
                MessageBox.Show(String.Format("{0:dd.MM.yyyy}", VekTarihi.SelectedDate));
            }
        }
    }
}
