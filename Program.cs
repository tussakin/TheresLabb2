using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheresLabb2.Controllers;
using TheresLabb2.Data;

namespace TheresLabb2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<TheresLabb2DbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<TheresLabb2DbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<TheresLabb2DbContext>();
                var controller = new StudentsController(dbContext);
                var controller1 = new CoursesController(dbContext);
                var controller2 = new TeachersController(dbContext);

                dbContext.Database.Migrate();
                dbContext.Seed();
                controller.AddCoursesStudent();
                controller1.AddTeachersToCourses();
                controller2.TeachersForProgramming1();
                controller.StudentsForProgramming1();


            }
           
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
