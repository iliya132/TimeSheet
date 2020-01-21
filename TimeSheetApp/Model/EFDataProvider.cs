using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Windows;

namespace TimeSheetApp.Model
{
    class EFDataProvider : IEFDataProvider
    {
        Model.TimeSheetDBEntities dataBase = new TimeSheetDBEntities();
        public EFDataProvider()
        {

        }
        public string GetCodeDescription(Process process) => $"{process.Block1.blockName}\r\n{process.SubBlockNav.subblockName}\r\n{process.procName}";
        public ObservableCollection<Process> GetProcesses()
        {
            return new ObservableCollection<Process>(dataBase.Process);
        }
        public BusinessBlock[] GetBusinessBlocks()
        {
            return dataBase.BusinessBlock.ToArray();
        }
        /// <summary>
        /// Возвращает объект из модели соответствующий входящему id
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <param name="Type">0-BusinessBlock, 1-Supports, 2-Escalation, 3-RiskChoice</param>
        /// <returns></returns>
        public object GetChoice(int ObjectId, int Type)
        {
            switch (Type)
            {
                case (0):
                    return dataBase.BusinessBlockChoiceSet.FirstOrDefault(i => i.id == ObjectId);
                case (1):
                    return dataBase.supportChoiceSet.FirstOrDefault(i => i.id == ObjectId);
                case (2):
                    return dataBase.EscalationChoiceSet.FirstOrDefault(i => i.id == ObjectId);
                case (3):
                    return dataBase.riskChoise.FirstOrDefault(i => i.id == ObjectId);
                default:
                    return null;
            }

        }

        public int AddRiskChoice(riskChoise riskChoise)
        {
            if (riskChoise == null)
            {
                riskChoise = new riskChoise()
                {
                    Risk_id = 1
                };
            }
            if (dataBase.riskChoise.Any(i =>
            i.Risk_id == riskChoise.Risk_id &&
            i.Risk_id1 == riskChoise.Risk_id1 &&
            i.Risk_id2 == riskChoise.Risk_id2 &&
            i.Risk_id3 == riskChoise.Risk_id3 &&
            i.Risk_id4 == riskChoise.Risk_id4 &&
            i.Risk_id5 == riskChoise.Risk_id5 &&
            i.Risk_id6 == riskChoise.Risk_id6 &&
            i.Risk_id7 == riskChoise.Risk_id7 &&
            i.Risk_id8 == riskChoise.Risk_id8
            ))
            {
                return dataBase.riskChoise.First(
            i => i.Risk_id == riskChoise.Risk_id &&
            i.Risk_id1 == riskChoise.Risk_id1 &&
            i.Risk_id2 == riskChoise.Risk_id2 &&
            i.Risk_id3 == riskChoise.Risk_id3 &&
            i.Risk_id4 == riskChoise.Risk_id4 &&
            i.Risk_id5 == riskChoise.Risk_id5 &&
            i.Risk_id6 == riskChoise.Risk_id6 &&
            i.Risk_id7 == riskChoise.Risk_id7 &&
            i.Risk_id8 == riskChoise.Risk_id8).id;
            }
            else
            {
                dataBase.riskChoise.Add(riskChoise);
                dataBase.SaveChanges();
                return riskChoise.id;
            }
        }
        public int AddBusinessBlockChoice(BusinessBlockChoice BBChoice)
        {
            if (BBChoice == null)
            {
                BBChoice = new BusinessBlockChoice() { BusinessBlockid = 1 };
            }
            if (dataBase.BusinessBlockChoiceSet.Any(i =>
            i.BusinessBlockid == BBChoice.BusinessBlockid &&
            i.BusinessBlock_id1 == BBChoice.BusinessBlock_id1 &&
            i.BusinessBlock_id2 == BBChoice.BusinessBlock_id2 &&
            i.BusinessBlock_id3 == BBChoice.BusinessBlock_id3 &&
            i.BusinessBlock_id4 == BBChoice.BusinessBlock_id4 &&
            i.BusinessBlock_id5 == BBChoice.BusinessBlock_id5 &&
            i.BusinessBlock_id6 == BBChoice.BusinessBlock_id6 &&
            i.BusinessBlock_id7 == BBChoice.BusinessBlock_id7 &&
            i.BusinessBlock_id8 == BBChoice.BusinessBlock_id8
            ))
            {
                return dataBase.BusinessBlockChoiceSet.First(
            i => i.BusinessBlockid == BBChoice.BusinessBlockid &&
            i.BusinessBlock_id1 == BBChoice.BusinessBlock_id1 &&
            i.BusinessBlock_id2 == BBChoice.BusinessBlock_id2 &&
            i.BusinessBlock_id3 == BBChoice.BusinessBlock_id3 &&
            i.BusinessBlock_id4 == BBChoice.BusinessBlock_id4 &&
            i.BusinessBlock_id5 == BBChoice.BusinessBlock_id5 &&
            i.BusinessBlock_id6 == BBChoice.BusinessBlock_id6 &&
            i.BusinessBlock_id7 == BBChoice.BusinessBlock_id7 &&
            i.BusinessBlock_id8 == BBChoice.BusinessBlock_id8).id;
            }
            else
            {
                dataBase.BusinessBlockChoiceSet.Add(BBChoice);
                dataBase.SaveChanges();
                return BBChoice.id;
            }
        }
        public int AddEscalationChoice(EscalationChoice escalationChoice)
        {
            if (escalationChoice == null)
            {
                escalationChoice = new EscalationChoice() { Escalation_id = 1 };
            }
            if (dataBase.EscalationChoiceSet.Any(i =>
            i.Escalation_id == escalationChoice.Escalation_id &&
            i.Escalation_id1 == escalationChoice.Escalation_id1 &&
            i.Escalation_id2 == escalationChoice.Escalation_id2 &&
            i.Escalation_id3 == escalationChoice.Escalation_id3 &&
            i.Escalation_id4 == escalationChoice.Escalation_id4 &&
            i.Escalation_id5 == escalationChoice.Escalation_id5 &&
            i.Escalation_id6 == escalationChoice.Escalation_id6 &&
            i.Escalation_id7 == escalationChoice.Escalation_id7 &&
            i.Escalation_id8 == escalationChoice.Escalation_id8
            ))
            {
                return dataBase.EscalationChoiceSet.First(
            i => i.Escalation_id == escalationChoice.Escalation_id &&
            i.Escalation_id1 == escalationChoice.Escalation_id1 &&
            i.Escalation_id2 == escalationChoice.Escalation_id2 &&
            i.Escalation_id3 == escalationChoice.Escalation_id3 &&
            i.Escalation_id4 == escalationChoice.Escalation_id4 &&
            i.Escalation_id5 == escalationChoice.Escalation_id5 &&
            i.Escalation_id6 == escalationChoice.Escalation_id6 &&
            i.Escalation_id7 == escalationChoice.Escalation_id7 &&
            i.Escalation_id8 == escalationChoice.Escalation_id8).id;
            }
            else
            {
                dataBase.EscalationChoiceSet.Add(escalationChoice);
                dataBase.SaveChanges();
                return escalationChoice.id;
            }
        }
        public int AddSupportChoiceSet(supportChoice _suppChoice)
        {
            if (_suppChoice == null)
            {
                _suppChoice = new supportChoice() { Support_id = 1 };
            }
            if (dataBase.supportChoiceSet.Any(i =>
            i.Support_id == _suppChoice.Support_id &&
            i.Support_id1 == _suppChoice.Support_id1 &&
            i.Support_id2 == _suppChoice.Support_id2 &&
            i.Support_id3 == _suppChoice.Support_id3 &&
            i.Support_id4 == _suppChoice.Support_id4 &&
            i.Support_id5 == _suppChoice.Support_id5 &&
            i.Support_id6 == _suppChoice.Support_id6 &&
            i.Support_id7 == _suppChoice.Support_id7 &&
            i.Support_id8 == _suppChoice.Support_id8
            ))
            {
                return dataBase.supportChoiceSet.First(
            i => i.Support_id == _suppChoice.Support_id &&
            i.Support_id1 == _suppChoice.Support_id1 &&
            i.Support_id2 == _suppChoice.Support_id2 &&
            i.Support_id3 == _suppChoice.Support_id3 &&
            i.Support_id4 == _suppChoice.Support_id4 &&
            i.Support_id5 == _suppChoice.Support_id5 &&
            i.Support_id6 == _suppChoice.Support_id6 &&
            i.Support_id7 == _suppChoice.Support_id7 &&
            i.Support_id8 == _suppChoice.Support_id8).id;
            }
            else
            {
                dataBase.supportChoiceSet.Add(_suppChoice);
                dataBase.SaveChanges();
                return _suppChoice.id;
            }
        }
        public void AddActivity(TimeSheetTable activity)
        {
            TimeSpan span = activity.timeEnd - activity.timeStart;
            activity.TimeSpent = (int)span.TotalMinutes;
            activity.supportChoice = dataBase.supportChoiceSet.FirstOrDefault(i => i.id == activity.supportChoice_id);
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
            return new List<string>(dataBase.Block.Select(i => i.blockName).ToArray());
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
        public Visibility isAnalyticHasAccess(Analytic currentUser)
        {
            if (currentUser.Role.Id < 6)
                return Visibility.Visible;
            else return Visibility.Hidden;
        }

        public Analytic LoadAnalyticData()
        {
            string user = Environment.UserName;
            if (dataBase.Analytic.Any(i => i.userName == user))
            {
                return dataBase.Analytic.FirstOrDefault(i => i.userName.ToLower().Equals(Environment.UserName.ToLower()));
            }
            else
            {
                dataBase.Analytic.Add(new Analytic()
                {
                    userName = user,
                    DepartmentsId = 1,
                    DirectionsId = 1,
                    FirstName = "NotSet",
                    LastName = "NotSet",
                    FatherName = "NotSet",
                    OtdelTableId = 1,
                    PositionsId = 1,
                    RoleTableId = 1,
                    UpravlenieTableId = 1
                });
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
            oldProcess.BusinessBlockChoice = newProcess.BusinessBlockChoice;
            oldProcess.supportChoice = newProcess.supportChoice;
            oldProcess.Process = newProcess.Process;
            oldProcess.timeEnd = newProcess.timeEnd;
            oldProcess.TimeSpent = newProcess.TimeSpent;
            oldProcess.ClientWays = newProcess.ClientWays;
            oldProcess.EscalationChoice = newProcess.EscalationChoice;
            oldProcess.Formats = newProcess.Formats;
            oldProcess.riskChoise = newProcess.riskChoise;
            oldProcess.timeStart = newProcess.timeStart;

            dataBase.SaveChanges();
        }
        public bool IsCollisionedWithOtherRecords(TimeSheetTable record)
        {
            foreach (TimeSheetTable historyRecord in dataBase.TimeSheetTable.Where(i => i.AnalyticId == record.AnalyticId))
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

            foreach (Analytic analytic in analytics)
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
                    row["BusinessBlockName"] = ReportEntity[i].BusinessBlockChoice_id;
                    row["SupportsName"] = ReportEntity[i].supportChoice_id;
                    row["ClientWaysName"] = ReportEntity[i].ClientWays.Name;
                    row["EscalationsName"] = ReportEntity[i].EscalationChoice_id;
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
