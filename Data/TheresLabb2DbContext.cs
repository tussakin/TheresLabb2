using Labb2Theres.Models;
using Microsoft.EntityFrameworkCore;

namespace TheresLabb2.Data
{
    public class TheresLabb2DbContext : DbContext
    {
        public TheresLabb2DbContext(DbContextOptions<TheresLabb2DbContext> options)
           : base(options)
        { 
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentYear> StudentYears { get; set; }
        public DbSet<Course> Courses { get; set; }




        protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses)
             .UsingEntity("CourseStudent");

            modelBuilder.Entity<Course>()
              .HasMany(c => c.Teachers) 
              .WithMany(t => t.Courses)
              .UsingEntity("CourseTeacher");


            modelBuilder.Entity<StudentYear>()
                .HasMany(s => s.Students)
                .WithOne(sy => sy.StudentYear)
                .HasForeignKey(s => s.FkStudentYearId);



        }

        public void Seed()
        {

            if (!Courses.Any())
            {
                Courses.AddRange(
                    new Course { CourseTitle = "Programming 1", CourseDescription = "Intro to programming. C#" },
                    new Course { CourseTitle = "Arts", CourseDescription = "Tap into your creative flows and choose your medium" },
                    new Course { CourseTitle = "Design 101", CourseDescription = "Introduction to design thinking" },
                    new Course { CourseTitle = "3D modelling", CourseDescription = "Learn how to create 3D designs" },
                    new Course { CourseTitle = "Programming 2", CourseDescription = "Continuation of programming. C#" }
                );
                SaveChanges();
            }


            if (!Teachers.Any())
            {
                Teachers.AddRange(
                    new Teacher { TeacherName = "Sally King" },
                    new Teacher { TeacherName = "Kelly Kox" },
                    new Teacher { TeacherName = "Maxwell Simons" },
                    new Teacher { TeacherName = "Thomas Philips" },
                    new Teacher { TeacherName = "Alexandra Lillhammer" },
                    new Teacher { TeacherName = "Reidar" },
                    new Teacher { TeacherName = "Tobias" }
                );
                SaveChanges();
            }




            if (!StudentYears.Any())
            {
                StudentYears.AddRange(
                    new StudentYear { StudentYearName = "1a" },
                    new StudentYear { StudentYearName = "1b" },
                    new StudentYear { StudentYearName = "1c" },
                    new StudentYear { StudentYearName = "2a" },
                    new StudentYear { StudentYearName = "2b" },
                    new StudentYear { StudentYearName = "3a" },
                    new StudentYear { StudentYearName = "4a" }
                );
                SaveChanges();
            }


            if (!Students.Any())
            {
                Students.AddRange(
                    new Student { StudentName = "Kassandra Lardy", FkStudentYearId = 1 },
                    new Student { StudentName = "Ebba Samson", FkStudentYearId = 2 },
                    new Student { StudentName = "Millie Amar", FkStudentYearId = 4 },
                    new Student { StudentName = "Phylis Well", FkStudentYearId = 1 },
                    new Student { StudentName = "Veronica Alterdome", FkStudentYearId = 5 },
                    new Student { StudentName = "Hannes Sjölund", FkStudentYearId = 3 },
                    new Student { StudentName = "Lilly Sandstorm", FkStudentYearId = 1 },
                    new Student { StudentName = "Kassidy Moore", FkStudentYearId = 2 },
                    new Student { StudentName = "Adalfo Philips", FkStudentYearId = 4 },
                    new Student { StudentName = "Liam Saroha", FkStudentYearId = 1 },
                    new Student { StudentName = "Harald Olver", FkStudentYearId = 5 },
                    new Student { StudentName = "Leland Atari", FkStudentYearId = 3 }
                );
                SaveChanges();

            }
        }
    }
}
