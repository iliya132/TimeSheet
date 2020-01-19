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
        public ObservableCollection<BusinessBlock> GetBusinessBlocks()
        {
            return new ObservableCollection<BusinessBlock>(dataBase.BusinessBlock.ToArray());
        }

        public void AddActivity(TimeSheetTable activity)
        {
            TimeSpan span = activity.timeEnd - activity.timeStart;
            activity.TimeSpent = (int)span.TotalMinutes;
            dataBase.TimeSheetTable.Add(activity);
            dataBase.SaveChanges();
            
        }

        public void DeleteRecord(TimeSheetTable record)
        {
            dataBase.TimeSheetTable.Remove(record);
            dataBase.SaveChanges();
        }

        public bool ForcedToQuit()
        {
            return false;
        }

        public List<string> GetBlocksList()
        {
            return new List<string>(dataBase.Block.Select(i=>i.blockName).ToArray());
        }


        public ObservableCollection<ClientWays> GetClientWays()
        {
            return new ObservableCollection<ClientWays>(dataBase.ClientWays.ToArray());
        }

        public ObservableCollection<Escalations> GetEscalation()
        {
            return new ObservableCollection<Escalations>(dataBase.Escalations.ToArray());
        }

        public ObservableCollection<Formats> GetFormat()
        {
            return new ObservableCollection<Formats>(dataBase.Formats.ToArray());
        }

        public ObservableCollection<Analytic> GetMyAnalyticsData(Analytic currentUser)
        {
            return new ObservableCollection<Analytic>(dataBase.Analytic.Where(i => i.OtdelTableId == currentUser.OtdelTableId).ToArray());
        }



        public void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end)
        {
            return;
        }

        public ObservableCollection<Risk> GetRisks()
        {
            return new ObservableCollection<Risk>(dataBase.RiskSet.ToArray());
        }

        public List<string> GetSubBlocksList()
        {
            return new List<string>(dataBase.SubBlock.Select(i => i.subblockName).ToArray());
        }

        public ObservableCollection<Supports> GetSupports()
        {
            return new ObservableCollection<Supports>(dataBase.Supports.ToArray());
        }


        public Analytic LoadAnalyticData()
        {
            string user = Environment.UserName;
            return dataBase.Analytic.FirstOrDefault(i => i.userName.ToLower().Equals(Environment.UserName.ToLower()));
        }

        public Process LoadHistoryProcess(DateTime timeStart, Analytic user)
        {
            DbSet<TimeSheetTable> timeTable = dataBase.TimeSheetTable;
            return new Process();
        }

        public List<TimeSheetTable> LoadTimeSheetRecords(DateTime date, Analytic user)
        {
            return new List<TimeSheetTable>(dataBase.TimeSheetTable.Where(i => i.AnalyticId == user.Id && DbFunctions.TruncateTime(i.timeStart) == date.Date));
        }

        public void UpdateProcess(TimeSheetTable oldProcess, TimeSheetTable newProcess)
        {

            oldProcess.Subject = newProcess.Subject;
            oldProcess.comment = newProcess.comment;
            oldProcess.Process = newProcess.Process;
            oldProcess.BusinessBlock = newProcess.BusinessBlock;
            oldProcess.Supports = newProcess.Supports;
            oldProcess.Process = newProcess.Process;
            oldProcess.timeEnd = newProcess.timeEnd;
            oldProcess.TimeSpent = newProcess.TimeSpent;
            oldProcess.ClientWays = newProcess.ClientWays;
            oldProcess.Escalations = newProcess.Escalations;
            oldProcess.Formats = newProcess.Formats;
            oldProcess.riskChoise = newProcess.riskChoise;
            oldProcess.timeStart = newProcess.timeStart;
            
            dataBase.SaveChanges();
        }
        public bool IsCollisionedWithOtherRecords(TimeSheetTable record)
        {
            foreach(TimeSheetTable historyRecord in dataBase.TimeSheetTable.Where(i=>i.AnalyticId == record.AnalyticId))
            {
                if (isInInterval(record.timeStart, historyRecord.timeStart, historyRecord.timeEnd))
                    {
                    return true;
                    }
            }
            return false;
        }
        private bool isInInterval(DateTime checkedValue, DateTime intervalStart, DateTime intervalEnd)
        {
            if (checkedValue >= intervalStart && checkedValue < intervalEnd)
            {
                return true;
            }
            return false;
        }
    }
}
