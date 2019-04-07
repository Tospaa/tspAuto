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

        private async void DataGrid_RightClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = ((ContextMenu)(sender as MenuItem).Parent).PlacementTarget as DataGrid;
                DataRowView rowView = (DataRowView)dataGrid.SelectedItem;
                string tablo = dataGrid.Name;

                if (rowView != null)
                {
                    long girdiID = Convert.ToInt64(rowView["ID"]);

                    var view = new AramaYapDialog
                    {
                        DataContext = new AramaYapDialogViewModel(tablo, girdiID)
                    };

                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
                        //sil butonuna basılmış
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
                            Arama();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    string table = (sender as DataGrid).Name;
                    string column = e.Column.SortMemberPath;
                    long girdiID = Convert.ToInt64((e.Row.Item as DataRowView)["ID"]);

                    var view = new BenimDialog
                    {
                        DataContext = new BenimDialogViewModel("Bu değeri güncellemek istediğinizden emin misiniz?", "EVET", "HAYIR")
                    };

                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
                        if (e.EditingElement.GetType() == typeof(TextBox))
                        {
                            string newVal = (e.EditingElement as TextBox).Text;
                            string commandString = $"UPDATE {table} SET {column}=@newVal WHERE ID={girdiID}";

                            MethodPack.VeritabaniKodBlogu((con) => {
                                con.Open();
                                SQLiteCommand command = new SQLiteCommand(commandString, con);
                                command.Parameters.AddWithValue("newVal", newVal);
                                command.ExecuteNonQuery();
                            });
                        }
                        else if (e.EditingElement.GetType() == typeof(CheckBox))
                        {
                            bool newVal = (bool)(e.EditingElement as CheckBox).IsChecked;
                            string command = $"UPDATE {table} SET {column}={newVal} WHERE ID={girdiID}";

                            MethodPack.VeritabaniKodBlogu((con) => {
                                con.Open();
                                new SQLiteCommand(command, con).ExecuteNonQuery();
                            });
                        }
                    }
                    else
                    {
                        // TODO: Şurda tekrar arama yaptırma. Eski değere ulaş. Çöz.
                        e.Cancel = true;
                        Arama();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GuncellemeModuAcik.IsChecked = false;
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

    public class DateToStringConverter : System.Windows.Data.IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string newVal = string.Format("{0:dd.MM.yyyy}", value);
                return newVal;
            }
            catch
            {
                throw new InvalidCastException("Value can't be converted to string.");
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                DateTime? newVal = System.Convert.ToDateTime(value);
                return newVal;
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
