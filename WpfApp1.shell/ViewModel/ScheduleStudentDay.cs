// ScheduleStudentDay.cs
namespace WpfApp1.shell.ViewModel.Students
{
    public class ScheduleStudentDay
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }
        public List<ScheduleStudentSubject> Subjects { get; set; }
    }

    public class ScheduleStudentSubject
    {
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
    }
}