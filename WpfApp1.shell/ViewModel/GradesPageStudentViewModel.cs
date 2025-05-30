using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WpfApp1.shell.Model;
using WpfApp1.shell.Model.Entities;

namespace WpfApp1.shell.ViewModel
{
    public class GradesPageStudentViewModel : BindableBase
    {
        private readonly SchoolDbContext _dbContext;
        private ObservableCollection<Subjekt> _subjects;
        private ObservableCollection<Quarter> _quarters;
        private ObservableCollection<DateWithGrades> _dates;
        private ObservableCollection<DateWithGrades> _allDates;
        private Quarter _selectedQuarter;
        private Student _currentStudent;
        private readonly int _accountId;
        private string _searchDate;
        private bool _showAverages;

        public ObservableCollection<Subjekt> Subjects
        {
            get => _subjects;
            set => SetProperty(ref _subjects, value);
        }

        public ObservableCollection<Quarter> Quarters
        {
            get => _quarters;
            set => SetProperty(ref _quarters, value);
        }

        public ObservableCollection<DateWithGrades> Dates
        {
            get => _dates;
            set => SetProperty(ref _dates, value);
        }

        public Quarter SelectedQuarter
        {
            get => _selectedQuarter;
            set
            {
                if (SetProperty(ref _selectedQuarter, value))
                {
                    LoadGradesData();
                }
            }
        }

        public Student CurrentStudent
        {
            get => _currentStudent;
            set => SetProperty(ref _currentStudent, value);
        }

        public string SearchDate
        {
            get => _searchDate;
            set
            {
                if (SetProperty(ref _searchDate, value))
                {
                    ApplyDateFilter();
                }
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand ShowAveragesCommand { get; }

        public GradesPageStudentViewModel(int accountId)
        {
            _accountId = accountId;
            _dbContext = new SchoolDbContext();
            Subjects = new ObservableCollection<Subjekt>();
            Quarters = new ObservableCollection<Quarter>();
            Dates = new ObservableCollection<DateWithGrades>();
            _allDates = new ObservableCollection<DateWithGrades>();

            RefreshCommand = new RelayCommand(_ =>
            {
                _showAverages = false;
                LoadGradesData();
            });

            ShowAveragesCommand = new RelayCommand(_ =>
            {
                _showAverages = true;
                CalculateAverages();
            });

            LoadInitialData();
        }

        private void LoadInitialData()
        {
            try
            {
                CurrentStudent = _dbContext.Students
                    .Include(s => s.StudentClass)
                    .FirstOrDefault(s => s.Account.IdAccount == _accountId);

                Quarters = new ObservableCollection<Quarter>(_dbContext.Quarters
                    .OrderBy(q => q.IdQuarter)
                    .Select(q => new Quarter
                    {
                        IdQuarter = q.IdQuarter,
                        Name = $"Четверть {q.IdQuarter}",
                        StartDate = q.StartDate,
                        EndDate = q.EndDate
                    })
                    .ToList());

                Subjects = new ObservableCollection<Subjekt>(_dbContext.Subjects.ToList());

                if (Quarters.Any())
                {
                    SelectedQuarter = Quarters.First();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void LoadGradesData()
        {
            try
            {
                if (CurrentStudent == null || SelectedQuarter == null) return;

                Dates.Clear();
                _allDates.Clear();

                var quarterDates = _dbContext.Dates
                    .Where(d => d.IdQuarter == SelectedQuarter.IdQuarter)
                    .OrderBy(d => d.DateValue)
                    .ToList();

                var grades = _dbContext.JournalGrades
                    .Include(jg => jg.TeacherSubject)
                        .ThenInclude(ts => ts.Subject)
                    .Include(jg => jg.Date)
                    .Where(jg => jg.StudentClass.IdStudent == CurrentStudent.IdStudent &&
                                jg.Date.IdQuarter == SelectedQuarter.IdQuarter)
                    .ToList();

                foreach (var date in quarterDates)
                {
                    var dateWithGrades = new DateWithGrades
                    {
                        Date = date,
                        GradesForDate = new List<string>(),
                        FormattedDate = date.DateValue.ToString("dd.MM.yyyy")
                    };

                    foreach (var subject in Subjects)
                    {
                        var grade = grades
                            .FirstOrDefault(g => g.TeacherSubject.IdSubject == subject.IdSubject &&
                                               g.Date.IdDate == date.IdDate);

                        dateWithGrades.GradesForDate.Add(grade?.Grade.ToString() ?? "-");
                    }

                    Dates.Add(dateWithGrades);
                    _allDates.Add(dateWithGrades);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки оценок: {ex.Message}");
            }
        }

        private void CalculateAverages()
        {
            try
            {
                if (CurrentStudent == null || SelectedQuarter == null) return;

                var grades = _dbContext.JournalGrades
                    .Include(jg => jg.TeacherSubject)
                        .ThenInclude(ts => ts.Subject)
                    .Include(jg => jg.Date)
                    .Where(jg => jg.StudentClass.IdStudent == CurrentStudent.IdStudent &&
                                jg.Date.IdQuarter == SelectedQuarter.IdQuarter)
                    .ToList();

                Dates.Clear();
                _allDates.Clear();

                var averageRow = new DateWithGrades
                {
                    Date = new Date { DateValue = DateTime.Now },
                    GradesForDate = new List<string>(),
                    FormattedDate = "Итоговые оценки"
                };

                foreach (var subject in Subjects)
                {
                    var subjectGrades = grades
                        .Where(g => g.TeacherSubject.IdSubject == subject.IdSubject &&
                                   g.Grade.HasValue)
                        .Select(g => g.Grade.Value)
                        .ToList();

                    if (subjectGrades.Any())
                    {
                        var average = Math.Round(subjectGrades.Average(), 2);
                        averageRow.GradesForDate.Add(average.ToString("0.00"));
                    }
                    else
                    {
                        averageRow.GradesForDate.Add("-");
                    }
                }

                Dates.Add(averageRow);
                _allDates.Add(averageRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета средних оценок: {ex.Message}");
            }
        }

        private void ApplyDateFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchDate))
            {
                Dates.Clear();
                foreach (var date in _allDates)
                {
                    Dates.Add(date);
                }
            }
            else
            {
                var searchParts = SearchDate.Split('.');
                var normalizedSearch = string.Join(".", searchParts.Select(p => p.Trim()));

                var filteredDates = _allDates
                    .Where(d => d.FormattedDate.Contains(normalizedSearch))
                    .ToList();

                Dates.Clear();
                foreach (var date in filteredDates)
                {
                    Dates.Add(date);
                }
            }
        }

        public class DateWithGrades : BindableBase
        {
            public Date Date { get; set; }
            public List<string> GradesForDate { get; set; } = new List<string>();
            public string FormattedDate { get; set; }
        }

        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

            public void Execute(object parameter) => _execute(parameter);

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }
    }
}