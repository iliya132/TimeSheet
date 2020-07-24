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

            List<TimeSheetTable> records = _tsContext.TimeSheetTableSet.Include("BusinessBlocks").Where(rec => rec.TimeStart > start && rec.TimeEnd < end && rec.Process_id != 62 && rec.Process_id != 63).ToList();
            List<BusinessBlock> blocks = _tsContext.BusinessBlockSet.ToList();
            List<Process> processes = _tsContext.ProcessSet.ToList();
            allUnits = CreateMVZList();

            using (ExcelPackage excel = new ExcelPackage())
            {
                ExcelWorksheet sheetWithTime = excel.Workbook.Worksheets.Add("Аллокации ДК (min)");
                ExcelWorksheet sheetWithPercentage = excel.Workbook.Worksheets.Add("Аллокации ДК (%)");
                int currentShetWithTimeRow = 2;
                int currentSheetWithTimeCol = 6;
                int currentShetPercentageTimeRow = 2;
                int currentSheetWithPercentageCol = 6;

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
                sheetWithTime.Column(1).Width = 5;
                sheetWithTime.Column(2).Width = 60;
                sheetWithTime.Column(3).Width = 9;
                sheetWithTime.Column(4).Width = 70;
                sheetWithTime.Column(5).Width = 9;
                sheetWithTime.Column(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetWithTime.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetWithTime.Column(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetWithTime.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetWithTime.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetWithTime.Column(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetWithTime.Column(10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetWithTime.Column(11).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetWithTime.Column(12).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheetWithTime.Column(13).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
                    sheetWithTime.Cells[1, currentSheetWithTimeCol++].Value = block.BusinessBlockName;
                    sheetWithPercentage.Cells[1, currentSheetWithPercentageCol++].Value = block.BusinessBlockName;
                }
                sheetWithTime.Cells[1, currentSheetWithTimeCol++].Value = "Total на процесс";
                #endregion
                int BusinessBlocksCount = blocks.Count();
                foreach (MVZ currentUnit in allUnits)
                {
                    if (currentUnit.Analytics.Count < 1)
                        continue;
                    System.Windows.Forms.Application.DoEvents();
                    Console.WriteLine(currentUnit.UnitName);
                    string UnitName = currentUnit.UnitName;
                    
                    float totalTimeSpentForUnit = (float?)records.
                        Where(x => currentUnit.Analytics.Select(i => i.Id).Contains(x.AnalyticId)).
                        Select(i=>(float?)i.TimeSpent).
                        Sum() ?? 0;
                    int AnalyticsCount = records.
                            Where(record => currentUnit.Analytics.Select(i => i.Id).Contains(record.AnalyticId)).Select(record => record.AnalyticId).Distinct().Count();

                    int AllocatedBusinessBlockCount = currentUnit.AllocationRules.Values.Sum();

                    foreach (Process process in processes)
                    {
                        currentSheetWithTimeCol = 6;
                        currentSheetWithPercentageCol = 6;
                        sheetWithTime.Cells[currentShetWithTimeRow, 1].Value = currentUnit.Name;
                        sheetWithTime.Cells[currentShetWithTimeRow, 2].Value = UnitName;
                        sheetWithTime.Cells[currentShetWithTimeRow, 3].Value = process.Id;
                        sheetWithTime.Cells[currentShetWithTimeRow, 4].Value = process.ProcName;
                        sheetWithPercentage.Cells[currentShetPercentageTimeRow, 1].Value = currentUnit.Name;
                        sheetWithPercentage.Cells[currentShetPercentageTimeRow, 2].Value = UnitName;
                        sheetWithPercentage.Cells[currentShetPercentageTimeRow, 3].Value = process.Id;
                        sheetWithPercentage.Cells[currentShetPercentageTimeRow, 4].Value = process.ProcName;
                        float currentProcessTimeSpent = (int?)records.
                            Where(record => currentUnit.Analytics.Select(i => i.Id).Contains(record.AnalyticId) &&
                                record.Process_id == process.Id).
                            Select(i => (int?)i.TimeSpent).
                            Sum() ?? 0;

                        float currentProcessFTE = currentProcessTimeSpent / totalTimeSpentForUnit * AnalyticsCount;
                        sheetWithTime.Cells[currentShetWithTimeRow, 5].Value = currentProcessFTE;
                        sheetWithTime.Cells[currentShetWithTimeRow, 5].Style.Numberformat.Format = "0.00";
                        sheetWithPercentage.Cells[currentShetPercentageTimeRow, 5].Value = currentProcessFTE;
                        sheetWithPercentage.Cells[currentShetPercentageTimeRow, 5].Style.Numberformat.Format = "0.00";

                        float BusinessBlockNotSet = (int?)records.
                                Where(record => currentUnit.Analytics.Select(i => i.Id).Contains(record.AnalyticId) && record.BusinessBlocks.Count == 0 &&
                                record.Process_id == process.Id).
                                Select(i => (float?)i.TimeSpent).
                                Sum() ?? 0;

                        foreach (BusinessBlock block in blocks)
                        {
                            bool isAllocatedToCurrentBlock = (currentUnit.AllocationRules[block.Id] == 1);

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

                            sheetWithTime.Column(currentSheetWithTimeCol).Width = 18;
                            sheetWithTime.Cells[currentShetWithTimeRow, currentSheetWithTimeCol].Style.Numberformat.Format = "0.00";
                            sheetWithTime.Cells[currentShetWithTimeRow, currentSheetWithTimeCol++].Value = currentBusinessTimeSpent;
                            sheetWithPercentage.Column(currentSheetWithPercentageCol).Width = 18;
                            sheetWithPercentage.Cells[currentShetPercentageTimeRow, currentSheetWithPercentageCol].Style.Numberformat.Format = "0.00%";
                            sheetWithPercentage.Cells[currentShetPercentageTimeRow, currentSheetWithPercentageCol++].Value = currentBusinessTimePercentage;
                        }
                        
                        sheetWithTime.Cells[currentShetWithTimeRow, currentSheetWithTimeCol++].Value = currentProcessTimeSpent;
                        if (currentProcessTimeSpent == 0)
                        {
                            sheetWithTime.Cells[currentShetWithTimeRow, 1, currentShetWithTimeRow, 15].Clear();
                            sheetWithPercentage.Cells[currentShetPercentageTimeRow, 1, currentShetWithTimeRow, 15].Clear();
                            currentShetWithTimeRow--;
                            currentShetPercentageTimeRow--;
                        }
                        currentShetWithTimeRow++;
                        currentShetPercentageTimeRow++;
                    }
                }
                #region SaveFile & Open
                string newFileName = $"ReportAllocations{DateTime.Now.ToString("ddMMyyyymmss")}.xlsx";
                excel.SaveAs(new FileInfo(newFileName));;
                System.Diagnostics.Process.Start(newFileName);
                #endregion
            }
        }

        const int RETAIL_BUSINESS = 1; //Розничный бизнес
        const int A_CLUB = 2; //А-Клуб
        const int MASS_BUSINESS = 3; //Малый и микробизнес
        const int MEDIUM_BUSINESS = 4; //Средний бизнес
        const int BIG_BUSINESS = 5; //Корпоративный и инвестиционный бизнес
        const int DIGITAL_BUSINESS = 6; //Цифровой бизнес
        const int TREASURY = 7; //Казначейство
        private List<MVZ> CreateMVZList()
        {
            List<MVZ> exportValue = new List<MVZ>();
            exportValue.Add(new MVZ
            {
                Name = "2КИ",
                UnitName = "Служба финансового мониторинга в г. Санкт-Петербурге",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 20)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  0 },
                    { A_CLUB,           0 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 0 },
                    { TREASURY,         0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "3ГЖ",
                UnitName = "Служба финансового мониторинга в г. Новосибирске",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 11)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  0 },
                    { A_CLUB,           0 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 0 },
                    { TREASURY,         0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "3Ж3",
                UnitName = "Группа финансового мониторинга в г. Омске",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 17)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  0 },
                    { A_CLUB,           0 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 0 },
                    { TREASURY,         0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "44А",
                UnitName = "Служба финансового контроля и отчетности в г. Ростове-на-Дону",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 10)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  1 },
                    { A_CLUB,           1 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 1 },
                    { TREASURY,         0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "5ВЖ",
                UnitName = "Служба финансового мониторинга в г. Хабаровске",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 16)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  0 },
                    { A_CLUB,           0 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 0 },
                    { TREASURY,         0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Г",
                UnitName = "Дирекция финансового мониторинга",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.DirectionId == 1 &&
                analytic.OtdelId!=16 && analytic.OtdelId != 10 && analytic.OtdelId != 17 && analytic.OtdelId != 11 && analytic.OtdelId != 20)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  1 },
                    { A_CLUB,           0 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 0 },
                    { TREASURY,         0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Д",
                UnitName = "Отдел развития процессов, проектов и аналитики",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.OtdelId == 1)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  1 },
                    { A_CLUB,           1 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 1 },
                    { TREASURY,         1 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Е",
                UnitName = "Управление комплаенс-поддержки бизнес-процессов",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.UpravlenieId == 1)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  1 },
                    { A_CLUB,           0 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 1 },
                    { TREASURY,         0 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5Ж",
                UnitName = "Управление международного комплаенса",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.UpravlenieId == 2)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  1 },
                    { A_CLUB,           1 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 1 },
                    { TREASURY,         1 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р5И",
                UnitName = "Управление специальных проектов",
                Analytics = new List<Analytic>(analytics.Where(analytic => analytic.UpravlenieId == 3)),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  1 },
                    { A_CLUB,           1 },
                    { MASS_BUSINESS,    1 },
                    { MEDIUM_BUSINESS,  1 },
                    { BIG_BUSINESS,     1 },
                    { DIGITAL_BUSINESS, 1 },
                    { TREASURY,         1 }
                }
            });
            exportValue.Add(new MVZ
            {
                Name = "Р82",
                UnitName = "Дирекция финансового мониторинга-AP",
                Analytics = new List<Analytic>(),
                AllocationRules = new Dictionary<int, int>
                {
                    { RETAIL_BUSINESS,  0 },
                    { A_CLUB,           1 },
                    { MASS_BUSINESS,    0 },
                    { MEDIUM_BUSINESS,  0 },
                    { BIG_BUSINESS,     0 },
                    { DIGITAL_BUSINESS, 0 },
                    { TREASURY,         0 }
                }
            });//TODO Разработать правила отбора аналитиков, работающих с Альфа-Прайват

            return exportValue;
        }
    }
}
