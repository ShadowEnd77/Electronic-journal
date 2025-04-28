using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class Subjekt
    {
        public int IdSubject { get; set; } // id_предмета
        public string Name { get; set; } // Предмет

        public List<TeacherSubject> TeacherSubjects { get; set; } // Связь 1:М с Учитель-Предмет
        public List<JournalSubject> JournalSubjects { get; set; } // Связь 1:М с ЖурналПредмет
    }
}
