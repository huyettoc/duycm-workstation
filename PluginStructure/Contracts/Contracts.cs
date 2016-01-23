using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.AddIn.Pipeline;
using System.AddIn.Contract;

namespace Contract
{
    /// <summary>
    /// The actual AddIn contract that is implemented by the
    /// <see cref="AddInSideAdapter.NumberProcessorViewToContractAdapter">AddIn Adapter</see>
    /// </summary>
    [AddInContract]
    public interface INumberProcessorContract : IContract
    {
        #region Methods
        List<int> ProcessNumbers(int fromNumber, int toNumber);
        void Initialize(IHostObjectContract hostObj);
        #endregion
    }

    /// <summary>
    /// The actual Host contract that is implemented by the
    /// <see cref="HostInSideAdapter.HostObjectViewToContractHostAdapter">Host Adapter</see>
    /// Which enabled the AddIn to talk back to the host
    /// </summary>
    public interface IHostObjectContract : IContract
    {
        #region Methods
        void ReportProgress(int progressPercent);
        #endregion
    }

    /// <summary> 
    /// Defines the services that an add-in will provide to a host application. 
    /// In this case, the add-in is a UI. 
    /// </summary>
    //[AddInContract]
    //public interface IWPFAddInContract : INativeHandleContract { }

    /// <summary> 
    /// Defines the services that an add-in will provide to a host application 
    /// </summary>
    [AddInContract]
    public interface IWPFAddInContract : IContract
    {
        // Return a UI to the host application
        INativeHandleContract GetAddInUI();

        bool EndMess(bool mess);
    }
}
