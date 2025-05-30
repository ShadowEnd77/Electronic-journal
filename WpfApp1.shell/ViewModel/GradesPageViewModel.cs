// GradesPageViewModel.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp1.shell.Model;
using WpfApp1.shell.Model.Entities;

namespace WpfApp1.shell.ViewModel
{
    public class GradeEntry : BindableBase
    {
        private Date _date;
        private string _grade;

        public Date Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public string Grade
        {
            get => _grade;
            set => SetProperty(ref _grade, value);
        }
    }

    public class StudentGradesRow : BindableBase
    {
        private string _studentName;
        private Student _student;
        private ObservableCollection<GradeEntry> _grades = new();
        private string _averageGrade;

        public string StudentName
        {
            get => _studentName;
            set => SetProperty(ref _studentName, value);
        }

        public Student Student
        {
            get => _student;
            set => SetProperty(ref _student, value);
        }

        public ObservableCollection<GradeEntry> Grades
        {
            get => _grades;
            set => SetProperty(ref _grades, value);
        }

        public string AverageGrade
        {
            get => _averageGrade;
            set => SetProperty(ref _averageGrade, value);
        }
    }

    public class GradesPageViewModel : BindableBase
    {
        private readonly SchoolDbContext _dbContext;

        private ObservableCollection<Student> _students;
        private ObservableCollection<Quarter> _quarters;
        private ObservableCollection<Subjekt> _subjects;
        private ObservableCollection<Date> _dates;
        private ObservableCollection<StudentGradesRow> _studentGradesRows;
        private bool _isAverageMode;

        private Student _selectedStudent;
        private Quarter _selectedQuarter;
        private Subjekt _selectedSubject;

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        public ObservableCollection<Quarter> Quarters
        {
            get => _quarters;
            set => SetProperty(ref _quarters, value);
        }

        public ObservableCollection<Subjekt> Subjects
        {
            get => _subjects;
            set => SetProperty(ref _subjects, value);
        }

        public ObservableCollection<Date> Dates
        {
            get => _dates;
            set => SetProperty(ref _dates, value);
        }

        public ObservableCollection<StudentGradesRow> StudentGradesRows
        {
            get => _studentGradesRows;
            set => SetProperty(ref _studentGradesRows, value);
        }

        public bool IsAverageMode
        {
            get => _isAverageMode;
            set => SetProperty(ref _isAverageMode, value);
        }

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set => SetProperty(ref _selectedStudent, value);
        }

        public Quarter SelectedQuarter
        {
            get => _selectedQuarter;
            set
            {
                if (SetProperty(ref _selectedQuarter, value))
                {
                    LoadDates();
                    LoadGrades();
                }
            }
        }

        public Subjekt SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                if (SetProperty(ref _selectedSubject, value))
                {
                    LoadGrades();
                }
            }
        }

        public ICommand RefreshCommand { get; private set; }
        public ICommand SaveAllCommand { get; private set; }
        public ICommand CalculateAverageCommand { get; private set; }

        public GradesPageViewModel()
        {
            _dbContext = new SchoolDbContext();
            InitializeCollections();
            SetupCommands();
            LoadInitialData();
        }

        private void InitializeCollections()
        {
            Students = new ObservableCollection<Student>();
            Quarters = new ObservableCollection<Quarter>();
            Subjects = new ObservableCollection<Subjekt>();
            Dates = new ObservableCollection<Date>();
            StudentGradesRows = new ObservableCollection<StudentGradesRow>();
        }

        private void SetupCommands()
        {
            RefreshCommand = new RelayCommand(_ =>
            {
                LoadInitialData();
                LoadDates();
                LoadGrades();
            });

            SaveAllCommand = new RelayCommand(_ => SaveAllGrades());
            CalculateAverageCommand = new RelayCommand(_ => ToggleAverageMode());
        }

        private void ToggleAverageMode()
        {
            IsAverageMode = !IsAverageMode;
            LoadGrades();
        }

        private void LoadInitialData()
        {
            try
            {
                Students = new ObservableCollection<Student>(_dbContext.Students
                    .Include(s => s.StudentClass)
                    .ToList());

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

                SelectedQuarter = Quarters.FirstOrDefault();
                SelectedSubject = Subjects.FirstOrDefault();
                SelectedStudent = Students.FirstOrDefault();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void LoadDates()
        {
            if (SelectedQuarter == null) return;

            Dates = new ObservableCollection<Date>(_dbContext.Dates
                .Where(d => d.IdQuarter == SelectedQuarter.IdQuarter)
                .OrderBy(d => d.DateValue)
                .ToList());
        }

        private void LoadGrades()
        {
            if (SelectedSubject == null || SelectedQuarter == null) return;

            StudentGradesRows.Clear();

            if (IsAverageMode)
            {
                // Режим отображения средних оценок
                foreach (var student in Students)
                {
                    var grades = _dbContext.JournalGrades
                        .Where(g =>
                            g.StudentClass.IdStudent == student.IdStudent &&
                            g.TeacherSubject.Subject.IdSubject == SelectedSubject.IdSubject &&
                            g.Date.IdQuarter == SelectedQuarter.IdQuarter)
                        .Select(g => (double?)g.Grade) // Исправление здесь
                        .ToList();

                    string averageDisplay = "-";

                    if (grades.Any() && grades.All(g => g.HasValue))
                    {
                        double average = grades.Average().Value; // Явное преобразование
                        averageDisplay = average.ToString("F2");
                    }

                    var row = new StudentGradesRow
                    {
                        StudentName = $"{student.LastName} {student.FirstName} {student.Patronymic}",
                        Student = student,
                        AverageGrade = averageDisplay
                    };

                    StudentGradesRows.Add(row);
                }
            }
            else
            {
                // Обычный режим (оценки по датам)
                foreach (var student in Students)
                {
                    var row = new StudentGradesRow
                    {
                        StudentName = $"{student.LastName} {student.FirstName} {student.Patronymic}",
                        Student = student,
                        Grades = new ObservableCollection<GradeEntry>()
                    };

                    foreach (var date in Dates)
                    {
                        var grade = _dbContext.JournalGrades
                            .Include(jg => jg.TeacherSubject)
                            .ThenInclude(ts => ts.Subject)
                            .FirstOrDefault(g =>
                                g.StudentClass.IdStudent == student.IdStudent &&
                                g.Date.IdDate == date.IdDate &&
                                g.TeacherSubject.Subject.IdSubject == SelectedSubject.IdSubject);

                        row.Grades.Add(new GradeEntry
                        {
                            Date = date,
                            Grade = grade?.Grade.ToString() ?? "-"
                        });
                    }

                    StudentGradesRows.Add(row);
                }
            }
        }

        private void SaveAllGrades()
        {
            try
            {
                foreach (var row in StudentGradesRows)
                {
                    foreach (var gradeEntry in row.Grades)
                    {
                        UpdateGrade(row, gradeEntry);
                    }
                }
                MessageBox.Show("Все оценки успешно сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        public void UpdateGrade(StudentGradesRow row, GradeEntry gradeEntry)
        {
            try
            {
                string newGrade = gradeEntry.Grade?.Trim() ?? string.Empty;
                int? parsedGrade = null;

                if (!string.IsNullOrEmpty(newGrade) && newGrade != "-")
                {
                    if (!int.TryParse(newGrade, out int tempGrade) || tempGrade < 1 || tempGrade > 5)
                    {
                        MessageBox.Show("Оценка должна быть числом от 1 до 5");
                        gradeEntry.Grade = "-";
                        return;
                    }
                    parsedGrade = tempGrade;
                }

                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var student = row.Student ?? throw new ArgumentNullException("Студент не найден");
                        var date = gradeEntry.Date ?? throw new ArgumentNullException("Дата не найдена");

                        var dbEntry = _dbContext.JournalGrades
                            .Include(jg => jg.TeacherSubject)
                                .ThenInclude(ts => ts.Subject)
                            .Include(jg => jg.StudentClass)
                            .Include(jg => jg.Date)
                            .FirstOrDefault(g =>
                                g.StudentClass.IdStudent == student.IdStudent &&
                                g.Date.IdDate == date.IdDate &&
                                g.TeacherSubject.Subject.IdSubject == SelectedSubject.IdSubject);

                        if (dbEntry == null && parsedGrade.HasValue)
                        {
                            var teacherSubject = _dbContext.TeacherSubjects
                                .FirstOrDefault(ts => ts.Subject.IdSubject == SelectedSubject.IdSubject);

                            var studentClass = _dbContext.StudentClasses
                                .FirstOrDefault(sc => sc.IdStudent == student.IdStudent);

                            dbEntry = new JournalGrade
                            {
                                IdStudentClass = studentClass.IdStudentClass,
                                IdTeacherSubject = teacherSubject.IdTeacherSubject,
                                IdDate = date.IdDate,
                                Grade = parsedGrade.Value
                            };
                            _dbContext.JournalGrades.Add(dbEntry);
                        }
                        else if (dbEntry != null)
                        {
                            if (parsedGrade.HasValue)
                            {
                                dbEntry.Grade = parsedGrade.Value;
                                _dbContext.Entry(dbEntry).State = EntityState.Modified;
                            }
                            else
                            {
                                _dbContext.JournalGrades.Remove(dbEntry);
                            }
                        }

                        _dbContext.SaveChanges();
                        transaction.Commit();

                        gradeEntry.Grade = parsedGrade?.ToString() ?? "-";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        string errorMessage = ex.InnerException?.Message ?? ex.Message;
                        MessageBox.Show($"Ошибка сохранения: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
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

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // Если параметр "inverse", инвертируем значение
                if (parameter is string param && param.ToLower() == "inverse")
                {
                    return boolValue ? Visibility.Collapsed : Visibility.Visible;
                }
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}