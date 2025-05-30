using Microsoft.EntityFrameworkCore;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.shell.Model;
using WpfApp1.shell.Model.Entities;
using WpfApp1.shell.ViewModel.Teachers;

namespace WpfApp1.shell.ViewModel
{
    public class SchedulePageViewModel : BindableBase
    {
        private readonly SchoolDbContext _dbContext;
        private int _currentTeacherId;
        private Quarter _selectedQuarter;
        private string _searchDate;
        private List<ScheduleTeacherDay> _allScheduleDays;

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

        public string SearchDate
        {
            get => _searchDate;
            set
            {
                SetProperty(ref _searchDate, value);
                ApplyDateFilter();
            }
        }

        private List<ScheduleTeacherDay> _filteredScheduleDays;
        public List<ScheduleTeacherDay> FilteredScheduleDays
        {
            get => _filteredScheduleDays;
            set => SetProperty(ref _filteredScheduleDays, value);
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
                    FormattedDate = g.Key.ToString("dd.MM.yyyy"),
                    Subjects = g.Select(js => new ScheduleTeacherSubject
                    {
                        SubjectName = js.Subject?.Name ?? "Без названия",
                        ClassName = js.Class?.ClassName ?? "Класс не указан"
                    }).ToList()
                })
                .OrderBy(d => d.Date)
                .ToList();

            _allScheduleDays = days;
            ApplyDateFilter();
        }

        private void ApplyDateFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchDate))
            {
                FilteredScheduleDays = _allScheduleDays;
            }
            else
            {
                var searchParts = SearchDate.Split('.');
                var normalizedSearch = string.Join(".", searchParts.Select(p => p.Trim()));

                FilteredScheduleDays = _allScheduleDays
                    .Where(d => d.FormattedDate.Contains(normalizedSearch))
                    .ToList();
            }
        }
    }

    
}