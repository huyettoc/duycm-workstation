using System.Collections.Generic;

namespace PlaybackPlugin.Model
{
    public class CameraGroup
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<CameraItem> CameraList { get; set; }
    }
}
