namespace AspNetCoreMVC.Models
{
    public class Trainee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Level { get; set; }
        public string ImageUri { get; set; } = null!;

        public virtual ICollection<CourseResult> CourseResults { get; set; } = null!;

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
    }
}