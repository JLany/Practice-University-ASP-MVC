using AspNetCoreMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVC.Dtos
{
    public class InstructorDto
    {
        public int Id { get; init; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        public IFormFile Image { get; set; }

        public InstructorDto()
        {
            
        }

        public InstructorDto(Instructor instructor)
        {
            Id = instructor.Id;
            Name = instructor.Name;
            Address = instructor.Address;
            Salary = instructor.Salary;
            CourseId = instructor.CourseId;
            DepartmentId = instructor.DepartmentId;
        }
    }
}
