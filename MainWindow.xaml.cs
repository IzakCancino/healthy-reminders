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
                        {"Reset", BtnEyeCareReset},
                        {"Skip", BtnEyeCareSkip}
                    }
                )
            );
            EventTimers.Add(
                "PostureHealth",
                new EventTimer(
                    this,
                    TimeSpan.FromSeconds(Properties.Settings.Default.TimerDelayPostureHealthInSeconds), 
                    TimeSpan.FromSeconds(Properties.Settings.Default.TimerEventPostureHealthInSeconds),
                    new HealthEvent("Posture health"),
                    new Dictionary<string, System.Windows.Controls.Label>()
                    {
                        {"Countdown", LabelPostureHealthTime},
                        {"Status", LabelPostureHealthStatus}
                    },
                    new Dictionary<string, System.Windows.Controls.Button>()
                    {
                        {"Play", BtnPostureHealthPlay},
                        {"Reset", BtnPostureHealthReset},
                        {"Skip", new System.Windows.Controls.Button()}
                    }
                )
            ); 
            EventTimers.Add(
                "PhysicalActivity",
                new EventTimer(
                    this,
                    TimeSpan.FromSeconds(Properties.Settings.Default.TimerDelayPhysicalActivityInSeconds),
                    TimeSpan.FromSeconds(Properties.Settings.Default.TimerEventPhysicalActivityInSeconds),
                    new HealthEvent("Physical activity"),
                    new Dictionary<string, System.Windows.Controls.Label>()
                    {
                        {"Countdown", LabelPhysicalActivityTime},
                        {"Status", LabelPhysicalActivityStatus}
                    },
                    new Dictionary<string, System.Windows.Controls.Button>()
                    {
                        {"Play", BtnPhysicalActivityPlay},
                        {"Reset", BtnPhysicalActivityReset},
                        {"Skip", BtnPhysicalActivitySkip}
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

        private void BtnEyeCareReset_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["EyeCare"].Reset();
        }

        private void BtnEyeCareSkip_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["EyeCare"].Skip();
        }



        /*
         Control buttons for "Posture Health" timer
         */

        private void BtnPostureHealthPlay_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["PostureHealth"].Start();
        }

        private void BtnPostureHealthReset_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["PostureHealth"].Reset();
        }



        /*
         Control buttons for "Physical activity" timer
         */

        private void BtnPhysicalActivityPlay_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["PhysicalActivity"].Start();
        }

        private void BtnPhysicalActivityReset_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["PhysicalActivity"].Reset();
        }

        private void BtnPhysicalActivitySkip_Click(object sender, RoutedEventArgs e)
        {
            EventTimers["PhysicalActivity"].Skip();
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