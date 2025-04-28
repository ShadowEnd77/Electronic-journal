using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Subjects;
using WpfApp1.shell.Model;
using WpfApp1.shell.Model.Entities;

namespace WpfApp1.shell.ViewModel
{
    public class GradesPageViewModel : BindableBase
    {
        private readonly SchoolDbContext _dbContext;
        private ObservableCollection<JournalGrade> _grades;
        private ObservableCollection<Class> _classes;
        private ObservableCollection<Subjekt> _subjects;
        private Class _selectedClass;
        private Subjekt _selectedSubject;

        public ObservableCollection<JournalGrade> Grades
        {
            get => _grades;
            set => SetProperty(ref _grades, value);
        }

        public ObservableCollection<Class> Classes
        {
            get => _classes;
            set => SetProperty(ref _classes, value);
        }

        public ObservableCollection<Subjekt> Subjects
        {
            get => _subjects;
            set => SetProperty(ref _subjects, value);
        }

        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                SetProperty(ref _selectedClass, value);
                LoadGrades();
            }
        }

        public Subjekt SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                SetProperty(ref _selectedSubject, value);
                LoadGrades();
            }
        }

        public DelegateCommand RefreshCommand { get; private set; }

        public GradesPageViewModel(SchoolDbContext dbContext)
        {
            _dbContext = dbContext;
            Grades = new ObservableCollection<JournalGrade>();
            Classes = new ObservableCollection<Class>();
            Subjects = new ObservableCollection<Subjekt>();
            RefreshCommand = new DelegateCommand(LoadGrades);
            LoadInitialData();
        }

        private void LoadInitialData()
        {
            Classes = new ObservableCollection<Class>(_dbContext.Classes.ToList());
            Subjects = new ObservableCollection<Subjekt>(_dbContext.Subjects.ToList());
            LoadGrades();
        }

        private void LoadGrades()
        {
            var query = _dbContext.JournalGrades
                .Include(jg => jg.StudentClass)
                .ThenInclude(sc => sc.Student)
                .Include(jg => jg.TeacherSubject)
                .ThenInclude(ts => ts.Subject)
                .Include(jg => jg.Date)
                .AsQueryable();

            if (SelectedClass != null)
            {
                query = query.Where(jg => jg.StudentClass.IdClass == SelectedClass.IdClass);
            }

            if (SelectedSubject != null)
            {
                query = query.Where(jg => jg.TeacherSubject.IdSubject == SelectedSubject.IdSubject);
            }

            Grades = new ObservableCollection<JournalGrade>(query.ToList());
        }
    }
}