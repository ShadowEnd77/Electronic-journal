using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class JournalGrade
    {
        public int IdRecord { get; set; } // id_записи
        public int IdStudentClass { get; set; } // id_ученик-класс
        public int IdTeacherSubject { get; set; } // id_учитель-предмет
        public int IdDate { get; set; } // id_дата
        public int Grade { get; set; } // Оценка

        public StudentClass StudentClass { get; set; } // Связь М:1 с Ученик-Класс
        public TeacherSubject TeacherSubject { get; set; } // Связь М:1 с Учитель-Предмет
        public Date Date { get; set; } // Связь М:1 с Дата
    }
}