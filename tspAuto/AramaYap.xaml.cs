using System;
using System.Windows;
using System.Windows.Controls;
using tspAuto.Domain;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Linq;
using tspAuto.Model;
using System.Data.Entity;
using System.Windows.Input;

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GuncellemeModuAcik.IsChecked = false;
            AramaKutusu.Focus();
            AramaKutusu.SelectAll();
        }

        private void AramaKutusu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AramaKutusu.Text.Length >= 1)
            {
                Arama();
            }
        }

        private string[] SecilenKolonlar(ItemCollection items)
        {
            List<string> columnList = new List<string>();

            foreach (var item in items)
            {
                if (item.GetType() == typeof(MenuItem) && (item as MenuItem).IsCheckable && (item as MenuItem).IsChecked)
                {
                    columnList.Add((item as MenuItem).Tag.ToString());
                }
            }

            return columnList.ToArray();
        }

        public void Arama()
        {
            string[] columns = SecilenKolonlar(MuvekkilSahisContextMenu.Items);
            if (columns.Length > 0)
            {
                try
                {
                    using (var db = new DbConnection())
                    {
                        // AsEnumerable() from: https://entityframework.net/linq-does-not-recognize-method
                        // StringComparison.CurrentCultureIgnoreCase from: https://stackoverflow.com/a/8879798
                        List<MuvekkilSahis> queryResult = db.MuvekkilSahis_tt
                            .AsEnumerable()
                            .Where(s => s.GetConcatenatedString(columns).IndexOf(AramaKutusu.Text, StringComparison.CurrentCultureIgnoreCase) != -1)
                            .ToList();
                        MuvekkilSahis_tt.ItemsSource = queryResult;
                        MuvekkilSahisExpander.Header = $"Şahıs Müvekkiller ({queryResult.Count.ToString()})";
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }

            MuvekkilSahis_tt.SelectedItem = null;

            columns = SecilenKolonlar(MuvekkilSirketContextMenu.Items);
            if (columns.Length > 0)
            {
                try
                {
                    using (var db = new DbConnection())
                    {
                        List<MuvekkilSirket> queryResult = db.MuvekkilSirket_tt
                            .AsEnumerable()
                            .Where(s => s.GetConcatenatedString(columns).IndexOf(AramaKutusu.Text, StringComparison.CurrentCultureIgnoreCase) != -1)
                            .ToList();
                        MuvekkilSirket_tt.ItemsSource = queryResult;
                        MuvekkilSirketExpander.Header = $"Şirket Müvekkiller ({queryResult.Count.ToString()})";
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }

            MuvekkilSirket_tt.SelectedItem = null;

            columns = SecilenKolonlar(DosyaIcraContextMenu.Items);
            if (columns.Length > 0)
            {
                try
                {
                    using (var db = new DbConnection())
                    {
                        //böyle include şekli yapmazsam o kolonlar boş kalıyor :( üzücü baya
                        List<DosyaIcra> queryResult = db.DosyaIcra_tt
                            .Include(s => s.Alacakli)
                            .Include(s => s.Borclu)
                            .Include(s => s.AlacakliVekil)
                            .Include(s => s.BorcluVekil)
                            .AsEnumerable()
                            .Where(s => s.GetConcatenatedString(columns).IndexOf(AramaKutusu.Text, StringComparison.CurrentCultureIgnoreCase) != -1)
                            .ToList();
                        DosyaIcra_tt.ItemsSource = queryResult;
                        DosyaIcraExpander.Header = $"İcra Dosyaları ({queryResult.Count.ToString()})";
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }

            DosyaIcra_tt.SelectedItem = null;

            columns = SecilenKolonlar(DosyaDavaContextMenu.Items);
            if (columns.Length > 0)
            {
                try
                {
                    using (var db = new DbConnection())
                    {
                        List<DosyaDava> queryResult = db.DosyaDava_tt
                            .Include(s => s.Davaci)
                            .Include(s => s.Davali)
                            .Include(s => s.DavaciVekil)
                            .Include(s => s.DavaliVekil)
                            .AsEnumerable()
                            .Where(s => s.GetConcatenatedString(columns).IndexOf(AramaKutusu.Text, StringComparison.CurrentCultureIgnoreCase) != -1)
                            .ToList();
                        DosyaDava_tt.ItemsSource = queryResult;
                        DosyaDavaExpander.Header = $"Dava Dosyaları ({queryResult.Count.ToString()})";
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }

            DosyaDava_tt.SelectedItem = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private async void DataGrid_ContextMenu_SilButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = ((ContextMenu)(sender as MenuItem).Parent).PlacementTarget as DataGrid;
                IData_tspAuto item = (IData_tspAuto)dataGrid.SelectedItem;
                string tablo = dataGrid.Name;

                if (item != null)
                {
                    var view = new AramaYapDialog
                    {
                        DataContext = new AramaYapDialogViewModel(item)
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
                            try
                            {
                                using (var db = new DbConnection())
                                {
                                    db.Database.ExecuteSqlCommand("DELETE FROM dbo." + tablo + " WHERE ID={0}", item.ID);
                                    db.SaveChanges();
                                }
                            }
                            catch (Exception ex) { MessageBox.Show("Silme işlemi esnasında bir hata oluştu.\n\n" + ex.ToString()); }
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

        private void ContextMenu_TumunuSec_Click(object sender, RoutedEventArgs e)
        {
            ItemCollection items = ((MenuItem)(sender as MenuItem).Parent).Items;

            foreach (var item in items)
            {
                if (item.GetType() == typeof(MenuItem) && (item as MenuItem).IsCheckable)
                {
                    (item as MenuItem).IsChecked = true;
                }
            }
        }

        private void ContextMenu_SecimleriKaldir_Click(object sender, RoutedEventArgs e)
        {
            ItemCollection items = ((MenuItem)(sender as MenuItem).Parent).Items;

            foreach (var item in items)
            {
                if (item.GetType() == typeof(MenuItem) && (item as MenuItem).IsCheckable)
                {
                    (item as MenuItem).IsChecked = false;
                }
            }
        }

        private async void DosyaIslemEkle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = ((ContextMenu)(sender as MenuItem).Parent).PlacementTarget as DataGrid;
                IDosya_tspAuto item = (IDosya_tspAuto)dataGrid.SelectedItem;
                string tablo = dataGrid.Name;

                if (item != null)
                {
                    var view = new DosyaIslemEkleDialog();

                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
                        try
                        {
                            using (var db = new DbConnection())
                            {
                                if (tablo == "DosyaDava_tt")
                                {
                                    DosyaDava dosya = db.DosyaDava_tt.FirstOrDefault(s => s.ID == item.ID);
                                    dosya.Log = dosya.Log + "\n[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "] " + view.EklenecekIslem.Text;
                                    db.SaveChanges();
                                }
                                else if (tablo == "DosyaIcra_tt")
                                {
                                    DosyaIcra dosya = db.DosyaIcra_tt.FirstOrDefault(s => s.ID == item.ID);
                                    dosya.Log = dosya.Log + "\n[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "] " + view.EklenecekIslem.Text;
                                    db.SaveChanges();
                                }
                            }
                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private async void MuvekkilSahis_tt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GuncellemeModuAcik.IsChecked)
            {
                DataGrid dataGrid = sender as DataGrid;
                MuvekkilSahis item = (MuvekkilSahis)dataGrid.SelectedItem;

                if (item != null)
                {
                    var view = new DialogWithContentControl(new YeniMuvekkilEkle { DataContext = new YeniMuvekkilEkleViewModel(true, item) });
                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
                        try
                        {
                            using (var db = new DbConnection())
                            {
                                MuvekkilSahis guncellenecekGirdi = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == item.ID);

                                YeniMuvekkilEkle YMEInstance = view.Icerik.Content as YeniMuvekkilEkle;

                                guncellenecekGirdi.MuvekkilNo = YMEInstance.MuvekkilNo.Text;
                                guncellenecekGirdi.MuvekkilTuru = YMEInstance.MuvekkilTuru.Text;
                                guncellenecekGirdi.NoterIsmi = YMEInstance.NoterIsmi.Text;
                                guncellenecekGirdi.VekaletTarihi = YMEInstance.VekTarihi_Duzeltme();
                                guncellenecekGirdi.VekYevmiyeNo = YMEInstance.VekYevNo.Text;
                                guncellenecekGirdi.AhzuKabza = Convert.ToBoolean(YMEInstance.AhzuKabza.SelectedIndex);
                                guncellenecekGirdi.Feragat = Convert.ToBoolean(YMEInstance.Feragat.SelectedIndex);
                                guncellenecekGirdi.Ibra = Convert.ToBoolean(YMEInstance.Ibra.SelectedIndex);
                                guncellenecekGirdi.Sulh = Convert.ToBoolean(YMEInstance.Sulh.SelectedIndex);
                                guncellenecekGirdi.Banka = YMEInstance.Banka.Text;
                                guncellenecekGirdi.Sube = YMEInstance.Sube.Text;
                                guncellenecekGirdi.IBANno = YMEInstance.IBANno.Text;
                                guncellenecekGirdi.Adres = YMEInstance.Adres.Text;
                                guncellenecekGirdi.Telefon = YMEInstance.Telefon.Text;
                                guncellenecekGirdi.Fax = YMEInstance.Fax.Text;
                                guncellenecekGirdi.Email = YMEInstance.Email.Text;
                                guncellenecekGirdi.IsimSoyisim = YMEInstance.IsimSoyisim.Text;
                                guncellenecekGirdi.TCKimlik = YMEInstance.TCKimlik.Text;

                                db.SaveChanges();
                            }
                            MessageBox.Show("Güncelleme başarılı.");
                            Arama();
                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    }
                }
            }
        }

        private async void MuvekkilSirket_tt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GuncellemeModuAcik.IsChecked)
            {
                DataGrid dataGrid = sender as DataGrid;
                MuvekkilSirket item = (MuvekkilSirket)dataGrid.SelectedItem;

                if (item != null)
                {
                    var view = new DialogWithContentControl(new YeniMuvekkilEkle { DataContext = new YeniMuvekkilEkleViewModel(true, item) });
                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
                        try
                        {
                            using (var db = new DbConnection())
                            {
                                MuvekkilSirket guncellenecekGirdi = db.MuvekkilSirket_tt.FirstOrDefault(s => s.ID == item.ID);

                                YeniMuvekkilEkle YMEInstance = view.Icerik.Content as YeniMuvekkilEkle;

                                guncellenecekGirdi.MuvekkilNo = YMEInstance.MuvekkilNo.Text;
                                guncellenecekGirdi.MuvekkilTuru = YMEInstance.MuvekkilTuru.Text;
                                guncellenecekGirdi.NoterIsmi = YMEInstance.NoterIsmi.Text;
                                guncellenecekGirdi.VekaletTarihi = YMEInstance.VekTarihi_Duzeltme();
                                guncellenecekGirdi.VekYevmiyeNo = YMEInstance.VekYevNo.Text;
                                guncellenecekGirdi.AhzuKabza = Convert.ToBoolean(YMEInstance.AhzuKabza.SelectedIndex);
                                guncellenecekGirdi.Feragat = Convert.ToBoolean(YMEInstance.Feragat.SelectedIndex);
                                guncellenecekGirdi.Ibra = Convert.ToBoolean(YMEInstance.Ibra.SelectedIndex);
                                guncellenecekGirdi.Sulh = Convert.ToBoolean(YMEInstance.Sulh.SelectedIndex);
                                guncellenecekGirdi.Banka = YMEInstance.Banka.Text;
                                guncellenecekGirdi.Sube = YMEInstance.Sube.Text;
                                guncellenecekGirdi.IBANno = YMEInstance.IBANno.Text;
                                guncellenecekGirdi.Adres = YMEInstance.Adres.Text;
                                guncellenecekGirdi.Telefon = YMEInstance.Telefon.Text;
                                guncellenecekGirdi.Fax = YMEInstance.Fax.Text;
                                guncellenecekGirdi.Email = YMEInstance.Email.Text;
                                guncellenecekGirdi.SirketTuru = YMEInstance.SirketTuru.Text;
                                guncellenecekGirdi.SirketUnvan = YMEInstance.SirketUnvan.Text;
                                guncellenecekGirdi.VergiDairesi = YMEInstance.VergiDairesi.Text;
                                guncellenecekGirdi.VergiNo = YMEInstance.VergiNo.Text;
                                guncellenecekGirdi.MersisNo = YMEInstance.MersisNo.Text;

                                db.SaveChanges();
                            }
                            MessageBox.Show("Güncelleme başarılı.");
                            Arama();
                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    }
                }
            }
        }

        private async void DosyaIcra_tt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GuncellemeModuAcik.IsChecked)
            {
                DataGrid dataGrid = sender as DataGrid;
                DosyaIcra item = (DosyaIcra)dataGrid.SelectedItem;

                if (item != null)
                {
                    var view = new DialogWithContentControl(new YeniDosyaEkle { DataContext = new YeniDosyaEkleViewModel(true, item) });
                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
                        try
                        {
                            using (var db = new DbConnection())
                            {
                                DosyaIcra guncellenecekGirdi = db.DosyaIcra_tt.FirstOrDefault(s => s.ID == item.ID);

                                YeniDosyaEkle YDEInstance = view.Icerik.Content as YeniDosyaEkle;

                                guncellenecekGirdi.DosyaTuru = YDEInstance.DosyaTuru.Text;
                                guncellenecekGirdi.DosyaNo = YDEInstance.DosyaNo.Text;
                                guncellenecekGirdi.ArsivNo = YDEInstance.ArsivNo.Text;
                                guncellenecekGirdi.Alacakli = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)YDEInstance.IsimSoyisim_1.Tag);
                                guncellenecekGirdi.AlacakliVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)YDEInstance.IsimSoyisim_1_Vekil.Tag);
                                guncellenecekGirdi.Borclu = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)YDEInstance.IsimSoyisim_2.Tag);
                                guncellenecekGirdi.BorcluVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)YDEInstance.IsimSoyisim_2_Vekil.Tag);
                                guncellenecekGirdi.IcraDairesi = YDEInstance.IcraDairesi.Text;

                                db.SaveChanges();
                            }
                            MessageBox.Show("Güncelleme başarılı.");
                            Arama();
                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    }
                }
            }
        }

        private async void DosyaDava_tt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GuncellemeModuAcik.IsChecked)
            {
                DataGrid dataGrid = sender as DataGrid;
                DosyaDava item = (DosyaDava)dataGrid.SelectedItem;

                if (item != null)
                {
                    var view = new DialogWithContentControl(new YeniDosyaEkle { DataContext = new YeniDosyaEkleViewModel(true, item) });
                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
                        try
                        {
                            using (var db = new DbConnection())
                            {
                                DosyaDava guncellenecekGirdi = db.DosyaDava_tt.FirstOrDefault(s => s.ID == item.ID);

                                YeniDosyaEkle YDEInstance = view.Icerik.Content as YeniDosyaEkle;

                                guncellenecekGirdi.DosyaTuru = YDEInstance.DosyaTuru.Text;
                                guncellenecekGirdi.DosyaNo = YDEInstance.DosyaNo.Text;
                                guncellenecekGirdi.ArsivNo = YDEInstance.ArsivNo.Text;
                                guncellenecekGirdi.Davaci = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)YDEInstance.IsimSoyisim_1.Tag);
                                guncellenecekGirdi.DavaciVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)YDEInstance.IsimSoyisim_1_Vekil.Tag);
                                guncellenecekGirdi.Davali = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)YDEInstance.IsimSoyisim_2.Tag);
                                guncellenecekGirdi.DavaliVekil = db.MuvekkilSahis_tt.FirstOrDefault(s => s.ID == (int?)YDEInstance.IsimSoyisim_2_Vekil.Tag);
                                guncellenecekGirdi.Durum = YDEInstance.Durum.Text;
                                guncellenecekGirdi.DavaTuru = YDEInstance.DavaTuru.Text;
                                guncellenecekGirdi.Mahkeme = YDEInstance.Mahkeme.Text;
                                guncellenecekGirdi.DavaKonusu = YDEInstance.Konusu.Text;

                                db.SaveChanges();
                            }
                            MessageBox.Show("Güncelleme başarılı.");
                            Arama();
                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    }
                }
            }
        }
    }
}
