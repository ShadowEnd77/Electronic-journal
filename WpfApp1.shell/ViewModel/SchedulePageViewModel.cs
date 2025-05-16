using Microsoft.EntityFrameworkCore;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.shell.Model;
using WpfApp1.shell.Model.Entities;
using WpfApp1.shell.ViewModel.Teachers;
using WpfApp1.shell.ViewModel;

namespace WpfApp1.shell.ViewModel
{
    public class SchedulePageViewModel : BindableBase
    {
        private readonly SchoolDbContext _dbContext;
        private int _currentTeacherId;
        private Quarter _selectedQuarter;
        private List<ScheduleTeacherDay> _scheduleDays;

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

        public List<ScheduleTeacherDay> ScheduleDays
        {
            get => _scheduleDays;
            set => SetProperty(ref _scheduleDays, value);
        }

        public SchedulePageViewModel(SchoolDbContext dbContext, int teacherId)
        {
            _dbContext = dbContext;
            _currentTeacherId = teacherId;
            LoadQuarters();
        }

        private void LoadQuarters()
        {
            Quarters = _dbContext.Quarters
                .OrderBy(q => q.IdQuarter)
                .Select(q => new Quarter
                {
                    IdQuarter = q.IdQuarter,
                    Name = $"Четверть {q.IdQuarter}",
                    StartDate = q.StartDate,
                    EndDate = q.EndDate
                })
                .ToList();

            SelectedQuarter = Quarters.FirstOrDefault();
        }

        private void LoadSchedule()
        {
            if (SelectedQuarter == null) return;

            var scheduleData = _dbContext.JournalSubjects
                .Include(js => js.Date)
                .Include(js => js.Subject)
                .Include(js => js.TeacherSubject)
                .Include(js => js.Class)
                .Where(js => js.TeacherSubject.IdTeacher == _currentTeacherId
                            && js.Date.IdQuarter == SelectedQuarter.IdQuarter)
                .OrderBy(js => js.Date.DateValue)
                .AsNoTracking()
                .ToList();

            var days = scheduleData
                .GroupBy(js => js.Date.DateValue.Date)
                .Select(g => new ScheduleTeacherDay
                {
                    Date = g.Key,
                    DayOfWeek = g.Key.ToString("dddd"),
                    Subjects = g.Select(js => new ScheduleTeacherSubject
                    {
                        SubjectName = js.Subject?.Name ?? "Без названия",
                        ClassName = js.Class?.ClassName ?? "Класс не указан"
                    }).ToList()
                })
                .OrderBy(d => d.Date)
                .ToList();

            ScheduleDays = days;
        }
    }

}