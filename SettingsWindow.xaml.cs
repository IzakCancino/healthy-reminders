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
using System.Windows.Shapes;

namespace healthy_reminders
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly MainWindow _mainWindow;

        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            CheckTimersOnStart.IsChecked = Properties.Settings.Default.TimersOnStart;
            CheckMinimizedOnStart.IsChecked = Properties.Settings.Default.MinimizedOnStart;

            InputDelayEyeCare.Text = _mainWindow.EventTimers["EyeCare"].DelayTime.TotalSeconds.ToString();
            InputEventEyeCare.Text = _mainWindow.EventTimers["EyeCare"].EventTime.TotalSeconds.ToString();

            InputDelayPostureHealth.Text = _mainWindow.EventTimers["PostureHealth"].DelayTime.TotalSeconds.ToString();

            InputDelayPhysicalActivity.Text = _mainWindow.EventTimers["PhysicalActivity"].DelayTime.TotalSeconds.ToString();
            InputEventPhysicalActivity.Text = _mainWindow.EventTimers["PhysicalActivity"].EventTime.TotalSeconds.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Owner.Show();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Values validation
            if (!double.TryParse(InputDelayEyeCare.Text, out double valueDelayEyeCare))
            {
                InputDelayEyeCare.Text = string.Empty;
                InputDelayEyeCare.Focus();
                return;
            }
            if (!double.TryParse(InputEventEyeCare.Text, out double valueEventEyeCare))
            {
                InputEventEyeCare.Text = string.Empty;
                InputEventEyeCare.Focus();
                return;
            }

            if (!double.TryParse(InputDelayPostureHealth.Text, out double valueDelayPostureHealth))
            {
                InputDelayPostureHealth.Text = string.Empty;
                InputDelayPostureHealth.Focus();
                return;
            }

            if (!double.TryParse(InputDelayPhysicalActivity.Text, out double valueDelayPhysicalActivity))
            {
                InputDelayPhysicalActivity.Text = string.Empty;
                InputDelayPhysicalActivity.Focus();
                return;
            }
            if (!double.TryParse(InputEventPhysicalActivity.Text, out double valueEventPhysicalActivity))
            {
                InputEventPhysicalActivity.Text = string.Empty;
                InputEventPhysicalActivity.Focus();
                return;
            }

            // Update timers' values
            _mainWindow.EventTimers["EyeCare"].DelayTime = TimeSpan.FromSeconds(valueDelayEyeCare);
            _mainWindow.EventTimers["EyeCare"].EventTime = TimeSpan.FromSeconds(valueEventEyeCare);

            _mainWindow.EventTimers["PostureHealth"].DelayTime = TimeSpan.FromSeconds(valueDelayPostureHealth);

            _mainWindow.EventTimers["PhysicalActivity"].DelayTime = TimeSpan.FromSeconds(valueDelayPhysicalActivity);
            _mainWindow.EventTimers["PhysicalActivity"].EventTime = TimeSpan.FromSeconds(valueEventPhysicalActivity);

            // Reset timers
            _mainWindow.EventTimers["EyeCare"].Reset();
            _mainWindow.EventTimers["PostureHealth"].Reset();
            _mainWindow.EventTimers["PhysicalActivity"].Reset();

            // Save settings
            Properties.Settings.Default.MinimizedOnStart = CheckMinimizedOnStart.IsChecked.GetValueOrDefault();
            Properties.Settings.Default.TimersOnStart = CheckTimersOnStart.IsChecked.GetValueOrDefault();

            Properties.Settings.Default.TimerDelayEyeCareInSeconds = valueDelayEyeCare;
            Properties.Settings.Default.TimerEventEyeCareInSeconds = valueEventEyeCare;

            Properties.Settings.Default.TimerDelayPostureHealthInSeconds = valueDelayPostureHealth;
            
            Properties.Settings.Default.TimerDelayPhysicalActivityInSeconds = valueDelayPhysicalActivity;
            Properties.Settings.Default.TimerEventPhysicalActivityInSeconds = valueEventPhysicalActivity;

            Properties.Settings.Default.Save();            

            BtnCancel_Click(sender, e);
        }
    }
}
