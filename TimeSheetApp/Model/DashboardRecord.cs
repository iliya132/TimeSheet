using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheetApp.Model
{
    [Table("DashboardFromTimeSheet")]
    public class DashboardRecord
    {
        public int Id { get; set; }
        public int Process_Id { get; set; }
        public int Unit_id { get; set; }
        public double Etalon_productivity { get; set; }
        public int Sla_productivity { get; set; }
        public DateTime periodStart { get; set; }
        public DateTime periodEnd { get; set; }
    }
}
