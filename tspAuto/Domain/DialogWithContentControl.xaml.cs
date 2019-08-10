using System.Windows.Controls;

namespace tspAuto.Domain
{
    /// <summary>
    /// Interaction logic for DialogWithContentControl.xaml
    /// </summary>
    public partial class DialogWithContentControl : UserControl
    {
        public DialogWithContentControl(UserControl ıcerik)
        {
            InitializeComponent();

            Icerik.Content = ıcerik;
        }
    }
}
