using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TimeSheetApp.Model;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public IEFDataProvider EFDataProvider;

        #region DataCollections

        #region Список процессов для выбора
        /// <summary>
        /// Список процессов, доступных для выбора
        /// </summary>
        private static ObservableCollection<Process> _processes;
        public ObservableCollection<Process> Processes { get => _processes; set => _processes = value; }
        /// <summary>
        /// Список отфильтрованных процессов. К этой коллекции привязан лист ProcessList
        /// </summary>
        private ObservableCollection<Process> _processesFiltered = new ObservableCollection<Process>();
        public ObservableCollection<Process> ProcessFiltered { get => _processesFiltered; set => _processesFiltered = value; }
        #endregion

        #region ComboBox Collections
        /// <summary>
        /// Бизнес блоки, доступные для выбора
        /// </summary>
        private BusinessBlock[] _BusinessBlock;
        public BusinessBlock[] BusinessBlock { get => _BusinessBlock; set => _BusinessBlock = value; }
        /// <summary>
        /// Support's доступные для выбора
        /// </summary>
        private Supports[] _supportsArr;
        public Supports[] SupportsArr { get => _supportsArr; set => _supportsArr = value; }
        /// <summary>
        /// Клиентские пути, доступные для выбора
        /// </summary>
        private ClientWays[] _clientWays;
        public ClientWays[] ClientWays { get => _clientWays; set => _clientWays = value; }
        /// <summary>
        /// Форматы доступные для выбора
        /// </summary>
        private Formats[] _formatList;
        public Formats[] FormatList { get => _formatList; set => _formatList = value; }
        /// <summary>
        /// Риски, доступные для выбора
        /// </summary>
        private Risk[] _riskCol;
        public Risk[] RiskCol { get => _riskCol; set => _riskCol = value; }
        /// <summary>
        /// Эскалации, доступные для выбора
        /// </summary>
        private Escalation[] _escalations;
        public Escalation[] Escalations { get => _escalations; set => _escalations = value; }
        #endregion

        #region Исторические записи
        /// <summary>
        /// Исторические записи из БД. (Лист TimeSpanListView)
        /// </summary>
        private ObservableCollection<TimeSheetTable> _historyRecords = new ObservableCollection<TimeSheetTable>();
        public ObservableCollection<TimeSheetTable> HistoryRecords { get => _historyRecords; set => _historyRecords = value; }
        #endregion

        #region Список сотрудников в подчинении
        /// <summary>
        /// Список сотрудников в подчинении у текущего пользователя
        /// </summary>
        private ObservableCollection<Analytic> _subordinateEmployees = new ObservableCollection<Analytic>();
        public ObservableCollection<Analytic> SubordinateEmployees
        {
            get { return _subordinateEmployees; }
            set { _subordinateEmployees = value; }
        }
        private ObservableCollection<AnalyticOrdered> subordinatedOrdered = new ObservableCollection<AnalyticOrdered>();

        public ObservableCollection<AnalyticOrdered> SubordinatedOrdered
        {
            get { return subordinatedOrdered; }
            set { subordinatedOrdered = value; }
        }

        private ObservableCollection<Node> nodes = new ObservableCollection<Node>();
        public ObservableCollection<Node> NodesCollection { get => nodes; set => nodes = value; }

        private List<string> categories = new List<string>();
        #endregion

        #endregion

        #region CurrentValues
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
       

        DominoWorker worker = new DominoWorker();
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
            "Отчет по активности аналитиков",
            "Ресурсный план",
            "Процессы по отделам",
            "Аллокации"
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
                foreach (TimeSheetTable record in HistoryRecords)
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
        private string _currentUserFullName;
        public string CurrentUserFullName
        {
            get
            {
                return _currentUserFullName;
            }
            set
            {
                _currentUserFullName = value;
            }
        }
        private bool isEditState = false;
        private ObservableCollection<CalendarItem> _calendarItems = new ObservableCollection<CalendarItem>();

        public ObservableCollection<CalendarItem> CalendarItems
        {
            get { return _calendarItems; }
            set { _calendarItems = value; }
        }

        Timer loadCalendarTimer;

        #endregion

        #region Commands
        public RelayCommand<TimeSheetTable> AddProcess { get; }
        public RelayCommand<TimeSheetTable> EditProcess { get; }
        public RelayCommand<TimeSheetTable> DeleteProcess { get; }
        public RelayCommand ReloadTimeSheet { get; }
        public RelayCommand<string> FilterProcesses { get; }
        public RelayCommand<Process> LoadSelectionForSelectedProcess { get; }
        public RelayCommand ReloadHistoryRecords { get; }
        public RelayCommand CheckTimeForIntersection { get; }
        public RelayCommand GetReport { get; }
        public RelayCommand<AnalyticOrdered> SelectAnalytic { get; }
        public RelayCommand<AnalyticOrdered> UnselectAnalytic { get; }
        public RelayCommand ReportSelectionStore { get; }
        public RelayCommand SelectCalendarItem { get; }
        public RelayCommand<System.Windows.Input.KeyEventArgs> FinilizeEditUserName { get; }


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
                updateSubjectHints();
                AddProcess = new RelayCommand<TimeSheetTable>(AddRecordMethod);
                EditProcess = new RelayCommand<TimeSheetTable>(EditHistoryProcess);
                DeleteProcess = new RelayCommand<TimeSheetTable>(DeleteHistoryRecord);
                ReloadTimeSheet = new RelayCommand(UpdateTimeSpan);
                CheckTimeForIntersection = new RelayCommand(CheckTimeForIntesectionMethod);
                LoadSelectionForSelectedProcess = new RelayCommand<Process>(LoadSelection);
                FilterProcesses = new RelayCommand<string>(FilterProcessesMethod);
                GetReport = new RelayCommand(GetReportMethod);
                ReloadHistoryRecords = new RelayCommand(UpdateTimeSpan);
                ReportSelectionStore = new RelayCommand(ReportSelectionUpdate);
                SelectAnalytic = new RelayCommand<AnalyticOrdered>(SelectAnalyticMethod);
                UnselectAnalytic = new RelayCommand<AnalyticOrdered>(UnselectAnalyticMethod);
                SelectCalendarItem = new RelayCommand(SelectCalendarItemMethod);
                FinilizeEditUserName = new RelayCommand<System.Windows.Input.KeyEventArgs>(FinishEditingUserName);
                NewRecord.Analytic = CurrentUser;
                NewRecord.AnalyticId = CurrentUser.Id;
                GenerateNodes();
                loadCalendarTimer = new Timer(timerTick, null, 0, Timeout.Infinite);
                UpdateTimeSpan();
                IsReady = true;
            }
            catch(Exception ex)
            {
                if(MessageBox.Show($"{ex.Message}. {ex.InnerException}. {ex.StackTrace}", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Environment.Exit(0);
                }

            }
            
        }

        private void FinishEditingUserName(KeyEventArgs obj)
        {
            MessageBox.Show(obj.Key.ToString());
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
        private void UpdateCalendarItemsMethod()
        {
            DateTime date = CurrentDate;
            currentDispatcher.Invoke(() =>
            {
                loadCalendarTimer.Change(300000, Timeout.Infinite);
                CalendarItems.Clear();
                CalendarItemsCount = 0;
                IsCalendarLoading = true;
            });

            Task task = new Task(() =>
            {
                CalendarItems = GetDominoCalendar(date);
                IsCalendarLoading = false;
            });
            
            task.Start();

        }

        private void SelectCalendarItemMethod()
        {
            NewRecord.Subject = CurrentCalendarItem.Subject;
            NewRecord.TimeStart = CurrentCalendarItem.StartTime;
            NewRecord.TimeEnd = CurrentCalendarItem.EndTime;
            RaisePropertyChanged(nameof(NewRecord));
        }

        private void SelectAnalyticMethod(AnalyticOrdered analytic)
        {
            analytic.Selected = true;
            ReportSelectionUpdate();
        }
        private void UnselectAnalyticMethod(AnalyticOrdered analytic)
        {
            analytic.Selected = false;
            ReportSelectionUpdate();
        }

        private void ReportSelectionUpdate()
        {
            SelectedAnalytics.Clear();
            foreach (AnalyticOrdered analytic in SubordinatedOrdered)
            {
                if (analytic.Selected)
                {
                    SelectedAnalytics.Add(analytic.Analytic);
                }
            }

        }

        private void GenerateNodes()
        {

            foreach (AnalyticOrdered analytic in SubordinatedOrdered)
            {
                
                #region initialize
                if (NodesCollection.Count < 1)
                    NodesCollection.Add(new Node(analytic.FirstStructure));
                #endregion
                #region Generate Ierarhial
                foreach (Node node in NodesCollection)
                {
                    #region 1stGen
                    if (string.IsNullOrEmpty(analytic.FirstStructure)) break;

                    Node firstGen = Node.FindNode(analytic.FirstStructure, NodesCollection);
                    if (firstGen == null)
                    {
                        firstGen = new Node(analytic.FirstStructure);
                        NodesCollection.Add(firstGen);
                    }
                    #endregion

                    #region 2ndGen

                    if (string.IsNullOrEmpty(analytic.SecondStructure) || analytic.SecondStructure.Equals("Отсутствует"))
                    {
                        firstGen.Analytics.Add(analytic);
                        break;
                    }

                    Node secondGen = Node.FindNode(analytic.SecondStructure, NodesCollection);
                    if (secondGen == null)
                    {
                        firstGen.AddChild(new Node(analytic.SecondStructure));
                        secondGen = Node.FindNode(analytic.SecondStructure, NodesCollection);
                    }

                    #endregion

                    #region 3ndGen

                    if (string.IsNullOrEmpty(analytic.ThirdStructure) || analytic.ThirdStructure.Equals("Отсутствует"))
                    {
                        secondGen.Analytics.Add(analytic);
                        break;
                    }

                    Node thirdGen = Node.FindNode(analytic.ThirdStructure, NodesCollection);
                    if (thirdGen == null)
                    {
                        secondGen.AddChild(new Node(analytic.ThirdStructure));
                        thirdGen = Node.FindNode(analytic.ThirdStructure, NodesCollection);
                    }

                    #endregion

                    #region 4thGen

                    if (string.IsNullOrEmpty(analytic.FourStructure) || analytic.FourStructure.Equals("Отсутствует"))
                    {
                        thirdGen.Analytics.Add(analytic);
                        break;
                    }

                    Node fourGen = Node.FindNode(analytic.FourStructure, NodesCollection);
                    if (fourGen == null)
                    {
                        thirdGen.AddChild(new Node(analytic.FourStructure));
                        fourGen = Node.FindNode(analytic.FourStructure, NodesCollection);
                    }
                    fourGen.Analytics.Add(analytic);
                    #endregion


                }
                #endregion


            }
            foreach (Node node1 in NodesCollection)
            {
                node1.CountAnalytics(node1);
            }
        }

        private void updateSubjectHints()
        {
            subjectsFromDB = EFDataProvider.GetSubjectHints(NewRecord.Process);

            SubjectHints.Clear();
            int itemsCount = subjectsFromDB.Count;

            for (int i = itemsCount-1; i > 0 && SubjectHints.Count < 11; i--)
            {
                if (subjectsFromDB.Count > 0 && !SubjectHints.Any(subj=> subj.Equals(subjectsFromDB[i])))
                {
                    SubjectHints.Add(subjectsFromDB[i]);
                }
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

        private void LoadSelection(Process selectedProcess)
        {
            BusinessBlockChoiceCollection.Clear();
            SupportsChoiceCollection.Clear();
            EscalationsChoiceCollection.Clear();
            RiskChoiceCollection.Clear();
            if (selectedProcess != null)
            {
                TimeSheetTable lastRecord = EFDataProvider.GetLastActivityWithSameProcess(selectedProcess, CurrentUser);
                if (lastRecord != null) { 
                    foreach(BusinessBlockNew item in lastRecord.BusinessBlocks)
                    {
                        BusinessBlockChoiceCollection.Add(item.BusinessBlock);
                    }
                    foreach(SupportNew item in lastRecord.Supports)
                    {
                        SupportsChoiceCollection.Add(item.Supports);
                    }
                    foreach(EscalationNew item in lastRecord.Escalations)
                    {
                        EscalationsChoiceCollection.Add(item.Escalation);
                    }
                    foreach(RiskNew item in lastRecord.Risks)
                    {
                        RiskChoiceCollection.Add(item.Risk);
                    }

                    NewRecord.ClientWays = lastRecord.ClientWays;
                    NewRecord.Formats = lastRecord.Formats;
                }
                RaisePropertyChanged(nameof(NewRecord));
            }
            updateSubjectHints();
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
            CurrentUserFullName = $"{CurrentUser.LastName} {CurrentUser.FirstName} {CurrentUser.FatherName}";
            Processes = EFDataProvider.GetProcesses();
            BusinessBlock = EFDataProvider.GetBusinessBlocks();
            SupportsArr = EFDataProvider.GetSupports();
            ClientWays = EFDataProvider.GetClientWays();
            FormatList = EFDataProvider.GetFormat();
            Escalations = EFDataProvider.GetEscalation();
            RiskCol = EFDataProvider.GetRisks();
            FilterProcessesMethod(string.Empty);
            SubordinateEmployees = EFDataProvider.GetMyAnalyticsData(CurrentUser);
            SubordinatedOrdered = GetAnalyticOrdereds(SubordinateEmployees);
            
        }

        private ObservableCollection<AnalyticOrdered> GetAnalyticOrdereds(IEnumerable<Analytic> analytics)
        {
            ObservableCollection<AnalyticOrdered> exportVal = new ObservableCollection<AnalyticOrdered>();
            foreach (Analytic analytic in analytics)
            {
                AnalyticOrdered ordered = new AnalyticOrdered(analytic);
                ordered.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Ordered_PropertyChanged);
                exportVal.Add(ordered);
            }
            return exportVal;
        }

        private void Ordered_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Selected"))
            {
                ReportSelectionUpdate();
            }
        }

        private void UpdateTimeSpan()
        {
            UpdateCalendarItemsMethod();
            HistoryRecords.Clear();
            foreach (TimeSheetTable record in EFDataProvider.LoadTimeSheetRecords(CurrentDate, CurrentUser))
            {
                HistoryRecords.Add(record);
            }
            RaisePropertyChanged(nameof(TotalDurationInMinutes));
        }

        private async void FilterProcessesMethod(string filterText)
        {
            ProcessFiltered?.Clear();
            Dictionary<Process, int> orderedProcesses = new Dictionary<Process, int>();
            List<Process> processes = EFDataProvider.GetProcesses().ToList();



            if (!string.IsNullOrWhiteSpace(filterText))
            {
                processes = processes.Where(rec => rec.ProcName.ToLower().IndexOf(filterText.ToLower()) > -1 ||
                rec.Comment?.ToLower().IndexOf(filterText.ToLower()) > -1 || 
                rec.Id.ToString().Equals(filterText)).ToList();
                foreach(Process process in processes)
                {
                    orderedProcesses.Add(process, 1);
                }
            }
            else
            {
                List<TimeSheetTable> currentAnalyticRecords = EFDataProvider.GetTimeSheetRecordsForAnalytic(CurrentUser);
                foreach(Process process in processes)
                {
                    int currentProcCount = currentAnalyticRecords.Where(i => i.Process_id == process.Id).Count();
                    orderedProcesses.Add(process, currentProcCount);
                }
            }


            foreach (KeyValuePair<Process, int> keyValue in orderedProcesses.OrderByDescending(i => i.Value))
            {
                ProcessFiltered.Add(keyValue.Key);
            }
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
            updateSubjectHints();
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
            isEditState = true;

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
            isEditState = false;

        }

        private ObservableCollection<CalendarItem> GetDominoCalendar(DateTime date)
        {
            var items = new ObservableCollection<CalendarItem>(worker.GetCalendarRecords(date));
            CalendarItemsCount = items.Count;
            return items;
        }
    }
}