using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AudioPlayerLib;
using ITSWorkStation.EntityFramework;
using ITSWorkStation.Libs.NotificationButton;
using ITSWorkStation.Model;
using ITSWorkStation.Utils;
using ITSWorkStation.ViewModel;
//using AddInView;
using log4net.Config;
using NotificationLib;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using TreeView = System.Windows.Controls.TreeView;
using UserControl = System.Windows.Controls.UserControl;

namespace ITSWorkStation.View
{
    /// <summary>
    /// Interaction logic for LiveViewUsercontrol.xaml
    /// </summary>
    public partial class LiveViewUsercontrol : UserControl
    {
        #region Mems
        private HighCameraGroupViewModel _viewModel;
        //private BackgroundWorker _backgroundWorker;
        private Dictionary<int, UserControl> _allvideScreenType;
        private NotificationButton btnNotificationButton = null;
        //Controller
        private double panInit = 0;
        private double zoomInit = 1;
        private double tiltInit = 0;
        private double panValuebyMouse = 0;
        private double tiltValuebyMouse = 0;

        private string _ipselected = String.Empty;
        BackgroundWorker m_oWorker;
        private BackgroundWorker m_MousePTcontrolWorker;
        private BackgroundWorker m_HomePresetcontrolWorker;
        private bool _up = false;
        private bool _down = false;
        private bool _left = false;
        private bool _right = false;
        private bool _zoomIn = false;
        private bool _zoomOut = false;

        //private String _comRe = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1&html=no&zoom={1}&pan={2}&tilt={3}";
        
        //private String _comReUp = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1&html=no&continuouspantiltmove=0,300&imagerotation=0&timestamp=1418284961";
        //private String _comReDown = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1&html=no&continuouspantiltmove=0,-300&imagerotation=0&timestamp=1418284961";
        //private String _comReLeft = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1&html=no&continuouspantiltmove=300,0&imagerotation=0&timestamp=1418284961";
        //private String _comReRight = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1&html=no&continuouspantiltmove=-300,0&imagerotation=0&timestamp=1418284961";
        //private String _comReZoomOut = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1&html=no&continuouszoommove=300&imagerotation=0&timestamp=1418284961";
        private String _comReUp = @"http://{0}/axis-cgi/com/ptz.cgi?&imageheight=576&imagewidth=720&center=360,144";
        private String _comReDown = @"http://{0}/axis-cgi/com/ptz.cgi?&imageheight=576&imagewidth=720&center=360,432";
        private String _comReLeft = @"http://{0}/axis-cgi/com/ptz.cgi?&imageheight=576&imagewidth=720&center=180,288";
        private String _comReRight = @"http://{0}/axis-cgi/com/ptz.cgi?&imageheight=576&imagewidth=720&center=540,288";
        private String _comReByMouse = @"http://{0}/axis-cgi/com/ptz.cgi?&imageheight=576&imagewidth=720&center={1},{2}";
        
        private String _comReZoom = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1&html=no&zoom={1}";
        private String _comRequestInfo = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1";

        private String _comReHomePreset = @"http://{0}/axis-cgi/com/ptz.cgi?gotoserverpresetname=Home&camera=1&speed=100";

        private bool isPTZ = false;
        private double maxTilt, minTilt, maxZoom, minZoom, maxPan, minPan;
        private int currentScreenType = -1;
        private bool isImportanceScreen = false;
        private bool isCapture = false;
        private bool isKeyDown = false;
        //end

        //audio
        //audio notification
        private readonly AudioPlayer m_alertaudio;
        private readonly AudioPlayer m_motionaudio;
        private readonly AudioPlayer m_tamperaudio;

        //private readonly MediaPlayer m_alertaudio;
        //private readonly MediaPlayer m_motionaudio;
        //private readonly MediaPlayer m_tamperaudio;
        //private bool isplayingAlert, isplayingMotion, isplayingTamper;

        private string _alertfile = Directory.GetCurrentDirectory() + "/media/audio-alert.wav";
        private string _motionfile = Directory.GetCurrentDirectory() + "/media/motion-alert.wav";
        private string _tamperfile = Directory.GetCurrentDirectory() + "/media/tamper-alarm.wav";
        //end
        #endregion

        #region Contructors

        public LiveViewUsercontrol()
        {
            InitializeComponent();
            Loaded += LiveViewUsercontrol_Loaded;

            XmlConfigurator.Configure();
            ManageVideoScreen();
            centreshowScreen.Content = new UserControl();
            _viewModel = new HighCameraGroupViewModel();
            for (int i = 0; i < 25; i++)
            {
                VideoScreenUserControl video = new VideoScreenUserControl();
                video.MouseLeftButtonUp += Video_MouseLeftButtonUp;
                video.MouseLeftButtonDown += video_MouseLeftButtonDown;
                video.MouseWheel += video_MouseWheel;
                video.MouseDoubleClick += video_MouseDoubleClick;
                video.Stop += video_Stop;
                video.add += Video_add;
                ResourceUtil.Listvideo.Add(video);
            }

            DataContext = _viewModel;
            GetCameraData();
            InitComboboxChooseScreen();
            SetVideo(5);//Default set video is 5x5
            currentScreenType = 5;
            m_oWorker = new BackgroundWorker();
            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            //m_oWorker.ProgressChanged += new ProgressChangedEventHandler
            //        (m_oWorker_ProgressChanged);
            m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                    (m_oWorker_RunWorkerCompleted);
            //m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;

            m_MousePTcontrolWorker = new BackgroundWorker();
            m_MousePTcontrolWorker.DoWork += m_MousePTcontrolWorker_DoWork;
            m_MousePTcontrolWorker.RunWorkerCompleted += m_MousePTcontrolWorker_RunWorkerCompleted;
            m_MousePTcontrolWorker.WorkerSupportsCancellation = true;
            //manNotification = new ManNotificationButton();

            m_HomePresetcontrolWorker = new BackgroundWorker();
            m_HomePresetcontrolWorker.DoWork += m_HomePresetcontrolWorker_DoWork;
            m_HomePresetcontrolWorker.RunWorkerCompleted += m_HomePresetcontrolWorker_RunWorkerCompleted;
            m_HomePresetcontrolWorker.WorkerSupportsCancellation = true;
            //Audio alarm
            m_alertaudio = m_tamperaudio = m_motionaudio = null;
            if (File.Exists(_alertfile))
            {
                m_alertaudio = new AudioPlayer(_alertfile);
                m_alertaudio.PlaybackEnded += m_alertaudio_PlaybackEnded;
                //m_alertaudio = new MediaPlayer();
                //m_alertaudio.Open(new Uri(_alertfile));
                //m_alertaudio.MediaEnded += m_alertaudio_MediaEnded;
            }
            if (File.Exists(_motionfile))
            {
                m_motionaudio = new AudioPlayer(_motionfile);
                //m_motionaudio = new MediaPlayer();
                //m_motionaudio.Open(new Uri(_motionfile));
                //m_motionaudio.MediaEnded += m_motionaudio_MediaEnded;
            }
            if (File.Exists(_tamperfile))
            {
                m_tamperaudio = new AudioPlayer(_tamperfile);
                m_tamperaudio.PlaybackEnded += m_tamperaudio_PlaybackEnded;
                //m_tamperaudio = new MediaPlayer();
                //m_tamperaudio.Open(new Uri(_tamperfile));
                //m_tamperaudio.MediaEnded += MediaEnded;
            }
            //end
        }
        
        void video_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (m_oWorker.IsBusy || (zoomInit == maxZoom))
                return;

            if (!((sender as VideoScreenUserControl).IsImportanceScreen))
                return;

            if (e.Delta > 0)
            {
                _up = false;
                _down = false;
                _left = false;
                _right = false;
                _zoomIn = true;
                _zoomOut = false;

                zoomInit += 100;
                if (zoomInit > maxZoom)
                    zoomInit = maxZoom;
                m_oWorker.RunWorkerAsync();
            }
            else
            {
                _up = false;
                _down = false;
                _left = false;
                _right = false;
                _zoomIn = false;
                _zoomOut = true;
                zoomInit -= 100;
                if (zoomInit < minZoom)
                    zoomInit = minZoom;

                m_oWorker.RunWorkerAsync();
            }
        }
        
        void m_alertaudio_PlaybackEnded(object sender, EventArgs e)
        {
           // m_alertaudio.IsPlaying = false;
        }

        void m_tamperaudio_PlaybackEnded(object sender, EventArgs e)
        {
            
        }

        //Play the file
        private void TogglePlayback(AudioPlayer player,bool status)
        {
            if (player == null)
                return;

            //if (!player.IsPlaying && (status))
            if(status)
            {
                player.Stop();
                player.Play();
            }
        }

        void video_Stop()
        {
            if (currentScreenType != -1)
            {
                cbxChooseScreen.Visibility = Visibility.Visible;
                ClearAllVideo();
                if (currentScreenType == 10)
                {
                    if (_allvideScreenType.ContainsKey(7))
                    {
                        ClearAllVideo();
                        var specScreenOne = _allvideScreenType[7] as SpecScreenOne;
                        if (specScreenOne != null) specScreenOne.DrawControls();
                        centreshowScreen.Content = _allvideScreenType[7];

                    }
                    else
                    {
                        ClearAllVideo();
                        SpecScreenOne screen = new SpecScreenOne();
                        screen.DrawControls();
                        _allvideScreenType.Add(7, screen);
                        centreshowScreen.Content = screen;
                    }
                }
                else
                {
                    SetVideo(currentScreenType);
                }
            }
        }

        void video_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var video = sender as VideoScreenUserControl;
            if (video == null)
                return;
            foreach (var videoScreenUserControl in ResourceUtil.Listvideo)
            {
                if (video.Equals(videoScreenUserControl))
                {
                    videoScreenUserControl.IsSelected = true;
                    videoScreenUserControl.BorderThickness = new Thickness(2);
                    videoScreenUserControl.BorderBrush = new SolidColorBrush(Colors.Goldenrod);
                    _ipselected = videoScreenUserControl.Ip;
                    CreateThreadGetCamInfoFromServer();
                    if (videoScreenUserControl.Type != 0)
                    {
                        //Hide the controler button

                        //
                    }
                    video.HideEventCameratampering();//Hide the event EventCameratampering
                }
                else
                {
                    videoScreenUserControl.IsSelected = false;
                    videoScreenUserControl.BorderThickness = new Thickness(0);
                }
            }
        }

        void video_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (currentScreenType != -1)
            {
                if (sender is VideoScreenUserControl)
                {
                    var video = sender as VideoScreenUserControl;
                    if(!video.IsDisplaying)
                        return;
                    foreach (var videoScreenUserControl in ResourceUtil.Listvideo)
                    {
                        if (!video.Equals(videoScreenUserControl))
                        {
                            videoScreenUserControl.IsImportanceScreen = false;
                            videoScreenUserControl.TblImportance.Visibility = Visibility.Hidden;
                        }
                    }
                    //if (centreshowScreen.Content is CentreScreenVideo)
                    //{
                    //    var centre = centreshowScreen.Content as CentreScreenVideo;
                    //    //if (!video.IsFullScreen)
                    //    //{
                    //    //    //centre.RemoveVideoInGrid(video);
                    //    //    //var full = new FullScreen(video);
                    //    //    video.IsFullScreen = true;
                    //    //    //full.ShowDialog();
                    //    //}
                    //}

                    //Make importance this screen
                    //SpecScreenOne specScreen = new SpecScreenOne();
                    //currentScreenType = 10;
                    if (!video.IsImportanceScreen)
                    {
                        video.IsImportanceScreen = true;
                        cbxChooseScreen.Visibility = Visibility.Collapsed;
                        isImportanceScreen = true;
                        video.TblImportance.Visibility = Visibility.Visible;
                        if (_allvideScreenType.ContainsKey(7))
                        {
                            ClearAllVideo();
                            var specScreenOne = _allvideScreenType[7] as SpecScreenOne;
                            if (specScreenOne != null) specScreenOne.DrawControlsForMakeImportance(video);
                            centreshowScreen.Content = _allvideScreenType[7];
                            
                        }
                        else
                        {
                            ClearAllVideo();
                            SpecScreenOne screen = new SpecScreenOne();
                            screen.DrawControlsForMakeImportance(video);
                            _allvideScreenType.Add(7, screen);
                            centreshowScreen.Content = screen;
                        }
                    }
                    else
                    {
                        video.IsImportanceScreen = false;
                        video.TblImportance.Visibility = Visibility.Hidden;
                        cbxChooseScreen.Visibility = Visibility.Visible;
                        isImportanceScreen = false;
                        ClearAllVideo();
                        if (currentScreenType == 10)
                        {
                            if (_allvideScreenType.ContainsKey(7))
                            {
                                ClearAllVideo();
                                var specScreenOne = _allvideScreenType[7] as SpecScreenOne;
                                if (specScreenOne != null) specScreenOne.DrawControls();
                                centreshowScreen.Content = _allvideScreenType[7];
                                
                            }
                            else
                            {
                                ClearAllVideo();
                                SpecScreenOne screen = new SpecScreenOne();
                                screen.DrawControls();
                                _allvideScreenType.Add(7, screen);
                                centreshowScreen.Content = screen;
                            }
                        }
                        else
                        {
                            SetVideo(currentScreenType);
                        }
                    }
                    //End
                }
            }
        }

        ~LiveViewUsercontrol()
        {
            //DisposeAllScreen();
        }

        /// <summary>
        /// Contain all video screen
        /// </summary>
        public void ManageVideoScreen()
        {
            _allvideScreenType = new Dictionary<int, UserControl>();
            //_allvideScreen.Add(ucVideoScreen1);
            //_allvideScreen.Add(ucVideoScreen2);
            //_allvideScreen.Add(ucVideoScreen3);
            //_allvideScreen.Add(ucVideoScreen4);
        }

        void LiveViewUsercontrol_Loaded(object sender, RoutedEventArgs e)
        {
            //XmlConfigurator.Configure();
            //ManageVideoScreen();
            //centreshowScreen.Content = new UserControl();
            //_viewModel = new HighCameraGroupViewModel();
            //for (int i = 0; i < 36; i++)
            //{
            //    VideoScreenUserControl video = new VideoScreenUserControl();
            //    video.MouseLeftButtonUp += Video_MouseLeftButtonUp;
            //    video.add += Video_add;
            //    ResourceUtil.Listvideo.Add(video);
            //}

            //DataContext = _viewModel;
            //GetCameraData();
            //InitComboboxChooseScreen();
            //SetVideo(2);//Default set video is 2x2

            //m_oWorker = new BackgroundWorker();
            //m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            ////m_oWorker.ProgressChanged += new ProgressChangedEventHandler
            ////        (m_oWorker_ProgressChanged);
            //m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
            //        (m_oWorker_RunWorkerCompleted);
            ////m_oWorker.WorkerReportsProgress = true;
            if (btnNotificationButton == null)
            {
                btnNotificationButton = new NotificationButton();
                //btnNotificationButton.NotifyFunc = _viewModel.NotifyDelegate;
                btnNotificationButton.SetBinding(NotificationButton.NotifyFuncProperty,
                    new Binding
                    {
                        Source = _viewModel,
                        Path = new PropertyPath("NotifyDelegate"),
                        Mode = BindingMode.TwoWay
                    });
                btnNotificationButton.Click += BtnOpen_OnClick;
                TextBlock text = new TextBlock();
                text.Text = "Ảnh đã chụp";
                text.VerticalAlignment = VerticalAlignment.Center;
                text.HorizontalAlignment = HorizontalAlignment.Center;
                text.FontSize = 12;
                btnNotificationButton.Content = text;
                btnNotificationButton.Foreground = new SolidColorBrush(Colors.Black);
                btnNotificationButton.Width = 148;
                btnNotificationButton.Height = 23;
                panelNotification.Children.Add(btnNotificationButton);
                
            }
            //m_oWorker.WorkerSupportsCancellation = true;
        }

        void Video_add(string newIpselected)
        {
            _ipselected = newIpselected;
            CreateThreadGetCamInfoFromServer();
        }

        public void ConfigTheSelectedVideoScreen()
        {
            VideoScreenUserControl selectedVideo = GetSelectedScreen();
            if (selectedVideo == null)
            {
                _ipselected = string.Empty;
                //Hide the controller button

                //end
            }
            else
            {
                _ipselected = selectedVideo.Ip;
                int type = selectedVideo.Type;
                if (type != 0)
                    return;

                //Show the controller button

                //end
            }
        }

        #region BackgroundWorker
        private void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {

            }
        }

        void m_MousePTcontrolWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {

            }
        }

        private void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (m_oWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                //String resCom =
                //    string.Format(
                //        _comRe, _ipselected, zoomInit, panInit, tiltInit);
                string resCom = string.Empty;
                if (_zoomIn)
                {
                    resCom =
                    string.Format(
                        _comReZoom, _ipselected,zoomInit);
                }
                if (_zoomOut)
                {
                    resCom =
                    string.Format(
                        _comReZoom, _ipselected, zoomInit);
                }
                if (_up)
                {
                    resCom =
                    string.Format(
                        _comReUp, _ipselected);
                }
                if (_down)
                {
                    resCom =
                    string.Format(
                        _comReDown, _ipselected);
                }
                if (_left)
                {
                    resCom =
                    string.Format(
                        _comReLeft, _ipselected);
                }
                if (_right)
                {
                    resCom =
                    string.Format(
                        _comReRight, _ipselected);
                }

                //string comStop = string.Format(
                //        _comReStop, _ipselected);

                
                HttpWebRequest req = (HttpWebRequest)WebRequest.CreateHttp(resCom);
                
                req.PreAuthenticate = true;
                req.Credentials = new NetworkCredential("root", "admin");
                req.Timeout = 5000;
                req.Method = "GET";
                req.Proxy = null;
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                response.Close();

                //HttpWebRequest reqStop = (HttpWebRequest)WebRequest.CreateHttp(comStop);
                //reqStop.PreAuthenticate = true;
                //reqStop.Credentials = new NetworkCredential("root", "admin");
                //reqStop.Timeout = 5000;
                //reqStop.Method = "GET";
                //reqStop.Proxy = null;
                //HttpWebResponse responseStop = (HttpWebResponse)reqStop.GetResponse();
                //responseStop.Close();

            }
            catch (WebException ex)
            {
                MessageBox.Show("Quá thời gian chờ thao tác");
            }
        }

        void m_MousePTcontrolWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (m_MousePTcontrolWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                string resCom = string.Empty;
                resCom = string.Format(
                        _comReByMouse, _ipselected,((int)(panValuebyMouse + 360)),((int)(tiltValuebyMouse + 288))); //new X, new Y
                HttpWebRequest req = (HttpWebRequest)WebRequest.CreateHttp(resCom);

                req.PreAuthenticate = true;
                req.Credentials = new NetworkCredential("root", "admin");
                req.Timeout = 5000;
                req.Method = "GET";
                req.Proxy = null;
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                
            }
        }

        void m_HomePresetcontrolWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        void m_HomePresetcontrolWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (m_HomePresetcontrolWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                string resCom = string.Empty;
                resCom = string.Format(
                        _comReHomePreset, _ipselected); //new X, new Y
                HttpWebRequest req = (HttpWebRequest)WebRequest.CreateHttp(resCom);

                req.PreAuthenticate = true;
                req.Credentials = new NetworkCredential("root", "admin");
                req.Timeout = 5000;
                req.Method = "GET";
                req.Proxy = null;
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        /// <summary>
        /// Choose the video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Video_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(!((sender as VideoScreenUserControl).IsImportanceScreen))
                return;
            if(m_MousePTcontrolWorker.IsBusy)
                return;

            Point pos = e.GetPosition(sender as VideoScreenUserControl);
            Point centrePoint = (sender as VideoScreenUserControl).GetCentreOfVideo();

            panValuebyMouse = pos.X - centrePoint.X;
            tiltValuebyMouse = pos.Y - centrePoint.Y;
            if(m_MousePTcontrolWorker != null)
                m_MousePTcontrolWorker.RunWorkerAsync();
            
        }
        /// <summary>
        /// Create thread that get info of selected camera
        /// </summary>
        public void CreateThreadGetCamInfoFromServer()
        {
            Thread getcameraInfoThread = new Thread(() =>
            {
                try
                {
                    String resCom =
                string.Format(
                    _comRequestInfo, _ipselected);
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resCom);

                    req.PreAuthenticate = true;
                    req.Credentials = new NetworkCredential("root", "admin");
                    req.Timeout = 5000;
                    req.Method = "GET";
                    req.Proxy = null;
                    var response = (HttpWebResponse)req.GetResponse();
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        string[] infos = text.Split(new[] { "\r\n" }, StringSplitOptions.None);

                        foreach (var info in infos)
                        {
                            if (info.Contains("pan="))
                            {
                                string[] pan = info.Split(new[] { "pan=" }, StringSplitOptions.None);
                                if (pan.Count() > 0)
                                {
                                    double.TryParse(pan[1], out panInit);
                                }
                            }
                            if (info.Contains("MaxPan="))
                            {
                                string[] pan = info.Split(new[] { "MaxPan=" }, StringSplitOptions.None);
                                if (pan.Count() > 0)
                                {
                                    double.TryParse(pan[1], out maxPan);
                                }
                            }
                            if (info.Contains("MinPan="))
                            {
                                string[] pan = info.Split(new[] { "MinPan=" }, StringSplitOptions.None);
                                if (pan.Count() > 0)
                                {
                                    double.TryParse(pan[1], out minPan);
                                }
                            }
                            if (info.Contains("tilt="))
                            {
                                string[] tilt = info.Split(new[] { "tilt=" }, StringSplitOptions.None);
                                if (tilt.Count() > 0)
                                {
                                    double.TryParse(tilt[1], out tiltInit);
                                }
                            }
                            if (info.Contains("MaxTilt="))
                            {
                                string[] tilt = info.Split(new[] { "MaxTilt=" }, StringSplitOptions.None);
                                if (tilt.Count() > 0)
                                {
                                    double.TryParse(tilt[1], out maxTilt);
                                }
                            }
                            if (info.Contains("MinTilt="))
                            {
                                string[] tilt = info.Split(new[] { "MinTilt=" }, StringSplitOptions.None);
                                if (tilt.Count() > 0)
                                {
                                    double.TryParse(tilt[1], out minTilt);
                                }
                            }
                            if (info.Contains("zoom="))
                            {
                                string[] zoom = info.Split(new[] { "zoom=" }, StringSplitOptions.None);
                                if (zoom.Count() > 0)
                                {
                                    double.TryParse(zoom[1], out zoomInit);
                                }
                            }
                            if (info.Contains("MaxZoom="))
                            {
                                string[] zoom = info.Split(new[] { "MaxZoom=" }, StringSplitOptions.None);
                                if (zoom.Count() > 0)
                                {
                                    double.TryParse(zoom[1], out maxZoom);
                                }
                            }
                            if (info.Contains("MinZoom="))
                            {
                                string[] zoom = info.Split(new string[] { "MinZoom=" }, StringSplitOptions.None);
                                if (zoom.Count() > 0)
                                {
                                    double.TryParse(zoom[1], out minZoom);
                                }
                            }
                        }
                    }
                    isPTZ = true;
                    response.Close();
                }
                catch (Exception ex)
                {
                    //Log

                    isPTZ = false;
                }
            });

            getcameraInfoThread.Start();
        }
        #endregion

        #region Method
        /// <summary>
        /// Get selected video on screen
        /// </summary>
        /// <returns></returns>
        public VideoScreenUserControl GetSelectedScreen()
        {
            foreach (var videoScreenUserControl in ResourceUtil.Listvideo)
            {
                if (videoScreenUserControl.IsSelected)
                    return videoScreenUserControl;
            }
            return null;
        }

        /// <summary>
        /// Get camera info from database
        /// </summary>
        public void GetCameraData()
        {
            var en = new cctvEntities();
            try
            {
                var ls = en.GetCameraByUser(Account.Username);
                Dictionary<String, List<CameraItem>> camgroupbyname = new Dictionary<string, List<CameraItem>>();
                foreach (ComplexType1 complexType1 in ls)
                {
                    ComplexType1 com = complexType1;
                    var camItem = new CameraItem { Id = com.id, CameraUrl = com.cameraurl, Ip = com.ip, Type = com.type };
                    if (!camgroupbyname.ContainsKey(com.group_name))
                    {
                        camgroupbyname.Add(com.group_name, new List<CameraItem> { camItem });
                    }
                    else
                    {
                        camgroupbyname[com.group_name].Add(camItem);
                    }

                }
                var highgroup = new ObservableCollection<CameraGroupViewModel>();
                foreach (var cams in camgroupbyname)
                {
                    highgroup.Add(new CameraGroupViewModel(new CameraGroup { GroupName = cams.Key, CameraList = cams.Value }));
                }
                _viewModel.SetCameraGroupViewModel(highgroup);

            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        /// <summary>
        /// Init somw Screen Type
        /// </summary>
        public void InitComboboxChooseScreen()
        {
            ObservableCollection<ScreenTypeViewModel> types = new ObservableCollection<ScreenTypeViewModel>();

            types.Add(new ScreenTypeViewModel { Type = "Kiểu 2x2" });
            types.Add(new ScreenTypeViewModel { Type = "Kiểu 3x3" });
            types.Add(new ScreenTypeViewModel { Type = "Kiểu 4x4" });
            types.Add(new ScreenTypeViewModel { Type = "Kiểu 5x5" });
            //types.Add(new ScreenTypeViewModel { Type = "6x6 Screen" });
            types.Add(new ScreenTypeViewModel { Type = "Kiểu 5x1" });
            types.Add(new ScreenTypeViewModel { Type = "Kiểu 1x1" });
            _viewModel.SetSetScreenTypeViewModel(types);
            //cbxChooseScreen.DisplayMemberPath = Name;
            //cbxChooseScreen.DisplayMemberPath = "Type";
        }

        /// <summary>
        /// Drag from tree view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            TreeView item = e.Source as TreeView;
            if (item != null && e.LeftButton == MouseButtonState.Pressed)
            {
                if (item.SelectedItem != null)
                {
                    DataObject dataObject = new DataObject();
                    if (!(item.SelectedItem is CameraGroupViewModel) && (item.SelectedItem is CameraItemViewModel))
                    {
                        //Create a stream data that contain some infomations
                        //Format data: camurl;ip;type
                        String data = (item.SelectedItem as CameraItemViewModel).CamUrl + ";" +
                                      (item.SelectedItem as CameraItemViewModel).Ip + ";" +
                                      (item.SelectedItem as CameraItemViewModel).CamID;
                        //End create
                        dataObject.SetData("MyTreeViewItem", data);
                        DragDrop.DoDragDrop(item, dataObject, DragDropEffects.Copy);
                    }
                }
            }
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        //private void CvsVideo1_OnDragOver(object sender, DragEventArgs e)
        //{
        //    if (!e.Data.GetDataPresent("MyTreeViewItem") ||
        //        sender == e.Source)
        //    {
        //        e.Effects = DragDropEffects.None;
        //    }
        //    else
        //    {
        //        e.Effects = DragDropEffects.Copy;
        //    }
        //}

        //private void CvsVideo1_OnDrop(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent("MyTreeViewItem"))
        //    {
        //        String data = e.Data.GetData("MyTreeViewItem") as String;
        //        tblcameName.Content = data;
        //        tblcameName.Foreground = new SolidColorBrush(Colors.Black);
        //    }
        //}

        //private void CvsVideo2_OnDragOver(object sender, DragEventArgs e)
        //{
        //    if (!e.Data.GetDataPresent("MyTreeViewItem") ||
        //        sender == e.Source)
        //    {
        //        e.Effects = DragDropEffects.None;
        //    }
        //    else
        //    {
        //        e.Effects = DragDropEffects.Copy;
        //    }
        //}

        //private void CvsVideo2_OnDrop(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent("MyTreeViewItem"))
        //    {
        //        String data = e.Data.GetData("MyTreeViewItem") as String;
        //        tblcameName2.Content = data;
        //        tblcameName2.Foreground = new SolidColorBrush(Colors.Black);
        //    } 
        //}

        #region Event

        #endregion

        #region Dispose All screen

        public void DisposeAllScreen()
        {
            foreach (var videoScreenUserControl in ResourceUtil.Listvideo)
            {
                videoScreenUserControl.Dispose();
            }
            _allvideScreenType.Clear();
            ResourceUtil.Listvideo.Clear();
            grCentreboard.Children.Clear();
            grfooterleft.Children.Clear();
            btnShowHideLeft = null;
            //Dispatcher.InvokeShutdown();
        }

        #endregion

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {

            var comboBox = sender as ComboBox;
            // ... Assign the ItemsSource to the List.
            //comboBox.ItemsSource = new ScreenTypeViewModel();

            // ... Make the first item selected.
            //comboBox.SelectedIndex = 0;

            //comboBox.DisplayMemberPath = "Type";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;

            //test create a dynamic Screen
            //CentreScreenVideo screen = new CentreScreenVideo();
            //screen.SetNumberofScreen(2);
            //centreshowScreen.Content = screen;
            if (comboBox.SelectedIndex == 0) // 2x2 Screen
            {
                currentScreenType = 2;
                SetVideo(2);
            }
            if (comboBox.SelectedIndex == 1) //3x3 Screen
            {
                currentScreenType = 3;
                SetVideo(3);
            }
            if (comboBox.SelectedIndex == 2) //4x4 Screen
            {
                currentScreenType = 4;
                SetVideo(4);
            }
            if (comboBox.SelectedIndex == 3)//5x5 Screen
            {
                currentScreenType = 5;
                SetVideo(5);
            }
            //if (comboBox.SelectedIndex == )//6x6 Screen
            //{
            //    SetVideo(6);
            //}
            if (comboBox.SelectedIndex == 4)// Special Screen
            {
                currentScreenType = 10;
                if (_allvideScreenType.ContainsKey(7))
                {
                    ClearAllVideo();
                    var specScreenOne = _allvideScreenType[7] as SpecScreenOne;
                    if (specScreenOne != null) specScreenOne.DrawControls();
                    centreshowScreen.Content = _allvideScreenType[7];
                }
                else
                {
                    ClearAllVideo();
                    SpecScreenOne screen = new SpecScreenOne();
                    screen.DrawControls();
                    _allvideScreenType.Add(7, screen);
                    centreshowScreen.Content = screen;
                }
            }
            if (comboBox.SelectedIndex == 5) // Special Screen
            {
                currentScreenType = 1;
                SetVideo(1);
            }
        }
        /// <summary>
        /// Clear video in all screen type 
        /// </summary>
        public void ClearAllVideo()
        {
            foreach (var userControl in _allvideScreenType)
            {
                if (userControl.Value is CentreScreenVideo)
                    (userControl.Value as CentreScreenVideo).ClearCentreGrid();
                else if (userControl.Value is SpecScreenOne)
                {
                    (userControl.Value as SpecScreenOne).ClearListControl();
                }
            }
        }

        public void SetVideo(int num)
        {
            if (_allvideScreenType.ContainsKey(num))
            {
                ClearAllVideo();
                _allvideScreenType.Remove(num);
                //(_allvideScreenType[num] as CentreScreenVideo).AddVideoToCentreGrid(num);
                //centreshowScreen.Content = _allvideScreenType[num];
                CentreScreenVideo screen = new CentreScreenVideo();
                screen.SetNumberofScreen(num);
                _allvideScreenType.Add(num, screen);
                centreshowScreen.Content = _allvideScreenType[num];
            }
            else
            {
                ClearAllVideo();
                CentreScreenVideo screen = new CentreScreenVideo();
                screen.SetNumberofScreen(num);
                _allvideScreenType.Add(num, screen);
                centreshowScreen.Content = screen;
            }
        }
        #region Click Event
        private void BtnSnap_OnClick(object sender, RoutedEventArgs e)
        {
            var slectedVideo = GetSelectedScreen();
            if (slectedVideo == null)
                return;

            if (slectedVideo.IsDisplaying)
            {
                ImageSource soure = slectedVideo.imVideo.Source;

                // Configure save file dialog box
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Image"; // Default file name
                dlg.DefaultExt = ".png"; // Default file extension
                dlg.Filter = "Image documents (.png)|*.png"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    //string filename = dlg.FileName;

                    //var encoder = new JpegBitmapEncoder();
                    var encoder = new PngBitmapEncoder();
                    String photolocation = dlg.FileName;//+ "/" +photoID.ToString() + ".jpg";  //file name 
                    BitmapFrame outputFrame = BitmapFrame.Create((BitmapSource)soure);
                    encoder.Frames.Add(outputFrame);
                    //encoder.QualityLevel = 1;
                    using (var filestream = new FileStream(photolocation, FileMode.Create))
                        encoder.Save(filestream);

                    soure.Freeze();
                }
            }

        }

        public void SnapshotByKey()
        {
            if(isCapture)
                return;
            if(isKeyDown)
                return;
            isKeyDown = true;
            var slectedVideo = GetSelectedScreen();
            if (slectedVideo == null)
                return;

            if (slectedVideo.IsDisplaying)
            {
                isCapture = true;
                try
                {
                    ImageSource soure = slectedVideo.imVideo.Source;
                    // Show save file dialog box


                    // Process save file dialog box results
                    // Save document
                    //string filename = dlg.FileName;

                    //var encoder = new JpegBitmapEncoder();
                    var encoder = new PngBitmapEncoder();
                    string path = Directory.GetCurrentDirectory() + @"\images\";
                    Directory.CreateDirectory(path);
                    var time = DateTime.Now;
                    String photolocation = path; 
                        //slectedVideo.CamId.ToString()+
                        //time.Date.ToString() +
                        //time.TimeOfDay.ToString() +
                        //time.Ticks+
                        //".png";
                    photolocation += slectedVideo.CamId.ToString() + "_";
                    photolocation += time.Year.ToString();
                    photolocation += time.Month.ToString();
                    photolocation += time.Day.ToString() + "_";
                    photolocation += time.Hour.ToString();
                    photolocation += time.Minute.ToString();
                    photolocation += time.Millisecond.ToString();
                    photolocation +=".png";
                    BitmapFrame outputFrame = BitmapFrame.Create((BitmapSource) soure);
                    encoder.Frames.Add(outputFrame);
                    //encoder.QualityLevel = 1;
                    using (var filestream = new FileStream(photolocation, FileMode.Create))
                        encoder.Save(filestream);
                    if (btnNotificationButton != null)
                    {
                        _viewModel.IncreaseImage();
                        btnNotificationButton.UpdateContent();
                    }
                    soure.Freeze();
                    isCapture = false;
                    //Update to notification
                    //manNotification.IncreaseImage();
                    
                    //End
                }
                catch (Exception ex)
                {
                    isCapture = false;
                }
                
            }
        }

        public void KeyUp()
        {
            isKeyDown = false;
        }

        private void HomeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_HomePresetcontrolWorker.IsBusy)
                return;
            m_HomePresetcontrolWorker.RunWorkerAsync();
        }

        private void UpButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (tiltInit == maxTilt))
                return;

            _up = true;
            _down = false;
            _left = false;
            _right = false;
            _zoomIn = false;
            _zoomOut = false;

            tiltInit += 10;
            if (tiltInit < minTilt)
            {
                tiltInit = minTilt;
            }
            else if (tiltInit > maxTilt)
            {
                tiltInit = maxTilt;
            }
            m_oWorker.RunWorkerAsync();
        }

        private void LeftButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (panInit == minPan))
                return;
            _up = false;
            _down = false;
            _left = true;
            _right = false;
            _zoomIn = false;
            _zoomOut = false;
            panInit -= 10;
            if (panInit < minPan)
                panInit = minPan;

            m_oWorker.RunWorkerAsync();
        }

        private void RigButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (panInit == maxPan))
                return;

            _up = false;
            _down = false;
            _left = false;
            _right = true;
            _zoomIn = false;
            _zoomOut = false;

            panInit += 10;
            if (panInit > maxPan)
                panInit = maxPan;

            m_oWorker.RunWorkerAsync();
        }

        private void ZoomInButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (zoomInit == maxZoom))
                return;

            _up = false;
            _down = false;
            _left = false;
            _right = false;
            _zoomIn = true;
            _zoomOut = false;

            zoomInit += 200;
            if (zoomInit > maxZoom)
                zoomInit = maxZoom;
            m_oWorker.RunWorkerAsync();
        }

        private void ZoomOutButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (zoomInit == minZoom))
                return;
            _up = false;
            _down = false;
            _left = false;
            _right = false;
            _zoomIn = false;
            _zoomOut = true;
            zoomInit -= -200;
            if (zoomInit < minZoom)
                zoomInit = minZoom;

            m_oWorker.RunWorkerAsync();
        }

        private void DownButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (tiltInit == minTilt))
                return;
            _up = false;
            _down = true;
            _left = false;
            _right = false;
            _zoomIn = false;
            _zoomOut = false;

            tiltInit -= 10;
            if (tiltInit < minTilt)
            {
                tiltInit = minTilt;
            }
            else if (tiltInit > maxTilt)
            {
                tiltInit = maxTilt;
            }
            m_oWorker.RunWorkerAsync();
        }

        #endregion

        private void ShowHideLeft_OnClick(object sender, RoutedEventArgs e)
        {
            if (AbColumnDefinition.ActualWidth == 0)
            {
                AbColumnDefinition.Width = new GridLength(6, GridUnitType.Star);
            }
            else
            {
                AbColumnDefinition.Width = new GridLength(0, GridUnitType.Star);
            }
            //Redraw screen
            if (currentScreenType != -1)
            {
                if (currentScreenType != 10)
                {
                    SetVideo(currentScreenType);
                }
            }
        }

        public void SetFullScreen(bool status)
        {
            if (status)
            {
                AbColumnDefinition.Width = new GridLength(0, GridUnitType.Star);
                _viewModel.HideImage();
                btnNotificationButton.UpdateContent();
            }
            else
            {
                AbColumnDefinition.Width = new GridLength(6, GridUnitType.Star);
                _viewModel.ShowImage();
                btnNotificationButton.UpdateContent();
            }
            //Redraw screen
            if (currentScreenType != -1)
            {
                if (currentScreenType != 10 && !isImportanceScreen)
                {
                    SetVideo(currentScreenType);
                }
            }
        }

        private void GrCentre_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //if(btnShowHideLeft != null)
            //btnShowHideLeft.Visibility = Visibility.Visible;
        }

        private void GrCentre_OnMouseLeave(object sender, MouseEventArgs e)
        {
            //if (btnShowHideLeft != null)
            //btnShowHideLeft.Visibility = Visibility.Hidden;
        }

        public void ShowEvent(int eventId, string camId, bool status)
        {
            foreach (var video in ResourceUtil.Listvideo)
            {
                if (video.CamId == camId && video.IsDisplaying)
                {
                    video.ShowEvent(eventId,status);
                    //Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (video.GetAudioBoxCheck())
                        {
                            if (eventId == 6)
                            {
                                TogglePlayback(m_motionaudio, status);
                            }
                            else if (eventId == 7)
                            {
                                TogglePlayback(m_alertaudio, status);
                            }
                            else if (eventId == 8)
                            {
                                TogglePlayback(m_tamperaudio, status);
                            }
                        }
                    }
                    //}), DispatcherPriority.Normal, null);
                    break;
                }
            }
        }

        private void BtnOpen_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = Directory.GetCurrentDirectory() + @"\images\";
                if (Directory.Exists(path))
                    Process.Start(path);
                else
                {
                    var res = Directory.CreateDirectory(path);
                    if(res.Exists)
                        Process.Start(path);
                }
                if (btnNotificationButton != null && _viewModel!= null)
                {
                    _viewModel.ResetImageFolder();
                    btnNotificationButton.UpdateContent();
                }
            }
            catch (Exception ex)
            {
                if (btnNotificationButton != null && _viewModel != null)
                {
                    _viewModel.ResetImageFolder();
                    btnNotificationButton.UpdateContent();
                }
            }
            
        }
        
    }
}
