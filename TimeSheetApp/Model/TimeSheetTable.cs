//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeSheetApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TimeSheetTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TimeSheetTable()
        {
            this.Process_id = 1;
        }
    
        public int id { get; set; }
        public System.DateTime timeStart { get; set; }
        public System.DateTime timeEnd { get; set; }
        public int TimeSpent { get; set; }
        public string comment { get; set; }
        public string Subject { get; set; }
        public int ClientWaysId { get; set; }
        public int EscalationChoiceId { get; set; }
        public int FormatsId { get; set; }
        public int SupportChoiceId { get; set; }
        public int AnalyticId { get; set; }
        public int Process_id { get; set; }
        public int riskChoise_id { get; set; }
        public int EscalationChoice_id { get; set; }
        public int BusinessBlockChoice_id { get; set; }
    
        public virtual ClientWays ClientWays { get; set; }
        public virtual Formats Formats { get; set; }
        public virtual Analytic Analytic { get; set; }
        public virtual Process Process { get; set; }
        public virtual riskChoise riskChoise { get; set; }
        public virtual supportChoice supportChoice { get; set; }
        public virtual EscalationChoice EscalationChoice { get; set; }
        public virtual BusinessBlockChoice BusinessBlockChoice { get; set; }
    }
}
