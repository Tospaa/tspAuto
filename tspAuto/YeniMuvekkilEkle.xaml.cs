using System;
using System.Windows;
using System.Windows.Controls;
using tspAuto.Domain;
using tspAuto.Model;

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
            if (MuvekkilTuru.SelectedIndex == 0 && IsimSoyisim.Text != string.Empty)
            {
                try
                {
                    using (var db = new DbConnection())
                    {
                        var muvekkilsahis = new MuvekkilSahis
                        {
                            MuvekkilNo = MuvekkilNo.Text,
                            MuvekkilTuru = MuvekkilTuru.Text,
                            NoterIsmi = NoterIsmi.Text,
                            VekaletTarihi = VekTarihi_Duzeltme(),
                            VekYevmiyeNo = VekYevNo.Text,
                            AhzuKabza = Convert.ToBoolean(AhzuKabza.SelectedIndex),
                            Feragat = Convert.ToBoolean(Feragat.SelectedIndex),
                            Ibra = Convert.ToBoolean(Ibra.SelectedIndex),
                            Sulh = Convert.ToBoolean(Sulh.SelectedIndex),
                            Banka = Banka.Text,
                            Sube = Sube.Text,
                            IBANno = IBANno.Text,
                            Adres = Adres.Text,
                            Telefon = Telefon.Text,
                            Fax = Fax.Text,
                            Email = Email.Text,
                            IsimSoyisim = IsimSoyisim.Text,
                            TCKimlik = TCKimlik.Text
                        };

                        db.MuvekkilSahis_tt.Add(muvekkilsahis);
                        db.SaveChanges();
                    }

                    MessageBox.Show("Veritabanı girdisi başarılı.");
                    Temizle();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }

        private void Sirket_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MuvekkilTuru.SelectedIndex == 1 && SirketUnvan.Text != string.Empty)
            {
                try
                {
                    using (var db = new DbConnection())
                    {
                        var muvekkilsirket = new MuvekkilSirket
                        {
                            MuvekkilNo = MuvekkilNo.Text,
                            MuvekkilTuru = MuvekkilTuru.Text,
                            NoterIsmi = NoterIsmi.Text,
                            VekaletTarihi = VekTarihi_Duzeltme(),
                            VekYevmiyeNo = VekYevNo.Text,
                            AhzuKabza = Convert.ToBoolean(AhzuKabza.SelectedIndex),
                            Feragat = Convert.ToBoolean(Feragat.SelectedIndex),
                            Ibra = Convert.ToBoolean(Ibra.SelectedIndex),
                            Sulh = Convert.ToBoolean(Sulh.SelectedIndex),
                            Banka = Banka.Text,
                            Sube = Sube.Text,
                            IBANno = IBANno.Text,
                            Adres = Adres.Text,
                            Telefon = Telefon.Text,
                            Fax = Fax.Text,
                            Email = Email.Text,
                            SirketTuru = SirketTuru.Text,
                            SirketUnvan = SirketUnvan.Text,
                            VergiDairesi = VergiDairesi.Text,
                            VergiNo = VergiNo.Text,
                            MersisNo = MersisNo.Text
                        };

                        db.MuvekkilSirket_tt.Add(muvekkilsirket);
                        db.SaveChanges();
                    }

                    MessageBox.Show("Veritabanı girdisi başarılı.");
                    Temizle();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
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

        private DateTime VekTarihi_Duzeltme()
        {
            try
            {
                return (DateTime)VekTarihi.SelectedDate;
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
