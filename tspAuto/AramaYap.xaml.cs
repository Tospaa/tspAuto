﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Text.RegularExpressions;
using tspAuto.Domain;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Linq;
using tspAuto.Model;

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
        }

        private async void DataGrid_RightClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = ((ContextMenu)(sender as MenuItem).Parent).PlacementTarget as DataGrid;
                IDataModel_tspAuto item = (IDataModel_tspAuto)dataGrid.SelectedItem;
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
                            using (var db = new DbConnection())
                            {
                                db.Database.ExecuteSqlCommand("DELETE FROM dbo." + tablo + " WHERE ID={0}", item.ID);
                                db.SaveChanges();
                            }
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
                    int girdiID = ((sender as DataGrid).SelectedItem as IDataModel_tspAuto).ID;

                    var view = new BenimDialog
                    {
                        DataContext = new BenimDialogViewModel("Bu değeri güncellemek istediğinizden emin misiniz?", "EVET", "HAYIR")
                    };

                    var result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
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
