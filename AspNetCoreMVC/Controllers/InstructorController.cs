using AspNetCoreMVC.Data;
using AspNetCoreMVC.Dtos;
using AspNetCoreMVC.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics.Metrics;
using System.Net;

namespace AspNetCoreMVC.Controllers
{
    public class InstructorController : Controller
    {
        private readonly MVCUniversityContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InstructorController(IUniversityContext dbContext
            , IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext as MVCUniversityContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var instructors = _dbContext.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .ToList();
            return View(instructors);
        }

        public IActionResult Details(int id)
        {
            var instructor = _dbContext.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefault(i => i.Id == id);
            return View(instructor);
        }

        public IActionResult Create()
        {
            ViewBag.Courses = _dbContext.Courses.ToList();
            ViewBag.Departments = _dbContext.Departments.ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstructorDto dto)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Courses = _dbContext.Courses.ToList();
            //    ViewBag.Departments = _dbContext.Departments.ToList();

            //    var message = string.Join(" | ", ModelState.Values
            //        .SelectMany(v => v.Errors)
            //        .Select(e => e.ErrorMessage));

            //    ViewBag.Error = message;
            //    return View();
            //}

            string imageUri;

            try
            {
                imageUri = await SaveImageToStorageAsync(dto.Image);

                ViewBag.FileStatus = "File uploaded successfully.";
            }

            catch (Exception ex)
            {
                ViewBag.FileStatus = "Error while uploading file";

                return View(dto);
            }            

            var instructor = new Instructor(dto)
            {
                ImageUri = imageUri
            };

            await _dbContext.AddAsync(instructor);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Instructor? deleteCandidate = _dbContext.Instructors
                .FirstOrDefault(i => i.Id == id);

            if (deleteCandidate is null)
            {
                return BadRequest(new {Message = $"No instructor exists with the Id = {id}"});
            }

            _dbContext.Instructors.Remove(deleteCandidate);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Instructor? editCandidate = _dbContext.Instructors
                .FirstOrDefault(i => i.Id == id);

            if (editCandidate is null)
            {
                return BadRequest(new { Message = $"No instructor exists with the Id = {id}" });
            }

            ViewBag.Courses = _dbContext.Courses.ToList();
            ViewBag.Departments = _dbContext.Departments.ToList();

            return View(new InstructorDto(editCandidate));
        }

        [HttpPost]
        public async Task<IActionResult> Update(InstructorDto dto)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Courses = _dbContext.Courses.ToList();
            //    ViewBag.Departments = _dbContext.Departments.ToList();

            //    var message = string.Join(" | ", ModelState.Values
            //        .SelectMany(v => v.Errors)
            //        .Select(e => e.ErrorMessage));

            //    ViewBag.Error = message;

            //    return RedirectToAction("Edit");
            //}

            Instructor? updateCandidate = await _dbContext.Instructors
                .FirstOrDefaultAsync(i => i.Id == dto.Id);

            if (updateCandidate is null)
            {
                return BadRequest(new { Message = $"No instructor exists with the Id = {dto.Id}" });
            }

            updateCandidate.Name = dto.Name;
            updateCandidate.Address = dto.Address;
            updateCandidate.Salary = dto.Salary;
            updateCandidate.CourseId = dto.CourseId;
            updateCandidate.DepartmentId = dto.DepartmentId;

            string imageUri = "";
            if (dto.Image is not null)
            {
                imageUri = await SaveImageToStorageAsync(dto.Image);
            }

            if (!string.IsNullOrWhiteSpace(imageUri))
            {
                updateCandidate.ImageUri = imageUri;
            }

            _dbContext.Update(updateCandidate);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Save image to wwwroot path.
        /// </summary>
        /// <param name="image"></param>
        /// <returns>The relative path to the saved image in wwwroot.</returns>
        private async Task<string> SaveImageToStorageAsync(IFormFile image)
        {
            var imageUri = "";

            if (image is not null)
            {
                imageUri = Path.Combine(
                    "/uploads/images",
                    Path.GetFileName(image.FileName));

                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imageUri.Trim('/'));

                using FileStream stream = System.IO.File.Create(fullPath);
                await image.CopyToAsync(stream);
            }

            return imageUri;
        }
    }
}
