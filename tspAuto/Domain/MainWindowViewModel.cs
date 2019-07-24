using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using tspAuto.Model;

namespace tspAuto.Domain
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private Kullanici _mevcutKullanici;

        public MainWindowViewModel(Kullanici mevcutKullanici)
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

            _mevcutKullanici = mevcutKullanici;
        }

        public PanelItem[] PanelItems { get; }

        public Kullanici MevcutKullanici
        {
            get { return _mevcutKullanici; }
            private set
            {
                this.MutateVerbose(ref _mevcutKullanici, value, RaisePropertyChanged());
            }
        }

        public ICommand VeritabaniDialogKomutu => new AnotherCommandImplementation(VeritabaniDialogKomutuCalistir);

        private async void VeritabaniDialogKomutuCalistir(object o)
        {
            if (Properties.Settings.Default.DatabaseFilePath != "")
            {
                //Bir veritabanı halihazırda seçili demektir.
                var view = new BenimDialog
                {
                    DataContext = new BenimDialogViewModel($"Zaten bir veritabanı seçilmiş. Bu veritabanı yerine başka bir veritabanı seçmek istiyor musunuz?\n{Properties.Settings.Default.DatabaseFilePath}", "EVET", "HAYIR", "Mevcut veritabanının otomatik bir yedeği oluşturulacaktır.", "Mevcut veritabanını kullanmaya devam et.")
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

        public ICommand YeniKullaniciEkleKomutu => new AnotherCommandImplementation(YeniKullaniciEkleKomutuCalistir);

        private void YeniKullaniciEkleKomutuCalistir(object o)
        {
            Window window = new AddNewUserWindow();
            window.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
