using System;
using System.AddIn;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LiveViewPlugin.EntityFramework;
using LiveViewPlugin.Model;
using LiveViewPlugin.Utils;
using LiveViewPlugin.ViewModel;
using AddInView;
using log4net.Config;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using TreeView = System.Windows.Controls.TreeView;
using UserControl = System.Windows.Controls.UserControl;

namespace LiveViewPlugin.View
{
    /// <summary>
    /// Interaction logic for LiveViewUsercontrol.xaml
    /// </summary>
    //[AddIn("LiveView", Version = "1.0.0.0", Publisher = "Hoanglm",
    //    Description = "Returns an LiveView usercontrol")]
    public partial class LiveViewUsercontrol :UserControl
    {
        #region Mems    
        private HighCameraGroupViewModel _viewModel;
        //private BackgroundWorker _backgroundWorker;
        private Dictionary<int,UserControl> _allvideScreen;

        //Controller
        private double panInit = 0;
        private double zoomInit = 1;
        private double tiltInit = 0;
        private string _ipselected = String.Empty;
        BackgroundWorker m_oWorker;
        private String _comRe = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1&html=no&zoom={1}&pan={2}&tilt={3}";
        private String _comRequestInfo = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1";
        private bool isPTZ = false;
        private double maxTilt, minTilt, maxZoom, minZoom, maxPan, minPan;
        //end
        #endregion

        #region Contructors

        public LiveViewUsercontrol()
        {
            InitializeComponent();
            Loaded += LiveViewUsercontrol_Loaded;
            
        }

        ~LiveViewUsercontrol()
        {
        }

        /// <summary>
        /// Contain all video screen
        /// </summary>
        public void ManageVideoScreen()
        {
            _allvideScreen = new Dictionary<int, UserControl>();
            //_allvideScreen.Add(ucVideoScreen1);
            //_allvideScreen.Add(ucVideoScreen2);
            //_allvideScreen.Add(ucVideoScreen3);
            //_allvideScreen.Add(ucVideoScreen4);
        }

        void LiveViewUsercontrol_Loaded(object sender, RoutedEventArgs e)
        {
            XmlConfigurator.Configure();
            ManageVideoScreen();
            centreshowScreen.Content = new UserControl();
            _viewModel = new HighCameraGroupViewModel();
            for (int i = 0; i < 36; i++)
            {
                VideoScreenUserControl video = new VideoScreenUserControl();
                video.MouseLeftButtonUp += Video_MouseLeftButtonUp;
                video.add += Video_add;
                ResourceUtil.Listvideo.Add(video);
            }
            
            DataContext = _viewModel;
            GetCameraData();
            InitComboboxChooseScreen();
            SetVideo(2);//Default set video is 2x2

            m_oWorker = new BackgroundWorker();
            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            //m_oWorker.ProgressChanged += new ProgressChangedEventHandler
            //        (m_oWorker_ProgressChanged);
            m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                    (m_oWorker_RunWorkerCompleted);
            //m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;
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
                if(type !=0)
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

        private void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (m_oWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                String resCom =
                    string.Format(
                        _comRe, _ipselected, zoomInit, panInit, tiltInit);
                //resCom += moveValue;
                // Create Request
                HttpWebRequest req = (HttpWebRequest)WebRequest.CreateHttp(resCom);

                req.PreAuthenticate = true;
                req.Credentials = new NetworkCredential("root", "admin");
                req.Timeout = 5000;
                req.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (WebException ex)
            {
                MessageBox.Show("Request Time Out");
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
                }
                else
                {
                    videoScreenUserControl.IsSelected = false;
                    videoScreenUserControl.BorderThickness = new Thickness(0);
                }
            }
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
                    var response = (HttpWebResponse)req.GetResponse();
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        string[] infos = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                        foreach (var info in infos)
                        {
                            if (info.Contains("pan="))
                            {
                                string[] pan = info.Split(new string[] { "pan=" }, StringSplitOptions.None);
                                if (pan.Count() > 0)
                                {
                                    double.TryParse(pan[1], out panInit);
                                }
                            }
                            if (info.Contains("MaxPan="))
                            {
                                string[] pan = info.Split(new string[] { "MaxPan=" }, StringSplitOptions.None);
                                if (pan.Count() > 0)
                                {
                                    double.TryParse(pan[1], out maxPan);
                                }
                            }
                            if (info.Contains("MinPan="))
                            {
                                string[] pan = info.Split(new string[] { "MinPan=" }, StringSplitOptions.None);
                                if (pan.Count() > 0)
                                {
                                    double.TryParse(pan[1], out minPan);
                                }
                            }
                            if (info.Contains("tilt="))
                            {
                                string[] tilt = info.Split(new string[] { "tilt=" }, StringSplitOptions.None);
                                if (tilt.Count() > 0)
                                {
                                    double.TryParse(tilt[1], out tiltInit);
                                }
                            }
                            if (info.Contains("MaxTilt="))
                            {
                                string[] tilt = info.Split(new string[] { "MaxTilt=" }, StringSplitOptions.None);
                                if (tilt.Count() > 0)
                                {
                                    double.TryParse(tilt[1], out maxTilt);
                                }
                            }
                            if (info.Contains("MinTilt="))
                            {
                                string[] tilt = info.Split(new string[] { "MinTilt=" }, StringSplitOptions.None);
                                if (tilt.Count() > 0)
                                {
                                    double.TryParse(tilt[1], out minTilt);
                                }
                            }
                            if (info.Contains("zoom="))
                            {
                                string[] zoom = info.Split(new string[] { "zoom=" }, StringSplitOptions.None);
                                if (zoom.Count() > 0)
                                {
                                    double.TryParse(zoom[1], out zoomInit);
                                }
                            }
                            if (info.Contains("MaxZoom="))
                            {
                                string[] zoom = info.Split(new string[] { "MaxZoom=" }, StringSplitOptions.None);
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
                var ls = en.GetAllCamera();
                Dictionary<String, List<CameraItem>> camgroupbyname = new Dictionary<string, List<CameraItem>>();
                foreach (ComplexType1 complexType1 in ls)
                {
                    ComplexType1 com = complexType1;
                    var camItem = new CameraItem { Id = com.id, CameraUrl = com.cameraurl,Ip = com.ip, Type = com.type};
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
            
            types.Add(new ScreenTypeViewModel { Type = "2x2 Screen" });
            types.Add(new ScreenTypeViewModel { Type = "3x3 Screen" });
            types.Add(new ScreenTypeViewModel { Type = "4x4 Screen" });
            types.Add(new ScreenTypeViewModel { Type = "5x5 Screen" });
            types.Add(new ScreenTypeViewModel { Type = "6x6 Screen" });
            types.Add(new ScreenTypeViewModel { Type = "Spec One" });
            types.Add(new ScreenTypeViewModel { Type = "One Screen" });
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
                        String data = (item.SelectedItem as CameraItemViewModel).CamUrl +";"+
                                      (item.SelectedItem as CameraItemViewModel).Ip +";"+
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
            _allvideScreen.Clear();
            ResourceUtil.Listvideo.Clear();
            grCentreboard.Children.Clear();
            grfooterleft.Children.Clear();
            btnShowHideLeft = null;
            Dispatcher.InvokeShutdown();
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
                SetVideo(2);
            }
            if (comboBox.SelectedIndex == 1) //3x3 Screen
            {
                SetVideo(3);
            }
            if (comboBox.SelectedIndex == 2) //4x4 Screen
            {
                SetVideo(4);
            }
            if (comboBox.SelectedIndex == 3)//5x5 Screen
            {
                SetVideo(5);
            }
            if (comboBox.SelectedIndex == 4)//6x6 Screen
            {
                SetVideo(6);
            }
            if (comboBox.SelectedIndex == 5)// Special Screen
            {
                if (_allvideScreen.ContainsKey(7))
                {
                    ClearAllVideo();
                    var specScreenOne = _allvideScreen[7] as SpecScreenOne;
                    if (specScreenOne != null) specScreenOne.DrawControls();
                    centreshowScreen.Content = _allvideScreen[7];
                }
                else
                {
                    ClearAllVideo();
                    SpecScreenOne screen = new SpecScreenOne();
                    screen.DrawControls();
                    _allvideScreen.Add(7, screen);
                    centreshowScreen.Content = screen;
                }
            }
            if (comboBox.SelectedIndex == 6) // Special Screen
            {
                SetVideo(1);
            }
        }

        public void ClearAllVideo()
        {
            foreach (var userControl in _allvideScreen)
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
            if (_allvideScreen.ContainsKey(num))
            {
                ClearAllVideo();
                (_allvideScreen[num] as CentreScreenVideo).AddVideoToCentreGrid(num);
                centreshowScreen.Content = _allvideScreen[num];
            }
            else
            {
                ClearAllVideo();
                CentreScreenVideo screen = new CentreScreenVideo();
                screen.SetNumberofScreen(num);
                _allvideScreen.Add(num,screen);
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

        private void UpButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy  || (tiltInit == maxTilt)) 
                return;

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
            panInit -= 10;
            if (panInit < minPan)
                panInit = minPan; 
            
            m_oWorker.RunWorkerAsync();
        }

        private void RigButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (panInit == maxPan))
                return;
            panInit += 10;
            if (panInit > maxPan)
                panInit = maxPan; 

            m_oWorker.RunWorkerAsync();
        }

        private void ZoomInButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (zoomInit == maxZoom))
                return;

            zoomInit += 20;
            if (zoomInit > maxZoom)
                zoomInit = maxZoom;
            m_oWorker.RunWorkerAsync();
        }

        private void ZoomOutButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (zoomInit == minZoom))
                return;

            zoomInit -= 20;
            if (zoomInit < minZoom)
                zoomInit = minZoom;

            m_oWorker.RunWorkerAsync();
        }

        private void DownButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (m_oWorker.IsBusy || (tiltInit == minTilt))
                return;
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
        }

        private void GrCentre_OnMouseEnter(object sender, MouseEventArgs e)
        {
            btnShowHideLeft.Visibility = Visibility.Visible;
        }

        private void GrCentre_OnMouseLeave(object sender, MouseEventArgs e)
        {
            btnShowHideLeft.Visibility = Visibility.Hidden;
        }

        
    }

    /// <summary> 
    /// Add-In implementation 
    /// </summary>
    [AddIn("LiveView", Version = "1.0.0.0", Publisher = "Hoanglm",
        Description = "Returns an LiveView usercontrol")]
    public class WPFAddIn : IWPFAddInView
    {
        private LiveViewUsercontrol con;
        public FrameworkElement GetAddInUI()
        {
            // Return add-in UI 
            con = new LiveViewUsercontrol();
            return con;
        }

        /// <summary>
        /// Close all the video screen
        /// </summary>
        /// <param name="mess"></param>
        public bool EndMess(bool mess)
        {
            if(mess)
                con.DisposeAllScreen();

            return true;
        }
    }
}
