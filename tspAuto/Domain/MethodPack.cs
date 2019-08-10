using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Windows;
using tspAuto.Reminder;
using tspAuto.Model;

namespace tspAuto.Domain
{
    public static class MethodPack
    {
        public static void HatirlaticilarVeritabanina(Hatirlatici hatirlaticiInstance)
        {
            bool basarili = false;

            IReadOnlyCollection<TriggerKey> allTriggerKeys = hatirlaticiInstance.scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup()).GetAwaiter().GetResult();
            try
            {
                using (var db = new DbConnection())
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM dbo.Hatirlaticilar");

                    foreach (TriggerKey triggerKey in allTriggerKeys)
                    {
                        ITrigger triggerdetails = hatirlaticiInstance.scheduler.GetTrigger(triggerKey).GetAwaiter().GetResult();
                        IJobDetail jobDetail = hatirlaticiInstance.scheduler.GetJobDetail(triggerdetails.JobKey).GetAwaiter().GetResult();

                        if ((triggerdetails as Quartz.Impl.Triggers.SimpleTriggerImpl).TimesTriggered == 0)
                        {
                            var hatirlaticimodel = new HatirlaticiModel
                            {
                                Baslik = jobDetail.JobDataMap.GetString("Baslik"),
                                Aciklama = jobDetail.JobDataMap.GetString("Aciklama"),
                                Zaman = triggerdetails.StartTimeUtc.DateTime,
                                HatirlaticiTablo = jobDetail.JobDataMap.GetString("Tablo"),
                                HatirlaticiID = jobDetail.JobDataMap.GetInt("ID")
                            };

                            db.Hatirlaticilar.Add(hatirlaticimodel);
                        }
                    }
                    db.SaveChanges();
                    basarili = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            if (!basarili)
            {
                MessageBox.Show("Hatırlatıcıların veritabanına girdisi yapılamadı.");
            }
        }

        public static bool YeniHatirlatici(string baslik, string aciklama, DateTime tarih, string tablo = "Tablosuz", int id = 0)
        {
            if (baslik != string.Empty && aciklama != string.Empty && tarih != null)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        try
                        {
                            Hatirlatici hatirlaticiInstance = (Hatirlatici)((window as MainWindow).DataContext as MainWindowViewModel).PanelItems[5].Content;

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
                            hatirlaticiInstance.scheduler.ScheduleJob(job, trigger);
                            HatirlaticilarVeritabanina(hatirlaticiInstance);
                            return true;
                        }
                        catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    }
                }
            }
            else if (baslik == string.Empty || aciklama == string.Empty)
            {
                MessageBox.Show("Başlık ve açıklama kısımları boş olamaz.");
            }
            return false;
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }
    }
}
