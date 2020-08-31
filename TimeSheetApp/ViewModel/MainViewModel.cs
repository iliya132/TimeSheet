using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TimeSheetApp.Model;
using TimeSheetApp.Model.EntitiesBase;

using Process = TimeSheetApp.Model.EntitiesBase.Process;

namespace TimeSheetApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public IEFDataProvider EFDataProvider;

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
        public ObservableCollection<StructuredAnalytic> SubordinatedAnalytics { get; set; }
        public ObservableCollection<Node> SubordinatedAnalyticNodes { get; set; }
        public ObservableCollection<Analytic> CurrentUserTeam { get; set; }
        #endregion

        #endregion

        #region CurrentValues
        
        List<TimeSheetTable> SelectedRecords = new List<TimeSheetTable>();
        List<TimeSheetTable> CopiedRecords = new List<TimeSheetTable>();
        public TimeSpan TimeSelectorMin
        {
            get
            {
                return NewRecord.TimeStart.TimeOfDay;
            }
            set
            {
                NewRecord.TimeStart = new DateTime(NewRecord.TimeStart.Year, NewRecord.TimeStart.Month, NewRecord.TimeStart.Day, value.Hours, value.Minutes, value.Seconds);
                RaisePropertyChanged("NewRecord");
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
                RaisePropertyChanged("NewRecord");
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
                RaisePropertyChanged("IsReady");
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
                RaisePropertyChanged("Title");
            }
        }
        Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;

        public double LastMonthTimeSpent 
        { 
            get
            {
                DateTime thisMonthFirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime thisMonthLastDay = thisMonthFirstDay.AddMonths(1).AddDays(-1);
                DateTime lastMonthFirstDay = thisMonthFirstDay.AddMonths(-1);
                DateTime lastMonthLastDay = thisMonthLastDay.AddMonths(-1);
                return EFDataProvider.GetTimeSpent(CurrentUser, lastMonthFirstDay, lastMonthLastDay);
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
                return EFDataProvider.GetTimeSpent(CurrentUser, thisWeekFirstDay, DateTime.Today);
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
            set { _calendarItemsCount = value;
                RaisePropertyChanged("CalendarItemsCount");
            }
        }
        private bool isCalendarLoading = true;
        public bool IsCalendarLoading
        {
            get => isCalendarLoading;
            set { 
                isCalendarLoading = value;
                RaisePropertyChanged("LoadingVisibilityInverted");
                RaisePropertyChanged("LoadingVisibility");
                RaisePropertyChanged("IsCalendarReady");
                RaisePropertyChanged("IsCalendarLoading");
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
            set { Set(ref _currentEditedRecord, value); }
        }
        private DateTime initalTimeStart { get; set; }
        private DateTime initalTimeEnd { get; set; }

        private bool _isTimeCorrect = true;
        public bool IsTimeCorrect { get => _isTimeCorrect; set => _isTimeCorrect = value; }

        #endregion

        #region Формирование отчета
        private List<string> _reportsAvailable = new List<string>()
        {
            "Активность аналитиков",
            "Ресурсный план",
            "Распределение по процессам",
            "Распределение аллокаций"
        };

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
            get{
                List<(TimeSpan, TimeSpan)> exportValue = new List<(TimeSpan, TimeSpan)>();
                foreach(TimeSheetTable record in TodayRecords)
                {
                    exportValue.Add((record.TimeStart.TimeOfDay, record.TimeEnd.TimeOfDay));
                }
                return exportValue;
            } 
        }

        Timer loadCalendarTimer;

        #endregion

        #region Commands
        public RelayCommand<TimeSheetTable> AddProcess { get; set; }
        public RelayCommand<TimeSheetTable> EditProcess { get; set; }
        public RelayCommand<TimeSheetTable> DeleteProcess { get; set; }
        public RelayCommand ReloadTimeSheet { get; set; }
        public RelayCommand<string> FilterProcesses { get; set; }
        public RelayCommand<Process> LoadSelectionForSelectedProcess { get; set; }
        public RelayCommand ReloadHistoryRecords { get; set; }
        public RelayCommand CheckTimeForIntersection { get; set; }
        public RelayCommand GetReport { get; set; }
        public RelayCommand<StructuredAnalytic> SelectAnalytic { get; set; }
        public RelayCommand<StructuredAnalytic> UnselectAnalytic { get; set; }
        public RelayCommand ReportSelectionStore { get; set; }
        public RelayCommand SelectCalendarItem { get; set; }
        public RelayCommand<string> FinilizeEditUserName { get; set; }
        public RelayCommand<object> StoreSelectedRecords { get; set; }
        public RelayCommand CopyRecords { get; set; }
        public RelayCommand PasteRecords { get; set; }
        public RelayCommand TimeSelected { get; set; }


        #endregion


        public MainViewModel(IEFDataProvider dataProvider)
        {
            try
            {
                IsReady = false;
                Title = "TimeSheet";
                QuitIfStartedFromServer();
                EFDataProvider = dataProvider;
                
                FillDataCollections();
                UpdateSubjectsHints();
                InitializeCommads();
                NewRecord.Analytic = CurrentUser;
                NewRecord.AnalyticId = CurrentUser.Id;
                
                GenerateNodes();
                loadCalendarTimer = new Timer(timerTick, null, 0, Timeout.Infinite);
                UpdateTimeSpan();
                IsReady = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}. {ex.InnerException}. {ex.StackTrace}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        private void InitializeCommads()
        {
            AddProcess = new RelayCommand<TimeSheetTable>(AddRecordMethod);
            EditProcess = new RelayCommand<TimeSheetTable>(EditHistoryProcess);
            DeleteProcess = new RelayCommand<TimeSheetTable>(DeleteHistoryRecord);
            ReloadTimeSheet = new RelayCommand(UpdateTimeSpan);
            CheckTimeForIntersection = new RelayCommand(CheckTimeForIntesectionMethod);
            LoadSelectionForSelectedProcess = new RelayCommand<Process>(SetupSelectionAsLastTime);
            FilterProcesses = new RelayCommand<string>(FilterProcessesMethod);
            GetReport = new RelayCommand(GetReportMethod);
            ReloadHistoryRecords = new RelayCommand(UpdateTimeSpan);
            ReportSelectionStore = new RelayCommand(OnAnalyticSelectionChanged);
            SelectAnalytic = new RelayCommand<StructuredAnalytic>(SelectAnalyticMethod);
            UnselectAnalytic = new RelayCommand<StructuredAnalytic>(UnselectAnalyticMethod);
            SelectCalendarItem = new RelayCommand(SelectCalendarItemMethod);
            FinilizeEditUserName = new RelayCommand<string>(EditUserName);
            StoreSelectedRecords = new RelayCommand<object>(StoreSelectedRecordsMethod);
            CopyRecords = new RelayCommand(CopyRecordsMethod);
            PasteRecords = new RelayCommand(PasteRecordsMethod);
            TimeSelected = new RelayCommand(TimeChanged);
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
            if (CopiedRecords.Count < 1)
                return;
            foreach(TimeSheetTable record in CopiedRecords)
            {
                TimeSheetTable copiedRecord = new TimeSheetTable()
                {
                    Analytic = record.Analytic,
                    AnalyticId = record.AnalyticId,
                    Subject = record.Subject,
                    Comment = record.Comment,
                    Process = record.Process,
                    TimeStart = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, record.TimeStart.Hour, record.TimeStart.Minute, record.TimeStart.Second),
                    TimeEnd = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, record.TimeEnd.Hour, record.TimeEnd.Minute, record.TimeEnd.Second),
                    TimeSpent = record.TimeSpent,
                    ClientWays = record.ClientWays,
                    Formats = record.Formats,
                };
            }
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

        private void EditUserName(string newName)
        {
            string[] UserNameSplited = newName.Split(' ');
            if(UserNameSplited.Length < 3)
            {
                MessageBox.Show("Указано некорректное ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                CurrentUser.LastName = UserNameSplited[0];
                CurrentUser.FirstName = UserNameSplited[1];
                CurrentUser.FatherName = UserNameSplited[2];
                EFDataProvider.Commit();

            }
            RaisePropertyChanged(nameof(CurrentUserFullName));
        }

        private void QuitIfStartedFromServer()
        {
            if(Directory.GetCurrentDirectory().ToLower().
                IndexOf("орппа", 0) > -1)
            {
                Environment.Exit(0);
            }

        }

        private void timerTick(object state)
        {
            UpdateCalendarItemsMethod();
        }
        private async void UpdateCalendarItemsMethod()
        {
            if(worker == null)
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
                currentDispatcher.Invoke(()=> {
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
                Where(analytic=>analytic.Selected).
                    Select(analytic=>analytic.Analytic).
                        ToList().
                            ForEach(SelectedAnalytics.Add);
        }

        private void GenerateNodes()
        {
            SubordinatedAnalyticNodes = new ObservableCollection<Node>();

            foreach (StructuredAnalytic analytic in SubordinatedAnalytics)
            {
                #region initialize
                if (SubordinatedAnalyticNodes.Count < 1)
                    SubordinatedAnalyticNodes.Add(new Node(analytic.FirstStructure));
                #endregion
                #region Generate Ierarhial
                foreach (Node node in SubordinatedAnalyticNodes)
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

        private void GetReportMethod()
        {
            if (SelectedAnalytics.Count() < 1)
            {
                MessageBox.Show("Не выбрано ни одного аналитика", "Выберите аналитика", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            IsReady = false;
            Title = "TimeSheet (Выгружаю отчет...)";
            EFDataProvider.GetReport(SelectedReport, SelectedAnalytics.ToArray(), StartReportDate, EndReportDate);
            Title = "TimeSheet";
            IsReady = true;
        }

        private void CheckTimeForIntesectionMethod()
        {
            if (!(EditedRecord.TimeStart >= initalTimeStart &&
                EditedRecord.TimeStart < initalTimeEnd) &&
                EFDataProvider.IsCollisionedWithOtherRecords(EditedRecord) ||
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

        private void SetupSelectionAsLastTime(Process selectedProcess)
        {
            BusinessBlockChoiceCollection.Clear();
            SupportsChoiceCollection.Clear();
            EscalationsChoiceCollection.Clear();
            RiskChoiceCollection.Clear();
            if (selectedProcess != null)
            {
                TimeSheetTable lastRecord = EFDataProvider.GetLastRecordWithSameProcess(selectedProcess, CurrentUser);
                if (lastRecord != null) {
                    lastRecord.BusinessBlocks.Select(i=>i.BusinessBlock).ToList().ForEach(BusinessBlockChoiceCollection.Add);
                    lastRecord.Supports.Select(i=>i.Supports).ToList().ForEach(SupportsChoiceCollection.Add);
                    lastRecord.Escalations.Select(i=>i.Escalation).ToList().ForEach(EscalationsChoiceCollection.Add);
                    lastRecord.Risks.Select(i=>i.Risk).ToList().ForEach(RiskChoiceCollection.Add);

                    NewRecord.ClientWays = lastRecord.ClientWays;
                    NewRecord.Formats = lastRecord.Formats;
                }
                RaisePropertyChanged(nameof(NewRecord));
            }
            UpdateSubjectsHints();
        }

        private void UpdateSubjectsHints()
        {
            subjectsFromDB = EFDataProvider.GetSubjectHints(NewRecord.Process);

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

        private void DeleteHistoryRecord(TimeSheetTable record)
        {
            EFDataProvider.DeleteRecord(record);
            UpdateTimeSpan();
        }

        /// <summary>
        /// Заполняет коллекции списков значениями из БД
        /// </summary>
        private void FillDataCollections()
        {
            CurrentUser = EFDataProvider.LoadAnalyticData();
            AllProcesses = EFDataProvider.GetProcesses();
            BusinessBlocks = EFDataProvider.GetBusinessBlocks().ToList();
            Supports = EFDataProvider.GetSupports().ToList();
            ClientWays = EFDataProvider.GetClientWays().ToList();
            Formats = EFDataProvider.GetFormat().ToList();
            Escalations = EFDataProvider.GetEscalation().ToList();
            Risks = EFDataProvider.GetRisks().ToList();
            FilterProcessesMethod(string.Empty);
            SubordinatedAnalytics = ConvertToStructuredAnalytics(EFDataProvider.GetMyAnalyticsData(CurrentUser));
            CurrentUserTeam = EFDataProvider.GetMyAnalyticsData(CurrentUser);
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

        private void UpdateTimeSpan()
        {

            UpdateCalendarItemsMethod();
            TodayRecords.Clear();
            EFDataProvider.LoadTimeSheetRecords(CurrentDate, CurrentUser).ForEach(TodayRecords.Add);
            RaisePropertyChanged(nameof(TotalDurationInMinutes));
            RaisePropertyChanged(nameof(BusyTime));
        }

        private void FilterProcessesMethod(string userSearchInput)
        {
            if (UserFilteredProcesses == null)
                UserFilteredProcesses = new ObservableCollection<Process>();
            UserFilteredProcesses.Clear();
            List<Process> processes = AllProcesses.ToList();
            List<TimeSheetTable> currentAnalyticRecords = EFDataProvider.GetTimeSheetRecordsForAnalytic(CurrentUser);
            processes = !string.IsNullOrWhiteSpace(userSearchInput) ?
            processes.Where(rec => rec.ProcName.ToLower().IndexOf(userSearchInput.ToLower()) > -1 ||
                rec.Comment?.ToLower().IndexOf(userSearchInput.ToLower()) > -1 ||
                rec.Id.ToString().Equals(userSearchInput)).ToList()
            : processes;
            processes = processes.OrderByDescending(proc => currentAnalyticRecords.Where(i => i.Process_id == proc.Id).Count()).ThenBy(proc=>proc.Id).ToList();
            processes.ForEach(UserFilteredProcesses.Add);
        }

        private void AddRecordMethod(TimeSheetTable newItem)
        {
            #region UnitTest's
            if (IsIntersectsWithOtherRecords(newItem))
            {
                MessageBox.Show("Добавляемая запись пересекается с другой активностью. Выберите другое время.", "ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else if (newItem.TimeStart == newItem.TimeEnd)
            {
                MessageBox.Show("Время начала равно времени окончания. Укажите корректное время", "ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else if (newItem.TimeStart > newItem.TimeEnd)
            {
                MessageBox.Show("Время начала больше времени окончания. Укажите корректное время", "ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            } else if (BusinessBlockChoiceCollection.Count == 0)
            {
                MessageBox.Show("Не указан ни один бизнес блок. Укажите хотя бы один", "ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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


            EFDataProvider.AddActivity(newRec) ;

            #region Обновление представления
            UpdateTimeSpan();
            RaisePropertyChanged(nameof(TotalDurationInMinutes));
            UpdateSubjectsHints();
            RaisePropertyChanged("subjectHints");
            newItem.TimeStart = newItem.TimeEnd;
            newItem.TimeEnd = newItem.TimeEnd.AddMinutes(15);
            IgnoreSubjectTextChange = false;
            newItem.Subject = string.Empty;
            IgnoreSubjectTextChange = true;

            newItem.Comment = string.Empty;
            RaisePropertyChanged(nameof(NewRecord));
            #endregion
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

        private void EditHistoryProcess(TimeSheetTable Record)
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
            initalTimeStart = Record.TimeStart;
            initalTimeEnd = Record.TimeEnd;

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
                CheckTimeForIntesectionMethod();
                EFDataProvider.RemoveSelection(Record);
                Record.Risks.Clear();
                Record.BusinessBlocks.Clear();
                Record.Supports.Clear();
                Record.Escalations.Clear();
                Record.Risks.AddRange(RiskChoiceCollection.Select(item => new RiskNew { TimeSheetTableId = Record.Id, RiskId = item.Id }));
                Record.BusinessBlocks.AddRange(BusinessBlockChoiceCollection.Select(item => new BusinessBlockNew { TimeSheetTableId = Record.Id, BusinessBlockId = item.Id }));
                Record.Supports.AddRange(SupportsChoiceCollection.Select(item => new SupportNew { TimeSheetTableId = Record.Id, SupportId = item.Id }));
                Record.Escalations.AddRange(EscalationsChoiceCollection.Select(item => new EscalationNew { TimeSheetTableId = Record.Id, EscalationId = item.Id }));

                EFDataProvider.UpdateProcess(Record, EditedRecord);
                UpdateTimeSpan();
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