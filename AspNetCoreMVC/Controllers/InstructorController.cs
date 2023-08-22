using AspNetCoreMVC.Data;
using AspNetCoreMVC.Dtos;
using AspNetCoreMVC.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.Metrics;
using System.Net;

namespace AspNetCoreMVC.Controllers
{
    public class InstructorController : Controller
    {
        private const string CoursesKey = "courseList";

        private readonly MVCUniversityContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMemoryCache _cache;

        public InstructorController(IUniversityContext dbContext
            , IWebHostEnvironment webHostEnvironment
            , IMemoryCache cache)
        {
            _dbContext = dbContext as MVCUniversityContext;
            _webHostEnvironment = webHostEnvironment;
            _cache = cache;
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
            LoadLists();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstructorDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadLists();

                return View(dto);
            }

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

            LoadLists();

            return View(new InstructorDto(editCandidate));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InstructorDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadLists();

                return View(dto);
            }

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
        /// Loads necessary lists for the view to show from Database.
        /// </summary>
        private void LoadLists()
        {
            ViewBag.Courses = _cache.Get<List<Course>>(CoursesKey);

            if (ViewBag.Courses is null)
            {
                ViewBag.Courses = _dbContext.Courses.ToList();
                _cache.Set(CoursesKey, 
                    (List<Course>)ViewBag.Courses, TimeSpan.FromDays(1));
            }

            ViewBag.Departments = _cache.Get<List<Department>>(GlobalConfig.DepartmentListCacheKey);

            if (ViewBag.Departments is null)
            {
                ViewBag.Departments = _dbContext.Departments.ToList();
                _cache.Set(GlobalConfig.DepartmentListCacheKey, 
                    (List<Department>)ViewBag.Departemnts, TimeSpan.FromDays(1));
            }
        }

        /// <summary>
        /// Save image to wwwroot path.
        /// </summary>
        /// <param name="image"></param>
        /// <returns>The relative path to the saved image in wwwroot.</returns>
        private async Task<string> SaveImageToStorageAsync(IFormFile? image)
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
