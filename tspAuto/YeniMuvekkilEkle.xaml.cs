﻿using System;
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

        private SQLiteCommand Generate_Insert_Command(string table, string[] columns, object[] values, SQLiteConnection con)
        {
            string insertString = $"INSERT INTO {table}(";

            foreach (string i in columns)
            {
                insertString += i + ",";
            }

            insertString = insertString.Substring(0, insertString.Length - 1) + ") VALUES(";

            foreach (string i in columns)
            {
                insertString += "@" + i + ",";
            }

            insertString = insertString.Substring(0, insertString.Length - 1) + ");";

            SQLiteCommand command = new SQLiteCommand(insertString, con);
            
            for (int i = 0; i < columns.Length; i++)
            {
                command.Parameters.AddWithValue(columns[i], values[i]);
            }

            return command;
        }

        private void Sahis_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MuvekkilTuru.SelectedIndex == 0)
            {
                try
                {
                    if (Properties.Settings.Default.DatabaseFilePath != "" && File.Exists(Properties.Settings.Default.DatabaseFilePath))
                    {
                        string[] columns = new string[]
                        {
                            "MuvekkilNo",
                            "MuvekkilTuru",
                            "NoterIsmi",
                            "VekaletTarihi",
                            "VekYevmiyeNo",
                            "AhzuKabza",
                            "Feragat",
                            "Ibra",
                            "Sulh",
                            "Banka",
                            "Sube",
                            "IBANno",
                            "Adres",
                            "Telefon",
                            "Fax",
                            "Email",
                            "IsimSoyisim",
                            "TCKimlik"
                        };

                        object[] values = new object[]
                        {
                            MuvekkilNo.Text,
                            MuvekkilTuru.Text,
                            NoterIsmi.Text,
                            string.Format("{0:dd.MM.yyyy}", VekTarihi.SelectedDate),
                            VekYevNo.Text,
                            AhzuKabza.SelectedIndex,
                            Feragat.SelectedIndex,
                            Ibra.SelectedIndex,
                            Sulh.SelectedIndex,
                            Banka.Text,
                            Sube.Text,
                            IBANno.Text,
                            Adres.Text,
                            Telefon.Text,
                            Fax.Text,
                            Email.Text,
                            IsimSoyisim.Text,
                            TCKimlik.Text
                        };

                        bool basarili = false;

                        try
                        {
                            using (SQLiteConnection con = new SQLiteConnection($"Data Source={Properties.Settings.Default.DatabaseFilePath};"))
                            {
                                con.Open();
                                Generate_Insert_Command("MuvekkilSahis", columns, values, con).ExecuteNonQuery();

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
                                Temizle();
                            }
                            else
                            {
                                MessageBox.Show("Veritabanı girdisi yapılamadı.");
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
        }

        private void Sirket_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MuvekkilTuru.SelectedIndex == 1)
            {
                try
                {
                    if (Properties.Settings.Default.DatabaseFilePath != "" && File.Exists(Properties.Settings.Default.DatabaseFilePath))
                    {
                        string[] columns = new string[]
                        {
                            "MuvekkilNo",
                            "MuvekkilTuru",
                            "NoterIsmi",
                            "VekaletTarihi",
                            "VekYevmiyeNo",
                            "AhzuKabza",
                            "Feragat",
                            "Ibra",
                            "Sulh",
                            "Banka",
                            "Sube",
                            "IBANno",
                            "Adres",
                            "Telefon",
                            "Fax",
                            "Email",
                            "SirketTuru",
                            "SirketUnvan",
                            "VergiDairesi",
                            "VergiNo",
                            "MersisNo"
                        };

                        object[] values = new object[]
                        {
                            MuvekkilNo.Text,
                            MuvekkilTuru.Text,
                            NoterIsmi.Text,
                            string.Format("{0:dd.MM.yyyy}", VekTarihi.SelectedDate),
                            VekYevNo.Text,
                            AhzuKabza.SelectedIndex,
                            Feragat.SelectedIndex,
                            Ibra.SelectedIndex,
                            Sulh.SelectedIndex,
                            Banka.Text,
                            Sube.Text,
                            IBANno.Text,
                            Adres.Text,
                            Telefon.Text,
                            Fax.Text,
                            Email.Text,
                            SirketTuru.Text,
                            SirketUnvan.Text,
                            VergiDairesi.Text,
                            VergiNo.Text,
                            MersisNo.Text
                        };

                        bool basarili = false;

                        try
                        {
                            using (SQLiteConnection con = new SQLiteConnection($"Data Source={Properties.Settings.Default.DatabaseFilePath};"))
                            {
                                con.Open();
                                Generate_Insert_Command("MuvekkilSirket", columns, values, con).ExecuteNonQuery();

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
                                Temizle();
                            }
                            else
                            {
                                MessageBox.Show("Veritabanı girdisi yapılamadı.");
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
        }

        private void Temizle()
        {
            MuvekkilNo.Text = string.Empty;
            MuvekkilTuru.Text = null;
            NoterIsmi.Text = string.Empty;
            VekTarihi.Text = string.Empty;
            VekYevNo.Text = string.Empty;
            AhzuKabza.SelectedIndex = 1;
            Feragat.SelectedIndex = 1;
            Ibra.SelectedIndex = 1;
            Sulh.SelectedIndex = 1;
            Banka.Text = string.Empty;
            Sube.Text = string.Empty;
            IBANno.Text = string.Empty;
            Adres.Text = string.Empty;
            Telefon.Text = string.Empty;
            Fax.Text = string.Empty;
            Email.Text = string.Empty;
            IsimSoyisim.Text = string.Empty;
            TCKimlik.Text = string.Empty;
            SirketTuru.SelectedIndex = 1;
            SirketUnvan.Text = string.Empty;
            VergiDairesi.Text = string.Empty;
            VergiNo.Text = string.Empty;
            MersisNo.Text = string.Empty;
        }

        private void MuvekkilTuru_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MuvekkilTuru.SelectedIndex == 0)
            {
                //Şahıs seçili demektir
                SirketExpander.IsExpanded = false;
                SirketExpander.IsEnabled = false;
                SahisExpander.IsExpanded = true;
                SahisExpander.IsEnabled = true;
            }
            else if (MuvekkilTuru.SelectedIndex == 1)
            {
                //Şirket seçili demektir
                SahisExpander.IsExpanded = false;
                SahisExpander.IsEnabled = false;
                SirketExpander.IsExpanded = true;
                SirketExpander.IsEnabled = true;
            }
            else
            {
                //Boş demektir
                SahisExpander.IsExpanded = false;
                SahisExpander.IsEnabled = false;
                SirketExpander.IsExpanded = false;
                SirketExpander.IsEnabled = false;
            }
        }
    }
}
