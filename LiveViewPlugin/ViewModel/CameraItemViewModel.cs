using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveViewPlugin.Model;

namespace LiveViewPlugin.ViewModel
{
    public class CameraItemViewModel:ViewModelBase
    {
        #region Member
        public CameraItem CamItem { get; protected set; }
        #endregion

        #region Contructor
        public CameraItemViewModel(CameraItem cam)
        {
            CamItem = cam;
        }
        #endregion

        #region Properties

        public String CamID
        {
            get { return CamItem.Id; }
            set
            {
                if (CamItem.Id != value)
                {
                    CamItem.Id = value;
                    RaisePropertyChanged(() => CamID);
                }
            }
        }

        public String CamUrl
        {
            get { return CamItem.CameraUrl; }
            set
            {
                if (CamItem.CameraUrl != value)
                {
                    CamItem.CameraUrl = value;
                    RaisePropertyChanged(() => CamUrl);
                }
            }
        }

        public String Ip
        {
            get { return CamItem.Ip; }
            set
            {
                if (CamItem.Ip != value)
                {
                    CamItem.Ip = value;
                    RaisePropertyChanged(() => Ip);
                }
            }
        }

        public SByte Type
        {
            get { return CamItem.Type; }
            set
            {
                if (CamItem.Type != value)
                {
                    CamItem.Type = value;
                    RaisePropertyChanged(() => Type);
                }
            }
        }
        #endregion
    }
}
