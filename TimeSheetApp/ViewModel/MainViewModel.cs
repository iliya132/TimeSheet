using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using TimeSheetApp.Model;
namespace TimeSheetApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public IEFDataProvider EFDataProvider;
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
        private ObservableCollection<TimeSheetTable> _timeSheetColl = new ObservableCollection<TimeSheetTable>();
        public ObservableCollection<TimeSheetTable> TimeSheetColl { get => _timeSheetColl; set => _timeSheetColl = value; }
        private ObservableCollection<string> _escalations;
        public ObservableCollection<string> Escalations { get => _escalations; set => _escalations = value; }
        private ObservableCollection<Model.Analytic> _analyticsData;
        private List<Model.Analytic> SelectedAnalytics = new List<Model.Analytic>();
        public ObservableCollection<Model.Analytic> AnalyticsData
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
        private Process _currentProcess = new Process();
        public Process CurrentProcess { get => _currentProcess; set => Set(ref _currentProcess, value); }

        private Process _currentEditedProcess = new Process();
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
        private List<string> _reports = new List<string>() { "���������� ����������" };

        public List<string> Reports
        {
            get { return _reports; }
            set { _reports = value; }
        }
        private string _body = string.Empty;
        private string _subject = string.Empty;
        public string Body { get => _body; set => _body = value; }
        public string Subject { get => _subject; set => _subject = value; }

        private Model.Analytic _user;
        public Model.Analytic CurrentUser { get => _user; set => _user = value; }
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
        [PreferredConstructor]
        public MainViewModel(Model.IEFDataProvider dataProvider)
        {
            EFDataProvider = dataProvider;
            FillDataCollections();
            AddProcess = new RelayCommand<Process>(AddProcessMethod);
            EditProcess = new RelayCommand<DateTime>(EditHistoryProcess);
            //DeleteProcess = new RelayCommand<DateTime>(DeleteHistoryProcess);
            ReloadTimeSpan = new RelayCommand(UpdateTimeSpan);
            FilterProcesses = new RelayCommand<string>(FilterProcessesMethod);
            GetReport = new RelayCommand(GetReportMethod);
            SelectAnalytic = new RelayCommand<ICollection<object>>(AddAnalyticToCollection);
        }
        //public MainViewModel(Model.IDataProvider dataProvider)
        //{
        //    try
        //    {
        //        checkForUpdateTimer = new System.Threading.Timer(checkForUpdateMethod, null, 1000, 30000);

        //        DataProvider = dataProvider;

        //        CurrentUser = dataProvider.LoadAnalyticData();
        //        if (CurrentUser.Role != 6) IsImBoss.Visibility = Visibility.Visible;
        //        else IsImBoss.Visibility = Visibility.Hidden;


        //        FillDataCollections();
        //        LoadChoosenCount(ProcessListCol);
        //        isReady = true;
        //    } catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }
        //}
        private void AddAnalyticToCollection(ICollection<object> analytics)
        {
            try
            {
                SelectedAnalytics.Clear();
                List<Model.Analytic> analyticList = new List<Model.Analytic>();
                foreach (object item in analytics)
                {
                    if (item is Model.Analytic)
                        analyticList.Add(item as Model.Analytic);
                }
                foreach (Model.Analytic analytic_selected in analyticList)
                    SelectedAnalytics.Add(analytic_selected);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GetReportMethod()
        {

        }
        private void LoadChoosenCount(IEnumerable<Process> processCollection)
        {

        }
        private void checkForUpdateMethod(object obj)
        {
            if (EFDataProvider.ForcedToQuit())
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
                string codeFull = $"{process.Block_id}.{process.SubBlockId}.{process.id}";
                if (process.procName.ToLower().IndexOf(filterText.ToLower()) > -1 || codeFull.IndexOf(filterText) > -1)
                {
                    ProcessFiltered.Add(process);
                }
            }

        }

        private void FillDataCollections()
        {
            CurrentUser = EFDataProvider.LoadAnalyticData();
            ProcessListCol = EFDataProvider.GetProcesses();
            BusinessBlock = EFDataProvider.GetBusinessBlocks();
            NonBusinessBlock = EFDataProvider.GetSupports();
            ClientWays = EFDataProvider.GetClientWays();
            TimeSheetHistoryItemCol = EFDataProvider.GetTimeSheetItem();
            FormatList = EFDataProvider.GetFormat();
            Escalations = EFDataProvider.GetEscalation();
            RiskCol = EFDataProvider.GetRisks();
            BlocksList = EFDataProvider.GetBlocksList();
            SubBlockList = EFDataProvider.GetSubBlocksList();
            FilterProcessesMethod(string.Empty);
            AnalyticsData = EFDataProvider.GetMyAnalyticsData(CurrentUser);
            UpdateTimeSpan();

        }
        private void UpdateTimeSpan()
        {
            isReady = false;
            TimeSheetColl = EFDataProvider.LoadTimeSpan(CurrentDate, CurrentUser);
            isReady = true;
            RaisePropertyChanged("TotalDurationInMinutes");
        }
        private void AddProcessMethod(Process proc)
        {

        }

        private bool IsTimeCollision(DateTime start, DateTime end)
        {
            return false;
            //if (start > end)
            //    return true;
            //foreach (TimeSpanClass historyProcess in TimeSheetColl)
            //{
            //    if ((start.Hour==historyProcess.timeIn.Hour && start.Minute >= historyProcess.timeIn.Minute && start.Minute+1 < historyProcess.timeOut.Minute) ||
            //        (end.Hour==historyProcess.timeIn.Hour && end.Minute > historyProcess.timeIn.Minute && end.Minute < historyProcess.timeOut.Minute))
            //        return true;
            //}
            //return false;
        }
        private void EditHistoryProcess(DateTime timeStart)
        {
            //if (timeStart.Year > 100)
            //{
            //    CurrentEditedProcess = EFDataProvider.LoadHistoryProcess(timeStart, CurrentUser);

            //    EditViewModel editModel = SimpleIoc.Default.GetInstance<EditViewModel>();
            //    editModel.CurrentEDProcess = CurrentEditedProcess;
            //    editModel.GetIndex();
            //    for (int i = 0; i < ProcessListCol.Count; i++)
            //    {
            //        if (ProcessListCol[i].id == CurrentEditedProcess.id)
            //        {
            //            editModel.EditedProcessID = i;
            //            break;
            //        }
            //    }
            //    EditForm modal = new EditForm();
            //    if ((bool)modal.ShowDialog())
            //        if (EFDataProvider.UpdateProcess(CurrentEditedProcess, editModel.CurrentEDProcess) != -1)
            //        {
            //            foreach (TimeSpanClass timeSpan in TimeSheetColl)
            //                if (timeSpan.timeIn == timeStart)
            //                {

            //                    timeSpan.processName = editModel.CurrentEDProcess.procName;
            //                }
            //            UpdateTimeSpan();


            //        }
            //        else
            //            MessageBox.Show("��� ���������� ������ �������� ������. ������ ������ ���������", "������", MessageBoxButton.OK, MessageBoxImage.Error);
            //    //}
            //}
            //private void DeleteHistoryProcess(DateTime timeStart)
            //{
            //    isReady = false;

            //    if (EFDataProvider.DeleteProcess(timeStart, CurrentUser) != -1)
            //    {
            //        foreach (TimeSpanClass timeSpan in TimeSheetColl)
            //        {
            //            if (timeSpan.timeIn == timeStart)
            //            {
            //                TimeSheetColl.Remove(timeSpan);
            //                RaisePropertyChanged("TotalDurationInMinutes");
            //                break;
            //            }
            //        }
            //    }
            //    else
            //        MessageBox.Show("��� �������� ������ �������� ������. ������ ������ �������", "������", MessageBoxButton.OK, MessageBoxImage.Error);
            //    isReady = true;
            //}

        }
    }
}