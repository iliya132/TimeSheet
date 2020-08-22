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
using TimeSheetApp.Model.Report.Report_Allocation;

namespace TimeSheetApp.Model.Reports
{
    
    public class Report_Allocations : IReport
    {
        List<MVZ> allUnits = new List<MVZ>();
        const int EMPTY_OTDEL = 19;
        const int EMPTY_UPRAVLENIE = 6;
        const int EMPTY_DIRECTION = 19;

        TimeSheetContext _tsContext;
        private readonly Analytic[] analytics;

        public Report_Allocations(TimeSheetContext TimeSheetContext, Analytic[] analytics)
        {
            _tsContext = TimeSheetContext;
            this.analytics = analytics;
        }
        public void Generate(DateTime start, DateTime end)
        {
            List<TimeSheetTable> records = _tsContext.TimeSheetTableSet.Include("BusinessBlocks").Where(rec => rec.TimeStart >= start && rec.TimeEnd <= end && rec.Process_id != 62 && rec.Process_id != 63).ToList();
            List<BusinessBlock> blocks = _tsContext.BusinessBlockSet.ToList();
            List<Process> processes = _tsContext.ProcessSet.ToList();
            allUnits = CreateMVZList();
            using (ExcelPackage excel = new ExcelPackage())
            {
                ExcelWorksheet sheetWithTime = excel.Workbook.Worksheets.Add("Аллокации ДК (min)");
                ExcelWorksheet sheetWithPercentage = excel.Workbook.Worksheets.Add("Аллокации ДК (%)");
                ExcelWorksheet sheetWithAnalytics = excel.Workbook.Worksheets.Add("По сотрудникам (min)");
                ExcelWorksheet sheetWithAnalyticsPercentage = excel.Workbook.Worksheets.Add("По сотрудникам (%)");
                int currentRow = 2;
                int currentSheetWithTimeCol = 6;
                int SheetPercentageRow = 2;
                int SheetPercentageCol = 6;

                #region PlaceHeaders
                sheetWithTime.Cells[1, 1].Value = "МВЗ";
                sheetWithTime.Cells[1, 2].Value = "Наименование";
                sheetWithTime.Cells[1, 3].Value = "Номер процесса";
                sheetWithTime.Cells[1, 4].Value = "Процессы";
                sheetWithTime.Cells[1, 5].Value = "FTE";
                sheetWithPercentage.Cells[1, 1].Value = "МВЗ";
                sheetWithPercentage.Cells[1, 2].Value = "Наименование";
                sheetWithPercentage.Cells[1, 3].Value = "Номер процесса";
                sheetWithPercentage.Cells[1, 4].Value = "Процессы";
                sheetWithPercentage.Cells[1, 5].Value = "FTE";
                sheetWithAnalytics.Cells[1, 1].Value = "МВЗ";
                sheetWithAnalytics.Cells[1, 2].Value = "Наименование";
                sheetWithAnalytics.Cells[1, 3].Value = "ФИО";
                sheetWithAnalytics.Cells[1, 4].Value = "Процессы";
                sheetWithAnalyticsPercentage.Cells[1, 1].Value = "МВЗ";
                sheetWithAnalyticsPercentage.Cells[1, 2].Value = "Наименование";
                sheetWithAnalyticsPercentage.Cells[1, 3].Value = "ФИО";
                sheetWithAnalyticsPercentage.Cells[1, 4].Value = "Процессы";
                sheetWithTime.Column(1).Width = 5;
                sheetWithTime.Column(2).Width = 60;
                sheetWithTime.Column(3).Width = 9;
                sheetWithTime.Column(4).Width = 70;
                sheetWithTime.Column(5).Width = 9;
                sheetWithAnalytics.Column(1).Width = 5;
                sheetWithAnalytics.Column(2).Width = 60;
                sheetWithAnalyticsPercentage.Column(1).Width = 5;
                sheetWithAnalyticsPercentage.Column(2).Width = 60;
                sheetWithPercentage.Column(1).Width = 5;
                sheetWithPercentage.Column(2).Width = 60;
                sheetWithPercentage.Column(3).Width = 9;
                sheetWithPercentage.Column(4).Width = 70;
                sheetWithTime.Column(2).Style.WrapText = true;
                sheetWithTime.Column(4).Style.WrapText = true;
                sheetWithPercentage.Column(2).Style.WrapText = true;
                sheetWithPercentage.Column(4).Style.WrapText = true;
                foreach (BusinessBlock block in blocks)
                {
                    sheetWithTime.Cells[1, currentSheetWithTimeCol].Value = block.BusinessBlockName;
                    sheetWithPercentage.Cells[1, currentSheetWithTimeCol++].Value = block.BusinessBlockName;
                }
                sheetWithTime.Cells[1, currentSheetWithTimeCol++].Value = "Total на процесс";
                #endregion

                int BusinessBlocksCount = blocks.Count();
                int row = 2;
                int col = 5;
                foreach (MVZ currentUnit in allUnits)
                {
                    if (currentUnit.Analytics.Count < 1)
                        continue;

                    System.Windows.Forms.Application.DoEvents();

                    #region Analytics
                    
                    foreach (Analytic analytic in currentUnit.Analytics)
                    {
                        List<string> usedProcessesList = records.Where(rec => rec.AnalyticId == analytic.Id).Select(rec => rec.Process).Distinct().Select(i=>i.Id.ToString()).ToList();
                        bool isContainOrganisationProcess = false;
                        for(int i = usedProcessesList.Count-1; i>-1 ; i--)
                        {
                            if (int.Parse(usedProcessesList[i]) <= 12)
                            {
                                isContainOrganisationProcess = true;
                                usedProcessesList.RemoveAt(i);
                            }
                        }
                        if (isContainOrganisationProcess)
                            usedProcessesList.Add("12");
                        string usedProcesses = string.Join(";", usedProcessesList);
                        sheetWithAnalytics.Cells[row, 1].Value = currentUnit.Name;
                        sheetWithAnalytics.Cells[row, 2].Value = currentUnit.UnitName;
                        sheetWithAnalytics.Cells[row, 3].Value = $"{analytic.LastName} {analytic.FirstName} {analytic.FatherName}";
                        sheetWithAnalytics.Cells[row, 4].Value = usedProcesses;
                        sheetWithAnalyticsPercentage.Cells[row, 1].Value = currentUnit.Name;
                        sheetWithAnalyticsPercentage.Cells[row, 2].Value = currentUnit.UnitName;
                        sheetWithAnalyticsPercentage.Cells[row, 3].Value = $"{analytic.LastName} {analytic.FirstName} {analytic.FatherName}";
                        sheetWithAnalyticsPercentage.Cells[row, 4].Value = usedProcesses;
                        int TotalTimeSpent = (int?)records.Where(rec => rec.AnalyticId == analytic.Id).Select(rec => rec.TimeSpent).Sum() ?? 0;
                        int allocatedToCount = currentUnit.AllocationRules.Values.Sum();
                        float BusinessBlockNotSet = (int?)records.
                                Where(record => record.AnalyticId == analytic.Id && record.BusinessBlocks.Count == 0).
                                Select(i => (float?)i.TimeSpent).
                                Sum() ?? 0;
                        foreach (BusinessBlock block in blocks)
                        {
                            sheetWithAnalytics.Cells[1, col].Value = block.BusinessBlockName;
                            sheetWithAnalyticsPercentage.Cells[1, col].Value = block.BusinessBlockName;
                            bool isAllocatedToCurrentBlock = (currentUnit.AllocationRules[(MVZ.AllocationRule)block.Id] == 1);
                            float currentBlockTimeSpent = (int?)records.
                                    Where(record => record.AnalyticId == analytic.Id && record.BusinessBlocks.Any(o => o.BusinessBlockId == block.Id)).
                                    Select(i => (float?)i.TimeSpent / i.BusinessBlocks.Count).
                                    Sum() ?? 0;
                            float currentAllocationPercentage;
                            if (isAllocatedToCurrentBlock)
                            {
                                currentBlockTimeSpent += BusinessBlockNotSet / allocatedToCount;
                            }
                            if (TotalTimeSpent == 0)
                            {
                                currentAllocationPercentage = 0;
                            }
                            else
                            {
                                currentAllocationPercentage = currentBlockTimeSpent / TotalTimeSpent;
                            }
                            sheetWithAnalytics.Column(col).Width = 15;
                            sheetWithAnalytics.Cells[row, col].Style.Numberformat.Format = "0.00";
                            sheetWithAnalytics.Cells[row, col].Value = currentBlockTimeSpent;
                            sheetWithAnalyticsPercentage.Cells[row, col].Style.Numberformat.Format = "0.00%";
                            sheetWithAnalyticsPercentage.Column(col).Width = 15;
                            sheetWithAnalyticsPercentage.Cells[row, col++].Value = currentAllocationPercentage;
                        }
                        row++;
                        col = 5;
                    }
                    #endregion

                    Console.WriteLine(currentUnit.UnitName);
                    string UnitName = currentUnit.UnitName;
                    float currentUnitFTETotal = 0;
                    Dictionary<string, float> currentUnitAllocationTotals = new Dictionary<string, float>();
                    
                    float totalTimeSpentForUnit = (float?)records.
                        Where(x => currentUnit.Analytics.Select(i => i.Id).Contains(x.AnalyticId)).
                        Select(i=>(float?)i.TimeSpent).
                        Sum() ?? 0;

                    int AnalyticsCount = records.
                            Where(record => currentUnit.Analytics.Select(i => i.Id).Contains(record.AnalyticId)).Select(record => record.AnalyticId).Distinct().Count();

                    int AllocatedBusinessBlockCount = currentUnit.AllocationRules.Values.Sum();

                    #region Общее управление
                    currentSheetWithTimeCol = 6;
                    SheetPercentageCol = 6;
                    float ManagerFunctionTimeSpent = (int?)records.
                            Where(record => currentUnit.Analytics.Select(i => i.Id).Contains(record.AnalyticId) &&
                                record.Process.Block_Id == 0).
                            Select(i => (int?)i.TimeSpent).
                            Sum() ?? 0;
                    float ManagerFunctionFTE = ManagerFunctionTimeSpent / totalTimeSpentForUnit * AnalyticsCount;
                    currentUnitFTETotal += ManagerFunctionFTE;
                    sheetWithTime.Cells[currentRow, 1].Value = currentUnit.Name;
                    sheetWithTime.Cells[currentRow, 2].Value = UnitName;
                    sheetWithTime.Cells[currentRow, 3].Value = "0.*";
                    sheetWithTime.Cells[currentRow, 4].Value = "Общее управление";
                    sheetWithPercentage.Cells[SheetPercentageRow, 1].Value = currentUnit.Name;
                    sheetWithPercentage.Cells[SheetPercentageRow, 2].Value = UnitName;
                    sheetWithPercentage.Cells[SheetPercentageRow, 3].Value = "0.*";
                    sheetWithPercentage.Cells[SheetPercentageRow, 4].Value = "Общее управление";
                    sheetWithTime.Cells[currentRow, 5].Value = ManagerFunctionFTE;
                    sheetWithTime.Cells[currentRow, 5].Style.Numberformat.Format = "0.00";
                    sheetWithPercentage.Cells[SheetPercentageRow, 5].Value = ManagerFunctionFTE;
                    sheetWithPercentage.Cells[SheetPercentageRow, 5].Style.Numberformat.Format = "0.00";
                    float ManagerBusinessBlockNotSet = (int?)records.
                        Where(record => currentUnit.Analytics.Select(i => i.Id).Contains(record.AnalyticId) && record.BusinessBlocks.Count == 0 &&
                            record.Process.Block_Id == 0).
                        Select(i => (float?)i.TimeSpent).
                        Sum() ?? 0;
                    foreach (BusinessBlock block in blocks)
                    {
                        bool isAllocatedToCurrentBlock = (currentUnit.AllocationRules[(MVZ.AllocationRule)block.Id] == 1);

                        float currentBusinessTimeSpent = (float?)records.
                                    Where(record => currentUnit.Analytics.Select(i => i.Id).Contains(record.AnalyticId) &&
                                        record.BusinessBlocks.Any(o => o.BusinessBlockId == block.Id) &&
                                        record.Process.Block_Id == 0).
                                    Select(i => i.BusinessBlocks.Count > 0 ? (float?)i.TimeSpent / i.BusinessBlocks.Count : (float?)i.TimeSpent).
                                    Sum() ?? 0;

                        if (isAllocatedToCurrentBlock)
                        {
                            currentBusinessTimeSpent += ManagerBusinessBlockNotSet / AllocatedBusinessBlockCount;
                        }

                        float currentBusinessTimePercentage = currentBusinessTimeSpent / ManagerFunctionTimeSpent;
                        if (!currentUnitAllocationTotals.ContainsKey(block.BusinessBlockName))
                        {
                            currentUnitAllocationTotals.Add(block.BusinessBlockName, 0);
                        }
                        currentUnitAllocationTotals[block.BusinessBlockName] += ManagerFunctionFTE * currentBusinessTimePercentage;
                        sheetWithTime.Column(currentSheetWithTimeCol).Width = 18;
                        sheetWithTime.Cells[currentRow, currentSheetWithTimeCol].Style.Numberformat.Format = "0.00";
                        sheetWithTime.Cells[currentRow, currentSheetWithTimeCol++].Value = currentBusinessTimeSpent;
                        sheetWithPercentage.Column(SheetPercentageCol).Width = 18;
                        sheetWithPercentage.Cells[SheetPercentageRow, SheetPercentageCol].Style.Numberformat.Format = "0.00%";
                        sheetWithPercentage.Cells[SheetPercentageRow, SheetPercentageCol++].Value = currentBusinessTimePercentage;
                    }
                    sheetWithTime.Cells[currentRow, currentSheetWithTimeCol++].Value = ManagerFunctionTimeSpent;
                    if (ManagerFunctionTimeSpent == 0)
                    {
                        sheetWithTime.Cells[currentRow, 1, currentRow, 15].Clear();
                        sheetWithPercentage.Cells[SheetPercentageRow, 1, currentRow, 15].Clear();
                        currentRow--;
                        SheetPercentageRow--;
                    }
                    currentRow++;
                    SheetPercentageRow++;
                    #endregion

                    foreach (Process process in processes)
                    {
                        if (process.Block_Id == 0) //общее управление отображено отдельно
                            continue;

                        if(!records.Any(i=>i.Process_id == process.Id && currentUnit.Analytics.Select(o => o.Id).Contains(i.AnalyticId)))
                        {
                            continue;
                        }
                        currentSheetWithTimeCol = 6;
                        SheetPercentageCol = 6;
                        sheetWithTime.Cells[currentRow, 1].Value = currentUnit.Name;
                        sheetWithTime.Cells[currentRow, 2].Value = UnitName;
                        sheetWithTime.Cells[currentRow, 3].Value = process.Id;
                        sheetWithTime.Cells[currentRow, 4].Value = process.ProcName;
                        sheetWithPercentage.Cells[SheetPercentageRow, 1].Value = currentUnit.Name;
                        sheetWithPercentage.Cells[SheetPercentageRow, 2].Value = UnitName;
                        sheetWithPercentage.Cells[SheetPercentageRow, 3].Value = process.Id;
                        sheetWithPercentage.Cells[SheetPercentageRow, 4].Value = process.ProcName;
                        float currentProcessTimeSpent = (int?)records.
                            Where(record => currentUnit.Analytics.Select(i => i.Id).Contains(record.AnalyticId) &&
                                record.Process_id == process.Id).
                            Select(i => (int?)i.TimeSpent).
                            Sum() ?? 0;

                        float currentProcessFTE = currentProcessTimeSpent / totalTimeSpentForUnit * AnalyticsCount;
                        currentUnitFTETotal += currentProcessFTE;
                        sheetWithTime.Cells[currentRow, 5].Value = currentProcessFTE;
                        sheetWithTime.Cells[currentRow, 5].Style.Numberformat.Format = "0.00";
                        sheetWithPercentage.Cells[SheetPercentageRow, 5].Value = currentProcessFTE;
                        sheetWithPercentage.Cells[SheetPercentageRow, 5].Style.Numberformat.Format = "0.00";

                        float BusinessBlockNotSet = (int?)records.
                                Where(record => currentUnit.Analytics.Select(i => i.Id).Contains(record.AnalyticId) && record.BusinessBlocks.Count == 0 &&
                                record.Process_id == process.Id).
                                Select(i => (float?)i.TimeSpent).
                                Sum() ?? 0;

                        foreach (BusinessBlock block in blocks)
                        {
                            bool isAllocatedToCurrentBlock = (currentUnit.AllocationRules[(MVZ.AllocationRule)block.Id] == 1);

                            float currentBusinessTimeSpent = (float?)records.
                                        Where(record => currentUnit.Analytics.Select(i=>i.Id).Contains(record.AnalyticId) && 
                                            record.BusinessBlocks.Any(o => o.BusinessBlockId == block.Id) &&
                                            record.Process_id == process.Id).
                                        Select(i => i.BusinessBlocks.Count > 0 ? (float?)i.TimeSpent / i.BusinessBlocks.Count : (float?)i.TimeSpent).
                                        Sum() ?? 0;

                            if (isAllocatedToCurrentBlock)
                            {
                                currentBusinessTimeSpent += BusinessBlockNotSet / AllocatedBusinessBlockCount;
                            }

                            float currentBusinessTimePercentage = currentBusinessTimeSpent / currentProcessTimeSpent;

                            if (!currentUnitAllocationTotals.ContainsKey(block.BusinessBlockName))
                            {
                                currentUnitAllocationTotals.Add(block.BusinessBlockName, 0);
                            }
                            currentUnitAllocationTotals[block.BusinessBlockName] += currentProcessFTE * currentBusinessTimePercentage;
                            sheetWithTime.Column(currentSheetWithTimeCol).Width = 18;
                            sheetWithTime.Cells[currentRow, currentSheetWithTimeCol].Style.Numberformat.Format = "0.00";
                            sheetWithTime.Cells[currentRow, currentSheetWithTimeCol++].Value = currentBusinessTimeSpent;
                            sheetWithPercentage.Column(SheetPercentageCol).Width = 18;
                            sheetWithPercentage.Cells[SheetPercentageRow, SheetPercentageCol].Style.Numberformat.Format = "0.00%";
                            sheetWithPercentage.Cells[SheetPercentageRow, SheetPercentageCol++].Value = currentBusinessTimePercentage;
                        }
                        
                        sheetWithTime.Cells[currentRow, currentSheetWithTimeCol++].Value = currentProcessTimeSpent;
                        if (currentProcessTimeSpent == 0)
                        {
                            sheetWithTime.Cells[currentRow, 1, currentRow, 15].Clear();
                            sheetWithPercentage.Cells[SheetPercentageRow, 1, currentRow, 15].Clear();
                            currentRow--;
                            SheetPercentageRow--;
                        }
                        currentRow++;
                        SheetPercentageRow++;
                    }

                    sheetWithPercentage.Cells[SheetPercentageRow, 5].Value = currentUnitFTETotal;
                    sheetWithPercentage.Cells[SheetPercentageRow, 5].Style.Numberformat.Format = "0.00";

                    SheetPercentageCol = 6;
                    foreach (KeyValuePair<string, float> item in currentUnitAllocationTotals)
                    {
                        float x = item.Value / currentUnitFTETotal;
                        sheetWithPercentage.Cells[SheetPercentageRow, SheetPercentageCol].Style.Numberformat.Format = "0.00%";
                        sheetWithPercentage.Cells[SheetPercentageRow, SheetPercentageCol++].Value = x;
                    }
                    sheetWithPercentage.Cells[SheetPercentageRow, 1, SheetPercentageRow, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheetWithPercentage.Cells[SheetPercentageRow, 1, SheetPercentageRow++, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.ForestGreen);
                }
                #region SaveFile & Open
                string newFileName = $"ReportAllocations{DateTime.Now.ToString("ddMMyyyymmss")}.xlsx";
                excel.SaveAs(new FileInfo(newFileName));;
                System.Diagnostics.Process.Start(newFileName);
                #endregion
            }
        }


        private List<MVZ> CreateMVZList()
        {
            List<MVZ> exportValue = new List<MVZ>();
            exportValue.Add(new MVZ
            {
                Name = "2КИ",
                UnitName = "Служба финансового мониторинга в г. Санкт-Петербурге",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 20)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  0 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "3ГЖ",
                UnitName = "Служба финансового мониторинга в г. Новосибирске",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 11)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  0 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "3Ж3",
                UnitName = "Группа финансового мониторинга в г. Омске",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 17)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  0 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "44А",
                UnitName = "Служба финансового контроля и отчетности в г. Ростове-на-Дону",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 10)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           1 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "5ВЖ",
                UnitName = "Служба финансового мониторинга в г. Хабаровске",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 16)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  0 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });

            #region ДФМ
            //ДФМ разделен на маленькие подразделения
            //exportValue.Add(new MVZ
            //{
            //    Name = "Р5Г",
            //    UnitName = "Дирекция финансового мониторинга",
            //    Analytics = new List<Analytic>(analytics.Where(analytic => analytic.DirectionId == 1 &&
            //    analytic.OtdelId!=16 && analytic.OtdelId != 10 && analytic.OtdelId != 17 && analytic.OtdelId != 11 && analytic.OtdelId != 20)),
            //    AllocationRules = new Dictionary<MVZ.AllocationRule, int>
            //    {
            //        { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
            //        { A_CLUB,           0 },
            //        { MASS_BUSINESS,    1 },
            //        { MEDIUM_BUSINESS,  1 },
            //        { BIG_BUSINESS,     1 },
            //        { DIGITAL_BUSINESS, 0 },
            //        { TREASURY,         0 }
            //    }
            //});
            exportValue.Add(new MVZ
            {
                Name = "Р5Г",
                UnitName = "ОМО",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 43)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Г",
                UnitName = "ОНТиЭ",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 9)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Г",
                UnitName = "ОСРТКД",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 18)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Г",
                UnitName = "ОКОФЛ",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 14)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Г",
                UnitName = "ОФМА",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 13)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Г",
                UnitName = "ОСМВУО",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 15)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Г",
                UnitName = "ОФКО",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 12)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Г",
                UnitName = "Руководство",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.DirectionId == 1 && analytic.OtdelId == 19)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            #endregion

            exportValue.Add(new MVZ
            {
                Name = "Р5Д",
                UnitName = "Отдел развития процессов, проектов и аналитики",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 1)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           1 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         1 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });

            #region УКПБП
            //УКПБП было разложено на подразделения
            //exportValue.Add(new MVZ
            //{
            //    Name = "Р5Е",
            //    UnitName = "Управление комплаенс-поддержки бизнес-процессов",
            //    Analytics = new List<Analytic>(analytics.Where(analytic => analytic.UpravlenieId == 1)),
            //    AllocationRules = new Dictionary<MVZ.AllocationRule, int>
            //    {
            //        { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
            //        { A_CLUB,           0 },
            //        { MASS_BUSINESS,    1 },
            //        { MEDIUM_BUSINESS,  1 },
            //        { BIG_BUSINESS,     1 },
            //        { DIGITAL_BUSINESS, 1 },
            //        { TREASURY,         0 }
            //    }
            //});
            exportValue.Add(new MVZ
            {
                Name = "Р5Е",
                UnitName = "ОКЭБП",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 6)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Е",
                UnitName = "ОУРКД",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 8)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Е",
                UnitName = "Руководство",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.UpravlenieId == 1 && analytic.OtdelId == 19)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           0 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });

            #endregion

            #region УМК
            //УМК было декомпозировано
            //exportValue.Add(new MVZ
            //{
            //    Name = "Р5Ж",
            //    UnitName = "Управление международного комплаенса",
            //    Analytics = new List<Analytic>(analytics.Where(analytic => analytic.UpravlenieId == 2)),
            //    AllocationRules = new Dictionary<MVZ.AllocationRule, int>
            //    {
            //        { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
            //        { A_CLUB,           1 },
            //        { MASS_BUSINESS,    1 },
            //        { MEDIUM_BUSINESS,  1 },
            //        { BIG_BUSINESS,     1 },
            //        { DIGITAL_BUSINESS, 1 },
            //        { TREASURY,         1 }
            //    }
            //});
            exportValue.Add(new MVZ
            {
                Name = "Р5Ж",
                UnitName = "ОКВО",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 5)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           1 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         1 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Ж",
                UnitName = "ОМК",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 2)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           1 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         1 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Ж",
                UnitName = "Руководство",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.UpravlenieId == 2 && analytic.OtdelId == 19)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           1 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         1 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            #endregion

            #region УСП
            //УСП было декомпозировано
            //exportValue.Add(new MVZ
            //{
            //    Name = "Р5И",
            //    UnitName = "Управление специальных проектов",
            //    Analytics = new List<Analytic>(analytics.Where(analytic => analytic.UpravlenieId == 3)),
            //    AllocationRules = new Dictionary<MVZ.AllocationRule, int>
            //    {
            //        { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
            //        { A_CLUB,           1 },
            //        { MASS_BUSINESS,    1 },
            //        { MEDIUM_BUSINESS,  1 },
            //        { BIG_BUSINESS,     1 },
            //        { DIGITAL_BUSINESS, 1 },
            //        { TREASURY,         1 }
            //    }
            //});
            exportValue.Add(new MVZ
            {
                Name = "Р5И",
                UnitName = "ОРИНР",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 3)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           1 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         1 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5И",
                UnitName = "ОЭКО",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 4)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           1 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         1 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5И",
                UnitName = "Руководство",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.UpravlenieId == 3 && analytic.OtdelId == 19)),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  1 },
                    { MVZ.AllocationRule.A_CLUB,           1 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    1 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  1 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     1 },
                    { MVZ.AllocationRule.TREASURY,         1 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });
            #endregion

            exportValue.Add(new MVZ
            {
                Name = "Р82",
                UnitName = "Дирекция финансового мониторинга-AP",
                Analytics = new List<Analytic>(),
                AllocationRules = new Dictionary<MVZ.AllocationRule, int>
                {
                    { MVZ.AllocationRule.RETAIL_BUSINESS,  0 },
                    { MVZ.AllocationRule.A_CLUB,           1 },
                    { MVZ.AllocationRule.MASS_BUSINESS,    0 },
                    { MVZ.AllocationRule.MEDIUM_BUSINESS,  0 },
                    { MVZ.AllocationRule.BIG_BUSINESS,     0 },
                    { MVZ.AllocationRule.TREASURY,         0 },
                    { MVZ.AllocationRule.INVESTMENT,       0 }
                }
            });//TODO Разработать правила отбора аналитиков, работающих с Альфа-Прайват

            return exportValue;
        }
    }
}
