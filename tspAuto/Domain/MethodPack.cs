using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using tspAuto.Reminder;

namespace tspAuto.Domain
{
    public static class MethodPack
    {
        public static SQLiteCommand Generate_Insert_Command(string table, string[] columns, object[] values, SQLiteConnection con)
        {
            string insertString = $"INSERT INTO {table}(";

            foreach (string i in columns)
            {
                insertString += i + ",";
            }

            insertString = insertString.Substring(0, insertString.Length - 1) + ") VALUES(";

            foreach (string i in columns)
            {
                insertString += "@" + i + ",";
            }

            insertString = insertString.Substring(0, insertString.Length - 1) + ");";

            SQLiteCommand command = new SQLiteCommand(insertString, con);

            for (int i = 0; i < columns.Length; i++)
            {
                command.Parameters.AddWithValue(columns[i], values[i]);
            }

            return command;
        }

        public static SQLiteCommand Generate_Query_Command(string word, string table, string[] columns, SQLiteConnection con)
        {
            if (new Regex("[ğĞüÜşŞıİöÖçÇ]").Match(word).Success)
            {
                // (?i) ignores case sensitivity
                string arama = word;

                arama = new Regex("[ğĞ]").Replace(arama, "[ğĞ]");
                arama = new Regex("[üÜ]").Replace(arama, "[üÜ]");
                arama = new Regex("[şŞ]").Replace(arama, "[şŞ]");
                arama = new Regex("[ıI]").Replace(arama, "[ıI]");
                arama = new Regex("[iİ]").Replace(arama, "[iİ]");
                arama = new Regex("[öÖ]").Replace(arama, "[öÖ]");
                arama = new Regex("[çÇ]").Replace(arama, "[çÇ]");

                arama = "(?i)" + arama;

                string queryString = $"SELECT * FROM {table} WHERE(";

                foreach (string i in columns)
                {
                    queryString += $"{i} REGEXP @arama OR ";
                }

                queryString = queryString.Substring(0, queryString.Length - 4) + ")";

                SQLiteCommand command = new SQLiteCommand(queryString, con);

                command.Parameters.AddWithValue("arama", arama);

                return command;
            }
            else
            {
                string queryString = $"SELECT * FROM {table} WHERE(";

                foreach (string i in columns)
                {
                    queryString += $"{i} LIKE @arama OR ";
                }

                queryString = queryString.Substring(0, queryString.Length - 4) + ")";

                SQLiteCommand command = new SQLiteCommand(queryString, con);

                command.Parameters.AddWithValue("arama", "%" + word + "%");

                return command;
            }
        }

        public delegate void CodeBlock(SQLiteConnection con);

        public static void VeritabaniKodBlogu(CodeBlock codeBlock)
        {
            try
            {
                if (Properties.Settings.Default.DatabaseFilePath != "" && System.IO.File.Exists(Properties.Settings.Default.DatabaseFilePath))
                {
                    try
                    {
                        using (SQLiteConnection con = new SQLiteConnection($"Data Source={Properties.Settings.Default.DatabaseFilePath};"))
                        {
                            //code goes here
                            codeBlock.Invoke(con);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Veritabanı işlemi sırasında bir hata oluştu.\n\n" + ex.Message);
                        return;
                    }
                    finally
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                    }
                }
                else if (Properties.Settings.Default.DatabaseFilePath == "")
                {
                    MessageBox.Show("Veritabanı seçilmemiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                    return;
                }
                else if (!System.IO.File.Exists(Properties.Settings.Default.DatabaseFilePath))
                {
                    MessageBox.Show("Veritabanı silinmiş ya da erişim engellenmiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                    return;
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("Bazı dosyalar silinmiş ya da erişim engellenmiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                return;
            }
        }

        public static void HatirlaticilarVeritabanina()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    foreach (PanelItem item in (window as MainWindow).SolPanelListBox.Items)
                    {
                        if (item.Content.GetType() == typeof(Hatirlatici))
                        {
                            string[] columns = new string[]
                            {
                                "Baslik",
                                "Aciklama",
                                "Zaman",
                                "HatirlaticiTablo",
                                "HatirlaticiID"
                            };

                            bool basarili = false;

                            IReadOnlyCollection<TriggerKey> allTriggerKeys = (item.Content as Hatirlatici).scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup()).GetAwaiter().GetResult();

                            VeritabaniKodBlogu((con) => {
                                con.Open();

                                new SQLiteCommand("DELETE FROM Hatirlaticilar;", con).ExecuteNonQuery();

                                foreach (TriggerKey triggerKey in allTriggerKeys)
                                {
                                    ITrigger triggerdetails = (item.Content as Hatirlatici).scheduler.GetTrigger(triggerKey).GetAwaiter().GetResult();
                                    IJobDetail jobDetail = (item.Content as Hatirlatici).scheduler.GetJobDetail(triggerdetails.JobKey).GetAwaiter().GetResult();

                                    object[] values = new object[]
                                    {
                                        jobDetail.JobDataMap.GetString("Baslik"),
                                        jobDetail.JobDataMap.GetString("Aciklama"),
                                        triggerdetails.StartTimeUtc.DateTime.ToString("yyyy.MM.dd.HH.mm"),
                                        jobDetail.JobDataMap.GetString("Tablo"),
                                        jobDetail.JobDataMap.GetInt("ID")
                                    };

                                    Generate_Insert_Command("Hatirlaticilar", columns, values, con).ExecuteNonQuery();
                                }

                                basarili = true;
                            });

                            if (!basarili)
                            {
                                MessageBox.Show("Veritabanı girdisi yapılamadı.");
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public static bool YeniHatirlatici(string baslik, string aciklama, DateTime tarih, string tablo = "Tablosuz", long id = 0)
        {
            if (baslik != string.Empty && aciklama != string.Empty && tarih != null)
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
                                (item.Content as Hatirlatici).scheduler.ScheduleJob(job, trigger);
                                return true;
                            }
                        }
                    }
                }
            }
            else if (baslik == string.Empty || aciklama == string.Empty)
            {
                MessageBox.Show("Başlık ve açıklama kısımları boş olamaz.");
            }
            return false;
        }
    }
}
