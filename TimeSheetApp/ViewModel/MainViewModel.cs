using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
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

        #region MainForm multiChoiceCollections
        private ObservableCollection<Risk> _newRiskChoiceCollection = new ObservableCollection<Risk>();
        public ObservableCollection<Risk> NewRiskChoiceCollection { get => _newRiskChoiceCollection; set => _newRiskChoiceCollection = value; }
        private ObservableCollection<Escalation> _newEscalationsChoiceCollection = new ObservableCollection<Escalation>();
        public ObservableCollection<Escalation> NewEscalationsChoiceCollection { get => _newEscalationsChoiceCollection; set => _newEscalationsChoiceCollection = value; }
        private ObservableCollection<BusinessBlock> _newBusinessBlockChoiceCollection = new ObservableCollection<BusinessBlock>();
        public ObservableCollection<BusinessBlock> NewBusinessBlockChoiceCollection { get => _newBusinessBlockChoiceCollection; set => _newBusinessBlockChoiceCollection = value; }
        private ObservableCollection<Supports> _newSupportsChoiceCollection = new ObservableCollection<Supports>();
        public ObservableCollection<Supports> NewSupportsChoiceCollection { get => _newSupportsChoiceCollection; set => _newSupportsChoiceCollection = value; }
        #endregion

        public bool IgnoreSubjectTextChange { get; set; }
        /// <summary>
        /// подсказки для ввода текста в поле Тема
        /// </summary>

        private Stack<string> subjectsFromDB;
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
        private SupportChoice _currentSupports = new SupportChoice();
        public SupportChoice CurrentSupports
        {
            get { return _currentSupports; }
            set
            {
                NewRecord.SupportChoice = value;
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
                NewRecord.RiskChoice_id = value.Id;
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
            "Отчет по активности аналитиков",
            "Ресурсный план"
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

        #region Мультивыбор

        RiskChoice editRiskChoice = new RiskChoice();
        BusinessBlockChoice editBusinessBlockChoice = new BusinessBlockChoice();
        SupportChoice editSupportChoice = new SupportChoice();
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
                    totalSpan += record.TimeEnd - record.TimeStart;
                }
                return totalSpan;
            }
        }
        DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate { get => _currentDate; set => _currentDate = value; }
        private Analytic _currentUser = new Analytic();
        public Analytic CurrentUser { get => _currentUser; set => _currentUser = value; }
        private bool isEditState = false;

        private ObservableCollection<CalendarItem> _calendarItem = new ObservableCollection<CalendarItem>();

        public ObservableCollection<CalendarItem> CalendarItem
        {
            get { return _calendarItem; }
            set { _calendarItem = value; }
        }

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
        public RelayCommand StoreSelection { get; }
        public RelayCommand<AnalyticOrdered> SelectAnalytic { get; }
        public RelayCommand<AnalyticOrdered> UnselectAnalytic { get; }
        public RelayCommand ReportSelectionStore { get; }


        #endregion
        private void writeLog(string msg)
        {
            using (StreamWriter sw = new StreamWriter($"{Environment.UserName}_exception.txt", true)){
                sw.WriteLine(msg);
            }
        }
        public MainViewModel(IEFDataProvider dataProvider)
        {
            EFDataProvider = dataProvider;

            FillDataCollections();
            updateSubjectHints();
            AddProcess = new RelayCommand<TimeSheetTable>(AddProcessMethod);
            EditProcess = new RelayCommand<TimeSheetTable>(EditHistoryProcess);
            DeleteProcess = new RelayCommand<TimeSheetTable>(DeleteHistoryRecord);
            ReloadTimeSheet = new RelayCommand(UpdateTimeSpan);
            CheckTimeForIntersection = new RelayCommand(CheckTimeForIntesectionMethod);
            LoadSelectionForSelectedProcess = new RelayCommand<Process>(LoadSelection);
            FilterProcesses = new RelayCommand<string>(FilterProcessesMethod);
            GetReport = new RelayCommand(GetReportMethod);
            ReloadHistoryRecords = new RelayCommand(UpdateTimeSpan);
            StoreSelection = new RelayCommand(StoreMultiplyChoice);
            ReportSelectionStore = new RelayCommand(ReportSelectionUpdate);
            SelectAnalytic = new RelayCommand<AnalyticOrdered>(SelectAnalyticMethod);
            UnselectAnalytic = new RelayCommand<AnalyticOrdered>(UnselectAnalyticMethod);
            NewRecord.Analytic = CurrentUser;
            NewRecord.AnalyticId = CurrentUser.Id;
            GenerateNodes();
            CalendarItem = GetDominoCalendar();
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

                    if (string.IsNullOrEmpty(analytic.SecondStructure))
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

                    if (string.IsNullOrEmpty(analytic.ThirdStructure))
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

                    if (string.IsNullOrEmpty(analytic.FourStructure))
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
            
            for (int i = 0; i < 10 && i < itemsCount; i++)
            {
                if (subjectsFromDB.Count > 0)
                {
                    SubjectHints.Add(subjectsFromDB.Pop());
                }
            }

        }
        private void StoreMultiplyChoice()
        {
            editBusinessBlockChoice = new BusinessBlockChoice();
            editSupportChoice = new SupportChoice();
            editRiskChoice = new RiskChoice();
            editEscalationChoice = new EscalationChoice();
            ObservableCollection<BusinessBlock> currentBBCol;
            ObservableCollection<Escalation> currentEscCol;
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
                        case (0): editBusinessBlockChoice.BusinessBlock_id = currentBBCol[i].Id; break;
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
                        case (0): editRiskChoice.Risk_id = currentRiskCol[i].Id; break;
                        case (1): editRiskChoice.Risk_id1 = currentRiskCol[i].Id; break;
                        case (2): editRiskChoice.Risk_id2 = currentRiskCol[i].Id; break;
                        case (3): editRiskChoice.Risk_id3 = currentRiskCol[i].Id; break;
                        case (4): editRiskChoice.Risk_id4 = currentRiskCol[i].Id; break;
                        case (5): editRiskChoice.Risk_id5 = currentRiskCol[i].Id; break;
                        case (6): editRiskChoice.Risk_id6 = currentRiskCol[i].Id; break;
                        case (7): editRiskChoice.Risk_id7 = currentRiskCol[i].Id; break;
                        case (8): editRiskChoice.Risk_id8 = currentRiskCol[i].Id; break;
                        case (9): editRiskChoice.Risk_id9 = currentRiskCol[i].Id; break;

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
                NewRecord.RiskChoice = editRiskChoice;
                NewRecord.SupportChoice = editSupportChoice;
                NewRecord.EscalationChoice = editEscalationChoice;
                NewRecord.BusinessBlockChoice = editBusinessBlockChoice;
            }
            else
            {
                EditedRecord.RiskChoice = editRiskChoice;
                EditedRecord.SupportChoice = editSupportChoice;
                EditedRecord.EscalationChoice = editEscalationChoice;
                EditedRecord.BusinessBlockChoice = editBusinessBlockChoice;
            }

        }

        private void GetReportMethod()
        {
            if (SelectedAnalytics.Count() < 1)
            {
                MessageBox.Show("Не выбрано ни одного аналитика", "Выберите аналитика", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            EFDataProvider.GetReport(SelectedReport, SelectedAnalytics.ToArray(), StartReportDate, EndReportDate);
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
            NewBusinessBlockChoiceCollection.Clear();
            NewSupportsChoiceCollection.Clear();
            NewEscalationsChoiceCollection.Clear();
            NewRiskChoiceCollection.Clear();
            if (selectedProcess != null)
            {
                Selection loadedSelection = LocalWorker.GetSelection(selectedProcess.Id);

                if (loadedSelection != null)
                {
                    foreach (BusinessBlock block in EFDataProvider.GetChoice(loadedSelection.BusinessBlockSelected, 0))
                    {
                        NewBusinessBlockChoiceCollection.Add(block);
                    }
                    foreach (Supports support in EFDataProvider.GetChoice(loadedSelection.SupportSelected, 1))
                    {
                        NewSupportsChoiceCollection.Add(support);
                    }
                    foreach (Escalation escalation in EFDataProvider.GetChoice(loadedSelection.EscalationSelected, 2))
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
                    RaisePropertyChanged(nameof(NewRecord));
                }
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
            UpdateTimeSpan();
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
            HistoryRecords.Clear();
            foreach (TimeSheetTable record in EFDataProvider.LoadTimeSheetRecords(CurrentDate, CurrentUser))
            {
                HistoryRecords.Add(record);
            }
            RaisePropertyChanged(nameof(TotalDurationInMinutes));
        }

        private void FilterProcessesMethod(string filterText)
        {
            ProcessFiltered?.Clear();
            Dictionary<Process, int> sortRule = new Dictionary<Process, int>();

            if (string.IsNullOrWhiteSpace(filterText))
            {
                foreach (Process proc in Processes)
                {
                    sortRule.Add(proc, LocalWorker.ChoosenCounter(proc.Id));
                }
                foreach (KeyValuePair<Process, int> keyValue in sortRule.OrderByDescending(i => i.Value))
                {
                    ProcessFiltered.Add(keyValue.Key);
                }
                return;
            }
            foreach (Process process in Processes)
            {
                string codeFull = $"{process.Block_Id}.{process.SubBlock_Id}.{process.Id}";
                if (process.ProcName.ToLower().IndexOf(filterText.ToLower()) > -1 || codeFull.IndexOf(filterText) > -1)
                {
                    sortRule.Add(process, LocalWorker.ChoosenCounter(process.Id));
                }
            }
            foreach (KeyValuePair<Process, int> keyValue in sortRule.OrderByDescending(i => i.Value))
            {
                ProcessFiltered.Add(keyValue.Key);
            }


        }
        private void AddProcessMethod(TimeSheetTable newItem)
        {
            Console.WriteLine(newItem.Subject);

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
            }
            #endregion



            #region Добавление в БД
            int riskID = EFDataProvider.AddRiskChoice(newItem.RiskChoice);
            int BBID = EFDataProvider.AddBusinessBlockChoice(newItem.BusinessBlockChoice);
            int suppID = EFDataProvider.AddSupportChoiceSet(newItem.SupportChoice);
            int escalID = EFDataProvider.AddEscalationChoice(newItem.EscalationChoice);
            TimeSheetTable clonedActivity = new TimeSheetTable()
            {
                AnalyticId = newItem.Analytic.Id,
                BusinessBlockChoice_id = BBID,
                ClientWaysId = newItem.ClientWays.Id,
                EscalationChoice_id = escalID,
                FormatsId = newItem.Formats.Id,
                Process_id = newItem.Process.Id,
                Id = newItem.Id,
                RiskChoice_id = riskID,
                Subject = newItem.Subject,
                Comment = newItem.Comment,
                SupportChoice_id = suppID,
                TimeStart = newItem.TimeStart,
                TimeEnd = newItem.TimeEnd,
                TimeSpent = newItem.TimeSpent
            };
            EFDataProvider.AddActivity(clonedActivity);
            #endregion

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

            #region Запоминаем выбор
            LocalWorker.StoreSelection(new Selection(
                newItem.Process.Id,
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
                Comment = Record.Comment,
                Process = Record.Process,
                BusinessBlockChoice = Record.BusinessBlockChoice,
                SupportChoice = Record.SupportChoice,
                TimeStart = Record.TimeStart,
                TimeEnd = Record.TimeEnd,
                TimeSpent = Record.TimeSpent,
                ClientWays = Record.ClientWays,
                EscalationChoice = Record.EscalationChoice,
                Formats = Record.Formats,
                RiskChoice = Record.RiskChoice,
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

            foreach (BusinessBlock Bblock in EFDataProvider.LoadBusinessBlockChoice(Record.BusinessBlockChoice))
            {
                BusinessBlockChoiceCollection.Add(Bblock);
            }
            foreach (Risk risk in EFDataProvider.LoadRiskChoice(Record.RiskChoice))
            {
                RiskChoiceCollection.Add(risk);
            }
            foreach (Supports suport in EFDataProvider.LoadSupportsChoice(Record.SupportChoice))
            {
                SupportsChoiceCollection.Add(suport);
            }
            foreach (Escalation escalation in EFDataProvider.LoadEscalationChoice(Record.EscalationChoice))
            {
                EscalationsChoiceCollection.Add(escalation);
            }
            #endregion

            EditForm form = new EditForm();
            if (form.ShowDialog() == true)
            {
                CheckTimeForIntesectionMethod();
                int riskID = EFDataProvider.AddRiskChoice(EditedRecord.RiskChoice);
                int BBID = EFDataProvider.AddBusinessBlockChoice(EditedRecord.BusinessBlockChoice);
                int suppID = EFDataProvider.AddSupportChoiceSet(EditedRecord.SupportChoice);
                int escalID = EFDataProvider.AddEscalationChoice(EditedRecord.EscalationChoice);
                EditedRecord.RiskChoice_id = riskID;
                EditedRecord.BusinessBlockChoice_id = BBID;
                EditedRecord.SupportChoice_id = suppID;
                EditedRecord.EscalationChoice_id = escalID;
                EFDataProvider.UpdateProcess(Record, EditedRecord);

                UpdateTimeSpan();
            }
            isEditState = false;

        }

        private ObservableCollection<CalendarItem> GetDominoCalendar()
        {
            DominoWorker worker = new DominoWorker();
            return new ObservableCollection<CalendarItem>(worker.GetCalendarRecords());
        }
    }
}