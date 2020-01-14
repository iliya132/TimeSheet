using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp
{
    public class TimeSpanClass : INotifyPropertyChanged
    {
        private DateTime _timein;
        private DateTime _timeOut;
        private string _processName;
        private string _subject;
        private string _code;
        private string _codeFull;
        public DateTime timeIn { get => _timein; set { _timein = value; OnPropertyChanged("timeIn"); } }
        public DateTime timeOut { get => _timeOut; set { _timeOut = value; OnPropertyChanged("timeOut"); } }
        public string processName { get => _processName; set { _processName = value; OnPropertyChanged("processName"); } }
        public string Subject { get => _subject; set { _subject = value; OnPropertyChanged("Subject"); } }
        public string Code { get => _code; set { _code = value; OnPropertyChanged("Code"); } }
        public string CodeFull { get => _codeFull; set { _codeFull = value; OnPropertyChanged("CodeFull"); } }

        public TimeSpanClass(DateTime _timeIn, DateTime _timeOut, string _procName, string _subject, string _code=null, string _codeFull=null)
        {
            timeIn = _timeIn;
            timeOut = _timeOut;
            processName = _procName;
            Subject = _subject;
            Code = _code;
            CodeFull = _codeFull;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
