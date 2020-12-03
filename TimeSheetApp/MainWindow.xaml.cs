using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
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
        bool hightligthing = false;
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

                LoadConfig();

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

            if (!(sender as TimePicker).IsFocused && isInitialized && !hightligthing)
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
#if !DevAtHome
        string helpFilePath = "\\\\moscow\\hdfs\\WORK\\Архив необычных операций\\ОРППА\\Timesheet\\Data\\Help\\TimeSheetHelp.chm";
#else
        string helpFilePath = @"C:\Users\iliya\Source\Repos\iliya132\TimeSheet\TimeSheetApp\bin\Debug\Help\TimeSheetHelp.chm";
#endif
        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {

            if (!Directory.Exists($"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet"))
            {
                Directory.CreateDirectory($"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet");
            }
            File.Copy(helpFilePath, $"{Environment.ExpandEnvironmentVariables("%appdata%")}\\TimeSheet\\TimeSheetHelp.chm", true);
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

        private async void highlightControl(Control control)
        {
            hightligthing = true;
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
            hightligthing = false;
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

        private void PrevDate_Click(object sender, RoutedEventArgs e)
        {
            DateBox.SelectedDate = DateBox.SelectedDate.Value.AddDays(-1);
        }

        private void SelectToday_Click(object sender, RoutedEventArgs e)
        {
            DateBox.SelectedDate = DateTime.Today;
        }

        private void NextDate_Click(object sender, RoutedEventArgs e)
        {
            DateBox.SelectedDate = DateBox.SelectedDate.Value.AddDays(1);
        }

        private void HideOrShowBBSelectors(object sender, RoutedEventArgs e)
        {
            if(BBSelectors.Visibility == Visibility.Visible)
            {
                BBSelectors.Visibility = Visibility.Collapsed;
                bbminimg.Visibility = Visibility.Collapsed;
                bbmax.Visibility = Visibility.Visible;
            }
            else
            {
                BBSelectors.Visibility = Visibility.Visible;
                bbminimg.Visibility = Visibility.Visible;
                bbmax.Visibility = Visibility.Collapsed;
            }
        }
        private void HideOrShowSupSelectors(object sender, RoutedEventArgs e)
        {
            if (SupSelector.Visibility == Visibility.Visible)
            {
                SupSelector.Visibility = Visibility.Collapsed;
                Supminimg.Visibility = Visibility.Collapsed;
                supmax.Visibility = Visibility.Visible;
            }
            else
            {
                SupSelector.Visibility = Visibility.Visible;
                Supminimg.Visibility = Visibility.Visible;
                supmax.Visibility = Visibility.Collapsed;
            }
        }
        private void HideOrShowCWSelectors(object sender, RoutedEventArgs e)
        {
            if (CWSelector.Visibility == Visibility.Visible)
            {
                CWSelector.Visibility = Visibility.Collapsed;
                CWminimg.Visibility = Visibility.Collapsed;
                CWmax.Visibility = Visibility.Visible;
            }
            else
            {
                CWSelector.Visibility = Visibility.Visible;
                CWminimg.Visibility = Visibility.Visible;
                CWmax.Visibility = Visibility.Collapsed;
            }
        }
        private void HideOrShowEscSelectors(object sender, RoutedEventArgs e)
        {
            if (EscSelector.Visibility == Visibility.Visible)
            {
                EscSelector.Visibility = Visibility.Collapsed;
                Escminimg.Visibility = Visibility.Collapsed;
                Escmax.Visibility = Visibility.Visible;
            }
            else
            {
                EscSelector.Visibility = Visibility.Visible;
                Escminimg.Visibility = Visibility.Visible;
                Escmax.Visibility = Visibility.Collapsed;
            }
        }
        private void HideOrShowFormatSelectors(object sender, RoutedEventArgs e)
        {
            if (FormatSelector.Visibility == Visibility.Visible)
            {
                FormatSelector.Visibility = Visibility.Collapsed;
                Formatminimg.Visibility = Visibility.Collapsed;
                Formatmax.Visibility = Visibility.Visible;
            }
            else
            {
                FormatSelector.Visibility = Visibility.Visible;
                Formatminimg.Visibility = Visibility.Visible;
                Formatmax.Visibility = Visibility.Collapsed;
            }
        }
        private void HideOrShowRiskSelectors(object sender, RoutedEventArgs e)
        {
            if (RiskSelector.Visibility == Visibility.Visible)
            {
                RiskSelector.Visibility = Visibility.Collapsed;
                Riskminimg.Visibility = Visibility.Collapsed;
                Riskmax.Visibility = Visibility.Visible;
            }
            else
            {
                RiskSelector.Visibility = Visibility.Visible;
                Riskminimg.Visibility = Visibility.Visible;
                Riskmax.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadConfig()
        {
            string config = "111111";
            if (File.Exists("UIconfig.bin"))
            {
                using (StreamReader sr = new StreamReader("UIconfig.bin"))
                {
                    config = sr.ReadToEnd();
                    if (config.Length != 6)
                        return;
                }
            }
            for (int i = 0; i < config.Length; i++)
            {
                if (config[i].Equals('0'))
                {
                    switch (i) 
                    {
                        case (0):
                            HideOrShowBBSelectors(null, null);
                            break;
                        case (1):
                            HideOrShowSupSelectors(null, null);
                            break;
                        case (2):
                            HideOrShowCWSelectors(null, null);
                            break;
                        case (3):
                            HideOrShowEscSelectors(null, null);
                            break;
                        case (4):
                            HideOrShowFormatSelectors(null, null);
                            break;
                        case (5):
                            HideOrShowRiskSelectors(null, null);
                            break;
                    }
                }
            }
        }

        private void SaveConfig()
        {
            using(StreamWriter sw = new StreamWriter("UIconfig.bin", false))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(BBSelectors.Visibility == Visibility.Visible ? 1 : 0);
                sb.Append(SupSelector.Visibility == Visibility.Visible ? 1 : 0);
                sb.Append(CWSelector.Visibility == Visibility.Visible ? 1 : 0);
                sb.Append(EscSelector.Visibility == Visibility.Visible ? 1 : 0);
                sb.Append(FormatSelector.Visibility == Visibility.Visible ? 1 : 0);
                sb.Append(RiskSelector.Visibility == Visibility.Visible ? 1 : 0);
                sw.Write(sb.ToString());
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveConfig();
        }
    }
}
