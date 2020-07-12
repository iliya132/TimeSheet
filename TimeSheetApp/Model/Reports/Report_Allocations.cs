using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.EntitiesBase;
using TimeSheetApp.Model.Interfaces;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;

namespace TimeSheetApp.Model.Reports
{
    
    public class Report_Allocations : IReport
    {
        const int EMPTY_OTDEL = 19;
        const int EMPTY_UPRAVLENIE = 6;
        const int EMPTY_DIRECTION = 19;
        Dictionary<string, List<Analytic>> Units = new Dictionary<string, List<Analytic>>();

        TimeSheetContext _tsContext;
        private readonly Analytic[] analytics;

        public Report_Allocations(TimeSheetContext TimeSheetContext, Analytic[] analytics)
        {
            _tsContext = TimeSheetContext;
            this.analytics = analytics;
        }
        public void Generate(DateTime start, DateTime end)
        {
            
            List<TimeSheetTable> records = _tsContext.TimeSheetTableSet.Where(rec => rec.TimeStart > start && rec.TimeEnd < end).ToList();
            
            foreach(Analytic analytic in analytics)
            {
                if(analytic.OtdelId != EMPTY_OTDEL)
                {
                    string unitName = analytic.Otdel.shortName;
                    AddNewUnitIfNotExist(unitName);
                    Units[unitName].Add(analytic);
                    continue;
                }

                if (analytic.UpravlenieId != EMPTY_UPRAVLENIE)
                {
                    string unitName = analytic.Upravlenie.shortName;
                    AddNewUnitIfNotExist(unitName);
                    Units[unitName].Add(analytic);
                    continue;
                }

                if (analytic.DirectionId != EMPTY_DIRECTION)
                {
                    string unitName = analytic.Directions.shortName;
                    AddNewUnitIfNotExist(unitName);
                    Units[unitName].Add(analytic);
                    continue;
                }
            }

            using (ExcelPackage excel = new ExcelPackage())
            {
                int BusinessBlocksCount = _tsContext.BusinessBlockSet.Count();
                foreach (KeyValuePair<string, List<Analytic>> item in Units)
                {
                    string UnitName = item.Key;
                    ExcelWorksheet currentSheet;
                    if (!excel.Workbook.Worksheets.Any(i => i.Name.Equals(UnitName)))
                        currentSheet = excel.Workbook.Worksheets.Add(UnitName);
                    else
                        currentSheet = excel.Workbook.Worksheets[UnitName];
                    int currentRow = 1;
                    int currentCol = 4;
                    currentSheet.Cells[1, 1].Value = "Сотрудник";
                    currentSheet.Column(1).Width = 42;
                    currentSheet.Cells[1, 2].Value = "Номера процессов";
                    currentSheet.Column(2).Width = 20;
                    currentSheet.Column(2).Style.WrapText = true;
                    currentSheet.Cells[1, 3].Value = "Процессы";
                    currentSheet.Column(3).Width = 50;
                    currentSheet.Column(3).Style.WrapText = true;
                    foreach (BusinessBlock block in _tsContext.BusinessBlockSet)
                    {
                        currentSheet.Cells[currentRow, currentCol++].Value = block.BusinessBlockName;
                    }
                    currentSheet.Cells[currentRow, currentCol].Value = "Не указана БЛ";
                    currentRow++;
                    currentCol = 4;
                    foreach (Analytic analytic in item.Value)
                    {
                        currentSheet.Cells[currentRow, 1].Value = $"{analytic.LastName} {analytic.FirstName} {analytic.FatherName}";
                        int totalTimeSpent = (int?)_tsContext.TimeSheetTableSet.
                            Where(record => record.AnalyticId == analytic.Id && record.TimeStart > start && record.TimeEnd < end).Select(i => (int?)i.TimeSpent).
                            Sum() ?? 0;


                        float currentBlockTimeSpent;
                        List<Process> currentAnalyticUsedProcesses = _tsContext.TimeSheetTableSet.Include("Process").Where(record => record.TimeStart > start &&
                        record.TimeEnd < end && record.AnalyticId == analytic.Id).Select(record => record.Process).Distinct().ToList();
                        foreach (Process process in currentAnalyticUsedProcesses)
                        {
                            int totalProcessTimeSpent = (int?)_tsContext.TimeSheetTableSet.
                            Where(record => record.AnalyticId == analytic.Id && record.TimeStart > start && record.TimeEnd < end && record.Process_id == process.Id).Select(i => (int?)i.TimeSpent).
                            Sum() ?? 0;

                            currentSheet.Cells[currentRow, 2].Value = process.Id;
                            currentSheet.Cells[currentRow, 3].Value = process.ProcName;

                            foreach (BusinessBlock businessBlock in _tsContext.BusinessBlockSet)
                            {
                                currentBlockTimeSpent = (int?)_tsContext.TimeSheetTableSet.Include("BusinessBlocks").
                                    Where(record => record.TimeStart >= start && record.TimeEnd <= end && 
                                    record.AnalyticId == analytic.Id && record.BusinessBlocks.Any(o => o.BusinessBlockId == businessBlock.Id) &&
                                    record.Process_id == process.Id).
                                    Select(i=>i.BusinessBlocks.Count > 0 ? (float?)i.TimeSpent / i.BusinessBlocks.Count : (float?)i.TimeSpent).
                                    Sum() ?? 0;
                                float currentAllocationPercentage;
                                if (totalTimeSpent == 0)
                                {
                                    currentAllocationPercentage = 0;
                                }
                                else
                                {
                                    currentAllocationPercentage = currentBlockTimeSpent / totalTimeSpent;
                                }
                                currentSheet.Column(currentCol).Width = 15;
                                currentSheet.Cells[currentRow, currentCol].Style.Numberformat.Format = "0%";
                                currentSheet.Cells[currentRow, currentCol++].Value = currentAllocationPercentage;
                            }
                        
                            currentBlockTimeSpent = (int?)_tsContext.TimeSheetTableSet.Include("BusinessBlocks").
                                    Where(record => record.TimeStart >= start && record.TimeEnd <= end &&
                                    record.AnalyticId == analytic.Id && record.BusinessBlocks.Count == 0 &&
                                    record.Process_id == process.Id).
                                    Select(i => (float?)i.TimeSpent).
                                    Sum() ?? 0;
                            float currentallocation;
                            if (totalTimeSpent != 0)
                            {
                                currentallocation = currentBlockTimeSpent / totalTimeSpent;
                            }
                            else
                            {
                                currentallocation = 0;
                            }

                            currentSheet.Column(currentCol).Width = 15;
                            currentSheet.Cells[currentRow, currentCol].Style.Numberformat.Format = "0%";

                            currentSheet.Cells[currentRow, currentCol++].Value = currentallocation;
                            currentSheet.Cells[currentRow, currentCol++].Value = totalProcessTimeSpent;

                            currentRow++;
                            currentCol = 4;
                        }
                    }
                    
                }
                string newFileName = $"ReportAllocations{DateTime.Now.ToString("ddMMyyyymmss")}.xlsx";
                excel.SaveAs(new FileInfo(newFileName));;
                System.Diagnostics.Process.Start(newFileName);
            }
        }

        private void AddNewUnitIfNotExist(string unitName)
        {
            if (!Units.ContainsKey(unitName))
            {
                Units.Add(unitName, new List<Analytic>());
            }
        }
    }
}
