
using System;
using System.Collections.Generic;
using System.Linq;
using TimeSheetApp.Model.EntitiesBase;
using TimeSheetApp.Model.Interfaces;

namespace TimeSheetApp.Model.Reports
{
    public class Report_02 : IReport
    {
        private readonly TimeSheetContext dataBase;
        private List<TimeSheetTable> _timeSheetTable;
        private List<Process> _process;
        private List<Analytic> _analytics;

        public Report_02(TimeSheetContext dataBase, IEnumerable<Analytic> Analytics)
        {
            this.dataBase = dataBase;
            _analytics = Analytics.ToList();
        }

        public void Generate(DateTime start, DateTime end)
        {
            _timeSheetTable = dataBase.TimeSheetTableSet.Where(i =>
            i.TimeStart >= start &&
            i.TimeEnd >= start &&
            i.TimeStart <= end &&
            i.TimeEnd <= end &&
            i.Process_id != 62 &&
            i.Process_id != 63).ToList();

            _process = dataBase.ProcessSet.ToList();


            List<RowData> rowsResult = new List<RowData>();
            foreach (Analytic analytic in _analytics)
            {
                int timeTotal = 0;

                var daysWorked = _timeSheetTable.Where(i => i.Analytic.Id == analytic.Id &&
                    i.TimeStart >= start && i.TimeStart <= end &&
                    i.TimeEnd >= start && i.TimeEnd <= end &&
                    i.Process_id != 62 &&
                    i.Process_id != 63).GroupBy(i => i.TimeStart.Date).Count();

                int ShouldWorkMinutes = daysWorked * 8 * 60;

                foreach (Process process in _process)
                {
                    RowData row = new RowData();
                    row.analyticId = analytic.Id;
                    row.processType = process.ProcessType.ProcessTypeName;
                    row.blockName = process.Block.BlockName;
                    row.subBlock = process.SubBlock.SubblockName;
                    row.codeFull = $"{process.Block_Id}.{process.SubBlock_Id}.{process.Id}";
                    row.processName = process.ProcName;
                    row.result = process.Result1.ResultName;
                    row.direction = analytic.Directions.Name;
                    row.upravlenieName = analytic.Upravlenie.Name;
                    row.otdelName = analytic.Otdel.Name;
                    row.FIO = $"{analytic.LastName} {analytic.FirstName} {analytic.FatherName}";
                    row.daysWorked = daysWorked;
                    int timeSpent = 0;

                    IEnumerable<TimeSheetTable> sheetTables = _timeSheetTable.Where(record => record.Analytic.Id == analytic.Id && record.Process.Id == process.Id);

                    row.operationCount = sheetTables.Count();


                    foreach (TimeSheetTable table in sheetTables)
                    {
                        timeSpent += table.TimeSpent;
                    }
                    if (timeSpent > 0)
                    {
                        timeTotal += timeSpent;

                        row.timeSpent = timeSpent;
                        row.operationPercentOneDay = timeSpent * 1.0 / ShouldWorkMinutes;

                        rowsResult.Add(row);
                    }
                }


                foreach (RowData row in rowsResult.Where(i => i.analyticId == analytic.Id))
                {
                    if (timeTotal != 0)
                        row.operationPercentTotal = row.timeSpent * 1.00 / timeTotal;
                }

            }

            ExcelWorker.ExportToExcel(rowsResult);

        }

        internal class RowData
        {
            internal int analyticId { get; set; }
            internal string processType { get; set; }
            internal string blockName { get; set; }
            internal string subBlock { get; set; }
            internal string codeFull { get; set; }
            internal string processName { get; set; }
            internal string result { get; set; }
            internal string direction { get; set; }
            internal string upravlenieName { get; set; }
            internal string otdelName { get; set; }
            internal string FIO { get; set; }
            internal int timeSpent { get; set; }
            internal int daysWorked { get; set; }
            internal int operationCount { get; set; }
            internal double operationPercentTotal { get; set; }
            internal double operationPercentOneDay { get; set; }


        }
    }
}
