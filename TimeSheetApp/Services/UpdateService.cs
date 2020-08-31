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
        const string APP_NAME = "TimeSheetApp.exe";
        const string ROOT_PATH = @"\\moscow\hdfs\WORK\Архив необычных операций\ОРППА\Timesheet\Data";
        const string CHANGELOG_FILEPATH = @"\\moscow\hdfs\WORK\Архив необычных операций\ОРППА\Timesheet\Data\Changelog.txt";
        const string UPDATE_TEXT = "Обнаружена новая версия программы. TimeSheet будет перезапущен после обновления";

        public static void CheckForUpdate()
        {
            double.TryParse(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""), out currentVer);
            FileVersionInfo ServerFileVersion = FileVersionInfo.GetVersionInfo($"{ROOT_PATH}\\{APP_NAME}");
            double.TryParse(ServerFileVersion.FileVersion.Replace(".", string.Empty), out targetVer);
            if (currentVer < targetVer)
            {
                MessageBox.Show(UPDATE_TEXT, "Обновление", MessageBoxButton.OK, MessageBoxImage.Information);
                List<string> updateText = new List<string>();
                updateText.Add("-g");
                foreach (string fileName in Directory.GetFiles(ROOT_PATH))
                {
                    if (fileName.IndexOf("Changelog.txt") > -1)
                        continue;
                    updateText.Add($"\"{fileName}\"");
                }
                updateText.Add("-k");
                updateText.Add($"{Process.GetCurrentProcess().ProcessName}");
                updateText.Add("-r");
                updateText.Add(APP_NAME);
                updateText.Add("-d");
                using (StreamReader reader = new StreamReader(CHANGELOG_FILEPATH))
                {
                    string readedLine = string.Empty;
                    while ((readedLine = reader.ReadLine()) != null)
                    {
                        updateText.Add(readedLine);
                    }
                }
                updateText.Add(APP_NAME);
                Process.Start("updaterForm.exe", string.Join(" ", updateText.ToArray()));
            }
        }
    }
}
