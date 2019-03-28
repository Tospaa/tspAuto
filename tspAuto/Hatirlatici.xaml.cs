using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Controls;
using tspAuto.Reminder;

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

        private async Task DataGrid_Doldur()
        {
            IReadOnlyCollection<TriggerKey> allTriggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
            ObservableCollection<Girdi> girdiler = new ObservableCollection<Girdi>();

            foreach (TriggerKey triggerKey in allTriggerKeys)
            {
                ITrigger triggerdetails = await scheduler.GetTrigger(triggerKey);
                IJobDetail jobDetail = await scheduler.GetJobDetail(triggerdetails.JobKey);

                string isim = triggerdetails.JobKey.Name;
                string zaman = triggerdetails.StartTimeUtc.DateTime.ToString("dd MMMM yyyy dddd HH:mm");

                girdiler.Add(new Girdi(isim, zaman));
            }

            HatirlaticiTablosu.ItemsSource = girdiler;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (scheduler != null)
            {
                DataGrid_Doldur().GetAwaiter().GetResult();
            }
        }
    }

    public class Girdi
    {
        public string HatirlaticiAdi { get; set; }
        public string HatirlaticiZamani { get; set; }

        public Girdi(string hatirlatici_ismi, string hatirlatici_zamani)
        {
            HatirlaticiAdi = hatirlatici_ismi;
            HatirlaticiZamani = hatirlatici_zamani;
        }
    }
}
