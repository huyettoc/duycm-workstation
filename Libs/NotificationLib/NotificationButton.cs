using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace NotificationLib
{
    public class NotificationButton : Button
    {
        #region Private Members
        private readonly TaskScheduler _uiContext;
        private NotificationAdorder _adorner;
        private Timer _timer;
        #endregion

        #region Static Constructor
        /// <summary>
        /// Static metadate publish constructor
        /// </summary>
        static NotificationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationButton), new FrameworkPropertyMetadata(typeof(NotificationButton)));
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public NotificationButton()
        {
            _uiContext = TaskScheduler.FromCurrentSynchronizationContext();
            Loaded += NotificationButtonLoaded;
        }

        void NotificationButtonLoaded(object sender, RoutedEventArgs e)
        {
            var adornerLayler = AdornerLayer.GetAdornerLayer(this);
            _adorner = new NotificationAdorder(this) {DataContext = null};
            if(_adorner==null)
                return;
            adornerLayler.Add(_adorner);

            CheckContent();

            //_timer = new Timer(CheckContent, null, Timeout.Infinite, Timeout.Infinite);
            //if (Poll) _timer.Change(PollInterval * 1000, PollInterval * 1000);
        }
        #endregion

        /// <summary>
        /// Delegates the fake Web Service call onto a different thread, while retaining the UI Thread to update the UI
        /// </summary>
        /// <param name="state"></param>
        private void CheckContent(object state = null)
        {
            int? delegateReturn = 0;
            Func<int> func = null;

            Task.Factory.StartNew(() =>
                                      {
                                          func = NotifyFunc;
                                      }
                , new CancellationToken(), TaskCreationOptions.None, _uiContext)
                .ContinueWith(t =>
                                  {
                                      delegateReturn = func();
                                      if (delegateReturn < 1) delegateReturn = null; // Normalize to not show if not at least 1
                                  }
                )
                .ContinueWith(t =>
                                {
                                    _adorner.DataContext = delegateReturn;
                                }
                , new CancellationToken(), TaskContinuationOptions.None, _uiContext);
        }

        public void UpdateContent()
        {
            CheckContent();
        }

        #region Dependency Properties
        #region PollInterval
        public static readonly DependencyProperty PollIntervalProperty = DependencyProperty.Register(
            "PollInterval",
            typeof(Int32),
            typeof(NotificationButton),
            new UIPropertyMetadata(60) // default value for this DP
            );

        public int PollInterval
        {
            get { return (int)GetValue(PollIntervalProperty); }
            set { SetValue(PollIntervalProperty, value); }
        }
        #endregion

        #region Poll
        public static readonly DependencyProperty PollingProperty = DependencyProperty.Register(
            "Poll",
            typeof(Boolean),
            typeof(NotificationButton),
            new UIPropertyMetadata(true, OnPollChanged) // default value for this DP
            );

        public bool Poll
        {
            get { return (bool)GetValue(PollingProperty); }
            set { SetValue(PollingProperty, value); }
        }

        public void StartPolling()
        {
            //_timer.Change(PollInterval * 1000, PollInterval * 1000);
        }

        public void StopPolling()
        {
            //_timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private static void OnPollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var nb = (NotificationButton)d;

            //if ((bool)e.NewValue) nb.StartPolling();
            //else nb.StopPolling();
        }
        #endregion

        #region NotificationFunc
        public static readonly DependencyProperty NotifyFuncProperty = DependencyProperty.Register(
            "NotifyFunc",
            typeof(Func<int>),
            typeof(NotificationButton)
            );

        public Func<int> NotifyFunc
        {
            get { return (Func<int>)GetValue(NotifyFuncProperty); }
            set { SetValue(NotifyFuncProperty, value); }
        }
        #endregion
        #endregion
    }

    #region Adorner
    /// <summary>
    /// The Notification adorner.
    /// On Render implements direct drawing context calls.
    /// </summary>
    public class NotificationAdorder : Adorner
    {
        public NotificationAdorder(UIElement adornedElement)
            : base(adornedElement)
        {
            DataContextChanged += NotificationAdorderDataContextChanged;
        }

        private void NotificationAdorderDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (DataContext == null) return; // Break if we don't have the Adorner databound

            var adornedElementRect = new Rect(AdornedElement.DesiredSize);
            var typeFace = new Typeface(new FontFamily("Courier New"), FontStyles.Normal, FontWeights.ExtraBold,
                                        FontStretches.Condensed);

            var text = new FormattedText(
                DataContext.ToString(),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.RightToLeft,
                typeFace,
                12, Brushes.Black
                );

            var pointOfOrigin = new Point
                                    {
                                        X = adornedElementRect.BottomRight.X,
                                        Y = adornedElementRect.BottomRight.Y - text.Height * 0.7
                                    };

            var elipseCenter = new Point
                                   {
                                       X = pointOfOrigin.X - text.Width / 2,
                                       Y = pointOfOrigin.Y + text.Height / 2
                                   };

            var elipseBrush = new SolidColorBrush
                                  {
                                      Color = Colors.Goldenrod,
                                      Opacity = 0.8
                                  };

            drawingContext.DrawEllipse(
                elipseBrush,
                new Pen(Brushes.Transparent, 2),
                elipseCenter,
                text.Width * 0.9,
                text.Height * 0.5
                );

            drawingContext.DrawText(text, pointOfOrigin);
        }
    }
    #endregion
}