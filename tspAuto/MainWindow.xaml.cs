﻿using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        private ContextMenu contextMenu = new ContextMenu();
        private MenuItem menuItem1 = new MenuItem();
        private MenuItem seperator = new MenuItem();
        private MenuItem menuItem2 = new MenuItem();
        public NotifyIcon notifyIcon = new NotifyIcon();
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
            contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem1, menuItem2, seperator });

            // Initialize menuItem1
            menuItem1.Index = 0;
            menuItem1.Text = "Programı &Aç";
            menuItem1.Click += new EventHandler(notifyIcon_DoubleClick);
            menuItem1.DefaultItem = true;

            // Initialize seperator
            seperator.Index = 1;
            seperator.Text = "-";

            // Initialize menuItem2
            menuItem2.Index = 2;
            menuItem2.Text = "&Kapat";
            menuItem2.Click += new EventHandler(menuItem1_Click);

            // Create the NotifyIcon.
            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon.ContextMenu = contextMenu;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon.Text = Properties.Resources.Title;
            notifyIcon.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
            notifyIcon.BalloonTipClicked += new EventHandler(notifyIcon_BalloonTipClicked);
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            notification = new Notification((sender as NotifyIcon).BalloonTipText, (sender as NotifyIcon).BalloonTipTitle);
            notification.Show();
        }

        private void notifyIcon_DoubleClick(object Sender, EventArgs e)
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
                foreach (PanelItem item in SolPanelListBox.Items)
                {
                    if (item.Content.GetType() == typeof(AramaYap))
                    {
                        SolPanelListBox.SelectedIndex = 1;
                        (item.Content as AramaYap).AramaKutusu.Text = AramaTextbox.Text;
                        AramaTextbox.Text = string.Empty;
                        break;
                    }
                }
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
                notifyIcon.Visible = false;

                foreach (PanelItem item in SolPanelListBox.Items)
                {
                    if (item.Content.GetType() == typeof(Hatirlatici))
                    {
                        try
                        {
                            if (Properties.Settings.Default.DatabaseFilePath != "" && System.IO.File.Exists(Properties.Settings.Default.DatabaseFilePath))
                            {
                                string[] columns = new string[]
                                {
                                    "Baslik",
                                    "Aciklama",
                                    "Zaman",
                                    "HatirlaticiTablo",
                                    "HatirlaticiID"
                                };

                                bool basarili = false;

                                IReadOnlyCollection<TriggerKey> allTriggerKeys = (item.Content as Hatirlatici).scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup()).GetAwaiter().GetResult();

                                try
                                {
                                    using (SQLiteConnection con = new SQLiteConnection($"Data Source={Properties.Settings.Default.DatabaseFilePath};"))
                                    {
                                        con.Open();
                                        new SQLiteCommand("DELETE FROM Hatirlaticilar;", con).ExecuteNonQuery();
                                        foreach (TriggerKey triggerKey in allTriggerKeys)
                                        {
                                            ITrigger triggerdetails = (item.Content as Hatirlatici).scheduler.GetTrigger(triggerKey).GetAwaiter().GetResult();
                                            IJobDetail jobDetail = (item.Content as Hatirlatici).scheduler.GetJobDetail(triggerdetails.JobKey).GetAwaiter().GetResult();

                                            object[] values = new object[]
                                            {
                                                jobDetail.JobDataMap.GetString("Baslik"),
                                                jobDetail.JobDataMap.GetString("Aciklama"),
                                                triggerdetails.StartTimeUtc.DateTime.ToString("yyyy.MM.dd.HH.mm"),
                                                jobDetail.JobDataMap.GetString("Tablo"),
                                                jobDetail.JobDataMap.GetInt("ID")
                                            };

                                            MethodPack.Generate_Insert_Command("Hatirlaticilar", columns, values, con).ExecuteNonQuery();
                                        }

                                        basarili = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Windows.MessageBox.Show("Veritabanı işlemi sırasında bir hata oluştu.\n\n" + ex.Message);
                                }
                                finally
                                {
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                    if (!basarili)
                                    {
                                        System.Windows.MessageBox.Show("Veritabanı girdisi yapılamadı.");
                                    }
                                }
                            }
                            else if (Properties.Settings.Default.DatabaseFilePath == "")
                            {
                                System.Windows.MessageBox.Show("Veritabanı seçilmemiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                            }
                            else if (!System.IO.File.Exists(Properties.Settings.Default.DatabaseFilePath))
                            {
                                System.Windows.MessageBox.Show("Veritabanı silinmiş ya da erişim engellenmiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                            }
                        }
                        catch (System.IO.DirectoryNotFoundException)
                        {
                            System.Windows.MessageBox.Show("Bazı dosyalar silinmiş ya da erişim engellenmiş. Yeni bir veritabanı oluşturun ya da var olan bir veritabanı seçin.");
                        }

                        // and last shut down the scheduler when you are ready to close your program
                        (item.Content as Hatirlatici).scheduler.Shutdown();
                        break;
                    }
                }
            }
            else
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
