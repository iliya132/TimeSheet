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
        private BusinessBlock _currentBusinessBlock = new BusinessBlock();

        public BusinessBlock CurrentBusinessBlock
        {
            get { return _currentBusinessBlock; }
            set {
                NewRecord.BusinessBlockId = value.Id;
                _currentBusinessBlock = value;
            }
        }
        private Supports _currentSupports = new Supports();
        public Supports CurrentSupports
        {
            get { return _currentSupports; }
            set {
                NewRecord.SupportsId = value.Id;
                _currentSupports = value;
            }
        }
        private ClientWays _currentClientWays = new ClientWays();
        public ClientWays CurrentClientWays
        {
            get { return _currentClientWays; }
            set {
                NewRecord.ClientWaysId = value.Id;
                _currentClientWays = value;
            }
        }
        private Escalations _currentEscalation = new Escalations();
        public Escalations CurrentEscalation
        {
            get { return _currentEscalation; }
            set {
                NewRecord.EscalationsId = value.Id;
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
        public bool IsTimeCorrect { get=>_isTimeCorrect; set=>_isTimeCorrect=value; }

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

        #region Мультивыбор риск
        riskChoise riskChoise = new riskChoise();
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
        }

        private void StoreMultiplyChoice(IList<object> choice)
        {
            int countOfChoice = choice.Count();
            int counter = 0;
            for (int i = 0; i < countOfChoice; i++)
            {
                if (choice[i] is Risk)
                {
                    riskChoise[counter] = (choice[i] as Risk).id;
                    counter++;
                }
            }
            NewRecord.riskChoise = riskChoise;
        }

        private void GetReportMethod(IEnumerable<object> Analytics)
        {
            SelectedAnalytics.Clear();
            foreach(Analytic analytic in Analytics)
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
                    NewRecord.BusinessBlock = Array.Find(BusinessBlock, i => i.Id == loadedSelection.BusinessBlockSelected);
                    NewRecord.Supports = Array.Find(SupportsArr, i => i.Id == loadedSelection.SupportSelected);
                    NewRecord.ClientWays = Array.Find(ClientWays, i => i.Id == loadedSelection.ClientWaySelected);
                    NewRecord.Formats = Array.Find(FormatList, i => i.Id == loadedSelection.FormatSelected);
                    NewRecord.Escalations = Array.Find(Escalations, i => i.Id == loadedSelection.EscalationSelected);
                    //TODO: NewRecord.riskChoise = Array.Find(risk, i => i.Id == loadedSelection.BusinessBlockSelected);
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
            TimeSheetTable clonedActivity = new TimeSheetTable()
            {
                AnalyticId = newItem.Analytic.Id,
                BusinessBlockId = newItem.BusinessBlock.Id,
                ClientWaysId = newItem.ClientWays.Id,
                EscalationsId = newItem.Escalations.Id,
                FormatsId = newItem.Formats.Id,
                Process_id = newItem.Process.id,
                id = newItem.id,
                riskChoise_id = riskID,
                Subject = newItem.Subject,
                comment = newItem.comment,
                SupportsId = newItem.Supports.Id,
                timeStart = newItem.timeStart,
                timeEnd = newItem.timeEnd,
                TimeSpent = newItem.TimeSpent,
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
                newItem.BusinessBlock.Id,
                newItem.Supports.Id,
                newItem.ClientWays.Id,
                newItem.Escalations.Id,
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
                BusinessBlock = Record.BusinessBlock,
                Supports = Record.Supports,
                timeStart = Record.timeStart,
                timeEnd = Record.timeEnd,
                TimeSpent = Record.TimeSpent,
                ClientWays = Record.ClientWays,
                Escalations = Record.Escalations,
                Formats = Record.Formats,
                riskChoise = Record.riskChoise
            };
            initalTimeStart = Record.timeStart;
            initalTimeEnd = Record.timeEnd;
            EditForm form = new EditForm();
            if (form.ShowDialog() == true)
            {
                EFDataProvider.UpdateProcess(Record, EditedRecord);
                UpdateTimeSpan();
                
            }

        }
    }
}