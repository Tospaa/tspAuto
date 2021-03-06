﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using tspAuto.Domain;
using tspAuto.Model;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for YeniIsEkle.xaml
    /// </summary>
    public partial class YeniIsEkle : UserControl
    {
        public YeniIsEkle()
        {
            InitializeComponent();

            TarihSec.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
            SaatSec.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
            HatirlaticiTarihSec.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
            HatirlaticiSaatSec.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");

            TarihSec.SelectedDate = DateTime.Now;
            SaatSec.SelectedTime = DateTime.Now;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DbConnection())
                {
                    List<Kullanici> kullanicilar = db.Kullanicilar.Where(s => s.Yetki != Yetkiler.Yonetici).ToList();
                    IlgiliKisiComBox.ItemsSource = kullanicilar;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private async void KaydetButon_Click(object sender, RoutedEventArgs e)
        {
            #region Veritabanı
            DateTime isbitistarih = Convert.ToDateTime(TarihSec.SelectedDate);
            DateTime isbitissaat = Convert.ToDateTime(SaatSec.SelectedTime);
            if (yerelDosya != null && IlgiliKisiComBox.SelectedItem != null && IlgiliKisiComBox.SelectedItem.GetType() == typeof(Kullanici))
            {
                try
                {
                    using (var db = new DbConnection())
                    {
                        IsModel yeniIs = new IsModel
                        {
                            Baslik = Baslik.Text,
                            Aciklama = Aciklama.Text,
                            IsTuru = IsTuru.Text,
                            BitisTarihi = new DateTime(isbitistarih.Year, isbitistarih.Month, isbitistarih.Day, isbitissaat.Hour, isbitissaat.Minute, 0, DateTimeKind.Local),
                            IlgiliKisi = db.Kullanicilar.First(s => s.ID == ((Kullanici)IlgiliKisiComBox.SelectedItem).ID),
                            DosyaTuru = yerelDosya.DosyaTuru,
                            DosyaID = yerelDosya.ID
                        };

                        db.Isler.Add(yeniIs);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Veritabanı girdisi başarılı.");

                    #region Hatırlatıcı
                    if ((bool)HatirlaticiEklensin.IsChecked)
                    {
                        DateTime trh = Convert.ToDateTime(HatirlaticiTarihSec.SelectedDate);
                        DateTime st = Convert.ToDateTime(HatirlaticiSaatSec.SelectedTime);

                        DateTime tarih = new DateTime(trh.Year, trh.Month, trh.Day, st.Hour, st.Minute, 0, DateTimeKind.Local);

                        var view = new BenimDialog
                        {
                            DataContext = new BenimDialogViewModel($"Hatırlatıcı için seçilen zaman:\n{tarih.ToString("dd MMMM yyyy dddd HH:mm")}\nDevam etmek istiyor musunuz?",
                            "EVET",
                            "HAYIR")
                        };

                        var result = await DialogHost.Show(view, "RootDialog");

                        if (Convert.ToBoolean(result))
                        {
                            try
                            {
                                using (var db = new DbConnection())
                                {
                                    int isEntryID = db.Isler.OrderByDescending(s => s.ID).FirstOrDefault().ID;

                                    MethodPack.YeniHatirlatici(Baslik.Text, Aciklama.Text, tarih, ((Kullanici)IlgiliKisiComBox.SelectedItem).ID, isEntryID, "dbo.Isler");
                                }
                            }
                            catch (Exception ex) { MessageBox.Show("Hatırlatıcı eklemede bir hata oluştu.\n\n" + ex.ToString()); }
                        }
                    }
                    #endregion

                    Temizle();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
            #endregion
        }

        private void Temizle()
        {
            yerelDosya = null;
            Baslik.Text = string.Empty;
            Aciklama.Text = string.Empty;
            IsTuru.SelectedIndex = 0;
            TarihSec.SelectedDate = DateTime.Now;
            SaatSec.SelectedTime = DateTime.Now;
            SeciliDosyaNoEtiket.Visibility = Visibility.Collapsed;
        }

        private void DosyaSecButon_Click(object sender, RoutedEventArgs e)
        {
            Icerik.Content = new AramaYap();
        }

        IDosya_tspAuto yerelDosya = null;

        private void YeniIsEkleDialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return; // çok coolum ya mk xD

            if ((Icerik.Content as AramaYap).DosyaIcra_tt.SelectedItem != null)
            {
                yerelDosya = (Icerik.Content as AramaYap).DosyaIcra_tt.SelectedItem as DosyaIcra;

                SeciliDosyaNoEtiket.Text = "Seçili Dosya No: " + yerelDosya.DosyaNo.ToString();
                SeciliDosyaNoEtiket.Visibility = Visibility.Visible;
            }
            else if ((Icerik.Content as AramaYap).DosyaDava_tt.SelectedItem != null)
            {
                yerelDosya = (Icerik.Content as AramaYap).DosyaDava_tt.SelectedItem as DosyaDava;

                SeciliDosyaNoEtiket.Text = "Seçili Dosya No: " + yerelDosya.DosyaNo.ToString();
                SeciliDosyaNoEtiket.Visibility = Visibility.Visible;
            }
        }
    }
}
