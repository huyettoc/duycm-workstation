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
using LiveViewPlugin.Utils;

namespace LiveViewPlugin.View
{
    /// <summary>
    /// Interaction logic for SpecScreenOne.xaml
    /// </summary>
    public partial class SpecScreenOne : IDisposable
    {
        #region Mems
        //List<VideoScreenUserControl> videos = new List<VideoScreenUserControl>();
        private bool disposed = false;
        #endregion

        public SpecScreenOne()
        {
            InitializeComponent();
            Loaded += SpecScreenOne_Loaded;
        }

        void SpecScreenOne_Loaded(object sender, RoutedEventArgs e)
        {
            //GetAllScreen();
        }

        public void DrawControls()
        {

            //RowDefinition[] rows = new RowDefinition[2];
            //ColumnDefinition[] columns = new ColumnDefinition[2];
            //rows[0] = new RowDefinition();
            //rows[0].Height = new GridLength(2, GridUnitType.Star);
            //grLeftvideo.RowDefinitions.Add(rows[0]);
            //rows[1] = new RowDefinition();
            //rows[1].Height = new GridLength(1, GridUnitType.Star);
            //grLeftvideo.RowDefinitions.Add(rows[1]);

            //columns[0] = new ColumnDefinition();
            //columns[0].Width = new GridLength(2, GridUnitType.Star);
            //grLeftvideo.ColumnDefinitions.Add(columns[0]);


            //columns[1] = new ColumnDefinition();
            //columns[1].Width = new GridLength(2, GridUnitType.Star);
            //grLeftvideo.ColumnDefinitions.Add(columns[1]);

            //Video for leftgrid

            VideoScreenUserControl video = ResourceUtil.Listvideo[0];
            VideoScreenUserControl video3 = ResourceUtil.Listvideo[3];
            Grid.SetRow(video,0);
            Grid.SetColumn(video, 0);
            Grid.SetRow(video3, 1);
            Grid.SetColumn(video3, 1);
            grLeftvideo.Children.Add(video);
            grLeftvideo.Children.Add(video3);
            

            //Video for right grid
            VideoScreenUserControl video1 = ResourceUtil.Listvideo[1];
            VideoScreenUserControl video2 = ResourceUtil.Listvideo[2];
            Grid.SetColumn(video1, 0);
            Grid.SetRow(video1, 0);
            Grid.SetColumn(video2, 0);
            Grid.SetRow(video2, 1);

            grRightvideo.Children.Add(video1);
            grRightvideo.Children.Add(video2);

            //Video for right grid
            VideoScreenUserControl video4 = ResourceUtil.Listvideo[4];
            VideoScreenUserControl video5 = ResourceUtil.Listvideo[5];
            Grid.SetColumn(video4, 1);
            Grid.SetRow(video4, 0);
            Grid.SetColumn(video5,0);
            Grid.SetRow(video5, 0);
            grBottomVideo.Children.Add(video4);
            grBottomVideo.Children.Add(video5);

        }

        public void ClearListControl()
        {
            if (grBottomVideo.Children.Count > 0)
                grBottomVideo.Children.Clear();

            //if (grLeftvideo.Children.Count > 0)
                //grLeftvideo.Children.Clear();

            if (grRightvideo.Children.Count > 0)
                grRightvideo.Children.Clear();
            if(grLeftvideo.Children.Count >= 3)
                if (grLeftvideo.Children[2] != null)
                    grLeftvideo.Children.RemoveAt(2);
            if (grLeftvideo.Children.Count >= 3)
                if (grLeftvideo.Children[2] != null)
                    grLeftvideo.Children.RemoveAt(2);
            
        }


        private void UcVideoScreen1_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectVideoScreen(sender as VideoScreenUserControl);
        }

        private void UcVideoScreen2_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectVideoScreen(sender as VideoScreenUserControl);
        }

        public void SelectVideoScreen(VideoScreenUserControl screen)
        {
            foreach (var videoScreenUserControl in ResourceUtil.Listvideo)
            {
                if (screen.Equals(videoScreenUserControl))
                {
                    videoScreenUserControl.IsSelected = true;
                    videoScreenUserControl.BorderThickness = new Thickness(2);
                    videoScreenUserControl.BorderBrush = new SolidColorBrush(Colors.Gold);
                }
                else
                {
                    videoScreenUserControl.IsSelected = false;
                    videoScreenUserControl.BorderThickness = new Thickness(0);
                }
            }
        }


        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                   grBottomVideo.Children.Clear();
                   grLeftvideo.Children.Clear();
                   grRightvideo.Children.Clear();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
