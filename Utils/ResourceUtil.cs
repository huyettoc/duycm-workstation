using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ITSWorkStation.View;

namespace ITSWorkStation.Utils
{
    public static class ResourceUtil
    {
        #region String

        public static String WrongAccountInfo = "Tên hoặc mật khẩu không chính xác";//"Username or Passwork is wrong";
        public static String EmptyAccountInfo = "Điền tên và mật khẩu của bạn";
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

        public static int BuffSize = 1000;
        public static string Server = "227.1.1.1";
        public static int Port = 19009;
        public static bool LogStatus = false;
        #endregion

        #region Dic
        public static Dictionary<String, FrameworkElement> Tabview = new Dictionary<string, FrameworkElement>(); //Dic that containt all usercontrol loaded from plugin. 
        #endregion
    }


}
