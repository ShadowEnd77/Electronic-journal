using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using WpfApp1.shell.Model.Entities;

namespace WpfApp1.shell.Model
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<JournalGrade> JournalGrades { get; set; }
        public DbSet<JournalSubject> JournalSubjects { get; set; }
        public DbSet<Quarter> Quarters { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Subjekt> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }

        public SchoolDbContext(string connectionString) : base()
        {
            Database.EnsureCreated();
        }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=EJ;Username=postgres;Password=1111")
                              .EnableSensitiveDataLogging()
                              .EnableDetailedErrors();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка таблиц с русскими именами
            modelBuilder.Entity<Account>().ToTable("Аккаунты");
            modelBuilder.Entity<Teacher>().ToTable("Учителя");
            modelBuilder.Entity<Student>().ToTable("Ученики");
            modelBuilder.Entity<Class>().ToTable("Классы");
            modelBuilder.Entity<Subjekt>().ToTable("Предметы");
            modelBuilder.Entity<TeacherSubject>().ToTable("Учитель-предмет");
            modelBuilder.Entity<StudentClass>().ToTable("Ученик-класс");
            modelBuilder.Entity<Quarter>().ToTable("Четверть");
            modelBuilder.Entity<Date>().ToTable("Дата");
            modelBuilder.Entity<JournalSubject>().ToTable("ЖурналПредмет");
            modelBuilder.Entity<JournalGrade>().ToTable("ЖурналОценка");

            // Настройка столбцов и ключей для Account
            modelBuilder.Entity<Account>()
                .HasKey(a => a.IdAccount);
            modelBuilder.Entity<Account>()
                .Property(a => a.IdAccount)
                .HasColumnName("id_аккаунта");
            modelBuilder.Entity<Account>()
                .Property(a => a.Login)
                .HasColumnName("Логин");
            modelBuilder.Entity<Account>()
                .Property(a => a.Password)
                .HasColumnName("Пароль");

            // Настройка столбцов и ключей для Teacher
            modelBuilder.Entity<Teacher>()
                .HasKey(t => t.IdTeacher);
            modelBuilder.Entity<Teacher>()
                .Property(t => t.IdTeacher)
                .HasColumnName("id_учителя");
            modelBuilder.Entity<Teacher>()
                .Property(t => t.IdAccount)
                .HasColumnName("id_аккаунта");
            modelBuilder.Entity<Teacher>()
                .Property(t => t.LastName)
                .HasColumnName("Фамилия");
            modelBuilder.Entity<Teacher>()
                .Property(t => t.FirstName)
                .HasColumnName("Имя");
            modelBuilder.Entity<Teacher>()
                .Property(t => t.Patronymic)
                .HasColumnName("Отчество");
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Account)
                .WithOne(a => a.Teacher)
                .HasForeignKey<Teacher>(t => t.IdAccount);

            // Настройка столбцов и ключей для Student
            modelBuilder.Entity<Student>()
                .HasKey(s => s.IdStudent);
            modelBuilder.Entity<Student>()
                .Property(s => s.IdStudent)
                .HasColumnName("id_ученика");
            modelBuilder.Entity<Student>()
                .Property(s => s.IdAccount)
                .HasColumnName("id_аккаунта");
            modelBuilder.Entity<Student>()
                .Property(s => s.LastName)
                .HasColumnName("Фамилия");
            modelBuilder.Entity<Student>()
                .Property(s => s.FirstName)
                .HasColumnName("Имя");
            modelBuilder.Entity<Student>()
                .Property(s => s.Patronymic)
                .HasColumnName("Отчество");
            modelBuilder.Entity<Student>()
                .Property(s => s.BirthDate)
                .HasColumnName("Дата_Рождения");
            modelBuilder.Entity<Student>()
                .Property(s => s.Gender)
                .HasColumnName("Пол");
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Account)
                .WithOne(a => a.Student)
                .HasForeignKey<Student>(s => s.IdAccount);

            // Настройка столбцов и ключей для Class
            modelBuilder.Entity<Class>()
                .HasKey(c => c.IdClass);
            modelBuilder.Entity<Class>()
                .Property(c => c.IdClass)
                .HasColumnName("id_класса");
            modelBuilder.Entity<Class>()
                .Property(c => c.IdTeacher)
                .HasColumnName("id_учителя");
            modelBuilder.Entity<Class>()
                .Property(c => c.Letter)
                .HasColumnName("Литера");
            modelBuilder.Entity<Class>()
                .Property(c => c.Number)
                .HasColumnName("Цифра");
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Teacher)
                .WithOne(t => t.Class)
                .HasForeignKey<Class>(c => c.IdTeacher);

            // Настройка столбцов и ключей для StudentClass
            modelBuilder.Entity<StudentClass>()
                .HasKey(sc => sc.IdStudentClass);
            modelBuilder.Entity<StudentClass>()
                .Property(sc => sc.IdStudentClass)
                .HasColumnName("id_ученик-класс");
            modelBuilder.Entity<StudentClass>()
                .Property(sc => sc.IdStudent)
                .HasColumnName("id_ученика");
            modelBuilder.Entity<StudentClass>()
                .Property(sc => sc.IdClass)
                .HasColumnName("id_класса");
            modelBuilder.Entity<StudentClass>()
                .HasOne(sc => sc.Student)
                .WithOne(s => s.StudentClass)
                .HasForeignKey<StudentClass>(sc => sc.IdStudent);
            modelBuilder.Entity<StudentClass>()
                .HasOne(sc => sc.Class)
                .WithMany(c => c.StudentClasses)
                .HasForeignKey(sc => sc.IdClass);

            // Настройка столбцов и ключей для Subject
            modelBuilder.Entity<Subjekt>()
                .HasKey(s => s.IdSubject);
            modelBuilder.Entity<Subjekt>()
                .Property(s => s.IdSubject)
                .HasColumnName("id_предмета");
            modelBuilder.Entity<Subjekt>()
                .Property(s => s.Name)
                .HasColumnName("Предмет");

            // Настройка столбцов и ключей для TeacherSubject
            modelBuilder.Entity<TeacherSubject>()
                .HasKey(ts => ts.IdTeacherSubject); 
            modelBuilder.Entity<TeacherSubject>()
                .Property(ts => ts.IdTeacherSubject)
                .HasColumnName("id_учитель-предмет");
            modelBuilder.Entity<TeacherSubject>()
                .Property(ts => ts.IdTeacher)
                .HasColumnName("id_учителя");
            modelBuilder.Entity<TeacherSubject>()
                .Property(ts => ts.IdSubject)
                .HasColumnName("id_предмета");
            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(ts => ts.IdTeacher);
            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.IdSubject);

            // Настройка столбцов и ключей для Quarter
            modelBuilder.Entity<Quarter>()
                .HasKey(q => q.IdQuarter);
            modelBuilder.Entity<Quarter>()
                .Property(q => q.IdQuarter)
                .HasColumnName("id_четверти");
            modelBuilder.Entity<Quarter>()
                .Property(q => q.StartDate)
                .HasColumnName("Дата_нач");
            modelBuilder.Entity<Quarter>()
                .Property(q => q.EndDate)
                .HasColumnName("Дата_кон");

            // Настройка столбцов и ключей для Date
            modelBuilder.Entity<Date>()
                .HasKey(d => d.IdDate);
            modelBuilder.Entity<Date>()
                .Property(d => d.IdDate)
                .HasColumnName("id_Дата");
            modelBuilder.Entity<Date>()
                .Property(d => d.IdQuarter)
                .HasColumnName("id_четверти");
            modelBuilder.Entity<Date>()
                .Property(d => d.DateValue)
                .HasColumnName("Дата");
            modelBuilder.Entity<Date>()
                .HasOne(d => d.Quarter)
                .WithMany(q => q.Dates)
                .HasForeignKey(d => d.IdQuarter);

            // Настройка столбцов и ключей для JournalSubject
            modelBuilder.Entity<JournalSubject>()
                .HasKey(js => js.IdRDay);
            modelBuilder.Entity<JournalSubject>()
                .Property(js => js.IdRDay)
                .HasColumnName("id_РДень");
            modelBuilder.Entity<JournalSubject>()
                .Property(js => js.IdTeacherSubject)
                .HasColumnName("id_учитель-предмет");
            modelBuilder.Entity<JournalSubject>()
                .Property(js => js.IdDate)
                .HasColumnName("id_дата");
            modelBuilder.Entity<JournalSubject>()
                .Property(js => js.IdClass)
                .HasColumnName("id_класса");
            modelBuilder.Entity<JournalSubject>()
                .Property(js => js.IdSubject)
                .HasColumnName("id_предмета");
            modelBuilder.Entity<JournalSubject>()
                .HasOne(js => js.TeacherSubject)
                .WithMany(ts => ts.JournalSubjects)
                .HasForeignKey(js => js.IdTeacherSubject);
            modelBuilder.Entity<JournalSubject>()
                .HasOne(js => js.Date)
                .WithOne(d => d.JournalSubject)
                .HasForeignKey<JournalSubject>(js => js.IdDate);
            modelBuilder.Entity<JournalSubject>()
                .HasOne(js => js.Class)
                .WithMany(c => c.JournalSubjects)
                .HasForeignKey(js => js.IdClass);
            modelBuilder.Entity<JournalSubject>()
                .HasOne(js => js.Subject)
                .WithMany(s => s.JournalSubjects)
                .HasForeignKey(js => js.IdSubject);

            // Настройка столбцов и ключей для JournalGrade
            modelBuilder.Entity<JournalGrade>()
                .HasKey(jg => jg.IdRecord);
            modelBuilder.Entity<JournalGrade>()
                .Property(jg => jg.IdRecord)
                .HasColumnName("id_записи");
            modelBuilder.Entity<JournalGrade>()
                .Property(jg => jg.IdStudentClass)
                .HasColumnName("id_ученик-класс");
            modelBuilder.Entity<JournalGrade>()
                .Property(jg => jg.IdTeacherSubject)
                .HasColumnName("id_учитель-предмет");
            modelBuilder.Entity<JournalGrade>()
                .Property(jg => jg.IdDate)
                .HasColumnName("id_дата");
            modelBuilder.Entity<JournalGrade>()
                .Property(jg => jg.Grade)
                .HasColumnName("Оценка");
            modelBuilder.Entity<JournalGrade>()
                .HasOne(jg => jg.StudentClass)
                .WithMany(sc => sc.JournalGrades)
                .HasForeignKey(jg => jg.IdStudentClass);
            modelBuilder.Entity<JournalGrade>()
                .HasOne(jg => jg.TeacherSubject)
                .WithMany(ts => ts.JournalGrades)
                .HasForeignKey(jg => jg.IdTeacherSubject);
            modelBuilder.Entity<JournalGrade>()
                .HasOne(jg => jg.Date)
                .WithMany(d => d.JournalGrades)
                .HasForeignKey(jg => jg.IdDate);
        }
    }
}