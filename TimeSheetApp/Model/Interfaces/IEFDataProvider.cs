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
    public interface IEFDataProvider
    {
        List<string> GetSubjectHints(Process process);
        bool ForcedToQuit();
        ObservableCollection<Process> GetProcesses();
        List<BusinessBlock> GetBusinessBlocks();
        List<Supports> GetSupports();
        List<ClientWays> GetClientWays();
        List<Escalation> GetEscalation();
        List<Formats> GetFormat();
        List<Risk> GetRisks();
        ObservableCollection<Analytic> GetMyAnalyticsData(Analytic currentUser);
        List<string> GetProcessBlocks();
        List<string> GetSubBlocksNames();
        void AddActivity(TimeSheetTable activity);
        Analytic LoadAnalyticData();
        void UpdateProcess(TimeSheetTable oldProcess, TimeSheetTable newProcess);
        void DeleteRecord(TimeSheetTable record);
        List<TimeSheetTable> LoadTimeSheetRecords(DateTime date, Analytic user);
        void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end);
        bool IsCollisionedWithOtherRecords(TimeSheetTable record);
        Visibility IsAnalyticHasAccess(Analytic currentUser);
        TimeSheetTable GetLastRecordWithSameProcess(Process process, Analytic user);
        void RemoveSelection(TimeSheetTable record);
        List<TimeSheetTable> GetTimeSheetRecordsForAnalytic(Analytic currentUser);
        void Commit();
    }
}
