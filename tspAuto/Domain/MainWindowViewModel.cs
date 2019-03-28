using System;
using System.Data.SQLite;
using System.IO;
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
                    new PanelItem("Yeni Müvekkil Ekle", new YeniMuvekkilEkle(), "Add"),
                    new PanelItem("Yeni İş Ekle", new YeniIsEkle(), "Add"),
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

                //let's set up a little MVVM, cos that's what the cool kids are doing:
                var view = new BenimDialog
                {
                    DataContext = new BenimDialogViewModel($"Zaten bir veritabanı seçilmiş. Bu veritabanı yerine başka bir veritabanı seçmek istiyor musunuz?\n{Properties.Settings.Default.DatabaseFilePath}","EVET","HAYIR","Mevcut veritabanının otomatik bir yedeği oluşturulacaktır.","Mevcut veritabanını kullanmaya devam et.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog");

                //check the result...

                if (Convert.ToBoolean(result))
                {
                    //TODO: Veritabanının yedeğini oluştur.
                    Properties.Settings.Default.DatabaseFilePath = "";
                    Properties.Settings.Default.Save();

                    if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}"))
                    {
                        Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}");
                    }

                    //let's set up a little MVVM, cos that's what the cool kids are doing:
                    view = new BenimDialog
                    {
                        DataContext = new BenimDialogViewModel("Yeni bir veritabanı oluşturmak mı yoksa var olan bir veritabanını yüklemek mi istersiniz?", "YENİ", "VAR OLAN", "Sıfırdan yeni bir veri tabanı oluşturur.", "Daha önce oluşturulmuş bir veritabanını yüklemenizi sağlar.")
                    };

                    //show the dialog
                    result = await DialogHost.Show(view, "RootDialog");

                    //check the result...

                    if (Convert.ToBoolean(result))
                    {
                        Properties.Settings.Default.DatabaseFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}\\veri_{DateTime.Now.ToString("yyyyMMddHHmmss")}.db";
                        Properties.Settings.Default.Save();

                        SQLiteConnection.CreateFile(Properties.Settings.Default.DatabaseFilePath);
                    }
                    else
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
            else
            {
                //Bir veritabanı seçili değil demektir.

                if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}"))
                {
                    Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}");
                }

                //let's set up a little MVVM, cos that's what the cool kids are doing:
                var view = new BenimDialog
                {
                    DataContext = new BenimDialogViewModel("Yeni bir veritabanı oluşturmak mı yoksa var olan bir veritabanını yüklemek mi istersiniz?", "YENİ", "VAR OLAN", "Sıfırdan yeni bir veri tabanı oluşturur.", "Daha önce oluşturulmuş bir veritabanını yüklemenizi sağlar.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog");

                //check the result...

                if (Convert.ToBoolean(result))
                {
                    Properties.Settings.Default.DatabaseFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}\\veri_{DateTime.Now.ToString("yyyyMMddHHmmss")}.db";
                    Properties.Settings.Default.Save();

                    SQLiteConnection.CreateFile(Properties.Settings.Default.DatabaseFilePath);
                }
                else
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
    }
}
