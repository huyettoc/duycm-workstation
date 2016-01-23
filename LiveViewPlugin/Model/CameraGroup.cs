using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveViewPlugin.EntityFramework;

namespace LiveViewPlugin.Model
{
    public class CameraGroup
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public List<CameraItem> CameraList { get; set; }
    }
}
