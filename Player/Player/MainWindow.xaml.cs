using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
        }

        
        [DllImport("user32.dll")]
        static extern uint GetDoubleClickTime();

        System.Timers.Timer timeClick = new System.Timers.Timer((int)GetDoubleClickTime())
        {
            AutoReset = false
        };

        bool fullScreen = false;

        private void MediaEL_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!timeClick.Enabled)
            {
                timeClick.Enabled = true;
                return;
            }

            if (timeClick.Enabled)
            {
                if (!fullScreen)
                {
                    LayoutRoot.Children.Remove(mediaElement1);
                    this.Background = new SolidColorBrush(Colors.Black);
                    this.Content = mediaElement1;
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.Content = LayoutRoot;
                    LayoutRoot.Children.Add(mediaElement1);
                    this.Background = new SolidColorBrush(Colors.White);
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Normal;
                }
                fullScreen = !fullScreen;
            }
        }

        void  timer_Tick(object sender, EventArgs e)
        {
 	        Slide_Barre.Value = mediaElement1.Position.TotalSeconds;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            mediaElement1.Play();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            mediaElement1.Pause();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            mediaElement1.Stop();
        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement1.Volume = (double)Slide_Volume.Value;
        }

        private void Slide_Barre_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement1.Position = TimeSpan.FromSeconds(Slide_Barre.Value);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string filename = (string)((DataObject)e.Data).GetFileDropList()[0];
            mediaElement1.Source = new Uri(filename);

            mediaElement1.LoadedBehavior = MediaState.Manual;
            mediaElement1.UnloadedBehavior = MediaState.Manual;
            mediaElement1.Volume = (double)Slide_Volume.Value;
            mediaElement1.Play();
        }

        private void mediaElement1_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (mediaElement1.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = mediaElement1.NaturalDuration.TimeSpan;
                Slide_Barre.Maximum = ts.TotalSeconds;
                timer.Start();
            }
        }
    }  
   // public  EventHandler timer_Tick { get; set; }}
}