using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("TimeSheetTable")]
    public class TimeSheetTable
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("timeStart")]
        public DateTime TimeStart { get; set; }
        [Column("timeEnd")]
        public DateTime TimeEnd { get; set; }
        public int TimeSpent { get; set; }
        [Column("comment")]
        public string Comment { get; set; }
        public string Subject { get; set; }
        public int ClientWaysId { get; set; }
        public int FormatsId { get; set; }
        public int AnalyticId { get; set; }
        public int Process_id { get; set; }
        [Column("riskChoise_id")]
        public int RiskChoice_id { get; set; }
        public int EscalationChoice_id { get; set; }
        public int BusinessBlockChoice_id { get; set; }
        [Column("supportChoice_id")]
        public int SupportChoice_id { get; set; }

        [ForeignKey("ClientWaysId")]
        public virtual ClientWays ClientWays { get; set; }
        [ForeignKey("FormatsId")]
        public virtual Formats Formats { get; set; }
        [ForeignKey("AnalyticId")]
        public virtual Analytic Analytic { get; set; }
        [ForeignKey("Process_id")]
        public virtual Process Process { get; set; }
        [ForeignKey("RiskChoice_id")]
        public virtual RiskChoice RiskChoice { get; set; }
        [ForeignKey("EscalationChoice_id")]
        public virtual EscalationChoice EscalationChoice { get; set; }
        [ForeignKey("BusinessBlockChoice_id")]
        public virtual BusinessBlockChoice BusinessBlockChoice { get; set; }
        [ForeignKey("SupportChoice_id")]
        public virtual SupportChoice SupportChoice { get; set; }
    }
}
