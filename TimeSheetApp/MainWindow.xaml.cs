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
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(TimeSpanListView.ItemsSource);
            //view.SortDescriptions.Add(new SortDescription("timeIn", ListSortDirection.Ascending));
            CollectionView viewProcesses = (CollectionView)CollectionViewSource.GetDefaultView(ProcessList.ItemsSource);
            //viewProcesses.SortDescriptions.Add(new SortDescription("ChoosenCounter", ListSortDirection.Descending));
            viewProcesses.SortDescriptions.Add(new SortDescription("Block", ListSortDirection.Ascending));
            viewProcesses.SortDescriptions.Add(new SortDescription("SubBlock", ListSortDirection.Ascending));
            viewProcesses.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            PropertyGroupDescription propertyGroupDescription = new PropertyGroupDescription("ProcessType1.ProcessTypeName");
            viewProcesses.GroupDescriptions.Add(propertyGroupDescription);
        }

        private void HandleError(Exception exceptionObject)
        {
            MessageBox.Show(exceptionObject.Message, "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        private void ProcessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selection = LocalWorker.GetSelection((ProcessList.SelectedItem as Process).id);
                businessCombo.SelectedIndex = selection.BusinessBlockSelected;
                supportCombo.SelectedIndex = selection.SupportSelected;
                escalationCombo.SelectedIndex = selection.EscalationSelected;
                formatCombo.SelectedIndex = selection.FormatSelected;
                clientWaysCombo.SelectedIndex = selection.ClientWaySelected;
                riskCombo.SelectedIndex = selection.RiskSelected;
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
    }
}
