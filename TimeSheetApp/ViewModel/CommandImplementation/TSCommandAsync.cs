using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeSheetApp.ViewModel.CommandImplementation
{
    public class TSCommandAsync : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private static bool IsExecuting { get; set; }
        private Func<Task> Action { get; set; }

        public TSCommandAsync(Func<Task> task)
        {
            Action = task;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (!IsExecuting)
            {
                IsExecuting = true;
                await Task.Run(() => Action());
                IsExecuting = false;
            }
        }

    }
}
