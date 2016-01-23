using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveViewPlugin.View;

namespace LiveViewPlugin.Utils
{
    public static class ResourceUtil
    {
        #region

        public static String WrongAccountInfo = "Username or Passwork is wrong";
        public static String EmptyAccountInfo = "Please enter your account";
        public static String UrlCam = "rtsp://root:admin@172.30.1.2/axis-media/media.amp";
        public static List<String> ListItemScreenType = new List<string>
        {
            "2x2",
            "3x3",
            "4x4",
            "5x5",
            "6x6"
        }; 
        public static List<VideoScreenUserControl> Listvideo = new List<VideoScreenUserControl>(); 
        #endregion
    }
}
