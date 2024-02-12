using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Data
{
    public class AWDbContext : DbContext
    {
        public AWDbContext(DbContextOptions<AWDbContext> options) : base(options)
        { }
        public DbSet<Student> Student { get; set; } 
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseModule> CourseModule { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<TeacherModule> TeacherModules { get; set; }
        public DbSet<Teacher> Teacher { get; set; }

    }
}
