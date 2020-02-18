using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Windows;
using TimeSheetApp.Model.EntitiesBase;
using TimeSheetApp.Model.Reports;
using System.Text;

namespace TimeSheetApp.Model
{
    class EFDataProvider : IEFDataProvider
    {
        TimeSheetContext context = new TimeSheetContext();

        /// <summary>
        /// Получить подсказки для поля тема
        /// </summary>
        /// <param name="process">Процесс, к которому нужно подобрать подсказки</param>
        /// <returns>Стек тем, введенных ранее пользователем</returns>
        public Stack<string> GetSubjectHints(Process process)
        {
            Stack<string> subjects = new Stack<string>();
            Dictionary<string, int> subjectCounted = new Dictionary<string, int>();

            int proc_id;

            if (process != null)
            {
                proc_id = process.Id;

                foreach(string item in context.TimeSheetTableSet.Where(i=>i.Analytic.UserName.ToLower().Equals(Environment.UserName.ToLower()) &&
                i.Subject.Length > 0 && i.Process_id == proc_id).Select(i => i.Subject).ToArray())
                {
                    if (subjectCounted.ContainsKey(item))
                    {
                        subjectCounted[item]++;
                    }
                    else
                    {
                        subjectCounted.Add(item, 1);
                    }
                }
                foreach(KeyValuePair<string, int> item in (from i in subjectCounted orderby i.Value ascending select i))
                {
                    subjects.Push(item.Key);
                }
            }
            else
            {
                foreach (string item in context.TimeSheetTableSet.Where(i => i.Analytic.UserName.ToLower().Equals(Environment.UserName.ToLower()) &&
                 i.Subject.Length > 0).Select(i => i.Subject).ToArray())
                {
                    if (subjectCounted.ContainsKey(item))
                    {
                        subjectCounted[item]++;
                    }
                    else
                    {
                        subjectCounted.Add(item, 1);
                    }
                }
                foreach (KeyValuePair<string, int> item in (from i in subjectCounted orderby i.Value ascending select i))
                {
                    subjects.Push(item.Key);
                }
            }
            return subjects;
        }

        /// <summary>
        /// Получить расшифровку кода процесса
        /// </summary>
        /// <param name="process"></param>
        /// <returns>Строка, содержащая имена: Блок-Подблок-Процесс</returns>
        public string GetCodeDescription(Process process) => $"{process.Block.BlockName}\r\n{process.SubBlock.SubblockName}\r\n{process.ProcName}";

        /// <summary>
        /// Получить список всех существующих процессов
        /// </summary>
        /// <returns>ObservableCollection</returns>
        public ObservableCollection<Process> GetProcesses()
        {
            ObservableCollection<Process> processes;

            processes = new ObservableCollection<Process>(context.ProcessSet);

            return processes;
        }

        /// <summary>
        /// Получить список всех БизнесПодразделений
        /// </summary>
        /// <returns>OBservableCollection</returns>
        public BusinessBlock[] GetBusinessBlocks()
        {
            BusinessBlock[] businessBlocks;
            businessBlocks = context.BusinessBlockSet.ToArray();
            return businessBlocks;
        }

        /// <summary>
        /// Возвращает объект из модели соответствующий входящему id
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <param name="Type">0-BusinessBlock, 1-Supports, 2-Escalation, 3-RiskChoice</param>
        /// <returns></returns>
        public List<object> GetChoice(int ObjectId, int Type)
        {
            List<object> returnValue = new List<object>();
            switch (Type)
            {
                //TODO: переписать этот ужас)
                case (0):
                    BusinessBlockChoice choice = context.BusinessBlockChoiceSet.FirstOrDefault(i => i.Id == ObjectId);
                    if (choice != null)
                    {
                        #region Присваиваем значения выбора бизнес блока по порядку 
                        if (choice.BusinessBlock_id != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id));
                        if (choice.BusinessBlock_id1 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id1));
                        if (choice.BusinessBlock_id2 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id2));
                        if (choice.BusinessBlock_id3 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id3));
                        if (choice.BusinessBlock_id4 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id4));
                        if (choice.BusinessBlock_id5 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id5));
                        if (choice.BusinessBlock_id6 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id6));
                        if (choice.BusinessBlock_id7 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id7));
                        if (choice.BusinessBlock_id8 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id8));
                        if (choice.BusinessBlock_id9 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id9));
                        if (choice.BusinessBlock_id10 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id10));
                        if (choice.BusinessBlock_id11 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id11));
                        if (choice.BusinessBlock_id12 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id12));
                        if (choice.BusinessBlock_id13 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id13));
                        if (choice.BusinessBlock_id14 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id14));
                        if (choice.BusinessBlock_id15 != null) returnValue.Add(context.BusinessBlockSet.FirstOrDefault(i => i.Id == choice.BusinessBlock_id15));
                        #endregion
                    }
                    break;
                case (1):
                    SupportChoice supChoice = context.SupportChoiceSet.FirstOrDefault(i => i.Id == ObjectId);
                    if (supChoice == null) break;
                    if (supChoice.Support_id != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id));
                    if (supChoice.Support_id1 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id1));
                    if (supChoice.Support_id2 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id2));
                    if (supChoice.Support_id3 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id3));
                    if (supChoice.Support_id4 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id4));
                    if (supChoice.Support_id5 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id5));
                    if (supChoice.Support_id6 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id6));
                    if (supChoice.Support_id7 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id7));
                    if (supChoice.Support_id8 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id8));
                    if (supChoice.Support_id9 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id9));
                    if (supChoice.Support_id10 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id10));
                    if (supChoice.Support_id11 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id11));
                    if (supChoice.Support_id12 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id12));
                    if (supChoice.Support_id13 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id13));
                    if (supChoice.Support_id14 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id14));
                    if (supChoice.Support_id15 != null) returnValue.Add(context.SupportsSet.FirstOrDefault(i => i.Id == supChoice.Support_id15));
                    break;
                case (2):
                    EscalationChoice EscChoice = context.EscalationChoiceSet.FirstOrDefault(i => i.Id == ObjectId);
                    if (EscChoice == null) break;
                    if (EscChoice.Escalation_id != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id));
                    if (EscChoice.Escalation_id1 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id1));
                    if (EscChoice.Escalation_id2 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id2));
                    if (EscChoice.Escalation_id3 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id3));
                    if (EscChoice.Escalation_id4 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id4));
                    if (EscChoice.Escalation_id5 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id5));
                    if (EscChoice.Escalation_id6 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id6));
                    if (EscChoice.Escalation_id7 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id7));
                    if (EscChoice.Escalation_id8 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id8));
                    if (EscChoice.Escalation_id9 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id9));
                    if (EscChoice.Escalation_id10 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id10));
                    if (EscChoice.Escalation_id11 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id11));
                    if (EscChoice.Escalation_id12 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id12));
                    if (EscChoice.Escalation_id13 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id13));
                    if (EscChoice.Escalation_id14 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id14));
                    if (EscChoice.Escalation_id15 != null) returnValue.Add(context.EscalationsSet.FirstOrDefault(i => i.Id == EscChoice.Escalation_id15));
                    break;
                case (3):
                    RiskChoice riskChoice = context.RiskChoiceSet.FirstOrDefault(i => i.Id == ObjectId);
                    if (riskChoice == null) break;
                    if (riskChoice.Risk_id != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id));
                    if (riskChoice.Risk_id1 != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id1));
                    if (riskChoice.Risk_id2 != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id2));
                    if (riskChoice.Risk_id3 != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id3));
                    if (riskChoice.Risk_id4 != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id4));
                    if (riskChoice.Risk_id5 != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id5));
                    if (riskChoice.Risk_id6 != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id6));
                    if (riskChoice.Risk_id7 != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id7));
                    if (riskChoice.Risk_id8 != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id8));
                    if (riskChoice.Risk_id9 != null) returnValue.Add(context.RiskSet.FirstOrDefault(i => i.Id == riskChoice.Risk_id9));
                    break;
            }

            return returnValue;

        }

        /// <summary>
        /// Добавить выбор Риска
        /// </summary>
        /// <param name="RiskChoice">Мультивыбор</param>
        /// <returns>id новой строки RiskChoiceId</returns>
        public int AddRiskChoice(RiskChoice RiskChoice)
        {
            if (RiskChoice == null)
            {
                RiskChoice = new RiskChoice();
            }
            if (context.RiskChoiceSet.Any(i =>
            i.Risk_id == RiskChoice.Risk_id &&
            i.Risk_id1 == RiskChoice.Risk_id1 &&
            i.Risk_id2 == RiskChoice.Risk_id2 &&
            i.Risk_id3 == RiskChoice.Risk_id3 &&
            i.Risk_id4 == RiskChoice.Risk_id4 &&
            i.Risk_id5 == RiskChoice.Risk_id5 &&
            i.Risk_id6 == RiskChoice.Risk_id6 &&
            i.Risk_id7 == RiskChoice.Risk_id7 &&
            i.Risk_id8 == RiskChoice.Risk_id8
            ))
            {
                return context.RiskChoiceSet.First(
            i => i.Risk_id == RiskChoice.Risk_id &&
            i.Risk_id1 == RiskChoice.Risk_id1 &&
            i.Risk_id2 == RiskChoice.Risk_id2 &&
            i.Risk_id3 == RiskChoice.Risk_id3 &&
            i.Risk_id4 == RiskChoice.Risk_id4 &&
            i.Risk_id5 == RiskChoice.Risk_id5 &&
            i.Risk_id6 == RiskChoice.Risk_id6 &&
            i.Risk_id7 == RiskChoice.Risk_id7 &&
            i.Risk_id8 == RiskChoice.Risk_id8).Id;
            }
            else
            {
                context.RiskChoiceSet.Add(RiskChoice);
                context.SaveChanges();
                return RiskChoice.Id;
            }

        }

        /// <summary>
        /// Добавить выбор бизнес блоков
        /// </summary>
        /// <param name="BBChoice"></param>
        /// <returns>Id нового BusinessBlockChoice в БД</returns>
        public int AddBusinessBlockChoice(BusinessBlockChoice BBChoice)
        {
            if (BBChoice == null)
            {
                BBChoice = new BusinessBlockChoice();
            }
            int _id = 0;

            if (context.BusinessBlockChoiceSet.Any(i =>
            i.BusinessBlock_id == BBChoice.BusinessBlock_id &&
            i.BusinessBlock_id1 == BBChoice.BusinessBlock_id1 &&
            i.BusinessBlock_id2 == BBChoice.BusinessBlock_id2 &&
            i.BusinessBlock_id3 == BBChoice.BusinessBlock_id3 &&
            i.BusinessBlock_id4 == BBChoice.BusinessBlock_id4 &&
            i.BusinessBlock_id5 == BBChoice.BusinessBlock_id5 &&
            i.BusinessBlock_id6 == BBChoice.BusinessBlock_id6 &&
            i.BusinessBlock_id7 == BBChoice.BusinessBlock_id7 &&
            i.BusinessBlock_id8 == BBChoice.BusinessBlock_id8
            ))
            {
                _id = context.BusinessBlockChoiceSet.First(
            i => i.BusinessBlock_id == BBChoice.BusinessBlock_id &&
            i.BusinessBlock_id1 == BBChoice.BusinessBlock_id1 &&
            i.BusinessBlock_id2 == BBChoice.BusinessBlock_id2 &&
            i.BusinessBlock_id3 == BBChoice.BusinessBlock_id3 &&
            i.BusinessBlock_id4 == BBChoice.BusinessBlock_id4 &&
            i.BusinessBlock_id5 == BBChoice.BusinessBlock_id5 &&
            i.BusinessBlock_id6 == BBChoice.BusinessBlock_id6 &&
            i.BusinessBlock_id7 == BBChoice.BusinessBlock_id7 &&
            i.BusinessBlock_id8 == BBChoice.BusinessBlock_id8).Id;
            }
            else
            {
                context.BusinessBlockChoiceSet.Add(BBChoice);
                context.SaveChanges();
                _id = BBChoice.Id;
            }
            return _id;
        }

        /// <summary>
        /// Добавить в БД выбор Эскалации
        /// </summary>
        /// <param name="escalationChoice"></param>
        /// <returns>Id нового списка выбора в БД</returns>
        public int AddEscalationChoice(EscalationChoice escalationChoice)
        {
            if (escalationChoice == null)
            {
                escalationChoice = new EscalationChoice();
            }

            int _id = 0;

            if (context.EscalationChoiceSet.Any(i =>
            i.Escalation_id == escalationChoice.Escalation_id &&
            i.Escalation_id1 == escalationChoice.Escalation_id1 &&
            i.Escalation_id2 == escalationChoice.Escalation_id2 &&
            i.Escalation_id3 == escalationChoice.Escalation_id3 &&
            i.Escalation_id4 == escalationChoice.Escalation_id4 &&
            i.Escalation_id5 == escalationChoice.Escalation_id5 &&
            i.Escalation_id6 == escalationChoice.Escalation_id6 &&
            i.Escalation_id7 == escalationChoice.Escalation_id7 &&
            i.Escalation_id8 == escalationChoice.Escalation_id8
            ))
            {
                _id = context.EscalationChoiceSet.First(
            i => i.Escalation_id == escalationChoice.Escalation_id &&
            i.Escalation_id1 == escalationChoice.Escalation_id1 &&
            i.Escalation_id2 == escalationChoice.Escalation_id2 &&
            i.Escalation_id3 == escalationChoice.Escalation_id3 &&
            i.Escalation_id4 == escalationChoice.Escalation_id4 &&
            i.Escalation_id5 == escalationChoice.Escalation_id5 &&
            i.Escalation_id6 == escalationChoice.Escalation_id6 &&
            i.Escalation_id7 == escalationChoice.Escalation_id7 &&
            i.Escalation_id8 == escalationChoice.Escalation_id8).Id;
            }
            else
            {
                context.EscalationChoiceSet.Add(escalationChoice);
                context.SaveChanges();
                _id = escalationChoice.Id;
            }

            return _id;
        }

        /// <summary>
        /// Добавить в БД выбор Саппортов
        /// </summary>
        /// <param name="_suppChoice"></param>
        /// <returns>Id нового списка выбора в БД</returns>
        public int AddSupportChoiceSet(SupportChoice _suppChoice)
        {
            if (_suppChoice == null)
            {
                _suppChoice = new SupportChoice();
            }

            int _id = 0;

            if (context.SupportChoiceSet.Any(i =>
            i.Support_id == _suppChoice.Support_id &&
            i.Support_id1 == _suppChoice.Support_id1 &&
            i.Support_id2 == _suppChoice.Support_id2 &&
            i.Support_id3 == _suppChoice.Support_id3 &&
            i.Support_id4 == _suppChoice.Support_id4 &&
            i.Support_id5 == _suppChoice.Support_id5 &&
            i.Support_id6 == _suppChoice.Support_id6 &&
            i.Support_id7 == _suppChoice.Support_id7 &&
            i.Support_id8 == _suppChoice.Support_id8
            ))
            {
                _id = context.SupportChoiceSet.First(
            i => i.Support_id == _suppChoice.Support_id &&
            i.Support_id1 == _suppChoice.Support_id1 &&
            i.Support_id2 == _suppChoice.Support_id2 &&
            i.Support_id3 == _suppChoice.Support_id3 &&
            i.Support_id4 == _suppChoice.Support_id4 &&
            i.Support_id5 == _suppChoice.Support_id5 &&
            i.Support_id6 == _suppChoice.Support_id6 &&
            i.Support_id7 == _suppChoice.Support_id7 &&
            i.Support_id8 == _suppChoice.Support_id8).Id;
            }
            else
            {
                context.SupportChoiceSet.Add(_suppChoice);
                context.SaveChanges();
                _id = _suppChoice.Id;
            }

            return _id;
        }

        /// <summary>
        /// Добавить запись в TimeSheetTable
        /// </summary>
        /// <param name="activity">Процесс</param>
        public void AddActivity(TimeSheetTable activity)
        {
            TimeSpan span = activity.TimeEnd - activity.TimeStart;
            activity.TimeSpent = (int)span.TotalMinutes;
            activity.BusinessBlockChoice = context.BusinessBlockChoiceSet.FirstOrDefault(i => i.Id == activity.BusinessBlockChoice_id);
            activity.SupportChoice = context.SupportChoiceSet.FirstOrDefault(i => i.Id == activity.SupportChoice_id);
            activity.EscalationChoice = context.EscalationChoiceSet.FirstOrDefault(i => i.Id == activity.EscalationChoice_id);
            activity.RiskChoice = context.RiskChoiceSet.FirstOrDefault(i => i.Id == activity.RiskChoice_id);
            context.TimeSheetTableSet.Add(activity);
            context.SaveChanges();
        }

        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="record"></param>
        public void DeleteRecord(TimeSheetTable record)
        {
            context.TimeSheetTableSet.Remove(record);
            context.SaveChanges();
        }

        /// <summary>
        /// Отслеживание состояния принудительного выхода
        /// </summary>
        /// <returns>false</returns>
        public bool ForcedToQuit()
        {
            return false;
        }

        /// <summary>
        /// Возвращает названия всех блоков
        /// </summary>
        /// <returns></returns>
        public List<string> GetBlocksList()
        {
            List<string> BlocksList;
            BlocksList = new List<string>(context.BlockSet.Select(i => i.BlockName).ToArray());
            return BlocksList;
        }

        /// <summary>
        /// Получить список всех клиентских путей
        /// </summary>
        /// <returns></returns>
        public ClientWays[] GetClientWays()
        {
            ClientWays[] clientWays;
            clientWays = context.ClientWaysSet.ToArray();
            return clientWays;
        }

        /// <summary>
        /// Получить список всех Эскалаций
        /// </summary>
        /// <returns></returns>
        public Escalation[] GetEscalation()
        {
            Escalation[] escalations;
            escalations = context.EscalationsSet.ToArray();
            return escalations;
        }

        /// <summary>
        /// Получить список всех форматов
        /// </summary>
        /// <returns></returns>
        public Formats[] GetFormat()
        {
            Formats[] formats;
            formats = context.FormatsSet.ToArray();
            return formats;
        }

        /// <summary>
        /// Получить список выбора риска
        /// </summary>
        /// <param name="riskChoice"></param>
        /// <returns></returns>
        public Risk[] LoadRiskChoice(RiskChoice riskChoice)
        {
            Risk[] risks;
            risks = context.RiskSet.Where(
                risk =>
            risk.Id == riskChoice.Risk_id ||
            risk.Id == riskChoice.Risk_id1 ||
            risk.Id == riskChoice.Risk_id2 ||
            risk.Id == riskChoice.Risk_id3 ||
            risk.Id == riskChoice.Risk_id4 ||
            risk.Id == riskChoice.Risk_id5 ||
            risk.Id == riskChoice.Risk_id6 ||
            risk.Id == riskChoice.Risk_id7 ||
            risk.Id == riskChoice.Risk_id8 ||
            risk.Id == riskChoice.Risk_id9).ToArray();
            return risks;
        }

        /// <summary>
        /// Получить список выбора бизнесблоков
        /// </summary>
        /// <param name="businessBlockChoice"></param>
        /// <returns></returns>
        public BusinessBlock[] LoadBusinessBlockChoice(BusinessBlockChoice businessBlockChoice)
        {
            BusinessBlock[] businessBlocks;
            businessBlocks = context.BusinessBlockSet.Where(
                businessBlock =>
            businessBlock.Id == businessBlockChoice.BusinessBlock_id ||
            businessBlock.Id == businessBlockChoice.BusinessBlock_id1 ||
            businessBlock.Id == businessBlockChoice.BusinessBlock_id2 ||
            businessBlock.Id == businessBlockChoice.BusinessBlock_id3 ||
            businessBlock.Id == businessBlockChoice.BusinessBlock_id4 ||
            businessBlock.Id == businessBlockChoice.BusinessBlock_id5 ||
            businessBlock.Id == businessBlockChoice.BusinessBlock_id6 ||
            businessBlock.Id == businessBlockChoice.BusinessBlock_id7 ||
            businessBlock.Id == businessBlockChoice.BusinessBlock_id8 ||
            businessBlock.Id == businessBlockChoice.BusinessBlock_id9).ToArray();
            return businessBlocks;
        }

        /// <summary>
        /// Получить список выбора саппортов
        /// </summary>
        /// <param name="SupportChoice"></param>
        /// <returns></returns>
        public Supports[] LoadSupportsChoice(SupportChoice SupportChoice)
        {
            Supports[] supports;
            supports = context.SupportsSet.Where(
                support =>
            support.Id == SupportChoice.Support_id ||
            support.Id == SupportChoice.Support_id1 ||
            support.Id == SupportChoice.Support_id2 ||
            support.Id == SupportChoice.Support_id3 ||
            support.Id == SupportChoice.Support_id4 ||
            support.Id == SupportChoice.Support_id5 ||
            support.Id == SupportChoice.Support_id6 ||
            support.Id == SupportChoice.Support_id7 ||
            support.Id == SupportChoice.Support_id8 ||
            support.Id == SupportChoice.Support_id9).ToArray();
            return supports;
        }

        /// <summary>
        /// Получить список выбора эскалаций
        /// </summary>
        /// <param name="escalationChoice"></param>
        /// <returns></returns>
        public Escalation[] LoadEscalationChoice(EscalationChoice escalationChoice)
        {
            Escalation[] escalations;

            escalations = context.EscalationsSet.Where(
                escalation =>
            escalation.Id == escalationChoice.Escalation_id ||
            escalation.Id == escalationChoice.Escalation_id1 ||
            escalation.Id == escalationChoice.Escalation_id2 ||
            escalation.Id == escalationChoice.Escalation_id3 ||
            escalation.Id == escalationChoice.Escalation_id4 ||
            escalation.Id == escalationChoice.Escalation_id5 ||
            escalation.Id == escalationChoice.Escalation_id6 ||
            escalation.Id == escalationChoice.Escalation_id7 ||
            escalation.Id == escalationChoice.Escalation_id8 ||
            escalation.Id == escalationChoice.Escalation_id9).ToArray();

            return escalations;
        }

        /// <summary>
        /// Получить список сотрудников в подчинении
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public ObservableCollection<Analytic> GetMyAnalyticsData(Analytic currentUser)
        {
            ObservableCollection<Analytic> analytics = new ObservableCollection<Analytic>();
            switch (currentUser.RoleTableId)
            {
                case (1):
                    analytics = new ObservableCollection<Analytic>(context.AnalyticSet.Where(i => i.DepartmentId == currentUser.DepartmentId).ToArray());
                    break;
                case (2):
                    analytics = new ObservableCollection<Analytic>(context.AnalyticSet.Where(i => i.DirectionId == currentUser.DirectionId).ToArray());
                    break;
                case (3):
                    analytics = new ObservableCollection<Analytic>(context.AnalyticSet.Where(i => i.UpravlenieId == currentUser.UpravlenieId).ToArray());
                    break;
                case (4):
                    analytics = new ObservableCollection<Analytic>(context.AnalyticSet.Where(i => i.OtdelId == currentUser.OtdelId).ToArray());
                    break;
                case (5):
                    analytics = new ObservableCollection<Analytic>(context.AnalyticSet.ToArray());
                    break;
                default:
                    analytics = new ObservableCollection<Analytic>(context.AnalyticSet.Where(i => i.Id == currentUser.Id).ToArray());
                    break;
            }
            return analytics;
        }

        /// <summary>
        /// Выгрузить отчет
        /// </summary>
        /// <param name="ReportType">Тип отчета</param>
        /// <param name="analytics">список выбранных аналитиков</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void GetReport(int ReportType, Analytic[] analytics, DateTime start, DateTime end)
        {
            switch (ReportType)
            {
                case (0):
                    ExcelWorker.ExportDataTableToExcel(GetAnalyticsReport(analytics, start, end));
                    break;
                case (1):
                    Report_02 report = new Report_02(context, analytics);
                    report.Generate(start, end);
                    break;
            }
        }

        /// <summary>
        /// Получить список всех рисков
        /// </summary>
        /// <returns></returns>
        public Risk[] GetRisks()
        {
            Risk[] risks;
            risks = context.RiskSet.ToArray();
            return risks;
        }

        /// <summary>
        /// Получить лист подблоков
        /// </summary>
        /// <returns></returns>
        public List<string> GetSubBlocksList()
        {
            List<string> SubBlockNames;
            SubBlockNames = new List<string>(context.SubBlockSet.Select(i => i.SubblockName).ToArray());
            return SubBlockNames;
        }

        /// <summary>
        /// Получить массив всех саппортов
        /// </summary>
        /// <returns></returns>
        public Supports[] GetSupports()
        {
            Supports[] supports;
            supports = context.SupportsSet.ToArray();
            return supports;
        }

        /// <summary>
        /// Метод устанавливает свойство видимости вкладки "Кабинет руководителя"
        /// </summary>
        /// <param name="currentUser">Текущий пользователь</param>
        /// <returns></returns>
        public Visibility IsAnalyticHasAccess(Analytic currentUser)
        {
            if (currentUser.Role.Id < 6)
                return Visibility.Visible;
            else return Visibility.Hidden;
        }


        /// <summary>
        /// Получает информацию о текущем аналитике, и если запись в БД не существует - создаёт новую
        /// </summary>
        /// <returns></returns>
        public Analytic LoadAnalyticData()
        {
            string user = Environment.UserName;
            Analytic analytic = new Analytic();

            if (context.AnalyticSet.Any(i => i.UserName.ToLower().Equals(user.ToLower())))
            {
                analytic = context.AnalyticSet.FirstOrDefault(i => i.UserName.ToLower().Equals(user.ToLower()));
            }
            else
            {
                analytic = new Analytic()
                {
                    UserName = user,
                    DepartmentId = 1,
                    DirectionId = 1,
                    FirstName = "NotSet",
                    LastName = "NotSet",
                    FatherName = "NotSet",
                    OtdelId = 1,
                    PositionsId = 1,
                    RoleTableId = 1,
                    UpravlenieId = 1
                };
                context.AnalyticSet.Add(analytic);
                context.SaveChanges();
                analytic = context.AnalyticSet.FirstOrDefault(i => i.UserName.ToLower().Equals(Environment.UserName.ToLower()));
            }

            return analytic;
        }

        /// <summary>
        /// Загружает все записи в TimeSheetTable по аналитику за выбранную дату
        /// </summary>
        /// <param name="date"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<TimeSheetTable> LoadTimeSheetRecords(DateTime date, Analytic user)
        {
            List<TimeSheetTable> timeSheetTables;
            timeSheetTables = context.TimeSheetTableSet.Where(i => i.AnalyticId == user.Id && DbFunctions.TruncateTime(i.TimeStart) == date.Date).ToList();
            return timeSheetTables;
        }

        /// <summary>
        /// Изменить процесс
        /// </summary>
        /// <param name="oldProcess"></param>
        /// <param name="newProcess"></param>
        public void UpdateProcess(TimeSheetTable oldProcess, TimeSheetTable newProcess)
        {
            oldProcess.Subject = newProcess.Subject;
            oldProcess.Comment = newProcess.Comment;
            oldProcess.Process = newProcess.Process;
            oldProcess.BusinessBlockChoice = newProcess.BusinessBlockChoice;
            oldProcess.SupportChoice = newProcess.SupportChoice;
            oldProcess.Process = newProcess.Process;
            oldProcess.TimeEnd = newProcess.TimeEnd;
            oldProcess.TimeSpent = newProcess.TimeSpent;
            oldProcess.ClientWays = newProcess.ClientWays;
            oldProcess.EscalationChoice = newProcess.EscalationChoice;
            oldProcess.Formats = newProcess.Formats;
            oldProcess.RiskChoice = newProcess.RiskChoice;
            oldProcess.TimeStart = newProcess.TimeStart;
            oldProcess.BusinessBlockChoice = context.BusinessBlockChoiceSet.FirstOrDefault(i => i.Id == newProcess.BusinessBlockChoice_id);
            oldProcess.SupportChoice = context.SupportChoiceSet.FirstOrDefault(i => i.Id == newProcess.SupportChoice_id);
            oldProcess.EscalationChoice = context.EscalationChoiceSet.FirstOrDefault(i => i.Id == newProcess.EscalationChoice_id);
            oldProcess.RiskChoice = context.RiskChoiceSet.FirstOrDefault(i => i.Id == newProcess.RiskChoice_id);

            context.SaveChanges();

        }

        /// <summary>
        /// Проверяет пересекается ли переданная запись с другими во времени
        /// </summary>
        /// <param name="record"></param>
        /// <returns>true если пересекается, false если нет</returns>
        public bool IsCollisionedWithOtherRecords(TimeSheetTable record)
        {
            bool state = false;
            foreach (TimeSheetTable historyRecord in context.TimeSheetTableSet.Where(i => i.AnalyticId == record.AnalyticId))
            {
                if (historyRecord.Id != record.Id && isInInterval(record.TimeStart, record.TimeEnd, historyRecord.TimeStart, historyRecord.TimeEnd))
                {
                    state = true;
                }
            }
            return state;
        }

        /// <summary>
        /// Алгоритм проверки вхождение одного промежутка времени в другой
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="start2"></param>
        /// <param name="end2"></param>
        /// <returns></returns>
        private bool isInInterval(DateTime start, DateTime end, DateTime start2, DateTime end2)
        {
            if ((start >= start2 && start < end2) || //начальная дата в интервале
                (end > start2 && end <= end2) || //конечная дата в интервале
                (start <= start2 && end >= end2)) //промежуток времени между датами включает интервал
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Отчет по аналитикам
        /// </summary>
        /// <param name="analytics"></param>
        /// <param name="TimeStart"></param>
        /// <param name="TimeEnd"></param>
        /// <returns></returns>
        public DataTable GetAnalyticsReport(Analytic[] analytics, DateTime TimeStart, DateTime TimeEnd)
        {
            DataTable dataTable = new DataTable();

            #region placeColumns
            dataTable.Columns.Add("LastName");
            dataTable.Columns.Add("FirstName");
            dataTable.Columns.Add("FatherName");
            dataTable.Columns.Add("BlockName");
            dataTable.Columns.Add("SubblockName");
            dataTable.Columns.Add("ProcessName");
            dataTable.Columns.Add("Subject");
            dataTable.Columns.Add("Body");
            dataTable.Columns.Add("TimeStart");
            dataTable.Columns.Add("TimeEnd");
            dataTable.Columns.Add("TimeSpent");
            dataTable.Columns.Add("BusinessBlockName");
            dataTable.Columns.Add("SupportsName");
            dataTable.Columns.Add("ClientWaysName");
            dataTable.Columns.Add("EscalationsName");
            dataTable.Columns.Add("FormatsName");
            dataTable.Columns.Add("RiskName");
            #endregion

            #region getData
            foreach (Analytic analytic in analytics)
            {
                List<TimeSheetTable> ReportEntity = new List<TimeSheetTable>();
                ReportEntity = context.TimeSheetTableSet.Where(
                    record => record.AnalyticId == analytic.Id &&
                    record.TimeStart > TimeStart && record.TimeStart < TimeEnd).ToList();
                for (int i = 0; i < ReportEntity.Count; i++)
                {
                    DataRow row = dataTable.Rows.Add();
                    row["LastName"] = ReportEntity[i].Analytic.LastName;
                    row["FirstName"] = ReportEntity[i].Analytic.FirstName;
                    row["FatherName"] = ReportEntity[i].Analytic.FatherName;
                    row["BlockName"] = ReportEntity[i].Process.Block.BlockName;
                    row["SubblockName"] = ReportEntity[i].Process.SubBlock.SubblockName;
                    row["ProcessName"] = ReportEntity[i].Process.ProcName;
                    row["Subject"] = ReportEntity[i].Subject;
                    row["Body"] = ReportEntity[i].Comment;
                    row["TimeStart"] = ReportEntity[i].TimeStart;
                    row["TimeEnd"] = ReportEntity[i].TimeEnd;
                    row["TimeSpent"] = ReportEntity[i].TimeSpent;

                    #region Добавление информации о мультивыборе

                    StringBuilder choice = new StringBuilder();
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock1 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock1?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock2 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock2?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock3 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock3?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock4 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock4?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock5 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock5?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock6 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock6?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock7 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock7?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock8 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock8?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock9 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock9?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock10 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock10?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock11 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock11?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock12 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock12?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock13 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock13?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock14 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock14?.BusinessBlockName}; ");
                    if (ReportEntity[i].BusinessBlockChoice.BusinessBlock15 != null)
                        choice.Append($"{ReportEntity[i].BusinessBlockChoice.BusinessBlock15?.BusinessBlockName}; ");

                    row["BusinessBlockName"] = choice.ToString();
                    #endregion

                    #region Добавление строки о мультивыборе саппорта
                    choice.Clear();

                    if (ReportEntity[i].SupportChoice.Support1 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support1?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support2 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support2?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support3 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support3?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support4 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support4?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support5 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support5?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support6 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support6?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support7 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support7?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support8 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support8?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support9 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support9?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support10 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support10?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support11 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support11?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support12 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support12?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support13 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support13?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support14 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support14?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support15 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support15?.Name}; ");
                    if (ReportEntity[i].SupportChoice.Support16 != null)
                        choice.Append($"{ReportEntity[i].SupportChoice.Support16?.Name}; ");

                    row["SupportsName"] = choice.ToString();

                    #endregion

                    row["ClientWaysName"] = ReportEntity[i].ClientWays.Name;

                    #region добавление строки о мультивыбора эскалации

                    choice.Clear();

                    if (ReportEntity[i].EscalationChoice.Escalation1 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation1?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation2 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation2?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation3 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation3?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation4 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation4?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation5 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation5?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation6 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation6?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation7 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation7?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation8 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation8?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation9 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation9?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation10 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation10?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation11 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation11?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation12 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation12?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation13 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation13?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation14 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation14?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation15 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation15?.Name}; ");
                    if (ReportEntity[i].EscalationChoice.Escalation16 != null)
                        choice.Append($"{ReportEntity[i].EscalationChoice.Escalation16?.Name}; ");

                    row["EscalationsName"] = choice.ToString();

                    #endregion

                    row["FormatsName"] = ReportEntity[i].Formats.Name;

                    #region добавление строки о мультивыборе риска

                    choice.Clear();
                    if(ReportEntity[i].RiskChoice.Risk1!=null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk1?.RiskName}; ");
                    if (ReportEntity[i].RiskChoice.Risk2 != null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk2?.RiskName}; ");
                    if (ReportEntity[i].RiskChoice.Risk3 != null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk3?.RiskName}; ");
                    if (ReportEntity[i].RiskChoice.Risk4 != null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk4?.RiskName}; ");
                    if (ReportEntity[i].RiskChoice.Risk5 != null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk5?.RiskName}; ");
                    if (ReportEntity[i].RiskChoice.Risk6 != null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk6?.RiskName}; ");
                    if (ReportEntity[i].RiskChoice.Risk7 != null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk7?.RiskName}; ");
                    if (ReportEntity[i].RiskChoice.Risk8 != null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk8?.RiskName}; ");
                    if (ReportEntity[i].RiskChoice.Risk9 != null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk9?.RiskName}; ");
                    if (ReportEntity[i].RiskChoice.Risk10 != null)
                        choice.Append($"{ReportEntity[i].RiskChoice.Risk10?.RiskName}; ");

                    row["RiskName"] = choice.ToString();

                    #endregion
                }
            }
            #endregion
            return dataTable;
        }
    }
}
