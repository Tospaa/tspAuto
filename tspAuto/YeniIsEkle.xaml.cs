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
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window.GetType() == typeof(MainWindow))
                            {
                                foreach (PanelItem item in (window as MainWindow).SolPanelListBox.Items)
                                {
                                    if (item.Content.GetType() == typeof(Hatirlatici))
                                    {
                                        // define the job and tie it to our Gorev class
                                        IJobDetail job = JobBuilder.Create<Gorev>()
                                            .UsingJobData("Baslik", Baslik.Text)
                                            .UsingJobData("Aciklama", Aciklama.Text)
                                            .Build();

                                        // trigger builder creates simple trigger by default, actually an ITrigger is returned
                                        ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                            .StartAt(tarih)
                                            .Build();

                                        // Tell quartz to schedule the job using our trigger
                                        (item.Content as Hatirlatici).scheduler.ScheduleJob(job, trigger);
                                        break;
                                    }
                                }
                                break;
                            }
                        }
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
