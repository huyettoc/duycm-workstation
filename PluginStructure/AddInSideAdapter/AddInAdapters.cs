using System;
using System.AddIn.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.AddIn.Pipeline;
using System.Windows;
using AddInView;
using Contract;

namespace AddInSideAdapter
{

    /// <summary>
    /// Adapter use to talk to AddIn <see cref="Contract.INumberProcessorContract">AddIn Contract</see>
    /// </summary>
    [AddInAdapter]
    public class NumberProcessorViewToContractAdapter : ContractBase, Contract.INumberProcessorContract
    {
        #region Data
        private AddInView.NumberProcessorAddInView view;
        #endregion

        #region Ctor
        public NumberProcessorViewToContractAdapter(AddInView.NumberProcessorAddInView view)
        {
            this.view = view;
        }
        #endregion

        #region Public Methods
        public List<int> ProcessNumbers(int fromNumber, int toNumber)
        {
            return view.ProcessNumbers(fromNumber, toNumber);
        }

        public void Initialize(Contract.IHostObjectContract hostObj)
        {            
            view.Initialize(new HostObjectContractToViewAddInAdapter(hostObj));
        }
        #endregion
    }

    /// <summary>
    /// Allows AddIn adapter to talk back to HostView
    /// </summary>
    public class HostObjectContractToViewAddInAdapter : AddInView.HostObject
    {
        #region Data
        private Contract.IHostObjectContract contract;
        private ContractHandle handle;
        #endregion
        
        #region Ctor
        public HostObjectContractToViewAddInAdapter(Contract.IHostObjectContract contract)
        {
            this.contract = contract;
            this.handle = new ContractHandle(contract);
        }
        #endregion

        #region Public Methods
        public override void ReportProgress(int progressPercent)
        {
            contract.ReportProgress(progressPercent);
        }
        #endregion
    }

    /// <summary> 
    /// Adapts the add-in's view of the contract to the add-in contract 
    /// </summary>
    //[AddInAdapter]
    //public class WPFAddIn_ViewToContractAddInSideAdapter : ContractBase, Contract.IWPFAddInContract
    //{
    //    WPFAddInView wpfAddInView;

    //    public WPFAddIn_ViewToContractAddInSideAdapter(WPFAddInView wpfAddInView)
    //    {
    //        // Adapt the add-in view of the contract (WPFAddInView)  
    //        // to the contract (IWPFAddInContract) 
    //        this.wpfAddInView = wpfAddInView;
    //    }

    //    /// <summary> 
    //    /// ContractBase.QueryContract must be overridden to: 
    //    /// * Safely return a window handle for an add-in UI to the host  
    //    ///   application's application. 
    //    /// * Enable tabbing between host application UI and add-in UI, in the 
    //    ///   "add-in is a UI" scenario.
    //    /// </summary> 
    //    public override IContract QueryContract(string contractIdentifier)
    //    {
    //        if (contractIdentifier.Equals(typeof(INativeHandleContract).AssemblyQualifiedName))
    //        {
    //            return FrameworkElementAdapters.ViewToContractAdapter(this.wpfAddInView);
    //        }

    //        return base.QueryContract(contractIdentifier);
    //    }

    //    /// <summary> 
    //    /// GetHandle is called by the WPF add-in model from the host application's  
    //    /// application domain to to get the window handle for an add-in UI from the  
    //    /// add-in's application domain. GetHandle is called if a window handle isn't  
    //    /// returned by other means ie overriding ContractBase.QueryContract,  
    //    /// as shown above. 
    //    /// NOTE: This method requires UnmanagedCodePermission to be called  
    //    ///       (full-trust by default), to prevent illegal window handle 
    //    ///       access in partially trusted scenarios. If the add-in could 
    //    ///       run in a partially trusted application domain  
    //    ///       (eg AddInSecurityLevel.Internet), you can safely return a window 
    //    ///       handle by overriding ContractBase.QueryContract, as shown above. 
    //    /// </summary>
    //    [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    //    public IntPtr GetHandle()
    //    {
    //        return FrameworkElementAdapters.ViewToContractAdapter(this.wpfAddInView).GetHandle();
    //    }
    //}


    [AddInAdapter]
    public class WPFAddIn_ViewToContractAddInSideAdapter : ContractBase, IWPFAddInContract
    {
        IWPFAddInView wpfAddInView;

        public WPFAddIn_ViewToContractAddInSideAdapter(IWPFAddInView wpfAddInView)
        {
            // Adapt the add-in view of the contract (IWPFAddInView)  
            // to the contract (IWPFAddInContract) 
            this.wpfAddInView = wpfAddInView;
        }

        public INativeHandleContract GetAddInUI()
        {
            // Convert the FrameworkElement from the add-in to an INativeHandleContract  
            // that will be passed across the isolation boundary to the host application.
            FrameworkElement fe = this.wpfAddInView.GetAddInUI();
            INativeHandleContract inhc = FrameworkElementAdapters.ViewToContractAdapter(fe);
            return inhc;
        }

        public bool EndMess(bool mess)
        {
            if(wpfAddInView != null)
                return wpfAddInView.EndMess(mess);
            return false;
        }
    }
    
}

