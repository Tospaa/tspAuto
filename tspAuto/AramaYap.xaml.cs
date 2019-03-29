using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;
using tspAuto.Domain;

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
                string[] columns = new string[]
                {
                    "MuvekkilNo",
                    "MuvekkilTuru",
                    "NoterIsmi",
                    "VekaletTarihi",
                    "VekYevmiyeNo",
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
                using (SQLiteConnection con = new SQLiteConnection($@"Data Source={Properties.Settings.Default.DatabaseFilePath}"))
                {
                    using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(MethodPack.Generate_Query_Command(AramaKutusu.Text, "MuvekkilSirket", columns, con)))
                    {
                        dataAdapter.Fill(dataSet);
                    }
                }

                MuvekkilSirketSonuc.ItemsSource = dataSet.Tables[0].DefaultView;
                MuvekkilSirketExpander.Header = $"Şirket Müvekkiller ({dataSet.Tables[0].DefaultView.Count.ToString()})";
            }
            catch (Exception ex)
            {
                MessageBox.Show("kötü kötü olduk\n\n" + ex.ToString());
            }
            finally
            {
                MuvekkilSirketSonuc.SelectedItem = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            dataSet = new DataSet();

            try
            {
                string[] columns = new string[]
                {
                    "MuvekkilNo",
                    "MuvekkilTuru",
                    "NoterIsmi",
                    "VekaletTarihi",
                    "VekYevmiyeNo",
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
                using (SQLiteConnection con = new SQLiteConnection($@"Data Source={Properties.Settings.Default.DatabaseFilePath}"))
                {
                    using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(MethodPack.Generate_Query_Command(AramaKutusu.Text, "MuvekkilSahis", columns, con)))
                    {
                        dataAdapter.Fill(dataSet);
                    }
                }

                MuvekkilSahisSonuc.ItemsSource = dataSet.Tables[0].DefaultView;
                MuvekkilSahisExpander.Header = $"Şahıs Müvekkiller ({dataSet.Tables[0].DefaultView.Count.ToString()})";
            }
            catch (Exception ex)
            {
                MessageBox.Show("kötü kötü olduk\n\n" + ex.ToString());
            }
            finally
            {
                MuvekkilSahisSonuc.SelectedItem = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
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
