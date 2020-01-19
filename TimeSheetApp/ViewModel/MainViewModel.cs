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
        private ObservableCollection<BusinessBlock> _BusinessBlock;
        public ObservableCollection<BusinessBlock> BusinessBlock { get => _BusinessBlock; set => _BusinessBlock = value; }
        /// <summary>
        /// Support's доступные для выбора
        /// </summary>
        private ObservableCollection<Supports> _NonBusinessBlock;
        public ObservableCollection<Supports> NonBusinessBlock { get => _NonBusinessBlock; set => _NonBusinessBlock = value; }
        /// <summary>
        /// Клиентские пути, доступные для выбора
        /// </summary>
        private ObservableCollection<ClientWays> _clientWays;
        public ObservableCollection<ClientWays> ClientWays { get => _clientWays; set => _clientWays = value; }
        /// <summary>
        /// Форматы доступные для выбора
        /// </summary>
        private ObservableCollection<Formats> _formatList;
        public ObservableCollection<Formats> FormatList { get => _formatList; set => _formatList = value; }
        /// <summary>
        /// Риски, доступные для выбора
        /// </summary>
        private ObservableCollection<Risk> _riskCol;
        public ObservableCollection<Risk> RiskCol { get => _riskCol; set => _riskCol = value; }
        /// <summary>
        /// Эскалации, доступные для выбора
        /// </summary>
        private ObservableCollection<Escalations> _escalations;
        public ObservableCollection<Escalations> Escalations { get => _escalations; set => _escalations = value; }
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
        #endregion

        public MainViewModel(IEFDataProvider dataProvider)
        {
            EFDataProvider = dataProvider;
            FillDataCollections();
            AddProcess = new RelayCommand<TimeSheetTable>(AddProcessMethod);
            EditProcess = new RelayCommand<TimeSheetTable>(EditHistoryProcess);
            DeleteProcess = new RelayCommand<TimeSheetTable>(DeleteHistoryRecord);
            ReloadTimeSheet = new RelayCommand(UpdateTimeSpan);
            FilterProcesses = new RelayCommand<string>(FilterProcessesMethod);
            ExportReport = new RelayCommand(GetReportMethod);
            NewRecord.Analytic = CurrentUser;
            NewRecord.AnalyticId = CurrentUser.Id;
            NewRecord.riskChoise_id = 2;
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
            NonBusinessBlock = EFDataProvider.GetSupports();
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
            if (string.IsNullOrWhiteSpace(filterText))
            {
                foreach (Process proc in Processes)
                    ProcessFiltered.Add(proc);
                return;
            }
            foreach (Process process in Processes)
            {
                string codeFull = $"{process.Block_id}.{process.SubBlockId}.{process.id}";
                if (process.procName.ToLower().IndexOf(filterText.ToLower()) > -1 || codeFull.IndexOf(filterText) > -1)
                {
                    ProcessFiltered.Add(process);
                }
            }

        }

        private void AddProcessMethod(TimeSheetTable newItem)
        {
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

            TimeSheetTable clonedActivity = new TimeSheetTable()
            {
                AnalyticId = newItem.Analytic.Id,
                BusinessBlockId = newItem.BusinessBlock.Id,
                ClientWaysId = newItem.ClientWays.Id,
                EscalationsId = newItem.Escalations.Id,
                FormatsId = newItem.Formats.Id,
                Process_id = newItem.Process.id,
                id = newItem.id,
                riskChoise_id = 2,
                Subject = newItem.Subject,
                comment = newItem.comment,
                SupportsId = newItem.Supports.Id,
                timeStart = newItem.timeStart,
                timeEnd = newItem.timeEnd,
                TimeSpent = newItem.TimeSpent,
            };
            EFDataProvider.AddActivity(clonedActivity);
            UpdateTimeSpan();
            RaisePropertyChanged("TotalDurationInMinutes");
            newItem.timeStart = newItem.timeEnd;
            newItem.timeEnd = newItem.timeEnd.AddMinutes(15);
            newItem.Subject = string.Empty;
            newItem.comment = string.Empty;
            RaisePropertyChanged("NewRecord");
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
            EditForm form = new EditForm();
            if (form.ShowDialog() == true)
            {
                EFDataProvider.UpdateProcess(Record, EditedRecord);
                UpdateTimeSpan();
            }

        }
    }
}