using AspNetCoreMVC.Data;
using AspNetCoreMVC.Dtos;
using AspNetCoreMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVC.Attributes;

public class UniqueCourseAttribute : ValidationAttribute
{
    private readonly IUniversityContext _dbContext;

    public UniqueCourseAttribute()
    {
        _dbContext = (IUniversityContext)Activator.CreateInstance(typeof(MVCUniversityContext))!;
    }

    public string GetErrorMessage(string name) =>
        $"A Course with the name \"{name}\" already exists in this department.";

    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        var course = (CourseDto)validationContext.ObjectInstance;
        var courseName = (string)value!;

        if (_dbContext.Courses.Any(c => c.Name == courseName 
            && c.DepartmentId == course.DepartmentId))
        {
            return new ValidationResult(GetErrorMessage(courseName));
        }

        return ValidationResult.Success;
    }
}
