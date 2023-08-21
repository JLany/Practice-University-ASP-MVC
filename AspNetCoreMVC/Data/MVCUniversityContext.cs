using AspNetCoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVC.Data;

public class MVCUniversityContext : DbContext, IUniversityContext
{
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Trainee> Trainees { get; set; }
    public DbSet<CourseResult> CourseResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=Kilany; Database=MVCUniversityDemoDB; Trusted_Connection=True; Encrypt=False");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseResult>()
            .HasKey("CourseId", "TraineeId");

        base.OnModelCreating(modelBuilder);
    }
}
