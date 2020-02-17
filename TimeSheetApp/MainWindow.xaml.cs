using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace TimeSheetApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleError((Exception)e.ExceptionObject);
            try
            {
                InitializeComponent();
                #region Группировка добавленных активностей по времени начала
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(TimeSpanListView.ItemsSource);
                view?.SortDescriptions.Add(new SortDescription("TimeStart", ListSortDirection.Ascending));
                #endregion

                TimeIn.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
                Timeout.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 20, 0);
                DateBox.SelectedDate = DateTime.Now;
            }
            catch (Exception e)
            {
                HandleError(e);
                Environment.Exit(-1);
            }
        }
        public static void WriteLog(string msg)
        {
            using (StreamWriter sw = new StreamWriter($"{Environment.UserName}_exception.txt", false))
            {
                sw.WriteLine(msg);
            }
        }
        private void HandleError(Exception exceptionObject)
        {
            WriteLog($"{Environment.UserName}_exception.txt");
            WriteLog($"Source: {exceptionObject.Source}");
            WriteLog($"Message: {exceptionObject.Message}");
            WriteLog($"InnerException: {exceptionObject.InnerException}");
            WriteLog($"StackTrace: {exceptionObject.StackTrace}");

            if (exceptionObject.Data.Count > 0)
            {
                WriteLog($"ExtraData:");
                foreach (DictionaryEntry key in exceptionObject.Data)
                {
                    WriteLog($"DATA: Key:{key.Key}. Value: {key.Value}");
                }
            }
            else
            {
                WriteLog($"NoExtraData");
            }
            System.Windows.MessageBox.Show($"{exceptionObject.Message}, {exceptionObject.InnerException},", "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        
        }

        /// <summary>
        /// Разворачивает интересующую группу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Expander_Loaded(object sender, RoutedEventArgs e)
        {
            //var expander = e.Source as Expander;
            //if (expander == null)
            //    return;
            //expander.IsExpanded = expander.Tag.ToString() == "Организация";
        }
        /// <summary>
        /// При изменении выбранного времени устанавливает также и дату из поля DateBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if ((sender as TimePicker).Value != null && DateBox.SelectedDate != null)
            {
                DateTime time = (DateTime)(sender as TimePicker).Value.Value;
                DateTime date = (DateTime)(DateBox.SelectedDate);
                if ((sender as TimePicker).Value.Value.Day != date.Day)
                {
                    (sender as TimePicker).Value = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
                }
            }

        }
        /// <summary>
        /// При изменении даты устанавливает дату и в полях выбора времени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as DatePicker).SelectedDate != null)
            {
                DateTime dateSelected = (sender as DatePicker).SelectedDate.Value;

                if (TimeIn != null && Timeout != null)
                {
                    TimeIn.Value = new DateTime(dateSelected.Year, dateSelected.Month, dateSelected.Day, TimeIn.Value.Value.Hour, TimeIn.Value.Value.Minute, 0);
                    Timeout.Value = new DateTime(dateSelected.Year, dateSelected.Month, dateSelected.Day, Timeout.Value.Value.Hour, Timeout.Value.Value.Minute, 0);
                }
            }
        }
        /// <summary>
        /// При нажатии на иконку времени установить текущее время
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender == StartIcon)
            {
                TimeIn.Value = DateTime.Now;
            }
            else if (sender == EndIcon)
            {
                Timeout.Value = DateTime.Now;
            }
        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists($"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet"))
            {
                Directory.CreateDirectory($"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet");
            }
            File.Copy("\\\\moscow\\hdfs\\WORK\\Архив необычных операций\\ОРППА\\Timesheet\\Program Files\\Help\\TimeSheetHelp.chm", $"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet\\TimeSheetHelp.chm",true);
            System.Diagnostics.Process.Start($"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet\\TimeSheetHelp.chm");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
