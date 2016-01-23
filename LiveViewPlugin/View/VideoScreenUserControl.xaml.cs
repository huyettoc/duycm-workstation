using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Threading;
using System.Windows.Threading;
using LiveViewPlugin.Utils;
using LiveViewPlugin.ViewModel;
using log4net;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace LiveViewPlugin.View
{
    /// <summary>
    /// Interaction logic for VideoScreenUserControl.xaml
    /// </summary>
    public partial class VideoScreenUserControl : IDisposable
    {

        #region Variable
        private ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _url;
        //byte[] _image = new byte[800 * 600 * 3];
        //private Decoder _decode;
        //private BitmapSource bitmapSource = null;
        private CallbackDelegate _del;
        //private CallEventbackDelegate eventcb;
        //GCHandle _pinnedArray;
        //IntPtr _pointer;
        private bool _disposed; // to detect redundant calls
        public delegate void DelEventHandler(string newIpselected);
        public event DelEventHandler add;
        private bool _stopByUser = false;

        private DispatcherTimer timer;
        private double _startTime = -1;
        #endregion

        #region Properties

        public bool IsSelected { get; set; }
        public bool IsDisplaying { get; set; }
        public String Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public String Ip { get; set; }
        public int Type { get; set; }
        #endregion

        #region Declare
        unsafe public delegate void CallbackDelegate(IntPtr samples, uint length, uint timestamp, IntPtr user_data, uint reserved1, uint reserved2);
        //public unsafe delegate bool CallEventbackDelegate(uint eventtype);

        [DllImport("SharpRTSPClient.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void SetRawPictureCB(CallbackDelegate cbFuntion);

        [DllImport("SharpRTSPClient.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe static extern void StartRTSPClient(byte[] url);

        [DllImport("SharpRTSPClient.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe static extern void StopRTSPClient(byte[] url);

        [DllImport("SharpRTSPClient.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void StartRTSPClientWithCallback(CallbackDelegate cbFuntion, byte[] url);

        [DllImport("SharpRTSPClient.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void StopLoopEvent(byte[] url);

        //[DllImport("SharpRTSPClient.dll", CallingConvention = CallingConvention.Cdecl)]
        //private static extern unsafe void StartRTSPClientWithEventCallback(CallEventbackDelegate eventtype, CallbackDelegate cbFuntion, byte[] url);


        #endregion

        
        #region Contructors
        public VideoScreenUserControl()
        {
            InitializeComponent();
            Loaded += VideoScreenUserControl_Loaded;
            Url = String.Empty;
            Ip = String.Empty;
            _del = null;
            //eventcb = null;
            Type = -1;
            var viewmode = new VideoScreenViewModel();
            DataContext = viewmode;

            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(4);
            timer.Tick += timer_Tick;
        }

        void VideoScreenUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MouseLeftButtonDown += VideoScreenUserControl_MouseLeftButtonDown;
            
            //RenderOptions.SetCachingHint(imVideo, CachingHint.Cache);
            //RenderOptions.SetBitmapScalingMode(imVideo, BitmapScalingMode.LowQuality);
            btnClose.Visibility = Visibility.Hidden;
            
        }

        private void Callback(object state)
        {
            if(_url == String.Empty)
                return;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] arrUrl = encoding.GetBytes(_url);
            var res = DateTime.Now.Ticks - _startTime;
            TimeSpan elapsedSpan = new TimeSpan((long)res);
            
            if (elapsedSpan.TotalSeconds > 3)
            {
                tblInfo.Visibility = Visibility.Visible;
                Log.Info("Loading show:"+_url); 
                StopLoopEvent(arrUrl);
                StopRTSPClient(arrUrl);

                try
                {
                    //canRetry = true;
                    StartRTSPClientWithCallback(_del, arrUrl);
                    //StartRTSPClientWithEventCallback(eventcb,del1,arrUrl);
                }
                catch (Exception ex)
                {
                    Log.Debug(ex.ToString() + "\n");
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if(_url == String.Empty)
                return;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] arrUrl = encoding.GetBytes(_url);
            var res = DateTime.Now.Ticks - _startTime;
            TimeSpan elapsedSpan = new TimeSpan((long)res);
            
            if (elapsedSpan.TotalSeconds > 3)
            {
                tblInfo.Visibility = Visibility.Visible;
                Log.Info("Loading show:"+_url); 
                StopLoopEvent(arrUrl);
                StopRTSPClient(arrUrl);

                try
                {
                    //canRetry = true;
                    StartRTSPClientWithCallback(_del, arrUrl);
                    //StartRTSPClientWithEventCallback(eventcb,del1,arrUrl);
                }
                catch (Exception ex)
                {
                    Log.Debug(ex.ToString() + "\n");
                }
            }
        }

        private void InitImage()
        {
            System.Drawing.Image ima = new Bitmap(Resource1.iconwattermarkone, new System.Drawing.Size((int)cvsVideo.ActualWidth - 1,(int)cvsVideo.ActualHeight -1));
            //pictureBox.Height = (int)cvsVideo.ActualHeight;
            //pictureBox.Width = (int)cvsVideo.ActualWidth;
            //pictureBox.Image = ima;
        }

        void VideoScreenUserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = true;
        }

        public VideoScreenUserControl(string url)
        {
            _url = url;
            //Start the RTSP client

            //End
        }
        #endregion

        #region  Destructor
        ~VideoScreenUserControl()
        {
            //If(Video is displaying, stop it)
            if (IsDisplaying && _url != String.Empty)
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] arrUrl = encoding.GetBytes(_url);
                StopRTSPClient(arrUrl);
            }
        }
        #endregion

        private void CvsVideo2_OnDragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("MyTreeViewItem") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Copy;
            }
        }
        /// <summary>
        /// On drop event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CvsVideo2_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("MyTreeViewItem"))
            {
                _stopByUser = false;
                String data = e.Data.GetData("MyTreeViewItem") as String;
                
                //Concat data
                string oldUrl = Url;
                string[] splitdata = data.Split(';');
                if (splitdata.Count() == 3)
                {
                    if (Url == splitdata[0])
                        return ;
                    if(!CheckCameraUrl(splitdata[0]))
                        return;
                    Url = splitdata[0];//0 is url
                    Ip = splitdata[1]; //1 is Ip
                    int type;
                    int.TryParse(splitdata[2], out type);
                    Type = type;
                }
                else if (splitdata.Count() >= 1)
                {
                    if (Url == splitdata[0])
                        return ;
                    if (!CheckCameraUrl(splitdata[0]))
                        return;

                    Url = splitdata[0];
                }
                if (Url == String.Empty || Url == oldUrl)
                    return;
                //end
                if (splitdata.Count() == 3)
                {
                    tblcameName.Content = splitdata[2];
                    tblcameName.Foreground = new SolidColorBrush(Colors.White);
                }

                //if this video is selected raise a event to change ipselected to Liveview dll
                if (IsSelected)
                {
                    if (add != null)
                        add(Ip);
                }

                //end
                if (IsDisplaying && oldUrl != String.Empty)
                {
                     timer.Stop();//Stop retry
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    //byte[] arrUrl = encoding.GetBytes(Url);
                    byte[] arrUrl = encoding.GetBytes(oldUrl);
                    //StopLoopEvent(arrUrl); //Hoanglm close loop event
                    StopRTSPClient(arrUrl);
                    _del = null;
                    //eventcb = null;
                    IsDisplaying = false;
                }
                
                //Create a 
                Thread clientRtspThread = new Thread(() =>
                {
                    unsafe
                    {
                        _del = new CallbackDelegate(this.Decode);
                        //eventcb = new CallEventbackDelegate(mEventChange);
                        //Change the GB collection
                        //Hoanglm
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        //End
                        StartOrStopRTSPClien(true, Url);
                    }
                });
                clientRtspThread.Start();
                timer.Start(); //Start time to retry
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckCameraUrl(String camurl)
        {
            bool checkUrl = false;
            foreach (var video in ResourceUtil.Listvideo)
            {
                if (video.Url == camurl)
                {
                    checkUrl = true;
                    break;
                }
            }
            if (checkUrl)
                return false;
           
            return true;
        }

        /// <summary>
        /// Start or Stop the RTSP client
        /// </summary>
        /// <param name="status"></param>
        /// <param name="add"></param>
        public void StartOrStopRTSPClien(bool status, String add)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] arrUrl = encoding.GetBytes(add);
            try
            {
                if (!status)
                {
                    _url = String.Empty;
                    IsDisplaying = false;
                    StopLoopEvent(arrUrl); //close event loop
                    StopRTSPClient(arrUrl);
                }
                else
                {
                    _url = add;
                    IsDisplaying = true;
                    //StartRTSPClient(arrUrl);
                    StartRTSPClientWithCallback(_del, arrUrl);
                    //StartRTSPClientWithEventCallback(eventcb,_del,arrUrl);
                }
            }
            catch (Exception ex)
            {
                #if(DEBUG)
                    //Log.Debug(ex.ToString());
                #else
                #endif
            }
            
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        unsafe void Decode(IntPtr samples, uint length, uint timestamp, IntPtr user_data, uint reserved1, uint reserved2)
        {
            if(_stopByUser)
                return;
#if(DEBUG)
            //Log.Debug("Start get image");
#else
#endif
            Thread createImageThread = new Thread(() => CreateImage(samples, length, timestamp, user_data, reserved1, reserved2));
            createImageThread.Start();
            //Task taskA = Task.Factory.StartNew(() => CreateImage(samples, length, timestamp, user_data, reserved1, reserved2));
            //taskA.Start();
            
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        private unsafe bool mEventChange(uint eventtype)
        {
            if (_stopByUser)
                return false;

            Thread createEventThread = new Thread(() =>
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    Log.Info("Loading show:" + _url);
                    //tblInfo.Visibility = Visibility.Visible;

                }), DispatcherPriority.Normal, null);
            }

                );
            createEventThread.Start();
            return true;
        }

        private void CreateImage(IntPtr samples, uint length, uint timestamp, IntPtr user_data, uint reserved1,
            uint reserved2)
        {
            
            //Marshal.Copy(samples, image, 0, (int)(reserved1 * reserved2) * 3);
            //GCHandle pinnedArray;
            //YUV2RGBManaged(yuv, image, width, height);
            {
                //pinnedArray = GCHandle.Alloc(image, GCHandleType.Pinned);
                //pointer = pinnedArray.AddrOfPinnedObject();

                var width = (int) reserved1; // for example
                var height = (int) reserved2; // for example
                var dpiX = 96;//96d
                var dpiY = 96;
                var pixelFormat = PixelFormats.Rgb24; //
                var bytesPerPixel = (pixelFormat.BitsPerPixel + 7)/8; // == 1 in this example
                var stride = bytesPerPixel*width; // == width in this example

                //BitmapSource.Create()
                Dispatcher.BeginInvoke((Action) (() =>
                {
                    //_viewmode.ImageSource = BitmapSource.Create(width, height, dpiX, dpiY,
                    //    pixelFormat, null, samples, (int) length, stride);
                    try
                    {
                        //_viewmode.ImageSource = Imaging.CreateBitmapSourceFromMemorySection(samples,

                        //                              width,

                        //                              height,

                        //                              PixelFormats.Rgb24,

                        //                              stride,

                        //                              0);
                        if (_stopByUser == false)
                        {
                            try
                            {
                                //Log.Info("Create bitmap:" + _url);
                                imVideo.Source = BitmapSource.Create(width, height, dpiX, dpiY,
                                pixelFormat, null, samples, (int) length, stride);
                                //Log to disk
                                //if (_url.Contains("vcam7"))
                                //{
                                //    var encoder = new PngBitmapEncoder();
                                //    String photolocation = "d://imagelog.png"; //file name 
                                //    BitmapFrame outputFrame = BitmapFrame.Create((BitmapSource) imVideo.Source);
                                //    encoder.Frames.Add(outputFrame);
                                //    //encoder.QualityLevel = 1;
                                //    using (var filestream = new FileStream(photolocation, FileMode.Create))
                                //        encoder.Save(filestream);
                                //}
                                // soure.Freeze();
                                //end
                                _startTime = DateTime.Now.Ticks;
                                tblInfo.Visibility = Visibility.Hidden;
                                //Log.Info("Create bitmap Ok:" + _url);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Create bitmap error :"+ _url +":"+ex.ToString());
                            }
                            
                        }
                        //_viewmode.ImageSource = bitmapSource;
                        //_viewmode.ImageSource.Freeze();
                        //imVideo.Source = bitmapSource;
                    }
                    catch (Exception ex)
                    {
                        
                    }

                }), DispatcherPriority.Normal, null);

            }
        }

        private void CvsVideo_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //IsSelected = !IsSelected;
            //if (IsSelected)
            //{
            //    this.BorderThickness = new Thickness(2);
            //    BorderBrush = new SolidColorBrush(Colors.);
            //}
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (IsDisplaying)
                    {
                        StartOrStopRTSPClien(false,Url);
                        imVideo.Source = null;
                        _del = null;
                        //eventcb = null;
                    }
                    Dispatcher.InvokeShutdown();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            //tblInfo.Visibility = Visibility.Hidden;
            if (IsDisplaying && Url != string.Empty)
            {
                StartOrStopRTSPClien(false,Url);
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(@"/LiveViewPlugin;component/Images/iconwattermarkone.png", UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                imVideo.Source = bmp;
                tblcameName.Content = "CCTV";
                tblInfo.Visibility = Visibility.Hidden;
                IsDisplaying = false;
                _stopByUser = true;
            }
        }

        private void VideoScreen_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if(IsDisplaying)
                btnClose.Visibility = Visibility.Visible;
            else
            {
                btnClose.Visibility = Visibility.Hidden;
            }
        }

        private void VideoScreen_OnMouseLeave(object sender, MouseEventArgs e)
        {
            btnClose.Visibility = Visibility.Hidden;
        }
        
    }
}
