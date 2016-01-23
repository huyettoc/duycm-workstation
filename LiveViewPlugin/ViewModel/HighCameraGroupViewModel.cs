using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveViewPlugin.View;

namespace LiveViewPlugin.ViewModel
{
    class HighCameraGroupViewModel : ViewModelBase
    {
        #region Mems
        private ObservableCollection<CameraGroupViewModel> _camgroupCollection;
        private ObservableCollection<ScreenTypeViewModel> _screenTypeCollection;
        #endregion

        #region Properties
        public ObservableCollection<CameraGroupViewModel> CamGroupCollection
        {
            get { return _camgroupCollection; }
            set
            {
                if (_camgroupCollection != value)
                {
                    _camgroupCollection = value;
                    RaisePropertyChanged(() => CamGroupCollection);
                }
            }
        }


        public ObservableCollection<ScreenTypeViewModel> ScreenTypeCollection
        {
            get { return _screenTypeCollection; }
            set
            {
                if (_screenTypeCollection != value)
                {
                    _screenTypeCollection = value;
                    RaisePropertyChanged(() => ScreenTypeCollection);
                }
            }
        }
        
        #endregion

        #region Contructor
        public HighCameraGroupViewModel()
        {
            CamGroupCollection = new ObservableCollection<CameraGroupViewModel>();
            ScreenTypeCollection = new ObservableCollection<ScreenTypeViewModel>();
            //List<Department> departmentList = GetDepartmentList();
            //foreach (Department department in departmentList)
            //{
            //    DepartmentCollection.Add(new DepartmentViewModel(department));
            //}
        }

        public HighCameraGroupViewModel(ObservableCollection<CameraGroupViewModel> highCamlist)
        {
            CamGroupCollection = highCamlist;
        }

        public void SetCameraGroupViewModel(ObservableCollection<CameraGroupViewModel> highCamlist)
        {
            CamGroupCollection = highCamlist;
        }

        public void SetSetScreenTypeViewModel(ObservableCollection<ScreenTypeViewModel> screenTypelist)
        {
            ScreenTypeCollection = screenTypelist;
        }
        #endregion

    }
}
