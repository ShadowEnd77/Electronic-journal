using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class Class
    {
        public int IdClass { get; set; } // id_класса
        public int IdTeacher { get; set; } // id_учителя
        public string Letter { get; set; } // Литера
        public int Number { get; set; } // Цифра

        public Teacher Teacher { get; set; } // Связь 1:1 с Учителя
        public List<StudentClass> StudentClasses { get; set; } // Связь 1:М с Ученик-Класс
        public List<JournalSubject> JournalSubjects { get; set; } // Связь 1:М с ЖурналПредмет
    }
}
