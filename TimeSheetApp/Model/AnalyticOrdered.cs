using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    public class AnalyticOrdered : INotifyPropertyChanged
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
        public AnalyticOrdered(Analytic _analytic)
        {
            Analytic = _analytic;

            FIO = $"{Analytic.LastName} {Analytic.FirstName} {Analytic.FatherName}";
            userName = Analytic.UserName;

            FirstStructure = Analytic.Departments.Name;

            SecondStructure = Analytic.DirectionId != 4 ? Analytic.Directions.Name :
                Analytic.UpravlenieId != 4 ? Analytic.Upravlenie.Name : Analytic.Otdel.Name;
            if (Analytic.DirectionId == 0 && Analytic.UpravlenieId == 4) return;

            ThirdStructure = Analytic.UpravlenieId != 4 ? Analytic.Upravlenie.Name : Analytic.Otdel.Name;
            if (Analytic.UpravlenieId == 4) return;

            FourStructure = Analytic.Otdel.Name;
            Console.WriteLine($"{FIO}: {FirstStructure} {SecondStructure} {ThirdStructure} {FourStructure}");
        }
    }
}
