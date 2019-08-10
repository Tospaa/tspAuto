using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using tspAuto.Domain;
using tspAuto.Model;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for YeniDosyaEkle.xaml
    /// </summary>
    public partial class YeniDosyaEkle : UserControl
    {
        public YeniDosyaEkle()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null && (DataContext as YeniDosyaEkleViewModel).GuncellemeModu && (DataContext as YeniDosyaEkleViewModel).Item != null)
            {
                IDosya_tspAuto item = (DataContext as YeniDosyaEkleViewModel).Item;

                DosyaNo.Text = item.DosyaNo;
                ArsivNo.Text = item.ArsivNo;
                if (item.GetType().BaseType == typeof(DosyaDava))
                {
                    DosyaTuru.SelectedIndex = 0;
                    IsimSoyisim_1.Tag = (item as DosyaDava).Davaci.ID;
                    IsimSoyisim_1.Text = (item as DosyaDava).Davaci.IsimSoyisim;
                    TCKimlikNo_1.Text = (item as DosyaDava).Davaci.TCKimlik;
                    Adres_1.Text = (item as DosyaDava).Davaci.Adres;
                    if ((item as DosyaDava).DavaciVekil != null)
                    {
                        IsimSoyisim_1_Vekil.Tag = (item as DosyaDava).DavaciVekil.ID;
                        IsimSoyisim_1_Vekil.Text = (item as DosyaDava).DavaciVekil.IsimSoyisim;
                        TCKimlikNo_1_Vekil.Text = (item as DosyaDava).DavaciVekil.TCKimlik;
                        Adres_1_Vekil.Text = (item as DosyaDava).DavaciVekil.Adres;
                    }
                    IsimSoyisim_2.Tag = (item as DosyaDava).Davali.ID;
                    IsimSoyisim_2.Text = (item as DosyaDava).Davali.IsimSoyisim;
                    TCKimlikNo_2.Text = (item as DosyaDava).Davali.TCKimlik;
                    Adres_2.Text = (item as DosyaDava).Davali.Adres;
                    if ((item as DosyaDava).DavaliVekil != null)
                    {
                        IsimSoyisim_2_Vekil.Tag = (item as DosyaDava).DavaliVekil.ID;
                        IsimSoyisim_2_Vekil.Text = (item as DosyaDava).DavaliVekil.IsimSoyisim;
                        TCKimlikNo_2_Vekil.Text = (item as DosyaDava).DavaliVekil.TCKimlik;
                        Adres_2_Vekil.Text = (item as DosyaDava).DavaliVekil.Adres;
                    }
                    Durum.Text = (item as DosyaDava).Durum;
                    DavaTuru.Text = (item as DosyaDava).DavaTuru;
                    Mahkeme.Text = (item as DosyaDava).Mahkeme;
                    Konusu.Text = (item as DosyaDava).DavaKonusu;
                }
                else if (item.GetType().BaseType == typeof(DosyaIcra))
                {
                    DosyaTuru.SelectedIndex = 1;
                    IsimSoyisim_1.Tag = (item as DosyaIcra).Alacakli.ID;
                    IsimSoyisim_1.Text = (item as DosyaIcra).Alacakli.IsimSoyisim;
                    TCKimlikNo_1.Text = (item as DosyaIcra).Alacakli.TCKimlik;
                    Adres_1.Text = (item as DosyaIcra).Alacakli.Adres;
                    if ((item as DosyaIcra).AlacakliVekil != null)
                    {
                        IsimSoyisim_1_Vekil.Tag = (item as DosyaIcra).AlacakliVekil.ID;
                        IsimSoyisim_1_Vekil.Text = (item as DosyaIcra).AlacakliVekil.IsimSoyisim;
                        TCKimlikNo_1_Vekil.Text = (item as DosyaIcra).AlacakliVekil.TCKimlik;
                        Adres_1_Vekil.Text = (item as DosyaIcra).AlacakliVekil.Adres;
                    }
                    IsimSoyisim_2.Tag = (item as DosyaIcra).Borclu.ID;
                    IsimSoyisim_2.Text = (item as DosyaIcra).Borclu.IsimSoyisim;
                    TCKimlikNo_2.Text = (item as DosyaIcra).Borclu.TCKimlik;
                    Adres_2.Text = (item as DosyaIcra).Borclu.Adres;
                    if ((item as DosyaIcra).BorcluVekil != null)
                    {
                        IsimSoyisim_2_Vekil.Tag = (item as DosyaIcra).BorcluVekil.ID;
                        IsimSoyisim_2_Vekil.Text = (item as DosyaIcra).BorcluVekil.IsimSoyisim;
                        TCKimlikNo_2_Vekil.Text = (item as DosyaIcra).BorcluVekil.TCKimlik;
                        Adres_2_Vekil.Text = (item as DosyaIcra).BorcluVekil.Adres;
                    }
                    IcraDairesi.Text = (item as DosyaIcra).IcraDairesi;
                }
            }
        }

        private WorkModes workMode;

        private void Getir_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == "Getir1") { workMode = WorkModes.Filling_1; }
            else if ((sender as Button).Name == "Getir1_Vekil") { workMode = WorkModes.Filling_1_Vekil; }
            else if ((sender as Button).Name == "Getir2") { workMode = WorkModes.Filling_2; }
            else if ((sender as Button).Name == "Getir2_Vekil") { workMode = WorkModes.Filling_2_Vekil; }

            Icerik.Content = new AramaYap();
        }

        private void YeniDosyaEkleDialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return; // çok coolum ya mk xD

            try
            {
                Doldur((Icerik.Content as AramaYap).MuvekkilSahis_tt.SelectedItem as MuvekkilSahis);
            }
            catch (NullReferenceException) { return; }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }

        private void Doldur(MuvekkilSahis sahis)
        {
            if (workMode == WorkModes.Filling_1)
            {
                IsimSoyisim_1.Tag = sahis.ID;
                IsimSoyisim_1.Text = sahis.IsimSoyisim;
                TCKimlikNo_1.Text = sahis.TCKimlik;
                Adres_1.Text = sahis.Adres;
            }
            else if (workMode == WorkModes.Filling_1_Vekil)
            {
                IsimSoyisim_1_Vekil.Tag = sahis.ID;
                IsimSoyisim_1_Vekil.Text = sahis.IsimSoyisim;
                TCKimlikNo_1_Vekil.Text = sahis.TCKimlik;
                Adres_1_Vekil.Text = sahis.Adres;
            }
            else if (workMode == WorkModes.Filling_2)
            {
                IsimSoyisim_2.Tag = sahis.ID;
                IsimSoyisim_2.Text = sahis.IsimSoyisim;
                TCKimlikNo_2.Text = sahis.TCKimlik;
                Adres_2.Text = sahis.Adres;
            }
            else if (workMode == WorkModes.Filling_2_Vekil)
            {
                IsimSoyisim_2_Vekil.Tag = sahis.ID;
                IsimSoyisim_2_Vekil.Text = sahis.IsimSoyisim;
                TCKimlikNo_2_Vekil.Text = sahis.TCKimlik;
                Adres_2_Vekil.Text = sahis.Adres;
            }
        }

        private void DosyaDavaEkle_Click(object sender, RoutedEventArgs e)
        {
            if (IsimSoyisim_1.Tag != null && IsimSoyisim_2.Tag != null)
            {
                try
                {
                    using (var db = new DbConnection())
                    {
                        DosyaDava dosyaDava = new DosyaDava
                        {
                            DosyaTuru = DosyaTuru.Text,
                            DosyaNo = DosyaNo.Text,
                            ArsivNo = ArsivNo.Text,
                            Davaci = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_1.Tag),
                            DavaciVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_1_Vekil.Tag),
                            Davali = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_2.Tag),
                            DavaliVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_2_Vekil.Tag),
                            Durum = Durum.Text,
                            DavaTuru = DavaTuru.Text,
                            Mahkeme = Mahkeme.Text,
                            DavaKonusu = Konusu.Text,
                            Log = $"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}] Dosya oluşturuldu."
                        };

                        db.DosyaDava_tt.Add(dosyaDava);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Veritabanı girdisi başarılı.");
                    Temizle();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }

        private void DosyaIcraEkle_Click(object sender, RoutedEventArgs e)
        {
            if (IsimSoyisim_1.Tag != null && IsimSoyisim_2.Tag != null)
            {
                try
                {
                    using (var db = new DbConnection())
                    {
                        DosyaIcra dosyaIcra = new DosyaIcra
                        {
                            DosyaTuru = DosyaTuru.Text,
                            DosyaNo = DosyaNo.Text,
                            ArsivNo = ArsivNo.Text,
                            Alacakli = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_1.Tag),
                            AlacakliVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_1_Vekil.Tag),
                            Borclu = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_2.Tag),
                            BorcluVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)IsimSoyisim_2_Vekil.Tag),
                            IcraDairesi = IcraDairesi.Text,
                            Log = $"[{DateTime.Now.ToString("dd/MM/yyyy HH:mm")}] Dosya oluşturuldu."
                        };

                        db.DosyaIcra_tt.Add(dosyaIcra);
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
            DosyaNo.Text = string.Empty;
            ArsivNo.Text = string.Empty;
            IsimSoyisim_1.Tag = null;
            IsimSoyisim_1.Text = string.Empty;
            TCKimlikNo_1.Text = string.Empty;
            Adres_1.Text = string.Empty;
            IsimSoyisim_1_Vekil.Tag = null;
            IsimSoyisim_1_Vekil.Text = string.Empty;
            TCKimlikNo_1_Vekil.Text = string.Empty;
            Adres_1_Vekil.Text = string.Empty;
            IsimSoyisim_2.Tag = null;
            IsimSoyisim_2.Text = string.Empty;
            TCKimlikNo_2.Text = string.Empty;
            Adres_2.Text = string.Empty;
            IsimSoyisim_2_Vekil.Tag = null;
            IsimSoyisim_2_Vekil.Text = string.Empty;
            TCKimlikNo_2_Vekil.Text = string.Empty;
            Adres_2_Vekil.Text = string.Empty;
            Durum.SelectedIndex = 0;
            DavaTuru.SelectedIndex = 0;
            Mahkeme.SelectedIndex = 0;
            Konusu.Text = string.Empty;
            IcraDairesi.Text = string.Empty;
        }
    }

    public enum WorkModes { Filling_1, Filling_1_Vekil, Filling_2, Filling_2_Vekil }

    public class YeniDosyaEkleViewModel : INotifyPropertyChanged
    {
        private bool _guncellemeModu;
        private IDosya_tspAuto _item;

        public YeniDosyaEkleViewModel(bool guncellemeModu, IDosya_tspAuto item = null)
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

        public IDosya_tspAuto Item
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
