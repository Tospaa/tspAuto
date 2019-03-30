using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;
using tspAuto.Domain;
using MaterialDesignThemes.Wpf;

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

            MethodPack.VeritabaniKodBlogu((con) => {
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(MethodPack.Generate_Query_Command(AramaKutusu.Text, "MuvekkilSirket", columns, con)))
                {
                    dataAdapter.Fill(dataSet);
                }

                MuvekkilSirket.ItemsSource = dataSet.Tables[0].DefaultView;
                MuvekkilSirketExpander.Header = $"Şirket Müvekkiller ({dataSet.Tables[0].DefaultView.Count.ToString()})";
            });

            dataSet = new DataSet();

            columns = new string[]
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

            MethodPack.VeritabaniKodBlogu((con) => {
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(MethodPack.Generate_Query_Command(AramaKutusu.Text, "MuvekkilSahis", columns, con)))
                {
                    dataAdapter.Fill(dataSet);
                }

                MuvekkilSahis.ItemsSource = dataSet.Tables[0].DefaultView;
                MuvekkilSahisExpander.Header = $"Şahıs Müvekkiller ({dataSet.Tables[0].DefaultView.Count.ToString()})";
            });
        }

        private async void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DataRowView rowView = (DataRowView)(sender as DataGrid).SelectedItem;
                string tablo = (sender as DataGrid).Name;

                if (rowView != null)
                {
                    int girdiID = Convert.ToInt32(rowView["ID"]);

                    var view = new AramaYapDialog
                    {
                        DataContext = new AramaYapDialogViewModel(tablo, girdiID)
                    };

                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToInt32(result) == 1)
                    {
                        MessageBox.Show("güncelleyi seçtin");
                    }
                    else if (Convert.ToInt32(result) == 2)
                    {
                        var view_ = new BenimDialog
                        {
                            DataContext = new BenimDialogViewModel("Bu veritabanı girdisini silmek istediğinize emin misiniz?\n\nBu işlem geri alınamaz.",
                            "EVET",
                            "HAYIR")
                        };

                        result = await DialogHost.Show(view_, "RootDialog");

                        if (Convert.ToBoolean(result))
                        {
                            MethodPack.VeritabaniKodBlogu((con) => {
                                con.Open();
                                new SQLiteCommand($"DELETE FROM {tablo} WHERE ID={girdiID};", con).ExecuteNonQuery();
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
