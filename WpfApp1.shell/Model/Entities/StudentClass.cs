using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WpfApp1.shell.Model.Entities;

namespace WpfApp1.shell.Model.Entities
{
    public class StudentClass
{
    public int IdStudentClass { get; set; } // id_ученик-класс
    public int IdStudent { get; set; } // id_ученика
    public int IdClass { get; set; } // id_класса

    public Student Student { get; set; } // Связь 1:1 с Ученики
    public Class Class { get; set; } // Связь 1:1 с Классы
    public List<JournalGrade> JournalGrades { get; set; } // Связь 1:М с ЖурналОценка
}
}
