using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace TimeSheetApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        Model.IDataProvider DataProvider;
        public string TotalDurationInMinutes
        {
            get
            {
                int hours = 0;
                int mins = 0;
                int count = 0;
                
                foreach (TimeSpanClass proc in TimeSpanCol)
                {
                    count += (int)((TimeSpan)(proc.timeOut-proc.timeIn)).TotalMinutes;
                }
                hours = count / 60;
                mins = count % 60;
                return $"Общее время: {hours:00}:{mins:00}";
            }
            set { TotalDurationInMinutes = value; }
        }
        private bool _isBusy = false;
        public bool isReady { get => _isBusy; set => _isBusy = value; }
        DispatcherTimer timer1 = new DispatcherTimer();
        #region DataCollections

        private static ObservableCollection<Process> _processlistcol;
        private List<string> BlocksList;
        private List<string> SubBlockList;
        public ObservableCollection<Process> ProcessListCol { get => _processlistcol; set => _processlistcol = value; }
        private ObservableCollection<Process> _processFiltered = new ObservableCollection<Process>();
        public ObservableCollection<Process> ProcessFiltered { get => _processFiltered; set => _processFiltered = value; }
        private ObservableCollection<string> _BusinessBlock;
        public ObservableCollection<string> BusinessBlock { get => _BusinessBlock; set => _BusinessBlock = value; }
        private ObservableCollection<string> _NonBusinessBlock;
        public ObservableCollection<string> NonBusinessBlock { get => _NonBusinessBlock; set => _NonBusinessBlock = value; }
        private ObservableCollection<string> _clientWays;
        public ObservableCollection<string> ClientWays { get => _clientWays; set => _clientWays = value; }
        private ObservableCollection<TimeSheetHistoryItem> _timeSheetHistoryItemCol;
        public ObservableCollection<TimeSheetHistoryItem> TimeSheetHistoryItemCol { get => _timeSheetHistoryItemCol; set => _timeSheetHistoryItemCol = value; }
        private ObservableCollection<string> _formatList;
        public ObservableCollection<string> FormatList { get => _formatList; set => _formatList = value; }
        private ObservableCollection<string> _riskCol;
        public ObservableCollection<string> RiskCol { get => _riskCol; set => _riskCol = value; }
        private ObservableCollection<TimeSpanClass> _timeSpanCol = new ObservableCollection<TimeSpanClass>();
        public ObservableCollection<TimeSpanClass> TimeSpanCol { get => _timeSpanCol; set => _timeSpanCol = value; }
        private ObservableCollection<string> _escalations;
        public ObservableCollection<string> Escalations { get => _escalations; set => _escalations = value; }
        private ObservableCollection<Analytic> _analyticsData;
        private List<Analytic> SelectedAnalytics = new List<Analytic>();
        public ObservableCollection<Analytic> AnalyticsData
        {
            get { return _analyticsData; }
            set { _analyticsData = value; }
        }
        #endregion

        #region CurrentValues
        private UIElement _isImBoss = new UIElement();

        public UIElement IsImBoss
        {
            get { return _isImBoss; }
            set { _isImBoss = value; }
        }

        private Analytic[] _selectedAnalytic;

        public Analytic[] SelectedAnalytic
        {
            get { return _selectedAnalytic; }
            set { _selectedAnalytic = value; }
        }
        private Process _currentProcess = new Process("null", 0, 0, 0);
        public Process CurrentProcess { get => _currentProcess; set => Set(ref _currentProcess, value); }

        private Process _currentEditedProcess = new Process("null", 0, 0, 0);
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

        public Process CurrentEditedProcess
        {
            get { return _currentEditedProcess; }
            set { Set(ref _currentEditedProcess, value); }
        }
        private List<string> _reports = new List<string>() { "Активность аналитиков" };

        public List<string> Reports
        {
            get { return _reports; }
            set { _reports = value; }
        }
        private string _body = string.Empty;
        private string _subject = string.Empty;
        public string Body { get => _body; set => _body = value; }
        public string Subject { get => _subject; set => _subject = value; }

        private Analytic _user;
        public Analytic CurrentUser { get => _user; set => _user = value; }
        public int SelectedReport { get; set; }
        private DateTime _startReportDate = DateTime.Now.AddDays(-7);

        public DateTime StartReportDate
        {
            get { return new DateTime(_startReportDate.Year, _startReportDate.Month, _startReportDate.Day, 0, 0, 1); }
            set { _startReportDate = value; }
        }
        private DateTime _endReportDate = DateTime.Now;

        public DateTime EndReportDate
        {
            get { return new DateTime(_endReportDate.Year, _endReportDate.Month, _endReportDate.Day, 23, 59, 59); }
            set { _endReportDate = value; }
        }
        #endregion

        #region Commands
        public RelayCommand<Process> AddProcess { get; }
        public RelayCommand<DateTime> EditProcess { get; }
        public RelayCommand<DateTime> DeleteProcess { get; }
        public RelayCommand ReloadTimeSpan { get; }
        public RelayCommand<string> FilterProcesses { get; }
        public RelayCommand GetReport { get; }
        public RelayCommand<ICollection<object>> SelectAnalytic { get; }
        #endregion

        System.Threading.Timer checkForUpdateTimer;
        public MainViewModel(Model.IDataProvider dataProvider)
        {
            try
            {
                checkForUpdateTimer = new System.Threading.Timer(checkForUpdateMethod, null, 1000, 30000);
                
                DataProvider = dataProvider;
                AddProcess = new RelayCommand<Process>(AddProcessMethod);
                EditProcess = new RelayCommand<DateTime>(EditHistoryProcess);
                DeleteProcess = new RelayCommand<DateTime>(DeleteHistoryProcess);
                ReloadTimeSpan = new RelayCommand(UpdateTimeSpan);
                FilterProcesses = new RelayCommand<string>(FilterProcessesMethod);
                GetReport = new RelayCommand(GetReportMethod);
                SelectAnalytic = new RelayCommand<ICollection<object>>(AddAnalyticToCollection);
                CurrentUser = dataProvider.LoadAnalyticData();
                if (CurrentUser.Role != 6) IsImBoss.Visibility = Visibility.Visible;
                else IsImBoss.Visibility = Visibility.Hidden;
                

                FillDataCollections();
                LoadChoosenCount(ProcessListCol);
                isReady = true;
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void AddAnalyticToCollection(ICollection<object> analytics)
        {
            try
            {
                SelectedAnalytics.Clear();
                List<Analytic> analyticList = new List<Analytic>();
                foreach (object item in analytics)
                {
                    if (item is Analytic)
                        analyticList.Add(item as Analytic);
                }
                foreach (Analytic analytic_selected in analyticList)
                    SelectedAnalytics.Add(analytic_selected);
            }catch(Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GetReportMethod()
        {
            if (SelectedAnalytics.Count < 1)
                MessageBox.Show("Вы не выбрали аналитиков. Пожалуйста, выделите нужных Вам аналитиков и повторите попытку.", "Выберите аналитиков", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                DataProvider.GetReport(SelectedReport, SelectedAnalytics.ToArray(), StartReportDate, EndReportDate);
        }
        private void LoadChoosenCount(IEnumerable<Process> processCollection)
        {
            foreach (Process process in processCollection)
            {
                process.ChoosenCounter = Model.LocalWorker.ChoosenCounter(process.Id);
            }
        }
        private void checkForUpdateMethod(object obj)
        {
            if (DataProvider.ForcedToQuit())
                Environment.Exit(0);
        }
        private void FilterProcessesMethod(string filterText)
        {
                ProcessFiltered?.Clear();
                if (string.IsNullOrWhiteSpace(filterText))
                {
                    foreach (Process proc in ProcessListCol)
                        ProcessFiltered.Add(proc);
                    return;
                }
                foreach (Process process in ProcessListCol)
                {
                    string codeFull = $"{process.Block}.{process.SubBlock}.{process.Id}";
                    if (process.Name.ToLower().IndexOf(filterText.ToLower()) > -1 || codeFull.IndexOf(filterText) > -1)
                    {
                        ProcessFiltered.Add(process);
                    }
                }

        }

        private void FillDataCollections()
        {
            DataProvider.Connection.Open();
            ProcessListCol = DataProvider.GetProcesses();
            BusinessBlock = DataProvider.GetBusinessBlocks();
            NonBusinessBlock = DataProvider.GetSupports();
            ClientWays = DataProvider.GetClientWays();
            TimeSheetHistoryItemCol = DataProvider.GetTimeSheetItem();
            FormatList = DataProvider.GetFormat();
            Escalations = DataProvider.GetEscalation();
            RiskCol = DataProvider.GetRisks();
            BlocksList = DataProvider.GetBlocksList();
            SubBlockList = DataProvider.GetSubBlocksList();
            FilterProcessesMethod(string.Empty);
            DataProvider.Connection.Close();
            AnalyticsData = DataProvider.GetMyAnalyticsData(CurrentUser);
            UpdateTimeSpan();

        }
        private void UpdateTimeSpan()
        {
            isReady = false;
            DataProvider.LoadTimeSpan(CurrentDate, CurrentUser, TimeSpanCol);
            isReady = true;
            RaisePropertyChanged("TotalDurationInMinutes");
        }
        private void AddProcessMethod(Process proc)
        {

            isReady = false;
            if (proc.Id == 0)
            {
                MessageBox.Show("Не выбран процесс.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (proc.BodyNeeded && string.IsNullOrWhiteSpace(Body))
            {
                MessageBox.Show("Для выбранного процесса поле 'Комментарий' является обязательным", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (IsTimeCollision(CurrentTimeStart, CurrentTimeEnd))
            {
                MessageBox.Show("Указанное время пересекается с другой активностью или заполнено не корректно. Выберите другое время", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            proc.Body = Body;
            proc.Subject = Subject;
            proc.ProcDate = CurrentDate;
            proc.DateTimeStart = CurrentTimeStart;
            proc.DateTimeEnd = CurrentTimeEnd;
            proc.DateTimeStart.AddSeconds(DateTime.Now.Second);
            TimeSpanCol.Add(new TimeSpanClass(proc.DateTimeStart, proc.DateTimeEnd, proc.Name, proc.Subject, proc.Code, proc.CodeFull));
            RaisePropertyChanged("TotalDurationInMinutes");
            proc.Analytic = CurrentUser.ID;
            DataProvider.AddActivity(proc);
            try
            {
                Model.LocalWorker.StoreSelection(
                   new Model.Selection(proc.BusinessBlock, proc.Support, proc.ClientWay, proc.Escalation, proc.Format, proc.Risk) { ProcessID = proc.Id });
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            proc.ChoosenCounter++;
            CurrentProcess.DateTimeStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            CurrentProcess.DateTimeEnd = CurrentProcess.DateTimeStart.AddMinutes(15);
            isReady = true;
        }

        private bool IsTimeCollision(DateTime start, DateTime end)
        {
            if (start > end)
                return true;
            foreach (TimeSpanClass historyProcess in TimeSpanCol)
            {
                if ((start.Hour==historyProcess.timeIn.Hour && start.Minute >= historyProcess.timeIn.Minute && start.Minute+1 < historyProcess.timeOut.Minute) ||
                    (end.Hour==historyProcess.timeIn.Hour && end.Minute > historyProcess.timeIn.Minute && end.Minute < historyProcess.timeOut.Minute))
                    return true;
            }
            return false;
        }
        private void EditHistoryProcess(DateTime timeStart)
        {
            if (timeStart.Year > 100)
            {
                CurrentEditedProcess = DataProvider.LoadHistoryProcess(timeStart, CurrentUser);

                EditViewModel editModel = SimpleIoc.Default.GetInstance<EditViewModel>();
                editModel.CurrentEDProcess = CurrentEditedProcess.Clone() as Process;
                editModel.CurrentEDProcess.Analytic = CurrentUser.ID;
                editModel.CurrentDate = CurrentEditedProcess.ProcDate;
                editModel.CurrentTimeStart = CurrentEditedProcess.DateTimeStart;
                editModel.CurrentTimeEnd = CurrentEditedProcess.DateTimeEnd;
                editModel.ProcessListCol = ProcessListCol;
                editModel.GetIndex();
                for (int i = 0; i < ProcessListCol.Count; i++)
                {
                    if (ProcessListCol[i].Id == CurrentEditedProcess.Id)
                    {
                        editModel.EditedProcessID = i;
                        break;
                    }
                }
                EditForm modal = new EditForm();
                if ((bool)modal.ShowDialog())
                    if (DataProvider.UpdateProcess(CurrentEditedProcess, editModel.CurrentEDProcess) != -1)
                    {
                        foreach (TimeSpanClass timeSpan in TimeSpanCol)
                            if (timeSpan.timeIn == timeStart)
                            {
                                timeSpan.timeIn = editModel.CurrentEDProcess.DateTimeStart;
                                timeSpan.timeOut = editModel.CurrentEDProcess.DateTimeEnd;
                                timeSpan.Subject = editModel.CurrentEDProcess.Subject;
                                timeSpan.Code = editModel.CurrentEDProcess.Code;
                                timeSpan.CodeFull = editModel.CurrentEDProcess.CodeFull;
                                timeSpan.processName = editModel.CurrentEDProcess.Name;
                            }
                        UpdateTimeSpan();


                    }
                    else
                        MessageBox.Show("При обновлении данных возникла ошибка. Данные небыли обновлены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteHistoryProcess(DateTime timeStart)
        {
            isReady = false;
            if (DataProvider.DeleteProcess(timeStart, CurrentUser) != -1)
            {
                foreach (TimeSpanClass timeSpan in TimeSpanCol)
                {
                    if (timeSpan.timeIn == timeStart)
                    {
                        TimeSpanCol.Remove(timeSpan);
                        RaisePropertyChanged("TotalDurationInMinutes");
                        break;
                    }
                }
            }
            else
                MessageBox.Show("При удалении данных возникла ошибка. Данные небыли удалены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            isReady = true;
        }

    }
}