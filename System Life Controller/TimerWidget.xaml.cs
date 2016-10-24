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
using System.Windows.Threading;

namespace System_Life_Controller
{
    /// <summary>
    /// Interaction logic for TimerWidget.xaml
    /// </summary>
    public partial class TimerWidget : Window
    {

        DispatcherTimer dispatcherTimer;
        int hours, minutes, seconds;

        public TimerWidget(int hours, int minutes, int seconds, string color)
        {

            InitializeComponent();

            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;

            TimerLB.Content = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer.Start();

            
            if (color == "red")
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(255,0,0), Color.FromRgb(43,43,43), new Point(0.5, 0), new Point(0.5, 1));
                TimerLB.Foreground = gradientBrush;
            }
            if (color == "blue")
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(85,85,85), Color.FromRgb(0,0,255), new Point(0.5, 0), new Point(0.5, 1));
                TimerLB.Foreground = gradientBrush;
            }
            if (color == "black")
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(0, 0, 0), Color.FromRgb(40,40,40), new Point(0.5, 0), new Point(0.5, 1));
                TimerLB.Foreground = gradientBrush;
            }
            if (color == "green")
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(0, 0, 0), Color.FromRgb(0,128,0), new Point(0.5, 0), new Point(0.5, 1));
                TimerLB.Foreground = gradientBrush;
            }
            if (color == "white")
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush(Color.FromRgb(247,247,247), Color.FromRgb(68,68,68), new Point(0.5, 0), new Point(0.5, 1));
                TimerLB.Foreground = gradientBrush;
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
            }
            TimerLB.Content = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
        }


        private void TimerLB_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseLB_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }
    }
}
