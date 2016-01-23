using System;
using System.AddIn.Hosting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using BusyIndicatorLib;
using HostView;
using ITSWorkStation.Utils;
using ITSWorkStation.View;
using log4net;
using log4net.Config;


namespace ITSWorkStation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        #region Variable
        private IWPFAddInHostView _addin;
        private IList<AddInToken> _tokens;
        Collection<IWPFAddInHostView> addInViews = new Collection<IWPFAddInHostView>();
        private AddInToken _selectedItem;
        private BusyIndicatorHandler _busy;
        private bool _loadready; //Check if load complete
        private LiveViewUsercontrol _liveView = null;
        private Login _login;

        //Client that get the muticast message
        private ClientUdp<MultiCastMessage.MultiCastMessage> _infoClient;
        //End
        #endregion
        #region Method
        public MainWindow()
        {
            InitializeComponent();
            Closing+=MainWindow_Closing;
            _busy = new BusyIndicatorHandler();
            KeyDown += MainWindow_KeyDown;
            KeyUp += MainWindow_KeyUp;
            Loaded += MainWindow_Loaded;
            //Login();
           
        }

        void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (_liveView != null)
            {
                //_liveView.
                _liveView.KeyUp();
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (ConfigurationManager.AppSettings["log"] == "true")
                ResourceUtil.LogStatus = true;
            else
                ResourceUtil.LogStatus = false;
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (Topgrid.ActualHeight.Equals(0))
                {
                    Topgrid.Height = new GridLength(35);
                    if (_liveView != null)
                    {
                        _liveView.SetFullScreen(false);
                    }
                }
                else
                {
                    Topgrid.Height = new GridLength(0);
                    if (_liveView != null)
                    {
                        _liveView.SetFullScreen(true);
                    }
                }
                

            }
            if (e.Key == Key.B)
            {
                if (_liveView != null)
                {
                    _liveView.SnapshotByKey();
                }
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            //if(_addin != null)
            //    _addin.EndMess(true);
            Environment.Exit(1);
        }
        /// <summary>
        /// Create client muticast mess with MultiCastMessage
        /// </summary>
        public void CreateInfoClient()
        {
            _infoClient = new ClientUdp<MultiCastMessage.MultiCastMessage>(ResourceUtil.Server, ResourceUtil.Port);
            _infoClient.EventGetMess += InfoClient_EventGetMess;
        }

        private void InfoClient_EventGetMess(MultiCastMessage.MultiCastMessage mess)
        {
            if(_liveView == null)
                return;
            int eventId = 0;
            bool status = false;

            var str = System.Text.Encoding.Default.GetString(mess.xmlInfor);
            if (str.Contains("ON"))
                status = true;
            else
                status = false;

            int.TryParse(mess.eventID, out eventId);
            if (eventId != 0)
            {
                _liveView.ShowEvent(eventId,mess.cameraID,status);
            }

        }

        /// <summary>
        /// Call Login form when app start
        /// </summary>
        public void Login()
        {
            _login = new Login();
            _login.Owner = this;
            var res = _login.ShowDialog();
            //if(res == null)
            //    _login.Show();
            if (Account.Username == string.Empty)
            {
                try
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Close();
                }
                catch (Exception ex)
                {
                    
                }
            }
            //_backgroundWorker = new BackgroundWorker();
            //_backgroundWorker.DoWork += BackgroundWorker_DoWork;
            //_backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            //_backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            //_backgroundWorker.WorkerReportsProgress = true;
            //BusyIndicatorHandler busy = new BusyIndicatorHandler();
            _busy.Start();
            LoadPlugin();
            if (_liveView == null)
                _liveView = new LiveViewUsercontrol();
            _busy.Stop();
            _loadready = true;
            liveviewtab.Visibility = Visibility.Visible;
            LivevieLine.Visibility = Visibility.Visible;
            liveviewtab.Foreground = new SolidColorBrush(Colors.Goldenrod);
            //userControlview.Content = _liveView;
            userControlview.Content = _liveView;
            //Content.UpdateLayout();
            CreateInfoClient();
        }

        //void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    // get selected addin
        //    try
        //    {
        //        AddInToken token = _selectedItem;

        //        _addin = token.Activate<HostView.WPFAddInHostView>(AddInSecurityLevel.FullTrust);
        //        ResourceUtil.Tabview.Add(LbxMenu.SelectedIndex.ToString(CultureInfo.InvariantCulture), _addin);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    ResourceUtil.Tabview.Add(LbxMenu.SelectedIndex.ToString(CultureInfo.InvariantCulture), _addin);
        //    if (ResourceUtil.Tabview.ContainsKey(LbxMenu.SelectedIndex.ToString(CultureInfo.InvariantCulture)))
        //    {
        //        userControlview.Content =
        //            ResourceUtil.Tabview[LbxMenu.SelectedIndex.ToString(CultureInfo.InvariantCulture)];
        //        StartStopWait();
        //    }
        //}
        
        //void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //_backgroundWorker.ReportProgress(1);
        //    try
        //    {
        //        Dispatcher.Invoke(DispatcherPriority.Normal, new DoMywork(ThreadStartingPoint));
        //    }
        //    catch (Exception ex)
        //    {
        //        BackgroundWorker w = sender as BackgroundWorker;
        //    }
        //}
        //private delegate void DoMywork();
        //private void ThreadStartingPoint()
        //{
        //    try
        //    {
        //        _addin = _selectedItem.Activate<HostView.WPFAddInHostView>(AddInSecurityLevel.FullTrust);
                
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //}

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Login();
        }

        /// <summary>
        /// Load all plugin in AddIns folder
        /// </summary>
        public void LoadPlugin()
        {
            return;
            string path = Environment.CurrentDirectory;
            AddInStore.Update(path);
            AddInStore.RebuildAddIns(path);

            // IList<AddInToken> tokens = AddInStore.FindAddIns(typeof(HostView.NumberProcessorHostView), path);
            _tokens = AddInStore.FindAddIns(typeof(HostView.IWPFAddInHostView), path);
            LbxMenu.ItemsSource = _tokens;
            

        }

        /// <summary>
        /// User change select item on listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        
        private void LbxMenu_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbxMenu.SelectedIndex != -1)
            {
                
                //Reset Live view tab to unselected
                liveviewtab.Foreground = new SolidColorBrush(Colors.White);
                //End
                if (!ResourceUtil.Tabview.ContainsKey(LbxMenu.SelectedIndex.ToString(CultureInfo.InvariantCulture)))
                {
                    _selectedItem = (AddInToken)LbxMenu.SelectedItem;
                    //Start Busy
                    
                    _busy.Start();
                    #region Test
                    //_backgroundWorker.RunWorkerAsync();
                    //Thread nThread = new Thread(new ParameterizedThreadStart(WorkThreadFunction));
                    //nThread.SetApartmentState(ApartmentState.STA);
                    //nThread.IsBackground = true;
                    //nThread.Start(LbxMenu.SelectedIndex.ToString(CultureInfo.InvariantCulture));
                    //var result = Task<UserControl>.Factory.StartNew(() =>
                    //    {
                    //        AddInToken token = _selectedItem;
                    //        try
                    //        {
                    //            UserControl con = new UserControl();
                    //            con = token.Activate<HostView.WPFAddInHostView>(AddInSecurityLevel.FullTrust);
                    //        }
                    //        catch (Exception ex)
                    //        {
                                
                    //        }

                    //        return null;
                    //    }
                    //);
                    //// Wait for the task opening the file to complete
                    //result.Wait();
                    //userControlview.Content = (result.Result);
                    #endregion

                    _loadready = false;
                    //Main thread create a Usercontrol
                    AddInToken token = _selectedItem;
                    _addin = token.Activate<HostView.IWPFAddInHostView>(AddInSecurityLevel.FullTrust);
                    if(!addInViews.Contains(_addin))
                        addInViews.Add(_addin);

                    FrameworkElement addInUI = this._addin.GetAddInUI();

                    ResourceUtil.Tabview.Add(LbxMenu.SelectedIndex.ToString(CultureInfo.InvariantCulture), addInUI as FrameworkElement);
                    userControlview.Content = addInUI;
                    
                    //End
                    //Stop Busy
                    _busy.Stop();
                    _loadready = true;
                    
                }
                else
                {
                    userControlview.Content = ResourceUtil.Tabview[LbxMenu.SelectedIndex.ToString(CultureInfo.InvariantCulture)];
                }
            }
        }

        //public void WorkThreadFunction(object name)
        //{
        //    try
        //    {
        //        AddInToken token = _selectedItem;

        //        _addin = token.Activate<HostView.WPFAddInHostView>(AddInSecurityLevel.FullTrust);
        //        ResourceUtil.Tabview.Add((String)name, _addin);
        //        Dispatcher.BeginInvoke((Action)delegate
        //        {
        //            UserControl con = (_addin as UserControl);
        //            userControlview.Content = con;
        //            userControlview.IsEnabled = !userControlview.IsEnabled;
        //        },
        //    DispatcherPriority.Normal);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private void LogoutBlock_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_loadready)
                return;
            ResourceUtil.Tabview.Clear();
            Account.Username = String.Empty;
            liveviewtab.Foreground = new SolidColorBrush(Colors.White);
            liveviewtab.Visibility = Visibility.Hidden;
            LivevieLine.Visibility = Visibility.Hidden;
            //_tokens.Clear();
            //LbxMenu.ItemsSource = null;
            if (_addin != null)
            {
                if(!_addin.EndMess(true))
                    return;
                //Unload the addIn
                ShutdownPlugin();
                //end
                _addin = null;
                LbxMenu.ItemsSource = null;
                _loadready = false;
            }
            //userControlview.Content = new UserControl();
            _liveView.DisposeAllScreen();
            ((ContentControl)_liveView.Parent).RemoveChild(_liveView);
            _liveView = null;
            userControlview.Content = null;
            _infoClient.Disconnect();
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            
            Login();
        }

        public void ShutdownPlugin()
        {
            // Unload add-ins
            foreach (var addInView in this.addInViews)
            {
                Debug.Write("Shutdow start");
                try
                {
                    AddInController addInController = AddInController.GetAddInController(addInView);
                    addInController.Shutdown();
                }
                catch (Exception ex)
                {
                    
                }
                
                Debug.Write("Shutdow end");
            }
            addInViews.Clear();
        }

        #endregion

        private void Liveviewtab_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            liveviewtab.Foreground = new SolidColorBrush(Colors.Goldenrod);
            LbxMenu.UnselectAll();
            userControlview.Content = _liveView;
            //if (!userControlview.Children.Contains(_liveView))
            //{
            //    userControlview.Children.Clear();
            //    userControlview.Children.Add(_liveView);
            //}
        }
    }
}
