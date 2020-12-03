using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using TimeSheetApp.Model;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetTests
{
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
    [TestClass]
    public abstract class DataProviderTest
    {
        #region variables
        #region Connection string
#if !DevAtHome
        readonly string ConnectionString = @"data source=A105512\A105512;Initial Catalog=TimeSheet;Integrated Security=false;user id = TimeSheetuser; password = DK_user!; MultipleActiveResultSets=True;";
#else
        readonly string ConnectionString = @"data source=IlyaHome;Initial Catalog=TimeSheet;Integrated Security=true; MultipleActiveResultSets=True;";
#endif
        #endregion
        public abstract string ProviderName { get; }
        public abstract IDataProvider DataProvider { get; set; }
        readonly string TestUser = "u_m0x0c";
        #endregion

        [TestMethod]
        public void GetProcesses()
        {
            int expectedCount = GetTableCount("Process");
            List<Process> processes = DataProvider.GetProcesses().ToList();
            if (processes.Count != expectedCount)
            {
                throw new Exception($"Processes count is not 63, but {processes.Count}");
            }
            foreach(Process process in processes)
            {
                if (process.Block == null)
                {
                    throw new Exception("Process doesn't contains Blocks");
                }
                if (process.SubBlock == null)
                {
                    throw new Exception("Process doesn't contains SubBlocks");
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetBusinessBlocks()
        {
            bool succeed = true;
            int expectedCount = GetTableCount("BusinessBlock");
            List <BusinessBlock> businessBlocks = DataProvider.GetBusinessBlocks().ToList();
            if(businessBlocks.Count != expectedCount)
            {
                throw new Exception($"Количество возвращенных элементов не соответствует ожидаемым {expectedCount}. Фактическое количество: {businessBlocks.Count}");
            }
            Assert.IsTrue(succeed);
        }

        [TestMethod]
        public void GetSupports()
        {
            bool succeed = true;
            int expectedCount = GetTableCount("Supports");
            List<Supports> supports = DataProvider.GetSupports().ToList();
            if (supports.Count != expectedCount)
            {
                throw new Exception($"Количество возвращенных элементов не соответствует ожидаемым {expectedCount}. Фактическое количество: {supports.Count}");
            }
            Assert.IsTrue(succeed);
        }

        [TestMethod]
        public void GetClientWays()
        {
            bool succeed = true;
            int expectedCount = GetTableCount("ClientWays");
            List<ClientWays> clientWays = DataProvider.GetClientWays().ToList();
            if (clientWays.Count != expectedCount)
            {
                throw new Exception($"Количество возвращенных элементов не соответствует ожидаемым {expectedCount}. Фактическое количество: {clientWays.Count}");
            }
            Assert.IsTrue(succeed);
        }

        [TestMethod]
        public void GetEscalations()
        {
            bool succeed = true;
            int expectedCount = GetTableCount("Escalations");
            List<Escalation> escalations = DataProvider.GetEscalation().ToList();
            if (escalations.Count != expectedCount)
            {
                throw new Exception($"Количество возвращенных элементов не соответствует ожидаемым {expectedCount}. Фактическое количество: {escalations.Count}");
            }
            Assert.IsTrue(succeed);
        }

        [TestMethod]
        public void GetFormat()
        {
            bool succeed = true;
            int expectedCount = GetTableCount("Formats");
            List<Formats> formats = DataProvider.GetFormat().ToList();
            if (formats.Count != expectedCount)
            {
                throw new Exception($"Количество возвращенных элементов не соответствует ожидаемым {expectedCount}. Фактическое количество: {formats.Count}");
            }
            Assert.IsTrue(succeed);
        }

        [TestMethod]
        public void GetRisks()
        {
            bool succeed = true;
            int expectedCount = GetTableCount("RiskSet");
            List<Risk> formats = DataProvider.GetRisks().ToList();
            if (formats.Count != expectedCount)
            {
                throw new Exception($"Количество возвращенных элементов не соответствует ожидаемым {expectedCount}. Фактическое количество: {formats.Count}");
            }
            Assert.IsTrue(succeed);
        }

        [TestMethod]
        public void GetMyAnalyticsData()
        {
            Analytic TestAnalytic = DataProvider.LoadAnalyticData(TestUser);
            List<Analytic> analytics = DataProvider.GetMyAnalyticsData(TestAnalytic).ToList();

            if(analytics.Count < 1)
            {
                throw new Exception("Метод вернул 0 аналитиков");
            }
            
            //Проверяем наличие зависимых объектов
            foreach(Analytic analytic in analytics)
            {
                if (analytic.Departments == null)
                {
                    throw new Exception($"У аналитика {analytic.LastName} {analytic.FirstName} Department == null");
                }
                if (analytic.Directions == null)
                {
                    throw new Exception($"У аналитика {analytic.LastName} {analytic.FirstName} Direction == null");
                }
                if(analytic.Upravlenie == null)
                {
                    throw new Exception($"У аналитика {analytic.LastName} {analytic.FirstName} Upravlenie == null");
                }
                if (analytic.Otdel == null)
                {
                    throw new Exception($"У аналитика {analytic.LastName} {analytic.FirstName} Otdel == null");
                }
            }
            Assert.IsTrue(true);
        }
        
        [TestMethod]
        public void GetProcessBlocks()
        {
            int expectedCount = GetTableCount("Block");
            int factCount = DataProvider.GetProcessBlocks().Count();
            if (factCount != expectedCount)
            {
                throw new Exception($"Метод вернул не соответствующее ожиданию ({expectedCount}) количество записей ({factCount} )");
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetSubBlocksNames()
        {
            int expectedCount = GetTableCount("SubBlock");
            int factCount = DataProvider.GetSubBlocksNames().Count();
            if (factCount != expectedCount)
            {
                throw new Exception($"Метод вернул не соответствующее ожиданию ({expectedCount}) количество записей ({factCount} )");
            }
            Assert.IsTrue(true);
        }

        #region AddActivityTest
        [TestMethod]
        public void AddActivity()
        {
            TimeSheetTable testRecord = GenerateTestRecord();
            DataProvider.AddActivity(testRecord);
            DeleteIfExists(testRecord);
            Assert.IsTrue(true);
        }

        private TimeSheetTable GenerateTestRecord()
        {
            TimeSheetTable testRecord = new TimeSheetTable
            {
                AnalyticId = 1,
                Comment = "Тестовая запись",
                Process_id = 26,
                Subject = $"TestRecord_ToDelete!!!Provider: {ProviderName}{DateTime.Now:mm.ss}",
                TimeStart = DateTime.Now,
                TimeEnd = DateTime.Now.AddMinutes(15),
                FormatsId = 2,
                ClientWaysId = 1,
                TimeSpent = 15
            };
            testRecord.BusinessBlocks = new List<BusinessBlockNew>
            {
                new BusinessBlockNew
                {
                    BusinessBlockId = 1,
                    TimeSheetTableId = 0
                },
                new BusinessBlockNew
                {
                    BusinessBlockId = 2,
                    TimeSheetTableId = 0
                }
            };
            testRecord.Escalations = new List<EscalationNew>
            {
                new EscalationNew
                {
                    EscalationId = 1,
                    TimeSheetTableId = 0
                },
                new EscalationNew
                {
                    EscalationId = 2,
                    TimeSheetTableId = 0
                },
            };
            testRecord.Risks = new List<RiskNew>
            {
                new RiskNew
                {
                    RiskId = 1,
                    TimeSheetTableId= 0
                },
                new RiskNew
                {
                    RiskId = 2,
                    TimeSheetTableId= 0
                }
            };
            testRecord.Supports = new List<SupportNew>
            {
                new SupportNew
                {
                    SupportId = 1,
                    TimeSheetTableId = 0
                },
                new SupportNew
                {
                    SupportId = 2,
                    TimeSheetTableId = 0
                }
            };
            return testRecord;
        }


        private void DeleteIfExists(TimeSheetTable testRecord)
        {
            int? testRecordId;
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand GetRecordId = new SqlCommand($"SELECT id FROM TimeSheetTable where Subject = '{testRecord.Subject}'", connection);
            testRecordId = (int)GetRecordId.ExecuteScalar();
            if(!testRecordId.HasValue || testRecordId.Value == 0)
            {
                connection.Close();
                throw new Exception("Не удалось добавить тестовую запись в БД.");
            }
            SqlCommand isBusinessBlocksExists = new SqlCommand($"select count(*) from NewBusinessBlockSet where TimeSheetTableId = {testRecordId}", connection);
            SqlCommand isEscalationsExists = new SqlCommand($"select count(*) from NewEscalations where TimeSheetTableId = {testRecordId}", connection);
            SqlCommand isRisksExists = new SqlCommand($"select count(*) from NewRiskSet where TimeSheetTableId = {testRecordId}", connection);
            SqlCommand isSupportsExists = new SqlCommand($"select count(*) from NewSupportsSet where TimeSheetTableId = {testRecordId}", connection);

            int? BusinessBlocksCount = (int)isBusinessBlocksExists.ExecuteScalar();
            int? EscalationsCount = (int)isEscalationsExists.ExecuteScalar();
            int? RisksCount = (int)isRisksExists.ExecuteScalar();
            int? SupportsCount = (int)isSupportsExists.ExecuteScalar();

            if (!BusinessBlocksCount.HasValue && BusinessBlocksCount != 2)
            {
                connection.Close();
                throw new Exception("Запись была добавлена не полностью. Не добавлены бизнес блоки.");
            }
            if (!EscalationsCount.HasValue && EscalationsCount != 2)
            {
                connection.Close();
                throw new Exception("Запись была добавлена не полностью. Не добавлены эскалации.");
            }
            if (!RisksCount.HasValue && RisksCount != 2)
            {
                connection.Close();
                throw new Exception("Запись была добавлена не полностью. Не добавлены риски.");
            }
            if (!SupportsCount.HasValue && SupportsCount != 2)
            {
                connection.Close();
                throw new Exception("Запись была добавлена не полностью. Не добавлены саппорты.");
            }

            new SqlCommand($"delete FROM NewBusinessBlockSet where TimeSheetTableId = {testRecordId.Value}", connection).ExecuteNonQuery();
            new SqlCommand($"delete FROM NewEscalations where TimeSheetTableId = {testRecordId.Value}", connection).ExecuteNonQuery();
            new SqlCommand($"delete FROM NewRiskSet where TimeSheetTableId = {testRecordId.Value}", connection).ExecuteNonQuery();
            new SqlCommand($"delete FROM NewSupportsSet where TimeSheetTableId = {testRecordId.Value}", connection).ExecuteNonQuery();
            new SqlCommand($"delete FROM TimeSheetTable where id = {testRecordId.Value}", connection).ExecuteNonQuery();
            connection.Close();
        }

        #endregion

        [TestMethod]
        public void LoadAnalyticData()
        {
            string userName = "u_m0x0c";
            string expectedLastName = "Лебедев";
            Analytic loadedAnalytic = DataProvider.LoadAnalyticData(userName);
            if (loadedAnalytic.Departments == null || loadedAnalytic.Directions == null || loadedAnalytic.Upravlenie == null || loadedAnalytic.Otdel == null)
            {
                throw new Exception("Данные загружены не полностью. Отсутствует информация о подразделениях");
            }

            if (loadedAnalytic.AdminHead == null || loadedAnalytic.FunctionHead == null)
            {
                throw new Exception("Данные загружены не полностью. Отсутствует информация о руководителях");
            }

            if (!loadedAnalytic.LastName.Equals(expectedLastName))
            {
                throw new Exception($"Фамилия не соответствует ожидаемой. Фактически: {loadedAnalytic.LastName}. Ожидалось: {expectedLastName}");
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void UpdateProcess()
        {
            TimeSheetTable oldRecord = GenerateTSRecord();
            TimeSheetTable newRecord = new TimeSheetTable
            {
                TimeStart = new DateTime(2020,09,02,10,15,0),
                TimeEnd = new DateTime(2020,09,02,10,30,0),
                TimeSpent = 15,
                Comment = "TestEditedRecord",
                Subject = "TestEditedRecordSubject",
                ClientWaysId=1,
                FormatsId=2,
                AnalyticId=1,
                Process_id=25
            };
            newRecord.BusinessBlocks = new List<BusinessBlockNew>
            {
                new BusinessBlockNew
                {
                    BusinessBlockId=2,
                    TimeSheetTableId=oldRecord.Id
                }
            };
            DataProvider.UpdateProcess(oldRecord, newRecord);

            TimeSheetTable currentInstance = GetRecord(oldRecord.Id);

            if(currentInstance.TimeStart != newRecord.TimeStart ||
                currentInstance.TimeEnd != newRecord.TimeEnd ||
                currentInstance.TimeSpent != newRecord.TimeSpent ||
                !currentInstance.Comment.Equals(newRecord.Comment) ||
                !currentInstance.Subject.Equals(newRecord.Subject) ||
                currentInstance.ClientWaysId != newRecord.ClientWaysId ||
                currentInstance.FormatsId != newRecord.FormatsId ||
                currentInstance.AnalyticId != newRecord.AnalyticId ||
                currentInstance.Process_id != newRecord.Process_id)
            {
                DeleteRecord(oldRecord.Id);
                throw new Exception("Не все данные обновлены корректно");
            }else if(currentInstance.BusinessBlocks[0].BusinessBlockId != newRecord.BusinessBlocks[0].BusinessBlockId)
            {
                DeleteRecord(oldRecord.Id);
                throw new Exception("Подразделения не обновлены");
            }

            DeleteRecord(oldRecord.Id);
            Assert.IsTrue(true);
        }
        
        private TimeSheetTable GetRecord(int recordId)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand getRecord = new SqlCommand($"select * from TimeSheetTable join NewBusinessBlockSet on NewBusinessBlockSet.TimeSheetTableId = TimeSheetTable.id where TimeSheetTable.id = {recordId}", connection);
            SqlDataReader reader;
            TimeSheetTable recordFromDB = new TimeSheetTable();
            try
            {
                connection.Open();
                reader = getRecord.ExecuteReader();
                recordFromDB.BusinessBlocks = new List<BusinessBlockNew>();
                bool recFilled = false;
                while (reader.Read() != false)
                {
                    if (!recFilled)
                    {
                        recordFromDB.AnalyticId = Convert.ToInt32(reader["AnalyticId"].ToString());
                        recordFromDB.TimeSpent = Convert.ToInt32(reader["TimeSpent"].ToString());
                        recordFromDB.ClientWaysId = Convert.ToInt32(reader["ClientWaysId"].ToString());
                        recordFromDB.FormatsId = Convert.ToInt32(reader["FormatsId"].ToString());
                        recordFromDB.Process_id = Convert.ToInt32(reader["Process_id"].ToString());
                        recordFromDB.Id = Convert.ToInt32(reader["id"].ToString());
                        recordFromDB.TimeStart = Convert.ToDateTime(reader["TimeStart"].ToString());
                        recordFromDB.TimeEnd = Convert.ToDateTime(reader["TimeEnd"].ToString());
                        recordFromDB.Comment = reader["comment"].ToString();
                        recordFromDB.Subject = reader["Subject"].ToString();
                    }
                    recordFromDB.BusinessBlocks.Add(new BusinessBlockNew
                    {
                        BusinessBlockId = Convert.ToInt32(reader["BusinessBlockId"].ToString()),
                        TimeSheetTableId = Convert.ToInt32(reader["TimeSheetTableId"].ToString())
                    });
                }
            }catch(Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
            return recordFromDB;
        }

        #region Delete Test
        [TestMethod]
        public void DeleteRecord()
        {
            TimeSheetTable testRecord = GenerateTSRecord();
            DataProvider.DeleteRecord(testRecord.Id);
            if (CheckIfRecordExists(testRecord.Id))
            {
                DeleteRecord(testRecord.Id);
                throw new Exception("Запись не была удалена");
            }
            Assert.IsTrue(true);
        }

        private TimeSheetTable GenerateTSRecord()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "insert TimeSheetTable (timeStart, timeEnd, TimeSpent, comment, Subject, ClientWaysId, FormatsId, AnalyticId, Process_id) values " +
                "('2020-09-01 09:00', '2020-09-01 09:15', 15, 'TestRecord', 'TestRecordSubject', 1,2,1,26); SELECT SCOPE_IDENTITY();";
            TimeSheetTable record = new TimeSheetTable
            {
                TimeStart = new DateTime(2020,9,1,9,0,0),
                TimeEnd = new DateTime(2020,9,1,9,15,0),
                TimeSpent = 15,
                Comment = "TestRecord",
                Subject = "TestRecordSubject",
                ClientWaysId = 1,
                FormatsId = 2,
                AnalyticId = 1,
                Process_id = 26
            };
            SqlCommand command = new SqlCommand(sql, connection);
            int? newId;
            try
            {
                connection.Open();
                newId = Convert.ToInt32(command.ExecuteScalar());
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
            if (newId.HasValue)
            {
                record.Id = newId.Value;
            }
            else
            {
                throw new Exception("Failed to insert new TimeSheet record");
            }
            record.BusinessBlocks = new List<BusinessBlockNew>
            {
                new BusinessBlockNew
                {
                    BusinessBlockId = 1,
                    TimeSheetTableId = record.Id
                }
            };
            sql = $"insert NewBusinessBlockSet (BusinessBlockId, TimeSheetTableId) values (1,{record.Id}); SELECT SCOPE_IDENTITY();";
            SqlCommand createBB = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                newId = Convert.ToInt32(createBB.ExecuteScalar());
            }catch(Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
            if (newId.HasValue)
            {
                record.BusinessBlocks[0].Id = newId.Value;
            }
            else
            {
                throw new Exception("Failed to insert new BusinessBlock record");
            }
            return record;
        }

        private bool CheckIfRecordExists(int RecordId)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = $"select count(*) from TimeSheetTable where id = {RecordId}";
            SqlCommand command = new SqlCommand(sql, connection);
            int? count;
            try
            {
                connection.Open();
                count = (int)command.ExecuteScalar();
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
            if (!count.HasValue && count.Value==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DeleteRecord(int RecordId)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string DeleteBBSql = $"delete from NewBusinessBlockSet where TimeSheetTableId = {RecordId}";
            string DeleteRecSql = $"delete from TimeSheetTable where id = {RecordId}";
            SqlCommand deleteBB = new SqlCommand(DeleteBBSql, connection);
            SqlCommand deleteRecord = new SqlCommand(DeleteRecSql, connection);
            try
            {
                connection.Open();
                deleteBB.ExecuteNonQuery();
                deleteRecord.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        [TestMethod]
        public void LoadTimeSheetRecords()
        {
            int expectedCount = 6;
            string expectedSubject = "ТБСВК";
            DateTime date = new DateTime(2020, 7, 21);
            List<TimeSheetTable> records = DataProvider.LoadTimeSheetRecords(date, TestUser).ToList();
            if(records.Count != expectedCount)
            {
                throw new Exception($"Полученное значение не соответствует ожидаемому. Ожидалось: {expectedCount}. Получено {records.Count}");
            }
            if(!records.Any(i => i.Subject.Equals(expectedSubject)))
            {

                throw new Exception($"Не выгружена ожидаемая запись {expectedSubject}");
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void IsCollisionedWithOtherRecords()
        {
            TimeSheetTable colidedRecord = new TimeSheetTable
            {
                TimeStart = new DateTime(2020,7,21,13,45,20),
                TimeEnd = new DateTime(2020,7,21,14,0,0),
                AnalyticId = 1
            };
            TimeSheetTable notColidedRecord = new TimeSheetTable
            {
                TimeStart = new DateTime(2020, 7, 21, 18, 45, 20),
                TimeEnd = new DateTime(2020, 7, 21, 19, 0, 0),
                AnalyticId = 1
            };
            TimeSheetTable colidedRecordWithSameId = new TimeSheetTable
            {
                TimeStart = new DateTime(2020, 7, 21, 13, 5, 20),
                TimeEnd = new DateTime(2020, 7, 21, 13, 25, 0),
                AnalyticId = 1,
                Id = 121443
            };
            if (!DataProvider.IsCollisionedWithOtherRecords(colidedRecord))
            {
                throw new Exception("Пересечение не выявлено, хотя оно есть.");
            }
            if (DataProvider.IsCollisionedWithOtherRecords(notColidedRecord))
            {
                throw new Exception("Пересечение выявлено, хотя его нет.");
            }
            if (DataProvider.IsCollisionedWithOtherRecords(colidedRecordWithSameId))
            {
                throw new Exception("Пересечение выявлено, хотя его нет. Пересечение только с записью с тем же Id");
            }
            Assert.IsTrue(true);
        }


        [TestInitialize]
        public abstract void Setup();

        [TestCleanup]
        public abstract void Cleanup();

        #region доп методы
        private int GetTableCount(string TableName)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = $"select count(*) from {TableName}";
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            int? count = (int)command.ExecuteScalar();
            return count ?? 0;
        }
        #endregion
    }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
}
