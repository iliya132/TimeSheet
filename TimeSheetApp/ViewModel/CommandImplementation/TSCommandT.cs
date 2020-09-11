using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeSheetApp.ViewModel.CommandImplementation
{
    public class TSCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<T> Action { get; set; }
        public TSCommand(Action<T> action)
        {
            Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is T)
            {
                Action?.Invoke((T)parameter);
            }
            else
            {
                throw new FormatException($"Failed to convert {parameter.GetType()}");
            }
        }
    }
}
