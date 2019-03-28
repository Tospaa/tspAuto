using MaterialDesignThemes.Wpf;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Controls;
using tspAuto.Domain;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for Hatirlatici.xaml
    /// </summary>
    public partial class Hatirlatici : UserControl
    {
        public IScheduler scheduler;

        public Hatirlatici()
        {
            InitializeComponent();
            
            scheduler = RunProgram().GetAwaiter().GetResult();
        }

        private static async Task<IScheduler> RunProgram()
        {
            try
            {
                // Grab the Scheduler instance from the Factory
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" },
                    { "quartz.scheduler.instanceName", "tspAutoScheduler" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                // and start it off
                await scheduler.Start();

                return scheduler;
            }
            catch (SchedulerException se)
            {
                System.Windows.MessageBox.Show(se.ToString());
                return null;
            }
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (scheduler != null)
            {
                IReadOnlyCollection<TriggerKey> allTriggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
                ObservableCollection<Girdi> girdiler = new ObservableCollection<Girdi>();

                foreach (TriggerKey triggerKey in allTriggerKeys)
                {
                    ITrigger triggerdetails = await scheduler.GetTrigger(triggerKey);
                    IJobDetail jobDetail = await scheduler.GetJobDetail(triggerdetails.JobKey);

                    string isim = jobDetail.JobDataMap.GetString("Baslik");
                    string zaman = triggerdetails.StartTimeUtc.DateTime.ToString("dd MMMM yyyy dddd HH:mm");
                    JobKey key = triggerdetails.JobKey;

                    girdiler.Add(new Girdi(isim, zaman, key));
                }

                HatirlaticiTablosu.ItemsSource = girdiler;
                HatirlaticiTablosu.SelectedItem = null;
            }
        }

        private async void HatirlaticiTablosu_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Girdi item = (Girdi)HatirlaticiTablosu.SelectedItem;
            IJobDetail jobDetail = await scheduler.GetJobDetail(item.HatirlaticiKey);

            string baslik = jobDetail.JobDataMap.GetString("Baslik");
            string aciklama = jobDetail.JobDataMap.GetString("Aciklama");

            var view = new BenimDialog
            {
                DataContext = new BenimDialogViewModel($"{baslik}\n\n{aciklama}",
                        "İPTAL ET",
                        "KAPAT",
                        "Hatırlatıcıyı iptal eder")
            };

            var result = await DialogHost.Show(view, "RootDialog");

            if (Convert.ToBoolean(result))
            {
                await scheduler.DeleteJob(item.HatirlaticiKey);
            }
        }
    }

    public class Girdi
    {
        public string HatirlaticiAdi { get; set; }
        public string HatirlaticiZamani { get; set; }
        public JobKey HatirlaticiKey { get; set; }

        public Girdi(string hatirlatici_ismi, string hatirlatici_zamani, JobKey hatirlatici_key)
        {
            HatirlaticiAdi = hatirlatici_ismi;
            HatirlaticiZamani = hatirlatici_zamani;
            HatirlaticiKey = hatirlatici_key;
        }
    }
}
