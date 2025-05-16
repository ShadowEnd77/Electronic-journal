using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class Date
    {
        public int IdDate { get; set; } // id_Дата
        public int IdQuarter { get; set; } // id_четверти
        public DateTime DateValue { get; set; } // Дата

        public Quarter Quarter { get; set; } // Связь М:1 с Четверть
        public JournalSubject JournalSubject { get; set; } // Связь 1:1 с ЖурналПредмет
        public List<JournalGrade> JournalGrades { get; set; } // Связь 1:М с ЖурналОценка
    }
}
