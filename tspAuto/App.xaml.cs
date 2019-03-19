using System.Windows;

namespace tspAuto
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex _mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            _mutex = new System.Threading.Mutex(true, tspAuto.Properties.Resources.Title, out bool createdNew);

            if (!createdNew)
            {
                //app is already running! Exiting the application
                MessageBox.Show($"{tspAuto.Properties.Resources.Title} programı zaten arkada çalışıyor.");
                Current.Shutdown();
            }

            base.OnStartup(e);
        }
    }
}
