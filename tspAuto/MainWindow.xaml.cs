using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using tspAuto.Domain;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContextMenu contextMenu1 = new ContextMenu();
        private MenuItem menuItem1 = new MenuItem();
        private MenuItem menuItem2 = new MenuItem();
        private NotifyIcon notifyIcon1 = new NotifyIcon();
        private static Notification notification;
        bool reallyClose = false;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize contextMenu1
            contextMenu1.MenuItems.AddRange(new MenuItem[] { menuItem1, menuItem2 });

            // Initialize menuItem1
            menuItem1.Index = 0;
            menuItem1.Text = "Programı Aç";
            menuItem1.Click += new EventHandler(notifyIcon1_DoubleClick);

            // Initialize menuItem2
            menuItem2.Index = 1;
            menuItem2.Text = "Kapat";
            menuItem2.Click += new EventHandler(menuItem1_Click);

            // Create the NotifyIcon.
            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = Properties.Resources.Title;
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new EventHandler(notifyIcon1_DoubleClick);
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (!IsVisible)
            {
                Show();
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }

            // Activate the form.
            Activate();
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            reallyClose = true;
            Close();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is System.Windows.Controls.Primitives.ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        private void AramaKomutu(object sender, RoutedEventArgs e)
        {
            if (AramaTextbox.Text.Length >= 1)
            {
                Properties.Settings.Default.SonArama = AramaTextbox.Text;
                AramaTextbox.Text = string.Empty;
                SolPanelListBox.SelectedIndex = 1;

                notification = new Notification("Arama yapıldı beybii");
                notification.Show();
            }
        }

        private void AramaTextbox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AramaKomutu(new object(), new RoutedEventArgs());
            }
        }

        private void Temizle_Button_Click(object sender, RoutedEventArgs e)
        {
            AramaTextbox.Text = string.Empty;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (reallyClose)
            {
                notifyIcon1.Visible = false;
                Properties.Settings.Default.SonArama = string.Empty;
                Properties.Settings.Default.Save();
            }
            else
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
