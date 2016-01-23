using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HostView
{
    /// <summary>
    /// Abstract base class that should be inherited by the Host view
    /// </summary>
    public abstract class NumberProcessorHostView
    {
        #region Abstract Methods
        public abstract List<int> ProcessNumbers(int fromNumber, int toNumber);

        public abstract void Initialize(HostObject host);
        #endregion
    }

    /// <summary>
    /// Abstract base class that should be inherited by a class within the host
    /// application that can make use of the reported progress
    /// </summary>
    public abstract class HostObject
    {
        #region Abstract Methods
        public abstract void ReportProgress(int progressPercent);
        #endregion
    }

    /// <summary> 
    /// Defines the host's view of the add-in 
    /// </summary> 
    public interface IWPFAddInHostView
    {
        // The view returns as a class that directly or indirectly derives from  
        // FrameworkElement and can subsequently be displayed by the host  
        // application by embedding it as content or sub-content of a UI that is  
        // implemented by the host application.
        FrameworkElement GetAddInUI();
        bool EndMess(bool mess);
    }
}
