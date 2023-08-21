using AspNetCoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVC.Data
{
    public interface IUniversityContext 
    {
        DbSet<CourseResult> CourseResults { get; set; }
        DbSet<Course> Courses { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Instructor> Instructors { get; set; }
        DbSet<Trainee> Trainees { get; set; }
    }
}