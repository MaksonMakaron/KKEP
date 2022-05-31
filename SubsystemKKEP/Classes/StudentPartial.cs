using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubsystemKKEP.Classes
{
    public partial class Student
    {
        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName} {Patronymic}";
            }
            set { }
        }

        private string shortName;

        public string ShortName
        {
            get
            {
                if (Patronymic != null)
                {
                    return $"{LastName} {FirstName[0]}. {Patronymic[0]}.";
                }
                else
                {
                    return $"{LastName} {FirstName[0]}";
                }
            }
            set { shortName = value; }
        }
    }
}
