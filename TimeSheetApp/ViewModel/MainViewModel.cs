using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using TimeSheetApp.Model;
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
        private Escalations[] _escalations;
        public Escalations[] Escalations { get => _escalations; set => _escalations = value; }
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
        private ObservableCollection<Analytic> _subordinateEmployees;
        public ObservableCollection<Analytic> SubordinateEmployees
        {
            get { return _subordinateEmployees; }
            set { _subordinateEmployees = value; }
        }
        /// <summary>
        /// Выбранные сотрудники
        /// </summary>
        private List<Analytic> SelectedEmployees = new List<Analytic>();

        #endregion

        #endregion

        #region CurrentValues
        /// <summary>
        /// Общая длительность записей в коллекции HistoryRecords
        /// </summary>
        public Visibility ReportAcess { get; set; }

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
        private BusinessBlockChoice _currentBusinessBlock = new BusinessBlockChoice();

        public BusinessBlockChoice CurrentBusinessBlock
        {
            get { return _currentBusinessBlock; }
            set
            {
                NewRecord.BusinessBlockChoice = value;
                _currentBusinessBlock = value;
            }
        }
        private supportChoice _currentSupports = new supportChoice();
        public supportChoice CurrentSupports
        {
            get { return _currentSupports; }
            set
            {
                NewRecord.supportChoice = value;
                _currentSupports = value;
            }
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
        private EscalationChoice _currentEscalation = new EscalationChoice();
        public EscalationChoice CurrentEscalation
        {
            get { return _currentEscalation; }
            set
            {
                NewRecord.EscalationChoice = value;
                _currentEscalation = value;
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
        private Risk _currentRisk = new Risk();
        public Risk CurrentRisk
        {
            get { return _currentRisk; }
            set
            {
                NewRecord.riskChoise_id = value.id;
                _currentRisk = value;
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
            "Отчет по активности аналитиков"
        };

        public List<string> ReportsAvailable
        {
            get { return _reportsAvailable; }
            set { _reportsAvailable = value; }
        }

        private List<Analytic> _selectedAnalytics = new List<Analytic>();
        public List<Analytic> SelectedAnalytics { get => _selectedAnalytics; set => _selectedAnalytics = value; }
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

        #region Мультивыбор
        riskChoise riskChoise = new riskChoise();
        BusinessBlockChoice businessBlockChoice = new BusinessBlockChoice();
        supportChoice supportChoice = new supportChoice();
        EscalationChoice escalationChoice = new EscalationChoice();
        public BusinessBlock defBlock;
        #endregion

        public TimeSpan TotalDurationInMinutes
        {
            get
            {
                TimeSpan totalSpan = new TimeSpan();
                foreach (TimeSheetTable record in HistoryRecords)
                {
                    totalSpan += record.timeEnd - record.timeStart;
                }
                return totalSpan;
            }
        }
        DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate { get => _currentDate; set => _currentDate = value; }
        private Analytic _currentUser;
        public Analytic CurrentUser { get => _currentUser; set => _currentUser = value; }
        private bool isEditState = false;
        #endregion

        #region Commands
        public RelayCommand<TimeSheetTable> AddProcess { get; }
        public RelayCommand<TimeSheetTable> EditProcess { get; }
        public RelayCommand<TimeSheetTable> DeleteProcess { get; }
        public RelayCommand ReloadTimeSheet { get; }
        public RelayCommand<string> FilterProcesses { get; }
        public RelayCommand ExportReport { get; }
        public RelayCommand<Process> LoadSelectionForSelectedProcess { get; }
        public RelayCommand ReloadHistoryRecords { get; }
        public RelayCommand CheckTimeForIntersection { get; }
        public RelayCommand<IEnumerable<object>> GetReport { get; }
        public RelayCommand<IList<object>> StoreSelection { get; }
        #endregion

        public MainViewModel(IEFDataProvider dataProvider)
        {
            EFDataProvider = dataProvider;
            FillDataCollections();
            AddProcess = new RelayCommand<TimeSheetTable>(AddProcessMethod);
            EditProcess = new RelayCommand<TimeSheetTable>(EditHistoryProcess);
            DeleteProcess = new RelayCommand<TimeSheetTable>(DeleteHistoryRecord);
            ReloadTimeSheet = new RelayCommand(UpdateTimeSpan);
            CheckTimeForIntersection = new RelayCommand(CheckTimeForIntesectionMethod);
            LoadSelectionForSelectedProcess = new RelayCommand<Process>(LoadSelection);
            FilterProcesses = new RelayCommand<string>(FilterProcessesMethod);
            ExportReport = new RelayCommand(GetReportMethod);
            GetReport = new RelayCommand<IEnumerable<object>>(GetReportMethod);
            ReloadHistoryRecords = new RelayCommand(UpdateTimeSpan);
            StoreSelection = new RelayCommand<IList<object>>(StoreMultiplyChoice);
            NewRecord.Analytic = CurrentUser;
            NewRecord.AnalyticId = CurrentUser.Id;
            NewRecord.riskChoise_id = 2;
            ReportAcess = EFDataProvider.isAnalyticHasAccess(CurrentUser);
            RaisePropertyChanged("ReportAcess");
        }

        private void StoreMultiplyChoice(IList<object> choice)
        {
            int countOfChoice = choice.Count();
            for (int i = 0; i < countOfChoice; i++)
            {
                if (choice[i] is Risk)
                {
                    switch (i)
                    {
                        case (0): riskChoise.Risk_id = (choice[i] as Risk).id; break;
                        case (1): riskChoise.Risk_id1 = (choice[i] as Risk).id; break;
                        case (2): riskChoise.Risk_id2 = (choice[i] as Risk).id; break;
                        case (3): riskChoise.Risk_id3 = (choice[i] as Risk).id; break;
                        case (4): riskChoise.Risk_id4 = (choice[i] as Risk).id; break;
                        case (5): riskChoise.Risk_id5 = (choice[i] as Risk).id; break;
                        case (6): riskChoise.Risk_id6 = (choice[i] as Risk).id; break;
                        case (7): riskChoise.Risk_id7 = (choice[i] as Risk).id; break;
                        case (8): riskChoise.Risk_id8 = (choice[i] as Risk).id; break;
                        case (9): riskChoise.Risk_id9 = (choice[i] as Risk).id; break;
                    }

                }
                else if (choice[i] is BusinessBlock)
                {
                    switch (i)
                    {
                        case (0): businessBlockChoice.BusinessBlockid = (choice[i] as BusinessBlock).Id; break;
                        case (1): businessBlockChoice.BusinessBlock_id1 = (choice[i] as BusinessBlock).Id; break;
                        case (2): businessBlockChoice.BusinessBlock_id2 = (choice[i] as BusinessBlock).Id; break;
                        case (3): businessBlockChoice.BusinessBlock_id3 = (choice[i] as BusinessBlock).Id; break;
                        case (4): businessBlockChoice.BusinessBlock_id4 = (choice[i] as BusinessBlock).Id; break;
                        case (5): businessBlockChoice.BusinessBlock_id5 = (choice[i] as BusinessBlock).Id; break;
                        case (6): businessBlockChoice.BusinessBlock_id6 = (choice[i] as BusinessBlock).Id; break;
                        case (7): businessBlockChoice.BusinessBlock_id7 = (choice[i] as BusinessBlock).Id; break;
                        case (8): businessBlockChoice.BusinessBlock_id8 = (choice[i] as BusinessBlock).Id; break;
                        case (9): businessBlockChoice.BusinessBlock_id9 = (choice[i] as BusinessBlock).Id; break;
                        case (10): businessBlockChoice.BusinessBlock_id10 = (choice[i] as BusinessBlock).Id; break;
                        case (11): businessBlockChoice.BusinessBlock_id11 = (choice[i] as BusinessBlock).Id; break;
                        case (12): businessBlockChoice.BusinessBlock_id12 = (choice[i] as BusinessBlock).Id; break;
                        case (13): businessBlockChoice.BusinessBlock_id13 = (choice[i] as BusinessBlock).Id; break;
                        case (14): businessBlockChoice.BusinessBlock_id14 = (choice[i] as BusinessBlock).Id; break;
                    }
                }
                else if (choice[i] is Escalations)
                {
                    switch (i)
                    {
                        case (0): escalationChoice.Escalation_id = (choice[i] as Escalations).Id; break;
                        case (1): escalationChoice.Escalation_id1 = (choice[i] as Escalations).Id; break;
                        case (2): escalationChoice.Escalation_id2 = (choice[i] as Escalations).Id; break;
                        case (3): escalationChoice.Escalation_id3 = (choice[i] as Escalations).Id; break;
                        case (4): escalationChoice.Escalation_id4 = (choice[i] as Escalations).Id; break;
                        case (5): escalationChoice.Escalation_id5 = (choice[i] as Escalations).Id; break;
                        case (6): escalationChoice.Escalation_id6 = (choice[i] as Escalations).Id; break;
                        case (7): escalationChoice.Escalation_id7 = (choice[i] as Escalations).Id; break;
                        case (8): escalationChoice.Escalation_id8 = (choice[i] as Escalations).Id; break;
                        case (9): escalationChoice.Escalation_id9 = (choice[i] as Escalations).Id; break;
                        case (10): escalationChoice.Escalation_id10 = (choice[i] as Escalations).Id; break;
                        case (11): escalationChoice.Escalation_id11 = (choice[i] as Escalations).Id; break;
                        case (12): escalationChoice.Escalation_id12 = (choice[i] as Escalations).Id; break;
                        case (13): escalationChoice.Escalation_id13 = (choice[i] as Escalations).Id; break;
                        case (14): escalationChoice.Escalation_id14 = (choice[i] as Escalations).Id; break;
                    }
                }
                else if (choice[i] is Supports)
                {
                    switch (i)
                    {
                        case (0): supportChoice.Support_id = (choice[i] as Supports).Id; break;
                        case (1): supportChoice.Support_id1 = (choice[i] as Supports).Id; break;
                        case (2): supportChoice.Support_id2 = (choice[i] as Supports).Id; break;
                        case (3): supportChoice.Support_id3 = (choice[i] as Supports).Id; break;
                        case (4): supportChoice.Support_id4 = (choice[i] as Supports).Id; break;
                        case (5): supportChoice.Support_id5 = (choice[i] as Supports).Id; break;
                        case (6): supportChoice.Support_id6 = (choice[i] as Supports).Id; break;
                        case (7): supportChoice.Support_id7 = (choice[i] as Supports).Id; break;
                        case (8): supportChoice.Support_id8 = (choice[i] as Supports).Id; break;
                        case (9): supportChoice.Support_id9 = (choice[i] as Supports).Id; break;
                        case (10): supportChoice.Support_id10 = (choice[i] as Supports).Id; break;
                        case (11): supportChoice.Support_id11 = (choice[i] as Supports).Id; break;
                        case (12): supportChoice.Support_id12 = (choice[i] as Supports).Id; break;
                        case (13): supportChoice.Support_id13 = (choice[i] as Supports).Id; break;
                        case (14): supportChoice.Support_id14 = (choice[i] as Supports).Id; break;
                    }
                }
            }
            if (!isEditState)
            {
                NewRecord.riskChoise = riskChoise;
                NewRecord.supportChoice = supportChoice;
                NewRecord.EscalationChoice = escalationChoice;
                NewRecord.BusinessBlockChoice = businessBlockChoice;
            }
            else
            {
                EditedRecord.riskChoise = riskChoise;
                EditedRecord.supportChoice = supportChoice;
                EditedRecord.EscalationChoice = escalationChoice;
                EditedRecord.BusinessBlockChoice = businessBlockChoice;
            }
        }

        private void GetReportMethod(IEnumerable<object> Analytics)
        {
            SelectedAnalytics.Clear();
            foreach (Analytic analytic in Analytics)
            {
                SelectedAnalytics.Add(analytic);
            }
            EFDataProvider.GetReport(SelectedReport, SelectedAnalytics.ToArray(), StartReportDate, EndReportDate);
        }

        private void CheckTimeForIntesectionMethod()
        {
            if (!(EditedRecord.timeStart >= initalTimeStart && EditedRecord.timeStart < initalTimeEnd) && EFDataProvider.IsCollisionedWithOtherRecords(EditedRecord))
            {
                IsTimeCorrect = false;
            }
            else
            {
                IsTimeCorrect = true;
            }
            RaisePropertyChanged("IsTimeCorrect");
        }

        private void LoadSelection(Process selectedProcess)
        {
            if (selectedProcess != null)
            {
                Selection loadedSelection = LocalWorker.GetSelection(selectedProcess.id);
                if (loadedSelection != null)
                {

                    NewRecord.BusinessBlockChoice = (BusinessBlockChoice)EFDataProvider.GetChoice(loadedSelection.BusinessBlockSelected, 0);
                    NewRecord.supportChoice = (supportChoice)EFDataProvider.GetChoice(loadedSelection.SupportSelected, 1);
                    NewRecord.EscalationChoice = (EscalationChoice)EFDataProvider.GetChoice(loadedSelection.EscalationSelected, 2);
                    NewRecord.riskChoise = (riskChoise)EFDataProvider.GetChoice(loadedSelection.RiskSelected, 3);
                    if ((NewRecord.ClientWays = Array.Find(ClientWays, i => i.Id == loadedSelection.ClientWaySelected)) == null)
                    {
                        NewRecord.ClientWays = ClientWays[0];
                    }
                    if ((NewRecord.Formats = Array.Find(FormatList, i => i.Id == loadedSelection.FormatSelected)) == null)
                    {
                        NewRecord.Formats = FormatList[0];
                    }
                    RaisePropertyChanged("NewRecord");
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
            Processes = EFDataProvider.GetProcesses();
            BusinessBlock = EFDataProvider.GetBusinessBlocks();
            SupportsArr = EFDataProvider.GetSupports();
            ClientWays = EFDataProvider.GetClientWays();
            FormatList = EFDataProvider.GetFormat();
            Escalations = EFDataProvider.GetEscalation();
            RiskCol = EFDataProvider.GetRisks();
            FilterProcessesMethod(string.Empty);
            SubordinateEmployees = EFDataProvider.GetMyAnalyticsData(CurrentUser);
            UpdateTimeSpan();
            defBlock = BusinessBlock[0];
        }
        private void UpdateTimeSpan()
        {
            HistoryRecords.Clear();
            foreach (TimeSheetTable record in EFDataProvider.LoadTimeSheetRecords(CurrentDate, CurrentUser))
            {
                HistoryRecords.Add(record);
            }
            RaisePropertyChanged("TotalDurationInMinutes");
        }
        private void GetReportMethod()
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
            Dictionary<Process, int> sortRule = new Dictionary<Process, int>();

            if (string.IsNullOrWhiteSpace(filterText))
            {
                foreach (Process proc in Processes)
                {
                    sortRule.Add(proc, LocalWorker.ChoosenCounter(proc.id));
                }
                foreach (KeyValuePair<Process, int> keyValue in sortRule.OrderByDescending(i => i.Value))
                {
                    ProcessFiltered.Add(keyValue.Key);
                }
                return;
            }
            foreach (Process process in Processes)
            {
                string codeFull = $"{process.Block_id}.{process.SubBlockId}.{process.id}";
                if (process.procName.ToLower().IndexOf(filterText.ToLower()) > -1 || codeFull.IndexOf(filterText) > -1)
                {
                    sortRule.Add(process, LocalWorker.ChoosenCounter(process.id));
                }
            }
            foreach (KeyValuePair<Process, int> keyValue in sortRule.OrderByDescending(i => i.Value))
            {
                ProcessFiltered.Add(keyValue.Key);
            }


        }
        private void AddProcessMethod(TimeSheetTable newItem)
        {
            #region UnitTest's
            if (IsIntersectsWithOtherRecords(newItem))
            {
                MessageBox.Show("Добавляемая запись пересекается с другой активностью. Выберите другое время.", "ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else if (newItem.timeStart == newItem.timeEnd)
            {
                MessageBox.Show("Время начала равно времени окончания. Укажите корректное время", "ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else if (newItem.timeStart > newItem.timeEnd)
            {
                MessageBox.Show("Время начала больше времени окончания. Укажите корректное время", "ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            #endregion



            #region Добавление в БД
            int riskID = EFDataProvider.AddRiskChoice(newItem.riskChoise);
            int BBID = EFDataProvider.AddBusinessBlockChoice(newItem.BusinessBlockChoice);
            int suppID = EFDataProvider.AddSupportChoiceSet(newItem.supportChoice);
            int escalID = EFDataProvider.AddEscalationChoice(newItem.EscalationChoice);
            TimeSheetTable clonedActivity = new TimeSheetTable()
            {
                AnalyticId = newItem.Analytic.Id,
                BusinessBlockChoice_id = BBID,
                ClientWaysId = newItem.ClientWays.Id,
                EscalationChoice_id = escalID,
                FormatsId = newItem.Formats.Id,
                Process_id = newItem.Process.id,
                id = newItem.id,
                riskChoise_id = riskID,
                Subject = newItem.Subject,
                comment = newItem.comment,
                supportChoice_id = suppID,
                timeStart = newItem.timeStart,
                timeEnd = newItem.timeEnd,
                TimeSpent = newItem.TimeSpent
            };
            EFDataProvider.AddActivity(clonedActivity);
            #endregion

            #region Обновление представления
            UpdateTimeSpan();
            RaisePropertyChanged("TotalDurationInMinutes");
            newItem.timeStart = newItem.timeEnd;
            newItem.timeEnd = newItem.timeEnd.AddMinutes(15);
            newItem.Subject = string.Empty;
            newItem.comment = string.Empty;
            RaisePropertyChanged("NewRecord");
            #endregion

            #region Запоминаем выбор
            LocalWorker.StoreSelection(new Selection(
                newItem.Process.id,
                newItem.BusinessBlockChoice_id,
                newItem.supportChoice_id,
                newItem.ClientWays.Id,
                newItem.EscalationChoice_id,
                newItem.Formats.Id,
                2));
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
                comment = Record.comment,
                Process = Record.Process,
                BusinessBlockChoice = Record.BusinessBlockChoice,
                supportChoice = Record.supportChoice,
                timeStart = Record.timeStart,
                timeEnd = Record.timeEnd,
                TimeSpent = Record.TimeSpent,
                ClientWays = Record.ClientWays,
                EscalationChoice = Record.EscalationChoice,
                Formats = Record.Formats,
                riskChoise = Record.riskChoise
            };
            initalTimeStart = Record.timeStart;
            initalTimeEnd = Record.timeEnd;
            isEditState = true;
            EditForm form = new EditForm();
            if (form.ShowDialog() == true)
            {
                CheckTimeForIntesectionMethod();
                int riskID = EFDataProvider.AddRiskChoice(EditedRecord.riskChoise);
                int BBID = EFDataProvider.AddBusinessBlockChoice(EditedRecord.BusinessBlockChoice);
                int suppID = EFDataProvider.AddSupportChoiceSet(EditedRecord.supportChoice);
                int escalID = EFDataProvider.AddEscalationChoice(EditedRecord.EscalationChoice);
                EditedRecord.riskChoise_id = riskID;
                EditedRecord.BusinessBlockChoice_id = BBID;
                EditedRecord.supportChoice_id = suppID;
                EditedRecord.EscalationChoice_id = escalID;
                EFDataProvider.UpdateProcess(Record, EditedRecord);
                UpdateTimeSpan();
            }
            isEditState = false;

        }
    }
}