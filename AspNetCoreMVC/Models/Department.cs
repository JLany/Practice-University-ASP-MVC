namespace AspNetCoreMVC.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Manager { get; set; } = null!;

        public virtual ICollection<Instructor> Instructors { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; } = null!;
        public virtual ICollection<Trainee> Trainees { get; set; } = null!;
    }
}
