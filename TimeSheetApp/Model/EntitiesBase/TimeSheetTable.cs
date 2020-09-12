using System;
using System.Collections.Generic;

namespace TimeSheetApp.Model.EntitiesBase
{
    public class TimeSheetTable
    {
        public int Id { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int TimeSpent { get; set; }
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

        public virtual ClientWays ClientWays { get; set; }
        public virtual Formats Formats { get; set; }
        public virtual Analytic Analytic { get; set; }
        public virtual Process Process { get; set; }

        public override string ToString()
        {
            return Subject;
        }
    }
}
