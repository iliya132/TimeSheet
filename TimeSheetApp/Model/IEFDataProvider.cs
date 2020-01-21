using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TimeSheetApp.Model
{
    public interface IEFDataProvider
    {
        bool ForcedToQuit();
        object GetChoice(int choice, int set);
        ObservableCollection<Process> GetProcesses();
        BusinessBlock[] GetBusinessBlocks();
        Supports[] GetSupports();
        ClientWays[] GetClientWays();
        Escalations[] GetEscalation();
        Formats[] GetFormat();
        Risk[] GetRisks();
        ObservableCollection<Analytic> GetMyAnalyticsData(Analytic currentUser);
        List<string> GetBlocksList();
        List<string> GetSubBlocksList();
        void AddActivity(TimeSheetTable activity);
        Analytic LoadAnalyticData();
        Process LoadHistoryProcess(DateTime timeStart, Analytic user);
        void UpdateProcess(TimeSheetTable oldProcess, TimeSheetTable newProcess);
        void DeleteRecord(TimeSheetTable record);
        List<TimeSheetTable> LoadTimeSheetRecords(DateTime date, Analytic user);
        void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end);
        bool IsCollisionedWithOtherRecords(TimeSheetTable record);
        int AddRiskChoice(riskChoise riskChoise);
        int AddSupportChoiceSet(supportChoice _suppChoice);
        int AddEscalationChoice(EscalationChoice escalationChoice);
        int AddBusinessBlockChoice(BusinessBlockChoice BBChoice);
        Visibility isAnalyticHasAccess(Analytic currentUser);
    }
}
