using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TimeSheetApp
{
    /// <summary>
    /// Interaction logic for EditForm.xaml
    /// </summary>
    public partial class EditForm : Window
    {
        public EditForm()
        {
            InitializeComponent();
            EditDatePicker.SelectedDate = EditTimeStart.Value.Value.Date;
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void EditDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dateSelected = (sender as DatePicker).SelectedDate.Value;

            if (EditTimeStart != null && EditTimeEnd != null)
            {
                EditTimeStart.Value = new DateTime(dateSelected.Year, dateSelected.Month, dateSelected.Day, EditTimeStart.Value.Value.Hour, EditTimeStart.Value.Value.Minute, 0);
                EditTimeEnd.Value = new DateTime(dateSelected.Year, dateSelected.Month, dateSelected.Day, EditTimeEnd.Value.Value.Hour, EditTimeEnd.Value.Value.Minute, 0);
            }
        }
    }
}
