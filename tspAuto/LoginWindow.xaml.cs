using System;
using System.Linq;
using System.Windows;
using tspAuto.Domain;
using tspAuto.Model;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private Kullanici yerelKullanici = null;

        private bool Authenticated()
        {
            if (KullaniciAdiKutusu.Text != string.Empty && SifreKutusu.Password != string.Empty)
            {
                using (var db = new DbConnection())
                {
                    yerelKullanici = db.Kullanicilar.Where(s => s.KullaniciAdi == KullaniciAdiKutusu.Text).FirstOrDefault();
                }
                if (Cabbar_Izin())
                {
                    return true;
                }
                else
                {
                    Temizle();
                    return false;
                }
            }
            else
            {
                Temizle();
                return false;
            }
        }

        private bool Cabbar_Izin()
        {
            Hatalar.Items.Clear();

            bool kullaniciVar = yerelKullanici != null;
            if (!kullaniciVar)
            {
                Hatalar.Items.Add(" • Böyle bir kullanıcı yok.");
                return false;
            }

            bool kullaniciYetkin = yerelKullanici.Yetki != Yetkiler.Yetkisiz;
            if (!kullaniciYetkin) { Hatalar.Items.Add(" • Bu kullanıcının programı açma yetkisi yoktur."); }

            bool sifreDogru = MethodPack.ValidatePassword(SifreKutusu.Password, yerelKullanici.SifreHash);
            if (!sifreDogru) { Hatalar.Items.Add(" • Şifrenizi yanlış girdiniz."); }

            return kullaniciVar && kullaniciYetkin && sifreDogru;
        }

        private void Temizle()
        {
            KullaniciAdiKutusu.Text = string.Empty;
            SifreKutusu.Password = string.Empty;
            KullaniciAdiKutusu.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Authenticated())
            {
                Window window = new MainWindow
                {
                    DataContext = new MainWindowViewModel(yerelKullanici)
                };
                window.Show();
                Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KullaniciAdiKutusu.Focus();
        }
    }
}
