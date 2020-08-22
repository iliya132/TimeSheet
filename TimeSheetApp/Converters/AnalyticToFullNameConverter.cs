using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp
{
    public class AnalyticToFullNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Данные не загружены";
            Analytic analytic = value as Analytic;
            return $"{analytic.LastName} {analytic.FirstName} {analytic.FatherName}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
