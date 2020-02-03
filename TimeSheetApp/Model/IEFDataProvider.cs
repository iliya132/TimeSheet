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
        Stack<string> GetSubjectHints(Process process);
        bool ForcedToQuit();
        List<object> GetChoice(int ObjectId, int Type);
        ObservableCollection<Process> GetProcesses();
        BusinessBlock[] GetBusinessBlocks();
        Supports[] GetSupports();
        ClientWays[] GetClientWays();
        Escalation[] GetEscalation();
        Formats[] GetFormat();
        Risk[] GetRisks();
        ObservableCollection<Analytic> GetMyAnalyticsData(Analytic currentUser);
        List<string> GetBlocksList();
        List<string> GetSubBlocksList();
        void AddActivity(TimeSheetTable activity);
        Analytic LoadAnalyticData();
        void UpdateProcess(TimeSheetTable oldProcess, TimeSheetTable newProcess);
        void DeleteRecord(TimeSheetTable record);
        List<TimeSheetTable> LoadTimeSheetRecords(DateTime date, Analytic user);
        void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end);
        bool IsCollisionedWithOtherRecords(TimeSheetTable record);
        int AddRiskChoice(RiskChoice RiskChoice);
        int AddSupportChoiceSet(SupportChoice _suppChoice);
        int AddEscalationChoice(EscalationChoice escalationChoice);
        int AddBusinessBlockChoice(BusinessBlockChoice BBChoice);
        Visibility IsAnalyticHasAccess(Analytic currentUser);
        Risk[] LoadRiskChoice(RiskChoice choice);
        Escalation[] LoadEscalationChoice(EscalationChoice escalationChoice);
        Supports[] LoadSupportsChoice(SupportChoice SupportChoice);
        BusinessBlock[] LoadBusinessBlockChoice(BusinessBlockChoice businessBlockChoice);
    }
}
