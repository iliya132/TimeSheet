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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TimeSheetApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model.Selection selection;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(TimeSpanListView.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("timeIn", ListSortDirection.Ascending));
                CollectionView viewProcesses = (CollectionView)CollectionViewSource.GetDefaultView(ProcessList.ItemsSource);
                viewProcesses.SortDescriptions.Add(new SortDescription("ChoosenCounter", ListSortDirection.Descending));
                viewProcesses.SortDescriptions.Add(new SortDescription("Block", ListSortDirection.Ascending));
                viewProcesses.SortDescriptions.Add(new SortDescription("SubBlock", ListSortDirection.Ascending));
                viewProcesses.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                PropertyGroupDescription propertyGroupDescription = new PropertyGroupDescription("ProcTypeName");
                viewProcesses.GroupDescriptions.Add(propertyGroupDescription);
            }catch(Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ProcessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selection = Model.LocalWorker.GetSelection((ProcessList.SelectedItem as Process).Id);
                businessCombo.SelectedIndex = selection.BusinessBlockSelected;
                supportCombo.SelectedIndex = selection.SupportSelected;
                escalationCombo.SelectedIndex = selection.EscalationSelected;
                formatCombo.SelectedIndex = selection.FormatSelected;
                clientWaysCombo.SelectedIndex = selection.ClientWaySelected;
                riskCombo.SelectedIndex = selection.RiskSelected;
            }
            catch { }
        }
        private void Expander_Loaded(object sender, RoutedEventArgs e)
        {

            var expander = e.Source as Expander;
            if (expander == null)
                return;
            expander.IsExpanded = expander.Tag.ToString() == "Организация";


        }
    }
}
