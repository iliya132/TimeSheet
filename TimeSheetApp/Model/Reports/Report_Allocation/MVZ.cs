using System.Collections.Generic;

using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model.Report.Report_Allocation
{
    public class MVZ
    {
        public enum AllocationRule :int
        {
            RETAIL_BUSINESS =1,
            A_CLUB = 2,
            MASS_BUSINESS = 3,
            MEDIUM_BUSINESS = 4,
            BIG_BUSINESS = 5,
            TREASURY = 6,
            INVESTMENT = 8
        }
        public string Name { get; set; }
        public string UnitName { get; set; }
        public List<Analytic> Analytics { get; set; }
        public Dictionary<AllocationRule, int> AllocationRules { get; set; }
    }
}
