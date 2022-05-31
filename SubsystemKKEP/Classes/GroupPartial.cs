using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubsystemKKEP.Classes
{
    public partial class Group
    {
        public string IsArchiveText
        {
            get
            {
                return IsArchive ? "В архиве" : "Не в архиве";
            }
        }
    }
}
