using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using TimeSheetApp.Model.EntitiesBase;
using TimeSheetApp.Services;
using Xceed.Wpf.Toolkit;

namespace TimeSheetApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Что бы программа не реагировала на изменение значений при инициализации (highlightControl метод)
        private bool isInitialized = false;
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

                isInitialized = true;

                //Если это первый запуск после обновления - запустится help с информацией об обновлении
                if (File.Exists("updated.txt"))
                {
                    File.Delete("updated.txt");
                    HelpBtn_Click(null, null);
                }
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

            if (!(sender as TimePicker).IsFocused && isInitialized)
            {
                highlightControl(sender as Control);
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
        private void TimeIcon_MouseDown(object sender, RoutedEventArgs e)
        {
            if (sender == StartIcon)
            {
                TimeIn.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            }
            else if (sender == EndIcon)
            {
                Timeout.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            }
        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {

            if (!Directory.Exists($"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet"))
            {
                Directory.CreateDirectory($"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet");
            }
            File.Copy("\\\\moscow\\hdfs\\WORK\\Архив необычных операций\\ОРППА\\Timesheet\\Data\\Help\\TimeSheetHelp.chm", $"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet\\TimeSheetHelp.chm", true);
            System.Diagnostics.Process.Start($"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet\\TimeSheetHelp.chm");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateService.CheckForUpdate();
        }

        private void SelectCalendarItem_BtnClick(object sender, RoutedEventArgs e)
        {
            SelectCalendarItems sc = new SelectCalendarItems();
            sc.Owner = this;
            sc.ShowDialog();
        }

        private void Subject_TextChangedEvent()
        {
            if (!Subject.textField.IsFocused)
            {
                highlightControl(Subject.textField);
            }
        }

        private async void highlightControl(Control control)
        {
            await Task.Run(() =>
            {
                for (byte i = 125; i > 0; i--)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        control.Background = new SolidColorBrush(Color.FromRgb(255, 255, (byte)(i * 2)));
                    });

                    Thread.Sleep(1);
                }

                for (byte i = 0; i < 125; i++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        control.Background = new SolidColorBrush(Color.FromRgb(255, 255, (byte)(i * 2)));
                    });

                    Thread.Sleep(1);
                }

                this.Dispatcher.Invoke(() =>
                {
                    control.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                });
            });
        }

        private void SelectTimeAfterClick(object sender, RoutedEventArgs e)
        {
            if(TimeSpanListView.SelectedItem == null)
            {
                return;
            }
            DateTime startTime, endTime;
            startTime = (TimeSpanListView.SelectedItem as TimeSheetTable).TimeEnd;
            if (TimeSpanListView.SelectedIndex == TimeSpanListView.Items.Count - 1)
            {
                endTime = startTime.AddMinutes(15);
            }
            else
            {
                endTime = (TimeSpanListView.Items[TimeSpanListView.SelectedIndex + 1] as TimeSheetTable).TimeStart;
            }

            TimeIn.Value = startTime;
            Timeout.Value = endTime;

        }

        private void SelectTimeBeforeClick(object sender, RoutedEventArgs e)
        {
            if (TimeSpanListView.SelectedItem == null)
            {
                return;
            }
            DateTime startTime, endTime;
            endTime = (TimeSpanListView.SelectedItem as TimeSheetTable).TimeStart;
            if (TimeSpanListView.SelectedIndex == 0)
            {
                startTime = endTime.AddMinutes(-15);
            }
            else
            {
                startTime = (TimeSpanListView.Items[TimeSpanListView.SelectedIndex - 1] as TimeSheetTable).TimeEnd;
            }

            TimeIn.Value = startTime;
            Timeout.Value = endTime;
        }

        private void EditUserNameBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowEdit();
        }

        private void EditUserName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape || e.Key == System.Windows.Input.Key.Return)
            {
                HideEdit();
            }
        }

        private void EditUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            EditUserName.Text = UserNameLabel.Text;
            HideEdit();
        }

        private void ShowEdit()
        {
            UserNameLabel.Visibility = Visibility.Collapsed;
            EditUserName.Visibility = Visibility.Visible;
            editUserNameBtn.Visibility = Visibility.Collapsed;
            EditUserName.Focus();
            EditUserName.SelectAll();
        }

        private void HideEdit()
        {
            UserNameLabel.Visibility = Visibility.Visible;
            UserNameLabel.Focus();
            EditUserName.Visibility = Visibility.Collapsed;
            editUserNameBtn.Visibility = Visibility.Visible;
        }


    }
}
