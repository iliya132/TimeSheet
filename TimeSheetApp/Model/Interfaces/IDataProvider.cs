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
        bool CanConnect();
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
        TimeSheetTable AddActivity(TimeSheetTable activity);
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
        IEnumerable<Process> GetProcessesSortedByRelevance(string userName, string filter);
        int GetDaysWorkedCount(Analytic currentUser, DateTime lastMonthFirstDay, DateTime lastMonthLastDay);
        IEnumerable<string> GetReportsAvailable();
        IEnumerable<Analytic> GetTeam(Analytic analytic);
        Task<IEnumerable<Process>> GetProcessesSortedByRelevanceAsync(string userName, string filter);
        Task<IEnumerable<string>> GetSubjectHintsAsync(Process process);
        Task<bool> ForcedToQuitAsync();
        Task<IEnumerable<Process>> GetProcessesAsync();
        Task<IEnumerable<BusinessBlock>> GetBusinessBlocksAsync();
        Task<IEnumerable<Supports>> GetSupportsAsync();
        Task<IEnumerable<ClientWays>> GetClientWaysAsync();
        Task<IEnumerable<Escalation>> GetEscalationAsync();
        Task<IEnumerable<Formats>> GetFormatAsync();
        Task<IEnumerable<Risk>> GetRisksAsync();
        Task<IEnumerable<Analytic>> GetMyAnalyticsDataAsync(Analytic currentUser);
        Task<IEnumerable<string>> GetProcessBlocksAsync();
        Task<IEnumerable<string>> GetSubBlocksNamesAsync();
        Task<TimeSheetTable> AddActivityAsync(TimeSheetTable activity);
        Task<Analytic> LoadAnalyticDataAsync(string userName);
        Task UpdateProcessAsync(TimeSheetTable oldProcess, TimeSheetTable newProcess);
        Task DeleteRecordAsync(int record_id);
        Task<IEnumerable<TimeSheetTable>> LoadTimeSheetRecordsAsync(DateTime date, string userName);
        Task GetReportAsync(int ReportType, Analytic[] analytics, DateTime start, DateTime end);
        Task<bool> IsCollisionedWithOtherRecordsAsync(TimeSheetTable record);
        Task<TimeSheetTable> GetLastRecordWithSameProcessAsync(int process_id, string userName);
        Task RemoveSelectionAsync(int record_id);
        Task<IEnumerable<TimeSheetTable>> GetTimeSheetRecordsForAnalyticAsync(string userName);
        Task CommitAsync();
        Task<double> GetTimeSpentAsync(string userName, DateTime start, DateTime end);
        Task<int> GetDaysWorkedCountAsync(Analytic currentUser, DateTime lastMonthFirstDay, DateTime lastMonthLastDay);
        Task<IEnumerable<Analytic>> GetTeamAsync(Analytic analytic);
        Task<IEnumerable<string>> GetReportsAvailableAsync();
    }
}
