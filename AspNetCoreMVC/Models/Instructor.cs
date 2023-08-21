using AspNetCoreMVC.Dtos;

namespace AspNetCoreMVC.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Salary { get; set; }
        public string ImageUri { get; set; } = null!;

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public Instructor()
        {
            
        }

        public Instructor(InstructorDto dto)
        {
            Name = dto.Name;
            Address = dto.Address;
            Salary = dto.Salary;
            CourseId = dto.CourseId;
            DepartmentId = dto.DepartmentId;
        }
    }
}
