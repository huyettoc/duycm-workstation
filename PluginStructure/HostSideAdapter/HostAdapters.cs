using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.AddIn.Pipeline;
using System.Windows;
using Contract;
using HostView;

namespace HostSideAdapter
{

    /// <summary>
    /// Adapter use to talk to <see cref="HostView.NumberProcessorHostView">Host View</see>
    /// </summary>
    [HostAdapter]
    public class NumberProcessorContractToViewHostAdapter : HostView.NumberProcessorHostView
    {
        #region Data
        private Contract.INumberProcessorContract contract;
        private ContractHandle contractHandle;
        #endregion

        #region Ctor
        public NumberProcessorContractToViewHostAdapter(Contract.INumberProcessorContract contract)
        {            
            this.contract = contract;
            contractHandle = new ContractHandle(contract);
        }
        #endregion

        #region Public Methods
        public override List<int> ProcessNumbers(int fromNumber, int toNumber)
        {
            return contract.ProcessNumbers(fromNumber, toNumber);
        }

        public override void Initialize(HostView.HostObject host)
        {            
            HostObjectViewToContractHostAdapter hostAdapter = new HostObjectViewToContractHostAdapter(host);
            contract.Initialize(hostAdapter);
        }
        #endregion
    }


    /// <summary>
    /// Allows Host side adapter to talk back to HostView
    /// </summary>
    public class HostObjectViewToContractHostAdapter : ContractBase, Contract.IHostObjectContract
    {
        #region Data
        private HostView.HostObject view;
        #endregion

        #region Public Methods
        public HostObjectViewToContractHostAdapter(HostView.HostObject view)
        {
            this.view = view;
        }

        public void ReportProgress(int progressPercent)
        {
            view.ReportProgress(progressPercent);
        }
        #endregion
    }

    /// <summary> 
    /// Adapts the add-in contract to the host's view of the add-in 
    /// </summary>
    //[HostAdapter]
    //public class WPFAddIn_ContractToViewHostSideAdapter : WPFAddInHostView
    //{
    //    IWPFAddInContract wpfAddInContract;
    //    ContractHandle wpfAddInContractHandle;

    //    public WPFAddIn_ContractToViewHostSideAdapter(IWPFAddInContract wpfAddInContract)
    //    {
    //        // Adapt the contract (IWPFAddInContract) to the host application's 
    //        // view of the contract (WPFAddInHostView) 
    //        this.wpfAddInContract = wpfAddInContract;

    //        // Prevent the reference to the contract from being released while the 
    //        // host application uses the add-in 
    //        this.wpfAddInContractHandle = new ContractHandle(wpfAddInContract);

    //        // Convert the INativeHandleContract for the add-in UI that was passed  
    //        // from the add-in side of the isolation boundary to a FrameworkElement 
    //        string aqn = typeof(INativeHandleContract).AssemblyQualifiedName;
    //        INativeHandleContract inhc = (INativeHandleContract)wpfAddInContract.QueryContract(aqn);
    //        FrameworkElement fe = (FrameworkElement)FrameworkElementAdapters.ContractToViewAdapter(inhc);

    //        // Add FrameworkElement (which displays the UI provided by the add-in) as 
    //        // content of the view (a UserControl) 
    //        this.Content = fe;
    //    }
    //}


    /// <summary> 
    /// Adapts the add-in contract to the host's view of the add-in 
    /// </summary>
    [HostAdapter]
    public class WPFAddIn_ContractToViewHostSideAdapter : IWPFAddInHostView
    {
        IWPFAddInContract wpfAddInContract;
        ContractHandle wpfAddInContractHandle;

        public WPFAddIn_ContractToViewHostSideAdapter(IWPFAddInContract wpfAddInContract)
        {
            // Adapt the contract (IWPFAddInContract) to the host application's 
            // view of the contract (IWPFAddInHostView) 
            this.wpfAddInContract = wpfAddInContract;

            // Prevent the reference to the contract from being released while the 
            // host application uses the add-in 
            this.wpfAddInContractHandle = new ContractHandle(wpfAddInContract);
        }

        public FrameworkElement GetAddInUI()
        {
            // Convert the INativeHandleContract that was passed from the add-in side 
            // of the isolation boundary to a FrameworkElement
            INativeHandleContract inhc = this.wpfAddInContract.GetAddInUI();
            FrameworkElement fe = FrameworkElementAdapters.ContractToViewAdapter(inhc);
            return fe;
        }

        public bool EndMess(bool mess)
        {
            if(wpfAddInContract != null)
                return wpfAddInContract.EndMess(mess);
            return false;
        }
    }
}
