using AspNetCoreMVC.Attributes;
using AspNetCoreMVC.Data;
using AspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVC.Dtos
{
    public class CourseDto
    {
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Course name must be between 5 and 50 characters.")]
        [UniqueCourse]
        [Remote(action: "VerifyName", controller: "Course", AdditionalFields = nameof(DepartmentId))]
        public string Name { get; set; }

        [Required]
        [Remote(action: "VerifyFullMark", controller: "Course")]
        public double FullMark { get; set; }

        [Required(ErrorMessage = "You must enter a Success Mark for the course.")]
        [Remote(action: "VerifySuccessMark", controller: "Course", AdditionalFields = nameof(FullMark))]
        public double SuccessMark { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public CourseDto() { }

        public CourseDto(Course course)
        {
            Name = course.Name;
            FullMark = course.FullMark;
            SuccessMark = course.SuccessMark;
            DepartmentId = course.DepartmentId;
            DepartmentName = course.Department.Name;
        }
    }
}
