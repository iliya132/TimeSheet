using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace TimeSheetApp.Services
{
    public static class UpdateService
    {
        static double currentVer;
        static double targetVer;
        public static void CheckForUpdate()
        {
            double.TryParse(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""), out currentVer);
            FileVersionInfo ServerFileVersion = FileVersionInfo.GetVersionInfo(@"\\moscow\hdfs\WORK\Архив необычных операций\ОРППА\Timesheet\Data\TimeSheetApp.exe");
            double.TryParse(ServerFileVersion.FileVersion.Replace(".",string.Empty), out targetVer);
            if (currentVer < targetVer)
            {
                MessageBox.Show("Обнаружена новая версия программы. TimeSheet будет перезапущен после обновления", "Обновление", MessageBoxButton.OK, MessageBoxImage.Information);
                List<string> updateText = new List<string>();
                updateText.Add("-g");
                foreach (string fileName in Directory.GetFiles(@"\\moscow\hdfs\WORK\Архив необычных операций\ОРППА\Timesheet\Data"))
                {
                    updateText.Add($"\"{fileName}\"");
                }
                updateText.Add("-k");
                updateText.Add($"{Process.GetCurrentProcess().ProcessName}");
                updateText.Add("-r");
                updateText.Add($"TimeSheetApp.exe");
                Process.Start("updaterForm.exe", string.Join(" ", updateText.ToArray()));
            }
        }
    }
}
