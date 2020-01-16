using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Security;

namespace TimeSheetApp.Model
{
    class DataProvider : IDataProvider
    {

        SqlConnectionStringBuilder sqlConnectionString = new SqlConnectionStringBuilder();  
        private SqlConnection _connection;
        public SqlConnection Connection { get => _connection; set => _connection = value; }
        SqlCredential credential;
        SecureString secureString = new SecureString();
        SqlDataAdapter adapter;
        SqlCommand command;
        public DataProvider()
        {
            sqlConnectionString.DataSource = "A105512";
            sqlConnectionString.InitialCatalog = "TimeSheetDB";
            sqlConnectionString.ConnectTimeout = 30;
            sqlConnectionString.Encrypt = true;
            sqlConnectionString.ApplicationIntent = ApplicationIntent.ReadWrite;
            sqlConnectionString.TrustServerCertificate = true;
            sqlConnectionString.MultipleActiveResultSets = true;
            foreach (char pch in "DK_user!")
                secureString.AppendChar(pch);
            secureString.MakeReadOnly();
            credential = new SqlCredential("TimeSheetUser", secureString);
            _connection = new SqlConnection(sqlConnectionString.ConnectionString);
            _connection.Credential = credential;
        }

        public ObservableCollection<string> GetBusinessBlocks()
        {
            ObservableCollection<string> exportValue = new ObservableCollection<string>();
            command = new SqlCommand("Select BusinessBlockName from BusinessBlock", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach(DataRow row in dataTable.Rows)
                exportValue.Add(row[0].ToString());
            return exportValue;
        }

        public ObservableCollection<string> GetClientWays()
        {

            ObservableCollection<string> exportValue = new ObservableCollection<string>();
            command = new SqlCommand("Select Name from ClientWays", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
                exportValue.Add(row[0].ToString());
            return exportValue;
        }

        public ObservableCollection<string> GetEscalation()
        {
            ObservableCollection<string> exportValue = new ObservableCollection<string>();
            command = new SqlCommand("Select Name from Escalations", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
                exportValue.Add(row[0].ToString());
            return exportValue;
        }

        public ObservableCollection<string> GetFormat()
        {
            ObservableCollection<string> exportValue = new ObservableCollection<string>();
            command = new SqlCommand("Select Name from Formats", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
                exportValue.Add(row[0].ToString());
            return exportValue;
        }

        public ObservableCollection<Process> GetProcesses()
        {
            ObservableCollection<Process> exportValue = new ObservableCollection<Process>();
            command = new SqlCommand(
@"  Select procName, SubBlock, Process.Block, Process.id, Comment, Result, CommentNeeded, [dbo].Block.blockName, [dbo].SubBlock.subblockName, Process.ProcessType, [dbo].ProcessType.ProcessTypeName
    from Process
    JOIN [dbo].Block
        on[dbo].Block.Id = Process.Block
    join[dbo].SubBlock
        on[dbo].SubBlock.Id = Process.SubBlock
    join [dbo].ProcessType
        on[dbo].ProcessType.id = Process.ProcessType; ", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
                exportValue.Add(new Process(
                    row["procName"].ToString(),
                    int.Parse(row["Block"].ToString()),
                    int.Parse(row["SubBlock"].ToString()),
                    int.Parse(row["Id"].ToString()),
                    row["Comment"].ToString(),
                    int.Parse(row["Result"].ToString()),
                    row["blockName"].ToString(),
                    row["subblockName"].ToString(),
                    bool.Parse(row["CommentNeeded"].ToString())
                    )
                { ProcType = int.Parse(row["ProcessType"].ToString()),
                    ProcTypeName = row["ProcessTypeName"].ToString()
                });
            return exportValue;
        }

        public ObservableCollection<string> GetRisks()
        {
            ObservableCollection<string> exportValue = new ObservableCollection<string>();
            command = new SqlCommand("Select riskName from Risk", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
                exportValue.Add(row[0].ToString());
            return exportValue;
        }

        public ObservableCollection<string> GetSupports()
        {
            ObservableCollection<string> exportValue = new ObservableCollection<string>();
            command = new SqlCommand("Select Name from Supports", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
                exportValue.Add(row[0].ToString());
            return exportValue;
        }

        public ObservableCollection<TimeSheetHistoryItem> GetTimeSheetItem()
        {
            ObservableCollection<TimeSheetHistoryItem> exportValue = new ObservableCollection<TimeSheetHistoryItem>();
            command = new SqlCommand("Select timeStart, timeEnd, Subject from TimeSheetTable", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
                exportValue.Add(new TimeSheetHistoryItem(
                    DateTime.Parse(row[0].ToString()),
                    DateTime.Parse(row[1].ToString()),
                    row[2].ToString()
                    ));
            return exportValue;
        }
        public List<string> GetBlocksList()
        {
            List<string> exportValue = new List<string>();
            command = new SqlCommand("Select blockName from Block", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
                exportValue.Add(row[0].ToString());
            
            return exportValue;
        }
        public List<string> GetSubBlocksList()
        {
            List<string> exportValue = new List<string>();
            command = new SqlCommand("Select subblockName from SubBlock", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
                exportValue.Add(row[0].ToString());

            return exportValue;
        }

        public void AddActivity(Process activity)
        {
            Connection.Open();
            command = new SqlCommand("INSERT INTO TimeSheetTable ([_Process], [_Analytic], " +
                "[timeStart], [timeEnd], [TimeSpent], [comment], [_BusinessBlock], [_Risk], [_ClientWay], [_Escalation], [_Format], [_Support], [Subject]) " +
                "values (@Process, @Analytic, @timeStart, @timeEnd, @TimeSpent, @Comment, @BusinessBlock, @Risk, @ClientWay, @Escalation, @Format, @Support, @Subject);",
                Connection);
            command.Parameters.AddWithValue("@Process", activity.Id);
            command.Parameters.AddWithValue("@Analytic", activity.Analytic);
            command.Parameters.AddWithValue("@timeStart", activity.DateTimeStart);
            command.Parameters.AddWithValue("@timeEnd", activity.DateTimeEnd);
            command.Parameters.AddWithValue("@TimeSpent", activity.TimeSpent);
            command.Parameters.AddWithValue("@Comment", activity.Body);
            command.Parameters.AddWithValue("@BusinessBlock", activity.BusinessBlock + 1);
            command.Parameters.AddWithValue("@Risk", activity.Risk + 1);
            command.Parameters.AddWithValue("@ClientWay", activity.ClientWay + 1);
            command.Parameters.AddWithValue("@Escalation", activity.Escalation + 1);
            command.Parameters.AddWithValue("@Format", activity.Format + 1);
            command.Parameters.AddWithValue("@Support", activity.Support + 1);
            command.Parameters.AddWithValue("@Subject", activity.Subject);
            command.ExecuteNonQuery();
            Connection.Close();
        }
        private void AddAnalytic(string userName, int department, int direction, int upravlenie, int otdel, int position, int role, string FirstName, string LastName, string FatherName)
        {
            Connection.Open();
            SqlCommand command = new SqlCommand("INSERT INTO Analytic ([userName], [_department], " +
                "[_direction], [_upravlenie], [_otdel], [_position], [_role], [FirstName], [LastName], [FatherName]) " +
                "values (@userName, @department, @direction, @upravlenie, @otdel, @position, @role, @firstName, @LastName, @FatherName);",
                Connection);
            command.Parameters.AddWithValue("@userName", userName);
            command.Parameters.AddWithValue("@department", department);
            command.Parameters.AddWithValue("@direction", direction);
            command.Parameters.AddWithValue("@upravlenie", upravlenie);
            command.Parameters.AddWithValue("@otdel", otdel);
            command.Parameters.AddWithValue("@position", position);
            command.Parameters.AddWithValue("@role", role);
            command.Parameters.AddWithValue("@firstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@FatherName", FatherName);
            command.ExecuteNonQuery();
            Connection.Close();
        }
        public Analytic LoadAnalyticData()
        {
            string currentUser = Environment.UserName;
            command = new SqlCommand("SELECT Id, _department, _direction, _upravlenie, _otdel, _position, _role, FirstName, LastName, FatherName from Analytic Where userName = @userName", Connection);
            command.Parameters.AddWithValue("@userName", currentUser);
            adapter = new SqlDataAdapter(command);
            DataTable data = new DataTable();
            adapter.Fill(data);
            if (data.Rows.Count==0)
            {
                AddAnalytic(currentUser, 1, 1, 6, 1, 1, 6, "NULL", "NULL", "NULL");
                adapter = new SqlDataAdapter(command);
                data = new DataTable();
                adapter.Fill(data);
            }
            return new Analytic()
            {
                Direction = int.Parse(data.Rows[0]["_direction"].ToString()),
                ID = int.Parse(data.Rows[0]["Id"].ToString()),
                Otdel = int.Parse(data.Rows[0]["_otdel"].ToString()),
                Position = int.Parse(data.Rows[0]["_position"].ToString()),
                Role = int.Parse(data.Rows[0]["_role"].ToString()),
                Upravlenie = int.Parse(data.Rows[0]["_upravlenie"].ToString()),
                UserName = currentUser
            };
        }
        public Process LoadHistoryProcess(DateTime timeStart, Analytic user)
        {
            DataTable data = new DataTable();
            command = new SqlCommand(
@"Select _Process, _Analytic, timeStart, timeEnd, [dbo].TimeSheetTable.comment, _BusinessBlock,
    _Risk, _ClientWay, _Escalation, _Format, _Support, Subject, [dbo].BusinessBlock.BusinessBlockName, 
    [dbo].Process.Block, [dbo].Process.SubBlock, [dbo].Process.procName, [dbo].Process.Comment, [dbo].Process.Result,
    [dbo].Block.blockName, [dbo].SubBlock.subblockName
from [dbo].TimeSheetTable
	JOIN [dbo].BusinessBlock
        on[dbo].BusinessBlock.Id = [dbo].TimeSheetTable._BusinessBlock
	join [dbo].Process
		on [dbo].Process.id=[dbo].TimeSheetTable._Process
	join [dbo].Block
    on [dbo].Block.id=Process.Block AND Process.id=[dbo].TimeSheetTable._Process
	join [dbo].subBlock
    on [dbo].Process.SubBlock = [dbo].subBlock.id AND Process.id=[dbo].TimeSheetTable._Process
    Where _Analytic=@Analytic AND timeStart=@timeStart;", Connection);
            command.Parameters.AddWithValue("@Analytic", user.ID);
            command.Parameters.AddWithValue("@timeStart", timeStart);
            adapter = new SqlDataAdapter(command);
            adapter.Fill(data);
            DateTime processDate = DateTime.Parse(data.Rows[0]["timeStart"].ToString());
            Process newProcess = new Process(
                data.Rows[0]["procName"].ToString(),
                _block: int.Parse(data.Rows[0]["Block"].ToString()),
                _subBlock: int.Parse(data.Rows[0]["SubBlock"].ToString()),
                Id: int.Parse(data.Rows[0]["_Process"].ToString()),
                _comment: data.Rows[0]["Comment"].ToString(),
                _resul: int.Parse(data.Rows[0]["Result"].ToString()),
                _blockName: data.Rows[0]["blockName"].ToString(),
                _subBlockName: data.Rows[0]["subblockName"].ToString())
            {
                BusinessBlock = int.Parse(data.Rows[0]["_BusinessBlock"].ToString()) - 1,
                Risk = int.Parse(data.Rows[0]["_Risk"].ToString()) - 1,
                ClientWay = int.Parse(data.Rows[0]["_ClientWay"].ToString()) - 1,
                Escalation = int.Parse(data.Rows[0]["_Escalation"].ToString()) - 1,
                Format = int.Parse(data.Rows[0]["_Format"].ToString()) - 1,
                Support = int.Parse(data.Rows[0]["_Support"].ToString()) - 1,
                Subject = data.Rows[0]["Subject"].ToString(),
                Body = data.Rows[0]["comment"].ToString(),
                DateTimeStart = DateTime.Parse(data.Rows[0]["timeStart"].ToString()),
                DateTimeEnd = DateTime.Parse(data.Rows[0]["timeEnd"].ToString()),
                ProcDate = new DateTime(processDate.Year, processDate.Month, processDate.Day)
                
            };
            return newProcess;
        }

        public int UpdateProcess(Process oldProcess, Process newProcess)
        {
            int errorState = 0;
            command = new SqlCommand(@"
    UPDATE TimeSheetTable
    SET _Process = @process, 
timeStart = @timeStart, 
timeEnd = @timeEnd, 
comment = @Body, 
_BusinessBlock = @businessBlock, 
_Risk = @Risk, _ClientWay = @clientWay, 
_Escalation = @escalation, 
_Format = @format, 
_support = @support, 
Subject = @subject
    WHERE timeStart=@oldTimeStart AND _Analytic = @user", Connection);
            command.Parameters.AddWithValue("@process", newProcess.Id);
            command.Parameters.AddWithValue("@timeStart", newProcess.DateTimeStart);
            command.Parameters.AddWithValue("@timeEnd", newProcess.DateTimeEnd);
            command.Parameters.AddWithValue("@Body", newProcess.Body);
            command.Parameters.AddWithValue("@businessBlock", newProcess.BusinessBlock + 1);
            command.Parameters.AddWithValue("@Risk", newProcess.Risk + 1);
            command.Parameters.AddWithValue("@clientWay", newProcess.ClientWay + 1);
            command.Parameters.AddWithValue("@escalation", newProcess.Escalation + 1);
            command.Parameters.AddWithValue("@format", newProcess.Format + 1);
            command.Parameters.AddWithValue("@support", newProcess.Support + 1);
            command.Parameters.AddWithValue("@subject", newProcess.Subject);
            command.Parameters.AddWithValue("@oldTimeStart", oldProcess.DateTimeStart);
            command.Parameters.AddWithValue("@user", newProcess.Analytic);
            try
            {
                Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errorState = -1;
            }
            finally
            {
                Connection.Close();
            }
            return errorState;
        }

        public int DeleteProcess(DateTime timeStart, Analytic analytic)
        {
            int errorState = 0;
            command = new SqlCommand(@"
DELETE FROM TimeSheetTable
WHERE timeStart=@timeStart AND _Analytic = @user;", Connection);
            command.Parameters.AddWithValue("@timeStart", timeStart);
            command.Parameters.AddWithValue("@user", analytic.ID);
            try
            {
                Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errorState = -1;
            }
            finally
            {
                Connection.Close();
            }
            return errorState;
        }

        public void LoadTimeSpan(DateTime date, Analytic user, ObservableCollection<TimeSpanClass> timeSpans)
        {
            command = new SqlCommand(
                @"
    SELECT TimeSheetTable.timeStart, TimeSheetTable.timeEnd, Process.procName, 
            TimeSheetTable.Subject, Process.Block, Process.SubBlock, 
            TimeSheetTable._Process, Block.blockName, 
            Subblock.subblockName, Process.procName 
    FROM TimeSheetTable 
    join [dbo].Process
        on [dbo].Process.id = TimeSheetTable._Process
    join [dbo].Block
        on [dbo].Block.id = [dbo].Process.Block
    join [dbo].SubBlock
        on [dbo].SubBlock.id = [dbo].Process.SubBlock
    WHERE timeStart > @dateStart AND timeStart < @dateEnd AND _Analytic=@analytic;", Connection);
            command.Parameters.AddWithValue("@dateStart", new DateTime(date.Year, date.Month, date.Day, 00,00,01));
            command.Parameters.AddWithValue("@dateEnd", new DateTime(date.Year, date.Month, date.Day, 23, 59, 59));
            command.Parameters.AddWithValue("@analytic", user.ID);
            Connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            timeSpans.Clear();
            while (dataReader.Read())
            {
                timeSpans.Add(new TimeSpanClass(
    DateTime.Parse(dataReader[0].ToString()),
    DateTime.Parse(dataReader[1].ToString()),
    dataReader[2].ToString(),
    dataReader[3].ToString(),
    $"{dataReader[4].ToString()}.{dataReader[5].ToString()}.{dataReader[6].ToString()}",
    $"Блок: {dataReader[7].ToString()}\rПодблок: {dataReader[8].ToString()}\rПроцесс: {dataReader[9].ToString()}"
    ));
            }
            Connection.Close();
        }

        public bool ForcedToQuit()
        {
            
            command = new SqlCommand("Select State from ForceQuit", Connection);
            adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return bool.Parse(dataTable.Rows[0]["State"].ToString());
        }
        public ObservableCollection<Analytic> GetMyAnalyticsData(Analytic currentUser)
        {
            string AccessLevelDescription = "id";
            int AccessLevelCode = currentUser.ID;
            ObservableCollection<Analytic> result = new ObservableCollection<Analytic>();
            switch (currentUser.Role)
            {
                case (1):
                    AccessLevelDescription = "_department";
                    AccessLevelCode = 1;
                    break;
                case (2):
                    AccessLevelDescription = "_direction";
                    AccessLevelCode = currentUser.Direction;
                    break;
                case (3):
                    AccessLevelDescription = "_upravlenie";
                    AccessLevelCode = currentUser.Upravlenie;
                    break;
                case (4):
                    AccessLevelDescription = "_otdel";
                    AccessLevelCode = currentUser.Otdel;
                    break;
            }
            string sqlState = $@"select Analytic.FirstName, Analytic.LastName, Analytic.FatherName, Analytic.id, Analytic.userName
from Analytic
Where Analytic.{AccessLevelDescription} = @AccessLevel";
            command = new SqlCommand(sqlState, Connection);
            command.Parameters.AddWithValue("@AccessLevel", AccessLevelCode);
            Connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                result.Add(new Analytic()
                {
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    FatherName = dataReader["FatherName"].ToString(),
                    ID = int.Parse(dataReader["id"].ToString()),
                    UserName = dataReader["userName"].ToString()
                });
            }

            Connection.Close();
            return result;
        }
        public void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end)
        {
            switch (ReportType)
            {
                case (0):
                    GetReportAnalyticsActivity(analytics, start, end);
                    break;
            }
        }
        private void GetReportAnalyticsActivity(Analytic[] analytics, DateTime start, DateTime end)
        {
            string inClause = string.Join(",", analytics.Select(x => x.ID.ToString()));
            command = new SqlCommand(
$@"SELECT [dbo].Analytic.LastName, [dbo].Analytic.FirstName, [dbo].Analytic.FatherName, [dbo].Block.blockName, [dbo].SubBlock.subblockName, [dbo].Process.procName, [dbo].TimeSheetTable.Subject, [dbo].TimeSheetTable.comment,
[dbo].TimeSheetTable.timeStart, [dbo].TimeSheetTable.timeEnd, [dbo].TimeSheetTable.TimeSpent, 
[dbo].BusinessBlock.BusinessBlockName, [dbo].Supports.Name, [dbo].ClientWays.Name, [dbo].Escalations.Name, [dbo].Formats.Name, [dbo].Risk.riskName
from TimeSheetTable
join [dbo].Process
	on [dbo].Process.id = [dbo].TimeSheetTable._Process
join [dbo].Risk
	on [dbo].Risk.id = [dbo].TimeSheetTable._Risk
join [dbo].Supports
	on [dbo].Supports.Id = [dbo].TimeSheetTable._Support
join [dbo].Block
	on [dbo].Block.Id = [dbo].Process.Block
join [dbo].Formats
	on [dbo].Formats.Id = [dbo].TimeSheetTable._Format
join [dbo].BusinessBlock
	on [dbo].BusinessBlock.Id = [dbo].TimeSheetTable._BusinessBlock
join [dbo].ClientWays
	on [dbo].ClientWays.Id = [dbo].TimeSheetTable._ClientWay
join [dbo].Escalations
	on [dbo].Escalations.Id = [dbo].TimeSheetTable._Escalation
join [dbo].SubBlock
	on [dbo].SubBlock.Id = [dbo].Process.SubBlock
join [dbo].Analytic
	on [dbo].Analytic.id = [dbo].TimeSheetTable._Analytic
WHERE Analytic.id in ({inClause})
AND timeStart > @dateTimeStart AND timeStart < @dateTimeEnd", Connection);
            command.Parameters.AddWithValue("@dateTimeStart", start);
            command.Parameters.AddWithValue("@dateTimeEnd", end);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable data = new DataTable();
            adapter.Fill(data);
            DataView dv = data.DefaultView;
            dv.Sort = "LastName";

            ExcelWorker.ExportDataTableToExcel(dv.ToTable());

        }
    }
}
