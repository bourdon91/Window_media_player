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
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace MyWindowMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        bool fullscreen = false;
        bool videoPlay = false;
        bool visible = true;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(Time_Tick);
            LoadBibliotheque();
        }

        private void LoadBibliotheque()
        {
            System.Windows.Forms.ListViewItem item1 = new System.Windows.Forms.ListViewItem("riri", 0);
            System.Windows.Forms.ListViewItem item2 = new System.Windows.Forms.ListViewItem("fifi", 1);
            System.Windows.Forms.ListViewItem item3 = new System.Windows.Forms.ListViewItem("loulou", 0);
            this.listBibliotheque.Items.Add(item1);
            this.listBibliotheque.Items.Add(item2);
            this.listBibliotheque.Items.Add(item3);
            this.listBibliotheque.Items.Add("riri");
            this.listBibliotheque.Items.Add("fifi");
            this.listBibliotheque.Items.Add("loulou");
        }

        void Time_Tick(object sender, EventArgs e)
        {
            this.sliderVideo.Value = this.video.Position.TotalSeconds;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.visible == true)
            {
                this.controlPanel.Opacity = 0.0;
                this.visible = false;
            }
            else
            {
                this.controlPanel.Opacity = 1.0;
                this.visible = true;
            }

        }

        private void Window_Drop(object sender, System.Windows.DragEventArgs e)
        {
            string filename = (string)((System.Windows.DataObject)e.Data).GetFileDropList()[0];

            this.video.Source = new Uri(filename);
            this.video.LoadedBehavior = MediaState.Manual;
            this.video.UnloadedBehavior = MediaState.Manual;
            this.video.Volume = (double)this.sliderSon.Value;
            this.video.Play();
            this.videoPlay = true;
        }

        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            this.video.Pause();
            this.videoPlay = false;
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            this.video.Stop();
            this.videoPlay = false;
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (this.video.Source != null)
            {
                timer.Start();
                this.video.Play();
                this.videoPlay = true;
            }
            else
            {
                System.Windows.Forms.OpenFileDialog fileDialogue = new OpenFileDialog();
                fileDialogue.Filter = " All Files (*.*) |*.*| AVI Files (.avi) |*.avi| MPK Files (.mpk) |*.mpk| MP3 Files (.mp3) |*.mp3| MP4 Files (.mp4) |*.mp4";
                fileDialogue.FilterIndex = 1;
                if (fileDialogue.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.video.Source = new Uri(fileDialogue.FileName);
                    this.video.LoadedBehavior = MediaState.Manual;
                    this.video.UnloadedBehavior = MediaState.Manual;
                    this.video.Volume = (double)this.sliderSon.Value;
                    this.video.Play();
                    this.videoPlay = true;
                }
            }
        }

        private void sliderSon_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.video.Volume = (double)sliderSon.Value;
        }

        private void video_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (this.video.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = this.video.NaturalDuration.TimeSpan;
                this.sliderVideo.Maximum = ts.TotalSeconds;
                timer.Start();
            }
        }

        private void sliderVideo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.video.Position = TimeSpan.FromSeconds(this.sliderVideo.Value);
        }

        private void buttonFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (fullscreen == false)
            {
                this.videoGrid.Children.Remove(this.video);
                this.Background = new SolidColorBrush(Colors.Black);
                this.Content = this.video;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                this.video.Margin = new Thickness(0, 0, 0, 0);
                this.fullscreen = true;
            }
            else
            {
                this.Content = this.Root;
                this.videoGrid.Children.Add(this.video);
                this.Content = this.Root;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                this.fullscreen = false;
            }
        }

        private void videoGrid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space && this.fullscreen == false)
            {
                this.videoGrid.Children.Remove(this.video);
                this.Background = new SolidColorBrush(Colors.Black);
                this.Content = this.video;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                this.video.Margin = new Thickness(0, 0, 0, 0);
                this.fullscreen = true;
            }
            else if (e.Key == Key.Space && this.fullscreen == true)
            {
                this.Content = this.Root;
                this.videoGrid.Children.Add(this.video);
                this.Content = this.Root;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                this.fullscreen = false;
            }
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.fullscreen == false)
            {
                this.Root.Children.Remove(this.Window);
                this.Background = new SolidColorBrush(Colors.Black);
                this.Content = this.Window;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                this.video.Margin = new Thickness(0, 0, 0, 0);
                this.fullscreen = true;
            }
            else if (this.fullscreen == true)
            {
                this.Content = this.Root;
                this.Root.Children.Add(this.Window);
                this.Background = new SolidColorBrush(Colors.White);
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                this.fullscreen = false;
            }
        }

        private void ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBibliotheque.SelectedItems != null)
            {
                this.listPlayList.Items.Add(this.listBibliotheque.SelectedItems);
            }
        }
    }
}
