using System;
using System.Windows;
using System.Windows.Controls;
using tspAuto.Domain;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Linq;
using tspAuto.Model;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;

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

        private string[] SecilenKolonlar(ItemCollection items)
        {
            List<string> columnList = new List<string>();

            foreach (var item in items)
            {
                if (item.GetType() == typeof(MenuItem))
                {
                    if ((item as MenuItem).IsCheckable)
                    {
                        if ((item as MenuItem).IsChecked)
                        {
                            columnList.Add((item as MenuItem).Tag.ToString());
                        }
                    }
                }
            }

            return columnList.ToArray();
        }

        public void Arama()
        {
            string[]  columns = SecilenKolonlar(MuvekkilSahisContextMenu.Items);
            if (columns.Length > 0)
            {
                using (var db = new DbConnection())
                {
                    List<MuvekkilSahis> queryResult = db.MuvekkilSahis_tt.ToList();
                    MuvekkilSahis_tt.ItemsSource = queryResult;
                    MuvekkilSahisExpander.Header = $"Şahıs Müvekkiller ({queryResult.Count.ToString()})";
                }
            }

            MuvekkilSahis_tt.SelectedItem = null;

            columns = SecilenKolonlar(MuvekkilSirketContextMenu.Items);
            if (columns.Length > 0)
            {
                using (var db = new DbConnection())
                {
                    List<MuvekkilSirket> queryResult = db.MuvekkilSirket_tt.ToList();
                    MuvekkilSirket_tt.ItemsSource = queryResult;
                    MuvekkilSirketExpander.Header = $"Şirket Müvekkiller ({queryResult.Count.ToString()})";
                }
            }

            MuvekkilSirket_tt.SelectedItem = null;

            columns = SecilenKolonlar(DosyaIcraContextMenu.Items);
            if (columns.Length > 0)
            {
                using (var db = new DbConnection())
                {
                    //böyle include şekli yapmazsam o kolonlar boş kalıyor :( üzücü baya
                    db.DosyaIcra_tt
                        .Include(s => s.Alacakli)
                        .Include(s => s.Borclu)
                        .Include(s => s.AlacakliVekil)
                        .Include(s => s.BorcluVekil)
                        .Load();
                    ObservableCollection<DosyaIcra> queryResult = db.DosyaIcra_tt.Local;
                    DosyaIcra_tt.ItemsSource = queryResult;
                    DosyaIcraExpander.Header = $"İcra Dosyaları ({queryResult.Count.ToString()})";
                }
            }

            DosyaIcra_tt.SelectedItem = null;

            columns = SecilenKolonlar(DosyaDavaContextMenu.Items);
            if (columns.Length > 0)
            {
                using (var db = new DbConnection())
                {
                    db.DosyaDava_tt
                        .Include(s => s.Davali)
                        .Include(s => s.Davaci)
                        .Include(s => s.DavaliVekil)
                        .Include(s => s.DavaciVekil)
                        .Load();
                    ObservableCollection<DosyaDava> queryResult = db.DosyaDava_tt.Local;
                    DosyaDava_tt.ItemsSource = queryResult;
                    DosyaDavaExpander.Header = $"Dava Dosyaları ({queryResult.Count.ToString()})";
                }
            }

            DosyaDava_tt.SelectedItem = null;
        }

        private async void DataGrid_RightClick(object sender, RoutedEventArgs e)
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

        private async void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    string tablo = (sender as DataGrid).Name;
                    string kolon = e.Column.SortMemberPath;
                    int girdiID = ((sender as DataGrid).SelectedItem as IData_tspAuto).ID;

                    var view = new BenimDialog
                    {
                        DataContext = new BenimDialogViewModel("Bu değeri güncellemek istediğinizden emin misiniz?", "EVET", "HAYIR")
                    };

                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
                        // TODO: AramaYap'a güncelleme ekle.
                        //using (var db = new DbConnection())
                        //{
                        //    db.Database.ExecuteSqlCommand("UPDATE " + tablo + " SET " + kolon + "={0} WHERE ID={1}", newVal, girdiID);
                        //    db.SaveChanges();
                        //}
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
            AramaKutusu.Focus();
            AramaKutusu.SelectAll();
        }

        private void ContextMenu_TumunuSec_Click(object sender, RoutedEventArgs e)
        {
            ItemCollection items = ((MenuItem)(sender as MenuItem).Parent).Items;

            foreach (var item in items)
            {
                if (item.GetType() == typeof(MenuItem))
                {
                    if ((item as MenuItem).IsCheckable)
                    {
                        (item as MenuItem).IsChecked = true;
                    }
                }
            }
        }

        private void ContextMenu_SecimleriKaldir_Click(object sender, RoutedEventArgs e)
        {
            ItemCollection items = ((MenuItem)(sender as MenuItem).Parent).Items;

            foreach (var item in items)
            {
                if (item.GetType() == typeof(MenuItem))
                {
                    if ((item as MenuItem).IsCheckable)
                    {
                        (item as MenuItem).IsChecked = false;
                    }
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
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
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
