using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace healthy_reminders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly EventTimer _eventTimerEyeCare;

        private NotificationManager _notificationManager;
        internal NotificationManager NotificationManager { 
            get => _notificationManager; 
            set => _notificationManager = value; 
        }

        public MainWindow()
        {
            InitializeComponent();

            NotificationManager = new NotificationManager(this);

            // Eye care timer
            _eventTimerEyeCare = new EventTimer(
                this, 
                TimeSpan.FromMinutes(20), 
                new HealthEvent("Eye care"), 
                LabelTime
            );
            _eventTimerEyeCare.Stop();
        }



        /*
         Control buttons for "Eye care" timer
         */

        private void BtnEyeCarePlay_Click(object sender, RoutedEventArgs e)
        {
            _eventTimerEyeCare.Start();
        }

        private void BtnEyeCareStop_Click(object sender, RoutedEventArgs e)
        {
            _eventTimerEyeCare.Stop();
        }



        // Handle minimize to system tray
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (WindowState == WindowState.Minimized)
            {
                NotificationManager.HideApp();
            }
        }

        // Executed on main window closed
        private void Window_Closed(object sender, EventArgs e)
        {
            NotificationManager.CloseApp();
        }
    }
}