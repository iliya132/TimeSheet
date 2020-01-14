using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp
{
    public class Analytic
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private int _role;

        public int Role
        {
            get { return _role; }
            set { _role = value; }
        }
        private int _direction;

        public int Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        private int _upravlenie;

        public int Upravlenie
        {
            get { return _upravlenie; }
            set { _upravlenie = value; }
        }
        private int _otdel;

        public int Otdel
        {
            get { return _otdel; }
            set { _otdel = value; }
        }

        private int _position;

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        private string _fatherName;

        public string FatherName
        {
            get { return _fatherName; }
            set { _fatherName = value; }
        }

    }
}
