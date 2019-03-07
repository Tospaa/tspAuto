using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

            TarihSec.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
        }

        SQLiteConnection con;
        SQLiteDataAdapter da;
        SQLiteCommand cmd;
        //string Properties. = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}\\{Properties.Resources.DatabaseFilePath}";

        private void Muvekkil_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!File.Exists(Properties.Settings.Default.DatabaseFilePath))
                {
                    SQLiteConnection.CreateFile(Properties.Settings.Default.DatabaseFilePath);

                    string sql = @"CREATE TABLE Student(
                            ID INTEGER PRIMARY KEY AUTOINCREMENT ,
                            FirstName           TEXT      NOT NULL,
                            LastName            TEXT       NOT NULL
                            );";
                    con = new SQLiteConnection($"Data Source={Properties.Settings.Default.DatabaseFilePath};");
                    try
                    {
                        con.Open();
                        cmd = new SQLiteCommand(sql, con);
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        MessageBox.Show("Veritabanı oluşturulamadı.");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("Bazı dosyalar silinmiş.");
            }
        }

        private void Sirket_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Properties.Settings.Default.DatabaseFilePath);
        }
    }
}
