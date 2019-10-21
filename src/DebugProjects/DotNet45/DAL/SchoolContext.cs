using ContosoUniversity.Models;
using HodStudio.EntityFrameworkDiffLog.Repository;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ContosoUniversity.DAL
{
    public class SchoolContext : LoggingDbContext
    {
        public SchoolContext()
        {
            LogEntriesTableName = "LogEntries";
            LogEntriesSchemaName = "dbo";
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")
                    .MapRightKey("InstructorID")
                    .ToTable("CourseInstructor"));

            modelBuilder.Entity<Department>().MapToStoredProcedures();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            // your code here

            UserId = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            return base.SaveChanges();
        }
    }
}