using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp1.shell.Model;
using WpfApp1.shell.Model.Entities;

namespace WpfApp1.shell.ViewModel
{
    public class GradesPageViewModel : BindableBase
    {
        private readonly SchoolDbContext _dbContext;
        private ObservableCollection<StudentWithGrades> _students;
        private ObservableCollection<Subjekt> _subjects;
        private ObservableCollection<Date> _dates;
        private Subjekt _selectedSubject;

        public ObservableCollection<StudentWithGrades> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
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

        public Subjekt SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                SetProperty(ref _selectedSubject, value);
                LoadGradesData();
            }
        }

        public DelegateCommand RefreshCommand { get; private set; }

        public GradesPageViewModel(SchoolDbContext dbContext)
        {
            _dbContext = dbContext;
            Students = new ObservableCollection<StudentWithGrades>();
            Subjects = new ObservableCollection<Subjekt>();
            Dates = new ObservableCollection<Date>();
            RefreshCommand = new DelegateCommand(LoadGradesData);
            LoadInitialData();
        }

        private void LoadInitialData()
        {
            // Загружаем список предметов
            Subjects = new ObservableCollection<Subjekt>(_dbContext.Subjects.ToList());

            // Выбираем первый предмет по умолчанию
            if (Subjects.Any())
            {
                SelectedSubject = Subjects.First();
            }
        }

        private void LoadGradesData()
        {
            if (SelectedSubject == null) return;

            // Загружаем даты для выбранного предмета
            Dates = new ObservableCollection<Date>(
                _dbContext.Dates
                    .Include(d => d.JournalSubject)
                    .Where(d => d.JournalSubject != null &&
                               d.JournalSubject.Subject.IdSubject == SelectedSubject.IdSubject)
                    .OrderBy(d => d.DateValue)
                    .ToList()
            );

            // Загружаем студентов с их оценками
            Students = new ObservableCollection<StudentWithGrades>(
                _dbContext.Students
                    .Include(s => s.StudentClass)
                        .ThenInclude(sc => sc.JournalGrades)
                            .ThenInclude(jg => jg.Date)
                    .Include(s => s.StudentClass)
                        .ThenInclude(sc => sc.JournalGrades)
                            .ThenInclude(jg => jg.TeacherSubject)
                                .ThenInclude(ts => ts.Subject)
                    .Select(s => new StudentWithGrades
                    {
                        Student = s,
                        Grades = s.StudentClass.JournalGrades
                            .Where(jg => jg.TeacherSubject.Subject.IdSubject == SelectedSubject.IdSubject)
                            .OrderBy(jg => jg.Date.DateValue)
                            .ToList()
                    })
                    .Where(s => s.Grades.Any()) // Только студенты с оценками
                    .ToList()
            );
        }
    }

    public class StudentWithGrades
    {
        public Student Student { get; set; }
        public List<JournalGrade> Grades { get; set; }

        public string FullName => $"{Student.LastName} {Student.FirstName} {Student.Patronymic}";
    }
}