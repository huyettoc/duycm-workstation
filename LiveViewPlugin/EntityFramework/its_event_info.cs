//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LiveViewPlugin.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class its_event_info
    {
        public int id { get; set; }
        public string lane { get; set; }
        public string speed { get; set; }
        public string occupancy { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public sbyte classes { get; set; }
        public string height { get; set; }
        public Nullable<sbyte> class_vehicle { get; set; }
        public Nullable<sbyte> method { get; set; }
        public string density { get; set; }
        public string v85 { get; set; }
        public Nullable<sbyte> class_traffic_flow { get; set; }
        public string status { get; set; }
        public string period { get; set; }
        public string anpr { get; set; }
        public string anpr_confidence { get; set; }
        public string traffic_class_confidence { get; set; }
    }
}
