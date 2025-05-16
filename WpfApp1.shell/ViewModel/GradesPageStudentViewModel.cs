using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Subjects;
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
        private Quarter _selectedQuarter;
        private Student _currentStudent;
        private readonly int _accountId;

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

        public ICommand RefreshCommand { get; }

        public GradesPageStudentViewModel(int accountId)
        {
            _accountId = accountId;
            _dbContext = new SchoolDbContext();
            Subjects = new ObservableCollection<Subjekt>();
            Quarters = new ObservableCollection<Quarter>();
            Dates = new ObservableCollection<DateWithGrades>();
            RefreshCommand = new RelayCommand(_ => LoadGradesData());
            LoadInitialData();

        }

        private void LoadInitialData()
        {
            try
            {
                // Загрузка текущего ученика
                CurrentStudent = _dbContext.Students
                    .Include(s => s.StudentClass)
                    .FirstOrDefault(s => s.Account.IdAccount == _accountId);

                // Загрузка четвертей
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

                // Загрузка всех предметов
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

                // Очищаем предыдущие данные
                Dates.Clear();

                // Загрузка дат для выбранной четверти
                var quarterDates = _dbContext.Dates
                    .Where(d => d.IdQuarter == SelectedQuarter.IdQuarter)
                    .OrderBy(d => d.DateValue)
                    .ToList();

                // Загрузка всех оценок ученика за четверть
                var grades = _dbContext.JournalGrades
                    .Include(jg => jg.TeacherSubject)
                        .ThenInclude(ts => ts.Subject)
                    .Include(jg => jg.Date)
                    .Where(jg => jg.StudentClass.IdStudent == CurrentStudent.IdStudent &&
                                jg.Date.IdQuarter == SelectedQuarter.IdQuarter)
                    .ToList();

                // Формируем матрицу оценок
                foreach (var date in quarterDates)
                {
                    var dateWithGrades = new DateWithGrades
                    {
                        Date = date,
                        GradesForDate = new List<string>()
                    };

                    foreach (var subject in Subjects)
                    {
                        var grade = grades
                            .FirstOrDefault(g => g.TeacherSubject.IdSubject == subject.IdSubject &&
                                               g.Date.IdDate == date.IdDate);

                        dateWithGrades.GradesForDate.Add(grade?.Grade.ToString() ?? "-");
                    }

                    Dates.Add(dateWithGrades);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки оценок: {ex.Message}");
            }
        }

        public class DateWithGrades : BindableBase
        {
            public Date Date { get; set; }
            public List<string> GradesForDate { get; set; } = new List<string>();
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