using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class TeacherSubject
    {
        public int IdTeacherSubject { get; set; } // id_учитель-предмет
        public int IdTeacher { get; set; } // id_учителя
        public int IdSubject { get; set; } // id_предмета

        public Teacher Teacher { get; set; } // Связь М:1 с Учителя
        public Subjekt Subject { get; set; } // Связь М:1 с Предметы
        public List<JournalSubject> JournalSubjects { get; set; } // Связь 1:М с ЖурналПредмет
        public List<JournalGrade> JournalGrades { get; set; } // Связь 1:М с ЖурналОценка
    }
}