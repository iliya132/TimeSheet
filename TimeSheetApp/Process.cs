using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp
{
    public class Process:INotifyPropertyChanged, IDataErrorInfo, ICloneable
    {
        #region Variables
        private DateTime _procDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        public DateTime ProcDate
        {
            get { return _procDate; }
            set { _procDate = value; }
        }
        private int _result;

        public int Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public string BlockName, SubBlockName;

        private DateTime _dateTimeStart = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        public DateTime DateTimeStart {
            get
            {
                return new DateTime(ProcDate.Year, ProcDate.Month, ProcDate.Day, _dateTimeStart.Hour, _dateTimeStart.Minute, _dateTimeStart.Second);
            }
            set => _dateTimeStart = value; }
        private DateTime _dateTimeEnd = DateTime.Now.AddMinutes(15);
        public DateTime DateTimeEnd
        {
            get { return new DateTime(ProcDate.Year, ProcDate.Month, ProcDate.Day, _dateTimeEnd.Hour, _dateTimeEnd.Minute, _dateTimeEnd.Second); }
            set => _dateTimeEnd = value; }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private int _choosenCounter;
        public int ChoosenCounter
        {
            get { return _choosenCounter; }
            set
            {
                _choosenCounter = value;
                OnPropertyChanged("ChoosenCounter");
            }
        }
        public string ProcTypeName { get; set; }
        private int _block;
        public int Block
        {
            get => _block;
            set
            {
                _block = value;
                OnPropertyChanged("Block");
            }
        }
        private int _subBlock;
        public int SubBlock
        {
            get => _subBlock;
            set
            {
                _subBlock = value;
                OnPropertyChanged("SubBlock");
            }
        }
        private int _id=0;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public int IdForIndexes
        {
            get;
            set;
            
        }
        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }
        private string _comment=string.Empty;
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }
        private string _body="";

        public string Body
        {
            get { return _body; }
            set { _body = value;  }
        }

        private int _format;
        public int Format
        {
            get { return _format; }
            set { _format = value; OnPropertyChanged("Format"); }
        }
        private string _subject = string.Empty;
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; OnPropertyChanged("Subject"); }
        }
        private int _procType;
        public int ProcType
        {
            get { return _procType; }
            set { _procType = value; OnPropertyChanged("ProcType"); }
        }
        private int _escalation;

        public int Escalation
        {
            get { return _escalation; }
            set { _escalation = value; OnPropertyChanged("Escalation"); }
        }
        private int _risk;

        public int Risk
        {
            get { return _risk; }
            set { _risk = value; OnPropertyChanged("Risk"); }
        }
        private int _businessBlock;

        public int BusinessBlock
        {
            get { return _businessBlock; }
            set { _businessBlock = value; OnPropertyChanged("BusinessBlock"); }
        }
        private int _support;

        public int Support
        {
            get { return _support; }
            set { _support = value; OnPropertyChanged("Support"); }
        }
        private int _clientWay;

        public int ClientWay
        {
            get { return _clientWay; }
            set { _clientWay = value; OnPropertyChanged("ClientWay"); }
        }
        private int _analytic;

        public int Analytic
        {
            get => _analytic;
            set => _analytic = value;
        }
        public int TimeSpent
        {
            get
            {
                TimeSpan span = DateTimeEnd - DateTimeStart;
                return (int)span.TotalMinutes;
            }
        }

        public bool BodyNeeded { get; set; }

        public string Error { get { return string.Empty; } }

        public string this[string propertyName]
        {
            get
            {
                if (BodyNeeded && propertyName.Equals("Body"))
                {
                    return "Для данного процесса поле является обязательным";
                }
                else
                {

                    return "";
                }
                
            }
        }
        private string _codeFull;
        public string CodeFull { get => _codeFull; set => _codeFull = value; }
        #endregion
        public Process() : this(string.Empty, 0, 0, 0) { }
        public Process(string _name, int _block, int _subBlock, int Id) : this(_name, _block, _subBlock, Id, "null", -1) { }
        public Process(string _name,  int _block, int _subBlock, int Id, string _comment, int _resul, string _blockName="null", string _subBlockName="null", bool _bodyNeeded=false)
        {
            Name = _name;
            Block = _block;
            SubBlock = _subBlock;
            this.Id = Id;
            Comment = _comment;
            Code = $"{Block}.{SubBlock}.{this.Id}";
            BodyNeeded = _bodyNeeded;
            _result = _resul;
            BlockName = _blockName;
            SubBlockName = _subBlockName;
            CodeFull = $"Блок: {BlockName}\rПодблок: {SubBlockName}\rПроцесс: {Name}";

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
