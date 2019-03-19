using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace tspAuto
{
    public partial class Notification : Window
    {
        public Notification(string message, string title = "")
        {
            InitializeComponent();

            //force to bottom right
            Rect desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;

            //set title
            if (title == "")
            {
                lblTitle.Content = Properties.Resources.Title;
            }
            else
            {
                lblTitle.Content = title;
            }

            //set message
            txtMessage.Text = message;

            ShowActivated = false;
        }

        DispatcherTimer fadeTimer = new DispatcherTimer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fadeTimer.Interval = TimeSpan.FromMilliseconds(50);
            fadeTimer.Start();
            fadeTimer.Tick += FadeTimer_Tick;
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            Opacity -= .01;

            if (Opacity <= 0) Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            fadeTimer.Stop();
            Opacity = 1;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            fadeTimer.Start();
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            fadeTimer.Start();
        }
    }
}
