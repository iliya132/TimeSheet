using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        #region editForm multiChoice
        private ObservableCollection<Risk> riskChoiceCollection = new ObservableCollection<Risk>();
        public ObservableCollection<Risk> RiskChoiceCollection { get => riskChoiceCollection; set => riskChoiceCollection = value; }
        private ObservableCollection<Escalations> _escalationsChoiceCollection = new ObservableCollection<Escalations>();
        public ObservableCollection<Escalations> EscalationsChoiceCollection { get => _escalationsChoiceCollection; set => _escalationsChoiceCollection = value; }
        private ObservableCollection<BusinessBlock> _businessBlockChoiceCollection = new ObservableCollection<BusinessBlock>();
        public ObservableCollection<BusinessBlock> BusinessBlockChoiceCollection { get => _businessBlockChoiceCollection; set => _businessBlockChoiceCollection = value; }
        private ObservableCollection<Supports> _supportsChoiceCollection = new ObservableCollection<Supports>();
        public ObservableCollection<Supports> SupportsChoiceCollection { get => _supportsChoiceCollection; set => _supportsChoiceCollection = value; }
        #endregion
        #region MainForm multiChoiceCollections
        private ObservableCollection<Risk> _newRiskChoiceCollection = new ObservableCollection<Risk>();
        public ObservableCollection<Risk> NewRiskChoiceCollection { get => _newRiskChoiceCollection; set => _newRiskChoiceCollection = value; }
        private ObservableCollection<Escalations> _newEscalationsChoiceCollection = new ObservableCollection<Escalations>();
        public ObservableCollection<Escalations> NewEscalationsChoiceCollection { get => _newEscalationsChoiceCollection; set => _newEscalationsChoiceCollection = value; }
        private ObservableCollection<BusinessBlock> _newBusinessBlockChoiceCollection = new ObservableCollection<BusinessBlock>();
        public ObservableCollection<BusinessBlock> NewBusinessBlockChoiceCollection { get => _newBusinessBlockChoiceCollection; set => _newBusinessBlockChoiceCollection = value; }
        private ObservableCollection<Supports> _newSupportsChoiceCollection = new ObservableCollection<Supports>();
        public ObservableCollection<Supports> NewSupportsChoiceCollection { get => _newSupportsChoiceCollection; set => _newSupportsChoiceCollection = value; }
        #endregion
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

        riskChoise editRiskChoice = new riskChoise();
        BusinessBlockChoice editBusinessBlockChoice = new BusinessBlockChoice();
        supportChoice editSupportChoice = new supportChoice();
        EscalationChoice editEscalationChoice = new EscalationChoice();
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
        public RelayCommand StoreSelection { get; }

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
            StoreSelection = new RelayCommand(StoreMultiplyChoice);
            NewRecord.Analytic = CurrentUser;
            NewRecord.AnalyticId = CurrentUser.Id;
            NewRecord.riskChoise_id = 2;
            ReportAcess = EFDataProvider.isAnalyticHasAccess(CurrentUser);
            RaisePropertyChanged("ReportAcess");
        }

        private void StoreMultiplyChoice()
        {
            editBusinessBlockChoice = new BusinessBlockChoice();
            editSupportChoice = new supportChoice();
            editRiskChoice = new riskChoise();
            editEscalationChoice = new EscalationChoice();
            ObservableCollection<BusinessBlock> currentBBCol;
            ObservableCollection<Escalations> currentEscCol;
            ObservableCollection<Supports> currentSupCol;
            ObservableCollection<Risk> currentRiskCol;
            if (isEditState)
            {
                currentBBCol = BusinessBlockChoiceCollection;
                currentSupCol = SupportsChoiceCollection;
                currentRiskCol = RiskChoiceCollection;
                currentEscCol = EscalationsChoiceCollection;
            }
            else
            {
                currentBBCol = NewBusinessBlockChoiceCollection;
                currentSupCol = NewSupportsChoiceCollection;
                currentRiskCol = NewRiskChoiceCollection;
                currentEscCol = NewEscalationsChoiceCollection;
            }
            if (currentBBCol.Count > 0)
            {
                for (int i = 0; i < currentBBCol.Count; i++)
                {
                    switch (i)
                    {
                        case (0): editBusinessBlockChoice.BusinessBlockid = currentBBCol[i].Id; break;
                        case (1): editBusinessBlockChoice.BusinessBlock_id1 = currentBBCol[i].Id; break;
                        case (2): editBusinessBlockChoice.BusinessBlock_id2 = currentBBCol[i].Id; break;
                        case (3): editBusinessBlockChoice.BusinessBlock_id3 = currentBBCol[i].Id; break;
                        case (4): editBusinessBlockChoice.BusinessBlock_id4 = currentBBCol[i].Id; break;
                        case (5): editBusinessBlockChoice.BusinessBlock_id5 = currentBBCol[i].Id; break;
                        case (6): editBusinessBlockChoice.BusinessBlock_id6 = currentBBCol[i].Id; break;
                        case (7): editBusinessBlockChoice.BusinessBlock_id7 = currentBBCol[i].Id; break;
                        case (8): editBusinessBlockChoice.BusinessBlock_id8 = currentBBCol[i].Id; break;
                        case (9): editBusinessBlockChoice.BusinessBlock_id9 = currentBBCol[i].Id; break;
                        case (10): editBusinessBlockChoice.BusinessBlock_id10 = currentBBCol[i].Id; break;
                        case (11): editBusinessBlockChoice.BusinessBlock_id11 = currentBBCol[i].Id; break;
                        case (12): editBusinessBlockChoice.BusinessBlock_id12 = currentBBCol[i].Id; break;
                        case (13): editBusinessBlockChoice.BusinessBlock_id13 = currentBBCol[i].Id; break;
                        case (14): editBusinessBlockChoice.BusinessBlock_id14 = currentBBCol[i].Id; break;
                    }
                }
            }
            if (currentSupCol.Count > 0)
            {
                for (int i = 0; i < currentSupCol.Count; i++)
                {
                    switch (i)
                    {
                        case (0): editSupportChoice.Support_id = currentSupCol[i].Id; break;
                        case (1): editSupportChoice.Support_id1 = currentSupCol[i].Id; break;
                        case (2): editSupportChoice.Support_id2 = currentSupCol[i].Id; break;
                        case (3): editSupportChoice.Support_id3 = currentSupCol[i].Id; break;
                        case (4): editSupportChoice.Support_id4 = currentSupCol[i].Id; break;
                        case (5): editSupportChoice.Support_id5 = currentSupCol[i].Id; break;
                        case (6): editSupportChoice.Support_id6 = currentSupCol[i].Id; break;
                        case (7): editSupportChoice.Support_id7 = currentSupCol[i].Id; break;
                        case (8): editSupportChoice.Support_id8 = currentSupCol[i].Id; break;
                        case (9): editSupportChoice.Support_id9 = currentSupCol[i].Id; break;
                        case (10): editSupportChoice.Support_id10 = currentSupCol[i].Id; break;
                        case (11): editSupportChoice.Support_id11 = currentSupCol[i].Id; break;
                        case (12): editSupportChoice.Support_id12 = currentSupCol[i].Id; break;
                        case (13): editSupportChoice.Support_id13 = currentSupCol[i].Id; break;
                        case (14): editSupportChoice.Support_id14 = currentSupCol[i].Id; break;
                    }
                }
            }
            if (currentRiskCol.Count > 0)
            {
                for (int i = 0; i < currentRiskCol.Count; i++)
                {
                    switch (i)
                    {
                        case (0): editRiskChoice.Risk_id = currentRiskCol[i].id; break;
                        case (1): editRiskChoice.Risk_id1 = currentRiskCol[i].id; break;
                        case (2): editRiskChoice.Risk_id2 = currentRiskCol[i].id; break;
                        case (3): editRiskChoice.Risk_id3 = currentRiskCol[i].id; break;
                        case (4): editRiskChoice.Risk_id4 = currentRiskCol[i].id; break;
                        case (5): editRiskChoice.Risk_id5 = currentRiskCol[i].id; break;
                        case (6): editRiskChoice.Risk_id6 = currentRiskCol[i].id; break;
                        case (7): editRiskChoice.Risk_id7 = currentRiskCol[i].id; break;
                        case (8): editRiskChoice.Risk_id8 = currentRiskCol[i].id; break;
                        case (9): editRiskChoice.Risk_id9 = currentRiskCol[i].id; break;

                    }
                }
            }
            if (currentEscCol.Count > 0)
            {
                for (int i = 0; i < currentEscCol.Count; i++)
                {
                    switch (i)
                    {
                        case (0): editEscalationChoice.Escalation_id = currentEscCol[i].Id; break;
                        case (1): editEscalationChoice.Escalation_id1 = currentEscCol[i].Id; break;
                        case (2): editEscalationChoice.Escalation_id2 = currentEscCol[i].Id; break;
                        case (3): editEscalationChoice.Escalation_id3 = currentEscCol[i].Id; break;
                        case (4): editEscalationChoice.Escalation_id4 = currentEscCol[i].Id; break;
                        case (5): editEscalationChoice.Escalation_id5 = currentEscCol[i].Id; break;
                        case (6): editEscalationChoice.Escalation_id6 = currentEscCol[i].Id; break;
                        case (7): editEscalationChoice.Escalation_id7 = currentEscCol[i].Id; break;
                        case (8): editEscalationChoice.Escalation_id8 = currentEscCol[i].Id; break;
                        case (9): editEscalationChoice.Escalation_id9 = currentEscCol[i].Id; break;
                        case (10): editEscalationChoice.Escalation_id10 = currentEscCol[i].Id; break;
                        case (11): editEscalationChoice.Escalation_id11 = currentEscCol[i].Id; break;
                        case (12): editEscalationChoice.Escalation_id12 = currentEscCol[i].Id; break;
                        case (13): editEscalationChoice.Escalation_id13 = currentEscCol[i].Id; break;
                        case (14): editEscalationChoice.Escalation_id14 = currentEscCol[i].Id; break;
                    }
                }
            }

            if (!isEditState)
            {
                NewRecord.riskChoise = editRiskChoice;
                NewRecord.supportChoice = editSupportChoice;
                NewRecord.EscalationChoice = editEscalationChoice;
                NewRecord.BusinessBlockChoice = editBusinessBlockChoice;
            }
            else
            {
                EditedRecord.riskChoise = editRiskChoice;
                EditedRecord.supportChoice = editSupportChoice;
                EditedRecord.EscalationChoice = editEscalationChoice;
                EditedRecord.BusinessBlockChoice = editBusinessBlockChoice;
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
            NewBusinessBlockChoiceCollection.Clear();
            NewSupportsChoiceCollection.Clear();
            NewEscalationsChoiceCollection.Clear();
            NewRiskChoiceCollection.Clear();
            if (selectedProcess != null)
            {
                Selection loadedSelection = LocalWorker.GetSelection(selectedProcess.id);
                
                if (loadedSelection != null)
                {
                    foreach(BusinessBlock block in EFDataProvider.GetChoice(loadedSelection.BusinessBlockSelected, 0))
                    {
                        NewBusinessBlockChoiceCollection.Add(block);
                    }
                    foreach (Supports support in EFDataProvider.GetChoice(loadedSelection.SupportSelected, 1))
                    {
                        NewSupportsChoiceCollection.Add(support);
                    }
                    foreach (Escalations escalation in EFDataProvider.GetChoice(loadedSelection.EscalationSelected, 2))
                    {
                        NewEscalationsChoiceCollection.Add(escalation);
                    }
                    foreach (Risk risk in EFDataProvider.GetChoice(loadedSelection.RiskSelected, 3))
                    {
                        NewRiskChoiceCollection.Add(risk);
                    }
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
                BBID,
                suppID,
                newItem.ClientWays.Id,
                escalID,
                newItem.Formats.Id,
                riskID));
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

            #region LoadSelection
            BusinessBlockChoiceCollection.Clear();
            RiskChoiceCollection.Clear();
            EscalationsChoiceCollection.Clear();
            SupportsChoiceCollection.Clear();

            foreach (BusinessBlock Bblock in EFDataProvider.LoadBusinessBlockChoice(Record.BusinessBlockChoice))
            {
                BusinessBlockChoiceCollection.Add(Bblock);
            }
            foreach (Risk risk in EFDataProvider.LoadRiskChoice(Record.riskChoise))
            {
                RiskChoiceCollection.Add(risk);
            }
            foreach (Supports suport in EFDataProvider.LoadSupportsChoice(Record.supportChoice))
            {
                SupportsChoiceCollection.Add(suport);
            }
            foreach (Escalations escalation in EFDataProvider.LoadEscalationChoice(Record.EscalationChoice))
            {
                EscalationsChoiceCollection.Add(escalation);
            }
            #endregion

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