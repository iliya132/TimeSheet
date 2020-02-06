using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TimeSheetApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Exception.Source = sender.ToString();
            HandleError(e.Exception);
        }

        private void HandleError(Exception exceptionObject)
        {
            TimeSheetApp.MainWindow.WriteLog($"Source: {exceptionObject.Source}");
            TimeSheetApp.MainWindow.WriteLog($"Message: {exceptionObject.Message}");
            TimeSheetApp.MainWindow.WriteLog($"InnerException: {exceptionObject.InnerException}");
            TimeSheetApp.MainWindow.WriteLog($"StackTrace: {exceptionObject.StackTrace}");
            TimeSheetApp.MainWindow.WriteLog($"Source: {exceptionObject.Source}");
            MessageBox.Show($"{exceptionObject.Message}, {exceptionObject.InnerException},", "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
