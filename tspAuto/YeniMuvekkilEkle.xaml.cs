using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for YeniMuvekkilEkle.xaml
    /// </summary>
    public partial class YeniMuvekkilEkle : UserControl
    {
        public YeniMuvekkilEkle()
        {
            InitializeComponent();

            TarihSec.Language = System.Windows.Markup.XmlLanguage.GetLanguage("tr-TR");
        }

        private void Muvekkil_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ANAN");
        }

        private void Sirket_Kaydet_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ANAN");
        }
    }
}
