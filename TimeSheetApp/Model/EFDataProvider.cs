using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace TimeSheetApp.Model
{
    class EFDataProvider:IEFDataProvider
    {
        Model.TimeSheetDBEntities dataBase = new TimeSheetDBEntities();
        public EFDataProvider()
        {
            
        }
        public string GetCodeDescription(Process process)=> $"{process.Block1.blockName}\r\n{process.SubBlockNav.subblockName}\r\n{process.procName}";
        public ObservableCollection<Process> GetProcesses()
        {
            return new ObservableCollection<Process>(dataBase.Process);
        }
        public ObservableCollection<string> GetBusinessBlocks()
        {
            return new ObservableCollection<string>(dataBase.BusinessBlock.Select(i=>i.BusinessBlockName).ToArray());
        }

        public void AddActivity(Process activity)
        {
            dataBase.Process.Add(activity);
            dataBase.SaveChanges();
        }

        public int DeleteProcess(DateTime timeStart, Analytic analytic)
        {
            return 0;
        }

        public bool ForcedToQuit()
        {
            return false;
        }

        public List<string> GetBlocksList()
        {
            return new List<string>(dataBase.Block.Select(i=>i.blockName).ToArray());
        }


        public ObservableCollection<string> GetClientWays()
        {
            return new ObservableCollection<string>(dataBase.ClientWays.Select(i=>i.Name).ToArray());
        }

        public ObservableCollection<string> GetEscalation()
        {
            return new ObservableCollection<string>(dataBase.Escalations.Select(i=>i.Name).ToArray());
        }

        public ObservableCollection<string> GetFormat()
        {
            return new ObservableCollection<string>(dataBase.Formats.Select(i => i.Name).ToArray());
        }

        public ObservableCollection<Analytic> GetMyAnalyticsData(Analytic currentUser)
        {
            return new ObservableCollection<Analytic>(dataBase.Analytic.Where(i => i.OtdelTableId == currentUser.OtdelTableId).ToArray());
        }



        public void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end)
        {
            return;
        }

        public ObservableCollection<string> GetRisks()
        {
            return new ObservableCollection<string>(dataBase.RiskSet.Select(i => i.riskName).ToArray());
        }

        public List<string> GetSubBlocksList()
        {
            return new List<string>(dataBase.SubBlock.Select(i => i.subblockName).ToArray());
        }

        public ObservableCollection<string> GetSupports()
        {
            return new ObservableCollection<string>(dataBase.Supports.Select(i => i.Name).ToArray());
        }

        public ObservableCollection<TimeSheetHistoryItem> GetTimeSheetItem()
        {
            return new ObservableCollection<TimeSheetHistoryItem>();
        }

        public Analytic LoadAnalyticData()
        {
            string user = Environment.UserName;
            Console.WriteLine(Environment.UserName);
            return dataBase.Analytic.FirstOrDefault(i => i.userName.ToLower().Equals(Environment.UserName.ToLower()));
        }

        public Process LoadHistoryProcess(DateTime timeStart, Analytic user)
        {
            DbSet<TimeSheetTable> timeTable = dataBase.TimeSheetTable;
            return new Process();
        }

        public ObservableCollection<TimeSheetTable> LoadTimeSpan(DateTime date, Analytic user)
        {
            return new ObservableCollection<TimeSheetTable>(dataBase.TimeSheetTable.Where(i => i.AnalyticId == user.Id && DbFunctions.TruncateTime(i.timeStart) == date.Date));
        }

        public int UpdateProcess(Process oldProcess, Process newProcess)
        {
            dataBase.Process.Remove(oldProcess);
            dataBase.Process.Add(newProcess);
            dataBase.SaveChanges();
            return 0;
        }
    }
}
