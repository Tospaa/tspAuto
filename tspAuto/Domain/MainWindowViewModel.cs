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
                    new PanelItem("Yeni Müvekkil Ekle", new YeniMuvekkilEkle(), "Add"),
                    new PanelItem("Yeni İş Ekle", new YeniIsEkle(), "Add")
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
                var view = new VeritabaniDialog
                {
                    DataContext = new VeritabaniDialogViewModel($"Zaten bir veritabanı seçilmiş. Bu veritabanı yerine başka bir veritabanı seçmek istiyor musunuz?\n{Properties.Settings.Default.DatabaseFilePath}","EVET","HAYIR","Mevcut veritabanının otomatik bir yedeği oluşturulacaktır.","Mevcut veritabanını kullanmaya devam et.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog");

                //check the result...
                //MessageBox.Show("Dialog was closed, the CommandParameter used to close it was: " + (result.GetType()));

                if (Convert.ToBoolean(result))
                {
                    //TODO: Veritabanının yedeğini oluştur.
                    Properties.Settings.Default.DatabaseFilePath = "";
                    Properties.Settings.Default.Save();
                    //File Dialog aç
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
            else
            {
                //Bir veritabanı seçili değil demektir.

                if (!Directory.Exists($"{System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}"))
                {
                    Directory.CreateDirectory($"{System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}");
                }

                //let's set up a little MVVM, cos that's what the cool kids are doing:
                var view = new VeritabaniDialog
                {
                    DataContext = new VeritabaniDialogViewModel("Yeni bir veritabanı oluşturmak mı yoksa var olan bir veritabanını yüklemek mi istersiniz?", "YENİ", "VAR OLAN", "Sıfırdan yeni bir veri tabanı oluşturur.", "Daha önce oluşturulmuş bir veritabanını yüklemenizi sağlar.")
                };

                //show the dialog
                var result = await DialogHost.Show(view, "RootDialog");

                //check the result...
                //MessageBox.Show("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));

                if (Convert.ToBoolean(result))
                {
                    Properties.Settings.Default.DatabaseFilePath = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\{Properties.Resources.Title}\\veri.db";
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
