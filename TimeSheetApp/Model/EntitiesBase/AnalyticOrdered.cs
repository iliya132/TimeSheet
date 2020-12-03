using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    public class StructuredAnalytic : INotifyPropertyChanged
    {
        public Analytic Analytic;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string FIO { get; set; }
        public string userName { get; set; }
        private bool selected = false;
        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }
        public string FirstStructure { get; set; }
        public string SecondStructure { get; set; }
        public string ThirdStructure { get; set; }
        public string FourStructure { get; set; }
        public StructuredAnalytic(Analytic _analytic)
        {
            Analytic = _analytic;

            FIO = $"{Analytic.LastName} {Analytic.FirstName} {Analytic.FatherName}";
            userName = Analytic.UserName;

            //Первое подразделение всегда департамент
            FirstStructure = Analytic.Departments.Name;

            //если всё, кроме департамента отсутствует
            if (Analytic.UpravlenieId==4 && Analytic.DirectionId == 2 && Analytic.OtdelId == 19) 
            {
                return;
            }

            //Определили второе подразделение (дирекция(если не отсутствует), управление(если нет дирекции), отдел(если нет управления)
            SecondStructure = Analytic.DirectionId != 2 ? Analytic.Directions.Name :
                Analytic.UpravlenieId != 6 ? Analytic.Upravlenie.Name : Analytic.Otdel.Name;

            //если дирекция и управление отсутствуют, значит отдел записан во второе подразделение, и дальше ничего делать не надо
            if (Analytic.DirectionId == 2 && Analytic.UpravlenieId == 6) return;

            //Управление(если не отсутствует), отдел (если отсутствует управление)
            ThirdStructure = Analytic.UpravlenieId != 6 ? Analytic.Upravlenie.Name : Analytic.Otdel.Name;
            if (Analytic.UpravlenieId == 6) return;

            //Отдел (когда структура полная)
            FourStructure = Analytic.Otdel.Name;
        }
        public override string ToString()
        {
            return $"{Analytic.Id}. {Analytic.FirstName} {Analytic.LastName} {Analytic.FatherName}";
        }
    }
}
