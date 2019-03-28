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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)HatirlaticiEklensin.IsChecked)
            {
                if (TarihSec.Text != string.Empty && SaatSec.Text != string.Empty)
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(MainWindow))
                        {
                            foreach (PanelItem item in (window as MainWindow).SolPanelListBox.Items)
                            {
                                if (item.Content.GetType() == typeof(Hatirlatici))
                                {
                                    DateTime trh = Convert.ToDateTime(TarihSec.SelectedDate);
                                    DateTime st = Convert.ToDateTime(SaatSec.SelectedTime);

                                    DateTime tarih = new DateTime(trh.Year, trh.Month, trh.Day, st.Hour, st.Minute, 0);

                                    // define the job and tie it to our Gorev class
                                    IJobDetail job = JobBuilder.Create<Gorev>()
                                        .UsingJobData("Baslik", Baslik.Text)
                                        .UsingJobData("Aciklama", Aciklama.Text)
                                        .Build();

                                    // trigger builder creates simple trigger by default, actually an ITrigger is returned
                                    ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                        .StartAt(new DateTimeOffset(tarih, new TimeSpan(+3, 0, 0)))
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
                else
                {
                    MessageBox.Show("Tarih ve saat seçmelisiniz.");
                }
            }
        }
    }
}
