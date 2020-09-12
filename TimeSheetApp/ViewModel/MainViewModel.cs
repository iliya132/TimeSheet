using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TimeSheetApp.Model;
using TimeSheetApp.Model.EntitiesBase;
using TimeSheetApp.ViewModel.CommandImplementation;

using Process = TimeSheetApp.Model.EntitiesBase.Process;

namespace TimeSheetApp.ViewModel
{
    public class MainViewModel :INotifyPropertyChanged
    {
        public IDataProvider EFDataProvider;

        #region DataCollections

        #region Список процессов для выбора
        public ObservableCollection<Process> AllProcesses { get; set; }
        public ObservableCollection<Process> UserFilteredProcesses { get; set; }
        #endregion

        #region Аллокации для выбора
        public List<BusinessBlock> BusinessBlocks { get; set; }
        public List<Supports> Supports { get; set; }
        public List<ClientWays> ClientWays { get; set; }
        public List<Formats> Formats { get; set; }
        public List<Risk> Risks { get; set; }
        public List<Escalation> Escalations { get; set; }
        #endregion

        #region Исторические записи
        private ObservableCollection<TimeSheetTable> _todayRecords = new ObservableCollection<TimeSheetTable>();
        public ObservableCollection<TimeSheetTable> TodayRecords
        {
            get => _todayRecords;
            set => _todayRecords = value;
        }
        #endregion

        #region Список сотрудников в подчинении
        private ObservableCollection<StructuredAnalytic> _subordinatedAnalytics;
        public ObservableCollection<StructuredAnalytic> SubordinatedAnalytics
        {
            get
            {
                return _subordinatedAnalytics;
            }
            set
            {
                _subordinatedAnalytics = value;
                RaisePropertyChanged(nameof(SubordinatedAnalytics));
            }
        }
        private ObservableCollection<Node> _subordinatedAnalyticsNodes = new ObservableCollection<Node>();
        public ObservableCollection<Node> SubordinatedAnalyticNodes
        {
            get
            {
                return _subordinatedAnalyticsNodes;
            }
            set
            {
                _subordinatedAnalyticsNodes = value;
            }
        }
        private ObservableCollection<Analytic> _currentUserTeam;
        public ObservableCollection<Analytic> CurrentUserTeam
        {
            get
            {
                return _currentUserTeam;
            }
            set
            {
                _currentUserTeam = value;
                RaisePropertyChanged(nameof(CurrentUserTeam));
            }
        }
        #endregion

        #endregion

        #region CurrentValues

        readonly List<TimeSheetTable> SelectedRecords = new List<TimeSheetTable>();
        readonly List<TimeSheetTable> CopiedRecords = new List<TimeSheetTable>();
        public TimeSpan TimeSelectorMin
        {
            get
            {
                return NewRecord.TimeStart.TimeOfDay;
            }
            set
            {
                NewRecord.TimeStart = new DateTime(NewRecord.TimeStart.Year, NewRecord.TimeStart.Month, NewRecord.TimeStart.Day, value.Hours, value.Minutes, value.Seconds);
                RaisePropertyChanged(nameof(NewRecord));
            }
        }

        public TimeSpan TimeSelectorMax
        {
            get
            {
                return NewRecord.TimeEnd.TimeOfDay;
            }
            set
            {
                NewRecord.TimeEnd = new DateTime(NewRecord.TimeEnd.Year, NewRecord.TimeEnd.Month, NewRecord.TimeEnd.Day, value.Hours, value.Minutes, value.Seconds);
                RaisePropertyChanged(nameof(NewRecord));
            }
        }

        #region editForm multiChoice
        private ObservableCollection<Risk> riskChoiceCollection = new ObservableCollection<Risk>();
        public ObservableCollection<Risk> RiskChoiceCollection { get => riskChoiceCollection; set => riskChoiceCollection = value; }
        private ObservableCollection<Escalation> _escalationsChoiceCollection = new ObservableCollection<Escalation>();
        public ObservableCollection<Escalation> EscalationsChoiceCollection { get => _escalationsChoiceCollection; set => _escalationsChoiceCollection = value; }
        private ObservableCollection<BusinessBlock> _businessBlockChoiceCollection = new ObservableCollection<BusinessBlock>();
        public ObservableCollection<BusinessBlock> BusinessBlockChoiceCollection { get => _businessBlockChoiceCollection; set => _businessBlockChoiceCollection = value; }
        private ObservableCollection<Supports> _supportsChoiceCollection = new ObservableCollection<Supports>();
        public ObservableCollection<Supports> SupportsChoiceCollection { get => _supportsChoiceCollection; set => _supportsChoiceCollection = value; }
        #endregion
        private bool _isReady = true;
        public bool IsReady
        {
            get
            {
                return _isReady;
            }
            set
            {
                _isReady = value;
                RaisePropertyChanged(nameof(IsReady));
            }
        }
        private string _ReportBtnName = "Выгрузить отчет";
        public  string ReportBtnName
        {
            get => _ReportBtnName;
            set
            {
                _ReportBtnName = value;
                RaisePropertyChanged(nameof(ReportBtnName));
            }
        }

        private string _title = "TimeSheet";
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        readonly Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;

        public double LastMonthTimeSpent
        {
            get
            {
                DateTime thisMonthFirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime thisMonthLastDay = thisMonthFirstDay.AddMonths(1).AddDays(-1);
                DateTime lastMonthFirstDay = thisMonthFirstDay.AddMonths(-1);
                DateTime lastMonthLastDay = thisMonthLastDay.AddMonths(-1);
                return EFDataProvider.GetTimeSpent(CurrentUser.UserName, lastMonthFirstDay, lastMonthLastDay);
            }
        }

        public int LastMonthDaysWorked
        {
            get
            {
                DateTime thisMonthFirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime thisMonthLastDay = thisMonthFirstDay.AddMonths(1).AddDays(-1);
                DateTime lastMonthFirstDay = thisMonthFirstDay.AddMonths(-1);
                DateTime lastMonthLastDay = thisMonthLastDay.AddMonths(-1);
                return EFDataProvider.GetDaysWorkedCount(CurrentUser, lastMonthFirstDay, lastMonthLastDay);
            }
        }

        public double LastWeekTimeSpent
        {
            get
            {
                DateTime thisWeekFirstDay = DateTime.Today.AddDays(-7);
                return EFDataProvider.GetTimeSpent(CurrentUser.UserName, thisWeekFirstDay, DateTime.Today);
            }
        }

        public double LastWeekDaysWorked
        {
            get
            {
                DateTime thisWeekFirstDay = DateTime.Now.AddDays(-7);
                return EFDataProvider.GetDaysWorkedCount(CurrentUser, thisWeekFirstDay, DateTime.Now);
            }
        }

        public bool IgnoreSubjectTextChange { get; set; }
        /// <summary>
        /// подсказки для ввода текста в поле Тема
        /// </summary>

        private List<string> subjectsFromDB;
        private ObservableCollection<string> subjectHints = new ObservableCollection<string>();
        public ObservableCollection<string> SubjectHints { get => subjectHints; set => subjectHints = value; }

        /// <summary>
        /// Общая длительность записей в коллекции HistoryRecords
        /// </summary>
        //public Visibility ReportAcess { get; set; }

        #region Добавление нового процесса
        /// <summary>
        /// Текущий(добавляемый) процесс
        /// </summary>
        private TimeSheetTable _NewRecord = new TimeSheetTable();
        public TimeSheetTable NewRecord
        {
            get { return _NewRecord; }
            set { _NewRecord = value; }
        }
        private TimeSheetTable _editedRecord = new TimeSheetTable();
        public TimeSheetTable EditedRecord
        {
            get { return _editedRecord; }
            set { _editedRecord = value; }
        }

        private ClientWays _currentClientWays = new ClientWays();
        public ClientWays CurrentClientWays
        {
            get { return _currentClientWays; }
            set
            {
                NewRecord.ClientWaysId = value.Id;
                _currentClientWays = value;
            }
        }

        private Formats _currentFormat = new Formats();
        public Formats CurrentFormat
        {
            get { return _currentFormat; }
            set
            {
                NewRecord.FormatsId = value.Id;
                _currentFormat = value;
            }
        }


        DominoWorker worker;
        private CalendarItem _currentCalendarItem = new CalendarItem();
        public CalendarItem CurrentCalendarItem { get => _currentCalendarItem; set => _currentCalendarItem = value; }
        private int _calendarItemsCount;

        public int CalendarItemsCount
        {
            get { return _calendarItemsCount; }
            set
            {
                _calendarItemsCount = value;
                RaisePropertyChanged(nameof(CalendarItemsCount));
            }
        }
        private bool isCalendarLoading = true;
        public bool IsCalendarLoading
        {
            get => isCalendarLoading;
            set
            {
                isCalendarLoading = value;
                RaisePropertyChanged(nameof(LoadingVisibilityInverted));
                RaisePropertyChanged(nameof(LoadingVisibility));
                RaisePropertyChanged(nameof(IsCalendarReady));
                RaisePropertyChanged(nameof(IsCalendarLoading));
            }

        }
        public bool IsCalendarReady
        {
            get => !IsCalendarLoading;
        }
        public Visibility LoadingVisibility
        {
            get
            {
                if (IsCalendarLoading) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }
        public Visibility LoadingVisibilityInverted
        {
            get
            {
                if (!IsCalendarLoading) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }
        #endregion

        #region Редактирование процесса

        private Process _currentEditedRecord = new Process();
        public Process CurrentEditedRecord
        {
            get { return _currentEditedRecord; }
            set { _currentEditedRecord = value; }
        }
        private DateTime InitalTimeStart { get; set; }
        private DateTime InitalTimeEnd { get; set; }

        private bool _isTimeCorrect = true;
        public bool IsTimeCorrect { get => _isTimeCorrect; set => _isTimeCorrect = value; }

        #endregion

        #region Формирование отчета
        private List<string> _reportsAvailable = new List<string>();

        public List<string> ReportsAvailable
        {
            get { return _reportsAvailable; }
            set { _reportsAvailable = value; }
        }

        private ObservableCollection<Analytic> _selectedAnalytics = new ObservableCollection<Analytic>();
        public ObservableCollection<Analytic> SelectedAnalytics { get => _selectedAnalytics; set => _selectedAnalytics = value; }
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

        public TimeSpan TotalDurationInMinutes
        {
            get
            {
                TimeSpan totalSpan = new TimeSpan();
                foreach (TimeSheetTable record in TodayRecords)
                {
                    totalSpan += record.TimeEnd - record.TimeStart;
                }
                return totalSpan;
            }
        }
        DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate { get => _currentDate; set => _currentDate = value; }
        private Analytic _currentUser = new Analytic();
        public Analytic CurrentUser { get => _currentUser; set => _currentUser = value; }
        public string CurrentUserFullName
        {
            get
            {
                return $"{CurrentUser.LastName} {CurrentUser.FirstName} {CurrentUser.FatherName}";
            }
        }
        private ObservableCollection<CalendarItem> _calendarItems = new ObservableCollection<CalendarItem>();

        public ObservableCollection<CalendarItem> CalendarItems
        {
            get { return _calendarItems; }
            set { _calendarItems = value; }
        }

        public List<(TimeSpan, TimeSpan)> BusyTime
        {
            get
            {
                List<(TimeSpan, TimeSpan)> exportValue = new List<(TimeSpan, TimeSpan)>();
                foreach (TimeSheetTable record in TodayRecords)
                {
                    exportValue.Add((record.TimeStart.TimeOfDay, record.TimeEnd.TimeOfDay));
                }
                return exportValue;
            }
        }

        private Timer loadCalendarTimer;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Commands
        public TSCommandAsync<TimeSheetTable> AddProcess { get; set; }
        public TSCommandAsync<TimeSheetTable> EditProcess { get; set; }
        public TSCommandAsync<TimeSheetTable> DeleteProcess { get; set; }
        public TSCommandAsync ReloadTimeSheet { get; set; }
        public TSCommand<string> FilterProcesses { get; set; }
        public TSCommandAsync<Process> LoadSelectionForSelectedProcess { get; set; }
        public TSCommandAsync ReloadHistoryRecords { get; set; }
        public TSCommandAsync CheckTimeForIntersection { get; set; }
        public TSCommandAsync GetReport { get; set; }
        public TSCommand<StructuredAnalytic> SelectAnalytic { get; set; }
        public TSCommand<StructuredAnalytic> UnselectAnalytic { get; set; }
        public TSCommand ReportSelectionStore { get; set; }
        public TSCommand SelectCalendarItem { get; set; }
        public TSCommandAsync<string> FinilizeEditUserName { get; set; }
        public TSCommand<object> StoreSelectedRecords { get; set; }
        public TSCommand CopyRecords { get; set; }
        public TSCommand PasteRecords { get; set; }
        public TSCommand TimeSelected { get; set; }


        #endregion


        public MainViewModel(IDataProvider dataProvider)
        {
            EFDataProvider = dataProvider;
            Initialize();
            
        }

        private async Task Initialize()
        {
            IsReady = false;
            Title = "TimeSheet";
            QuitIfStartedFromServer();
            InitializeEvents();
            InitializeCommads();
            await FillDataCollectionsAsync();
            NewRecord.Analytic = CurrentUser;
            NewRecord.AnalyticId = CurrentUser.Id;
            loadCalendarTimer = new Timer(TimerTick, null, 0, Timeout.Infinite);
            GenerateNodes();
            await UpdateTimeSpanAsync();
            IsReady = true;
        }

        private void InitializeEvents()
        {
            TodayRecords.CollectionChanged += TodayRecords_CollectionChanged;
        }

        private void TodayRecords_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(TotalDurationInMinutes));
            RaisePropertyChanged(nameof(BusyTime));
        }

        private void InitializeCommads()
        {
            AddProcess = new TSCommandAsync<TimeSheetTable>(AddRecordMethod);
            EditProcess = new TSCommandAsync<TimeSheetTable>(EditHistoryProcessAsync);
            DeleteProcess = new TSCommandAsync<TimeSheetTable>(DeleteHistoryRecordAsync);
            ReloadTimeSheet = new TSCommandAsync(UpdateTimeSpanAsync);
            CheckTimeForIntersection = new TSCommandAsync(CheckTimeForIntesectionMethodAsync);
            LoadSelectionForSelectedProcess = new TSCommandAsync<Process>(SetupSelectionAsLastTimeAsync);
            FilterProcesses = new TSCommand<string>(StartTimerToFilterProcesses);
            GetReport = new TSCommandAsync(GetReportMethodAsync);
            ReloadHistoryRecords = new TSCommandAsync(UpdateTimeSpanAsync);
            ReportSelectionStore = new TSCommand(OnAnalyticSelectionChanged);
            SelectAnalytic = new TSCommand<StructuredAnalytic>(SelectAnalyticMethod);
            UnselectAnalytic = new TSCommand<StructuredAnalytic>(UnselectAnalyticMethod);
            SelectCalendarItem = new TSCommand(SelectCalendarItemMethod);
            FinilizeEditUserName = new TSCommandAsync<string>(EditUserNameAsync);
            StoreSelectedRecords = new TSCommand<object>(StoreSelectedRecordsMethod);
            CopyRecords = new TSCommand(CopyRecordsMethod);
            PasteRecords = new TSCommand(PasteRecordsMethod);
            TimeSelected = new TSCommand(TimeChanged);
        }

        private void TimeChanged()
        {
            RaisePropertyChanged(nameof(TimeSelectorMin));
            RaisePropertyChanged(nameof(TimeSelectorMax));
            RaisePropertyChanged(nameof(TotalDurationInMinutes));
            RaisePropertyChanged(nameof(BusyTime));

        }

        private void PasteRecordsMethod()
        {
            throw new NotImplementedException();

            //if (CopiedRecords.Count < 1)
            //    return;
            //foreach(TimeSheetTable record in CopiedRecords)
            //{
            //    TimeSheetTable copiedRecord = new TimeSheetTable()
            //    {
            //        Analytic = record.Analytic,
            //        AnalyticId = record.AnalyticId,
            //        Subject = record.Subject,
            //        Comment = record.Comment,
            //        Process = record.Process,
            //        TimeStart = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, record.TimeStart.Hour, record.TimeStart.Minute, record.TimeStart.Second),
            //        TimeEnd = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, record.TimeEnd.Hour, record.TimeEnd.Minute, record.TimeEnd.Second),
            //        TimeSpent = record.TimeSpent,
            //        ClientWays = record.ClientWays,
            //        Formats = record.Formats,
            //    };
            //}
        }

        private void CopyRecordsMethod()
        {
            CopiedRecords.Clear();
            CopiedRecords.AddRange(SelectedRecords);
        }

        private void StoreSelectedRecordsMethod(object records)
        {
            System.Collections.IList items = (System.Collections.IList)records;
            IEnumerable<TimeSheetTable> collection = items.Cast<TimeSheetTable>();
            SelectedRecords.Clear();
            SelectedRecords.AddRange(collection);
        }

        private async Task EditUserNameAsync(string newName)
        {
            string[] UserNameSplited = newName.Split(' ');
            if (UserNameSplited.Length < 3)
            {
                MessageBox.Show("Указано некорректное ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                CurrentUser.LastName = UserNameSplited[0];
                CurrentUser.FirstName = UserNameSplited[1];
                CurrentUser.FatherName = UserNameSplited[2];
                await EFDataProvider.CommitAsync();
            }
            RaisePropertyChanged(nameof(CurrentUserFullName));
        }

        private void QuitIfStartedFromServer()
        {
            if (Directory.GetCurrentDirectory().ToLower().
                IndexOf("орппа", 0) > -1)
            {
                Environment.Exit(0);
            }

        }

        private void TimerTick(object state)
        {
            UpdateCalendarItemsMethod();
        }
        private async void UpdateCalendarItemsMethod()
        {
            if (worker == null)
            {
                await Task.Run(() =>
                {
                    currentDispatcher.Invoke(() => worker = new DominoWorker());
                });
            }
            DateTime date = CurrentDate;
            currentDispatcher.Invoke(() =>
            {
                loadCalendarTimer.Change(300000, Timeout.Infinite);
                CalendarItems.Clear();
                CalendarItemsCount = 0;
                IsCalendarLoading = true;
            });
            await Task.Run(() =>
            {
                List<CalendarItem> calendarItems = GetDominoCalendar(CurrentDate);
                currentDispatcher.Invoke(() =>
                {
                    CalendarItems.Clear();
                    calendarItems.ForEach(CalendarItems.Add);
                    IsCalendarLoading = false;
                });
            });
        }

        private void SelectCalendarItemMethod()
        {
            NewRecord.Subject = CurrentCalendarItem.Subject;
            NewRecord.TimeStart = CurrentCalendarItem.StartTime;
            NewRecord.TimeEnd = CurrentCalendarItem.EndTime;
            RaisePropertyChanged(nameof(NewRecord));
        }

        private void SelectAnalyticMethod(StructuredAnalytic analytic)
        {
            analytic.Selected = true;
            OnAnalyticSelectionChanged();
        }
        private void UnselectAnalyticMethod(StructuredAnalytic analytic)
        {
            analytic.Selected = false;
            OnAnalyticSelectionChanged();
        }

        private void OnAnalyticSelectionChanged()
        {
            SelectedAnalytics.Clear();
            SubordinatedAnalytics.
                Where(analytic => analytic.Selected).
                    Select(analytic => analytic.Analytic).
                        ToList().
                            ForEach(SelectedAnalytics.Add);
        }

        private void GenerateNodes()
        {
            foreach (StructuredAnalytic analytic in SubordinatedAnalytics)
            {
                #region initialize
                if (SubordinatedAnalyticNodes.Count < 1)
                    SubordinatedAnalyticNodes.Add(new Node(analytic.FirstStructure));
                #endregion
                #region Generate Ierarhial
                for (int i = 0; i < SubordinatedAnalyticNodes.Count; i++)
                {
                    #region 1stGen
                    Node firstGen = Node.FindNode(analytic.FirstStructure, SubordinatedAnalyticNodes);
                    if (firstGen == null)
                    {
                        firstGen = new Node(analytic.FirstStructure);
                        SubordinatedAnalyticNodes.Add(firstGen);
                    }
                    #endregion

                    #region 2ndGen
                    if (string.IsNullOrEmpty(analytic.SecondStructure) || analytic.SecondStructure.Equals("Отсутствует"))
                    {
                        firstGen.Analytics.Add(analytic);
                        break;
                    }
                    Node secondGen = Node.FindNode(analytic.SecondStructure, SubordinatedAnalyticNodes);
                    if (secondGen == null)
                    {
                        firstGen.AddChild(new Node(analytic.SecondStructure));
                        secondGen = Node.FindNode(analytic.SecondStructure, SubordinatedAnalyticNodes);
                    }
                    #endregion

                    #region 3ndGen
                    if (string.IsNullOrEmpty(analytic.ThirdStructure) || analytic.ThirdStructure.Equals("Отсутствует"))
                    {
                        secondGen.Analytics.Add(analytic);
                        break;
                    }
                    Node thirdGen = Node.FindNode(analytic.ThirdStructure, SubordinatedAnalyticNodes);
                    if (thirdGen == null)
                    {
                        secondGen.AddChild(new Node(analytic.ThirdStructure));
                        thirdGen = Node.FindNode(analytic.ThirdStructure, SubordinatedAnalyticNodes);
                    }
                    #endregion

                    #region 4thGen
                    if (string.IsNullOrEmpty(analytic.FourStructure) || analytic.FourStructure.Equals("Отсутствует"))
                    {
                        thirdGen.Analytics.Add(analytic);
                        break;
                    }

                    Node fourGen = Node.FindNode(analytic.FourStructure, SubordinatedAnalyticNodes);
                    if (fourGen == null)
                    {
                        thirdGen.AddChild(new Node(analytic.FourStructure));
                        fourGen = Node.FindNode(analytic.FourStructure, SubordinatedAnalyticNodes);
                    }
                    fourGen.Analytics.Add(analytic);
                    #endregion
                }
                #endregion
            }

            foreach (Node node1 in SubordinatedAnalyticNodes)
            {
                node1.CountAnalytics(node1);
            }
        }

        private async Task GetReportMethodAsync()
        {
            if (SelectedAnalytics.Count() < 1)
            {
                MessageBox.Show("Не выбрано ни одного аналитика", "Выберите аналитика", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            IsReady = false;
            ReportBtnName = "Выгружаю отчет";
            await EFDataProvider.GetReportAsync(SelectedReport+1, SelectedAnalytics.ToArray(), StartReportDate, EndReportDate);
            ReportBtnName = "Выгрузить отчет";
            IsReady = true;
        }

        private async Task CheckTimeForIntesectionMethodAsync()
        {
            if (!(EditedRecord.TimeStart >= InitalTimeStart &&
                EditedRecord.TimeStart < InitalTimeEnd) &&
                await EFDataProvider.IsCollisionedWithOtherRecordsAsync(EditedRecord) ||
                (EditedRecord.TimeStart == EditedRecord.TimeEnd ||
                EditedRecord.TimeStart > EditedRecord.TimeEnd))
            {
                IsTimeCorrect = false;
            }
            else
            {
                IsTimeCorrect = true;
            }
            RaisePropertyChanged(nameof(IsTimeCorrect));
        }

        private async Task SetupSelectionAsLastTimeAsync(Process selectedProcess)
        {
            BusinessBlockChoiceCollection.Clear();
            SupportsChoiceCollection.Clear();
            EscalationsChoiceCollection.Clear();
            RiskChoiceCollection.Clear();
            if (selectedProcess != null)
            {
                TimeSheetTable lastRecord = await EFDataProvider.GetLastRecordWithSameProcessAsync(selectedProcess.Id, CurrentUser.UserName);
                if (lastRecord != null)
                {
                    List<BusinessBlock> blocks = lastRecord.BusinessBlocks.Select(i => i.BusinessBlock).Distinct().ToList();
                    foreach(BusinessBlock block in blocks)
                    {
                        BusinessBlockChoiceCollection.Add(block);
                    }
                    blocks.ForEach(BusinessBlockChoiceCollection.Add);
                    lastRecord.Supports.Select(i => i.Supports).Distinct().ToList().ForEach(SupportsChoiceCollection.Add);
                    lastRecord.Escalations.Select(i => i.Escalation).Distinct().ToList().ForEach(EscalationsChoiceCollection.Add);
                    lastRecord.Risks.Select(i => i.Risk).Distinct().ToList().ForEach(RiskChoiceCollection.Add);

                    NewRecord.ClientWays = lastRecord.ClientWays;
                    NewRecord.Formats = lastRecord.Formats;
                }
                RaisePropertyChanged(nameof(NewRecord));
            }
            await UpdateSubjectsHintsAsync();
        }

        private async Task UpdateSubjectsHintsAsync()
        {
            var result = await EFDataProvider.GetSubjectHintsAsync(NewRecord.Process);
            subjectsFromDB = result.ToList();

            SubjectHints.Clear();
            int itemsCount = subjectsFromDB.Count;

            for (int i = itemsCount - 1; i >= 0 && SubjectHints.Count < 11; i--)
            {
                if (subjectsFromDB.Count > 0 && !SubjectHints.Any(subj => subj.Equals(subjectsFromDB[i])))
                {
                    SubjectHints.Add(subjectsFromDB[i]);
                }
            }
        }

        private async Task DeleteHistoryRecordAsync(TimeSheetTable record)
        {
            TodayRecords.Remove(record);
            await EFDataProvider.DeleteRecordAsync(record.Id);
        }

        /// <summary>
        /// Заполняет коллекции списков значениями из БД
        /// </summary>
        private async Task FillDataCollectionsAsync()
        {
            string userName = Environment.UserName;
#if DevAtHome
            userName = "u_m0x0c";
#endif

            CurrentUser = EFDataProvider.LoadAnalyticData(userName);
            StartTimerToFilterProcesses(string.Empty);
            IEnumerable<BusinessBlock> bBlocksResult = await EFDataProvider.GetBusinessBlocksAsync();
            IEnumerable<Supports> supportsResult = await EFDataProvider.GetSupportsAsync();
            IEnumerable<ClientWays> clientWaysResult = await EFDataProvider.GetClientWaysAsync();
            IEnumerable<Formats> formatsResult = await EFDataProvider.GetFormatAsync();
            IEnumerable<Escalation> escalationsResult = await EFDataProvider.GetEscalationAsync();
            IEnumerable<Risk> risksResult = await EFDataProvider.GetRisksAsync();
            IEnumerable<Process> processesResult = await EFDataProvider.GetProcessesAsync();
            IEnumerable<string> reportsResult = await EFDataProvider.GetReportsAvailableAsync();
            BusinessBlocks = bBlocksResult.ToList();
            Supports = supportsResult.ToList();
            ClientWays = clientWaysResult.ToList();
            Formats = formatsResult.ToList();
            Escalations = escalationsResult.ToList();
            Risks = risksResult.ToList();
            ReportsAvailable = reportsResult.ToList();
            AllProcesses = new ObservableCollection<Process>(processesResult);
            RaisePropertyChanged(nameof(BusinessBlocks));
            RaisePropertyChanged(nameof(Supports));
            RaisePropertyChanged(nameof(ClientWays));
            RaisePropertyChanged(nameof(Formats));
            RaisePropertyChanged(nameof(Escalations));
            RaisePropertyChanged(nameof(Risks));
            RaisePropertyChanged(nameof(ReportsAvailable));
            await UpdateSubjectsHintsAsync();
            var analyticsAsync = await EFDataProvider.GetMyAnalyticsDataAsync(CurrentUser);
            SubordinatedAnalytics = ConvertToStructuredAnalytics(analyticsAsync);
            CurrentUserTeam = new ObservableCollection<Analytic>(analyticsAsync);

        }

        private ObservableCollection<StructuredAnalytic> ConvertToStructuredAnalytics(IEnumerable<Analytic> analytics)
        {
            ObservableCollection<StructuredAnalytic> exportVal = new ObservableCollection<StructuredAnalytic>();
            foreach (Analytic analytic in analytics)
            {
                StructuredAnalytic ordered = new StructuredAnalytic(analytic);
                ordered.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Ordered_PropertyChanged);
                exportVal.Add(ordered);
            }
            return exportVal;
        }

        private void Ordered_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Selected"))
            {
                OnAnalyticSelectionChanged();
            }
        }

        private async Task UpdateTimeSpanAsync()
        {
            UpdateCalendarItemsMethod();
            currentDispatcher.Invoke(() =>
            {
                TodayRecords.Clear();
            });
            
            IEnumerable<TimeSheetTable> records = await EFDataProvider.LoadTimeSheetRecordsAsync(CurrentDate, CurrentUser.UserName);
            currentDispatcher.Invoke(() =>
            {
                records.ToList().ForEach(TodayRecords.Add);
            });
        }

        private void StartTimerToFilterProcesses(string userSearchInput)
        {
            int timeout = string.IsNullOrWhiteSpace(userSearchInput) ? 0 : 500;
            delayTimer?.Dispose();
            delayTimer = new Timer(DoFilter, userSearchInput, timeout, Timeout.Infinite);
            UserFilteredProcesses = UserFilteredProcesses ?? new ObservableCollection<Process>();
        }

        private async void DoFilter(object obj)
        {
            IEnumerable<Process> queryResult = await EFDataProvider.GetProcessesSortedByRelevanceAsync(CurrentUser.UserName, (string)obj);
            await currentDispatcher.Invoke(async ()=>{
                UserFilteredProcesses.Clear();
                queryResult.ToList().ForEach(UserFilteredProcesses.Add);
                if (UserFilteredProcesses.Count > 0)
                {
                    await SetupSelectionAsLastTimeAsync(UserFilteredProcesses[0]);
                    RaisePropertyChanged(nameof(UserFilteredProcesses));
                }
            });
        }

        Timer delayTimer;

        private void ShowMessage(string msg, string subj, MessageBoxImage img)
        {
            currentDispatcher.Invoke(() =>
            {
                MessageBox.Show(msg, subj, MessageBoxButton.OK, img);
            });
        }

        private async Task AddRecordMethod(TimeSheetTable newItem)
        {
            #region UnitTest's
            if (IsIntersectsWithOtherRecords(newItem))
            {
                ShowMessage("Добавляемая запись пересекается с другой активностью. Выберите другое время.", "ошибка", MessageBoxImage.Exclamation);
                return;
            }
            else if (newItem.TimeStart == newItem.TimeEnd)
            {
                ShowMessage("Время начала равно времени окончания. Укажите корректное время", "ошибка", MessageBoxImage.Exclamation);
                return;
            }
            else if (newItem.TimeStart > newItem.TimeEnd)
            {
                ShowMessage("Время начала больше времени окончания. Укажите корректное время", "ошибка", MessageBoxImage.Exclamation);
                return;
            }
            else if (BusinessBlockChoiceCollection.Count == 0)
            {
                ShowMessage("Не указан ни один бизнес блок. Укажите хотя бы один", "ошибка", MessageBoxImage.Exclamation);
                return;
            }
            #endregion

            TimeSheetTable newRec = new TimeSheetTable
            {
                Analytic = CurrentUser,
                Risks = RiskChoiceCollection.Select(item => new RiskNew { TimeSheetTableId = newItem.Id, RiskId = item.Id }).ToList(),
                Escalations = EscalationsChoiceCollection.Select(item => new EscalationNew { TimeSheetTableId = newItem.Id, EscalationId = item.Id }).ToList(),
                BusinessBlocks = BusinessBlockChoiceCollection.Select(item => new BusinessBlockNew { TimeSheetTableId = newItem.Id, BusinessBlockId = item.Id }).ToList(),
                Supports = SupportsChoiceCollection.Select(item => new SupportNew { TimeSheetTableId = newItem.Id, SupportId = item.Id }).ToList(),
                ClientWaysId = newItem.ClientWays.Id,
                Comment = newItem.Comment,
                FormatsId = newItem.Formats.Id,
                Process_id = newItem.Process.Id,
                Subject = newItem.Subject,
                TimeStart = newItem.TimeStart,
                TimeEnd = newItem.TimeEnd,
                TimeSpent = (int)(newItem.TimeEnd - newItem.TimeStart).TotalMinutes
            };

            #region Обновление представления
            
            RaisePropertyChanged(nameof(TotalDurationInMinutes));
            newItem.TimeStart = newItem.TimeEnd;
            newItem.TimeEnd = newItem.TimeEnd.AddMinutes(15);
            IgnoreSubjectTextChange = true;
            newItem.Subject = string.Empty;
            IgnoreSubjectTextChange = false;
            newItem.Comment = string.Empty;
            RaisePropertyChanged(nameof(NewRecord));
            #endregion
            TimeSheetTable newRecord = await EFDataProvider.AddActivityAsync(newRec);
            TodayRecords.Add(newRecord);
        }

        private bool IsIntersectsWithOtherRecords(TimeSheetTable record)
        {
            if (EFDataProvider.IsCollisionedWithOtherRecords(record))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task EditHistoryProcessAsync(TimeSheetTable Record)
        {
            if (Record == null) return;
            EditedRecord = new TimeSheetTable()
            {
                Analytic = Record.Analytic,
                AnalyticId = Record.AnalyticId,
                Subject = Record.Subject,
                Comment = Record.Comment,
                Process = Record.Process,
                TimeStart = Record.TimeStart,
                TimeEnd = Record.TimeEnd,
                TimeSpent = Record.TimeSpent,
                ClientWays = Record.ClientWays,
                Formats = Record.Formats,
                Id = Record.Id
            };
            InitalTimeStart = Record.TimeStart;
            InitalTimeEnd = Record.TimeEnd;

            #region LoadSelection
            BusinessBlockChoiceCollection.Clear();
            RiskChoiceCollection.Clear();
            EscalationsChoiceCollection.Clear();
            SupportsChoiceCollection.Clear();

            foreach (BusinessBlockNew Bblock in Record.BusinessBlocks)
            {
                BusinessBlockChoiceCollection.Add(Bblock.BusinessBlock);
            }
            foreach (RiskNew risk in Record.Risks)
            {
                RiskChoiceCollection.Add(risk.Risk);
            }
            foreach (SupportNew suport in Record.Supports)
            {
                SupportsChoiceCollection.Add(suport.Supports);
            }
            foreach (EscalationNew escalation in Record.Escalations)
            {
                EscalationsChoiceCollection.Add(escalation.Escalation);
            }
            #endregion

            EditForm form = new EditForm();
            if (form.ShowDialog() == true)
            {
                CheckTimeForIntesectionMethodAsync();
                EFDataProvider.RemoveSelection(Record.Id);
                EditedRecord.Risks.Clear();
                EditedRecord.Process_id = EditedRecord.Process.Id;
                EditedRecord.ClientWaysId = EditedRecord.ClientWays.Id;
                EditedRecord.FormatsId = EditedRecord.Formats.Id;
                EditedRecord.BusinessBlocks.Clear();
                EditedRecord.Supports.Clear();
                EditedRecord.Escalations.Clear();
                EditedRecord.Risks.AddRange(RiskChoiceCollection.Select(item => new RiskNew { TimeSheetTableId = Record.Id, RiskId = item.Id }));
                EditedRecord.BusinessBlocks.AddRange(BusinessBlockChoiceCollection.Select(item => new BusinessBlockNew { TimeSheetTableId = Record.Id, BusinessBlockId = item.Id }));
                EditedRecord.Supports.AddRange(SupportsChoiceCollection.Select(item => new SupportNew { TimeSheetTableId = Record.Id, SupportId = item.Id }));
                EditedRecord.Escalations.AddRange(EscalationsChoiceCollection.Select(item => new EscalationNew { TimeSheetTableId = Record.Id, EscalationId = item.Id }));

                await EFDataProvider.UpdateProcessAsync(Record, EditedRecord);
                UpdateTimeSpanAsync();
            }
        }

        private List<CalendarItem> GetDominoCalendar(DateTime date)
        {
            var items = worker.GetCalendarRecords(date);
            CalendarItemsCount = items.Count;
            return items;
        }
    }
}