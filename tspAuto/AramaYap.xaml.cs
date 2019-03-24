using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;

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

        private SQLiteCommand Generate_Query_String(string table, string[] columns, SQLiteConnection con)
        {
            if (new Regex("[ğĞüÜşŞıİöÖçÇ]").Match(AramaKutusu.Text).Success)
            {
                string arama = AramaKutusu.Text;

                arama = new Regex("[ğĞ]").Replace(arama, "[ğĞ]");
                arama = new Regex("[üÜ]").Replace(arama, "[üÜ]");
                arama = new Regex("[şŞ]").Replace(arama, "[şŞ]");
                arama = new Regex("[ıiIİ]").Replace(arama, "[ıiIİ]");
                arama = new Regex("[öÖ]").Replace(arama, "[öÖ]");
                arama = new Regex("[çÇ]").Replace(arama, "[çÇ]");

                string queryString = $"SELECT * FROM {table} WHERE(";

                foreach (string i in columns)
                {
                    queryString += $"{i} REGEXP '{arama}' OR ";
                }

                queryString = queryString.Substring(0, queryString.Length - 4) + ")";

                SQLiteCommand command = new SQLiteCommand(queryString, con);

                //command.Parameters.AddWithValue("arama", arama);

                return command;
            }
            else
            {
                string queryString = $"SELECT * FROM {table} WHERE(";

                foreach (string i in columns)
                {
                    queryString += $"{i} LIKE @arama OR ";
                }

                queryString = queryString.Substring(0, queryString.Length - 4) + ")";

                SQLiteCommand command = new SQLiteCommand(queryString, con);

                command.Parameters.AddWithValue("arama", "%" + AramaKutusu.Text + "%");

                return command;
            }
        }

        public void Arama()
        {
            DataSet dataSet = new DataSet();

            try
            {
                string[] columns = new string[]
                {
                    "MuvekkilTuru",
                    "NoterIsmi",
                    "VekaletTarihi",
                    "VekYevmiyeNo",
                    "Banka",
                    "Sube",
                    "IBANno"
                };
                using (SQLiteConnection con = new SQLiteConnection($@"Data Source={Properties.Settings.Default.DatabaseFilePath}"))
                {
                    using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(Generate_Query_String("Muvekkil", columns, con)))
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
            finally
            {
                MuvekkilSonuc.SelectedItem = null;
            }
        }
    }
}
