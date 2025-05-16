using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class JournalGrade
    {
        public int IdRecord { get; set; }         // id_записи (первичный ключ)
        public int IdStudentClass { get; set; }   // id_ученик-класс
        public int IdTeacherSubject { get; set; } // id_учитель-предмет
        public int IdDate { get; set; }           // id_дата
        public int Grade { get; set; }            // Оценка

        public StudentClass StudentClass { get; set; }
        public TeacherSubject TeacherSubject { get; set; }
        public Date Date { get; set; }
    }

}