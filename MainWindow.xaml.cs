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
        public Dictionary<string, EventTimer> EventTimers = new Dictionary<string, EventTimer>();

        private NotificationManager _notificationManager;
        internal NotificationManager NotificationManager {
            get => _notificationManager;
            set => _notificationManager = value;
        }

        public MainWindow()
        {
            InitializeComponent();

            NotificationManager = new NotificationManager(this);

            // Declaration of timers based on user settings
            EventTimers.Add(
                "EyeCare",
                new EventTimer(
                    this,
                    TimeSpan.FromSeconds(Properties.Settings.Default.TimerDelayEyeCareInSeconds),
                    TimeSpan.FromSeconds(Properties.Settings.Default.TimerEventEyeCareInSeconds),
                    new HealthEvent("Eye care"),
                    new Dictionary<string, System.Windows.Controls.Label>()
                    {
                        {"Countdown", LabelEyeCareTime},
                        {"Status", LabelEyeCareStatus}
                    },
                    new Dictionary<string, System.Windows.Controls.Button>()
                    {
                        {"Play", BtnEyeCarePlay},
                        {"Stop", BtnEyeCareStop},
                        {"Skip", BtnEyeCareSkip}
                    }
                )
            );

            // Update timers' labels
            foreach (var (_, eventTimer) in EventTimers)
            {
                eventTimer.UpdateCountdown();
            }

            // If `TimersOnStart` setting is true, start all timers
            if (Properties.Settings.Default.TimersOnStart) {
                foreach (var (_, eventTimer) in EventTimers)
                {
                    eventTimer.Start();
                }
            }

            // If `MinimizedOnStart` setting is true, minimize the app window
            if (Properties.Settings.Default.MinimizedOnStart)
            {
                WindowState = WindowState.Minimized;
                OnStateChanged(EventArgs.Empty);
            }
        }



        /*
         Control buttons for "Eye care" timer
         */

        private void BtnEyeCarePlay_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["EyeCare"].Start();
        }

        private void BtnEyeCareStop_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["EyeCare"].Stop();
        }

        private void BtnEyeCareSkip_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["EyeCare"].Skip();
        }




        // Setting button clicked
        private void BtnSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(this);
            settingsWindow.Owner = this;
            this.Hide();
            settingsWindow.Show();
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