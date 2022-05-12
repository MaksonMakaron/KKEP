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
        }
    }
}
