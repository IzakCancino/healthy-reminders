using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace healthy_reminders
{
    public class EventTimer
    {
        public HealthEvent HealthEvent;
        public bool OnEventMode = false;
        public bool OnWaiting = true;

        // Window and elements variables
        private readonly MainWindow _mainWindow;
        public Dictionary<string, System.Windows.Controls.Label> ControlLabels;
        public Dictionary<string, System.Windows.Controls.Button> ControlButtons;

        // Timer and time variables
        public DispatcherTimer DispatcherTimer;
        public TimeSpan DelayTime;
        public TimeSpan EventTime;
        public TimeSpan CountdownTime;

        public EventTimer(
            MainWindow window, 
            TimeSpan delayTime, 
            TimeSpan eventTime, 
            HealthEvent healthEvent,
            Dictionary<string, System.Windows.Controls.Label> controlLabels,
            Dictionary<string, System.Windows.Controls.Button> controlButtons
        )
        {
            HealthEvent = healthEvent;

            _mainWindow = window;
            ControlLabels = controlLabels;
            ControlButtons = controlButtons;

            // Create and adjust the timer
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            DispatcherTimer.Interval = TimeSpan.FromSeconds(1);

            // Time variables
            DelayTime = delayTime;
            CountdownTime = delayTime;
            EventTime = eventTime;
        }

        // In every tick of the timer
        public void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            // Reduce one second in the timer and show it
            CountdownTime = CountdownTime.Subtract(TimeSpan.FromSeconds(1));
            UpdateCountdown();

            // Once the countdown reaches 00:00
            if (CountdownTime.Ticks <= 0)
            {
                Reset(false);

                if (OnEventMode)
                {
                    OnEventMode = false;
                    UpdateControls();

                    ControlLabels["Countdown"].FontWeight = FontWeights.Normal;
                    ControlLabels["Countdown"].Foreground = System.Windows.Media.Brushes.Black;

                    _mainWindow.NotificationManager.ShowNotification(
                        HealthEvent.Name,
                        "Event ended!"
                    );
                }
                else
                {
                    // Not event timer available for health event "PostureHealth"
                    if (HealthEvent.HasTimerEvent)
                    {
                        OnEventMode = true;
                        UpdateControls();

                        ControlLabels["Countdown"].FontWeight = FontWeights.Bold;
                        ControlLabels["Countdown"].Foreground = System.Windows.Media.Brushes.DarkGreen;
                    }

                    Random rand = new Random();
                    _mainWindow.NotificationManager.ShowNotification(
                        HealthEvent.Name,
                        HealthEvent.Alerts[rand.Next(HealthEvent.Alerts.Length)]
                    );
                }

                Reset();

                if (!HealthEvent.HasTimerEvent)
                {
                    Start();
                }
            }
        }

        // Start timer
        public void Start()
        {
            OnWaiting = false;
            UpdateControls();

            UpdateCountdownType();
            DispatcherTimer.Start();

            ControlLabels["Countdown"].FontWeight = FontWeights.Normal;
            ControlLabels["Countdown"].Foreground = System.Windows.Media.Brushes.Gray;
        }        

        // Reset timer
        public void Reset(bool resetEventMode = true)
        {
            OnWaiting = true;
            OnEventMode = (resetEventMode ? false : OnEventMode);
            UpdateControls();

            UpdateCountdownType();
            DispatcherTimer.Stop();

            ControlLabels["Countdown"].FontWeight = FontWeights.Normal;
            ControlLabels["Countdown"].Foreground = System.Windows.Media.Brushes.Black;
        }

        // Skip timer of event itself
        public void Skip()
        {
            // Reset current timer
            OnWaiting = true;
            DispatcherTimer.Stop();

            // Update labels and controls
            ControlLabels["Countdown"].FontWeight = (OnEventMode ? FontWeights.Normal : FontWeights.Bold);
            ControlLabels["Countdown"].Foreground = (OnEventMode ? System.Windows.Media.Brushes.Black : System.Windows.Media.Brushes.DarkGreen);

            OnEventMode = !OnEventMode;
            UpdateControls();

            UpdateCountdownType();
        }

        // Update countdown timer
        public void UpdateCountdown()
        {
            ControlLabels["Countdown"].Content = CountdownTime.ToString(
                CountdownTime.Hours >= 1 ? @"hh\:mm\:ss" : @"mm\:ss"
            );
        }

        // Update time goal in the countdown based on the type
        public void UpdateCountdownType()
        {
            CountdownTime = OnEventMode ? EventTime : DelayTime;
            UpdateCountdown();
        }

        // Update control buttons based on countdown states
        public void UpdateControls()
        {
            ControlButtons["Play"].IsEnabled = (OnWaiting);
            ControlButtons["Reset"].IsEnabled = (OnEventMode || !OnWaiting);
            ControlButtons["Skip"].IsEnabled = (OnEventMode || !OnWaiting);

            string message = "(";
            message += OnWaiting ? "Waiting to start " : "Running ";
            message += OnEventMode ? "event time" : "event delay";
            message += ")";
            ControlLabels["Status"].Content = message;
        }
    }
}
