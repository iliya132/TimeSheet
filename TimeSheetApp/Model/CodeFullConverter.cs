using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TimeSheetApp.Model;
namespace TimeSheetApp
{
    class CodeFullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Process)
            {
                Process ConvertedProcess = value as Process;
                return ($"{ConvertedProcess.Block_id}.{ConvertedProcess.SubBlockId}.{ConvertedProcess.id}");
            }
            else if (value is TimeSheetTable)
            {
                TimeSheetTable ConvertedTimeSheetTable = value as TimeSheetTable;
                return ($"{ConvertedTimeSheetTable.Process.Block_id}.{ConvertedTimeSheetTable.Process.SubBlockId}.{ConvertedTimeSheetTable.Process.id}");
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

    }
    class CodeDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Process)
            {
                Process proc = value as Process;
                return $"{proc.Block1.blockName}\r\n{proc.SubBlockNav.subblockName}\r\n{proc.procName}";
            } else if (value is TimeSheetTable)
            {
                Process proc = (value as TimeSheetTable).Process;
                return $"{proc.Block1.blockName}\r\n{proc.SubBlockNav.subblockName}\r\n{proc.procName}";
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

    }

    class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime && parameter is DateTime)
            {
                DateTime time = System.Convert.ToDateTime(value);
                DateTime date = System.Convert.ToDateTime(parameter);
                date.AddTicks(time.Ticks);
                return date;
            }
            else return null; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

    }
    [ValueConversion(typeof(bool), typeof(Visibility))]
    class InvertBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
            {
                throw new Exception("Неподдерживаемая операция приведения");
            }
            if ((bool)value)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

    }
}