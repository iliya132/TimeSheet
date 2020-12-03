using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdaterForm
{
    public partial class Form1 : Form
    {

        public Form1(string[] args)
        {
            InitializeComponent();
            if(args.Length==0 || args == null)
            {
                Environment.Exit(0);
            }
            Update(args);
        }
        enum Operation
        {
            Get = 0,
            Kill = 1,
            Run = 2,
            Delete = 3
        }

        public void Update(string[] args)
        {
            Show();
            Label.Text = "Подготовка к обновлению.";
            if (args.Length < 1) return;
            List<string> filesToCopy = new List<string>();
            List<string> filesToRun = new List<string>();
            List<string> filesToKill = new List<string>();
            List<string> filesToDelete = new List<string>();
            Operation currentOperation = 0;
            string currentDirectoryStr = Directory.GetCurrentDirectory();

            #region Наполняем коллекции операций
            foreach (string line in args)
            {
                if (line.ToLower().Equals("-g")) { currentOperation = Operation.Get; continue; }
                if (line.ToLower().Equals("-k")) { currentOperation = Operation.Kill; continue; }
                if (line.ToLower().Equals("-r")) { currentOperation = Operation.Run; continue; }
                if (line.ToLower().Equals("-d")) { currentOperation = Operation.Delete; continue; }

                switch (currentOperation)
                {
                    case (Operation.Get):
                        filesToCopy.Add(line);
                        break;
                    case (Operation.Kill):
                        filesToKill.Add(line);
                        break;
                    case (Operation.Run):
                        filesToRun.Add(line);
                        break;
                    case (Operation.Delete):
                        filesToDelete.Add(line);
                        break;
                }
            }
            #endregion

            #region KillProcesses
            foreach (string fileStr in filesToKill)
            {
                int count = 10;
                while (Process.GetProcessesByName(fileStr).Length > 0)
                {
                    Console.WriteLine($"Try to kill {fileStr} process");
                    Process.GetProcessesByName(fileStr).First().Kill();
                    Thread.Sleep(100);
                    count--;
                    if (count <= 0)
                    {
                        Console.WriteLine($"Unable to terminate {fileStr} process");
                        break;
                    }
                }
            }
            #endregion

            ProgressBar.Maximum = filesToDelete.Count + filesToCopy.Count;

            #region  Удаляем все файлы в директории
            foreach (string fileStr in filesToDelete)
            {
                ProgressBar.Value = ProgressBar.Value + 1;
                Label.Text = $"Удаление {fileStr}";
                Refresh();
                Update();
                // игнорируем файл updater.exe
                if (fileStr.ToLower().IndexOf("updater.exe") > -1) continue;

                //все остальные файлы пытаемся удалить
                try
                {
                    File.Delete(fileStr);
                    Console.WriteLine($"Deleting {new FileInfo(fileStr).Name}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            #endregion

            #region Копируем новые файлы
            foreach (string fileStr in filesToCopy)
            {
                ProgressBar.Value = ProgressBar.Value + 1;
                Label.Text = $"Копирование {fileStr}";
                Refresh();
                FileInfo fileInfo = new FileInfo(fileStr);
                Console.WriteLine($"Copy new file {fileInfo.Name}");
                try
                {
                    File.Copy(fileStr, $"{fileInfo.Name}");
                    //TODO: отобразить прогресс
                }
                catch { Console.WriteLine($"Unable to find {fileStr}"); }
            }


            #endregion
            Label.Text = $"Завершение";
            Refresh();
            #region Запускаем нужные программы
            using (StreamWriter writer = new StreamWriter("updated.txt"))
            {
                writer.WriteLine("Этот файл был сгенерирован автоматически для целей программы");
            }

            foreach (string fileStr in filesToRun)
            {
                Process.Start(fileStr);
            }
            #endregion

            Environment.Exit(0);
        }
    }
}
