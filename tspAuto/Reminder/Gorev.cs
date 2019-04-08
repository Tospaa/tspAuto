using Quartz;
using System;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using tspAuto.Domain;

namespace tspAuto.Reminder
{
    public class Gorev : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string baslik = dataMap.GetString("Baslik");
            string aciklama = dataMap.GetString("Aciklama");
            string tablo = dataMap.GetString("Tablo");
            int id = dataMap.GetInt("ID");

            #region E-Mail
            SecureString securePwd = new SecureString();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("tspauto7935@gmail.com");
                mail.To.Add("musaecer@gmail.com");
                mail.Subject = baslik;
                mail.Body = aciklama;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("tspauto7935", securePwd);
                SmtpServer.EnableSsl = true;

                //SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally { securePwd.Dispose(); }
            #endregion

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

                            foreach (PanelItem item in (window as MainWindow).SolPanelListBox.Items)
                            {
                                if (item.Content.GetType() == typeof(Hatirlatici))
                                {
                                    MethodPack.HatirlaticilarVeritabanina(item.Content as Hatirlatici);
                                }
                            }
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
