using MaterialDesignThemes.Wpf;
using Quartz;
using System;
using System.Windows;
using System.Windows.Controls;
using tspAuto.Domain;
using tspAuto.Reminder;

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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)HatirlaticiEklensin.IsChecked)
            {
                try
                {
                    DateTime trh = Convert.ToDateTime(TarihSec.SelectedDate);
                    DateTime st = Convert.ToDateTime(SaatSec.SelectedTime);

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
                        MethodPack.YeniHatirlatici(Baslik.Text, Aciklama.Text, tarih);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
