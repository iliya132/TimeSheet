using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using Domino;

namespace TimeSheetApp.Model
{
    public class DominoWorker
    {
        //NotesDatabase notesDB;
        //NotesDocument notesDoc;
        //NotesSession notesSession;
        //NotesView notesView;
        //NotesDbDirectory notesDir;
        List<CalendarItem> _calendarItems = new List<CalendarItem>();

        private void write(string msg)
        {
            //using (StreamWriter writer = new StreamWriter("DominoLog.txt", true))
            //{
            //    writer.WriteLine(msg);
            //}
        }
        public DominoWorker()
        {
            //Init();
        }

        public List<CalendarItem> GetCalendarRecords(DateTime date) => _calendarItems.Where(i => i.StartTime.Day == date.Day &&
                                                                                             i.StartTime.Month == date.Month &&
                                                                                             i.StartTime.Year == date.Year)
                                                                                            .Distinct()
                                                                                            .ToList();

        //private void Init()
        //{
        //    try
        //    {
        //        notesSession = new NotesSession();
        //        notesSession.Initialize(string.Empty);
        //        notesDir = notesSession.GetDbDirectory("Local");
        //        notesDB = notesDir.OpenMailDatabase();
        //        if (!notesDB.IsOpen)
        //        {
        //            notesDB.Open();
        //        }
        //        #region loading last month records
        //        notesView = notesDB.GetView("($Calendar)");
        //        notesDoc = notesView.GetLastDocument();
        //        DateTime start = DateTime.Now;
        //        DateTime end = DateTime.Now;
        //        List<NotesDocument> documents = new List<NotesDocument>();
        //        while (notesDoc != null)
        //        {
        //            documents.Add(notesDoc);
        //            notesDoc = notesView.GetPrevDocument(notesDoc);
        //        }

        //        foreach(NotesDocument doc in documents)
        //        {
        //            string type = doc.GetFirstItem("Form").Values[0];
        //            string subj = doc.GetFirstItem("Subject").Values[0];

        //            NotesItem startTime = doc.GetFirstItem("STARTDATETIME");
        //            NotesItem endTime = doc.GetFirstItem("EndDateTime");
        //            if(startTime == null || endTime == null)
        //            {
        //                continue;
        //            }
        //            start = startTime == null ? start : startTime.Values[0];
        //            end = endTime == null ? end : endTime.Values[0];

        //            string typeRus = type;
        //            if (type.Equals("Task")) typeRus = "Задача";
        //            if (type.Equals("Appointment")) typeRus = "Встреча";


        //            CalendarItem newItem = new CalendarItem()
        //            {
        //                Type = type,
        //                Subject = $"({typeRus}) {subj}",
        //                StartTime = start,
        //                EndTime = end
        //            };
        //            _calendarItems.Add(newItem);
        //        }
        //        #endregion
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}
    }
}
