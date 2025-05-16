using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class Quarter
    {
        public int IdQuarter { get; set; } // id_четверти
        public DateTime StartDate { get; set; } // Дата_нач
        public DateTime EndDate { get; set; } // Дата_кон

        public string Name { get; set; } // Название четвверти

        public List<Date> Dates { get; set; } // Связь 1:М с Дата
    }
}
