using AspNetCoreMVC.Data;
using AspNetCoreMVC.Dtos;
using AspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCoreMVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly MVCUniversityContext _dbContext;
        private readonly IMemoryCache _cache;

        public CourseController(IUniversityContext dbContext
            , IMemoryCache cache)
        {
            _dbContext = dbContext as MVCUniversityContext
                ?? 
                throw new ArgumentNullException(nameof(dbContext));
            _cache = cache;
        }

        public IActionResult Index()
        {
            var courses = _dbContext.Courses
                .Include(c => c.Department)
                .ToList()
                .Select(c => new CourseDto(c));

            return View(courses);
        }

        public IActionResult Create()
        {
            LoadLists();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadLists();

                return View(dto);
            }

            var course = new Course(dto);

            await _dbContext.AddAsync(course);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Results()
        {
            var results = _dbContext.CourseResults
                .Include(c => c.Course)
                .Include(c => c.Trainee)
                .Select(c => new CourseResultViewModel(c))
                .ToList();

            return View(results);
        }

        /// <summary>
        /// Loads necessary lists for the view to show from Database.
        /// </summary>
        private void LoadLists()
        {
            ViewBag.Departments = _cache.Get<List<Department>>(GlobalConfig.DepartmentListCacheKey);

            if (ViewBag.Departments is null)
            {
                ViewBag.Departments = _dbContext.Departments.ToList();
                _cache.Set(GlobalConfig.DepartmentListCacheKey,
                    (List<Department>)ViewBag.Departments, TimeSpan.FromDays(1));
            }
        }
    }
}
