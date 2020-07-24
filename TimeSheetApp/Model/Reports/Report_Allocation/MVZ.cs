using System.Collections.Generic;

using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model.Report.Report_Allocation
{
    public class MVZ
    {
        public string Name { get; set; }
        public string UnitName { get; set; }
        public List<Analytic> Analytics { get; set; }
        public Dictionary<int, int> AllocationRules { get; set; }
    }
}
