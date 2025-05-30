namespace WpfApp1.shell.ViewModel.Teachers
{
    public class ScheduleTeacherDay
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }
        public string FormattedDate { get; set; }
        public List<ScheduleTeacherSubject> Subjects { get; set; }
    }

    public class ScheduleTeacherSubject
    {
        public string SubjectName { get; set; }
        public string ClassName { get; set; }
    }
}