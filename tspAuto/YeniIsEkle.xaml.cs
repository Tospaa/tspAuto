using System;
using System.Windows;
using System.Windows.Controls;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for YeniIsEkle.xaml
    /// </summary>
    public partial class YeniIsEkle : UserControl
    {
        public YeniIsEkle()
        {
            InitializeComponent();

            TarihSec.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).notifyIcon.BalloonTipTitle = DateTime.Now.ToString("yyyyMMddHHmmss");
                    (window as MainWindow).notifyIcon.BalloonTipText = "Ebesininkinden notification yolladım.";
                    (window as MainWindow).notifyIcon.ShowBalloonTip(5000);
                }
            }
        }
    }
}
