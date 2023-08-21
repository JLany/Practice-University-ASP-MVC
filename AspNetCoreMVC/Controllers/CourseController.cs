using AspNetCoreMVC.Data;
using AspNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly MVCUniversityContext _dbContext;

        public CourseController(IUniversityContext dbContext)
        {
            _dbContext = dbContext as MVCUniversityContext
                ?? 
                throw new ArgumentNullException(nameof(dbContext));
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
    }
}
