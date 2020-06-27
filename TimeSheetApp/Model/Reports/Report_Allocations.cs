using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.EntitiesBase;
using TimeSheetApp.Model.Interfaces;

namespace TimeSheetApp.Model.Reports
{
    
    public class Report_Allocations : IReport
    {
        const int EMPTY_OTDEL = 19;
        const int EMPTY_UPRAVLENIE = 6;
        const int EMPTY_DIRECTION = 19;
        Dictionary<string, List<Analytic>> Units = new Dictionary<string, List<Analytic>>();

        TimeSheetContext _tsContext;

        public Report_Allocations(TimeSheetContext TimeSheetContext)
        {
            _tsContext = TimeSheetContext;
        }
        public void Generate(DateTime start, DateTime end)
        {
            List<TimeSheetTable> records = _tsContext.TimeSheetTableSet.Where(rec => rec.TimeStart > start && rec.TimeEnd < end).ToList();
            
            foreach(Analytic analytic in _tsContext.AnalyticSet)
            {
                if(analytic.OtdelId != EMPTY_OTDEL)
                {
                    string unitName = analytic.Otdel.Name;
                    AddNewUnitIfNotExist(unitName);
                    Units[unitName].Add(analytic);
                    continue;
                }

                if (analytic.UpravlenieId != EMPTY_UPRAVLENIE)
                {
                    string unitName = analytic.Upravlenie.Name;
                    AddNewUnitIfNotExist(unitName);
                    Units[unitName].Add(analytic);
                    continue;
                }

                if (analytic.UpravlenieId != EMPTY_UPRAVLENIE)
                {
                    string unitName = analytic.Upravlenie.Name;
                    AddNewUnitIfNotExist(unitName);
                    Units[unitName].Add(analytic);
                    continue;
                }
            }

        }

        private void AddNewUnitIfNotExist(string unitName)
        {
            if (Units.ContainsKey(unitName))
            {
                Units.Add(unitName, new List<Analytic>());
            }
        }
    }
}
