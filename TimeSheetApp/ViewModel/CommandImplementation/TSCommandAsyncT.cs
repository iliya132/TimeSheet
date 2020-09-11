using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeSheetApp.ViewModel.CommandImplementation
{
    public class TSCommandAsync<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Func<T, Task> Action { get; set; }
        private static bool IsExecuting { get; set; }

        public TSCommandAsync(Func<T, Task> action)
        {
            Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (!IsExecuting && parameter is T t)
            {
                IsExecuting = true;
                await Action(t);
                IsExecuting = false;
            }
        }
    }
}
