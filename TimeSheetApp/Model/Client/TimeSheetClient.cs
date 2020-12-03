
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.Client.Base;
using TimeSheetApp.Model.EntitiesBase;
using TimeSheetApp.Model.Interfaces;
using Process = TimeSheetApp.Model.EntitiesBase.Process;

namespace TimeSheetApp.Model.Client
{
    public class TimeSheetClient : BaseClient, IDataProvider
    {
        protected override string ServiceAddress { get; set; }
        private string CurrentUserName { get; set; }
        IIdentityProvider IdentityClient { get; set; }
        public TimeSheetClient(IIdentityProvider identityClient) :base(identityClient)
        {

#if DevAtHome
            ServiceAddress = @"https://localhost:44341/timesheet";
            CurrentUserName = "u_m0x0c";
#else
            ServiceAddress = @"http://172.25.100.210:81/timesheet";
            CurrentUserName = Environment.UserName;
#endif
            IdentityClient = identityClient;
        }

        public IEnumerable<string> GetSubjectHints(Process process)
        {
            if (process == null)
                return new List<string>();

            string url = GenerateUrl(nameof(GetSubjectHints), $"process_id={process.Id}&username={CurrentUserName}");
            var result = Get<List<string>>(url);
            return result;
        }

        public async Task<IEnumerable<string>> GetSubjectHintsAsync(Process process)
        {
            if (process == null)
                return new List<string>();

            string url = GenerateUrl(nameof(GetSubjectHints), $"process_id={process.Id}&username={CurrentUserName}");
            var result = await GetAsync<List<string>>(url);
            return result;
        }

        public bool ForcedToQuit()
        {
            return false;
        }

        public IEnumerable<Process> GetProcesses()
        {
            
            string url = $"{ServiceAddress}/{nameof(GetProcesses)}";
            
            List<Process> processes = Get<List<Process>>(url);

            return processes;
        }

        public async Task<IEnumerable<Process>> GetProcessesAsync()
        {

            string url = $"{ServiceAddress}/{nameof(GetProcesses)}";

            List<Process> processes = await GetAsync<List<Process>>(url);
            return processes;
        }

        public IEnumerable<BusinessBlock> GetBusinessBlocks()
        {
            string url = $"{ServiceAddress}/{nameof(GetBusinessBlocks)}";
            return Get<List<BusinessBlock>>(url);
        }

        public async Task<IEnumerable<BusinessBlock>> GetBusinessBlocksAsync()
        {
            string url = $"{ServiceAddress}/{nameof(GetBusinessBlocks)}";
            return await GetAsync<List<BusinessBlock>>(url);
        }

        public IEnumerable<Supports> GetSupports()
        {
            string url = $"{ServiceAddress}/{nameof(GetSupports)}";
            return Get<List<Supports>>(url);
        }

        public async Task<IEnumerable<Supports>> GetSupportsAsync()
        {
            string url = $"{ServiceAddress}/{nameof(GetSupports)}";
            return await GetAsync<List<Supports>>(url);
        }

        public IEnumerable<ClientWays> GetClientWays()
        {
            string url = $"{ServiceAddress}/{nameof(GetClientWays)}";
            return Get<List<ClientWays>>(url);
        }

        public async Task<IEnumerable<ClientWays>> GetClientWaysAsync()
        {
            string url = $"{ServiceAddress}/{nameof(GetClientWays)}";
            return await GetAsync<List<ClientWays>>(url);
        }

        public IEnumerable<Escalation> GetEscalation()
        {
            string url = $"{ServiceAddress}/{nameof(GetEscalation)}";
            return Get<List<Escalation>>(url);
        }

        public async Task<IEnumerable<Escalation>> GetEscalationAsync()
        {
            string url = $"{ServiceAddress}/{nameof(GetEscalation)}";
            return await GetAsync<List<Escalation>>(url);
        }

        public IEnumerable<Formats> GetFormat()
        {
            string url = $"{ServiceAddress}/{nameof(GetFormat)}";
            return Get<List<Formats>>(url);
        }

        public async Task<IEnumerable<Formats>> GetFormatAsync()
        {
            string url = $"{ServiceAddress}/{nameof(GetFormat)}";
            return await GetAsync<List<Formats>>(url);
        }

        public IEnumerable<Risk> GetRisks()
        {
            string url = $"{ServiceAddress}/{nameof(GetRisks)}";
            return Get<List<Risk>>(url);
        }

        public async Task<IEnumerable<Risk>> GetRisksAsync()
        {
            string url = $"{ServiceAddress}/{nameof(GetRisks)}";
            return await GetAsync<List<Risk>>(url);
        }

        public IEnumerable<Analytic> GetMyAnalyticsData(Analytic currentUser)
        {
            string url = GenerateUrl(nameof(GetMyAnalyticsData), $"username={currentUser.UserName}");
            return Get<List<Analytic>>(url);
        }

        public async Task<IEnumerable<Analytic>> GetMyAnalyticsDataAsync(Analytic currentUser)
        {
            string url = GenerateUrl(nameof(GetMyAnalyticsData), $"username={currentUser.UserName}");
            return await GetAsync<List<Analytic>>(url);
        }

        public IEnumerable<string> GetProcessBlocks()
        {
            string url = GenerateUrl(nameof(GetProcessBlocks));
            return Get<List<string>>(url);
        }

        public async Task<IEnumerable<string>> GetProcessBlocksAsync()
        {
            string url = GenerateUrl(nameof(GetProcessBlocks));
            return await GetAsync<List<string>>(url);
        }

        public IEnumerable<string> GetSubBlocksNames()
        {
            string url = GenerateUrl(nameof(GetSubBlocksNames));
            return Get<List<string>>(url);
        }

        public async Task<IEnumerable<string>> GetSubBlocksNamesAsync()
        {
            string url = GenerateUrl(nameof(GetSubBlocksNames));
            return await GetAsync<List<string>>(url);
        }

        public TimeSheetTable AddActivity(TimeSheetTable activity)
        {
            string url = GenerateUrl(nameof(AddActivity));
            return Post<TimeSheetTable>(url, activity).Content.ReadAsAsync<TimeSheetTable>().Result;
        }
        public async Task<TimeSheetTable> AddActivityAsync(TimeSheetTable activity)
        {
            string url = GenerateUrl(nameof(AddActivity));
            HttpResponseMessage msg = await PostAsync<TimeSheetTable>(url, activity);
            return await msg.Content.ReadAsAsync<TimeSheetTable>();
        }

        public Analytic LoadAnalyticData(string userName)
        {
            string url = GenerateUrl(nameof(LoadAnalyticData), $"UserName={userName}");
            return Get<Analytic>(url);
        }

        public async Task<Analytic> LoadAnalyticDataAsync(string userName)
        {
            string url = GenerateUrl(nameof(LoadAnalyticData), $"UserName={userName}");
            return await GetAsync<Analytic>(url);
        }

        public void UpdateProcess(TimeSheetTable oldProcess, TimeSheetTable newProcess)
        {
            string url = GenerateUrl(nameof(UpdateProcess), $"oldProcessId={oldProcess.Id}");
            Post(url, newProcess);
        }

        public async Task UpdateProcessAsync(TimeSheetTable oldProcess, TimeSheetTable newProcess)
        {
            string url = GenerateUrl(nameof(UpdateProcess), $"oldProcessId={oldProcess.Id}");
            await PostAsync(url, newProcess);
        }

        public void DeleteRecord(int record_id)
        {
            string url = GenerateUrl(nameof(DeleteRecord), $"record_id={record_id}");
            Delete(url);
        }

        public async Task DeleteRecordAsync(int record_id)
        {
            string url = GenerateUrl(nameof(DeleteRecord), $"record_id={record_id}");
            await DeleteAsync(url);
        }

        public IEnumerable<TimeSheetTable> LoadTimeSheetRecords(DateTime date, string userName)
        {
            string url = GenerateUrl(nameof(LoadTimeSheetRecords), $"date={date:yyyy-MM-dd}&userName={userName}");
            List<TimeSheetTable> records = Get<List<TimeSheetTable>>(url);
            return records;
        }

        public async Task<IEnumerable<TimeSheetTable>> LoadTimeSheetRecordsAsync(DateTime date, string userName)
        {
            string url = GenerateUrl(nameof(LoadTimeSheetRecords), $"date={date:yyyy-MM-dd}&userName={userName}");
            List<TimeSheetTable> records = await GetAsync<List<TimeSheetTable>>(url);
            return records;
        }

        public void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end)
        {
            string newfileName = $"Report{DateTime.Now:ddMMyyyymmss}.xlsx";
            string analyticsId = string.Join("*",analytics.Select(i => i.Id.ToString()));
            string url = GenerateUrl(nameof(GetReport), $"ReportType={ReportType}&analytics={analyticsId}&start={start:yyyy-MM-dd}&end={end:yyyy-MM-dd}");
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {IdentityClient.GetToken()}");
            client.DownloadFile(url, newfileName);
            System.Diagnostics.Process.Start(newfileName);
        }

        public async Task GetReportAsync(int ReportType, Analytic[] analytics, DateTime start, DateTime end)
        {
            string newfileName = $"Report{DateTime.Now:ddMMyyyymmss}.xlsx";
            string analyticsId = string.Join("*", analytics.Select(i => i.Id.ToString()));
            string url = GenerateUrl(nameof(GetReport), $"ReportType={ReportType}&analytics={analyticsId}&start={start:yyyy-MM-dd}&end={end:yyyy-MM-dd}");
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {IdentityClient.GetToken()}");
            client.DownloadFile(url, newfileName);
            System.Diagnostics.Process.Start(newfileName);
        }

        public bool IsCollisionedWithOtherRecords(TimeSheetTable record)
        {
            string url = GenerateUrl(nameof(IsCollisionedWithOtherRecords), $"start={record.TimeStart:yyyy-MM-ddTHH*mm*ss}&end={record.TimeEnd:yyyy-MM-ddTHH*mm*ss}&analyticId={record.AnalyticId}&recId={record.Id}");
            return Get<bool>(url);
        }

        public async Task<bool> IsCollisionedWithOtherRecordsAsync(TimeSheetTable record)
        {
            string recordJson = JsonConvert.SerializeObject(record);
            string url = GenerateUrl(nameof(IsCollisionedWithOtherRecords), $"record={recordJson}");
            return await GetAsync<bool>(url);
        }

        public TimeSheetTable GetLastRecordWithSameProcess(int process_id, string userName)
        {
            string url = GenerateUrl(nameof(GetLastRecordWithSameProcess), $"process_id={process_id}&userName={userName}");
            return Get<TimeSheetTable>(url);
        }

        public async Task<TimeSheetTable> GetLastRecordWithSameProcessAsync(int process_id, string userName)
        {
            string url = GenerateUrl(nameof(GetLastRecordWithSameProcess), $"process_id={process_id}&userName={userName}");
            return await GetAsync<TimeSheetTable>(url);
        }

        public void RemoveSelection(int record_id)
        {
            string url = GenerateUrl(nameof(RemoveSelection), $"record_id={record_id}");
            Delete(url);
        }

        public async Task RemoveSelectionAsync(int record_id)
        {
            string url = GenerateUrl(nameof(RemoveSelection), $"record_id={record_id}");
            await DeleteAsync(url);
        }

        public IEnumerable<TimeSheetTable> GetTimeSheetRecordsForAnalytic(string userName)
        {
            string url = GenerateUrl(nameof(GetTimeSheetRecordsForAnalytic), $"userName={userName}");
            return Get<List<TimeSheetTable>>(url);
        }

        public async Task<IEnumerable<TimeSheetTable>> GetTimeSheetRecordsForAnalyticAsync(string userName)
        {
            string url = GenerateUrl(nameof(GetTimeSheetRecordsForAnalytic), $"userName={userName}");
            return await GetAsync<List<TimeSheetTable>>(url);
        }

        public void Commit()
        {
            string url = GenerateUrl(nameof(Commit));
            Get(url);
        }

        public async Task CommitAsync()
        {
            string url = GenerateUrl(nameof(Commit));
            await GetAsync<int>(url);
        }

        public double GetTimeSpent(string userName, DateTime start, DateTime end)
        {
            string url = GenerateUrl(nameof(GetTimeSpent), $"userName={userName}&start={start:yyyy-MM-dd}&end={end:yyyy-MM-dd}");

            return Get<double>(url);
        }

        public async Task<double> GetTimeSpentAsync(string userName, DateTime start, DateTime end)
        {
            string url = GenerateUrl(nameof(GetTimeSpent), $"userName={userName}&start={start:yyyy-MM-dd}&end={end:yyyy-MM-dd}");
            return await GetAsync<double>(url);
        }

        public int GetDaysWorkedCount(Analytic currentUser, DateTime lastMonthFirstDay, DateTime lastMonthLastDay)
        {
            string url = GenerateUrl(nameof(GetDaysWorkedCount), $"userName={currentUser.UserName}&lastMonthFirstDay={lastMonthFirstDay:yyyy-MM-dd}&lastMonthLastDay={lastMonthLastDay:yyyy-MM-dd}");
            return Get<int>(url);
        }

        public async Task<int> GetDaysWorkedCountAsync(Analytic currentUser, DateTime lastMonthFirstDay, DateTime lastMonthLastDay)
        {
            string url = GenerateUrl(nameof(GetDaysWorkedCount), $"userName={currentUser.UserName}&lastMonthFirstDay={lastMonthFirstDay:yyyy-MM-dd}&lastMonthLastDay={lastMonthLastDay:yyyy-MM-dd}");
            return await GetAsync<int>(url);
        }

        public IEnumerable<Analytic> GetTeam(Analytic analytic)
        {
            string url = GenerateUrl(nameof(GetTeam), $"userName={analytic.UserName}");
            return Get<List<Analytic>>(url);
        }

        public async Task<IEnumerable<Analytic>> GetTeamAsync(Analytic analytic)
        {
            string url = GenerateUrl(nameof(GetTeam), $"userName={analytic.UserName}");
            return await GetAsync<List<Analytic>>(url);
        }

        public Task<bool> ForcedToQuitAsync()
        {
            return Task.FromResult(false);
        }

        public IEnumerable<string> GetReportsAvailable()
        {
            string url = GenerateUrl(nameof(GetReportsAvailable));
            return Get<List<string>>(url);
        }

        public async Task<IEnumerable<string>> GetReportsAvailableAsync()
        {
            string url = GenerateUrl(nameof(GetReportsAvailable));
            return await GetAsync<List<string>>(url);
        }

        public IEnumerable<Process> GetProcessesSortedByRelevance(string userName, string filter)
        {
            if (string.IsNullOrWhiteSpace(userName)) return null;
            string url = GenerateUrl(nameof(GetProcessesSortedByRelevance), $"userName={userName}&filter={filter}");
            return Get<List<Process>>(url);
        }

        public async Task<IEnumerable<Process>> GetProcessesSortedByRelevanceAsync(string userName, string filter)
        {
            if (string.IsNullOrWhiteSpace(userName)) return null;
            string url = GenerateUrl(nameof(GetProcessesSortedByRelevance), $"userName={userName}&filter={filter}");
            return await GetAsync<List<Process>>(url);
        }

        public bool CanConnect()
        {
            return !string.IsNullOrEmpty(IdentityClient.GetToken());
        }
    }
}
