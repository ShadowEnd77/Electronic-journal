using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class Teacher
    {
        public int IdTeacher { get; set; } // id_учителя
        public int IdAccount { get; set; } // id_аккаунта
        public string LastName { get; set; } // Фамилия
        public string FirstName { get; set; } // Имя
        public string Patronymic { get; set; } // Отчество

        public Account Account { get; set; } // Связь 1:1 с Аккаунты
        public Class Class { get; set; } // Связь 1:1 с Классы
        public List<TeacherSubject> TeacherSubjects { get; set; } // Связь 1:М с Учитель-Предмет
    }
}
