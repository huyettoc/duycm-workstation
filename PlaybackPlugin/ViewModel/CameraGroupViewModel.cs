using System;

using System.Collections.ObjectModel;
using PlaybackPlugin.Model;


namespace PlaybackPlugin.ViewModel
{
    public class CameraGroupViewModel : ViewModelBase
    {
        #region Contructor

        public CameraGroupViewModel(CameraGroup cameraGroup)
        {
            CamGroup = cameraGroup;
            CamList = new ObservableCollection<CameraItemViewModel>();
            foreach (var cam in CamGroup.CameraList)
            {
                CamList.Add(new CameraItemViewModel(new CameraItem() { Id = cam.Id, CameraUrl = cam.CameraUrl,Ip = cam.Ip,Type = cam.Type}));
            }
        }

        #endregion

        #region Members
       
        
        #endregion

        #region Properties
        public CameraGroup CamGroup { get; protected set; }

        public String GroupName
        {
            get { return CamGroup.GroupName; }
            set 
            {
                if (CamGroup.GroupName != value)
                {
                    CamGroup.GroupName = value;
                    RaisePropertyChanged("GroupName");
                }
            }
        }

        private ObservableCollection<CameraItemViewModel> _camList;
        public ObservableCollection<CameraItemViewModel> CamList
        {
            get { return _camList; }
            set
            {
                if (_camList != value)
               {
                   _camList = value;
                   RaisePropertyChanged("CamList");
               }
            }
        }

        #endregion
        
    }
}
