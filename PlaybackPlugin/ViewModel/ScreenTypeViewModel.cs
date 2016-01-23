using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaybackPlugin.ViewModel;

namespace PlaybackPlugin.ViewModel
{
    public class ScreenTypeViewModel:ViewModelBase
    {
        #region Properties

        //private ObservableCollection<ScreenType> _types;

        private ScreenType _screenType;
        public ScreenType ScreenType
        {
            get { return _screenType; }
            set
            {
                if (_screenType != value)
                {
                    _screenType = value;
                    RaisePropertyChanged(() => _screenType);
                }
            }
        }

        public String Type
        {
            get { return ScreenType.Type; }
            set
            {
                if (ScreenType.Type != value)
                {
                    ScreenType.Type = value;
                    RaisePropertyChanged("Type");
                }
            }
        }

        #endregion

        #region Contructors
        public ScreenTypeViewModel()
        {
            //_types = new ObservableCollection<ScreenType>();
            //LoadScreenTypeViewMode();
            _screenType = new ScreenType();
        }

        //public void LoadScreenTypeViewMode()
        //{
        //    _types.Add(new ScreenType{Type = "2x2"});
        //    _types.Add(new ScreenType { Type = "3x3" });
        //    _types.Add(new ScreenType { Type = "4x4" });
        //    _types.Add(new ScreenType { Type = "5x5" });
        //}

        #endregion
        
    }

    public class ScreenType
    {
        #region Propertie
        public string Type { get; set; }
        #endregion
    }
}
