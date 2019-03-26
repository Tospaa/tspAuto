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
                // (?i) ignores case sensitivity
                string arama = AramaKutusu.Text;

                arama = new Regex("[ğĞ]").Replace(arama, "[ğĞ]");
                arama = new Regex("[üÜ]").Replace(arama, "[üÜ]");
                arama = new Regex("[şŞ]").Replace(arama, "[şŞ]");
                arama = new Regex("[ıI]").Replace(arama, "[ıI]");
                arama = new Regex("[iİ]").Replace(arama, "[iİ]");
                arama = new Regex("[öÖ]").Replace(arama, "[öÖ]");
                arama = new Regex("[çÇ]").Replace(arama, "[çÇ]");

                arama = "(?i)" + arama;

                string queryString = $"SELECT * FROM {table} WHERE(";

                foreach (string i in columns)
                {
                    queryString += $"{i} REGEXP @arama OR ";
                }

                queryString = queryString.Substring(0, queryString.Length - 4) + ")";

                SQLiteCommand command = new SQLiteCommand(queryString, con);

                command.Parameters.AddWithValue("arama", arama);

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

    // from https://stackoverflow.com/questions/172735/create-use-user-defined-functions-in-system-data-sqlite
    // taken from http://sqlite.phxsoftware.com/forums/p/348/1457.aspx#1457
    [SQLiteFunction(Name = "REGEXP", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class RegExSQLiteFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            return Regex.IsMatch(Convert.ToString(args[1]), Convert.ToString(args[0]));
        }
    }
}
