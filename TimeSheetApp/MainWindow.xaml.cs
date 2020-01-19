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
        Selection selection;
        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleError((Exception) e.ExceptionObject);
            InitializeComponent();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(TimeSpanListView.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("timeStart", ListSortDirection.Ascending));
            CollectionView viewProcesses = (CollectionView)CollectionViewSource.GetDefaultView(ProcessList.ItemsSource);
            //viewProcesses.SortDescriptions.Add(new SortDescription("ChoosenCounter", ListSortDirection.Descending));
            viewProcesses.SortDescriptions.Add(new SortDescription("Block_id", ListSortDirection.Ascending));
            viewProcesses.SortDescriptions.Add(new SortDescription("SubBlockId", ListSortDirection.Ascending));
            viewProcesses.SortDescriptions.Add(new SortDescription("id", ListSortDirection.Ascending));
            PropertyGroupDescription propertyGroupDescription = new PropertyGroupDescription("ProcessType1.ProcessTypeName");
            viewProcesses.GroupDescriptions.Add(propertyGroupDescription);
            TimeIn.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
            Timeout.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 20, 0);
            DateBox.SelectedDate = DateTime.Now;
            
        }
        private void HandleError(Exception exceptionObject)
        {
            System.Windows.MessageBox.Show(exceptionObject.Message, "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ProcessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selection = LocalWorker.GetSelection((ProcessList.SelectedItem as Process).id);
            }
            catch { }
        }
        /// <summary>
        /// Свернуть не нужную категорию
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

        private void TimeIn_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.MessageBox.Show((sender as TimePicker).Value.ToString());
        }

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
        private void DateBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dateSelected = (sender as DatePicker).SelectedDate.Value;

            if (TimeIn != null && Timeout != null)
            {
                TimeIn.Value = new DateTime(dateSelected.Year, dateSelected.Month, dateSelected.Day, TimeIn.Value.Value.Hour, TimeIn.Value.Value.Minute, 0);
                Timeout.Value = new DateTime(dateSelected.Year, dateSelected.Month, dateSelected.Day, Timeout.Value.Value.Hour, Timeout.Value.Value.Minute, 0);
            }
        }

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
    }
}
