﻿using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Windows;

using TimeSheetApp.Model.Client.Base;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model.Client
{
    public class TimeSheetClient : BaseClient, IDataProvider
    {
        protected override string ServiceAddress { get; set; }
        private string currentUserName { get; set; }

        public TimeSheetClient(IConfiguration configuration) :base(configuration)
        {
#if DevAtHome
            ServiceAddress = @"http://localhost:81/timesheet";
            currentUserName = "u_m0x0c";
#else
            ServiceAddress = @"http://172.25.100.210:81/timesheet";
            currentUserName = Environment.UserName;
#endif
        }

        //http://localhost:81/timesheet/GetSubjectHints?process_id=26&username=u_m0x0c
        public IEnumerable<string> GetSubjectHints(Process process)
        {
            string url = $"{ServiceAddress}/{nameof(GetSubjectHints)}?process_id={process.Id}&username={currentUserName}";
            var result = Get<List<string>>(url);
            return result;
        }

        public bool ForcedToQuit()
        {
            return false;
        }

        //http://localhost:81/timesheet/GetProcesses
        public IEnumerable<Process> GetProcesses()
        {
            string url = $"{ServiceAddress}/{nameof(GetProcesses)}";
            return Get<List<Process>>(url);
        }

        //http://localhost:81/timesheet/GetBusinessBlocks
        public IEnumerable<BusinessBlock> GetBusinessBlocks()
        {
            string url = $"{ServiceAddress}/{nameof(GetBusinessBlocks)}";
            return Get<List<BusinessBlock>>(url);
        }

        //http://localhost:81/timesheet/GetSupports
        public IEnumerable<Supports> GetSupports()
        {
            string url = $"{ServiceAddress}/{nameof(GetSupports)}";
            return Get<List<Supports>>(url);
        }

        //http://localhost:81/timesheet/GetClientWays
        public IEnumerable<ClientWays> GetClientWays()
        {
            string url = $"{ServiceAddress}/{nameof(GetClientWays)}";
            return Get<List<ClientWays>>(url);
        }

        //http://localhost:81/timesheet/GetEscalation
        public IEnumerable<Escalation> GetEscalation()
        {
            string url = $"{ServiceAddress}/{nameof(GetEscalation)}";
            return Get<List<Escalation>>(url);
        }

        //http://localhost:81/timesheet/GetFormat
        public IEnumerable<Formats> GetFormat()
        {
            string url = $"{ServiceAddress}/{nameof(GetFormat)}";
            return Get<List<Formats>>(url);
        }

        //http://localhost:81/timesheet/GetRisks
        public IEnumerable<Risk> GetRisks()
        {
            string url = $"{ServiceAddress}/{nameof(GetRisks)}";
            return Get<List<Risk>>(url);
        }

        //http://localhost:81/timesheet/GetMyAnalyticsData?username=u_m0x0c
        public IEnumerable<Analytic> GetMyAnalyticsData(Analytic currentUser)
        {
            string url = $"{ServiceAddress}/{nameof(GetMyAnalyticsData)}?username={currentUser.UserName}";
            return Get<List<Analytic>>(url);
        }
        //http://localhost:81/timesheet/GetProcessBlocks
        public IEnumerable<string> GetProcessBlocks()
        {
            string url = GenerateUrl(nameof(GetProcessBlocks));
            return Get<List<string>>(url);
        }

        //http://localhost:81/timesheet/GetSubBlocksNames
        public IEnumerable<string> GetSubBlocksNames()
        {
            string url = GenerateUrl(nameof(GetSubBlocksNames));
            return Get<List<string>>(url);
        }

        //http://localhost:81/timesheet/AddActivity?activity={}
        public void AddActivity(TimeSheetTable activity)
        {
            string url = GenerateUrl(nameof(AddActivity));
            Post(url, activity);
        }

        public Analytic LoadAnalyticData(string userName)
        {
            string url = GenerateUrl(nameof(LoadAnalyticData), userName);
            return Get<Analytic>(url);
        }

        public void UpdateProcess(TimeSheetTable oldProcess, TimeSheetTable newProcess)
        {
            string url = GenerateUrl(nameof(UpdateProcess));
            Post(url, (oldProcess, newProcess));
        }

        public void DeleteRecord(int record_id)
        {
            string url = GenerateUrl(nameof(DeleteRecord), $"?record_id={record_id}");
            Delete(url);
        }

        public IEnumerable<TimeSheetTable> LoadTimeSheetRecords(DateTime date, string userName)
        {
            string url = GenerateUrl(nameof(LoadTimeSheetRecords), $"?date={date:dd.MM.yyyy};", $"userName={userName}");
            return Get<List<TimeSheetTable>>(url);
        }

        public void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public bool IsCollisionedWithOtherRecords(TimeSheetTable record)
        {
            string recordJson = JsonConvert.SerializeObject(record);
            string url = GenerateUrl(nameof(IsCollisionedWithOtherRecords), $"?record={recordJson}");
            return Get<bool>(url);
        }

        public bool IsAnalyticHasAccess(string userName)
        {
            string url = GenerateUrl(nameof(IsAnalyticHasAccess), $"?userName={userName}");
            return Get<bool>(url);
        }

        public TimeSheetTable GetLastRecordWithSameProcess(int process_id, string userName)
        {
            string url = GenerateUrl(nameof(GetLastRecordWithSameProcess), $"?process_id={process_id};userName={userName}");
            return Get<TimeSheetTable>(url);
        }

        public void RemoveSelection(int record_id)
        {
            string url = GenerateUrl(nameof(RemoveSelection), $"?record_id={record_id}");
            Delete(url);
        }

        public IEnumerable<TimeSheetTable> GetTimeSheetRecordsForAnalytic(string userName)
        {
            string url = GenerateUrl(nameof(GetTimeSheetRecordsForAnalytic), $"?userName={userName}");
            return Get<List<TimeSheetTable>>(url);
        }

        public void Commit()
        {
            string url = GenerateUrl(nameof(Commit));
            Get(url);
        }

        public double GetTimeSpent(string userName, DateTime start, DateTime end)
        {
            string url = GenerateUrl(nameof(GetTimeSpent), $"?userName={userName};start={start:dd.MM.yyyy};end={end:dd.MM.yyyy}");
            return Get<double>(url);
        }

        public int GetDaysWorkedCount(Analytic currentUser, DateTime lastMonthFirstDay, DateTime lastMonthLastDay)
        {
            string url = GenerateUrl(nameof(GetDaysWorkedCount), $"?userName={currentUser.UserName};lastMonthFirstDay={lastMonthFirstDay:dd.MM.yyyy};lastMonthLastDay={lastMonthLastDay:dd.MM.yyyy}");
            return Get<int>(url);
        }

        public IEnumerable<Analytic> GetTeam(Analytic analytic)
        {
            string url = GenerateUrl(nameof(GetTeam), $"?userName={analytic.UserName}");
            return Get<List<Analytic>>(url);
        }

        private string GenerateUrl(string senderName, params string[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            if (parameters == null)
            {
                return $"{ServiceAddress}/{senderName}";
            }
            else
            {

                foreach(string param in parameters)
                {
                    sb.Append($"{param}&");
                }
                return $"{ServiceAddress}/{senderName}?{sb.Remove(sb.Length-1, 1)}";
            }
        }
    }
}
