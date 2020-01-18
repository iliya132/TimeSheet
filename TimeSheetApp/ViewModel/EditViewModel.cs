using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TimeSheetApp.Model;

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
            CurrentEDProcess.id = ProcessListCol[id].id;
            CurrentEDProcess.procName = ProcessListCol[id].procName;
            
            
            GetIndex();
        }
        public void GetIndex()
        {
            for (int i = 0; i < ProcessListCol.Count; i++)
            {
                if (ProcessListCol[i].id == CurrentEDProcess.id)
                {
                    SelectedProcessID = i;
                }
            }
        }
        private void UpdateDateTimeMethod()
        {
        }
    }
}
