namespace AspNetCoreMVC.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double FullMark { get; set; }
        public double SuccessMark { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; } = null!;
        public virtual ICollection<CourseResult> CourseResults { get; set; } = null!;

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
    }
}