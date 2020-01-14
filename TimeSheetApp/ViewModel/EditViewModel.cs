using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.ViewModel
{
    public class EditViewModel : ViewModelBase
    {
        public int SelectedProcessID{get;set;}
        private Process _currentProcess;
        private static ObservableCollection<Process> _processlistcol;
        public ObservableCollection<Process> ProcessListCol { get => _processlistcol; set => _processlistcol = value; }
        public Process CurrentEDProcess
        {
            get => _currentProcess; set => _currentProcess = value;
        }
        DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate { get => _currentDate; set => _currentDate = value; }
        private DateTime _currentTimeStart = DateTime.Now;
        public DateTime CurrentTimeStart
        {
            get => new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, _currentTimeStart.Hour, _currentTimeStart.Minute, _currentTimeStart.Second);
            set => _currentTimeStart = value;
        }
        private DateTime _currentTimeEnd = DateTime.Now.AddMinutes(15);
        public DateTime CurrentTimeEnd
        {
            get => new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, _currentTimeEnd.Hour, _currentTimeEnd.Minute, _currentTimeEnd.Second);
            set => _currentTimeEnd = value;
        }
        public int EditedProcessID { get; set; }
        public RelayCommand UpdateTime { get; set; }
        public RelayCommand<int> ChangeProcess { get; set; }

        public EditViewModel()
        {
            UpdateTime = new RelayCommand(UpdateDateTimeMethod);
            ChangeProcess = new RelayCommand<int>(ChangeProcessMethod);
        }
        private void ChangeProcessMethod(int id)
        {
            CurrentEDProcess.Id = ProcessListCol[id].Id;
            CurrentEDProcess.Name = ProcessListCol[id].Name;
            CurrentEDProcess.Code = ProcessListCol[id].Code;
            CurrentEDProcess.CodeFull = ProcessListCol[id].CodeFull;
            GetIndex();
        }
        public void GetIndex()
        {
            for (int i = 0; i < ProcessListCol.Count; i++)
            {
                if (ProcessListCol[i].Id == CurrentEDProcess.Id)
                {
                    SelectedProcessID = i;
                }
            }
        }
        private void UpdateDateTimeMethod()
        {
            CurrentEDProcess.ProcDate = CurrentDate;
            CurrentEDProcess.DateTimeStart = CurrentTimeStart;
            CurrentEDProcess.DateTimeEnd = CurrentTimeEnd;
        }
    }
}
