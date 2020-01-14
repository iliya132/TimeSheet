using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
namespace TimeSheetApp.Model
{
    public interface IDataProvider
    {
        bool ForcedToQuit();
        SqlConnection Connection { get; set; }
        ObservableCollection<Process> GetProcesses();
        ObservableCollection<string> GetBusinessBlocks();
        ObservableCollection<string> GetSupports();
        ObservableCollection<string> GetClientWays();
        ObservableCollection<string> GetEscalation();
        ObservableCollection<string> GetFormat();
        ObservableCollection<string> GetRisks();
        ObservableCollection<Analytic> GetMyAnalyticsData(Analytic currentUser);
        
        ObservableCollection<TimeSheetHistoryItem> GetTimeSheetItem();
        List<string> GetBlocksList();
        List<string> GetSubBlocksList();
        void AddActivity(Process activity);
        Analytic LoadAnalyticData();
        Process LoadHistoryProcess(DateTime timeStart, Analytic user);
        int UpdateProcess(Process oldProcess, Process newProcess);
        int DeleteProcess(DateTime timeStast, Analytic analytic);
        void LoadTimeSpan(DateTime date, Analytic user, ObservableCollection<TimeSpanClass> timeSpans);
        void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end);
    }
}
