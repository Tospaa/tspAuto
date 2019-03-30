using System;
using System.Windows;
using System.Windows.Controls;
using tspAuto.Domain;

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

        private void Sahis_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MuvekkilTuru.SelectedIndex == 0)
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

                MethodPack.VeritabaniKodBlogu((con) => {
                    con.Open();
                    MethodPack.Generate_Insert_Command("MuvekkilSahis", columns, values, con).ExecuteNonQuery();

                    basarili = true;
                });

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

        private void Sirket_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MuvekkilTuru.SelectedIndex == 1)
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

                MethodPack.VeritabaniKodBlogu((con) => {
                    con.Open();
                    MethodPack.Generate_Insert_Command("MuvekkilSirket", columns, values, con).ExecuteNonQuery();

                    basarili = true;
                });

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

        private void Temizle()
        {
            MuvekkilNo.Text = string.Empty;
            MuvekkilTuru.SelectedIndex = 0;
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
    }

    public class BooleanInverseConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var newVal = System.Convert.ToBoolean(value);
                return !newVal;
            }
            catch
            {
                throw new InvalidCastException("Value can't be converted to boolean.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var newVal = System.Convert.ToBoolean(value);
                return !newVal;
            }
            catch
            {
                throw new InvalidCastException("Value can't be converted to boolean.");
            }
        }
    }
}
