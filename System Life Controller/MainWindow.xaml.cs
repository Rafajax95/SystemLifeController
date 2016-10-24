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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using System.ComponentModel;

namespace System_Life_Controller
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int hours, minutes, seconds;
        int textHours, textMinutes;
        DateTime ActionTime, NowTime;
        DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Forms.NotifyIcon notify;
        TimerWidget widget;




        #region Settings
        void SaveSettings()
        {
            FileStream fs = new FileStream("settings.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(TimerWidgetCB.IsChecked);
            sw.WriteLine(ShutDownRB.IsChecked);
            sw.WriteLine(SleepRB.IsChecked);
            sw.WriteLine(RestartRB.IsChecked);
            sw.WriteLine(LogoutRB.IsChecked);
            sw.WriteLine(NormalRB.IsChecked);
            sw.WriteLine(MinimalizeRB.IsChecked);
            sw.WriteLine(GoToTrayRB.IsChecked);
            sw.WriteLine(BlackRB.IsChecked);
            sw.WriteLine(BlueRB.IsChecked);
            sw.WriteLine(GreenRB.IsChecked);
            sw.WriteLine(RedRB.IsChecked);
            sw.WriteLine(WhiteRB.IsChecked);
            sw.Close();
        }

        void LoadSettings()
        {
            try
            {
                FileStream fs = new FileStream("settings.txt", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                TimerWidgetCB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                ShutDownRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                SleepRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                RestartRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                LogoutRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                NormalRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                MinimalizeRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                GoToTrayRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                BlackRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                BlueRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                GreenRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                RedRB.IsChecked = Convert.ToBoolean(sr.ReadLine());
                WhiteRB.IsChecked = Convert.ToBoolean(sr.ReadLine());


                sr.Close();
            }

            catch
            {
                FileStream fs = new FileStream("settings.txt", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("true");
                sw.WriteLine("true");
                sw.WriteLine("false");
                sw.WriteLine("false");
                sw.WriteLine("false");
                sw.WriteLine("true");
                sw.WriteLine("false");
                sw.WriteLine("false");
                sw.WriteLine("true");
                sw.WriteLine("false");
                sw.WriteLine("false");
                sw.WriteLine("false");
                sw.WriteLine("false");
                sw.Close();
                LoadSettings();
            }


        }
        #endregion

        private void TimerActualization()
        {
            TimerLB.Content = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
            if (TimerLB.Content.ToString() != "00:00:00") StartBT.IsEnabled = true;
            else StartBT.IsEnabled = false;
        }

        public MainWindow()
        {
            InitializeComponent();

            notify = new System.Windows.Forms.NotifyIcon();
            notify.Icon = Properties.Resources.icon48;
            notify.Click += new EventHandler(notify_Click);


            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            SetHourRB.IsChecked = true;
            LoadSettings();
            hours = 0;
            minutes = 0;
            seconds = 0;
        }

        private void notify_Click(object sender, EventArgs e)
        {
            Show();
            notify.Visible = false;
        }

        private void SetTimeRB_Checked(object sender, RoutedEventArgs e)
        {
            HoursTB.IsEnabled = true;
            MinutesTB.IsEnabled = true;
            TimeHourTB.IsEnabled = false;
            TimeMinutesTB.IsEnabled = false;
            DateDP.IsEnabled = false;
            HoursTB_LostFocus(null, null);
            Minutes_LostFocus(null, null);


        }

        private void SetHourRB_Checked(object sender, RoutedEventArgs e)
        {
            HoursTB.IsEnabled = false;
            MinutesTB.IsEnabled = false;
            TimeHourTB.IsEnabled = true;
            TimeMinutesTB.IsEnabled = true;
            DateDP.IsEnabled = true;
            TimeHourTB_LostFocus(null, null);
            TimeMinutes_LostFocus(null, null);
        }



        private void SaveSettingsBT_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }



        private void TimeTextBoxes_KeyEnter(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Keyboard.ClearFocus();
        }


        private void DateDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            seconds = 0;
            NowTime = DateTime.Now;

            if (DateDP.SelectedDate != null) ActionTime = new DateTime(DateDP.SelectedDate.Value.Year, DateDP.SelectedDate.Value.Month, DateDP.SelectedDate.Value.Day, textHours, textMinutes, 0);
            else ActionTime = new DateTime(NowTime.Year, NowTime.Month, NowTime.Day, textHours, textMinutes, 0);


            if (ActionTime > NowTime)
            {
                hours = (int)ActionTime.Subtract(NowTime).TotalHours;
                minutes = (int)ActionTime.Subtract(NowTime).TotalMinutes;
                minutes -= hours * 60;
                TimerActualization();
            }
        }

        private void TimeHourTB_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textHours = int.Parse(TimeHourTB.Text);
                if (textHours < 0) textHours = 0;
                if (textHours >= 23) textHours = 23;

                TimeHourTB.Text = textHours.ToString("D2");
                DateDP_SelectedDateChanged(null, null);
            }
            catch
            {
                TimeHourTB.Text = "";
                textHours = 00;
            }
        }

        private void TimeMinutes_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textMinutes = int.Parse(TimeMinutesTB.Text);
                if (textMinutes < 0) textMinutes = 0;
                if (textMinutes >= 60) textMinutes = 59;


                TimeMinutesTB.Text = textMinutes.ToString("D2");
                DateDP_SelectedDateChanged(null, null);
            }
            catch
            {
                TimeMinutesTB.Text = "";
                textMinutes = 0;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (widget != null)
            {
                widget.Close();
                widget = null;
            }

        }

        private void HoursTB_LostFocus(object sender, RoutedEventArgs e)
        {
            seconds = 0;
            try
            {
                hours = int.Parse(HoursTB.Text);
                if (hours < 0) hours = 0;
                HoursTB.Text = hours.ToString("D2");
                TimerActualization();
            }
            catch
            {
                HoursTB.Text = "";
                hours = 00;
            }
        }



        private void Minutes_LostFocus(object sender, RoutedEventArgs e)
        {
            seconds = 0;
            try
            {
                minutes = int.Parse(MinutesTB.Text);
                if (minutes < 0) minutes = 0;
                if (minutes > 60) minutes = 60;
                MinutesTB.Text = minutes.ToString("D2");
                TimerActualization();
            }
            catch
            {
                MinutesTB.Text = "";
                minutes = 0;
            }
        }


        private void StartBT_Click(object sender, RoutedEventArgs e)
        {

            if (StartBT.Content.ToString() == "START")
            {
                dispatcherTimer.Start();
                StartBT.Content = "Zatrzymaj";
                TimerWidgetCB.IsEnabled = false;
                settingsGrid.IsEnabled = false;
                OperationGrid.IsEnabled = false;
                if (MinimalizeRB.IsChecked == true) WindowState = WindowState.Minimized;
                if (GoToTrayRB.IsChecked == true)
                {
                    Hide();
                    notify.Visible = true;
                }

                if (TimerWidgetCB.IsChecked == true)
                {
                    string color = "";
                    if (RedRB.IsChecked == true) color = "red";
                    if (BlackRB.IsChecked == true) color = "black";
                    if (GreenRB.IsChecked == true) color = "green";
                    if (WhiteRB.IsChecked == true) color = "white";
                    if (BlueRB.IsChecked == true) color = "blue";
                    widget = new TimerWidget(hours, minutes, seconds, color);
                    widget.Show();
                }

            }
            else
            {
                dispatcherTimer.Stop();
                StartBT.Content = "START";
                TimerWidgetCB.IsEnabled = true;
                settingsGrid.IsEnabled = true;
                OperationGrid.IsEnabled = true;
                if (widget != null)
                {
                    widget.Close();
                    widget = null;
                }

            }

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            seconds--;
            if (seconds < 0)
            {
                seconds = 59;
                minutes--;

            }
            if (minutes < 0)
            {
                minutes = 59;
                hours--;
            }

            if (hours <= 0 && minutes <= 0 && seconds <= 0)
            {
                dispatcherTimer.Stop();
                StartBT.Content = "START";
                TimerWidgetCB.IsEnabled = true;
                settingsGrid.IsEnabled = true;
                OperationGrid.IsEnabled = true;
                if (ShutDownRB.IsChecked == true) System.Diagnostics.Process.Start("shutdown", "-s -t 0");
                if (SleepRB.IsChecked == true) System.Diagnostics.Process.Start("shutdown", "-h");
                if (LogoutRB.IsChecked == true) System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                if (RestartRB.IsChecked == true) System.Diagnostics.Process.Start("shutdown","-r -t 0");
            }
            TimerActualization();
        }
    }
}
