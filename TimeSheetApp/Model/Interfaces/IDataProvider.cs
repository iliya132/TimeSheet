using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    public interface IDataProvider
    {
        IEnumerable<string> GetSubjectHints(Process process);
        bool ForcedToQuit();
        IEnumerable<Process> GetProcesses();
        IEnumerable<BusinessBlock> GetBusinessBlocks();
        IEnumerable<Supports> GetSupports();
        IEnumerable<ClientWays> GetClientWays();
        IEnumerable<Escalation> GetEscalation();
        IEnumerable<Formats> GetFormat();
        IEnumerable<Risk> GetRisks();
        IEnumerable<Analytic> GetMyAnalyticsData(Analytic currentUser);
        IEnumerable<string> GetProcessBlocks();
        IEnumerable<string> GetSubBlocksNames();
        void AddActivity(TimeSheetTable activity);
        Analytic LoadAnalyticData(string userName);
        void UpdateProcess(TimeSheetTable oldProcess, TimeSheetTable newProcess);
        void DeleteRecord(int record_id);
        IEnumerable<TimeSheetTable> LoadTimeSheetRecords(DateTime date, string userName);
        void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end);
        bool IsCollisionedWithOtherRecords(TimeSheetTable record);
        TimeSheetTable GetLastRecordWithSameProcess(int process_id, string userName);
        void RemoveSelection(int record_id);
        IEnumerable<TimeSheetTable> GetTimeSheetRecordsForAnalytic(string userName);
        void Commit();
        double GetTimeSpent(string userName, DateTime start, DateTime end);
        int GetDaysWorkedCount(Analytic currentUser, DateTime lastMonthFirstDay, DateTime lastMonthLastDay);
        IEnumerable<Analytic> GetTeam(Analytic analytic);
    }
}
