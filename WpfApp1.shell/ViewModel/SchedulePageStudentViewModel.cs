using Microsoft.EntityFrameworkCore;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp1.shell.Model;
using WpfApp1.shell.Model.Entities;

namespace WpfApp1.shell.ViewModel
{
    public class SchedulePageStudentViewModel : BindableBase
    {
        private readonly SchoolDbContext _dbContext;
        private int _currentStudentId;
        private Quarter _selectedQuarter;
        private List<ScheduleDay> _scheduleDays;

        public List<Quarter> Quarters { get; private set; }
        public Quarter SelectedQuarter
        {
            get => _selectedQuarter;
            set
            {
                SetProperty(ref _selectedQuarter, value);
                LoadSchedule();
            }
        }

        public List<ScheduleDay> ScheduleDays
        {
            get => _scheduleDays;
            set => SetProperty(ref _scheduleDays, value);
        }

        public SchedulePageStudentViewModel(SchoolDbContext dbContext, int studentId)
        {
            _dbContext = dbContext;
            _currentStudentId = studentId;
            LoadQuarters();
        }

        private void LoadQuarters()
        {
            var quarters = _dbContext.Quarters
                .OrderBy(q => q.IdQuarter)
                .Select(q => new Quarter
                {
                    IdQuarter = q.IdQuarter,
                    Name = $"Четверть {q.IdQuarter}",
                    StartDate = q.StartDate,
                    EndDate = q.EndDate
                })
                .ToList();

            Quarters = quarters;
            SelectedQuarter = Quarters.FirstOrDefault();
        }

        private void LoadSchedule()
        {
            if (SelectedQuarter == null) return;

            var studentClass = _dbContext.StudentClasses
                .Include(sc => sc.Class)
                .FirstOrDefault(sc => sc.IdStudent == _currentStudentId);

            if (studentClass?.Class == null) return;

            var scheduleData = _dbContext.JournalSubjects
                .Include(js => js.Date)
                .Include(js => js.Subject)
                .Include(js => js.TeacherSubject)
                    .ThenInclude(ts => ts.Teacher)
                .Where(js => js.IdClass == studentClass.IdClass
                             && js.Date != null
                             && js.Date.IdQuarter == SelectedQuarter.IdQuarter)
                .OrderBy(js => js.Date.DateValue)
                .AsNoTracking()
                .ToList();

            var days = scheduleData
                .GroupBy(js => js.Date.DateValue.Date)
                .Select(g => new ScheduleDay
                {
                    Date = g.Key,
                    DayOfWeek = g.Key.ToString("dddd"),
                    // Убираем группировку по предметам - оставляем все записи
                    Subjects = g.Select(js => new ScheduleSubject
                    {
                        SubjectName = js.Subject?.Name ?? "Без названия",
                        TeacherName = FormatTeacherName(js.TeacherSubject?.Teacher)
                    }).ToList()
                })
                .OrderBy(d => d.Date)
                .ToList();

            ScheduleDays = days;
        }

        private static string FormatTeacherName(Teacher teacher)
        {
            if (teacher == null) return "Преподаватель не указан";

            var lastName = teacher.LastName ?? "Неизвестно";
            var firstName = teacher.FirstName?.FirstOrDefault().ToString() ?? "?";
            var patronymic = teacher.Patronymic?.FirstOrDefault().ToString() ?? "?";

            return $"{lastName} {firstName}.{patronymic}.";
        }
    }

    public class ScheduleDay
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }
        public List<ScheduleSubject> Subjects { get; set; }
    }

    public class ScheduleSubject
    {
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
    }
}