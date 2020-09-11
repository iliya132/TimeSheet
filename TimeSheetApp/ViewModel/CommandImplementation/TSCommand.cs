using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeSheetApp.ViewModel.CommandImplementation
{
    public class TSCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action Action { get; set; }

        public TSCommand(Action action)
        {
            Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Action?.Invoke();
        }
    }
}
