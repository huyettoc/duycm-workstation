using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiveViewPlugin.ViewModel
{
    public class VideoScreenViewModel : ViewModelBase
    {

        ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                //OnPropertyChanged("ImageSource");
                RaisePropertyChanged(() => ImageSource);
            }
        }

        public VideoScreenViewModel()
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(@"/LiveViewPlugin;component/Images/iconwattermarkone.png", UriKind.RelativeOrAbsolute);
            bmp.EndInit();
            ImageSource = bmp;
        }
    }
}
