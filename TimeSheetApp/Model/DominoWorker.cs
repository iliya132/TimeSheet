using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domino;

namespace TimeSheetApp.Model
{
    public class DominoWorker
    {
        NotesDatabase notesDB;
        NotesDocument notesDoc;
        NotesSession notesSession;
        NotesView notesView;
        NotesDbDirectory notesDir;

        public List<CalendarItem> GetCalendarRecords()
        {
            List<CalendarItem> exportVal = new List<CalendarItem>();

            Init();
            notesView = notesDB.GetView("($Calendar)");
            
            notesDoc = notesView.GetFirstDocument();

            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            while (notesDoc != null) {
                if (notesDoc.GetFirstItem("StartDate").Values[0] == today) {

                    string type = notesDoc.GetFirstItem("Form").Values[0];
                    string subj = notesDoc.GetFirstItem("Subject").Values[0];
                    DateTime start = notesDoc.GetFirstItem("STARTDATETIME").Values[0];
                    DateTime end = notesDoc.GetFirstItem("EndDateTime").Values[0];

                    CalendarItem newItem = new CalendarItem()
                    {
                        Type = type,
                        Subject = subj,
                        StartTime = start,
                        EndTime = end
                    };

                    exportVal.Add(newItem);
                }
                    notesDoc = notesView.GetNextDocument(notesDoc);
            }


            return exportVal;
        }
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
