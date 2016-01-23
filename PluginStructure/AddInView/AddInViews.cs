using System;
using System.Collections.Generic;
using System.AddIn.Pipeline;
using System.Windows;
using System.Windows.Controls;

namespace AddInView
{    
    /// <summary>
    /// Abstract base class that should be inherited by all AddIns
    /// </summary>
    [AddInBase]
    public abstract class NumberProcessorAddInView
    {
        #region Abstract Methods
        public abstract List<int> ProcessNumbers(int fromNumber, int toNumber);

        public abstract void Initialize(HostObject hostObj);
        #endregion
    }

    /// <summary>
    /// Abstract class that should be inherited by an object that needs to communicate
    /// between the host Contract to View adapter <see cref="AddInSideAdapter.HostObjectContractToViewAddInAdapter">
    /// HostObjectContractToViewAddInAdapter</see>
    /// </summary>
    public abstract class HostObject
    {
        #region Abstract Methods
        public abstract void ReportProgress(int progressPercent);
        #endregion
    }

    /// <summary> 
    /// Defines the add-in's view of the contract. 
    /// </summary>
    //[AddInBase]
    //public class WPFAddInView : UserControl { }

    /// <summary> 
    /// Defines the add-in's view of the contract 
    /// </summary>
    [AddInBase]
    public interface IWPFAddInView
    {
        // The add-in's implementation of this method will return 
        // a UI type that directly or indirectly derives from  
        // FrameworkElement.
        FrameworkElement GetAddInUI();
        bool EndMess(bool mess);
    }
}
