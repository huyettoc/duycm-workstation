using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveViewPlugin.Model
{
    public class CameraItem
    {
        public string Id { get; set; }
        public String CameraUrl { get; set; }
        public String Ip { get; set; }
        public SByte Type { get; set; }
    }
}
