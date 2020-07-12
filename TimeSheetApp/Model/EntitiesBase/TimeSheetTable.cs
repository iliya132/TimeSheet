using System;
using System.Collections.Generic;
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

        private List<BusinessBlockNew> businessBlockNews = new List<BusinessBlockNew>();
        public List<BusinessBlockNew> BusinessBlocks { get => businessBlockNews; set => businessBlockNews = value; }
        private List<RiskNew> _risks = new List<RiskNew>();
        public List<RiskNew> Risks { get => _risks; set => _risks = value; }
        private List<EscalationNew> _escalations = new List<EscalationNew>();
        public List<EscalationNew> Escalations { get => _escalations; set => _escalations = value; }
        public List<SupportNew> _supports = new List<SupportNew>();
        public List<SupportNew> Supports { get => _supports; set => _supports = value; }

        [ForeignKey("ClientWaysId")]
        public virtual ClientWays ClientWays { get; set; }
        [ForeignKey("FormatsId")]
        public virtual Formats Formats { get; set; }
        [ForeignKey("AnalyticId")]
        public virtual Analytic Analytic { get; set; }
        [ForeignKey("Process_id")]
        public virtual Process Process { get; set; }

        public override string ToString()
        {
            return Subject;
        }
    }
}
