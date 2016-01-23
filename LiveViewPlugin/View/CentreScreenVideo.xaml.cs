using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LiveViewPlugin.Utils;

namespace LiveViewPlugin.View
{
    /// <summary>
    /// Interaction logic for _2x2Screen.xaml
    /// </summary>
    public partial class CentreScreenVideo : IDisposable
    {
        #region Variable
        //List<VideoScreenUserControl> videos = new List<VideoScreenUserControl>();
        private bool _disposed = false;
        #endregion

        #region Constructor
        public CentreScreenVideo()
        {
            InitializeComponent();
            Loaded+=Screen_Loaded;
        }

        public void SetNumberofScreen(int numscreen)
        {
            DrawControls(numscreen);
        }
        private void Screen_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        #endregion

        #region Method

        private void DrawControls(int numberScreen)
        {
            grCentreScreen.Children.Clear();
            // Adding Rows and Colums to Grid.
            //int count = 2; // 2x2
            RowDefinition[] rows = new RowDefinition[numberScreen];
            ColumnDefinition[] columns = new ColumnDefinition[numberScreen ];
            // Draw Rows.
            for (int i = 0; i < numberScreen; i++)
            {
                rows[i] = new RowDefinition();
                // Setting Row height.
                rows[i].Height = new GridLength(1, GridUnitType.Star);
                grCentreScreen.RowDefinitions.Add(rows[i]);
               
            }

            // Draw Columns.
            for (int i = 0; i < numberScreen; i++)
            {
                columns[i] = new ColumnDefinition();
                columns[i].Width = new GridLength(1, GridUnitType.Star);
                grCentreScreen.ColumnDefinitions.Add(columns[i]);
                
            }

            //Draw Screen in Grid
            for (int i = 0; i < numberScreen ; i++)
                for (int j = 0; j < numberScreen ; j++)
                {
                    //VideoScreenUserControl video = new VideoScreenUserControl();
                    VideoScreenUserControl video = ResourceUtil.Listvideo[i*numberScreen + j];
                    video.MouseLeftButtonDown += video_MouseLeftButtonDown;
                    Grid.SetRow(video,i);
                    Grid.SetColumn(video,j);
                    grCentreScreen.Children.Add(video);
                    //videos.Add(video);
                }
        }

        public void ClearCentreGrid()
        {
            if(grCentreScreen.Children.Count > 0)
                grCentreScreen.Children.Clear();
        }

        public void AddVideoToCentreGrid(int numScreen)
        {
            ClearCentreGrid();
            for (int i = 0; i < numScreen; i++)
                for (int j = 0; j < numScreen; j++)
                {
                    //VideoScreenUserControl video = new VideoScreenUserControl();
                    VideoScreenUserControl video = ResourceUtil.Listvideo[i * numScreen + j];
                    video.MouseLeftButtonDown += video_MouseLeftButtonDown;
                    Grid.SetRow(video, i);
                    Grid.SetColumn(video, j);
                    grCentreScreen.Children.Add(video);
                    //videos.Add(video);
                }
        }

        void video_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var video = sender as VideoScreenUserControl;
            //if (video == null)
            //    return;
            //foreach (var videoScreenUserControl in ResourceUtil.Listvideo)
            //{
            //    if (video.Equals(videoScreenUserControl))
            //    {
            //        videoScreenUserControl.IsSelected = true;
            //        videoScreenUserControl.BorderThickness = new Thickness(2);
            //        videoScreenUserControl.BorderBrush = new SolidColorBrush(Colors.Goldenrod);
            //    }
            //    else
            //    {
            //        videoScreenUserControl.IsSelected = false;
            //        videoScreenUserControl.BorderThickness = new Thickness(0);
            //    }
            //}
        }
        #endregion

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    grCentreScreen.Children.Clear();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
