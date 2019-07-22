using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace tspAuto.Domain
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            PanelItems = new[]
                {
                    new PanelItem("Ana Sayfa", new Home(), "HomeVariant"),
                    new PanelItem("Arama Yap", new AramaYap(), "Magnify"),
                    new PanelItem("Yeni Müvekkil Ekle", new YeniMuvekkilEkle(), "UserPlus"),
                    new PanelItem("Yeni Dosya Ekle", new YeniDosyaEkle(), "NotePlus"),
                    new PanelItem("Yeni İş Ekle", new YeniIsEkle(), "BooksPlus"),
                    new PanelItem("Hatırlatıcı", new Hatirlatici(), "Bell")
                };
        }

        public PanelItem[] PanelItems { get; }

        public ICommand VeritabaniDialogKomutu => new AnotherCommandImplementation(VeritabaniDialogKomutuCalistir);
        
        private async void VeritabaniDialogKomutuCalistir(object o)
        {
            if (Properties.Settings.Default.DatabaseFilePath != "")
            {
                //Bir veritabanı halihazırda seçili demektir.
                var view = new BenimDialog
                {
                    DataContext = new BenimDialogViewModel($"Zaten bir veritabanı seçilmiş. Bu veritabanı yerine başka bir veritabanı seçmek istiyor musunuz?\n{Properties.Settings.Default.DatabaseFilePath}","EVET","HAYIR","Mevcut veritabanının otomatik bir yedeği oluşturulacaktır.","Mevcut veritabanını kullanmaya devam et.")
                };

                var result = await DialogHost.Show(view, "RootDialog");

                if (Convert.ToBoolean(result))
                {
                    //TODO: Veritabanının yedeğini oluştur.
                    Properties.Settings.Default.DatabaseFilePath = "";
                    Properties.Settings.Default.Save();

                    //Documents dizininde bizim klasör yoksa oluşturuyoruz
                    KlasorYarat();

                    view = new BenimDialog
                    {
                        DataContext = new BenimDialogViewModel("Yeni bir veritabanı oluşturmak mı yoksa var olan bir veritabanını yüklemek mi istersiniz?", "YENİ", "VAR OLAN", "Sıfırdan yeni bir veri tabanı oluşturur.", "Daha önce oluşturulmuş bir veritabanını yüklemenizi sağlar.")
                    };

                    result = await DialogHost.Show(view, "RootDialog");

                    if (Convert.ToBoolean(result))
                    {
                        VeritabaniYarat();
                    }
                    else
                    {
                        VeritabaniSec();
                    }
                }
            }
            else
            {
                //Bir veritabanı seçili değil demektir.
                KlasorYarat();

                var view = new BenimDialog
                {
                    DataContext = new BenimDialogViewModel("Yeni bir veritabanı oluşturmak mı yoksa var olan bir veritabanını yüklemek mi istersiniz?", "YENİ", "VAR OLAN", "Sıfırdan yeni bir veri tabanı oluşturur.", "Daha önce oluşturulmuş bir veritabanını yüklemenizi sağlar.")
                };

                var result = await DialogHost.Show(view, "RootDialog");

                if (Convert.ToBoolean(result))
                {
                    VeritabaniYarat();
                }
                else
                {
                    VeritabaniSec();
                }
            }
        }

        private void KlasorYarat()
        {
            if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}"))
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}");
            }
        }

        private void VeritabaniYarat()
        {
            //Properties.Settings.Default.DatabaseFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}\\veri_{DateTime.Now.ToString("yyyyMMddHHmmss")}.db";
            //Properties.Settings.Default.Save();

            //SQLiteConnection.CreateFile(Properties.Settings.Default.DatabaseFilePath);

            //string sql1 = @"CREATE TABLE IF NOT EXISTS MuvekkilSirket(
            //                ID   INTEGER PRIMARY KEY AUTOINCREMENT,
            //                MuvekkilNo          TEXT      NOT NULL,
            //                MuvekkilTuru        TEXT      NOT NULL,
            //                NoterIsmi           TEXT      NOT NULL,
            //                VekaletTarihi       TEXT      NOT NULL,
            //                VekYevmiyeNo        TEXT      NOT NULL,
            //                AhzuKabza           BOOLEAN   NOT NULL,
            //                Feragat             BOOLEAN   NOT NULL,
            //                Ibra                BOOLEAN   NOT NULL,
            //                Sulh                BOOLEAN   NOT NULL,
            //                Banka               TEXT      NOT NULL,
            //                Sube                TEXT      NOT NULL,
            //                IBANno              TEXT      NOT NULL,
            //                Adres               TEXT      NOT NULL,
            //                Telefon             TEXT      NOT NULL,
            //                Fax                 TEXT      NOT NULL,
            //                Email               TEXT      NOT NULL,
            //                SirketTuru          TEXT      NOT NULL,
            //                SirketUnvan         TEXT      NOT NULL,
            //                VergiDairesi        TEXT      NOT NULL,
            //                VergiNo             TEXT      NOT NULL,
            //                MersisNo            TEXT      NOT NULL
            //                );";

            //string sql2 = @"CREATE TABLE IF NOT EXISTS MuvekkilSahis(
            //                ID   INTEGER PRIMARY KEY AUTOINCREMENT,
            //                MuvekkilNo          TEXT      NOT NULL,
            //                MuvekkilTuru        TEXT      NOT NULL,
            //                NoterIsmi           TEXT      NOT NULL,
            //                VekaletTarihi       TEXT      NOT NULL,
            //                VekYevmiyeNo        TEXT      NOT NULL,
            //                AhzuKabza           BOOLEAN   NOT NULL,
            //                Feragat             BOOLEAN   NOT NULL,
            //                Ibra                BOOLEAN   NOT NULL,
            //                Sulh                BOOLEAN   NOT NULL,
            //                Banka               TEXT      NOT NULL,
            //                Sube                TEXT      NOT NULL,
            //                IBANno              TEXT      NOT NULL,
            //                Adres               TEXT      NOT NULL,
            //                Telefon             TEXT      NOT NULL,
            //                Fax                 TEXT      NOT NULL,
            //                Email               TEXT      NOT NULL,
            //                IsimSoyisim         TEXT      NOT NULL,
            //                TCKimlik            TEXT      NOT NULL
            //                );";

            //string sql3 = @"CREATE TABLE IF NOT EXISTS Hatirlaticilar(
            //                Baslik              TEXT      NOT NULL,
            //                Aciklama            TEXT      NOT NULL,
            //                Zaman               TEXT      NOT NULL,
            //                HatirlaticiTablo    TEXT      NOT NULL,
            //                HatirlaticiID       INTEGER   NOT NULL
            //                );";

            //bool basarili = false;

            //try
            //{
            //    using (SQLiteConnection con = new SQLiteConnection($"Data Source={Properties.Settings.Default.DatabaseFilePath};"))
            //    {
            //        con.Open();
            //        new SQLiteCommand(sql1 + sql2 + sql3, con).ExecuteNonQuery();

            //        basarili = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Veritabanı oluşturma işlemi sırasında bir hata oluştu.\n\n" + ex.Message);
            //}
            //finally
            //{
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    if (basarili)
            //    {
            //        MessageBox.Show("Veritabanı oluşturma başarılı.");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Veritabanı oluşturması yapılamadı.");
            //    }
            //}
        }

        private void VeritabaniSec()
        {
            OpenFileDialog file = new OpenFileDialog
            {
                Filter = "Veritabanı |*.db",
                CheckFileExists = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + Properties.Resources.Title,
                Title = "Veritabanı Dosyası Seçiniz..",
                Multiselect = false
            };

            if (Convert.ToBoolean(file.ShowDialog()))
            {
                Properties.Settings.Default.DatabaseFilePath = file.FileName;
                Properties.Settings.Default.Save();
            }
        }
    }
}
