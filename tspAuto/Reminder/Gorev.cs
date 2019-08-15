using Quartz;
using System;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using tspAuto.Domain;
using tspAuto.Model;

namespace tspAuto.Reminder
{
    public class Gorev : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string baslik = dataMap.GetString("Baslik");
            string aciklama = dataMap.GetString("Aciklama");
            int ilgiliID = dataMap.GetInt("IlgiliID");
            int tabloID = dataMap.GetInt("TabloID");
            string tablo = dataMap.GetString("Tablo");

            Kullanici ilgiliKisi;
            IsModel ilgiliIs;

            using (var db = new DbConnection())
            {
                ilgiliKisi = db.Kullanicilar.FirstOrDefault(s => s.ID == ilgiliID);
                ilgiliIs = db.Isler.FirstOrDefault(s => s.ID == tabloID);
            }

            try
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(MainWindow) && window.DataContext != null && ilgiliKisi != null)
                        {
                            MainWindowViewModel MainWindowDataContext = (window as MainWindow).DataContext as MainWindowViewModel;

                            if (ilgiliKisi.ID == MainWindowDataContext.MevcutKullanici.ID)
                            {
                                (window as MainWindow).notifyIcon.BalloonTipTitle = baslik;
                                (window as MainWindow).notifyIcon.BalloonTipText = aciklama;
                                (window as MainWindow).notifyIcon.ShowBalloonTip(5000);
                            }

                            #region E-Mail
                            SecureString securePwd = new SecureString();
                            try
                            {
                                MailMessage mail = new MailMessage();
                                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                                mail.From = new MailAddress("tspauto7935@gmail.com");
                                mail.To.Add(ilgiliKisi.Email);
                                mail.Subject = baslik;
                                mail.Body = aciklama;

                                SmtpServer.Port = 587;
                                SmtpServer.Credentials = new System.Net.NetworkCredential("tspauto7935", securePwd);
                                SmtpServer.EnableSsl = true;

                                SmtpServer.Send(mail);
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.ToString());
                                // TODO: Buradaki hatayı sadece log'a yazdır, messagebox'la program bloklanmasın.
                            }
                            finally { securePwd.Dispose(); }
                            #endregion

                            Hatirlatici hatirlaticiInstance = (Hatirlatici)MainWindowDataContext.PanelItems[5].Content;
                            MethodPack.HatirlaticilarVeritabanina(hatirlaticiInstance);
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
