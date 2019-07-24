using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using tspAuto.Domain;
using tspAuto.Model;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for AddNewUserWindow.xaml
    /// </summary>
    public partial class AddNewUserWindow : Window
    {
        public AddNewUserWindow()
        {
            InitializeComponent();

            YetkiComBox.ItemsSource = Enum.GetValues(typeof(Yetkiler)).Cast<Yetkiler>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Cabbar_Izin())
            {
                Kullanici yeniKullanici = new Kullanici
                {
                    KullaniciAdi = kullaniciAdi.Text,
                    SifreHash = MethodPack.HashPassword(sifreKutusu.Password),
                    Unvan = unvan.Text,
                    IsimSoyisim = isimSoyisim.Text,
                    Yetki = (Yetkiler)YetkiComBox.SelectedItem
                };

                using (var db = new DbConnection())
                {
                    if (db.Kullanicilar.First(s => s.KullaniciAdi == yeniKullanici.KullaniciAdi) == null)
                    {
                        db.Kullanicilar.Add(yeniKullanici);
                        db.SaveChanges();
                        MessageBox.Show("Kullanıcı kaydı başarılı.");
                        Close();
                    }
                    else
                    {
                        Hatalar.Items.Add(" • Bu kullanıcı adıyla daha önce kayıt yapılmış.");
                    }
                }
            }
        }

        private bool Cabbar_Izin()
        {
            Hatalar.Items.Clear();

            //username has to be start with letters and contains letters and
            //number characters and dots in 6 to 20 length
            //from: https://stackoverflow.com/a/10213497
            bool kullaniciAdiDogru = new Regex(@"^[a-zA-Z][a-zA-Z0-9.]{5,19}$").IsMatch(kullaniciAdi.Text);
            if (!kullaniciAdiDogru) { Hatalar.Items.Add(" • Kullanıcı adı en az 6, en fazla 20 karakter içermelidir.\nKullanıcı adı sayıyla başlayamaz ve nokta hariç özel karakter içeremez."); }

            //Minimum eight characters, at least one letter and one number
            //other than that every character is acceptable, except whitespaces.
            //from: https://stackoverflow.com/a/21456918
            bool sifreDogru = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[^\s]{8,}$").IsMatch(sifreKutusu.Password);
            if (!sifreDogru) { Hatalar.Items.Add(" • Şifre en az 8 karakterden oluşmalı ve en az bir harf, en az bir rakam içermelidir."); }

            bool sifrelerAyni = sifreKutusu.Password == sifreKutusuTekrar.Password;
            if (!sifrelerAyni) { Hatalar.Items.Add(" • Şifreler aynı değil."); }

            bool bosYerKalmasin = unvan.Text.Length > 0 && isimSoyisim.Text.Length > 0 && YetkiComBox.SelectedItem != null;
            if (!bosYerKalmasin) { Hatalar.Items.Add(" • Formda boş kutucuk kalmamalıdır."); }

            return kullaniciAdiDogru && sifreDogru && sifrelerAyni && bosYerKalmasin;
        }
    }
}
