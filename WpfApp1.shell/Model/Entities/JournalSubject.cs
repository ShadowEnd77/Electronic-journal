using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class JournalSubject
    {
        public int IdRDay { get; set; } // id_РДень
        public int IdTeacherSubject { get; set; } // id_учитель-предмет
        public int IdDate { get; set; } // id_дата
        public int IdClass { get; set; } // id_класса
        public int IdSubject { get; set; } // id_предмета

        public TeacherSubject TeacherSubject { get; set; } // Связь 1:1 с Учитель-Предмет
        public Date Date { get; set; } // Связь 1:1 с Дата
        public Class Class { get; set; } // Связь 1:1 с Классы
        public Subjekt Subject { get; set; } // Связь М:1 с Предметы
    }
}
