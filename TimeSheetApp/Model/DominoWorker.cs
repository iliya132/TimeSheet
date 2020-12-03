using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
#if !DevAtHome
using Domino;
#endif

namespace TimeSheetApp.Model
{
    public class DominoWorker
    {
#if !DevAtHome
        NotesSession notesSession;
        NotesDatabase notesDB;
        NotesDocument notesDoc;
        NotesView notesView;
        NotesDbDirectory notesDir;
        List<CalendarItem> _calendarItems = new List<CalendarItem>();
        List<CalendarItem> CalendarItems => _calendarItems;

        public DominoWorker()
        {
            Init();
        }
#endif

        public List<CalendarItem> GetCalendarRecords(DateTime date)
        {
#if !DevAtHome
            return CalendarItems.Where(i => i.StartTime.Date.CompareTo(date.Date) == 0).OrderBy(i=>i.StartTime).ToList();
#else
            return new List<CalendarItem>();
#endif
        }

#if !DevAtHome
        private void Init()
        {
            try
            {
                
                notesSession = new NotesSession();
                notesSession.Initialize();
                
                notesDir = notesSession.GetDbDirectory("Local");
                notesDB = notesDir.OpenMailDatabase();
                if (!notesDB.IsOpen)
                {
                    notesDB.Open();
                }
#region loading last month records
                notesView = notesDB.GetView("($Calendar)");
                notesDoc = notesView.GetLastDocument();

                List<NotesDocument> documents = new List<NotesDocument>();
                while (notesDoc != null)
                {
                    NotesItem startDateItem = notesDoc.GetFirstItem("StartDate");
                    NotesItem startTimeItem = notesDoc.GetFirstItem("StartTime");
                    NotesItem endDateItem = notesDoc.GetFirstItem("EndDate");
                    NotesItem endTimeItem = notesDoc.GetFirstItem("EndTime");
                    NotesItem repeatedItem = notesDoc.GetFirstItem("Repeats");
                    NotesItem startDateTimeItem = notesDoc.GetFirstItem("STARTDATETIME");
                    DateTime startDate = Convert.ToDateTime(startDateItem?.Text);
                    DateTime startTime = Convert.ToDateTime(startTimeItem?.Text);
                    DateTime endDate = Convert.ToDateTime(endDateItem?.Text);
                    DateTime endTime = Convert.ToDateTime(endTimeItem?.Text);
                    int repeated = Convert.ToInt32(repeatedItem?.Text);
                    string subj = notesDoc.GetFirstItem("Subject").Text;
                    string type = notesDoc.GetFirstItem("Form").Text;
                    DateTime startResult = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Hour, startTime.Minute, startTime.Second);
                    DateTime endResult = new DateTime(startDate.Year, startDate.Month, startDate.Day, endTime.Hour, endTime.Minute, endTime.Second);
                    if (repeated == 0)
                    {
                        CalendarItem item = new CalendarItem
                        {
                            Type = type,
                            Subject = subj,
                            StartTime = startResult,
                            EndTime = endResult
                        };
                        CalendarItems.Add(item);
                    }
                    else
                    {
                        string[] arr = startDateTimeItem.Text.Split(';');
                        foreach(string dateStr in arr)
                        {
                            DateTime date = Convert.ToDateTime(dateStr);
                            if (!CalendarItems.Any(i => i.Subject.Equals(subj) && i.StartTime.Date == date.Date))
                            {
                                CalendarItem item = new CalendarItem
                                {
                                    Type = type,
                                    Subject = subj,
                                    StartTime = date,//new DateTime(date.Year, date.Month, date.Day, startResult.Hour, startResult.Minute, 0),
                                    EndTime = new DateTime(date.Year, date.Month, date.Day, endResult.Hour, endResult.Minute, 0),
                                };
                                CalendarItems.Add(item);
                            }
                        }
                    }
                    notesDoc = notesView.GetPrevDocument(notesDoc);
                }
#endregion
            }
            catch
            {
                
            }

    }
#endif

    }
}
