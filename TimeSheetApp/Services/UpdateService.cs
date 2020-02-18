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
            using (StreamReader reader = new StreamReader(@"\\moscow\hdfs\WORK\Архив необычных операций\ОРППА\Timesheet\Program Files\version.txt"))
            {
                Console.WriteLine();
                double.TryParse(reader.ReadLine(), out targetVer);
            }
            Console.WriteLine($"{currentVer} - {targetVer}");
            if (currentVer != targetVer)
            {
                MessageBox.Show("Обнаружена новая версия программы. TimeSheet будет перезапущен после обновления", "Обновление", MessageBoxButton.OK, MessageBoxImage.Information);
                List<string> updateText = new List<string>();
                using (StreamReader reader = new StreamReader(@"\\moscow\hdfs\WORK\Архив необычных операций\ОРППА\Timesheet\Program Files\updateData.txt"))
                {
                    string line = reader.ReadLine();
                    while (!string.IsNullOrEmpty(line))
                    {
                        updateText.Add($"{line}");
                        line = reader.ReadLine();
                    }
                }
                updateText.Add("-k");
                updateText.Add($"{Process.GetCurrentProcess().ProcessName}");
                
                Process.Start("updater.exe", string.Join(" ", updateText.ToArray()));
            }
        }
    }
}
