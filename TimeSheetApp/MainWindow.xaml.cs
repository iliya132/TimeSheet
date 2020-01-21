using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TimeSheetApp.Model;
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
            InitializeComponent();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(TimeSpanListView.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("timeStart", ListSortDirection.Ascending));
            CollectionView viewProcesses = (CollectionView)CollectionViewSource.GetDefaultView(ProcessList.ItemsSource);
            TimeIn.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            Timeout.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 20, 0);
            DateBox.SelectedDate = DateTime.Now;
            businessCombo.SelectedItem = businessCombo.Items[0];
            supportCombo.SelectedItem = supportCombo.Items[0];
            escalationCombo.SelectedItem = escalationCombo.Items[0];
            riskCombo.SelectedItem = riskCombo.Items[0];
        }
        private void HandleError(Exception exceptionObject)
        {
            System.Windows.MessageBox.Show(exceptionObject.Message, "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            DateTime dateSelected = (sender as DatePicker).SelectedDate.Value;

            if (TimeIn != null && Timeout != null)
            {
                TimeIn.Value = new DateTime(dateSelected.Year, dateSelected.Month, dateSelected.Day, TimeIn.Value.Value.Hour, TimeIn.Value.Value.Minute, 0);
                Timeout.Value = new DateTime(dateSelected.Year, dateSelected.Month, dateSelected.Day, Timeout.Value.Value.Hour, Timeout.Value.Value.Minute, 0);
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
            System.Diagnostics.Process.Start("TimeSheetHelp.chm");
        }
    }
}
