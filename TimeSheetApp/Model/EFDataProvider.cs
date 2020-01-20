using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Data;

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
        public BusinessBlock[] GetBusinessBlocks()
        {
            return dataBase.BusinessBlock.ToArray();
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


        public ClientWays[] GetClientWays()
        {
            return dataBase.ClientWays.ToArray();
        }

        public Escalations[] GetEscalation()
        {
            return dataBase.Escalations.ToArray();
        }

        public Formats[] GetFormat()
        {
            return dataBase.Formats.ToArray();
        }

        public ObservableCollection<Analytic> GetMyAnalyticsData(Analytic currentUser)
        {
            return new ObservableCollection<Analytic>(dataBase.Analytic.Where(i => i.OtdelTableId == currentUser.OtdelTableId).ToArray());
        }



        public void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end)
        {
            switch (ReportType)
            {
                case (0):
                    ExcelWorker.ExportDataTableToExcel(GetAnalyticsReport(analytics, start, end));
                    break;
            }
        }

        public Risk[] GetRisks()
        {
            return dataBase.RiskSet.ToArray();
        }

        public List<string> GetSubBlocksList()
        {
            return new List<string>(dataBase.SubBlock.Select(i => i.subblockName).ToArray());
        }

        public Supports[] GetSupports()
        {
            return dataBase.Supports.ToArray();
        }


        public Analytic LoadAnalyticData()
        {
            string user = Environment.UserName;
            if (dataBase.Analytic.Any(i=>i.userName==user))
            {
                return dataBase.Analytic.FirstOrDefault(i => i.userName.ToLower().Equals(Environment.UserName.ToLower()));
            }
            else
            {
                dataBase.Analytic.Add(new Analytic() { userName = user,DepartmentsId=1, DirectionsId=1, FirstName="NotSet", LastName="NotSet",
                FatherName="NotSet", OtdelTableId=1, PositionsId=1, RoleTableId=1, UpravlenieTableId=1});
                dataBase.SaveChanges();
                return dataBase.Analytic.FirstOrDefault(i => i.userName.ToLower().Equals(Environment.UserName.ToLower()));
            }
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
                if (isInInterval(record.timeStart, record.timeEnd, historyRecord.timeStart, historyRecord.timeEnd))
                    {
                        return true;
                    }
            }
            return false;
        }
        private bool isInInterval(DateTime checkedValueStart, DateTime checkedValueEnd, DateTime intervalStart, DateTime intervalEnd)
        {
            if ((checkedValueStart >= intervalStart && checkedValueStart < intervalEnd) || //начальная дата в интервале
                (checkedValueEnd > intervalStart && checkedValueEnd <= intervalEnd) || //конечная дата в интервале
                (checkedValueStart < intervalStart && checkedValueEnd > intervalEnd)) //промежуток времени между датами включает интервал
            {
                return true;
            }
            return false;
        }

        public DataTable GetAnalyticsReport(Analytic[] analytics, DateTime timeStart, DateTime timeEnd)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("LastName");
            dataTable.Columns.Add("FirstName");
            dataTable.Columns.Add("FatherName");
            dataTable.Columns.Add("BlockName");
            dataTable.Columns.Add("SubBlockName");
            dataTable.Columns.Add("ProcessName");
            dataTable.Columns.Add("Subject");
            dataTable.Columns.Add("Body");
            dataTable.Columns.Add("timeStart");
            dataTable.Columns.Add("timeEnd");
            dataTable.Columns.Add("TimeSpent");
            dataTable.Columns.Add("BusinessBlockName");
            dataTable.Columns.Add("SupportsName");
            dataTable.Columns.Add("ClientWaysName");
            dataTable.Columns.Add("EscalationsName");
            dataTable.Columns.Add("FormatsName");
            dataTable.Columns.Add("RiskName");

            foreach(Analytic analytic in analytics)
            {
                List<TimeSheetTable> ReportEntity = new List<TimeSheetTable>();
                ReportEntity = dataBase.TimeSheetTable.Where(
                    record => record.AnalyticId == analytic.Id &&
                    record.timeStart > timeStart && record.timeStart < timeEnd).ToList();
                for (int i = 0; i < ReportEntity.Count; i++)
                {
                DataRow row = dataTable.Rows.Add();
                row["LastName"] = ReportEntity[i].Analytic.LastName;
                row["FirstName"] = ReportEntity[i].Analytic.FirstName;
                row["FatherName"] = ReportEntity[i].Analytic.FatherName;
                row["BlockName"] = ReportEntity[i].Process.Block1.blockName;
                row["SubBlockName"] = ReportEntity[i].Process.SubBlockNav.subblockName;
                row["ProcessName"] = ReportEntity[i].Process.procName;
                row["Subject"] = ReportEntity[i].Subject;
                row["Body"] = ReportEntity[i].comment;
                row["timeStart"] = ReportEntity[i].timeStart;
                row["timeEnd"] = ReportEntity[i].timeEnd;
                row["TimeSpent"] = ReportEntity[i].TimeSpent;
                row["BusinessBlockName"] = ReportEntity[i].BusinessBlock.BusinessBlockName;
                row["SupportsName"] = ReportEntity[i].Supports.Name;
                row["ClientWaysName"] = ReportEntity[i].ClientWays.Name;
                row["EscalationsName"] = ReportEntity[i].Escalations.Name;
                row["FormatsName"] = ReportEntity[i].Formats.Name;
                row["RiskName"] = 0;
                }
            }
            return dataTable;
        }
    }
}
/*
 * 
*/