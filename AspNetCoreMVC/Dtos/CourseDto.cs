﻿using AspNetCoreMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVC.Dtos
{
    public class CourseDto
    {
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Course name must be between 5 and 50 characters.")]
        public string Name { get; set; }

        [Required]
        [Range(100, 200)]
        public double FullMark { get; set; }
        [Required]
        [Range(30, 200)]
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
