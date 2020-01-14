using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp
{
    public class TimeSheetHistoryItem
    {
        DateTime start;
        DateTime end;
        string Subject;

        public TimeSheetHistoryItem(DateTime _start, DateTime _end, string _subject)
        {
            start = _start;
            end = _end;
            Subject = _subject;
        }
    }
}
