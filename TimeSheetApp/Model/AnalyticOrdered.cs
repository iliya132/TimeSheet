using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    public class AnalyticOrdered
    {
        Analytic analytic;
        public string FIO { get; set; }
        public string userName { get; set; }
        
        public string FirstStructure { get; set; }
        public string SecondStructure { get; set; }
        public string ThirdStructure { get; set; }
        public string FourStructure { get; set; }
        public AnalyticOrdered(Analytic _analytic)
        {
            analytic = _analytic;

            FIO = $"{analytic.LastName} {analytic.FirstName} {analytic.FatherName}";
            userName = analytic.UserName;

            FirstStructure = analytic.Departments.Name;

            SecondStructure = analytic.DirectionId != 4 ? analytic.Directions.Name :
                analytic.UpravlenieId != 4 ? analytic.Upravlenie.Name : analytic.Otdel.Name;
            if (analytic.DirectionId == 0 && analytic.UpravlenieId == 4) return;

            ThirdStructure = analytic.UpravlenieId != 4 ? analytic.Upravlenie.Name : analytic.Otdel.Name;
            if (analytic.UpravlenieId == 4) return;

            FourStructure = analytic.Otdel.Name;

        }
    }
}
