using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.IO;

namespace TimeSheetApp.Model
{
    static class ExcelWorker
    {
        private static ExcelPackage excel;
        private static ExcelWorksheet worksheet;

        public static void ExportDataTableToExcel(DataTable data)
        {
            #region EPPLUS
            excel = new ExcelPackage();
            worksheet = excel.Workbook.Worksheets.Add("Sheet1");
            worksheet.Cells[2, 1].LoadFromDataTable(data, false);
            worksheet.Cells[1, 1].Value = "ФИО";
            worksheet.Cells[1, 2].Value = "Имя";
            worksheet.Cells[1, 3].Value = "Отчество";
            worksheet.Cells[1, 4].Value = "Блок";
            worksheet.Cells[1, 5].Value = "Подблок";
            worksheet.Cells[1, 6].Value = "Процесс";
            worksheet.Cells[1, 7].Value = "Тема";
            worksheet.Cells[1, 8].Value = "Комментарий";
            worksheet.Cells[1, 9].Value = "Начало";
            worksheet.Cells[1, 10].Value = "Окончание";
            worksheet.Cells[1, 11].Value = "Длительность";
            worksheet.Cells[1, 12].Value = "Бизнес подразделение";
            worksheet.Cells[1, 13].Value = "Саппорт";
            worksheet.Cells[1, 14].Value = "Клиентский путь";
            worksheet.Cells[1, 15].Value = "Эскалация";
            worksheet.Cells[1, 16].Value = "Формат";
            worksheet.Cells[1, 17].Value = "Риск";
            worksheet.Column(11).Style.Numberformat.Format = "0";
            worksheet.Column(9).Style.Numberformat.Format = @"dd.MM.yyyy hh:mm";
            worksheet.Column(10).Style.Numberformat.Format = @"dd.MM.yyyy hh:mm";
            worksheet.Column(1).Width = 35;
            worksheet.Column(4).Width = 35;
            worksheet.Column(5).Width = 35;
            worksheet.Column(6).Width = 35;
            worksheet.Column(7).Width = 30;
            worksheet.Column(8).Width = 30;
            worksheet.Column(9).Width = 15.5;
            worksheet.Column(10).Width = 15.5;
            worksheet.Column(11).Width = 13.5;
            worksheet.Column(12).Width = 25;
            worksheet.Column(13).Width = 25;
            worksheet.Column(14).Width = 25;
            worksheet.Column(15).Width = 25;
            worksheet.Column(16).Width = 25;
            worksheet.Column(17).Width = 25;
            for (int i = 2; i < worksheet.Dimension.Rows + 1; i++)
                worksheet.Cells[i, 1].Value = $"{worksheet.Cells[i, 1].Value} {worksheet.Cells[i, 2].Value} {worksheet.Cells[i, 3].Value}";
            worksheet.DeleteColumn(2);
            worksheet.DeleteColumn(2);
            worksheet.Row(2).OutlineLevel = 1;
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            string analyticName = worksheet.Cells[2, 1].Text;
            int analyticStartRow = 2;
            int RowsCount = worksheet.Dimension.Rows + 1;
            for (int i = 3; i < RowsCount + 1; i++)
            {
                worksheet.Row(i - 1).Collapsed = true;
                if (!string.IsNullOrWhiteSpace(analyticName) && analyticName == worksheet.Cells[i, 1].Text)
                {
                    worksheet.Row(i).OutlineLevel = 2;

                }
                else if (!string.IsNullOrWhiteSpace(analyticName))
                {
                    worksheet.InsertRow(i, 1);

                    worksheet.Cells[i, 1, i, 15].Style.Font.Bold = true;
                    worksheet.Cells[i, 1, i, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[i, 1, i, 15].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    worksheet.Cells[i, 9].Formula = $"SUM(I{analyticStartRow}:I{i - 1})";
                    worksheet.Cells[i, 1].Value = $"ИТОГО: {worksheet.Cells[i - 1, 1].Text}";
                    worksheet.Row(i).OutlineLevel = 0;
                    i++;
                    RowsCount++;
                    analyticStartRow = i;
                    if (string.IsNullOrWhiteSpace((analyticName = worksheet.Cells[i, 1].Text)))
                        break;
                    worksheet.Row(i).OutlineLevel = 2;
                }
            }

            string fileName = $"Reports\\Report{Environment.UserName}{DateTime.Now.ToString($"ddMMyyyy_HHmmss")}.xlsx";
            FileInfo newExcelFile = new FileInfo(fileName);
            excel.SaveAs(newExcelFile);
            excel.Dispose();
            #endregion
            System.Diagnostics.Process.Start(fileName);
        }
    }
}