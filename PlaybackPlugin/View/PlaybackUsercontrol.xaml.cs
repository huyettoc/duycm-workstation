using System;
using System.AddIn;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using AddInView;
using PlaybackPlugin.EntityFramework;
using PlaybackPlugin.Model;
using PlaybackPlugin.ViewModel;

namespace PlaybackPlugin.View
{
    /// <summary>
    /// Interaction logic for PlaybackUsercontrol.xaml
    /// </summary>
    public partial class PlaybackUsercontrol : UserControl
    {

        #region Mems
        private HighCameraGroupViewModel _viewModel;
        //private BackgroundWorker _backgroundWorker;
        private Dictionary<int, UserControl> _allvideScreen;

        //Controller
        //private int panInit = 0;
        //private int zoomInit = 1;
        //private int tilt = 0;
        //private string _ipselected = String.Empty;
        //BackgroundWorker m_oWorker;
        //private String _comRe = @"http://{0}/axis-cgi/com/ptz.cgi?query=position,limits&camera=1&html=no&zoom={1}&pan={2}&tilt={3}";
        //end
        #endregion
        public PlaybackUsercontrol()
        {
            InitializeComponent();
            Loaded += PlaybackUsercontrol_Loaded;
        }

        void PlaybackUsercontrol_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = new HighCameraGroupViewModel();
            DataContext = _viewModel;
            GetCameraData();
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
                                      (item.SelectedItem as CameraItemViewModel).Type;
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
    }


    /// <summary> 
    /// Add-In implementation 
    /// </summary>
    [AddIn("PlayBack", Version = "1.0.0.0", Publisher = "Hoanglm",
        Description = "Returns an PlayBack usercontrol")]
    public class WPFAddIn : IWPFAddInView
    {
        private PlaybackUsercontrol con;
        public FrameworkElement GetAddInUI()
        {
            // Return add-in UI 
            con = new PlaybackUsercontrol();
            return con;
        }

        ///// <summary>
        ///// Close all the video screen
        ///// </summary>
        ///// <param name="mess"></param>
        public bool EndMess(bool mess)
        {
            //if (mess)
            //    con.DisposeAllScreen();

            return true;
        }
    }
    
}
