using Quartz;
using System;
using System.Windows;

namespace tspAuto.Reminder
{
    public class Gorev : IJob
    {
        public async System.Threading.Tasks.Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string baslik = dataMap.GetString("Baslik");
            string aciklama = dataMap.GetString("Aciklama");

            try
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(MainWindow))
                        {
                            (window as MainWindow).notifyIcon.BalloonTipTitle = baslik;
                            (window as MainWindow).notifyIcon.BalloonTipText = aciklama;
                            (window as MainWindow).notifyIcon.ShowBalloonTip(5000);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
