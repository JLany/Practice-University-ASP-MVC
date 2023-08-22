using AspNetCoreMVC.Attributes;
using AspNetCoreMVC.Data;
using AspNetCoreMVC.Models;

namespace AspNetCoreMVC
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services
				.AddSingleton<ProductRepository>()
				.AddSingleton<IUniversityContext, MVCUniversityContext>()
				.AddMemoryCache()
				.AddScoped<UniqueCourseAttribute>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}