using System;
using System.ComponentModel;
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

            VekTarihi.SelectedDate = DateTime.Now;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null && (DataContext as YeniMuvekkilEkleViewModel).GuncellemeModu && (DataContext as YeniMuvekkilEkleViewModel).Item != null)
            {
                IMuvekkil_tspAuto item = (DataContext as YeniMuvekkilEkleViewModel).Item;

                MuvekkilNo.Text = item.MuvekkilNo;
                NoterIsmi.Text = item.NoterIsmi;
                VekTarihi.SelectedDate = item.VekaletTarihi;
                VekYevNo.Text = item.VekYevmiyeNo;
                AhzuKabza.SelectedIndex = Convert.ToInt32(item.AhzuKabza);
                Feragat.SelectedIndex = Convert.ToInt32(item.Feragat);
                Ibra.SelectedIndex = Convert.ToInt32(item.Ibra);
                Sulh.SelectedIndex = Convert.ToInt32(item.Sulh);
                Banka.Text = item.Banka;
                Sube.Text = item.Sube;
                IBANno.Text = item.IBANno;
                Adres.Text = item.Adres;
                Telefon.Text = item.Telefon;
                Fax.Text = item.Fax;
                Email.Text = item.Email;
                if (item.GetType() == typeof(MuvekkilSahis))
                {
                    MuvekkilTuru.SelectedIndex = 0;
                    IsimSoyisim.Text = (item as MuvekkilSahis).IsimSoyisim;
                    TCKimlik.Text = (item as MuvekkilSahis).TCKimlik;
                }
                else if (item.GetType() == typeof(MuvekkilSirket))
                {
                    MuvekkilTuru.SelectedIndex = 1;
                    SirketTuru.Text = (item as MuvekkilSirket).SirketTuru;
                    SirketUnvan.Text = (item as MuvekkilSirket).SirketUnvan;
                    VergiDairesi.Text = (item as MuvekkilSirket).VergiDairesi;
                    VergiNo.Text = (item as MuvekkilSirket).VergiNo;
                    MersisNo.Text = (item as MuvekkilSirket).MersisNo;
                }
            }
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
            NoterIsmi.Text = string.Empty;
            VekTarihi.SelectedDate = DateTime.Now;
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

        public DateTime VekTarihi_Duzeltme()
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

    public class YeniMuvekkilEkleViewModel : INotifyPropertyChanged
    {
        private bool _guncellemeModu;
        private IMuvekkil_tspAuto _item;

        public YeniMuvekkilEkleViewModel(bool guncellemeModu, IMuvekkil_tspAuto item = null)
        {
            _guncellemeModu = guncellemeModu;
            _item = item;
        }

        public bool GuncellemeModu
        {
            get { return _guncellemeModu; }
            set
            {
                this.MutateVerbose(ref _guncellemeModu, value, RaisePropertyChanged());
            }
        }

        public IMuvekkil_tspAuto Item
        {
            get { return _item; }
            set
            {
                this.MutateVerbose(ref _item, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
