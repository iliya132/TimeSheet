using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeSheetApp.Model.EntitiesBase;
using TimeSheetApp.Model.Interfaces;

namespace TimeSheetApp.Model.Reports
{
    public class Report_ExportToDashboard :IReport
    {
        private Analytic[] analytics;
        private TimeSheetContext context;
        private DashboardContext dashboardContext;

        public Report_ExportToDashboard(Analytic[] analytics, TimeSheetContext context)
        {
            this.analytics = analytics;
            this.context = context;
            dashboardContext = new DashboardContext();
        }

        public void Generate(DateTime start, DateTime end)
        {
            List<DashboardRecord> ExportValues = new List<DashboardRecord>();
            List<Process> _processes = context.ProcessSet.ToList();
            List<TimeSheetTable> TimeSheetTableDB = context.TimeSheetTableSet.Where(i => i.TimeStart >= start &&
                i.TimeStart <= end).
                ToList();
            List<Analytic> analytics = context.AnalyticSet.ToList();

            foreach (Process process in _processes)
            {
                #region Для каждого подразделения считаем значение
                if (process.Id != 62 && process.Id != 63)
                {
                    foreach (DashboardUnit unit in dashboardContext.DashboardUnits)
                    {
                        DashboardRecord newRecord = new DashboardRecord();
                        int timeSpent = 0; //Время потраченное в рамках одного процесса
                        int timeSpentTotal = 0; //Время потраченное на все процессы
                        int daysWorked = 0;


                        foreach (Analytic analytic in context.AnalyticSet.Where(a=>a.DepartmentId == unit.Department_Id &&
                        a.DirectionId == unit.Direction_Id && a.UpravlenieId == unit.Upravlenie_Id &&
                        a.OtdelId == unit.Otdel_Id))
                        {
                            if (TimeSheetTableDB.Any(i => i.AnalyticId == analytic.Id &&
                                    i.TimeStart > start &&
                                    i.TimeStart < end))
                            {
                                timeSpentTotal = TimeSheetTableDB.
                                    Where(i => i.AnalyticId == analytic.Id &&
                                        i.TimeStart > start &&
                                        i.TimeStart < end &&
                                        i.Process_id != 62 &&
                                        i.Process_id != 63).
                                    Sum(i => i.TimeSpent);
                            }

                            if (TimeSheetTableDB.Any(i => i.AnalyticId == analytic.Id &&
                                i.Process_id == process.Id &&
                                i.TimeStart > start &&
                                i.TimeStart < end))
                            {
                                timeSpent = TimeSheetTableDB.
                                    Where(i => i.AnalyticId == analytic.Id &&
                                    i.Process_id == process.Id &&
                                    i.TimeStart > start &&
                                    i.TimeStart < end).
                                    Sum(i => i.TimeSpent);
                                daysWorked = Convert.ToInt32((TimeSheetTableDB.Max(day => day.TimeEnd) - TimeSheetTableDB.Min(day => day.TimeStart)).TotalDays);
                                
                            }
                            
                            
                            newRecord.Process_Id = process.Id;
                            newRecord.Sla_productivity = timeSpent;
                            newRecord.Unit_id = unit.Id;
                            newRecord.periodStart = start;
                            newRecord.periodEnd = end;
                            newRecord.Etalon_productivity = daysWorked * 8 * 60;
                            ExportValues.Add(newRecord);
                            
                        }
                    }
                }
                #endregion
            }
            using (ExcelPackage excel = new ExcelPackage())
            {
                int row = 2;
                int col = 1;
                
                ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("Sheet1");
                sheet.Cells[1, 1].Value = "ID";
                sheet.Cells[1, 2].Value = "periodStart";
                sheet.Cells[1, 3].Value = "periodEnd";
                sheet.Cells[1, 4].Value = "Etalon_productivity";
                sheet.Cells[1, 5].Value = "Sla_productivity";
                sheet.Cells[1, 6].Value = "Unit";
                foreach(DashboardRecord rec in ExportValues)
                {
                    sheet.Cells[row, col++].Value = rec.Id;
                    sheet.Cells[row, col++].Value = rec.periodStart;
                    sheet.Cells[row, col++].Value = rec.periodEnd;
                    sheet.Cells[row, col++].Value = rec.Etalon_productivity;
                    sheet.Cells[row, col++].Value = rec.Sla_productivity;
                    sheet.Cells[row, col++].Value = dashboardContext.DashboardUnits.FirstOrDefault(un => un.Id == rec.Unit_id).ShortName;

                    col = 1;
                    row++;
                }
                excel.SaveAs(new System.IO.FileInfo("testExport.xlsx"));
            }
            System.Diagnostics.Process.Start("testExport.xlsx");
        }

    }
}
/*
            #region Для каждого процесса


            foreach (Process process in _processes)
            {
                
                #region Для каждого подразделения считаем значение
                if (process.Id != 62 && process.Id != 63)
                {
                    foreach (StructureData structure in structuresData)
                    {
                        double operationPercentTotal = 0.0;
int timeSpent = 0; //Время потраченное в рамках одного процесса
int timeSpentTotal = 0; //Время потраченное на все процессы

                        foreach (AnalyticOrdered analytic in structure.analytics)
                        {
                            if (TimeSheetTableDB.Any(i => i.AnalyticId == analytic.Analytic.Id &&
                            i.TimeStart > start &&
                            i.TimeStart<end))
                            {
                                timeSpentTotal = TimeSheetTableDB.
                                    Where(i => i.AnalyticId == analytic.Analytic.Id &&
                                        i.TimeStart > start &&
                                        i.TimeStart<end &&
                                        i.Process_id != 62 &&
                                        i.Process_id != 63).
                                    Sum(i => i.TimeSpent);
                            }

                            if (TimeSheetTableDB.Any(i => i.AnalyticId == analytic.Analytic.Id &&
                            i.Process_id == process.Id &&
                            i.TimeStart > start &&
                            i.TimeStart<end))
                            {
                                timeSpent = TimeSheetTableDB.
                                    Where(i => i.AnalyticId == analytic.Analytic.Id &&
                                    i.Process_id == process.Id &&
                                    i.TimeStart > start &&
                                    i.TimeStart<end).
                                    Sum(i => i.TimeSpent);
                            }
                            else { timeSpent = 0; }
                            if (timeSpentTotal != 0)
                            {
                                operationPercentTotal += timeSpent* 1.00 / timeSpentTotal;

                            }
                        }
                        if (operationPercentTotal > 0)
                            structure.processValues.Add(process.ProcName, operationPercentTotal);
                    }
                }
                #endregion
            }


            #endregion

            #region итог записываем в excel
            using (ExcelPackage excel = new ExcelPackage())
            {
                ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("Report_03");
int row = 2;

sheet.Cells[1, 1].Value = "Процесс";
                sheet.Cells[1, 2].Value = "Код процесса";

                foreach (Process process in _processes)
                {
                    sheet.Cells[row, 1].Value = process.ProcName;
                    sheet.Cells[row++, 2].Value = $"{process.Block_Id}.{process.SubBlock_Id}.{process.Id}";
                }
                for (int i = 0; i<structuresData.Count; i++)
                {
                    sheet.Cells[1, i + 3].Value = structuresData[i].structName;
                }

                for (int i = 2; i <= _processes.Count; i++)
                {
                    string procName = sheet.Cells[i, 1].Text;
                    
                    for (int j = 0; j<structuresData.Count; j++)
                    {

                        if (structuresData[j].processValues.ContainsKey(procName))
                        {
                            sheet.Cells[i, j + 3].Value = structuresData[j].processValues[procName];
                        }
                    }
                }


                string fileName = $"Reports\\Report_03_{Environment.UserName}{DateTime.Now.ToString($"ddMMyyyy_HHmmss")}.xlsx";
FileInfo newExcelFile = new FileInfo(fileName);
                if (!Directory.Exists("Reports\\"))
                {
                    Directory.CreateDirectory("Reports\\");
                }
                excel.SaveAs(newExcelFile);
                excel.Dispose();
                System.Diagnostics.Process.Start(fileName);
            }
            #endregion
            */