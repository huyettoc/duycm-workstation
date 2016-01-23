using System;
using System.Threading;
using System.Windows;

namespace BusyIndicatorLib
{
    public class BusyIndicatorHandler
    {
        private Thread _statusThread;

        private BusyWindow _popup = null;

        public void Start()
        {
            //create the thread with its ThreadStart method
            _statusThread = new Thread(() =>
            {
                try
                {
                    this._popup = new BusyWindow();
                    this._popup.Show();
                    this._popup.Closed += (lsender, le) =>
                    {
                        //when the window closes, close the thread invoking the shutdown of the dispatcher
                        this._popup.Dispatcher.InvokeShutdown();
                        this._popup = null;
                        this._statusThread = null;
                    };

                    //this call is needed so the thread remains open until the dispatcher is closed
                    System.Windows.Threading.Dispatcher.Run();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                }
            });

            //run the thread in STA mode to make it work correctly
            this._statusThread.SetApartmentState(ApartmentState.STA);
            this._statusThread.Priority = ThreadPriority.Normal;
            this._statusThread.Start();
        }

        public void Stop()
        {
            if (this._popup != null)
            {
                //need to use the dispatcher to call the Close method, because the window is created in another thread, and this method is called by the main thread
                this._popup.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this._popup.Close();
                }));
            }
        }
    }
}
