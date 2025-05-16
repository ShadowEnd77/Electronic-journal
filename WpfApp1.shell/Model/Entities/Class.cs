using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class Class
    {
        public int IdClass { get; set; }
        public int IdTeacher { get; set; }
        public string Letter { get; set; }
        public int Number { get; set; }

        // Добавляем вычисляемое свойство для имени класса
        public string ClassName => $"{Number}{Letter}";

        public Teacher Teacher { get; set; }
        public List<StudentClass> StudentClasses { get; set; }
        public List<JournalSubject> JournalSubjects { get; set; }
    }
}
