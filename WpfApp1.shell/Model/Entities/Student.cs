using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class Student
    {
        public int IdStudent { get; set; } // id_ученика
        public int IdAccount { get; set; } // id_аккаунта
        public string LastName { get; set; } // Фамилия
        public string FirstName { get; set; } // Имя
        public string Patronymic { get; set; } // Отчество
        public string BirthDate { get; set; } // Дата_Рождения
        public string Gender { get; set; } // Пол

        public Account Account { get; set; } // Связь 1:1 с Аккаунты
        public StudentClass StudentClass { get; set; } // Связь 1:1 с Ученик-Класс
    }
}
