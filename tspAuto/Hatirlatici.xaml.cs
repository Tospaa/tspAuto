using MaterialDesignThemes.Wpf;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using tspAuto.Domain;
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

            VeritabaniOku().GetAwaiter().GetResult();

            TarihSec.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
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
                MessageBox.Show(se.ToString());
                return null;
            }
        }

        private async Task VeritabaniOku()
        {
            DataSet dataSet = new DataSet();

            MethodPack.VeritabaniKodBlogu((con) => {
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Hatirlaticilar", con))
                {
                    dataAdapter.Fill(dataSet);
                }
            });

            try
            {
                foreach (DataRowView i in dataSet.Tables[0].DefaultView)
                {
                    string baslik = i["Baslik"].ToString();
                    string aciklama = i["Aciklama"].ToString();
                    string[] strzmn = i["Zaman"].ToString().Split(new char[] { '.' });
                    int[] zmn = Array.ConvertAll(strzmn, int.Parse);
                    DateTime tarih = new DateTime(zmn[0], zmn[1], zmn[2], zmn[3], zmn[4], 0, DateTimeKind.Local);
                    string tablo = i["HatirlaticiTablo"].ToString();
                    int id = Convert.ToInt32(i["HatirlaticiID"]);

                    // define the job and tie it to our Gorev class
                    IJobDetail job = JobBuilder.Create<Gorev>()
                        .UsingJobData("Baslik", baslik)
                        .UsingJobData("Aciklama", aciklama)
                        .UsingJobData("Tablo", tablo)
                        .UsingJobData("ID", id)
                        .Build();

                    // trigger builder creates simple trigger by default, actually an ITrigger is returned
                    ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                        .StartAt(tarih)
                        .Build();

                    // Tell quartz to schedule the job using our trigger
                    await scheduler.ScheduleJob(job, trigger);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void Yenile_Button_Click(object sender, RoutedEventArgs e)
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

                    girdiler.Add(new Girdi(isim, zaman, key, triggerKey));
                }

                HatirlaticiTablosu.ItemsSource = girdiler;
                HatirlaticiTablosu.SelectedItem = null;
            }
        }

        private async void HatirlaticiTablosu_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Girdi item = (Girdi)HatirlaticiTablosu.SelectedItem;

            try
            {
                IJobDetail jobDetail = await scheduler.GetJobDetail(item.HatirlaticiKey);

                string baslik = jobDetail.JobDataMap.GetString("Baslik");
                string aciklama = jobDetail.JobDataMap.GetString("Aciklama");
                string zaman = item.HatirlaticiZamani;

                var view = new BenimDialog
                {
                    DataContext = new BenimDialogViewModel($"{baslik}\n{zaman}\n{aciklama}",
                    "İPTAL ET",
                    "KAPAT",
                    "Hatırlatıcıyı iptal eder")
                };

                var result = await DialogHost.Show(view, "RootDialog");

                if (Convert.ToBoolean(result))
                {
                    await scheduler.DeleteJob(item.HatirlaticiKey);
                    MethodPack.HatirlaticilarVeritabanina(this);
                }

                Yenile_Button_Click(sender, e);
            }
            catch (NullReferenceException) { Yenile_Button_Click(sender, e); }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Hatirlatici_DialogHost_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return; // çok coolum ya mk xD

            try
            {
                DateTime trh = Convert.ToDateTime(TarihSec.SelectedDate);
                DateTime st = Convert.ToDateTime(SaatSec.SelectedTime);

                DateTime tarih = new DateTime(trh.Year, trh.Month, trh.Day, st.Hour, st.Minute, 0, DateTimeKind.Local);

                bool basarili = MethodPack.YeniHatirlatici(Baslik.Text, Aciklama.Text, tarih);

                if (basarili) { Yenile_Button_Click(sender, eventArgs); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Yenile_Button_Click(sender, e);
        }
    }

    public class Girdi
    {
        public string HatirlaticiAdi { get; set; }
        public string HatirlaticiZamani { get; set; }
        public JobKey HatirlaticiKey { get; set; }
        public TriggerKey HatirlaticiTrigger { get; set; }

        public Girdi(string hatirlatici_ismi, string hatirlatici_zamani, JobKey hatirlatici_key, TriggerKey trigger_key)
        {
            HatirlaticiAdi = hatirlatici_ismi;
            HatirlaticiZamani = hatirlatici_zamani;
            HatirlaticiKey = hatirlatici_key;
            HatirlaticiTrigger = trigger_key;
        }
    }
}



